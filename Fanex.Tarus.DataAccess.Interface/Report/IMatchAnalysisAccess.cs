
using System.Data;
using Fanex.Tarus.Entities.Report;

namespace Fanex.Tarus.DataAccess.Interface.Report
{
    public interface IMatchAnalysisAccess
    {
        ReportType GetReportType(int reportType);

        DataTable SelMatchAnalysisByType(MatchAnalysisParam param);

        DataTable SelLeague(MatchAnalysisParam param);

        DataTable SelMatchByLeagueids(MatchAnalysisParam param);

        DataTable SelBettypeByMatchId(MatchAnalysisParam param);

    }
}
