using System;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.BetAggregator
{
    [Serializable]
    [DataContract]
    public class Report_LicenseeWLBA_Row
    {
        [DataMember]
        public int index { get; set; }

        [DataMember]
        public string ReportTypeId { get; set; }

        [DataMember]
        public string CustId { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string CurrencyName { get; set; }

        [DataMember]
        public string CurrencyId { get; set; }

        [DataMember]
        public string BetCount { get; set; }


        [DataMember]
        public string Turnover { get; set; }

        [DataMember]
        public string Winlost { get; set; }
        
        [DataMember]
        public string Comm { get; set; }

        [DataMember]
        public string Total { get; set; }

        
        [DataMember]
        public string AWinlost { get; set; }

        [DataMember]
        public string AComm { get; set; }

        [DataMember]
        public string ATotal { get; set; }

        [DataMember]
        public string Virtual { get; set; }

        [DataMember]
        public string Company { get; set; }

    }

    [Serializable]
    [DataContract]
    public class Report_LicenseeWLBA_RowV2
    {
        public int index { get; set; }

        [DataMember]
        public string ReportTypeId { get; set; }

        [DataMember]
        public string CustId { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string CurrencyName { get; set; }

        [DataMember]
        public string CurrencyId { get; set; }

        [DataMember]
        public string BetCount { get; set; }


        [DataMember]
        public string Turnover { get; set; }

        [DataMember]
        public string CustomerWL { get; set; }

        [DataMember]
        public string LicenseeWL { get; set; }

        [DataMember]
        public string BA { get; set; }

        [DataMember]
        public string RB { get; set; }

        [DataMember]
        public string Total { get; set; }

        [DataMember]
        public string AmtDuetoBA { get; set; }

        [DataMember]
        public string AmtDuetoRB { get; set; }

    }

}
