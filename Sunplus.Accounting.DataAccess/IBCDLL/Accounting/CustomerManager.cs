using System;
using System.Collections;
using System.Data;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Entities;
using System.Collections.Generic;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    /// <summary>
    /// @Author: Phuong.Nguyen
    /// </summary>
    public sealed class CustomerManager:DataAccess
    {
        public static StoreProcedureElement GetCustomerStoreProcedure(string alias)
        {
            var sps = CustomerSession.Instance[CustomerSession.StoreCollection.CustomerControl];
            return sps != null ? sps[alias] : null;
        }
        public static StoreProcedureElement GetHRStoreProcedure(RacingSession.StoreCollection storeCollection, string alias)
        {
            var sps = RacingSession.Instance[storeCollection];
            return sps != null ? sps[alias] : null;
        }

        private CustomerManager()
        {
        }

        /// <summary>
        /// call store procedure Acc_CheckAllowedCurrencyAll to check showed Permission.
        /// </summary>
        /// <param name="custId"></param>
        public static DbResult CheckAllowedCurrencyAll(int custId)
        {
            DbResult dbCheckCurrency = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);

                var sps = HelperSession.Instance[Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.HelperSession.StoreCollection.HelperControl];
                var spElement = sps != null ? sps["CheckAllowedShow"] : null;
                

                dbCheckCurrency = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
            return dbCheckCurrency;
        }

        /// <summary>
        /// call store procedure Acc_CustomerSelDisableHR
        /// </summary>
        /// <param name="custId">customer id</param>
        /// <returns>Disabled HR status, true, false</returns>
        public static bool IsDisabledHR(int custId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                var spElement = GetCustomerStoreProcedure("CustomerSelDisableHR");
                DbResult ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
                DataRow row1 = ds.ResultSet.Tables[0].Rows[0];
                bool parentDisabledHR = Convert.IsDBNull(row1["DisableHorseracing"])
                                            ? false
                                            : Convert.ToBoolean(row1["DisableHorseracing"]);
                return parentDisabledHR;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        public static bool IsDisabledFinancial(int custId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                var spElement = GetCustomerStoreProcedure("CustomerSelDisableFI");
                DbResult ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
                DataRow row1 = ds.ResultSet.Tables[0].Rows[0];
                bool parentDisabledHR = Convert.IsDBNull(row1["DisableFinancial"])
                                            ? false
                                            : Convert.ToBoolean(row1["DisableFinancial"]);
                return parentDisabledHR;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// call store procedure Acc_CustomerSelDisableOutright
        /// </summary>
        /// <param name="custId">customer id</param>
        /// <returns>Disabled Outright status, true, false</returns>
        public static bool IsDisabledOutright(int custId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                var spElement = GetCustomerStoreProcedure("CustomerSelDisableOutright");
                DbResult ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
                DataRow row1 = ds.ResultSet.Tables[0].Rows[0];
                bool parentDisabledOutright = Convert.IsDBNull(row1["DisableOutright"])
                                                  ? false
                                                  : Convert.ToBoolean(row1["DisableOutright"]);
                return parentDisabledOutright;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        public static bool IsSuspended(int custId)
        {
            try
            {
                bool suspended = false;
                var param = new Hashtable();
                param.Add("custid", custId);

                var spElement = GetCustomerStoreProcedure("CustomerSelSuspended");
                DbResult ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
                if (ds.ResultSet.Tables[0].Rows.Count > 0)
                {
                    DataRow row1 = ds.ResultSet.Tables[0].Rows[0];
                    suspended = Convert.IsDBNull(row1["Suspended"])
                                    ? false
                                    : Convert.ToBoolean(row1["Suspended"]);
                }
                return suspended;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }


        public static DbResult GetAllHRSetting(int custid, int roleid)
        {
            #region Using DNAide Db

            DbResult dsHRBetSetting = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);
                switch (roleid)
                {
                    case 1:
                        var spElementMe = GetHRStoreProcedure(RacingSession.StoreCollection.RacingControl, "GetMemberSetting");
                        dsHRBetSetting = DbHelper.Execute(spElementMe.Name, param, spElementMe.DbName);

                        break;
                    case 2:
                        var spElementA = GetHRStoreProcedure(RacingSession.StoreCollection.RacingControl, "GetAgentSetting");
                        dsHRBetSetting = DbHelper.Execute(spElementA.DbName, param, spElementA.DbName);

                        break;
                    case 3:
                        var spElementM = GetHRStoreProcedure(RacingSession.StoreCollection.RacingControl, "GetMasterSetting");
                        dsHRBetSetting = DbHelper.Execute(spElementM.Name, param, spElementM.DbName);

                        break;
                    case 4:
                        var spElementS = GetHRStoreProcedure(RacingSession.StoreCollection.RacingControl, "GetSuperSetting");
                        dsHRBetSetting = DbHelper.Execute(spElementS.DbName, param, spElementS.DbName);

                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            #endregion

            return dsHRBetSetting;
        }

        public static DbResult GetHRBetSetting(int custId)
        {
            #region DNAide Db

            DbResult dsHRBetSetting = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                var spElement = GetHRStoreProcedure(RacingSession.StoreCollection.MeetingComm, "CustomerSelMinMaxBetHR");
                dsHRBetSetting = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch
            {
#if DEBUG
                throw;
#endif
            }

            #endregion

            return dsHRBetSetting;
        }

        public static DbResult GetHRCommission(int custId)
        {
            #region DNAide lib

            DbResult dsHRBetSetting = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                var spElement = GetHRStoreProcedure(RacingSession.StoreCollection.MeetingComm, "CustomerSelCommissionHR");
                dsHRBetSetting = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            #endregion

            return dsHRBetSetting;
        }

        public static Hashtable UpdateCustomerHRCommission(int custId, int userId, int roleId, string language,
                                                           string ip, double _UKComm, double _OtherComm, double groupb,
                                                           double groupc, double groupd)
        {
            var hResult = new Hashtable();
            try
            {
                //string strStoreName = "";

                var hParams = new Hashtable();
                hParams.Add("custid", custId);
                hParams.Add("UserID", userId);
                hParams.Add("Language", language);
                hParams.Add("IP", ip);
                hParams.Add("UKComm", _UKComm);
                hParams.Add("OtherComm", _OtherComm);
                hParams.Add("groupb", groupb);
                hParams.Add("groupc", groupc);
                hParams.Add("groupd", groupd);

                #region DNAide lib

                DbResult ds = null;
                switch (roleId)
                {
                    case 1:
                        ds = DbHelper.ExecuteCmd("Acc_CustomerUpdMember_CommissionHR", hParams, DBType.MAIN, 600);
                        break;
                    case 2:
                        ds = DbHelper.ExecuteCmd("Acc_CustomerUpdAgent_CommissionHR", hParams, DBType.MAIN, 600);
                        break;
                    case 3:
                        ds = DbHelper.ExecuteCmd("Acc_CustomerUpdMaster_CommissionHR", hParams, DBType.MAIN, 600);
                        break;
                    case 4:
                        ds = DbHelper.ExecuteCmd("Acc_CustomerUpdSuper_CommissionHR", hParams, DBType.MAIN, 600);
                        break;
                }

                if (ds != null)
                {
                    hResult.Add("errCode", ds.Outputs["errcode"]);
                    hResult.Add("errMsg", ds.Outputs["errmsg"]);
                }

                #endregion
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return hResult;
        }

        public static Hashtable UpdateCustomerHRBetSetting(int custId, int userId, int roleId, string language,
                                                           string ip, double minbet, double maxbet, double maxpermatch)
        {
            var hResult = new Hashtable();
            try
            {
                var hParams = new Hashtable();
                hParams.Add("custid", custId);
                hParams.Add("UserID", userId);
                hParams.Add("Language", language);
                hParams.Add("IP", ip);
                hParams.Add("MinBet", minbet);
                hParams.Add("MaxBet", maxbet);
                hParams.Add("MaxPerMatch", maxpermatch);

                #region DNAide lib

                DbResult ds = null;
                switch (roleId)
                {
                    case 1:
                        ds = DbHelper.ExecuteCmd("Acc_CustomerUpdMember_MinMaxBetHR", hParams, DBType.MAIN, 600);
                        break;
                    case 2:
                        ds = DbHelper.ExecuteCmd("Acc_CustomerUpdAgent_MinMaxBetHR", hParams, DBType.MAIN, 600);
                        break;
                    case 3:
                        ds = DbHelper.ExecuteCmd("Acc_CustomerUpdMaster_MinMaxBetHR", hParams, DBType.MAIN, 600);
                        break;
                    case 4:
                        ds = DbHelper.ExecuteCmd("Acc_CustomerUpdSuper_MinMaxBetHR", hParams, DBType.MAIN, 600);
                        break;
                }

                if (ds != null)
                {
                    hResult.Add("errCode", ds.Outputs["errcode"]);
                    hResult.Add("errMsg", ds.Outputs["errmsg"]);
                }

                #endregion
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return hResult;
        }


        public static Hashtable UpdateCustomerHRSetting(int custId, int userId, bool DisableHorseracing, string language,
                                                        string ip, double minbet, double maxbet, double maxpermatch,
                                                        double _UKComm, double _OtherComm, bool isUpdDownline)
        {
            var hResult = new Hashtable();
            try
            {
                var hParams = new Hashtable();
                hParams.Add("custid", custId);
                hParams.Add("userid", userId);
                hParams.Add("language", language);
                hParams.Add("ip", ip);

                hParams.Add("disablehorseracing", DisableHorseracing);

                //Bet setting
                hParams.Add("minbet", minbet);
                hParams.Add("maxbet", maxbet);
                hParams.Add("maxpermatch", maxpermatch);

                //Commission
                hParams.Add("ukcomm", _UKComm);
                hParams.Add("othercomm", _OtherComm);
                hParams.Add("disabledownline", isUpdDownline);

                #region DNAide lib

                DbResult ds = null;
                ds = DbHelper.ExecuteCmd("Acc_CustomerUpdAllSettingHR", hParams, DBType.MAIN, 600);
                if (ds != null)
                {
                    hResult.Add("errCode", ds.Outputs["errcode"]);
                    hResult.Add("errMsg", ds.Outputs["errmsg"]);
                }

                #endregion
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return hResult;
        }

        public static int GetParentId(DbResult dsCustInfo)
        {
            int parentId = 0;
            try
            {
                if (dsCustInfo != null)
                {
                    if (dsCustInfo.ResultSet.Tables.Count > 1)
                    {
                        DataRow drAccExt = dsCustInfo.ResultSet.Tables[0].Rows[0];
                        int roleId = Convert.ToInt32(drAccExt["roleid"]);
                        switch (roleId)
                        {
                            case 1: // member
                                parentId = Convert.ToInt32(drAccExt["agentId"]);
                                break;
                            case 2: // agent

                                parentId = Convert.ToInt32(drAccExt["masterid"]);
                                break;
                            case 3: // master

                                //phuong.nguyen, set parentId = superid
                                parentId = Convert.ToInt32(drAccExt["superId"]);
                                break;
                            case 4: // super
                                parentId = 0;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return parentId;
        }

        private static bool IsNullDB(DbResult ds)
        {
            if (ds != null && ds.ResultSet.Tables[0].Rows.Count > 0)
                return false;
            return true;
        }

        public static Sunplus.Accounting.Entities.Customer.CustomerExRate SelectCustomerExRate(string username)
        {
            try
            {
                var sp = CustomerSession.Instance[CustomerSession.StoreCollection.CustomerControl]["Acc_customerSelExRate"];
                var param = new Hashtable();
                param.Add("username", username);
                DbResult dbResult = DbHelper.Execute(sp.Name, param, sp.DbName);
                if (dbResult.ResultSet.Tables.Count > 0 && dbResult.ResultSet.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dbResult.ResultSet.Tables[0].Rows[0];
                    return new Sunplus.Accounting.Entities.Customer.CustomerExRate
                    {
                        custid = DNAide.Core.CastAide.AsInt32(row["custid"]),
                        username = DNAide.Core.CastAide.AsString(row["username"]),
                        currency = DNAide.Core.CastAide.AsString(row["currency"]),
                        actualrate = Math.Round(DNAide.Core.CastAide.AsDouble(row["actualrate"]), 4)
                    };
                }
                return default(Sunplus.Accounting.Entities.Customer.CustomerExRate);
            }
            catch
            {
                return default(Sunplus.Accounting.Entities.Customer.CustomerExRate);
            }
        }

        public static Result CustomerCrossTransferInsert(int fromCustID, int toCustID, double fromAmount, double toAmount, double fromExRate,
            double toExRate, string forexUsername, string remark, int custid)
        {
            try
            {
                Result result = new Result();
                var sp = CustomerSession.Instance[CustomerSession.StoreCollection.CustomerControl]["Acc_customerCrossTransferIns"];
                var param = new Hashtable();
                param.Add("fromcustid", fromCustID);
                param.Add("tocustid", toCustID);
                param.Add("fromamount", fromAmount);
                param.Add("toamount", toAmount);
                param.Add("fromexrate", fromExRate);
                param.Add("toexrate", toExRate);
                param.Add("forexusername", forexUsername);
                param.Add("remark", remark);
                param.Add("userid", custid);
                param.Add("ip",  Sunplus.Accounting.Core.Helper.Utils.GetClientIP());
                DbResult dbResult = DbHelper.Execute(sp.Name, param, sp.DbName);
                result.ErrorCode = DNAide.Core.CastAide.AsInt32(dbResult.Outputs["errcode"]);
                result.ErrorMessage = DNAide.Core.CastAide.AsString(dbResult.Outputs["errmsg"]);
                return result;
            }
            catch(Exception e)
            {
                return new Result { ErrorCode = 1, ErrorMessage = e.Message };
            }
        }

        public static List<Sunplus.Accounting.Entities.Customer.ForexAccount> SelectAllForexAccount()
        {
            try
            {
                var listForexAccount = new List<Sunplus.Accounting.Entities.Customer.ForexAccount>();
                var sp = CustomerSession.Instance[CustomerSession.StoreCollection.CustomerControl]["Acc_CustomerSelForexAccount"];
                var param = new Hashtable();
                DbResult dbResult = DbHelper.Execute(sp.Name, param, sp.DbName);
                if (dbResult.ResultSet.Tables.Count > 0 && dbResult.ResultSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dbResult.ResultSet.Tables[0].Rows)
                    {
                        listForexAccount.Add(new Sunplus.Accounting.Entities.Customer.ForexAccount { 
                            username = DNAide.Core.CastAide.AsString(row["vvalue"]),
                        });
                    }
                }
                return listForexAccount;
            }
            catch
            {
                return new List<Sunplus.Accounting.Entities.Customer.ForexAccount>();
            }
        }
    }
}
