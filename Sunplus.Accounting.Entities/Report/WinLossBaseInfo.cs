using System;

namespace Sunplus.Accounting.Entities.Report
{
    /// <summary>
    /// Summary description for WinLossBaseInfo
    /// </summary>
    [Serializable]
    public class WinLossBaseInfo
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public double TurnOver { get; set; }

        public double GrossComm { get; set; }

        public double UplineTotal { get; set; }
    }
}