using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Poker
{
    [Serializable]
    [DataContract]
    public class PokerRow
    {
        [DataMember]
        public string Date { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string CurrencyName { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string GrossComm { get; set; }

        [DataMember]
        public string MWinLoss { get; set; }

        [DataMember]
        public string MComm { get; set; }

        [DataMember]
        public string MTotal { get; set; }

        [DataMember]
        public string AgentComm { get; set; }

        [DataMember]
        public string MasterComm { get; set; }

        [DataMember]
        public string SuperComm { get; set; }

        [DataMember]
        public string CompanyComm { get; set; }

        [DataMember]
        public string GrossCommBase { get; set; }

        [DataMember]
        public string CompanyCommBase { get; set; }
    }

    
    [Serializable]
    [DataContract]
    public class PokerMainBO
    {
        [DataMember]
        public List<PokerRow> Items { get; set; }

        [DataMember]
        public List<PokerRow> Total { get; set; }

        [DataMember]
        public PokerRow GrandTotal { get; set; }

        [DataMember]
        public int TotalRows { get; set; }

    }
}
