using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Mixparlay
{
    [Serializable]
    [DataContract]
    public class MixparlayWonBO
    {
        [DataMember]
        public string TransID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string Exchange { get; set; }

        [DataMember]
        public string Stake { get; set; }

        [DataMember]
        public string Winloss { get; set; }

        [DataMember]
        public string BaseStake { get; set; }

        [DataMember]
        public string BaseWinloss { get; set; }

        [DataMember]
        public string Selection { get; set; }

        [DataMember]
        public string WinOdds { get; set; }
    }
}
