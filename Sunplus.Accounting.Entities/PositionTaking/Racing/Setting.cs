using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.PositionTaking.Racing
{
    [Serializable]
    [DataContract]
    public class Setting
    {
        [DataMember]
        public List<Sport> RacingSportType { get; set; }

        [DataMember]
        public CustInfo CustInfo { get; set; }

        [DataMember]
        public Decimal MaxPosition { get; set; }

        [DataMember]
        public bool IsHideCopyBetSetting { get; set; }

        [DataMember]
        public string DefaultRacingSportType { get; set; }

        [DataMember]
        public string DefaultRacingSportTypeId { get; set; }

        [DataMember]
        public bool IsUpdateAllMember { get; set; }

        [DataMember]
        public decimal NewPt { get; set; }

        [DataMember]
        public string Crypting { get; set; }
    }

    public class CustInfo
    {
        public enum Roles 
        {
            Super = 4,
            Master = 3,
            Agent = 2,
            Member = 1,
            SuperOnMember = -1,
            MasterOnMember = -2,
            SuperMin = -3,
            MasterMinMax = -4,
            AgentMinMax = -5
        }
        public int CustId { get; set; }
        public int CustLevel {get; set;}
        public string UserName {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Phone {get; set;}
        public string MobilePhone {get; set;}
        public string Fax { get; set; }
        public int Currency { get; set; }
    }
}
