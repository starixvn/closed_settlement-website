using System;
using System.Collections;
using System.Data;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using CasinoLibrary.SBAgent;
/// <summary>
/// Summary description for IBCLibProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.APIClient
{
    public class IBCLibProvider : IBCCustomerProvider
    {
        public override CSCustomerDetail GetBasicCustInfoIBC(int _IBCCustId)
        {
            var _custSetting = new CSCustomerDetail();
            try
            {
                var param = new Hashtable();
                param.Add("custid", _IBCCustId);
                var spElement =
                    new StoreCustomerProcedureConfiguration().GetStoreProcedureInfo(
                        Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl, "CustomerSelUserInfo");
                DbResult drCustomer = DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);
                DataRow row = null;

                if (drCustomer.ResultSet.Tables[0].Rows.Count > 0)
                {
                    row = drCustomer.ResultSet.Tables[0].Rows[0];
                    _custSetting.UserName = CastAide.AsString(row["username"]);
                    _custSetting.UserName2 = CastAide.AsString(row["username2"]);
                    _custSetting.FirstName = CastAide.AsString(row["firstname"]);
                    _custSetting.LastName = CastAide.AsString(row["lastname"]);
                    _custSetting.Phone = CastAide.AsString(row["phone"]);
                    _custSetting.MobilePhone = CastAide.AsString(row["mobilephone"]);
                    _custSetting.Fax = CastAide.AsString(row["fax"]);
                    _custSetting.Email = CastAide.AsString(row["email"]);
                    _custSetting.RoleId = CastAide.AsInt32(row["roleid"]);
                    _custSetting.Currency = CastAide.AsInt32(row["currency"]);
                    _custSetting.Closed = CastAide.AsBoolean(row["closed"]);
                    _custSetting.Suspend = CastAide.AsBoolean(row["suspend"]);
                    _custSetting.SIBCCustID = CastAide.AsInt32(row["superid"]);
                    _custSetting.MIBCCustID = CastAide.AsInt32(row["masterid"]);
                    _custSetting.AIBCCustID = CastAide.AsInt32(row["agentid"]);

                    //_custSetting.GivenCredit = CastAide.AsInt32(row["credit"]);
                    _custSetting.GivenCredit = CastAide.AsInt64(row["credit"]);

                    _custSetting.CasinoSync = CastAide.AsBoolean(row["synccasino"]);
                    _custSetting.CasinoDisabled = CastAide.AsBoolean(row["disablecasino"]);
                    _custSetting.IBCCustID = CastAide.AsInt32(row["custid"]);
                    _custSetting.UserPwd = CastAide.AsString(row["userpwd"]);
                    _custSetting.CurrencyName = CastAide.AsString(row["currencyname"]);
                    _custSetting.SyncBingo = CastAide.AsBoolean(row["syncbingo"]);
                    _custSetting.LiveCasinoDisabled =  WebSettings.EnableLiveCasino ? CastAide.AsBoolean(row["disablelivecasino"]):true;
                    _custSetting.LiveCasinoSync = WebSettings.EnableLiveCasino ? CastAide.AsBoolean(row["synclivecasino"]) : false;
                    

                    _custSetting.NoIBCData = false;
                }
                Globals.IBCSetting.Casino.UseWebConfigProviderType = true;
                return _custSetting;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _custSetting;
        }

        /// <summary>
        /// Get Customer Information from IBC system
        /// Username, username2, lastname, firstname,...
        /// </summary>
        /// <param name="_IBCCustId"></param>
        /// <returns></returns>
        /// 
        public override CSCustomerDetail GetCustInfoIBC(int _IBCCustId)
        {
            var _custSetting = new CSCustomerDetail();
            try
            {
                var param = new Hashtable();
                param.Add("custid", _IBCCustId);

                var spElement =
                    new StoreCustomerProcedureConfiguration().GetStoreProcedureInfo(
                        Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl, "CustomerSelUserInfo");
                DbResult drCustomer = DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);
                DataRow row = null;

                if (drCustomer.ResultSet.Tables[0].Rows.Count > 0)
                {
                    row = drCustomer.ResultSet.Tables[0].Rows[0];
                    _custSetting.UserName = CastAide.AsString(row["username"]);
                    _custSetting.UserName2 = CastAide.AsString(row["username2"]);
                    _custSetting.FirstName = CastAide.AsString(row["firstname"]);
                    _custSetting.LastName = CastAide.AsString(row["lastname"]);
                    _custSetting.Phone = CastAide.AsString(row["phone"]);
                    _custSetting.MobilePhone = CastAide.AsString(row["mobilephone"]);
                    _custSetting.Fax = CastAide.AsString(row["fax"]);
                    _custSetting.Email = CastAide.AsString(row["email"]);
                    _custSetting.RoleId = CastAide.AsInt32(row["roleid"]);
                    _custSetting.Currency = CastAide.AsInt32(row["currency"]);
                    _custSetting.Closed = CastAide.AsBoolean(row["closed"]);
                    _custSetting.Suspend = CastAide.AsBoolean(row["suspend"]);
                    _custSetting.SIBCCustID = CastAide.AsInt32(row["superid"]);
                    _custSetting.MIBCCustID = CastAide.AsInt32(row["masterid"]);
                    _custSetting.AIBCCustID = CastAide.AsInt32(row["agentid"]);

                    //_custSetting.GivenCredit = CastAide.AsInt32(row["credit"]);
                    _custSetting.GivenCredit = CastAide.AsInt64(row["credit"]);

                    _custSetting.CasinoSync = CastAide.AsBoolean(row["synccasino"]);
                    _custSetting.CasinoDisabled = CastAide.AsBoolean(row["disablecasino"]);
                    _custSetting.IBCCustID = CastAide.AsInt32(row["custid"]);
                    _custSetting.UserPwd = CastAide.AsString(row["userpwd"]);
                    _custSetting.CurrencyName = CastAide.AsString(row["currencyname"]);

                    DataTable dt = null;
                    var _cust = new Customer();
                    if (_custSetting.RoleId != 4)
                    {
                        dt = _cust.GetDefaultMaxTransfer(_custSetting.UpperCustID, 0, false);
                        _custSetting.MaxTransferCreditLimit = CastAide.AsDouble((dt.Rows[0]["MaxLimit"]));
                        _custSetting.MinTransferCreditLimit = CastAide.AsDouble((dt.Rows[0]["MinLimit"]));
                        _custSetting.DefaultTransferCredit = CastAide.AsDouble((dt.Rows[0]["DefMaxTransferCredit"]));
                    }
                    else //Super
                    {
                        dt = _cust.GetDefaultMaxTransfer(0, _custSetting.Currency, true);
                        if (dt != null)
                        {
                            _custSetting.MaxTransferCreditLimit = CastAide.AsDouble((dt.Rows[0]["MaxTransferCredit"]));
                            _custSetting.MinTransferCreditLimit = _custSetting.MaxTransferCreditLimit;
                            _custSetting.DefaultTransferCredit = CastAide.AsDouble((dt.Rows[0]["SuperMaxTransfer"]));
                        }
                    }

                    _custSetting.NoIBCData = false;
                }
                Globals.IBCSetting.Casino.UseWebConfigProviderType = true;
                return _custSetting;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _custSetting;
        }

        /// <summary>
        /// Initialize basics property of an object from posted values of a form. Be used in new customer function.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override CSCustomerDetail GetCustInfoIBC(string custid, Hashtable r)
        {
            var _custSetting = new CSCustomerDetail();


            _custSetting.UserName = CastAide.AsString(r["username"]);
            _custSetting.UserPwd = CastAide.AsString(r["userpwd"]);
            _custSetting.FirstName = CastAide.AsString(r["firstname"]);
            _custSetting.LastName = CastAide.AsString(r["lastname"]);
            _custSetting.Phone = CastAide.AsString(r["phone"]);
            _custSetting.MobilePhone = CastAide.AsString(r["mobilephone"]);
            _custSetting.Fax = CastAide.AsString(r["fax"]);
            _custSetting.Email = CastAide.AsString(r["email"]);
            _custSetting.RoleId = AppSession.RoleID - 1;

            _custSetting.Closed = CastAide.AsBoolean(r["closed"]);
            _custSetting.Suspend = CastAide.AsBoolean(r["suspend"] == null ? r["suspended"] : r["suspend"]);

            _custSetting.SIBCCustID = AppSession.RoleID == 4 ? AppSession.CustID : 0;
            _custSetting.MIBCCustID = AppSession.RoleID == 3 ? AppSession.CustID : 0;
            _custSetting.AIBCCustID = AppSession.RoleID == 2 ? AppSession.CustID : 0;

            _custSetting.CasinoSync = false;
            _custSetting.CasinoDisabled = false;
            _custSetting.IBCCustID = CastAide.AsInt32(custid);


            return _custSetting;
        }

        public override bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS)
        {
            var param = new Hashtable();
            param.Add("custid", custid);
            param.Add("disabledcs", bDisabledCS);
            param.Add("synccs", bSyncCS);
            param.Add("enabledownline", false);

            param.Add("userid", AppSession.AccId);
            param.Add("subaccid", 0);
            param.Add("loginip", HttpContext.Current.Request.UserHostAddress);

            try
            {
                var spElement = new StoreProcedureConfiguration().GetStoreProcedureInfo(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CasinoSession.StoreCollection.CasinoControl, "UpdCSStatus");
                DbResult drReturn = DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);
                bool status = CastAide.AsBoolean(drReturn.Outputs["status"]);
                if (!status) throw new Exception(drReturn.Outputs["errmsg"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var cust = new Customer();
            return cust.UpdateCustEnableCasino(custid, !bDisabledCS, true,
                                               HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                                               AppSession.AccId, AppSession.Username, 3);
        }


        public override bool UpdateCSStatus(int custid, bool bSyncCS, bool bDisabledCS, bool isUpdDownline)
        {
            var param = new Hashtable();
            param.Add("custid", custid);
            param.Add("disabledcs", bDisabledCS);
            param.Add("synccs", bSyncCS);
            param.Add("enabledownline", isUpdDownline);

            param.Add("userid", AppSession.AccId);
            param.Add("subaccid", 0);
            param.Add("loginip", HttpContext.Current.Request.UserHostAddress);
            try
            {
                var spElement = new StoreProcedureConfiguration().GetStoreProcedureInfo(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CasinoSession.StoreCollection.CasinoControl, "UpdCSStatus");
                DbResult drReturn = DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);
                bool status = CastAide.AsBoolean(drReturn.Outputs["status"]);
                if (!status) throw new AccException(drReturn.Outputs["errmsg"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

            var cust = new Customer();
            return cust.UpdateCustEnableCasino(custid, !bDisabledCS, isUpdDownline,
                                               HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"],
                                               AppSession.AccId, AppSession.Username, 3);
        }


        public override bool IsDisabledCasino(int custId)
        {
            bool isDisabledCasino = false;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                var spElement =
                    new StoreCustomerProcedureConfiguration().GetStoreProcedureInfo(
                        Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl, "CustomerSelStatus");
                DbResult drCustomer = DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);
                isDisabledCasino = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["DisableCasino"], true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDisabledCasino;
        }

        public override CSCustomerDetail GetCasinoStatus(int custId)
        {
            var _cust = new CSCustomerDetail();
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                var spElement =
                    new StoreCustomerProcedureConfiguration().GetStoreProcedureInfo(
                        Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl, "CustomerSelStatus");
                var drCustomer = DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);

                if (drCustomer != null && drCustomer.ResultSet.Tables != null && drCustomer.ResultSet.Tables[0].Rows.Count > 0)
                {
                    _cust.CasinoDisabled = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["DisableCasino"], true);
                    _cust.CasinoSync = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["SyncCasino"], false);
                    _cust.LiveCasinoSync = WebSettings.EnableLiveCasino
                                               ? CastAide.AsBoolean(
                                                   drCustomer.ResultSet.Tables[0].Rows[0]["SyncLiveCasino"], false)
                                               : false;
                    _cust.LiveCasinoDisabled =
                        WebSettings.EnableLiveCasino
                        ? CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["DisableLiveCasino"], true): true;
                }
                
                return _cust;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _cust;
        }

        public override CustomerDetail GetCustomerStatus(int custId)
        {
            var _cust = new CustomerDetail();
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                var spElement =
                    new StoreCustomerProcedureConfiguration().GetStoreProcedureInfo(
                        Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl, "CustomerSelStatus");
                DbResult drCustomer = DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);

                _cust.IsClosed = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["Closed"], true);
                _cust.IsAutoSuspend = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["AutoSuspend"], true);
                _cust.IsSuspended = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["Suspend"], true);
                _cust.IsDisabledFI = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["DisableFI"], true);
                // _cust.IsDisabledHR = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["DisableHorseracing"],
                //                                        true);
                //_cust.IsDisabledOpenTote = CastAide.AsBoolean(
                //    drCustomer.ResultSet.Tables[0].Rows[0]["DisableOpenTote"], true);
                _cust.IsDisableCasino = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["DisableCasino"], true);
                _cust.IsCasinoSync = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["SyncCasino"], false);
                _cust.IsBingoSync = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["SyncBingo"], false);
                _cust.IsAllowAutoMasterPT =
                    !CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["DisableAutoMPT"], true);
                _cust.IsDisableOutright = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["DisableOutright"],
                                                             true);
                _cust.IsDisabledBingoGame = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["DisableBingo"], true);
                _cust.IsDisabledNumberGame = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["DisableNumber"], true);
                //if(DataUtils.IsExistKey(drCustomer.ResultSet.Tables[0].Rows[0], "IsInternal"))
                _cust.IsInternalUser = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["IsInternal"], false);

                _cust.IsLiveCasinoSync = WebSettings.EnableLiveCasino
                                             ? CastAide.AsBoolean(
                                                 drCustomer.ResultSet.Tables[0].Rows[0]["SyncLiveCasino"], false)
                                             : false;
                _cust.IsDisableLiveCasino = WebSettings.EnableLiveCasino
                                                ? CastAide.AsBoolean(
                                                    drCustomer.ResultSet.Tables[0].Rows[0]["DisableLiveCasino"], false)
                                                : true;
                return _cust;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _cust;
        }

        public override CustomerDetail GetCustomerDetail(string username)
        {
            var _cust = new CustomerDetail();
            try
            {
                var param = new Hashtable();
                param.Add("username", username);
                var spElement = new StoreCustomerProcedureConfiguration().GetStoreProcedureInfo(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl, "CustomerSelCustomerInfo");

                DbResult drCustomer = DbHelper.Execute(spElement.Name, param, spElement.DbName, 300);

                if (drCustomer.ResultSet.Tables[0].Rows.Count > 0)
                {
                    _cust.CustID = CastAide.AsInt32(drCustomer.ResultSet.Tables[0].Rows[0]["custid"], 0);
                    _cust.IsDisableCasino = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["DisableCasino"],
                                                               true);
                    _cust.IsCasinoSync = CastAide.AsBoolean(drCustomer.ResultSet.Tables[0].Rows[0]["synccasino"], false);
                    _cust.SuperUserName = CastAide.AsString(drCustomer.ResultSet.Tables[0].Rows[0]["supername"], "");
                    _cust.MasterUserName = CastAide.AsString(drCustomer.ResultSet.Tables[0].Rows[0]["mastername"], "");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _cust;
        }

        private class StoreProcedureConfiguration : DataAccess
        {
            public StoreProcedureElement GetStoreProcedureInfo(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CasinoSession.StoreCollection storeCollection, string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }
        }
        private class StoreCustomerProcedureConfiguration : DataAccess
        {
            public StoreProcedureElement GetStoreProcedureInfo(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection storeCollection, string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }
        }
    }

    
}
