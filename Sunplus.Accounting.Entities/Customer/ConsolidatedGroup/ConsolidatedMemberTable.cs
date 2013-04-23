using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Customer.ConsolidatedGroup
{
    [DataContract]
    public class ConsolidatedMemberTable
    {
        [DataMember]
        public int GroupID { get; set; }

        [DataMember]
        public int CustID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public bool IsHeader { get; set; }

        [DataMember]
        public string CurrencyName { get; set; }
    }
}
