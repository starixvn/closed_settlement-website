using System;
using System.Collections.Generic;
using System.Data;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Entities.Report.Monitor;

namespace Sunplus.Accounting.DataAccess.IReportDAO
{
    public interface IMonitorDAO
    {
        DataTable GetCurrency();

        DataTable GetSports();

        DataTable GetBettypes(List<Items> option, int sporttype);

        DataTable GetLeague(List<Items> option, int sportId, string fromdate, string todate, int currency, int bettype, int live);

        DataTable GetMAMonitorReport(Param param);
    }
}
