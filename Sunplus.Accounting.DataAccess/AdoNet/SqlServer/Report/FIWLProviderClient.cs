using System;
using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Provider.Report;

/// <summary>
/// Summary description for FIWLProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.Report
{
    public class FIWLProviderClient : FIWLProvider
    {
        public override DbResult FIWLDetails(Hashtable param)
        {
            try
            {
                return DbHelper.Execute("Acc_Rpt_WinlostFI_Details", param, DBType.REPORT, 600);
            }
            catch (InvalidOperationException invalidOEx)
            {
                throw invalidOEx;
            }
        }

        public override DbResult FIWLDetails_14(Hashtable param)
        {
            try
            {
                return DbHelper.Execute("Acc_Rpt_WinlostFI_Details14", param, DBType.REPORT, 600);
            }
            catch (InvalidOperationException invalidOEx)
            {
                throw invalidOEx;
            }
        }
    }
}