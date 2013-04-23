
using System.Collections.Generic;
using ExcelUtil.Spreadsheet;
using Fanex.Tarus.Entities.Report;
using Fanex.Tarus.Entities.Sport;

namespace Fanex.Tarus.Business.Interface.Report
{
    public interface IMatchAnalysisService
    {
        MatchAnalysisDTO SelMatchAnalysisByType(MatchAnalysisParam param);

        ExcelFile ExportMatchAnalysis(MatchAnalysisParam param);

        List<Sporttype> SelLeague(MatchAnalysisParam param);
        List<League> SelMatchByLeagueids(MatchAnalysisParam param);
        List<Bettype> SelBettypeByMatchId(MatchAnalysisParam param);
    }
}
