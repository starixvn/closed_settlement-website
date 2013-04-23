using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunplus.Data.DataAbstract.Entity;
using Sunplus.Data.DataAbstract.Storage.Cache.MemTable;
using DNAide.Core;

namespace Sunplus.Data.DataAbstract.Business
{
    public class MatchManager
    {
        public Match GetMatchDetail(string matchId)
        {
            Match match = null;
            var matchMem = new MatchMem();
            match = matchMem[CastAide.AsInt32(matchId)];
            return match;
        }

        public bool Insert(Match data)
        {
            if (data != null)
            {
                var matchid = CastAide.AsInt32(data.Matchid);
                (new MatchMem())[matchid] = data;
                return true;
            }
            return false;
        }

        public bool Update(Match data)
        {
            if (data != null)
            {
                var matchid = CastAide.AsInt32(data.Matchid);
                (new MatchMem())[matchid] = data;
                return true;
            }
            return false;
        }

        public bool Delete(object data)
        {
            if (data != null)
            {
                var matchid = CastAide.AsInt32(data);
                (new MatchMem())[matchid] = null;
                return true;
            }
            return false;
        }
    }
}
