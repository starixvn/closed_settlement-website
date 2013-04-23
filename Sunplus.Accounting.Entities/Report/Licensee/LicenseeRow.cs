using System;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.Licensee
{
    [Serializable]
    [DataContract]
    public class LicenseeRow
    {
        [DataMember]
        public int index { get; set; }

        [DataMember]
        public string ReportType { get; set; }

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
        public string Company { get; set; }

    }

    [Serializable]
    [DataContract]
    public class LicenseeRowV2
    { 
        [DataMember]
        public int index { get; set; }

        [DataMember]
        public string ReportType { get; set; }

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
        public string Racing { get; set; }

        [DataMember]
        public string WinlossRB { get; set; }

        [DataMember]
        public string Total { get; set; }

        [DataMember]
        public string AmtDuetoBA { get; set; }

        [DataMember]
        public string AmtDuetoRB { get; set; }

        [DataMember]
        public string Company { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Item
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool EnableBA { get; set; }

        [DataMember]
        public string Level { get; set; }

        [DataMember]
        public string To { get; set; }

        [DataMember]
        public string Cc { get; set; }

        [DataMember]
        public string Bcc { get; set; }
    }
}
