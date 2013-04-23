using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web;
using DNAide.Data;
using IBCUtils;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Core.LoggingLib;
using Encryption = DNAide.Utils.Encryption;

namespace Sunplus.Accounting.DataAccess.IBCDLL
{
    using Fanex.Data;

    public class UserController:DataAccess
    {
        public static StoreProcedureElement GetUserControlStoreProcedure(UserControlSession.StoreCollection storeCollection, string alias)
        {
            var sps = UserControlSession.Instance[storeCollection];
            return sps != null ? sps[alias] : null;
        }
        public static StoreProcedureElement GetCustomerStoreProcedure(string alias)
        {
            var sps = CustomerSession.Instance[CustomerSession.StoreCollection.CustomerControl];
            return sps != null ? sps[alias] : null;
        }

        private static readonly CultureInfo cult = CultureInfo.CurrentCulture;

        /// <summary>
        /// Execure SP On_UserUpd
        /// @PhuongDN
        /// </summary>
        /// <param name="onlineUserId"></param>
        /// <returns>
        /// true: user's still online
        /// false: user's session time out
        /// </returns>
        /// Phuong.Nguyen modified <2008-12-02>
        public static bool OnlineUserUpdate(int onlineUserId)
        {
            bool online = false;
            var param = new Hashtable();
            param.Add("on_userid", onlineUserId);
           

            param.Add("iskickedout", online);
           
            var spElement = new UserController().GetStoreProcedure(UserControlSession.StoreCollection.Login, "UserUpd");

            DbResult rs = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
            online = !Convert.ToBoolean(rs.Outputs["iskickedout"], cult);
            
            return online;
        }

        /// <summary>
        /// call SP OnUserSelByCustomer
        /// @author: Vu Hanh
        /// </summary>
        /// <param name="custId">custId</param>
        /// <returns></returns>
        public static DbResult OnUserSelByCustomer(int custId)
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custId);

