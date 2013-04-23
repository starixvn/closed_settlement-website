using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.PositionTaking.Racing
{
    [Serializable]
    [DataContract]
    public class Tote
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool Visible { get; set; }
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public bool IsTop { get; set; }

        

        [DataMember]
        public Decimal MaxBet { get; set; }
        [DataMember]
        public Decimal MinBet { get; set; }
        [DataMember]
        public Decimal MaxPerMatch { get; set; }


        [DataMember]
        public Decimal MaxBetMax { get; set; }
        [DataMember]
        public Decimal MinBetMin { get; set; }
        [DataMember]
        public Decimal MaxPerMatchMax { get; set; }

        [DataMember]
        public bool UpdateDisable { get; set; }
        [DataMember]
        public bool Enable { get; set; }
        [DataMember]
        public bool EnableDownline { get; set; }
        [DataMember]
        public bool EnableAutoMPT { get; set; }

        [DataMember]
        public bool EnableUpline { get; set; }
        
        [DataMember]
        public Decimal Comm { get; set; }
        [DataMember]
        public bool UpdateComm { get; set; }

        [DataMember]
        public Decimal MasterComm { get; set; }
        
        [DataMember]
        public Decimal AgentComm { get; set; }
        
        [DataMember]
        public Decimal MemberComm { get; set; }

        [DataMember]
        public Decimal MaxComm { get; set; }

        [DataMember]
        public Decimal MinComm { get; set; }

        [DataMember]
        public Decimal HousePt { get; set; }

        [DataMember]
        public Decimal MaxHousePt { get; set; }

        [DataMember]
        public Decimal MinHousePt { get; set; }

        [DataMember]
        public List<Bettype> Bettypes { get; set; }

        [DataMember]
        public bool IsApplyPresetPT { get; set; }

        [DataMember]
        public bool UpdateMinMax { get; set; }
        [DataMember]
        public bool UpdatePT { get; set; }
    }
}
