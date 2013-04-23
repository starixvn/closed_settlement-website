using System;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Provider.Report;

/// <summary>
/// Summary description for PunterAnalysisProviderClient
/// </summary>
namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.Report
{
    public class NonePTMemSummaryProviderClient : NonePTMemSummaryProvider
    {
        public override DbResult NonePTMemSummary(System.Collections.Hashtable param)
        {
            try
            {
                return DbHelper.Execute("Acc_Rpt_NonePTMemSummary", param, DBType.REPORT, 1000);
            }
            catch (InvalidOperationException invalidOEx)
            {
                throw invalidOEx;
            }
        }  
    }
}