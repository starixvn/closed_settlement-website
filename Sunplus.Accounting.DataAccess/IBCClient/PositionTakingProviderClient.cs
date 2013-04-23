using System;
using System.Collections;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using IBCUtils;

using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for PositionTakingProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.IBCClient
{
    public class PositionTakingProviderClient : PositionTakingProvider
    {
        public override DbResult GetPT(int custId, Globals.AccountType accountType, int uplineId)
        {
            //return base.GetPT(custId);
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                switch (accountType)
                {
                    case Globals.AccountType.Super:
                        var spElementS =
                            new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFIEditSuper");
                        return DbHelper.Execute(spElementS.Name, param, spElementS.DbName);
                    case Globals.AccountType.Master:
                        param.Add("srecommend", uplineId);
                        var spElementM =
                            new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFIEditMaster");
                        return DbHelper.Execute(spElementM.Name, param, spElementM.DbName);
                    case Globals.AccountType.Agent:
                        param.Add("mrecommend", uplineId);
                        var spElementA =
                            new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFIEditAgent");
                        return DbHelper.Execute(spElementA.Name, param, spElementA.DbName);
                    case Globals.AccountType.Member:
                        var spElementMe =
                            new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFIEditMember");
                        return DbHelper.Execute(spElementMe.Name, param, spElementMe.DbName);
                }
                return null;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        public override DbResult GetPTMasterEditMember(int custId, int uplineId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("agentid", uplineId);

                var spElement =
                            new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFIMasterEditMember");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        public override DbResult GetPTSuperEditMember(int custId, int uplineId)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("agentid", uplineId);
                var spElement =
                            new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFISuperEditMember");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        /// <summary>
        /// Update PT
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="accountType"></param>
        /// <param name="uplineId"></param>
        /// <returns></returns>
        public override IbcError UpdatePT(Globals.AccountType accountType, Hashtable param)
        {
            //return base.GetPT(custId);
            try
            {
                DbResult result = null;
                param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                switch (accountType)
                {
                    case Globals.AccountType.Super:
                        result = DbHelper.Execute("Acc_CustomerUpdSuperFI_All", param, DBType.MAIN);
                        break;
                    case Globals.AccountType.Master:
                        result = DbHelper.Execute("Acc_CustomerUpdMasterFI_All", param, DBType.MAIN);
                        break;
                    case Globals.AccountType.Agent:
                        result = DbHelper.Execute("Acc_CustomerUpdAgentFI_All", param, DBType.MAIN);
                        break;
                    case Globals.AccountType.Member:
                        result = DbHelper.Execute("Acc_CustomerUpdMemberFI_All", param, DBType.MAIN);
                        break;
                }

                //ErrorDetail error = new ErrorDetail();
                IbcError error = IbcError.SuccessResult;
                AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, result.Outputs["errcode"].ToString(),
                                        result.Outputs["errmsg"].ToString());
                //error.ErrorCode = CastAide.AsInt32(result.Outputs["errcode"]);
                //error.ErrorMessage = CastAide.AsString(result.Outputs["errmsg"]);

                return error;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        public override IbcError UpdatePTMasterOnMember(Hashtable param)
        {
            try
            {
                DbResult result = null;
                param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                result = DbHelper.Execute("Acc_CustomerUpdMemberFI_MAg", param, DBType.MAIN);

                IbcError error = IbcError.SuccessResult;
                AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, result.Outputs["errcode"].ToString(),
                                        result.Outputs["errmsg"].ToString());

                return error;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        public override IbcError UpdatePTSuperOnMember(Hashtable param)
        {
            try
            {
                DbResult result = null;
                param.Add("ip", HttpContext.Current.Request.UserHostAddress);

                result = DbHelper.Execute("Acc_CustomerUpdMemberFI_SAg", param, DBType.MAIN);
                IbcError error = IbcError.SuccessResult;
                AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, result.Outputs["errcode"].ToString(),
                                        result.Outputs["errmsg"].ToString());

                return error;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        public override DbResult GetMinMaxPT(int custId, Globals.AccountType accountType)
        {
            //return base.GetPT(custId);
            //return base.GetPT(custId);
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);

                switch (accountType)
                {
                    case Globals.AccountType.Super:
                        var spElementS = new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFISuperGetMinMaxPt");
                        return DbHelper.Execute(spElementS.Name, param, spElementS.DbName);
                    case Globals.AccountType.Master:
                        var spElementM = new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFIMasterGetMinMaxPt");
                        return DbHelper.Execute(spElementM.Name, param, spElementM.DbName);
                    case Globals.AccountType.Agent:
                        var spElementA = new SpConfig().GetCustomerStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl,
                                "CustomerSelFIAgentGetMinMaxPt");
                        return DbHelper.Execute(spElementA.Name, param, spElementA.DbName);
                }
                return null;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        public override ErrorDetail UpdateMinMaxPT(Globals.AccountType accountType, Hashtable param)
        {
            //return base.GetPT(custId);
            try
            {
                DbResult result = null;
                param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                switch (accountType)
                {
                    case Globals.AccountType.Super:
                        result = DbHelper.Execute("Acc_CustOverrideUpdFI", param, DBType.MAIN);
                        break;
                    case Globals.AccountType.Master:
                        result = DbHelper.Execute("Acc_CustOverrideUpdFI", param, DBType.MAIN);
                        break;
                    case Globals.AccountType.Agent:
                        result = DbHelper.Execute("Acc_CustOverrideUpdFI", param, DBType.MAIN);
                        break;
                    default:
                        result = DbHelper.Execute("Acc_CustOverrideUpdFI", param, DBType.MAIN);
                        break;
                }

                var error = new ErrorDetail();
                error.ErrorCode = CastAide.AsInt32(result.Outputs["errcode"]);
                error.ErrorMessage = CastAide.AsString(result.Outputs["errmsg"]);

                return error;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
        }

        private class SpConfig : DataAccess
        {

            public StoreProcedureElement GetCustomerStoreProcedure(Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection storeCollection, string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }
        }
    }
}