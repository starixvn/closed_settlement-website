using System;
using System.Collections.Generic;
using System.Linq;
using Sunplus.Data.DataAbstract.BootStrappers;
using Sunplus.Data.DataAbstract.DAO;
using Sunplus.Data.DataAbstract.Entity;
using Sunplus.Data.DataAbstract.Log;

namespace Sunplus.Data.DataAbstract.Business
{
    public class BetListManager
    {
        private readonly IBetListDAO _betListDao;
        private readonly Storage.Cache.Manager _betListCache;

        public BetListManager(IBetListDAO betListDao)
        {
            _betListDao = betListDao;
        }

        public BetListManager()
        {
            _betListDao = CommonBootStrapper.Locator.GetInstance<IBetListDAO>();
            _betListCache = new Storage.Cache.Manager();
        }

        public List<BetListDetail> GetLatestBetList(int totalRow)
        {
            var listBettran = _betListDao.GetLatestBetList(totalRow); 

            var betList = listBettran.Select(bettran => new BetListDetail
            {
                Transid = bettran.Transid,
                BettranDetail = bettran
            }).ToList();
            return betList;
        }

        public List<BetListDetail> GetLatestBABetList(int totalRow)
        {
            var listBettran = _betListDao.GetLatestBABetList(totalRow);

            var betList = new List<BetListDetail>();
            foreach (var bettran in listBettran)
            {
                var betListDetail = new BetListDetail {Transid = bettran.Transid, BettranDetail = bettran};
                
                betList.Add(betListDetail);
            }
            
            
            return betList;
        }


        public bool InsertBettran(Bettran bettran)
        {
            try
            {
                var result = _betListDao.InsertBettran(bettran);
                //result = _betListCache.Insert<Bettran>("bettrans_" + bettran.Transid, bettran);
                return result;
            }
            catch (Exception exception)
            {
                Error.WriteLog(exception);
                return false;
            }
        }

        public bool UpdateBettran(Bettran bettran)
        {
            try
            {
                var result = _betListDao.UpdateBettran(bettran);
                //result = _betListCache.Delete<Bettran>("bettrans_" + bettran.Transid, bettran);
                return result;
            }
            catch (Exception exception)
            {
                Error.WriteLog(exception);
                return false;
            }
        }

        public bool DeleteBettran(string transId)
        {
            try
            {
                var result = _betListDao.DeleteBettran(transId);
                //result = _betListCache.Delete<Bettran>("bettrans_" + transId, null);
                return result;
            }
            catch (Exception exception)
            {
                Error.WriteLog(exception);
                return false;
            }
        }

        public BetListDetail GetBetlistDetail(string transId)
        {
            try
            {
                var bettran = GetBettran(transId);
                var betlist = new BetListDetail
                                  {
                                      Transid = transId,
                                      BettranDetail = bettran
                                  };

                return betlist;
            }
            catch (Exception exception)
            {
                Error.WriteLog(exception);
            }
            return null;
        }

        public BetListDetail GetBetlistDetail(Bettran bettran)
        {
            try
            {
                if (bettran == null)
                    return null;

                var match = _betListDao.GetMatch(bettran.Matchid);

                if (match != null)
                {
                    bettran.LeagueId = match.Leagueid;
                    bettran.HomeId = match.Homeid;
                    bettran.AwayId = match.Awayid;
                    bettran.SportId = match.Sporttype;
                }

                var betlist = new BetListDetail
                {
                    Transid = bettran.Transid,
                    BettranDetail = bettran
                };

                Error.WriteLog(new Exception(Sunplus.Data.DataAbstract.Utils.JsonUtils.SerializeObject(match)));

                return betlist;

            }
            catch (Exception exception)
            {
                Error.WriteLog(exception);
            }
            return null;
        }

        public Bettran GetBettran(string transId)
        {
            try
            {
                var result = _betListDao.GetBettran(transId);
                return result;
            }
            catch (Exception exception)
            {
                Error.WriteLog(exception);
            }
            return null;
        }

        
    }
}
