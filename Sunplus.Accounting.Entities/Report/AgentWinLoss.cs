using System;

namespace Sunplus.Accounting.Entities.Report
{
    /// <summary>
    /// Summary description for AgentWinLoss
    /// </summary>
    [Serializable]
    public class AgentWinLoss : MemberWinLoss
    {
        public WinLossComm Master { get; set; }
    }
}