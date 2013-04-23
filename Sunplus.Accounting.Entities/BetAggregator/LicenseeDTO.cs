using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sunplus.Accounting.Entities.Report.Deposit;

namespace Sunplus.Accounting.Entities.BetAggregator
{
    
    [Serializable]
    [DataContract]
    public class Report_LicenseeWLBA_DTO
    {

        [DataMember]
        public IEnumerable<DepositCurrencyBO> currency { get; set; }

        [DataMember]
        public List<Report_LicenseeWLBA_Row> Items { get; set; }

        [DataMember]
        public List<Report_LicenseeWLBA_Row> Totals { get; set; }

        [DataMember]
        public List<Report_LicenseeWLBA_Row> TotalsBaseCurrency { get; set; }
      
        [DataMember]
        public int totalrow { get; set; }

        [DataMember]
        public List<int> DateGroup { get; set; }

        [DataMember]
        public string fdate { get; set; }

        [DataMember]
        public string tdate { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Report_LicenseeWLBA_DTOV2
    {

        [DataMember]
        public IEnumerable<DepositCurrencyBO> currency { get; set; }

        [DataMember]
        public List<Report_LicenseeWLBA_RowV2> Items { get; set; }

        [DataMember]
        public List<Report_LicenseeWLBA_RowV2> Totals { get; set; }

        [DataMember]
        public List<Report_LicenseeWLBA_RowV2> TotalsBaseCurrency { get; set; }

        [DataMember]
        public int totalrow { get; set; }

        [DataMember]
        public string fdate { get; set; }

        [DataMember]
        public string tdate { get; set; }
    }

    [Serializable]
    [DataContract]
    public class LicenseeWLBA
    {

        [DataMember]
        public string LicenseeId { get; set; }
        [DataMember]
        public string Licensee { get; set; }
        [DataMember]
        public List<DepositCurrencyBO> currency { get; set; }

        [DataMember]
        public List<int > DateGroup { get; set; }

        [DataMember]
        public List<Report_LicenseeWLBA_Row> Items { get; set; }

        [DataMember]
        public List<Report_LicenseeWLBA_Row> Totals { get; set; }

        [DataMember]
        public List<Report_LicenseeWLBA_Row> TotalsBaseCurrency { get; set; }
    }

    [Serializable]
    [DataContract]
    public class LicenseesWLBA
    {
        [DataMember]
        public List<LicenseeWLBA> Items { get; set; }

        [DataMember]
        public List<Report_LicenseeWLBA_Row> TotalsBaseCurrency { get; set; }

        [DataMember]
        public int totalrow { get; set; }

        [DataMember]
        public string fdate { get; set; }

        [DataMember]
        public string tdate { get; set; }
    }
}
