using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DNAide.Data;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess.Settlement
{
    public interface ISettlementDAO
    {
        DbResult UdpMatch_Pending(string matchid, bool ispending);

        string ReCalcNumberGame(int matchid, int firstball_value, int secondball_value, int thirdball_value, string ip);

        DbResult GetNumberGameBalls(int matchid);

        ResultNG UpdBallNoByMatchID(int matchid, string ballNo, string siteNo, int adminid, string adminname, string ip);

        string RefundRunningBetDaqTickets(int Matchid, int Creator, string IP);

        void SetMatchIsUndecided(int matchId, bool isUndecided, int userId);

        DataTable MatchSelHedge(int matchId);
    }
}
