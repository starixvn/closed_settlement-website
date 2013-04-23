using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Deposit
{
    [Serializable]
    [DataContract]
    public class DepositCurrencyBO
    {
        [DataMember]
        public string currency { get; set; }

        [DataMember]
        public int currencyId { get; set; }

        [DataMember]
        public long custId{ get; set; }
    }
}
