using System.Collections.Generic;
using System.Data;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Settle;

namespace Sunplus.Accounting.Business.Settle
{
    public interface ISettlement
    {
        string SelStatusConfirming(DataRow dataRow, ref bool isPending);

        bool IsPending(DataRow dataRow);

        Result UdpMatch_Pending(string matchid, bool ispending);

        string ReCalcNumberGame(int matchid, int firstball_value, int secondball_value, int thirdball_value, string ip);

        List<NumberGameBalls> GetNumberGameBalls(int matchid);

        void UpdBallNoByMatchID(int matchid, string ballNo, string siteNo, int adminid, string adminname, string ip);

        string RefundRunningBetDaqTickets(int Matchid, int Creator, string IP);

        string RefundRunningBetFairTickets(int Matchid, int Creator, string IP);

        void AssignDataUnDecided(ref string tpl, string permission, DataRow dr);

        bool IsHasPermissionUnDecided(string permission);

        void SetMatchIsUndecided(int matchId, bool isUndecided, int userId);

        void AssignApplyHedgingMatch(ref string tpl, int matchid);
    }
}
