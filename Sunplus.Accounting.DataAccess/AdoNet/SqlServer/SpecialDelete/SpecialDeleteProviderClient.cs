using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Provider.SpecialDelete;

/// <summary>
/// Summary description for SpecialDeleteProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.SpecialDelete
{
    public class SpecialDeleteProviderClient : SpecialDeleteProvider
    {
        public override DbResult BettransSelSpecialDel(Hashtable param)
        {
            var spElement =
                GetStoreProcedure(
                    Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Sportbook, "BettransSelSpecialDel");
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
        }

        public override DbResult BettransDelSpecial_OutrightSel(int LeagueID)
        {
            var param = new Hashtable();
            param.Add("leagueid", LeagueID);
            var spElement =
                GetStoreProcedure(
                    Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Sportbook, "Special_OutrightSel");
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 200);
        }

        public override DbResult BettransSelDelSpecial_Outright(Hashtable param)
        {
            var spElement =
                GetStoreProcedure(
                    Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Sportbook, "Special_Outright");
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
        }

        public override DbResult BettransSelBet_BatchMsg(Hashtable param)
        {
            return DbHelper.Execute("Acc_BettransSelBet_BatchMsg", param, DBType.REPORT, 600);
        }
    }
}