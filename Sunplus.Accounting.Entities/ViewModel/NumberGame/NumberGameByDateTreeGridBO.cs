using System.Runtime.Serialization;
using System.Collections.Generic;
using System;

namespace Sunplus.Accounting.Entities.ViewModel
{
    [Serializable]
    [DataContract]
    public class NumberGameByDateTreeGridBO
    {
        [DataMember]
        public List<NumberGameWinLostByDateBO> rows { get; set; }
        [DataMember]
        public int total { get; set; }
        [DataMember]
        public int pageindex { get; set; }
        [DataMember]
        public NumberGameWinLostByDateBO totalrow;
    }
}
