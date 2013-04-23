using System.Collections.Generic;
using System.Runtime.Serialization;
using Sunplus.Accounting.Entities.Customer;

namespace Sunplus.Accounting.Entities.PositionTaking.Poker
{
    [DataContract]
    public class DefaultSetting
    {
        [DataMember]
        public List<DefaultSettingItem> items { get; set; }

        [DataMember]
        public int errCode { get; set; }

        [DataMember]
        public string errMessage { get; set; }
       
    }

    public class DefaultSettingItem
    {
        [DataMember]
        public string CurrencyID { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string MinCredit { get; set; }

        [DataMember]
        public string MaxCredit { get; set; }

        [DataMember]
        public string Maxsdiscount { get; set; }

        [DataMember]
        public string Maxadiscount { get; set; }

        [DataMember]
        public string Maxdiscount { get; set; }

        [DataMember]
        public string Maxplayerdiscount { get; set; }
    }
}
