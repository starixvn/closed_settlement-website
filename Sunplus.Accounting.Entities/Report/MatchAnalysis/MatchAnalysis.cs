using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.MatchAnalysis
{
    [Serializable]
    [DataContract]
    public class MatchAnalysisDTO
    {
        [DataMember]
        public string ItemName { get; set; }

        [DataMember]
        public string MemberTO { get; set; }

        [DataMember]
        public string MemberWL { get; set; }

        [DataMember]
        public string BetCount { get; set; }

        [DataMember]
        public string HouseTO { get; set; }

        [DataMember]
        public string HouseWL { get; set; }

        [DataMember]
        public string SMATO { get; set; }

        [DataMember]
        public string SMAWL { get; set; }

        [DataMember]
        public string Currency { get; set; }
    }

    [Serializable]
    [DataContract]
    public class MatchAnalysisBO
    {
        [DataMember]
        public List<MatchAnalysisDTO> Items { get; set; }

        [DataMember]
        public MatchAnalysisDTO Total { get; set; }
    }

    public enum ReportType
    {
        Currency,
        Bettype,
        Super,
        Member,
        Country
    }

   
}
