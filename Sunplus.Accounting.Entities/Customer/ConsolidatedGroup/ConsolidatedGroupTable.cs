using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Customer.ConsolidatedGroup
{
    [DataContract]
    public class ConsolidatedGroupTable
    {
        [DataMember]
        public int GroupID { get; set; }

        [DataMember]
        public int CustID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string CurrencyName { get; set; }

        [DataMember]
        public int TotalMember { get; set; }
    }
}
