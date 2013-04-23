using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunplus.Data.DataAbstract.Entity;
using Sunplus.Data.DataAbstract.Storage.Cache;

namespace Sunplus.Data.DataAbstract.Storage.Cache.MemTable
{
    public class MatchMem
    {
        private Manager _MemClient = null;
        private string _identify = "match_d_{0}";
        public MatchMem() 
        {
            _MemClient = new Manager();
        }

        public Match this[int matchid] 
        {
            get 
            {
                return _MemClient.Get<Match>(string.Format(_identify, matchid.ToString())); 
            }
            set 
            {
                if (value == null)
                {
                    var key = string.Format(_identify, matchid.ToString());
                    _MemClient.Delete<Match>(key, null);
                }
                else
                {
                    _MemClient.Insert<Match>(string.Format(_identify, matchid.ToString()), value);
                }
            }
        }

        public IEnumerator<Match> Rows
        {
            get 
            {
                return null;
            }
            set
            {
                ;
            }
        }
        ~MatchMem() { _MemClient = null; }
    }
}
