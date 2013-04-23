using System.Runtime.Serialization;
using System.Collections.Generic;
using System;
using Sunplus.Accounting.Entities.Report.Mixparlay;

namespace Sunplus.Accounting.Entities.ViewModel
{
    [Serializable]

    [DataContract]
    public class MixparlayDataGridBO
    {
        [DataMember]
        public List<MixparlayWonBO> rows { get; set; }
        
        [DataMember]
        public int total { get; set; }
        
        [DataMember]
        public int pageindex { get; set; }
        
        [DataMember]
        public List<MixparlayWonBO> footer { get; set; }
    }
}
