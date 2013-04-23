using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.DailyWL
{
    [Serializable]
    [DataContract]
    public class DailyWLBO
    {
        [DataMember]
        public List<DailyWLRowBO> Items { get; set; }

        [DataMember]
        public List<DailyWLRowBO> Totals { get; set; }
    }
}
