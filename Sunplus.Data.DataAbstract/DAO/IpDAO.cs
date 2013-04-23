using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Norm.Collections;
using Sunplus.Data.DataAbstract.Entity;

namespace Sunplus.Data.DataAbstract.DAO
{
    public class IpVnDAO: DataAccess, IIpVnDAO
    {
        public List<IpVn> GetVnIpCollection()
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
            var mongoCollecion = DataStorage.GetCollection<IpVn>() as MongoCollection<IpVn>;
            if (mongoCollecion != null)
            {
                var listIps = mongoCollecion.AsQueryable().Select(b => b).ToList();

                return listIps;
            }

            return null;
        }


        public bool InsertIp(IpVn ipDetail)
        {
            
            return DataStorage.Insert(string.Empty, ipDetail);
        }

        public IpVn GetIp(string ipId)
        {
            return DataStorage.Select<IpVn>(ipId);
        }

        public bool UpdateIp(IpVn ipDetail)
        {
            return DataStorage.Update(string.Empty, ipDetail);
        }

        public bool DeleteIp(string ipId)
        {
            return DataStorage.Delete<IpVn>(string.Empty, ipId);

        }
    }
}
