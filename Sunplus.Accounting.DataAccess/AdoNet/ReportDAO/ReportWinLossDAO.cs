using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class ReportWinLossDAO: DataAccessReport, IReportWinLossDAO
    {
        public DbResult SelLeagueWLbySports(string winlostdate)
        {
            string subfix = GetStoreMappingSubfix(DNAide.Core.CastAide.AsDateTime(winlostdate));
            var rs = ReportSession.Instance[ReportSession.StoreCollection.WinLoss]["LeagueWL_Sporttype" + subfix];
            Hashtable hashtable = new Hashtable();
            hashtable.Add("winlostdate", winlostdate);
            hashtable.Add("winlost_8rm",0);
            hashtable.Add("winlost_barm", 0);


            return DbHelper.Execute(rs.Name, hashtable, rs.DbName, 600);
        }


        public DbResult SelLeagueWLByBettype(int sportId, int leagueId, string winlostDate)
        {
            var subfix = GetStoreMappingSubfix(DNAide.Core.CastAide.AsDateTime(winlostDate));
            var rs = ReportSession.Instance[ReportSession.StoreCollection.WinLoss]["LeagueWL_Bettype" + subfix];
            var hashtable = new Hashtable();
            hashtable.Add("winlostdate", winlostDate);
            hashtable.Add("sporttype", sportId);
            hashtable.Add("leagueid", leagueId);
            hashtable.Add("winlost_8rm", 0);
            hashtable.Add("winlost_barm", 0);

            return DbHelper.Execute(rs.Name, hashtable, rs.DbName, 600);
        }


        public DbResult SeleLeagueWLDetailByBettype(string winlostDate, int sportId, int leagueId, int bettypeId, bool isLive, bool isHedge)
        {
            var subfix = GetStoreMappingSubfix(DNAide.Core.CastAide.AsDateTime(winlostDate));
            var rs = ReportSession.Instance[ReportSession.StoreCollection.WinLoss]["LeagueWL_BettypeDetail" + subfix];
            var hashtable = new Hashtable();
            hashtable.Add("winlostdate", winlostDate);
            hashtable.Add("leagueid", leagueId);
            hashtable.Add("sporttype", sportId);
            hashtable.Add("bettype", bettypeId);
            hashtable.Add("liveindicator", isLive);
            hashtable.Add("ishedge", isHedge);
            hashtable.Add("winlost_8rm", 0);
            hashtable.Add("winlost_barm", 0);

            return DbHelper.Execute(rs.Name, hashtable, rs.DbName, 600);
        }

       
    }
}
