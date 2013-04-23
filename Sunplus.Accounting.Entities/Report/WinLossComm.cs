using System;

namespace Sunplus.Accounting.Entities.Report
{
    /// <summary>
    /// Summary description for WinLossComm
    /// </summary>
    [Serializable]
    public class WinLossComm
    {
        /// <summary>
        /// WinLoss + Comm
        /// </summary>
        public double m_Total;

        public double WinLoss { get; set; }

        public double Comm { get; set; }

        public double Total
        {
            get
            {
                m_Total = WinLoss + Comm;
                return m_Total;
            }
            set { m_Total = value; }
        }
    }
}