using System;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.PositionTaking.Racing
{
    [Serializable]
    [DataContract]
    public class Data
    {
      
        [DataMember]
        public Decimal Pt { get; set; }
        [DataMember]
        public Decimal MinPt { get; set; }
        [DataMember]
        public Decimal MaxPt { get; set; }

        [DataMember]
        public Decimal AutoPt { get; set; }
        [DataMember]
        public Decimal MinAutoPt { get; set; }
        [DataMember]
        public Decimal MaxAutoPt { get; set; }

        [DataMember]
        public bool IsAutoPt { get; set; }

        [DataMember]
        public Decimal UplinePt { get; set; }
        [DataMember]
        public Decimal MinUplinePt { get; set; }
        [DataMember]
        public Decimal MaxUplinePt { get; set; }


       

        [DataMember]
        public Decimal MasterPt { get; set; }

        [DataMember]
        public Decimal AgentPt { get; set; }

        // check master auto on member
        [DataMember]
        public bool IsUplineAutoPt { get; set; }
    }
}
