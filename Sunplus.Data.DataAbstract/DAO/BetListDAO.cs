using System.Collections.Generic;
using System.Linq;
using Norm.Collections;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Data.DataAbstract.DAO
{
    public class BetListDAO : DataAccess, IBetListDAO
    {

        public List<Bettran> GetLatestBetList(int totalRow)
        {
            /*var listMatch = (new Sunplus.Accounting.DataAbstract.Storage.NoSQL.MongoDB.Match())
                .GetRows<Sunplus.Accounting.DataAbstract.Data.Match>("Eventstatus", "running");
            return (from match in listMatch
                    from bettran in listBettrans
                    where bettran.Matchid == match.Matchid
                    select new BetListDTO
                               {
                                  Transid = bettrans.Transid , RefNo = Convert.ToString(bettrans.Refno), TransDate = Convert.ToString(bettrans.Transdate), BetTeam = Convert.ToString(bettrans.Betteam), LiveHomeScore = Convert.ToString(match.Livehomescore), LiveAwayScore = Convert.ToString(match.Liveawayscore), BetType = Convert.ToString(bettrans.Bettype), Currency = Convert.ToString(bettrans.Currency), CurrencyName = Convert.ToString(bettrans.CurrencyName), Hdp = Convert.ToString(bettrans.Hdp1), HomeTeam = Convert.ToString(match.Homeid), AwayTeam = Convert.ToString(match.Awayid), SportName = Convert.ToString(match.Sporttype), LeagueName = Convert.ToString(match.Leagueid), Odd = Convert.ToString(bettrans.Odds), OddType = Convert.ToString(bettrans.Oddstype), Stake = Convert.ToString(bettrans.Stake), Winloss = Convert.ToString(bettrans.Winlost), VirtualRate = Convert.ToString(bettrans.Virtualrate), Status = Convert.ToString(bettrans.Status), IP = Convert.ToString(bettrans.Ip), AgentPT = Convert.ToString(bettrans.Apositiontaking), AgentWinLoss = Convert.ToString(bettrans.Awinlost), CommPercent = Convert.ToString(bettrans.Acomm) + "%", Comm = Convert.ToString(bettrans.Comm)
                               }).ToList();
             * 
             * */

            //var listBettran = Database.GetCollection<Bettran>().AsQueryable().Where(b => b.Matchid != 0).OrderByDescending(b => b.Transid).Take(totalRow).ToList();
            var mongoCollecion = DataStorage.GetCollection<Bettran>() as MongoCollection<Bettran>;
            if (mongoCollecion != null)
            {
                var listBettran = mongoCollecion.AsQueryable().Where(b => b.Matchid != 0).OrderByDescending(b => b.Transid).Take(totalRow).ToList();
                return listBettran;
            }

            return new List<Bettran>();
        }

        public List<Bettran> GetLatestBABetList(int totalRow)
        {
            //var listBettran = Database.GetCollection<Bettran>().AsQueryable().Where(b => b.Matchid != 0).OrderByDescending(b => b.Transid).Take(totalRow).ToList();
            var mongoCollecion = DataStorage.GetCollection<Bettran>() as MongoCollection<Bettran>;
            if (mongoCollecion != null)
            {
                var listBettran = mongoCollecion.AsQueryable().Where(b => b.Matchid != 0).OrderByDescending(b => b.Transid).Take(totalRow).ToList();
                return listBettran;
            }

            return new List<Bettran>();
        }

        public bool InsertBettran(Bettran bettran)
        { 
            var match = DataStorage.Select<Match>(bettran.Matchid);
            if (match != null)
            {
                bettran.LeagueId = match.Leagueid;
                bettran.HomeId = match.Homeid;
                bettran.AwayId = match.Awayid;
                bettran.SportId = match.Sporttype;
            }
            return DataStorage.Insert(string.Empty, bettran);
        }

        public Bettran GetBettran(string transId)
        {
            return DataStorage.Select<Bettran>(transId);
        }

        public bool UpdateBettran(Bettran bettran)
        {
            return DataStorage.Update(string.Empty, bettran);
        }

        public bool DeleteBettran(string transId)
        {
            return DataStorage.Delete<Bettran>(string.Empty, transId);
            
        }

        public Match GetMatch(int matchId)
        {
            var match = DataStorage.Select<Match>(matchId);
            
            return match;
        }
    }
}
