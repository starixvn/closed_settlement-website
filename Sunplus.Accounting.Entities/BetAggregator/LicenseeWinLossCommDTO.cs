using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.BetAggregator
{
    [Serializable]
    [DataContract]
    public class LicenseeWinLossCommDTO
    {
        [DataMember]
        public List<LicenseeWinLossCommBO> Items { get; set; }
        [DataMember]
        public LicenseeWinLossCommLevel Total { get; set; }
    }

    [Serializable]
    [DataContract]
    public class LicenseeWinLossCommBO
    {
        [DataMember]
        public string ReportDate { get; set; }
        [DataMember]
        public List<LicenseeWinLossCurrency> Items { get; set; }
        [DataMember]
        public int TotalChilds { get; set; }
        [DataMember]
        public LicenseeWinLossCommLevel SubTotal { get; set; }
    }

    [Serializable]
    [DataContract]
    public class LicenseeWinLossCurrency
    {
        [DataMember]
        public string CurrencyId { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public List<LicenseeWinLossCommLevel> Items { get; set; }
    }

    [Serializable]
    [DataContract]
    public class LicenseeWinLossCommLevel
    {
        [DataMember]
        public string Comm { get; set; }
        [DataMember]
        public string Bettype { get; set; }
        [DataMember]
        public string TotalTrans { get; set; }
        [DataMember]
        public string BaseTurnover { get; set; }
        [DataMember]
        public string BaseCustWinLoss { get; set; }
        [DataMember]
        public string BaseLinceseeComm { get; set; }
        [DataMember]
        public string BaseAmountDue { get; set; }
        [DataMember]
        public string Turnover { get; set; }
        [DataMember]
        public string CustWinLoss { get; set; }
        [DataMember]
        public string LicenseeComm { get; set; }
        [DataMember]
        public string AmountDue { get; set; }
    }
}
