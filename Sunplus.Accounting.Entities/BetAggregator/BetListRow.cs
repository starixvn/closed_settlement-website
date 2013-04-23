using System;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.BetAggregator
{
    [Serializable]
    [DataContract]
    public class BetListRow
    {
        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public string RefNo { get; set; }

        [DataMember]
        public string TransDate { get; set; }

        [DataMember]
        public string BetTeam { get; set; }

        [DataMember]
        public string LiveHomeScore { get; set; }

        [DataMember]
        public string LiveAwayScore { get; set; }

        [DataMember]
        public string BetType { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string Hdp { get; set; }

        [DataMember]
        public string HomeTeam { get; set; }

        [DataMember]
        public string AwayTeam { get; set; }

        [DataMember]
        public string SportName { get; set; }

        [DataMember]
        public string LeagueName { get; set; }
        
        [DataMember]
        public string Odd { get; set; }

        [DataMember]
        public string OddType { get; set; }

        [DataMember]
        public string Stake { get; set; }

        [DataMember]
        public string Winloss { get; set; }

        [DataMember]
        public string VirtualRate { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string IP { get; set; }

        [DataMember]
        public string AgentPT { get; set; }

        [DataMember]
        public string AgentWinLoss { get; set; }
        
        [DataMember]
        public string CommPercent  { get; set; }

        [DataMember]
        public string Comm { get; set; }

    }

}
