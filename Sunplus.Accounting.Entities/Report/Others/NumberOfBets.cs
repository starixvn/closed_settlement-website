using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Sunplus.Accounting.Entities.Report.Others
{
    [Serializable]
    [DataContract]
    public class NumberOfBets
    {
        [DataMember]
        public string Site { get; set; }
        [DataMember]
        public int BetCount { get; set; }
    }

    [Serializable]
    [DataContract]
    public class NumberOfBetsDTO
    {
        [DataMember]
        public List<NumberOfBets> rows { get; set; }
        [DataMember]
        public NumberOfBets total { get; set; }
    }
}
