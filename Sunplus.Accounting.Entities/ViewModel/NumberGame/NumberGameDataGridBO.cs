using System.Runtime.Serialization;
using System.Collections.Generic;
using System;

namespace Sunplus.Accounting.Entities.ViewModel
{
    [Serializable]

    [DataContract]
    public class NumberGameDataGridBO
    {
        [DataMember]
        public List<NumberGameWinLostDetailBO> rows { get; set; }
        
        [DataMember]
        public int total { get; set; }
        
        [DataMember]
        public int pageindex { get; set; }
        
        [DataMember]
        public List<NumberGameWinLostDetailBO> footer { get; set; }
    }
}
