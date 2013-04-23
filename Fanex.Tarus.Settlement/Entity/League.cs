using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Fanex.Tarus.Settlement.Entity
{
    [Serializable]
    [DataContract]
    public class League
    {
        [DataMember]
        public int LeagueId { get; set; }

        [DataMember]
        public string LeagueName { get; set; }

        [DataMember]
        public int Sporttype { get; set; }

        [DataMember]
        public List<Match> Matches { get; set; }
    }
}
