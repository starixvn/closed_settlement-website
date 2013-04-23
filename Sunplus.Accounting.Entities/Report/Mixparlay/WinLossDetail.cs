using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Mixparlay
{
    [Serializable]
    [DataContract]
    public class WinLossDetail
    {
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string WinLoss { get; set; }

        [DataMember]
        public string Position { get; set; }

        [DataMember]
        public string Comm { get; set; }

        [DataMember]
        public string LineTotal { get; set; }

        [DataMember]
        public string Turnover { get; set; }

        [DataMember]
        public string Margin { get; set; }
    }
}
