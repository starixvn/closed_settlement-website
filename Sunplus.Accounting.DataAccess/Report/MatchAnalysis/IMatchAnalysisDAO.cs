
using System.Data;
using Sunplus.Accounting.Entities.Report.MatchAnalysis;

namespace Sunplus.Accounting.DataAccess.Report.MatchAnalysis
{
    public interface IMatchAnalysisDAO
    {
        DataTable SelMatchAnalysisByType(int matchId, int statusId, ReportType reportType);        
    }
}
