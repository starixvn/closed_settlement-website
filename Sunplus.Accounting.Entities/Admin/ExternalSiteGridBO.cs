using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Sunplus.Accounting.Entities.Customer;

namespace Sunplus.Accounting.Entities.Admin
{
    [Serializable]

    [DataContract]
    public class ExternalSiteGridBO
    {
        [DataMember]
        public List<CustDetail> rows { get; set; }

        [DataMember]
        public int total { get; set; }

        [DataMember]
        public int pageindex { get; set; }
    }
}
