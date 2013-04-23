using DNAide.Data;

namespace Sunplus.Accounting.DataAccess
{
    public interface IReportWinLossDAO
    {
        DbResult SelLeagueWLbySports(string winlostdate);
        DbResult SelLeagueWLByBettype(int sportId, int leagueId, string winlostDate);

        DbResult SeleLeagueWLDetailByBettype(string winlostDate, int sportId, int leagueId, int bettypeId, bool isLive,
                                             bool isHedge);
    }
}
