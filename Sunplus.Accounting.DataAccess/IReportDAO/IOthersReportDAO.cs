using System;
using System.Data;
using DNAide.Data;

namespace Sunplus.Accounting.DataAccess.IReportDAO
{
    public interface IOthersReportDAO
    {
        DataTable SelectNumberOfBetsForCreditSites(DateTime fromDate, DateTime toDate);

        DbResult SelDuplicateTickets(string wldate);
    }
}
