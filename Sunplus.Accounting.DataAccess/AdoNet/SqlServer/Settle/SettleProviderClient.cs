using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Provider.Settle;

/// <summary>
/// Summary description for SettleProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.Settle
{
    public class SettleProviderClient : SettleProvider
    {
        public override DbResult SelMatchInfo(Hashtable param)
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SettlementSession.StoreCollection.SettlementControl, "Settle_MatchSelByCode");
            //return DbHelper.Execute("Acc_UserLog_Settle_MatchSelByCode", param, DBType.ARCHIVE);
            return DbHelper.Execute(spElement.Name, param, spElement.DbName);
        }

        public override DbResult SelMatchSettleLog(Hashtable param)
        {
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SettlementSession.StoreCollection.SettlementControl, "Settle_MatchSelByID");
            //return DbHelper.Execute("Acc_UserLog_Settle_MatchSelByID", param, DBType.LOG);
            return DbHelper.Execute(spElement.Name, param, spElement.DbName);
        }
    }
}