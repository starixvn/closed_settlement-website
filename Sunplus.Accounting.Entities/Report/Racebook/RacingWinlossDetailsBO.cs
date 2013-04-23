using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.Racebook
{
    [Serializable]
    [DataContract]
    public class RacingWinlossDetailsBO
    {
        [DataMember]
        public List<RacingWinlossCurrencyBO> Items { get; set; }

        [DataMember]
        public List<RacingWinlossRowBO> Totals { get; set; }

        [DataMember]
        public List<RacingWinlossRowBO> GrandTotals { get; set; }

    }
}
