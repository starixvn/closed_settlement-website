using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Fanex.Tarus.Settlement.Entity
{
    [Serializable]
    [DataContract]
    public class Score
    {
        [DataMember]
        public int MatchId { get; set; }

        [DataMember]
        public List<ScoreDetail> ScoreDetails { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ScoreDetail
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}
