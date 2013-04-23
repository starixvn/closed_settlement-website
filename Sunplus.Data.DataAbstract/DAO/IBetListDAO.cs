using System.Collections.Generic;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Data.DataAbstract.DAO
{
    public interface IBetListDAO
    {
        List<Bettran> GetLatestBetList(int totalRow);

        List<Bettran> GetLatestBABetList(int totalRow);

        bool InsertBettran(Bettran bettran);
        bool UpdateBettran(Bettran bettran);

        bool DeleteBettran(string transId);

        Bettran GetBettran(string transId);

        Match GetMatch(int matchId);
    }
}