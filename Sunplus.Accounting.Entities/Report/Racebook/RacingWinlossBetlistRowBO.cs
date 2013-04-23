using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Racebook
{
    [Serializable]
    [DataContract]
    public class RacingWinlossBetlistRowBO
    {
        [DataMember]
        public string No {get; set;}

        [DataMember]
        public string BettypeId { get; set; }

        [DataMember]
        public string Member { get; set; }

        [DataMember]
        public string RefNo { get; set; }

        [DataMember]
        public string TransTime { get; set; }

        [DataMember]
        public string HorseBet { get; set; }

        [DataMember]
        public string Bettype { get; set; }

        [DataMember]
        public string Odds { get; set; }

        [DataMember]
        public string winOdds { get; set; }

        [DataMember]
        public string placeOdds { get; set; }

        [DataMember]
        public string Stake { get; set; }

        [DataMember]
        public string ExRate { get; set; }

        [DataMember]
        public string ExRateName { get; set; }

        [DataMember]
        public string MemberWL { get; set; }

        [DataMember]
        public string MemberCom { get; set; }

        [DataMember]
        public string AgencyWL { get; set; }

        [DataMember]
        public string AgencyCom { get; set; }

        [DataMember]
        public string IBCWL { get; set; }

        [DataMember]
        public string IBCCom { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Dividend { get; set; }

        [DataMember]
        public bool IsShowOtExRate { get; set; }
        
        [DataMember]
        public string WinStatus { get; set; }

        [DataMember]
        public string PlaceStatus { get; set; }

        
    }
}
