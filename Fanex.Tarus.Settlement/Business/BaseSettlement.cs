using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DNAide.Core;
using Fanex.Tarus.Settlement.DataAccess;
using Fanex.Tarus.Settlement.Entity;
using Sunplus.Accounting.Core.Cache;

namespace Fanex.Tarus.Settlement.Business
{
    using Fanex.Caching;

    public class BaseSettlement
    {
        protected readonly ICache CacheService;
        
        private BaseDataAccess _baseDataAccess;

        public BaseSettlement()
        {
            _baseDataAccess = new BaseDataAccess();
            CacheService = Fanex.Caching.CacheService.CreateInstance(CacheType.MemCache);
        }

        public List<League> GetLeagues(string fdate, string tdate)
        {
            var ds = _baseDataAccess.GetLeagues(fdate, tdate);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count <= 0)
                return new List<League>();
            var listResult = new List<League>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                listResult.Add(new League
                {
                    LeagueId = CastAide.AsInt32(row["leagueid"]),
                    LeagueName = CastAide.AsString(row["leaguename"]),
                    Sporttype = CastAide.AsInt32(row["sporttype"])
                });
            }
            return listResult;
        }

        public virtual Match GetMatch(int MatchId, int bettypeId)
        {
            throw new NotImplementedException();
        }
    }
}
