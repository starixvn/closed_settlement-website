using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Licensee
{
    [Serializable]
    [DataContract]
    public class LicenseeTotalTransction
    {
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public string total { get; set; }
    }

    [Serializable]
    [DataContract]
    public class  LicenseeTotalTransctionDTO
    {
        [DataMember]
        public List<Reseller> Items { get; set; }

        [DataMember]
        public Reseller Total { get; set; }

        [DataMember]
        public List<string> SelDate { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Licensee
    {
        [DataMember]
        public string LicenseeID { get; set; }

        [DataMember]
        public string LicenseeName { get; set; }
        
        [DataMember]
        public string Total { get; set; }

        [DataMember]
        public string EURCost { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Reseller
    {
        [DataMember]
        public string ResellerID { get; set; }

        [DataMember]
        public string ResellerName { get; set; }

        [DataMember]
        public string Total { get; set; }
        [DataMember]
        public string EURCost { get; set; }
        
        [DataMember]
        public List<Licensee> Licensees { get; set; }
    }
}
