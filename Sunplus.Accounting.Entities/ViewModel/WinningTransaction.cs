using System;

namespace Sunplus.Accounting.Entities.ViewModel
{
    /// <summary>
    /// Summary description for DailyTransaction
    /// </summary>
    public class WinningTransaction
    {
        public DateTime Date { get; set; }

        public int Type { get; set; }

        public string Remark { get; set; }

        public decimal Amount { get; set; }
    }
}