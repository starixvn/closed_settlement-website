using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.LogDisplay.SettingPT
{
    [Serializable]
    [DataContract]
    public class TrackingLog
    {
        [DataMember]
        public int Level { get; set; }
        [DataMember]
        public string SP { get; set; }
        [DataMember]
        public List<TrackingNode> Vals { get; set; }
        [DataMember]
        public List<TrackingElement> Attrs { get; set; }
    }
    [Serializable]
    [DataContract]
    public class TrackingNode
    {
        [DataMember]
        public string N { get; set; } // name
        [DataMember]
        public List<TrackingElement> Vals { get; set; } // values
        [DataMember]
        public List<TrackingGroup> Groups { get; set; } // groups
    }
    [Serializable]
    [DataContract]
    public class TrackingGroup
    {
        [DataMember]
        public string N { get; set; } // name
        [DataMember]
        public List<TrackingElement> Vals { get; set; } // value
    }

    [Serializable]
    [DataContract]
    public class TrackingValue
    {
        [DataMember]
        public string P { get; set; } // param name
        [DataMember]
        public string Val { get; set; } // value
    }
    [Serializable]
    [DataContract]
    public class TrackingElement : TrackingValue
    {
        [DataMember]
        public string N { get; set; } // name
    }
}
