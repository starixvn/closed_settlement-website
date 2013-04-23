using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.Racebook
{
    [Serializable]
    [DataContract]
    public class RacingWinlossBO
    {
        [DataMember]
        public List<RacingWinlossMeetingBO> Items { get; set; }

        [DataMember]
        public List<RacingWinlossRowBO> Totals { get; set; }

        [DataMember]
        public bool IsGotDumpFile { get; set; }

        [DataMember]
        public bool IsTotalTOMatch { get; set; }
        [DataMember]
        public bool IsDividendMatch { get; set; }

        [DataMember]
        public List<RacingWinlossDumpFileBO> Comparision { get; set; }

    }
}
