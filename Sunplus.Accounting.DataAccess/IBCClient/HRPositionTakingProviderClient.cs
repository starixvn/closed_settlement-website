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

/// <summary>
/// Summary description for PositionTakingProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.IBCClient
{
    public class HRPositionTakingProviderClient : PositionTakingProvider
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
                            new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl, "GetSuperSetting");
                        return DbHelper.Execute(spElementS.Name, param, spElementS.DbName);
                    case Globals.AccountType.Master:
                        param.Add("srecommend", uplineId);
                        var spElementM =
                            new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl, "GetMasterSetting");
                        return DbHelper.Execute(spElementM.Name, param, spElementM.DbName);
                    case Globals.AccountType.Agent:
                        param.Add("mrecommend", uplineId);
                        var spElementA =
                            new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl, "GetAgentSetting");
                        return DbHelper.Execute(spElementA.Name, param, spElementA.DbName);
                    case Globals.AccountType.Member:
                        var spElementMe =
                            new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl, "GetMemberSetting");
                        return DbHelper.Execute(spElementMe.Name, param, spElementMe.DbName);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
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
                            new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl, "GetMasterOnMember");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                throw ex;
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
                            new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl, "GetSuperOnMember");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                throw ex;
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
                        var spElementS = new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl, "UpdateSuperSetting");
                        result = DbHelper.Execute(spElementS.Name, param, spElementS.DbName);
                        break;
                    case Globals.AccountType.Master:
                        var spElementM = new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl, "UpdateMasterSetting");
                        result = DbHelper.Execute(spElementM.Name, param, spElementM.DbName);
                        break;
                    case Globals.AccountType.Agent:
                        var spElementA = new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl, "UpdateAgentSetting");
                        result = DbHelper.Execute(spElementA.Name, param, spElementA.DbName);
                        break;
                    case Globals.AccountType.Member:
                        var spElementMe = new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl, "UpdateMemberSetting");
                        result = DbHelper.Execute(spElementMe.Name, param, spElementMe.DbName);
                        break;
                }

                IbcError error = IbcError.SuccessResult;
                if (param["isdisablesa"] != null)
                {
                    string isdisablesa = param["isdisablesa"].ToString();
                    if (isdisablesa == "1")
                    {
                        result.Outputs["errcode"] =
                            result.Outputs["errcode"].ToString().Replace("-109", "").Replace("-112", "");
                    }
                }
                AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, result.Outputs["errcode"].ToString(),
                                        result.Outputs["errmsg"].ToString());
                return error;

                return error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override IbcError UpdatePTMasterOnMember(Hashtable param)
        {
            try
            {
                DbResult result = null;
                param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                var spElement = new HRSpConfig().GetHRStoreProcedure(
                    Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl,
                    "UpdateMasterOnMember");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName);
                
                IbcError error = IbcError.SuccessResult;
                AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, result.Outputs["errcode"].ToString(),
                                        result.Outputs["errmsg"].ToString());

                return error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override IbcError UpdatePTSuperOnMember(Hashtable param)
        {
            try
            {
                DbResult result = null;
                param.Add("ip", HttpContext.Current.Request.UserHostAddress);
                var spElement = new HRSpConfig().GetHRStoreProcedure(
                    Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl,
                    "UpdateSuperOnMember");
                result = DbHelper.Execute(spElement.Name, param, spElement.DbName);

                IbcError error = IbcError.SuccessResult;
                AlertMessage.ParseError(ref error.ErrorCode, ref error.ErrorMsg, result.Outputs["errcode"].ToString(),
                                        result.Outputs["errmsg"].ToString());

                return error;
            }
            catch (Exception ex)
            {
                throw ex;
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
                        var spElementS =
                            new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl,
                                "GetSuperMin");
                        return DbHelper.Execute(spElementS.Name, param, spElementS.DbName);
                    case Globals.AccountType.Master:
                        var spElementM =
                            new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl,
                                "GetMasterMinMax");
                        return DbHelper.Execute(spElementM.Name, param, spElementM.DbName);
                    case Globals.AccountType.Agent:
                        var spElementA =
                            new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl,
                                "GetAgentMinMax");
                        return DbHelper.Execute(spElementA.Name, param, spElementA.DbName);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
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
                        var spElementS =
                            new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl,
                                "UpdateMinMaxPt");
                        result = DbHelper.Execute(spElementS.Name, param, spElementS.DbName);
                        break;
                    case Globals.AccountType.Master:
                        var spElementM =
                            new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl,
                                "UpdateMinMaxPt");
                        result = DbHelper.Execute(spElementM.Name, param, spElementM.DbName);
                        break;
                    case Globals.AccountType.Agent:
                        var spElementA =
                            new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl,
                                "UpdateMinMaxPt");
                        result = DbHelper.Execute(spElementA.Name, param, spElementA.DbName);
                        break;
                    default:
                        var spElementD =
                            new HRSpConfig().GetHRStoreProcedure(
                                Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl,
                                "UpdateMinMaxPt");
                        result = DbHelper.Execute(spElementD.Name, param, spElementD.DbName);
                        break;
                }

                var error = new ErrorDetail();
                error.ErrorCode = CastAide.AsInt32(result.Outputs["errcode"]);
                error.ErrorMessage = CastAide.AsString(result.Outputs["errmsg"]);

                return error;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private class HRSpConfig: DataAccess
        {
            
            public StoreProcedureElement GetHRStoreProcedure(Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection storeCollection, string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }

        }
    }
}