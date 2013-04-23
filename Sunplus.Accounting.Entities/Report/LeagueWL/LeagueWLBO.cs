using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.LeagueWL
{
    [Serializable]
    [DataContract]
    public class LeagueWLRow
    {
        [DataMember]
        public string LeagueId { get; set; }

        [DataMember]
        public string LeagueName { get; set; }
        
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
    public class LeagueWLBySportsBO
    {
        [DataMember]
        public string SportId { get; set; }

        [DataMember]
        public string SportName { get; set; }

        [DataMember]
        public List<LeagueWLRow> Items { get; set; }

        [DataMember]
        public LeagueWLRow Total { get; set; }
        
    }

    [Serializable]
    [DataContract]
    public class LeagueWLBySportsDTO
    {
        [DataMember]
        public List<LeagueWLBySportsBO> Items { get; set; }
        [DataMember]
        public LeagueWLRow GrandTotal { get; set; }
        [DataMember]
        public string GrandTotal8RM { get; set; }

        [DataMember]
        public string GrandTotalBARM { get; set; }

        [DataMember]
        public string LineTotal { get; set; }
    }
}
