using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Provider.Report;

/// <summary>
/// Summary description for PunterAnalysisProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.Report
{
    public class HRGroupDailyProviderClient : HRGroupDailyProvider
    {
        /// <summary>
        /// OpenTote - new report
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override DbResult WinlostHR_Group_Daily(Hashtable param)
        {
            return DbHelper.Execute("Acc_Rpt_WinlostHR_Group_Daily", param, DBType.REPORT, 600);
        }


        /// <summary>
        /// Open Tote - new report
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override DbResult WinlostHR_Group(Hashtable param)
        {
            return DbHelper.Execute("Acc_Rpt_WinlostHR_Group", param, DBType.REPORT, 600);
        }
    }
}