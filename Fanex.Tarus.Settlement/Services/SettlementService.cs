using System.Collections.Generic;
using System.ServiceModel.Activation;
using Fanex.Tarus.Settlement.Business;
using Fanex.Tarus.Settlement.Entity;
using System;

namespace Fanex.Tarus.Settlement.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SettlementService : ISettlementService
    {
        public List<Entity.League> GetLeagues(string fdate, string tdate)
        {
            return (new BaseSettlement()).GetLeagues(fdate, tdate);
        }

        public List<League> GetMatches(string fdate, string tdate, string LeagueIds, int statusId)
        {
            return (new LiveCasinoManager()).GetMatches(fdate, tdate, LeagueIds, statusId);
        }

        public string GetMatchesHtml(string fdate, string tdate, string LeagueIds, int statusId)
        {
            return (new LiveCasinoManager()).GetMatchesHtml(fdate, tdate, LeagueIds, statusId);
        }

        public string ReCalc(int MatchId, int IBCBetTypeID, string BankerCard, string PlayerCard, int AdminID, string AdminName, string ShowDate)
        {
            return (new LiveCasinoManager()).ReCalc(MatchId, IBCBetTypeID, BankerCard, PlayerCard, AdminID, AdminName, Sunplus.Accounting.Core.Helper.Utils.GetClientIP(), ShowDate);
        }

        public string Refund(int MatchID, int AdminID, string AdminName, string ShowDate)
        {
            return (new LiveCasinoManager()).RefundAtLiveCasioDB(MatchID, AdminID, AdminName, Sunplus.Accounting.Core.Helper.Utils.GetClientIP(), ShowDate);
        }
    }
}
