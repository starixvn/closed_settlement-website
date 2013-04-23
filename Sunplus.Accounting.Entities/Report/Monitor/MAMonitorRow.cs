using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Monitor
{
     [Serializable]
    [DataContract]
    public class MAMonitorDTO
     {
         [DataMember]
         public List<MAMonitorRow> Item { get; set; }

         [DataMember]
         public MAMonitorRow Total { get; set; }

         [DataMember]
         public int TotalRows { get; set; }

     }

    [Serializable]
    [DataContract]
    public class MAMonitorRow
    {
        [DataMember]
        public string index { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Turnover { get; set; }

        [DataMember]
        public string Winloss { get; set; }

        [DataMember]
        public string Betcount { get; set; }

        [DataMember]
        public string ATurnover { get; set; }

        [DataMember]
        public string AWinloss { get; set; }

        [DataMember]
        public string MTurnover { get; set; }

        [DataMember]
        public string MWinloss { get; set; }

        [DataMember]
        public string STurnover { get; set; }

        [DataMember]
        public string SWinloss { get; set; }

       [DataMember]
        public string Home { get; set; }

        [DataMember]
        public string Away { get; set; }
    }
}
