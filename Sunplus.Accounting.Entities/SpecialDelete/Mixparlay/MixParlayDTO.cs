using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.SpecialDelete.Mixparlay
{
    [DataContract]
    public class MixParlayBO
    {
        [DataMember]
        public MatchDTO MatchInfo { get; set; }

        [DataMember]
        public List<MixParlayBettype> BetTypes { get; set; }

        [DataMember]
        public int ErrorCode { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }

    [DataContract]
    public class MixParlayBOPager
    {
        [DataMember]
        public MatchDTO MatchInfo { get; set; }

        [DataMember]
        public List<MixParlayDTO> Rows { get; set; }

        [DataMember]
        public int Total { get; set; }

        [DataMember]
        public int ErrorCode { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public int Page { get; set; }

        [DataMember]
        public int Counter { get; set; }
    }

    [DataContract]
    public class MatchDTO
    {
        [DataMember]
        public string MatchCode { get; set; }

        [DataMember]
        public string HomeName { get; set; }

        [DataMember]
        public string AwayName { get; set; }

        [DataMember]
        public string EventDate { get; set; }
    }

    [DataContract]
    public class MixParlayBettype
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string OptionType { get; set; }

        [DataMember]
        public bool IsAllowOptionDelete { get; set; }

        [DataMember]
        public List<MixParlayDTO> Rows { get; set; }
    }

    [DataContract]
    public class MixParlayDTO
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public string Account { get; set; }
        
        [DataMember]
        public string Refno { get; set; }

        [DataMember]
        public string TransId { get; set; }

        [DataMember]
        public string SubTransId { get; set; }

        [DataMember]
        public string Bettype { get; set; }

        [DataMember]
        public string Odds { get; set; }

        [DataMember]
        public string OddType { get; set; }

        [DataMember]
        public string Amount { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string TransTime { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string IsDelete { get; set; }

        [DataMember]
        public string SPT { get; set; } // super PT
        
        [DataMember]
        public string MPT { get; set; } // master PT
        
        [DataMember]
        public string APT { get; set; } // agent PT
        
        [DataMember]
        public string Choice { get; set; }

        [DataMember]
        public string WinLoss { get; set; }

        [DataMember]
        public bool Liveindicator { get; set; }

        [DataMember]
        public string GroupName { get; set; }
    }
}
