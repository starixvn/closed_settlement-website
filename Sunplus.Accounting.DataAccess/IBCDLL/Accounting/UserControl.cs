using System;
using System.Collections;
using System.Data;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using IBCUtils;

using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Accounting.Core.Configurations;
using Fanex.Data;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public sealed class UserControl : DataAccess
    {
        /// <summary>
        /// Select Account Admin
        /// SP: Acc_AccAdminSel
        /// @viet hoang
        /// </summary>
        /// <returns></returns>
        public static IDataReader AccountAdminSelect()
        {
            try
            {
                Hashtable param = null;
                //return DbHelper.ExecuteReader("Acc_AccAdminSel", param, DBType.MAIN);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "AdminSel");
                return DbHelper.ExecuteReader(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Account Admin password update
        /// SP: Acc_CustomerUpdPwdReset
        /// @viet hoang
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="accid"></param>
        /// <param name="userpwd"></param>
        /// <returns></returns>
        /// Phuong.Nguyen modified
        public static void AccountAdminPasswordUpdate(int custid, int accid, string userpwd)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("userid", accid);
                param.Add("userpwd", userpwd);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("loginip", HttpContext.Current.Request.UserHostAddress);

                DbResult dr = null;
                using (ObjectDb db = new ObjectDb("UpdPwdReset"))
                {
                    db.AssignParamValues(param);
                    var ds = db.ExecuteDataSet();
                    var output = db.GetOuputParams();
                    dr = new DbResult(output, ds);

                }

                if ((int) dr.Outputs["errcode"] > 0)
                    throw new AccException(dr.Outputs["errmsg"].ToString());
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }


        /// <summary>
        /// Account admin status update
        /// SP: Acc_Con_AdminUpdClosed
        /// @viet hoang
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="accid"></param>
        /// <param name="closed"></param>
        /// Phuong.Nguyen modified
        public static IbcError AccountAdminStatusUpdate(int custid, int accid, bool closed)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("userid", accid);
                param.Add("closed", closed);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                //DbResult dr = DbHelper.ExecuteCmd("Acc_Con_AdminUpdClosed", param, DBType.MAIN);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "AdminUpdClosed");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                if (Convert.ToInt32(dr.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(dr.Outputs["errcode"]);
                    error.ErrorMsg = dr.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Account admin page access update
        /// SP: Acc_AccAdminUpdPageAccess
        /// @viet hoang
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="accid"></param>
        /// <param name="role"></param>
        /// <param name="pageAccess"></param>
        /// Phuong.Nguyen modified
        public static void AccountAdminPageAccessUpdate(int custid, int accid, bool role, string pageAccess)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                param.Add("userid", accid);
                param.Add("addnew", role);
                param.Add("updpageaccess", pageAccess);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                //DbResult dr = DbHelper.ExecuteCmd("Acc_AccAdminUpdPageAccess", param, DBType.MAIN);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "AdminUpdPageAccess");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                if ((int) dr.Outputs["errcode"] > 0) throw new AccException(dr.Outputs["errmsg"].ToString());
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }


        /// <summary>
        /// Add new account admin
        /// SP: Acc_AccAdminIns
        /// @viet hoang
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="pageaccess"></param>
        /// Phuong.Nguyen modified
        public static void AccountAdminInsert(int accid, string username, string password, string firstname,
                                              string lastname, string pageaccess)
        {
            try
            {
                var param = new Hashtable();
                param.Add("userid", accid);
                param.Add("username", username);
                param.Add("userpwd", password);
                param.Add("firstname", firstname);
                param.Add("lastname", lastname);
                param.Add("pageaccess", pageaccess);
                param.Add("custid", 0);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                //DbResult dr = DbHelper.ExecuteCmd("Acc_AccAdminIns", param, DBType.MAIN);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "AdminIns");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                if ((int) dr.Outputs["errcode"] > 0)
                    throw new AccException(dr.Outputs["errmsg"].ToString());
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        //Phuong.Nguyen modified
        public static void AccountAdminInsertFromExistedAdmin(int accid, string username)
        {
            try
            {
                var param = new Hashtable();
                param.Add("username", username);
                param.Add("creator", accid);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("language", "E");

                //DbResult dr = DbHelper.ExecuteCmd("Acc_AccAdminInsFromExisted", param, DBType.MAIN);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "AdminInsFromExisted");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                if ((int) dr.Outputs["errcode"] > 0) throw new AccException(dr.Outputs["errmsg"].ToString());
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        /// <summary>
        /// Account Admin Update Information
        /// SP: Acc_AccAdminUpd
        /// @viet hoang
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="custid"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <param name="pageaccess"></param>
        /// <returns></returns>
        /// Phuong.Nguyen modified
        public static void AccountAdminUpdateInformation(int accid, int custid, string firstname, string lastname,
                                                         string pageaccess)
        {
            try
            {
                var param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("firstname", firstname);
                param.Add("lastname", lastname);
                param.Add("pageaccess", pageaccess);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                //DbResult dr = DbHelper.ExecuteCmd("Acc_AccAdminUpd", param, DBType.MAIN);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "AdminUpd");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                if ((int) dr.Outputs["errcode"] > 0)
                    throw new AccException(dr.Outputs["errmsg"].ToString());
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }


        /// <summary>
        /// Select Account Admin By CustID
        /// sp: Acc_AccAdminSelByID
        /// @viet hoang
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        /// Phuong.Nguyen modified
        public static IDataReader AccountAdminSelectById(int custId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);

                //return DbHelper.ExecuteReader("Acc_AccAdminSelByID", param, DBType.MAIN);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "AdminSelByID");
                return DbHelper.ExecuteReader(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        //********* new functions for User Control session ***********
        /// <summary>
        /// Select All Group Info
        /// sp: Acc_Con_GroupInfoSel        
        /// </summary>        
        /// <returns>IDataReader</returns>
        /// Phuong.Nguyen modified <2008-12-02>
        public static IDataReader GroupInfoSelect()
        {
            try
            {
                Hashtable param = null;

                //return DbHelper.ExecuteReader("Acc_Con_GroupInfoSel", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "GroupInfoSel");
                return DbHelper.ExecuteReader(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Select Group by ID
        /// sp: Acc_Con_GroupInfoSel        
        /// </summary>        
        /// <param name="groupId"></param>
        /// <returns>IDataReader</returns>
        /// Phuong.Nguyen modified
        public static IDataReader GroupInfoSelect(int groupId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("groupid", groupId);

                //return DbHelper.ExecuteReader("Acc_Con_GroupInfoSel", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "GroupInfoSel");
                return DbHelper.ExecuteReader(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        //********* new functions for User Control session ***********
        /// <summary>
        /// Select All Function Info
        /// sp: Acc_Con_FuncInfoSel        
        /// </summary>        
        /// <returns>IDataReader</returns>
        /// Phuong.Nguyen modified
        public static DataTable FunctionInfoSelect(Enumerations.AccountingSystem systemType)
        {
            try
            {
                var param = new Hashtable {{"system", systemType.ToString().ToUpper()}};

                //return DbHelper.Execute("Acc_Con_FuncInfoSel", param, DBType.DB03).ResultSet.Tables[0];
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "FuncInfoSel");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName).ResultSet.Tables[0];
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        public static DataTable FunctionInfoSelect()
        {
            try
            {
                //return DbHelper.Execute("Acc_Con_FuncInfoSel", DBType.DB03).ResultSet.Tables[0];
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "FuncInfoSel");
                return DbHelper.Execute(spElement.Name, spElement.DbName).ResultSet.Tables[0];
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return null;
            }
        }

        /// <summary>
        /// Check a username exists in a list of groupname
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataTable CheckUserNameExistInListGroupName(Hashtable param)
        {
            try
            {
                //return DbHelper.Execute("Acc_Con_AdminSelByListGroupNameUserName", param, DBType.DB03).ResultSet.Tables[0];
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "AdminSelByListGroupNameUserName");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName).ResultSet.Tables[0];
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        //Phuong.Nguyen modified
        public static IDataReader FunctionParent(string system)
        {
            var param = new Hashtable();
            param.Add("system", system);
            try
            {
                //return DbHelper.ExecuteReader("Acc_Con_FuncInfoSelParent", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "FuncInfoSelParent");
                return DbHelper.ExecuteReader(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        //Phuong.Nguyen modified
        public static IbcError InsertFunction(string strName, string strDesc, string strFileNames, string strParent,
                                              int intUserId, string system)
        {
            IbcError error = IbcError.SuccessResult;
            var param = new Hashtable();
            param.Add("funcid", 0);
            param.Add("funcname", strName);
            param.Add("funcnote", "");
            param.Add("funcdesc", strDesc);
            param.Add("displayorder", 1);
            if (!string.IsNullOrEmpty(strParent))
                param.Add("parentid", strParent);
            param.Add("filenames", strFileNames);
            param.Add("userid", intUserId);
            param.Add("language", "E");
            param.Add("system", system);
            param.Add("errcode", 0);
            param.Add("errmsg", "");

            //DbResult dr = DbHelper.ExecuteCmd("Acc_Con_FuncInfoIns", param, DBType.DB03);
            var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "FuncInfoIns");
            DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
            if (Convert.ToInt32(dr.Outputs["errcode"]) > 0)
            {
                error.ErrorCode = Convert.ToInt32(dr.Outputs["errcode"]);
                error.ErrorMsg = dr.Outputs["errmsg"] + "";
            }
            return error;
        }

        //Phuong.Nguyen modified
        /// <summary>
        /// Update Function
        /// </summary>
        /// <param name="intFuncId"></param>
        /// <param name="intUserId"></param>
        /// <param name="strName"></param>
        /// <param name="strDesc"></param>
        /// <param name="strFileNames"></param>
        /// <param name="System"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static IbcError UpdateFunction(int intFuncId, int intUserId, string strName, string strDesc,
                                              string strFileNames, string System, string parentId)
        {
            IbcError error = IbcError.SuccessResult;
            var param = new Hashtable();
            param.Add("funcid", intFuncId);
            param.Add("funcname", strName);
            param.Add("funcnote", "");
            param.Add("funcdesc", strDesc);
            param.Add("displayorder", 1);
            param.Add("filenames", strFileNames);
            param.Add("userid", intUserId);
            param.Add("language", "E");
            param.Add("system", System);
            param.Add("errcode", 0);
            param.Add("errmsg", "");
            param.Add("parentid", string.IsNullOrEmpty(parentId) ? "0" : parentId);

            //DbResult dr = DbHelper.ExecuteCmd("Acc_Con_FuncInfoUpd", param, DBType.DB03);
            var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "FuncInfoUpd");
            DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
            if (Convert.ToInt32(dr.Outputs["errcode"]) > 0)
            {
                error.ErrorCode = Convert.ToInt32(dr.Outputs["errcode"]);
                error.ErrorMsg = dr.Outputs["errmsg"] + "";
            }
            return error;
        }

        public static IbcError DeleteFunction(int intFuncId, int intUserId)
        {
            IbcError error = IbcError.SuccessResult;
            var param = new Hashtable();
            param.Add("funcid", intFuncId);
            param.Add("language", "E");
            param.Add("errcode", 0);
            param.Add("errmsg", "");
            param.Add("userid", intUserId);

            //DbResult dr = DbHelper.ExecuteCmd("Acc_Con_FuncInfoDel", param, DBType.DB03);
            var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "FuncInfoDel");
            DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
            if (Convert.ToInt32(dr.Outputs["errcode"]) > 0)
            {
                error.ErrorCode = Convert.ToInt32(dr.Outputs["errcode"]);
                error.ErrorMsg = dr.Outputs["errmsg"] + "";
            }
            return error;
        }

        /// <summary>
        /// Select info for Update Multiple Policy Admin
        /// sp: Acc_Con_Batch_AdminPolicySel         
        /// </summary>        
        /// <param name="custId"></param>
        /// <returns>IDataReader</returns>
        /// Phuong.Nguyen modified
        //public static DataResult SelectMultUpdPolicy(string custId)
        public static DbResult SelectMultUpdPolicy(string custId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custids", custId);
                param.Add("groupid", 0);

                //return DbHelper.Execute("Acc_Con_Batch_AdminPolicySel", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "AdminPolicySel");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Add new Group
        /// SP: Acc_Con_GroupInfoIns        
        /// @vu hanh
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="groupDesc"></param>
        /// <param name="userID"></param>    
        /// Phuong.Nguyen modified <2008-12-02> 
        public static IbcError InsertGroup(string groupName, string groupDesc, int userID)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                var param = new Hashtable();
                param.Add("groupid", 0);
                param.Add("groupname", groupName);
                param.Add("groupdesc", groupDesc);
                param.Add("userid", userID);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                //DbResult dr = DbHelper.ExecuteCmd("Acc_Con_GroupInfoIns", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "GroupInfoIns");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                if (Convert.ToInt32(dr.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(dr.Outputs["errcode"]);
                    error.ErrorMsg = dr.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Update Group info
        /// SP: Acc_Con_GroupInfoUpd        
        /// @vu hanh
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="groupName"></param>
        /// <param name="groupDesc"></param>
        /// <param name="userID"></param>       
        /// Phuong.Nguyen modified 
        public static IbcError UpdateGroup(int groupID, string groupName, string groupDesc, int userID)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                var param = new Hashtable();
                param.Add("groupid", groupID);
                param.Add("groupname", groupName);
                param.Add("groupdesc", groupDesc);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("userid", userID);

                //DbResult dr = DbHelper.ExecuteCmd("Acc_Con_GroupInfoUpd", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "GroupInfoUpd");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                if (Convert.ToInt32(dr.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(dr.Outputs["errcode"]);
                    error.ErrorMsg = dr.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }


        /// <summary>
        /// Delete Group
        /// SP: Acc_Con_GroupInfoDel        
        /// @vu hanh
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="userID"></param>      
        /// Phuong.Nguyen modified <2008-12-02>  
        public static IbcError DeleteGroup(int groupID, int userID)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                var param = new Hashtable();
                param.Add("groupid", groupID);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("userid", userID);

                //DbResult dr = DbHelper.ExecuteCmd("Acc_Con_GroupInfoDel", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "GroupInfoDel");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                if (Convert.ToInt32(dr.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(dr.Outputs["errcode"]);
                    error.ErrorMsg = dr.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Update Group Policy
        /// SP: Acc_Con_GroupPolicyUpd        
        /// @vu hanh
        /// </summary>
        /// <param name="groupPolicies"></param>
        /// <param name="userID"></param>        
        public static IbcError UpdateGroupPolicy(string groupPolicies, int userID)
        {
            return UpdateGroupPolicy(groupPolicies, userID, 0);
        }
        /// <summary>
        /// Update Group Policy v2
        /// SP: Acc_Con_GroupPolicyUpd        
        /// @bobby.nguyen
        /// </summary>
        /// <param name="groupPolicies"></param>
        /// <param name="userID"></param>   
        /// <param name="groupID"></param>   
        public static IbcError UpdateGroupPolicy(string groupPolicies, int userID, int groupID)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                var param = new Hashtable();
                param.Add("grouppolicys", groupPolicies);
                param.Add("userid", userID);
                param.Add("groupid", groupID);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                var spElement = GetStoreInfo(UserControlSession.StoreCollection.Policy, "GroupPolicy");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName, 3000);
                if (Convert.ToInt32(dr.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(dr.Outputs["errcode"]);
                    error.ErrorMsg = dr.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Select Account List
        /// sp: Acc_Con_AdminSelByGroupID        
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        /// Phuong.Nguyen modified <2008-12-02>
        public static IDataReader SelectAdminByGroup(Hashtable param)
        {
            try
            {
                //return DbHelper.ExecuteReader("Acc_Con_AdminSelByGroupID", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "AdminSelByGroupID");
                return DbHelper.ExecuteReader(spElement.Name, param, spElement.DbName);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Select Group Policy
        /// sp: Acc_Con_GroupPolicySel        
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        /// Phuong.Nguyen modified <2008-12-02>
        public static DbResult SelectGroupPolicy(int groupId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("groupid", groupId);

                //return DbHelper.Execute("Acc_Con_GroupPolicySel", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "GroupPolicySel");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Select Admin Policy by cust id
        /// sp: Acc_Con_AdminPolicySelByCustID        
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        /// Phuong.Nguyen modified <2008-12-02>
        public static DbResult SelectAdminPolicy(int custId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("groupid", 0);

                //return DbHelper.Execute("Acc_Con_AdminPolicySelByCustID", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "AdminPolicySelByCustID");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Select Funcname by cust id
        /// sp: Acc_Con_FuncNameSelByCustID        
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public static DbResult Acc_Con_FileNameSelByCustID(int custId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);

                //return DbHelper.Execute("Acc_Con_FileNameSelByCustID", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "FileNameSelByCustID");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }


        /// <summary>
        /// Add new Function
        /// SP: Acc_Con_FuncInfoIns        
        /// @vu hanh
        /// </summary>
        /// <param name="funcName"></param>
        /// <param name="funcDesc"></param>
        /// <param name="funcRender"></param>     
        /// <param name="userID"></param>    
        public static IbcError InsertFunction(string funcName, string funcDesc, int funcRender, int userID)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                var param = new Hashtable();
                param.Add("funcid", 0);
                param.Add("funcname", funcName);
                param.Add("funcdesc", funcDesc);
                param.Add("funcrender", funcRender);
                param.Add("userid", userID);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                //DbResult dr = DbHelper.ExecuteCmd("Acc_Con_FuncInfoIns", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "FuncInfoIns");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                if (Convert.ToInt32(dr.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(dr.Outputs["errcode"]);
                    error.ErrorMsg = dr.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Update Function info
        /// SP: Acc_Con_FuncInfoUpd        
        /// @vu hanh
        /// </summary>
        /// <param name="funcId"></param>
        /// <param name="funcName"></param>
        /// <param name="funcDesc"></param>
        /// <param name="funcRender"></param>     
        /// <param name="userID"></param>    
        public static IbcError UpdateFunction(int funcId, string funcName, string funcDesc, string strFilenames,
                                              int userID)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                var param = new Hashtable();
                param.Add("funcid", funcId);
                param.Add("funcname", funcName);
                param.Add("funcdesc", funcDesc);
                param.Add("filenames", strFilenames);
                param.Add("userid", userID);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                //DbResult dr = DbHelper.ExecuteCmd("Acc_Con_FuncInfoUpd", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "FuncInfoUpd");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                if (Convert.ToInt32(dr.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(dr.Outputs["errcode"]);
                    error.ErrorMsg = dr.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Update Admin Policy
        /// SP: Acc_Con_AdminPolicyUpd        
        /// @vu hanh
        /// </summary>
        /// <param name="memberPolicies"></param>        
        /// <param name="userID"></param>  
        /// Phuong.Nguyen modified  
        public static IbcError UpdateAdminPolicy(string memberPolicies, int userID)
        {
            return UpdateAdminPolicy(memberPolicies, userID, 0);
        }
        /// <summary>
        /// Bobby.nguyen
        /// </summary>
        /// <param name="memberPolicies"></param>
        /// <param name="userID"></param>
        /// <param name="custID"></param>
        /// <returns></returns>
        public static IbcError UpdateAdminPolicy(string memberPolicies, int userID, int custID)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                var param = new Hashtable();
                param.Add("mempolicys", memberPolicies);
                param.Add("userid", userID);
                param.Add("custid", custID);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);

                var spElement = GetStoreInfo(UserControlSession.StoreCollection.Policy, "AdminPolicy");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName, 3000);
                if (Convert.ToInt32(dr.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(dr.Outputs["errcode"]);
                    error.ErrorMsg = dr.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }
        /// <summary>
        /// Insert Member to Group
        /// SP: Acc_Con_AdminIns        
        /// @vu hanh
        /// </summary>
        /// <param name="param"></param>     
        /// Phuong.Nguyen modified             
        public static IbcError InsertAdmin(Hashtable param)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                //DbResult dr = DbHelper.ExecuteCmd("Acc_Con_AdminIns", param, DBType.MAIN);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "Con_AdminIns");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                if (Convert.ToInt32(dr.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(dr.Outputs["errcode"]);
                    error.ErrorMsg = dr.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Update Member Info
        /// SP: Acc_Con_AdminUpd        
        /// @vu hanh
        /// </summary>
        /// <param name="param"></param>                
        public static IbcError UpdateAdmin(Hashtable param)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                //DbResult dr = DbHelper.ExecuteCmd("Acc_Con_AdminUpd", param, DBType.MAIN);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "Con_AdminUpd");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                if (Convert.ToInt32(dr.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(dr.Outputs["errcode"]);
                    error.ErrorMsg = dr.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Add Existed Admin
        /// SP: Acc_Con_AdminInsFromExisted        
        /// @vu hanh
        /// </summary>
        /// <param name="param"></param>                
        /// <param name="groupId"></param>                
        /// <param name="creatorId"></param>     
        /// Phuong.Nguyen modified           
        public static IbcError AddExistedAdmin(Hashtable param)
        {
            IbcError error = IbcError.SuccessResult;
            try
            {
                //DbResult dr = DbHelper.ExecuteCmd("Acc_Con_AdminInsFromExisted", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "Con_AdminInsFromExisted");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                if (Convert.ToInt32(dr.Outputs["errcode"]) > 0)
                {
                    error.ErrorCode = Convert.ToInt32(dr.Outputs["errcode"]);
                    error.ErrorMsg = dr.Outputs["errmsg"] + "";
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Select Admin info by cust id
        /// sp: Acc_Con_AdminSelByCustID        
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public static IDataReader SelectAdminInfo(string custId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);

                //return DbHelper.ExecuteReader("Acc_Con_AdminSelByCustID", param, DBType.DB03);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "Con_AdminSelByCustID");
                return DbHelper.ExecuteReader(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Check host permission 
        /// sp: Acc_Con_CheckPermission        
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public static bool CheckHostPermission(int custID, int funcId)
        {
            bool isHost = false;
            try
            {
                var p = new Hashtable();
                p.Add("custid", custID);
                p.Add("funcid", funcId);
                p.Add("language", "E");
                p.Add("errcode", 0);
                p.Add("errmsg", string.Empty);

                //DbResult dr = DbHelper.Execute("Acc_Con_CheckPermission", p, DBType.DB03, 120);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "Con_CheckPermission");
                DbResult dr = DbHelper.Execute(spElement.Name, p, spElement.DbName, 120);
                if (dr != null)
                {
                    if (CastAide.AsInt32(dr.Outputs["errcode"]) == 0)
                    {
                        isHost = true;
                    }
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return isHost;
        }


        /// <summary>
        /// Check host permissions for Settle page 
        /// @huynguyen
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        


        public static DbResult CheckHostPermission(int custID, string funcname)
        {
            //DataResult dr = null;
            DbResult dr = null;
            try
            {
                var p = new Hashtable();
                p.Add("custid", custID);
                p.Add("funcname", funcname);
                p.Add("language", "E");
                p.Add("errcode", 0);
                p.Add("errmsg", string.Empty);

                //dr = DbHelper.Execute("Acc_Con_CheckPermission", p, DBType.DB03, 120);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "Con_CheckPermission");
                dr = DbHelper.Execute(spElement.Name, p, spElement.DbName, 120);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dr;
        }

        public static DbResult CheckReadPermission(int custID, string funcname)
        {
            //DataResult dr = null;
            DbResult dr = null;
            try
            {
                var p = new Hashtable();
                p.Add("custid", custID);
                p.Add("funcname", funcname);
                p.Add("language", "E");
                p.Add("errcode", 0);
                p.Add("errmsg", string.Empty);

                //dr = DbHelper.Execute("Acc_Con_CheckPermission_All", p, DBType.DB03, 120);
                var spElement = GetStoreInfo(UserControlSession.StoreCollection.AccountInfo, "Con_CheckPermission_All");
                dr = DbHelper.Execute(spElement.Name, p, spElement.DbName, 120);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dr;
        }

        public static StoreProcedureElement GetStoreInfo(UserControlSession.StoreCollection storeCollection, string aliasName) 
        {
            return (new UserControl()).GetStoreProcedure(storeCollection, aliasName);
        }
    }
}