                rs = DbHelper.Execute("On_UserSelByCustomer", hash, DBType.ON_USER);
            }
            catch
            {
                throw;
            }
            return rs;
        }

        public static DbResult AuthenticateAgent(string username, string agentpwd, int serverId, string loginIp,
                                                 string site, out int roleId, out int custId)
        {
            roleId = 0;
            custId = 0;
            DbResult rs = null;
            try
            {
                string encodedPass = Encryption.CfsEnCode(agentpwd);
                var param = new Hashtable();
                param.Add("username", username);
                param.Add("userpwd", encodedPass);
                param.Add("loginip", loginIp);
                param.Add("sessionid", 0);
                param.Add("serverid", serverId);
                param.Add("roleid", 0);
                param.Add("custid", 0);
                param.Add("on_userid", 0);
                param.Add("currency", "");
                param.Add("masterusername", "");
                param.Add("subaccid", 0);
                param.Add("site", site);

                rs = DbHelper.ExecuteCmd("LoginDownline", param, DBType.MAIN);
                roleId = Convert.ToInt32(rs.Outputs["roleid"]);
                custId = Convert.ToInt32(rs.Outputs["custid"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rs;
        }

        public static int AuthenticateMember(int custId, string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                username = "";
            }
            int roleId = 0;
            DbResult rs = null;
            var param = new Hashtable();
            param.Add("custid", custId);
            param.Add("username", username);
            param.Add("roleid", 0);
            try
            {
                var spElement = GetUserControlStoreProcedure(UserControlSession.StoreCollection.Login, "CustomerSelBelonging");
                rs = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                roleId = Convert.ToInt32(rs.Outputs["roleid"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return roleId;
        }

        /// <summary>
        /// Get customer list of a user id
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public static DbResult GetCustList(int custId)
        {
            DbResult rs = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                var spElement = GetCustomerStoreProcedure("CustomerSelDownLineList");
                rs = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rs;
        }

        /// <summary>
        /// Validate for accountant log-in
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="loginIp"></param>
        /// <param name="custId"></param>
        /// <param name="PageAccess"></param>
        /// <returns></returns>
        public static IbcError AccLoginAdmin(ref string username, string password, string loginIp, string fromUrl,
                                             out int custId, out int roleId, out string system, string domainName, string browser, string platform, string countryCode)
        {
            custId = roleId = 0;
            system = string.Empty;
            var error = new IbcError(IbcError.SUCCESS, IbcError.SUCCESS_MSG);
            try
            {
                using (ObjectDb objDb = new ObjectDb("Acc_LoginAdmin"))
                {
                    var param = new Hashtable();
                    param.Add("username", username);
                    param.Add("userpwd", Encryption.CfsEnCode(password));
                    param.Add("loginip", loginIp);
                    param.Add("from", fromUrl);
                    param.Add("custid", 0);
                    param.Add("errcode", 0);
                    param.Add("errmsg", 0);
                    param.Add("roleid", AppSession.RoleID);
                    param.Add("logindomain", domainName);
                    param.Add("browsername", browser);
                    param.Add("platform", platform);
                    param.Add("countrycode", countryCode);

                    objDb.AssignParamValues(param);
                    objDb.ExecuteNonQuery();

                    custId = Convert.ToInt32(objDb.GetParameterValue("custid"));
                    roleId = Convert.ToInt32(objDb.GetParameterValue("roleid"));
                    system = objDb.GetParameterValue("system").ToString();
                    int errCode = Convert.ToInt32(objDb.GetParameterValue("errcode"));

                    if (errCode != 0)
                    {
                        string errmsg = objDb.GetParameterValue("errmsg").ToString();
                        error.ErrorMsg = errmsg;
                        error.ErrorCode = errCode;
                    }
                    else
                    {
                        username = objDb.GetParameterValue("username").ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                error.ErrorCode = IbcError.INTERNAL_ERROR;
                error.ErrorMsg = ex.Message;
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        public static IbcError AccLoginBAAdmin(ref string username, string password, string loginIp, string fromUrl,
                                             out int custId, out int roleId, out string system)
        {
            custId = roleId = 0;
            system = "";
            var error = new IbcError(IbcError.SUCCESS, IbcError.SUCCESS_MSG);
            try
            {
                using (ObjectDb objDb = new ObjectDb("Acc_LoginAdmin"))
                {
                    var param = new Hashtable();
                    param.Add("username", username);
                    param.Add("userpwd", password);
                    param.Add("loginip", loginIp);
                    param.Add("from", fromUrl);
                    param.Add("custid", 0);
                    param.Add("errcode", 0);
                    param.Add("errmsg", 0);
                    param.Add("roleid", AppSession.RoleID);

                    objDb.AssignParamValues(param);
                    objDb.ExecuteNonQuery();

                    custId = Convert.ToInt32(objDb.GetParameterValue("custid"));
                    roleId = Convert.ToInt32(objDb.GetParameterValue("roleid"));
                    system = objDb.GetParameterValue("system").ToString();
                    int errCode = Convert.ToInt32(objDb.GetParameterValue("errcode"));

                    if (errCode != 0)
                    {
                        string errmsg = objDb.GetParameterValue("errmsg").ToString();
                        error.ErrorMsg = errmsg;
                        error.ErrorCode = errCode;
                    }
                    else
                    {
                        username = objDb.GetParameterValue("username").ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                error.ErrorCode = IbcError.INTERNAL_ERROR;
                error.ErrorMsg = ex.Message;

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        public static DbResult AccAdminLogin(string username, string password, string loginIp, string fromUrl)
        {
            try
            {
                var p = new Hashtable();
                p.Add("username", username);
                p.Add("userpwd", Encryption.CfsEnCode(password));
                p.Add("loginip", loginIp);
                p.Add("from", fromUrl);
                p.Add("custid", 0);
                p.Add("errcode", 0);
                p.Add("errmsg", string.Empty);
                p.Add("roleid", AppSession.RoleID);

                var spElement = new UserController().GetStoreProcedure(UserControlSession.StoreCollection.Login, "LoginAdmin");

                return DbHelper.ExecuteCmd(spElement.Name, p, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Insert into on_user table for accountant log-in
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="serverId"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static int AccLoginAdminOnuser(int custId, int serverId, string ip)
        {
            int onUserId = 0;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("serverid", serverId);
                param.Add("ip", ip);
                param.Add("on_userid", 0);
                param.Add("subaccid", 3);
                param.Add("netsessionid", HttpContext.Current.Session.SessionID);

                var spElement = new UserController().GetStoreProcedure(UserControlSession.StoreCollection.Login, "Checkin");

                DbResult ds = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                onUserId = Convert.ToInt32(ds.Outputs["on_userid"]);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return onUserId;
        }

        /// <summary>
        /// Accountant log out
        /// </summary>
        /// <param name="onAccId"></param>
        /// <param name="systemId"></param>
        public static void AccLogOut(int onAccId, int systemId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("on_userid", onAccId);
                param.Add("roleid", AppSession.RoleID);
                param.Add("serverid", systemId);
                var spElement = new UserController().GetStoreProcedure(UserControlSession.StoreCollection.Login, "UserUpdLogout");

                DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Accountant log out
        /// </summary>
        /// <param name="onAccId"></param>
        public static void AccLogOut(int onAccId, string username, int systemId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("on_userid", onAccId);
                param.Add("username", username);
                param.Add("serverid", systemId);
                param.Add("roleid", AppSession.RoleID);

                var spElement = new UserController().GetStoreProcedure(UserControlSession.StoreCollection.Login, "UserUpdLogout");

                DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static string GetUsernameById(string id)
        {
            var param = new Hashtable();
            param.Add("inputuser", id);
            var spElement = GetCustomerStoreProcedure("DBA_GetAgent");
            DbResult ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            if (ds.ResultSet.Tables[0].Rows.Count > 0)
            {
                if (!Convert.IsDBNull(ds.ResultSet.Tables[0].Rows[0]["membername"]))
                    return ds.ResultSet.Tables[0].Rows[0]["membername"].ToString();
                if (!Convert.IsDBNull(ds.ResultSet.Tables[0].Rows[0]["agentName"]))
                    return ds.ResultSet.Tables[0].Rows[0]["agentName"].ToString();
                if (!Convert.IsDBNull(ds.ResultSet.Tables[0].Rows[0]["masterName"]))
                    return ds.ResultSet.Tables[0].Rows[0]["masterName"].ToString();
                if (!Convert.IsDBNull(ds.ResultSet.Tables[0].Rows[0]["superName"]))
                    return ds.ResultSet.Tables[0].Rows[0]["superName"].ToString();
            }
            return "";
        }

        /// <summary>
        /// Select list of online of a user
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public static DataRowCollection CustomerSelOnline(int custId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);

                using (var obj = new ObjectDb("CustomerSelOnline"))
                {
                    obj.AssignParamValues(param);
                    var ds = obj.ExecuteDataSet();
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) return ds.Tables[0].Rows;
                    
                }
            }
            catch (Exception ex)
            {
                    Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return null;
        }
    }
}
