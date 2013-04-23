using System;

namespace Sunplus.Accounting.Entities.ViewModel
{
    /// <summary>
    /// Summary description for DailyTransaction
    /// </summary>
    public class ResetWinning
    {
        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public bool IsReset { get; set; }
    }
}