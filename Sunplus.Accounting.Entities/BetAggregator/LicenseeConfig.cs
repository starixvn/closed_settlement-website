using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.BetAggregator
{
    [DataContract]
    public class LicenseeConfig
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double ValueGroupA { get; set; }

        [DataMember]
        public double ValueGroupB { get; set; }

        [DataMember]
        public double ValueGroupC { get; set; }

        [DataMember]
        public double ValueGroupD { get; set; }

        [DataMember]
        public string ValueGroup { get; set; }

        [DataMember]
        public double Value1X2Comm { get; set; }

        [DataMember]
        public double ValueOtherComm { get; set; }

        [DataMember]
        public string ModifyBy { get; set; }

        [DataMember]
        public string ModifyDate { get; set; }

        [DataMember]
        public bool EnableBa { get; set; }


    }

    [DataContract]
    public  class CommType
    {
        [DataMember]
        public string CommName { get; set; }

        [DataMember]
        public double CommMin { get; set; }

        [DataMember]
        public double CommMax { get; set; }
    }

    [DataContract]
    public class LicenseeConfigLog
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double ValueHdpComm { get; set; }

        [DataMember]
        public double ValueGroupA { get; set; }

        [DataMember]
        public double ValueGroupB { get; set; }

        [DataMember]
        public double ValueGroupC { get; set; }

        [DataMember]
        public double ValueGroupD { get; set; }

        [DataMember]
        public string ValueGroup { get; set; }

        [DataMember]
        public double Value1X2Comm { get; set; }

        [DataMember]
        public double ValueOtherComm { get; set; }

        [DataMember]
        public double ValueGameComm { get; set; }

        [DataMember]
        public string ModifyBy { get; set; }

        [DataMember]
        public bool EnableBa { get; set; }

        [DataMember]
        public string ModifyDate { get; set; }

        [DataMember]
        public string Description { get; set; }
       
        [DataMember]
        public string IP { get; set; }
    }

    [DataContract]
    public class LicenseeLog
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public List<LicenseeConfigLog> Items { get; set; }
       
    }

}

