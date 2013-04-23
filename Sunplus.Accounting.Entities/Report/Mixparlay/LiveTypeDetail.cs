using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Mixparlay
{
    [Serializable]
    [DataContract]
    public class LiveTypeDetail
    {
        [DataMember]
        public string LiveName { get; set; }

        [DataMember]
        public List<SportTypeDetail> ListSportType { get; set; }

        [DataMember]
        public WinLossDetail Total { get; set; }

        [DataMember]
        public int TotalItems { get; set; }
    }
}
