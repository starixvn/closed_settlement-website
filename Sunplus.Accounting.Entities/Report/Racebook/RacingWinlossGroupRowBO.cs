using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Racebook
{
    [Serializable]
    [DataContract]
    public class RacingWinlossGroupRowBO
    {
       
        [DataMember]
        public string WinlostDate { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string TurnoverAgency { get; set; }

        [DataMember]
        public string TurnoverIBC { get; set; }

        [DataMember]
        public string Turnover3rd { get; set; }

        [DataMember]
        public string TurnoverTotalTO { get; set; }

        [DataMember]
        public string IBCPT { get; set; }

        [DataMember]
        public string IBCNetComm { get; set; }
        
        [DataMember]
        public string IBCTotalIncome { get; set; }
        

        [DataMember]
        public string One2BetPT { get; set; }

        [DataMember]
        public string One2BetComm { get; set; }

        [DataMember]
        public string One2BetTotalIncome { get; set; }

        [DataMember]
        public string GroupTotalIncome { get; set; }

        [DataMember]
        public string PartnerTurnover { get; set; }

        [DataMember]
        public string PartnerDividendWon { get; set; }

        [DataMember]
        public string PartnerCommExpense { get; set; }

        [DataMember]
        public string PartnerFees { get; set; }

        [DataMember]
        public string PartnerAmountDueto { get; set; }
    }
}
