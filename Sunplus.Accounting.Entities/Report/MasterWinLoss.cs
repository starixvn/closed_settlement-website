using System;

namespace Sunplus.Accounting.Entities.Report
{
    /// <summary>
    /// Summary description for MasterWinLoss
    /// </summary>
    [Serializable]
    public class MasterWinLoss : AgentWinLoss
    {
        public WinLossComm Super { get; set; }
    }
}