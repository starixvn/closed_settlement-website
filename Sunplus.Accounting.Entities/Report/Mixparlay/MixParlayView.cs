using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Mixparlay
{
    [Serializable]
    [DataContract]
    public class MixParlayView
    {
        [DataMember]
        public List<LiveTypeDetail> ListLiveType { get; set; }

        [DataMember]
        public WinLossDetail GrandTotal { get; set; }
    }
}
