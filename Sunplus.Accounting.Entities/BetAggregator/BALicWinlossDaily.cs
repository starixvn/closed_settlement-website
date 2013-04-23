using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunplus.Accounting.Entities.BetAggregator
{
    public class BALicWinlossDaily
    {
        public string LicenseeName { get; set; }
        public int LicenseeID { get; set; }
        public int Currency { get; set; }
        public string CurrencyName { get; set; }
        public double TurnOverRM { get; set; }
        public double WinlostVirtual { get; set; }
        public double WinlostRM { get; set; }
        public double AmtDueToBAVirtual { get; set; }
        public double AmtDueToBARM { get; set; }
        public double CommVirtual { get; set; }
        public double CommRM { get; set; }
        public double BetCount { get; set; }
        public bool LiveIndicator { get; set; }
        public DateTime WinlostDate { get; set; }
    }
}
