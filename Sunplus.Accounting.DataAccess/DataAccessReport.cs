using System;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.Report;

/// <summary>
/// Summary description for DataAccess
/// </summary>

namespace Sunplus.Accounting.DataAccess
{
    public class DataAccessReport:DataAccess
    {
        protected static string GetStoreMappingSubfix(DateTime wldate)
        {
            if (wldate > ReportUtils.BalanceUp.MoveBetsDay)
                return string.Empty;
            else if (wldate >= ReportUtils.BalanceUp.BalanceUpDay)
                return "_14";
            else
                return "_WH";
        }
    }
}