using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNAide.Data;
using System.Data;

namespace Fanex.Tarus.Settlement.DataAccess
{
    public interface ILiveCasinoDataAccess
    {
        DataSet GetMatches(string fdate, string tdate, int LeagueId, int statusId);
        DataSet GetLiveCasinoScore(int MatchId);
        DataSet GetMatch(int MatchId);
        string RefundAtLiveCasioDB(int MatchID, int AdminID, string AdminName);
        string RefundAtIBCDB(int MatchID, string Action, int AdminID, string betstatus, decimal CasinoCommRate, string IP, string CardsResult);
        bool UpdateStatus(int MatchID, int Banker, int Player, string ResultData, int bettypeId);
    }
}
