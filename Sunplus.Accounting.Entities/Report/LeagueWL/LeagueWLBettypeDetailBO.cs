using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.LeagueWL
{
    [Serializable]
    [DataContract]
    public class LeagueWLBettypeDetailRow
    {
        [DataMember]
        public string Code { get; set; }
        [DataMember]
        public string Event { get; set; }
        [DataMember]
        public string WinLoss { get; set; }

        [DataMember]
        public string Position { get; set; }

        [DataMember]
        public string Comm { get; set; }

        [DataMember]
        public string LineTotal { get; set; }

        [DataMember]
        public string Turnover { get; set; }

        [DataMember]
        public string Margin { get; set; }
    }


    [Serializable]
    [DataContract]
    public class LeagueWLBettypeDetailBO
    {
        [DataMember]
        public bool IsNeutral { get; set; }
        [DataMember]
        public List<LeagueWLBettypeDetailRow> Items { get; set; }
        [DataMember]
        public LeagueWLBettypeDetailRow GrandTotal { get; set; }
        [DataMember]
        public string GrandTotal8RM { get; set; }

        [DataMember]
        public string GrandTotalBARM { get; set; }

        [DataMember]
        public string LineTotal { get; set; }
    }
}
