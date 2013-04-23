using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.NumberGame
{
    [Serializable]
    [DataContract]
    public class BettypesWinLossDTO
    {
        [DataMember]
        public BettypeWinLossDetail Live { get; set; }
        [DataMember]
        public BettypeWinLossDetail NoneLive { get; set; }
    }

    [Serializable]
    [DataContract]
    public class BettypeWinLossDetailDTO
    {
        [DataMember]
        public List<BettypeWinLossDetail> Items { get; set; }
        [DataMember]
        public BettypesWinLossRow Total { get; set; }
    }

    [Serializable]
    [DataContract]
    public class BettypeWinLossDetail
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<BettypesWinLossRow> BettypesWinLossRows { get; set; }
        [DataMember]
        public BettypesWinLossRow Total { get; set; }
    }

    [Serializable]
    [DataContract]
    public class BettypesWinLossRow
    {
        [DataMember]
        public string BettypeId { get; set; }
        [DataMember]
        public string Bettype { get; set; }
        [DataMember]
        public string WinLoss { get; set; }
        [DataMember]
        public string Position { get; set; }
        [DataMember]
        public string Comm { get; set; }
        [DataMember]
        public string LineTotalReal { get; set; }
        [DataMember]
        public string LineTotalVirtual { get; set; }
        [DataMember]
        public string Turnover { get; set; }
        [DataMember]
        public string Margin { get; set; }
        [DataMember]
        public string Variance { get; set; }
    }
}
