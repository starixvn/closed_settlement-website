using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Customer.Racing
{
    [Serializable]
    [DataContract]
    public class EnableRacebooksStatus
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public bool Enable { get; set; }
        [DataMember]
        public bool EnableDownline { get; set; }
    }

    [Serializable]
    [DataContract]
    public class EnableRacebooksStatusDTO
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool Visible { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public List<EnableRacebooksStatus> RacebooksStatus { get; set; }
    }
}
