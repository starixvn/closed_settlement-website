using DNAide.Data;
using System;

namespace Sunplus.Accounting.DataAccess.Report.MemberStatistics
{
    public interface IGamesActiveMembers
    {
        DbResult GetNumberOfActiveMembers(DateTime fromDate, DateTime toDate, DateTime balanceUpDay);
    }
}
