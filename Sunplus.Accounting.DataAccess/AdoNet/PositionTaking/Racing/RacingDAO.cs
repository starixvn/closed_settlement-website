using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.PositionTaking.Racing;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.PositionTaking.Racing;

namespace Sunplus.Accounting.DataAccess.AdoNet.PositionTaking.Racing
{
    public class RacingDAO : DataAccess, IRacingDAO
    {
        public System.Data.DataTable GetRacingStatus(int custId)
        {
            DbResult dbResult = null;
            string spMaping = "GetRacingStatus";
            var param = new Hashtable { { "custid", custId } };
            if (!string.IsNullOrEmpty(spMaping))
            {
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
                if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                    return dbResult.ResultSet.Tables[0];
            }
            return null;
        }
        public System.Data.DataTable GetRacingSetting(int custId, int roleId)
        {
            DbResult dbResult = null;
            string spMaping = null;
            var param = new Hashtable { { "custid", custId } };
            switch (roleId)
            {
                case (int)CustInfo.Roles.Super:
                    spMaping = "GetSuperSetting";
                    break;

                case (int)CustInfo.Roles.Master:
                    spMaping = "GetMasterSetting";
                    break;

                case (int)CustInfo.Roles.Agent:
                    spMaping = "GetAgentSetting";
                    break;

                case (int)CustInfo.Roles.Member:
                    spMaping = "GetMemberSetting";
                    break;
                case (int)CustInfo.Roles.SuperOnMember: //*******Super on Member params*****
                    spMaping = "GetSuperOnMember";
                    break;
                case (int)CustInfo.Roles.MasterOnMember: //*******Master on Member params*****
                    spMaping = "GetMasterOnMember";
                    break;
                case (int)CustInfo.Roles.SuperMin: //*******Super Min params*****
                    spMaping = "GetSuperMin";
                    break;
                case (int)CustInfo.Roles.MasterMinMax: //*******Master Min/Max params*****
                    spMaping = "GetMasterMinMax";
                    break;
                case (int)CustInfo.Roles.AgentMinMax: //*******Agent Min/Max params*****
                    spMaping = "GetAgentMinMax";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(spMaping))
            {
                //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Racing, spMaping);
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
                if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                    return dbResult.ResultSet.Tables[0];
            }
            return null;

        }

        public Result UpdateRacingStatus(Hashtable settingParams)
        {
            var result = new Result();
            string spMaping = "UpdateStatus";
            if (!string.IsNullOrEmpty(spMaping))
            {
                //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Racing, spMaping);
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl, spMaping);
                var ds = DbHelper.Execute(spElement.Name, settingParams, spElement.DbName, 2000);
                result.HandleError(ref result, ds, ResourceExtension.Resources.ErrorMsg.ToString());
            }
            return result;
        }

        public Result UpdateRacingSetting(Hashtable settingParams, int roleId)
        {

            var result = new Result();
            string spMaping = null;
            switch (roleId)
            {
                case (int)CustInfo.Roles.Super:
                    spMaping = "UpdateSuperSetting";
                    break;

                case (int)CustInfo.Roles.Master:
                    spMaping = "UpdateMasterSetting";
                    break;

                case (int)CustInfo.Roles.Agent:
                    spMaping = "UpdateAgentSetting";
                    break;

                case (int)CustInfo.Roles.Member:
                    spMaping = "UpdateMemberSetting";
                    break;
                case (int)CustInfo.Roles.SuperOnMember: //*******Super on Member params*****
                    spMaping = "UpdateSuperOnMember";
                    break;
                case (int)CustInfo.Roles.MasterOnMember: //*******Master on Member params*****
                    spMaping = "UpdateMasterOnMember";
                    break;
                case (int)CustInfo.Roles.SuperMin: //*******Super Min params*****
                case (int)CustInfo.Roles.MasterMinMax: //*******Master Min/Max params*****
                case (int)CustInfo.Roles.AgentMinMax: //*******Agent Min/Max params*****
                    spMaping = "UpdateMinMaxPt";
                    break;

                default:
                    break;
            }
            if (!string.IsNullOrEmpty(spMaping))
            {
                //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Racing, spMaping);
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.RacingSession.StoreCollection.RacingControl, spMaping);
                var ds = DbHelper.Execute(spElement.Name, settingParams, spElement.DbName, 2000);
                result.HandleError(ref result, ds, ResourceExtension.Resources.ErrorMsg.ToString());
            }
            return result;
        }
    }
}
