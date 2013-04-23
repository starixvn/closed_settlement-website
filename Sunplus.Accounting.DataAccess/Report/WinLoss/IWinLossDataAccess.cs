using System.Collections;
using System.Collections.Generic;
using Sunplus.Accounting.Entities.Report;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.DataAccess.Report.WinLoss
{
    /// <summary>
    /// Summary description for IWinLossDataAccess
    /// </summary>
    public interface IWinLossDataAccess
    {
        List<MasterWinLoss> GetSuperWinLossData(Hashtable param);
        List<MasterWinLoss> GetMasterWinLossData(Hashtable param);
        List<AgentWinLoss> GetAgentWinLossData(Hashtable param);
        List<MemberWinLoss> GetMemberWinLossData(Hashtable param);
        List<WinLossDailyView> GetCompanyWinLossDaily(Hashtable param, bool hasGetFromBestTrans14, int reportLevel);
        int GetTotalOnlineMember(Enumerations.AccountingSystem system);
    }
}