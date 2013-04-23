using System.Collections.Generic;
using Sunplus.Data.DataAbstract.Entity;
using System;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Customer.PT.ViewPT;
using Sunplus.Accounting.Entities.Customer;
using System.Data;
using DNAide.Data;


namespace Sunplus.Accounting.DataAccess
{
    public interface IBetListDAO
    {
        List<BetListDetail> GetLatestBetList(int totalRow);

        BABetList GetBABetList(int pagesize, int pageindex, string fromdate, string todate, string username, List<IPVNBean> IPVNList, int currency, int bettype, int fromStake, int toStake, int excludeTest);

        List<BetMonitor> GetLatestBABetList(int totalRow, int amount, Int64 lastTransId);

        List<BetListDetail> GetMixParlayTickets(string refNo);

        BABetList GetBABetListByTransId(string refno, List<IPVNBean> IPVNList);

        DataTable GetBettype();

        DataTable GetCurrency();

        CombinationDetailMP GetCombinationDetailMP(string refNo);

        TicketsListingBetlistDTO GetTicketsListingBetlist(bool fromCache, bool isMain, int Type, int matchID,
                                                          string Licensees, string bettypes, string bettypesLive,
                                                          int groupBy, int Language, string LiveType, string HomeScore,
                                                          string AwayScore, string FromTransDate, string ToTransDate, int SAFilter);

        DataSet GetBetListSystemParlayByTransId(string transId, int custId, DateTime moveBetDay, DateTime balanceUpDay);
        //SystemParlayDetail GetBetListSystemParlayByTransId(string transId, int custId);
    }
}
