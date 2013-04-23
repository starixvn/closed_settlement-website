using System;
using System.Collections;
using DNAide.Core;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Entities;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Accounting.Core.Extensions;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    using Fanex.Data;

    public sealed class CustomerUpdateStatus:DataAccess
    {
        private StoreProcedureElement GetCustomerStoreProcedure(string aliasName)
        {
            return GetStoreProcedure(Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl, aliasName);
        }

        /// <summary>
        /// Update Open/Closed status for customer
        /// sp: Acc_CustomerUpdClosed
        /// @viet hoang
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="custid"></param>
        /// <param name="isClosed"></param>
        ///  protected StoreProcedureElement GetStoreProcedure(string site)
        public static void CustomerUpdateClosed(int accid, int custid, bool isClosed, string Ip)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("closed", isClosed);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", Ip);// Vinh add Ip for closed member

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdClosed", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
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
        public static void CustomerUpdateAutoMPT(int accid, int custid, bool isEnabled, string Ip)
        {
            try
            {
                var param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("disableautompt", !isEnabled);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", Ip);// Vinh add Ip for closed member

                using (ObjectDb objDB = new ObjectDb("CustomerUpdateMasterAutoPT"))
                {
                    objDB.AssignParamValues(param);
                    objDB.ExecuteNonQuery();
                    
                    if (CastAide.AsInt32(objDB.GetParameterValue("errcode")) > 0)
                    {
                        throw new AccException(CastAide.AsString(objDB.GetParameterValue("errmsg")));
                    }
                }
                
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
        /// Update suspended status for customer
        /// sp: Acc_CustomerUpdClosed
        /// @Vinh
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="custid"></param>
        /// <param name="isClosed"></param>
        public static void CustomerUpdateSuspend(int accid, int custid, bool isSpecial, string Ip)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("suspended", isSpecial);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", Ip);

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdSuspended", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
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
        /// Update DisableOutright  status for customer
        /// sp: Acc_CustomerUpdDisableOutright
        /// @Vinh
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="custid"></param>
        /// <param name="isClosed"></param>
        public static void CustomerUpdateDisableOutright(int accid, int custid, bool disableoutright, string Ip)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("disableoutright", disableoutright);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", Ip);

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdDisableOutright", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
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
        /// update special status for customer
        /// sp: Acc_CustomerUpdImportant
        /// @viet hoang
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="custid"></param>
        /// <param name="isClosed"></param>
        public static void CustomerUpdateSpecial(int accid, int custid, bool isSpecial)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("important", isSpecial);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", System.Web.HttpContext.Current.Request.UserHostAddress);
                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdImportant", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
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
        /// Update dangerous status member for customer
        /// sp: Acc_CustomerUpdDangerCust
        /// @viet hoang
        /// @dungtong changed :use the new integer field danger
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="custid"></param>
        /// <param name="isClosed"></param>
        // public static void CustomerUpdateDangerous(int accid, int custid, bool isDangerous)
        public static bool CustomerUpdateDangerous(int accid, int custid, int dangerLevel, int dangertype)
        {
            bool isBA = false;
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("dangertype", dangertype);
                param.Add("dangerlevel", dangerLevel);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("isba", 0);
                param.Add("ip", System.Web.HttpContext.Current.Request.UserHostAddress);

                var sps = Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CustomerSession.Instance[Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl];
                var spElement = sps != null ? sps["UpdateDangerous"] : null;
                var dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);

                //DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdDangerCust", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
                    throw new AccException(dr.Outputs["errmsg"].ToString());
                isBA = CastAide.AsInt32(dr.Outputs["isba"]) == 0 ? false : true;
                
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return isBA;
        }


        /// <summary>
        /// Update disableHR status for customer
        /// sp: Acc_CustomerUpdDisableHR
        /// @phuong.nguyen 
        /// Create date: <2008-10-22>
        /// </summary>
        ///<param name="accid"></param>
        public static void CustomerUpdateDisabledHR(int accid, int custid, bool disabledHR, string Ip, bool isDisableDownline)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("disablehorseracing", disabledHR);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", Ip);
                param.Add("disabledownline", isDisableDownline);

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdDisableHR", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
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
        /// Update disableOpenTote status for customer
        /// sp: Acc_CustomerUpdDisableOpenTote
        /// @huynguyen
        /// Create date: <2010-04-16>
        /// </summary>
        ///<param name="accid"></param>
        public static void CustomerUpdateDisabledHROpenTote(int accid, int custid, bool disabledHR, string Ip, bool isDisableDownline)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("disableopentote", disabledHR);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", Ip);
                param.Add("disabledownline", isDisableDownline);

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdDisableOpenTote", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
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
        /// Update DisabledBingoGame status for customer
        /// sp: Acc_CustomerUpdDisableOpenTote
        /// @huynguyen
        /// Create date: <2010-04-16>
        /// </summary>
        ///<param name="accid"></param>
        public static void CustomerUpdateDisabledBingoGame(int accid, int custid, bool disabledBingo, string Ip, bool isEnabledDownline)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("disabledbingo", disabledBingo);
                param.Add("syncbg", AppSession.IsSyncBingoCurrentCust);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("loginip", Ip);
                param.Add("enabledownline", isEnabledDownline);

                DbResult dr = DbHelper.ExecuteCmd("Acc_UpdBingoStatus", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
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
        /// Update DisabledNumberGame status for customer
        /// sp: Acc_CustomerUpdDisableOpenTote
        /// @huynguyen
        /// Create date: <2010-04-16>
        /// </summary>
        ///<param name="accid"></param>
        public static void CustomerUpdateDisabledNumberGame(int accid, int custid, bool disabledNumber, string Ip, bool isDisableDownline)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("disablenumber", disabledNumber);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", Ip);
                param.Add("disabledownline", isDisableDownline);

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdDisableNumber", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
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
        /// Update Limit Per IP
        /// @huynguyen
        /// Create date: <2010-10-15>
        /// </summary>
        /// <param name="accid"></param>
        /// <param name="custid"></param>
        /// <param name="limitperid"></param>
        /// <param name="Ip"></param>
        /// <param name="isDisableDownline"></param>
        public static void CustomerUpdateLimitPerIP(int accid, int custid, int limitperid, string Ip, bool isDisableDownline)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("limitperip", limitperid);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", Ip);
                param.Add("removedownlines", isDisableDownline);

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdIPLimit", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
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
        /// Update disableFinancial status for customer
        /// sp: Acc_CustomerUpdDisableFincancial
        /// @phuong.nguyen 
        /// Create date: <2008-10-22>
        /// </summary>
        ///<param name="accid"></param>
        public static void CustomerUpdateDisabledFinancial(int accid, int custid, bool disabledFinancial, string Ip, bool isUpdDownline)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("disablefinancial", disabledFinancial);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", Ip);
                param.Add("disabledownlinefi", isUpdDownline);

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdDisableFI", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
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
        /// Update customer Internal status
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="autoSuspend"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static void CustomerUpdInternal(int custId, bool isInternal, int userId, string ip)
        {
           
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", userId);
                param.Add("custid", custId);
                param.Add("isinternal", isInternal);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", ip);

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdInternal", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
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
        public static void CustomerUpdateSelectedBetting(int accid, int custid, bool selectedBetting, string Ip, bool isUpdDownline)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("selectedbetting", selectedBetting);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                param.Add("ip", Ip);
                param.Add("removedownlines", isUpdDownline);

                DbResult dr = DbHelper.ExecuteCmd("Acc_CustomerUpdSelectedBetting", param, DBType.MAIN);
                if ((int)dr.Outputs["errcode"] > 0)
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

        // update level dangerous for customer
        //@sp: Acc_CustomerUpdDangercust
        // by: faisall.tran
        public static Result UpdateLevelDangerous(int custId, int updateDownline, int dangerType, int dangerLevel, int userId, string ip)
        {
            var result = new Result();
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("updatedownline", updateDownline);
                param.Add("dangertype", dangerType);
                param.Add("dangerlevel", dangerLevel);
                param.Add("userid", userId);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", "Updated Successfully");
                param.Add("ip", ip);

                var sps = Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CustomerSession.Instance[Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl];
                var spElement = sps != null ? sps["UpdateDangerous"] : null;
                var rs = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                result.ErrorCode = CastAide.AsInt32(rs.Outputs["errcode"]);
                result.ErrorMessage = CastAide.AsString(rs.Outputs["errmsg"]);

            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
               
            }
           
            return result;

        }

        public void CustomerUpdateDisabledLiveCasino(int accid, int custid, bool disabledLiveCS, string ip, bool isEnableDownlines)
        {
            try
            {
                Hashtable param = new Hashtable();
                param.Add("userid", accid);
                param.Add("custid", custid);
                param.Add("disabledlivecs", disabledLiveCS);
                param.Add("language", "E");
                param.Add("ip", ip);
                param.Add("enabledownlineslivecs", isEnableDownlines);
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                var spElement = GetCustomerStoreProcedure("UpdateDisableLiveCS");

                var dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName);
                if ((int)dr.Outputs["errcode"] > 0)
                    throw new AccException(dr.Outputs["errmsg"].ToString());
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
        }
    }
}
