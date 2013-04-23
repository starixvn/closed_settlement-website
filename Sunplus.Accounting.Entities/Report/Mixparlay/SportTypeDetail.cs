using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Mixparlay
{
    [Serializable]
    [DataContract]
    public class SportTypeDetail
    {
        [DataMember]
        public string SportName { get; set; }

        [DataMember]
        public List<WinLossDetail> ListWlDetail { get; set; }

        [DataMember]
        public WinLossDetail SubTotal { get; set; }

        [DataMember]
        public int TotalItems { get; set; }
    }
}
