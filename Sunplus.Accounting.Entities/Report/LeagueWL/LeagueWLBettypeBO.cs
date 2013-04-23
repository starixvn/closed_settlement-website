using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.LeagueWL
{
    
    [Serializable]
    [DataContract]
    public class LeagueWLBettypeRow
    {
        [DataMember]
        public string BettypeId { get; set; }

        [DataMember]
        public string BettypeName { get; set; }

        [DataMember]
        public string IsHedge { get; set; }

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
    public class LeagueWLBettypeBO
    {
        [DataMember]
        public bool IsLive { get; set; }
        [DataMember]
        public List<LeagueWLBettypeRow> Items { get; set; }
        [DataMember]
        public LeagueWLBettypeRow GrandTotal { get; set; }
        [DataMember]
        public string GrandTotal8RM { get; set; }

        [DataMember]
        public string GrandTotalBARM { get; set; }

        [DataMember]
        public string LineTotal { get; set; }
    }

    [Serializable]
    [DataContract]
    public class LeagueWLBettypeDTO
    {
        [DataMember]
        public string SportId { get; set; }
        [DataMember]
        public string LeagueId { get; set; }
        [DataMember]
        public List<LeagueWLBettypeBO> Items { get; set; }
    }
}
