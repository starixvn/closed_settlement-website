using System;
using System.Collections.Generic;
using System.Data;
using Sunplus.Data.DataAbstract.Entity;
using Sunplus.Accounting.Entities;


namespace Sunplus.Accounting.DataAccess.NoSql
{
    public class BetListDAO : IBetListDAO
    {

        #region IBetListDAO Members

        public List<BetListDetail> GetLatestBetList(int totalRow)
        {
            throw new NotImplementedException();
        }

        public BABetList GetBABetList(int pagesize, int pageindex, string fromdate, string todate, string username, List<IPVNBean> IPVNList, int currency, int bettype, int fromStake, int toStake, int excludeTest)
        {
            throw new NotImplementedException();
        }

        public List<BetMonitor> GetLatestBABetList(int totalRow, int amount, long lastTransId)
        {
            throw new NotImplementedException();
        }

        public List<BetListDetail> GetMixParlayTickets(string refNo)
        {
            throw new NotImplementedException();
        }

        public BABetList GetBABetListByTransId(string refno, List<IPVNBean> IPVNList)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetBettype()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetCurrency()
        {
            throw new NotImplementedException();
        }

        public CombinationDetailMP GetCombinationDetailMP(string refNo)
        {
            throw new NotImplementedException();
        }

        public TicketsListingBetlistDTO GetTicketsListingBetlist(bool fromCache, bool isMain, int Type, int matchID, string Licensees, string bettypes, string bettypesLive, int groupBy, int Language, string LiveType, string HomeScore, string AwayScore, string FromTransDate, string ToTransDAte, int SAFilter)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IBetListDAO Members


        public DataSet GetBetListSystemParlayByTransId(string transId, int custId, DateTime movebetday, DateTime balanceUpDay)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
