using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Settle
{
    [DataContract]
    public class NumberGameBalls
    {
        [DataMember]
        public int MatchId { get; set; }

        [DataMember]
        public int Seq { get; set; }

        [DataMember]
        public int Number { get; set; }
    }
}
