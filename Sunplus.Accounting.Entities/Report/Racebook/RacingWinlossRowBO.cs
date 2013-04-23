using System;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.Racebook
{
    [Serializable]
    [DataContract]
    public class RacingWinlossRowBO
    {
        
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string CurrencyId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string MeetingID { get; set; }
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string RaceNo { get; set; }

        [DataMember]
        public string AgencyWinTO { get; set; }
        [DataMember]
        public string AgencyPlaceTO { get; set; }
        [DataMember]
        public string AgencyWPTO { get; set; }
        [DataMember]
        public string AgencyTotalTO { get; set; }
        [DataMember]
        public string AgencyCommIncome { get; set; }
        [DataMember]
        public string AgencyPTWL { get; set; }

        [DataMember]
        public string IBCWinTO { get; set; }
        [DataMember]
        public string IBCPlaceTO { get; set; }
        [DataMember]
        public string IBCWPTO { get; set; }
        [DataMember]
        public string IBCTotalTO { get; set; }
        [DataMember]
        public string IBCCommIncome { get; set; }
        [DataMember]
        public string IBCToteFee { get; set; }
        [DataMember]
        public string IBCCommExpense { get; set; }
        [DataMember]
        public string IBCPTWL { get; set; }
        [DataMember]
        public string IBCNetIncome { get; set; }

        [DataMember]
        public string PartnerWinTO { get; set; }
        [DataMember]
        public string PartnerPlaceTO { get; set; }
        [DataMember]
        public string PartnerWPTO { get; set; }
        [DataMember]
        public string PartnerTotalTO { get; set; }
        [DataMember]
        public string PartnerDividendWon { get; set; }
        [DataMember]
        public string PartnerCommExpense { get; set; }
        [DataMember]
        public string PartnerToteFee { get; set; }
        [DataMember]
        public string PartnerAmountDue { get; set; }
       
    }
}
