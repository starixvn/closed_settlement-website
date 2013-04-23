using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sunplus.Accounting.Entities.Report.Deposit;

namespace Sunplus.Accounting.Entities.Report.Licensee
{
    [Serializable]
    [DataContract]
    public class LicenseeDTO
    {
        [DataMember]
        public IEnumerable<DepositCurrencyBO> currency { get; set; }

        [DataMember]
        public List<LicenseeRow> Items { get; set; }

        [DataMember]
        public List<LicenseeRow> Totals { get; set; }

        [DataMember]
        public List<LicenseeRow> TotalsBaseCurrency { get; set; }
       

        [DataMember]
        public string fdate { get; set; }

        [DataMember]
        public string tdate { get; set; }

        [DataMember]
        public int totalrow { get; set; }

      

    }

    [Serializable]
    [DataContract]
    public class LicenseeDTOV2
    {
        [DataMember]
        public IEnumerable<DepositCurrencyBO> currency { get; set; }

        [DataMember]
        public List<LicenseeRowV2> Items { get; set; }

        [DataMember]
        public List<LicenseeRowV2> Totals { get; set; }

        [DataMember]
        public List<LicenseeRowV2> TotalsBaseCurrency { get; set; }

        [DataMember]
        public string fdate { get; set; }

        [DataMember]
        public string tdate { get; set; }

        [DataMember]
        public int totalrow { get; set; }

    }
}
