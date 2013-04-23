using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.PositionTaking.Racing
{
    [Serializable]
    [DataContract]
    public class Partner
    {
        [DataMember]
        public int PartnerId { get; set; }
        [DataMember]
        public string PartnerName { get; set; }
    }

    [Serializable]
    [DataContract]
    public class PartnerItem
    {
        [DataMember]
        public string MeetingName { get; set; }
        [DataMember]
        public string PoolCurrency { get; set; }
        [DataMember]
        public decimal CommWin { get; set; }
        [DataMember]
        public decimal CommPlace { get; set; }
        [DataMember]
        public decimal Fee { get; set; }
    }
}
