using Sunplus.Accounting.DataAccess.PositionTaking;
using DNAide.Data;
using System.Collections;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess.AdoNet.PositionTaking
{
    public class PokerDAO : DataAccess, IPokerDAO
    {
        public DbResult GetCustomerStatus(int custId)
        {
            DbResult dbResult = null;
            string spMaping = "SelectDisableP2P";
            var param = new Hashtable { { "custid", custId } };
          
            if (!string.IsNullOrEmpty(spMaping))
            {
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.P2PSession.StoreCollection.P2PControl, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
                return dbResult;
            }
            return null;
        }

        public DbResult GetCustomerSetting(int custId, int roleId)
        {
            DbResult dbResult = null;
            string spMaping = null;
            var param = new Hashtable { { "custid", custId } };
            switch (roleId)
            {
                case (int)Roles.Super:
                    spMaping = "GetSuperSetting";
                    break;
                case (int)Roles.Master:
                    spMaping = "GetMasterSetting";
                    break;
                case (int)Roles.Agent:
                    spMaping = "GetAgentSetting";
                    break;
                case (int)Roles.Member:
                    spMaping = "GetMemberSetting";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(spMaping))
            {
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.P2PSession.StoreCollection.P2PControl, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
                return dbResult;
            }
            return null;
        }

        public DbResult UpdateP2P(Hashtable Params, int type)
        {

            DbResult dbResult = null;
            string spMaping = null;
            switch (type)
            {
                case 41:
                    spMaping = "UpdateSuperComm";
                    break;
                case 42:
                    spMaping = "UpdateSuperGameSetting";
                    break;
                case 43:
                    spMaping = "UpdateSuperTransfer";
                    break;
                case 31:
                    spMaping = "UpdateMasterComm";
                    break;
                case 32:
                    spMaping = "UpdateMasterGameSetting";
                    break;
                case 33:
                    spMaping = "UpdateMasterTransfer";
                    break;
                case 21:
                    spMaping = "UpdateAgentComm";
                    break;
                case 22:
                    spMaping = "UpdateAgentGameSetting";
                    break;
                case 23:
                    spMaping = "UpdateAgentTransfer";
                    break;
                case 11:
                    spMaping = "UpdateMemberComm";
                    break;
                case 12:
                    spMaping = "UpdateMemberGameSetting";
                    break;
                case 13:
                    spMaping = "UpdateMemberTransfer";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(spMaping))
            {
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.P2PSession.StoreCollection.P2PControl, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, Params, spElement.DbName, 2000);
            }
            return dbResult;
        }

        public DbResult UpdateP2PStaus(Hashtable Params)
        {

            DbResult dbResult = null;
            string spMaping = "UpdateDisableP2P";
         
            if (!string.IsNullOrEmpty(spMaping))
            {
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.P2PSession.StoreCollection.P2PControl, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, Params, spElement.DbName, 2000);
            }
            return dbResult;
        }

        public DbResult GetDefaultSetting()
        {
            DbResult dbResult = null;
            string spMaping = "SelectDefaultSettingP2P";
            var param = new Hashtable();

            if (!string.IsNullOrEmpty(spMaping))
            {
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.P2PSession.StoreCollection.P2PControl, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
                return dbResult;
            }
            return null;
        }

        public DbResult UpdateDefaultSettingP2P(Hashtable Params)
        {

            DbResult dbResult = null;
            string spMaping = "UpdateDefaultSettingP2P";

            if (!string.IsNullOrEmpty(spMaping))
            {
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.P2PSession.StoreCollection.P2PControl, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, Params, spElement.DbName, 2000);
            }
            return dbResult;
        }

        public DbResult SelectEnableCustomer(int custId)
        {

            DbResult dbResult = null;
            Hashtable Params = new Hashtable();
            Params.Add("custid", custId);
            Params.Add("errcode", 0);
            Params.Add("errmsg", "");
            string spMaping = "SelectEnableCustomer";

            if (!string.IsNullOrEmpty(spMaping))
            {
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.P2PSession.StoreCollection.P2PControl, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, Params, spElement.DbName, 2000);
            }

            return dbResult;
        }

        public enum Roles
        {
            Super = 4,
            Master = 3,
            Agent = 2,
            Member = 1
        }
    }
    
}
