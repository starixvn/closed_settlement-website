using System;

namespace Sunplus.Accounting.Entities.Report
{
    /// <summary>
    /// Summary description for MemberWinLoss
    /// </summary>
    [Serializable]
    public class MemberWinLoss : IWinLoss
    {
        public WinLossComm Member { get; set; }

        public WinLossComm Agent { get; set; }

        #region IWinLoss Members

        public int CustId { get; set; }

        public string Username { get; set; }

        public double Turnover { get; set; }

        public double GrossComm { get; set; }

        public double UplineTotal { get; set; }

        #endregion
    }
}