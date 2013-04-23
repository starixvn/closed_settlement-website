using System;

namespace Sunplus.Accounting.Entities.ViewModel
{
    /// <summary>
    /// Summary description for DailyTransaction
    /// </summary>
    public class FinanceTransaction
    {
        public int BetType { get; set; }

        public string Transdesc { get; set; }

        public string Remark { get; set; }

        public Decimal Amount { get; set; }
    }
}