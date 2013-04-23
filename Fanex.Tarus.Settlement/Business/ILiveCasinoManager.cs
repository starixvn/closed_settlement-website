using Fanex.Tarus.Settlement.Entity;
using System.Collections.Generic;

namespace Fanex.Tarus.Settlement.Business
{
    public interface ILiveCasinoManager
    {
        List<League> GetMatches(string fdate, string tdate, string LeagueIds, int statusId);
        string GetMatchesHtml(string fdate, string tdate, string LeagueIds, int statusId);
        Score GetScore(int MatchId);
        string ReCalc(int MatchId, int IBCBetTypeID, string BankerCard, string PlayerCard, int AdminID, string AdminName, string IP, string ShowDate);
        string RefundAtLiveCasioDB(int MatchID, int AdminID, string AdminName, string IP, string ShowDate);
    }
}