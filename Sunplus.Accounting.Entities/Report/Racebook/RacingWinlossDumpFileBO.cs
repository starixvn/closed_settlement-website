using System;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.Racebook
{
    [Serializable]
    [DataContract]
    public class RacingWinlossDumpFileBO
    {
        [DataMember]
        public string currency { get; set; }

        [DataMember]
        public string ibcTO { get; set; }
        
        [DataMember]
        public string ibcDividendWon { get; set; }

        [DataMember]
        public string partnerTO { get; set; }

        [DataMember]
        public string partnerDividendWon { get; set; }

        [DataMember]
        public bool IsMatchTO { get; set; }

        [DataMember]
        public bool IsMatchDividend { get; set; }
    }
}
