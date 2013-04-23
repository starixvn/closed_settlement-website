using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunplus.Accounting.Entities.BetAggregator
{
    public class AmtLimitLicensee
    {
        public int LicenseeID { get; set; }
        public string LicenseeName { get; set; }
        public double LimitAmt { get; set; }
    }
}
