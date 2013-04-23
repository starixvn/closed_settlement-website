using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.BetAggregator
{
    [Serializable]
    [DataContract]
    public class BACurrenyGroupDTO
    {
        [DataMember]
        public string LicensesId { get; set; }
        [DataMember]
        public string LicenseeName { get; set; }
        [DataMember]
        public List<GroupSetting> GroupSettings { get; set; }
    }

    [Serializable]
    [DataContract]
    public class GroupSetting
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public List<CurrencyInfo> Currencies { get; set; }
    }

    [Serializable]
    [DataContract]
    public  class CurrencyInfo
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool Checked { get; set; }
    }
}
