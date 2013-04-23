using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.Racebook
{
    [Serializable]
    [DataContract]
    public class RacingWinlossMeetingBO
    {
        [DataMember]
        public List<RacingWinlossRowBO> Items { get; set; }

        [DataMember]
        public List<RacingWinlossRowBO> Totals { get; set; }
       
    }
}
