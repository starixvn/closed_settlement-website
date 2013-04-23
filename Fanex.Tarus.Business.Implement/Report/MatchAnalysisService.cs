using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using DNAide.Core;
using DNAide.Utils;
using ExcelUtil.Spreadsheet;
using Fanex.Tarus.Business.Interface.Report;
using Fanex.Tarus.DataAccess.Implement.Report;
using Fanex.Tarus.DataAccess.Interface.Report;
using Fanex.Tarus.Entities.Report;
using Fanex.Tarus.Entities.Sport;

using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;

namespace Fanex.Tarus.Business.Implement.Report
{
    public class MatchAnalysisService : IMatchAnalysisService
    {
        private IMatchAnalysisAccess _analysisDao;
        private const int MaxRetryNumber = 10;

        public MatchAnalysisService(IMatchAnalysisAccess analysisDao)
        {
            _analysisDao = analysisDao ?? new MatchAnalysisAccess(); ;
        }

        public MatchAnalysisService()
            : this(null)
        {
        }

        #region GetKeyForSession

        private string GetKeyForSession(ReportType reportType)
        {
            string key = "";
            switch (reportType)
            {
                case ReportType.Currency:
                    key = "MatchAnalysisByCurrency";
                    break;
                case ReportType.Bettype:
                    key = "MatchAnalysisByBettype";
                    break;
                case ReportType.Super:
                    key = "MatchAnalysisBySuper";
                    break;
                case ReportType.Member:
                    key = "MatchAnalysisByMember";
                    break;
                case ReportType.Country:
                    key = "MatchAnalysisByCountry";
                    break;
            }
            return key;
        }

        #endregion GetKeyForSession
        #region GetNameSheet

        private string GetNameSheet(ReportType reportType)
        {
            string key = "";
            switch (reportType)
            {
                case ReportType.Currency:
                    key = "MatchAnalysisByCurrency";
                    break;
                case ReportType.Bettype:
                    key = "MatchAnalysisByBettype";
                    break;
                case ReportType.Super:
                    key = "MatchAnalysisBySuper";
                    break;
                case ReportType.Member:
                    key = "MatchAnalysisByMember";
                    break;
                case ReportType.Country:
                    key = "MatchAnalysisByCountry";
                    break;
            }
            return key;
        }

        #endregion GetNameSheet
        #region GetColumnNameForExcel

        private string GetColumnNameForExcel(ReportType reportType, int statusId)
        {
            string name = "";
            switch (reportType)
            {
                case ReportType.Currency:
                    name = "Currency";
                    break;
                case ReportType.Bettype:
                    name = "Bet type";
                    break;
                case ReportType.Super:
                    name = "Super";
                    break;
                case ReportType.Member:
                    switch (statusId)
                    {
                        case 112:
                            name = "Top 100 (WON) Member";
                            break;
                        case 113:
                            name = "Top 100 (LOSE) Member";
                            break;
                    }

                    break;
                case ReportType.Country:
                    name = "MatchAnalysisByCountry";
                    break;
            }
            return name;
        }

        #endregion GetColumnNameForExcel
        #region SelMatchAnalysisByType

        public MatchAnalysisDTO SelMatchAnalysisByType(MatchAnalysisParam param)
        {
            MatchAnalysisDTO match = new MatchAnalysisDTO();
            match.Items = new List<MatchAnalysis>();
            match.Total = new MatchAnalysis();

            try
            {
                DataTable dataTable = _analysisDao.SelMatchAnalysisByType(param);
                if (dataTable != null)
                {
                    DataRow[] rows = dataTable.Select();
                    string itemName = "", currency = "";
                    int uusCurrencyId = SpecialMatchAnalysisSection.Instance.Currency["UUS"].Id;
                    #region Assign Data
                    foreach (DataRow row in rows)
                    {
                        var isExcludeRow = false;
                        switch (_analysisDao.GetReportType(param.ReportType))
                        {
                            case ReportType.Currency:
                                int currencyId = CastAide.AsInt32(row["currency"]);
                                if (currencyId == uusCurrencyId)
                                {
                                    isExcludeRow = true;
                                }
                                itemName = CacheUtils.GetExchangeName(currencyId);
                                break;
                            case ReportType.Bettype:
                                itemName = ExecuteCacheRefWithRetry(row["bettype"], CacheRef.GetBettype);
                                break;
                            case ReportType.Super:
                                itemName = CastAide.AsString(row["username"]);
                                break;
                            case ReportType.Member:
                                itemName = CastAide.AsString(row["username"]);
                                currency = CacheUtils.GetExchangeName(CastAide.AsInt32(row["currency"]));
                                break;
                        }
                        if (isExcludeRow)
                        {
                            continue;
                        }
                        match.Items.Add(new MatchAnalysis()
                        {
                            ItemName = itemName,
                            Currency = currency,
                            BetCount = NumberFormat.DecFormat((row["betcount"]), 0),
                            MemberTO = NumberFormat.DecFormat((row["MemberTurnover"]), 2),
                            MemberWL = NumberFormat.DecFormat((row["MemberWinLost"]), 2),
                            HouseTO = NumberFormat.DecFormat((row["HouseTurnover"]), 2),
                            HouseWL = NumberFormat.DecFormat((row["HouseWinloss"]), 2),
                            SMATO = NumberFormat.DecFormat((row["SMATurnover"]), 2),
                            SMAWL = NumberFormat.DecFormat((row["SMAWinLost"]), 2),
                        });

                    }

                    match.Total = new MatchAnalysis()
                    {
                        ItemName = "TOTAL",
                        BetCount = NumberFormat.DecFormat(match.Items.Sum(s => CastAide.AsDecimal(s.BetCount)), 0),
                        MemberTO = NumberFormat.DecFormat(match.Items.Sum(s => CastAide.AsDecimal(s.MemberTO)), 2),
                        MemberWL = NumberFormat.DecFormat(match.Items.Sum(s => CastAide.AsDecimal(s.MemberWL)), 2),
                        HouseTO = NumberFormat.DecFormat(match.Items.Sum(s => CastAide.AsDecimal(s.HouseTO)), 2),
                        HouseWL = NumberFormat.DecFormat(match.Items.Sum(s => CastAide.AsDecimal(s.HouseWL)), 2),
                        SMATO = NumberFormat.DecFormat(match.Items.Sum(s => CastAide.AsDecimal(s.SMATO)), 2),
                        SMAWL = NumberFormat.DecFormat(match.Items.Sum(s => CastAide.AsDecimal(s.SMAWL)), 2),
                        Currency = string.Empty,
                    };
                }
                    #endregion
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            string key = GetKeyForSession(_analysisDao.GetReportType(param.ReportType));
            AppSession.AddSession(HttpContext.Current.Session, key, match);

            return match;
        }

        #endregion SelMatchAnalysisByType
        #region GetStatusById

        private string GetStatusById(int statusId)
        {
            string status = string.Empty;
            switch (statusId)
            {
                case 0:
                    status = "All";
                    break;
                case 112:
                    status = "Won";
                    break;
                case 113:
                    status = "Lose";
                    break;
                case 111:
                    status = "Draw";
                    break;
            }
            return status;
        }

        #endregion GetStatusById
        #region GetLiveStatusById

        private string GetLiveStatusById(int liveStatusId)
        {
            string liveStatus = string.Empty;
            switch (liveStatusId)
            {
                case -1:
                    liveStatus = "All";
                    break;
                case 1:
                    liveStatus = "Live";
                    break;
                case 0:
                    liveStatus = "None-Live";
                    break;
            }
            return liveStatus;
        }

        #endregion GetLiveStatusById
        #region GetBetypeDisplay

        private string GetBetypeDisplay(string bettypes)
        {
            string bettypesResult = "";
            if (string.IsNullOrEmpty(bettypes))
            {
                bettypesResult = "All";
            }
            else
            {
                var betTypeList = bettypes.Split(',');
                foreach (var bettype in betTypeList)
                {
                    bettypesResult += ExecuteCacheRefWithRetry(bettype, CacheRef.GetBettype) + ", ";
                }
                if (bettypesResult.Length >= 2)
                {
                    bettypesResult = bettypesResult.Substring(0, bettypesResult.Length - ", ".Length);
                }
            }
            return bettypesResult;
        }

        #endregion GetBetypeDisplay
        #region ExportMatchAnalysis

        public ExcelFile ExportMatchAnalysis(MatchAnalysisParam param)
        {
            ExcelFile excelFile = new ExcelFile();
            var exlStyle = new ExcelGemboxUtils();

            ExcelWorksheetCollection worksheets = excelFile.Worksheets;
            ExcelWorksheet ws = null;

            var reportType = _analysisDao.GetReportType(param.ReportType);

            string tag = GetNameSheet(_analysisDao.GetReportType(param.ReportType));
            ws = worksheets.Add(tag);
            int indexRow = 6;
            try
            {
                string key = GetKeyForSession(reportType);
                MatchAnalysisDTO match = SelMatchAnalysisByType(param);

                int colspan = 7;
                if (reportType == ReportType.Member)
                {
                    colspan = 8;
                }
                int r = 142, g = 179, b = 231;

                #region Title
                ws.Cells.GetSubrangeAbsolute(0, 0, 1, colspan).Value = "Special Match Analysis (Base on RM) ";
                ws.Cells.GetSubrangeAbsolute(0, 0, 1, colspan).Style = exlStyle.GetStyleTitle(252, 213, 180);
                ws.Cells.GetSubrangeAbsolute(0, 0, 1, colspan).Merged = true;

                ws.Cells.GetSubrangeAbsolute(2, 0, 4, colspan).Value = "[" + param.EventDate + "] "
                    + ExecuteCacheRefWithRetry(param.LeagueId, CacheRef.GetLeagueName) + ": "
                    + GetMatchDetail(ExecuteCacheRefWithRetry(param.HomeId, CacheRef.GetTeamName),
                        " vs ", ExecuteCacheRefWithRetry(param.AwayId, CacheRef.GetTeamName),
                        param.SportTypeId, param.ShowTime, param.MatchCode) + "\n"
                    + "Report Type: " + reportType.ToString()
                    + " - Match Code: " + param.MatchCode
                    + " - Status: " + GetStatusById(param.StatusId)
                    + " - Live Status: " + GetLiveStatusById(param.Liveindicator) + "\n"
                    + "Bet Types: " + GetBetypeDisplay(param.Bettypes);
                ws.Cells.GetSubrangeAbsolute(2, 0, 4, colspan).Style = exlStyle.GetStyleText(217, 217, 217, HorizontalAlignmentStyle.Left, true);
                ws.Cells.GetSubrangeAbsolute(2, 0, 4, colspan).Merged = true;


                #endregion Title

                #region Header

                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + 1, 0).Value = GetColumnNameForExcel(reportType, param.StatusId);
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + 1, 0).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 0, indexRow + 1, 0).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 1, indexRow + 1, 1).Value = "Member Turnover";
                ws.Cells.GetSubrangeAbsolute(indexRow, 1, indexRow + 1, 1).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 1, indexRow + 1, 1).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow + 1, 2).Value = "Member Winloss";
                ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow + 1, 2).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 2, indexRow + 1, 2).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 3, indexRow + 1, 3).Value = "Bet Count";
                ws.Cells.GetSubrangeAbsolute(indexRow, 3, indexRow + 1, 3).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 3, indexRow + 1, 3).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 4, indexRow + 1, 4).Value = "House Turnover";
                ws.Cells.GetSubrangeAbsolute(indexRow, 4, indexRow + 1, 4).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 4, indexRow + 1, 4).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 5, indexRow + 1, 5).Value = "House Winloss";
                ws.Cells.GetSubrangeAbsolute(indexRow, 5, indexRow + 1, 5).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 5, indexRow + 1, 5).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 6, indexRow + 1, 6).Value = "SMA Turnover";
                ws.Cells.GetSubrangeAbsolute(indexRow, 6, indexRow + 1, 6).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 6, indexRow + 1, 6).Merged = true;

                ws.Cells.GetSubrangeAbsolute(indexRow, 7, indexRow + 1, 7).Value = "SMA Winloss";
                ws.Cells.GetSubrangeAbsolute(indexRow, 7, indexRow + 1, 7).Style = exlStyle.GetStyleTitle(r, g, b);
                ws.Cells.GetSubrangeAbsolute(indexRow, 7, indexRow + 1, 7).Merged = true;

                if (reportType == ReportType.Member)
                {
                    ws.Cells.GetSubrangeAbsolute(indexRow, 8, indexRow + 1, 8).Value = "Currency";
                    ws.Cells.GetSubrangeAbsolute(indexRow, 8, indexRow + 1, 8).Style = exlStyle.GetStyleTitle(r, g, b);
                    ws.Cells.GetSubrangeAbsolute(indexRow, 8, indexRow + 1, 8).Merged = true;
                }

                #endregion

                indexRow++;
                #region Content

                r = g = b = 255;
                foreach (MatchAnalysis item in match.Items)
                {
                    indexRow++;

                    ws.Cells[indexRow, 0].Value = item.ItemName;
                    ws.Cells[indexRow, 0].Style = exlStyle.GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);

                    ws.Cells[indexRow, 1].Value = CastAide.AsDecimal(item.MemberTO);
                    ws.Cells[indexRow, 1].Style = exlStyle.GetStyleNumber(true, 0, "", r, g, b);

                    ws.Cells[indexRow, 2].Value = CastAide.AsDecimal(item.MemberWL);
                    ws.Cells[indexRow, 2].Style = exlStyle.GetStyleNumber(true, 0, "", r, g, b);

                    ws.Cells[indexRow, 3].Value = CastAide.AsDecimal(item.BetCount.Replace(".00", ""));
                    ws.Cells[indexRow, 3].Style = exlStyle.GetStyleIntNumberPunterAnalysis("", Color.FromArgb(r, g, b), false, false);
                    ws.Cells[indexRow, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                    ws.Cells[indexRow, 4].Value = CastAide.AsDecimal(item.HouseTO);
                    ws.Cells[indexRow, 4].Style = exlStyle.GetStyleNumber(true, 0, "", r, g, b);

                    ws.Cells[indexRow, 5].Value = CastAide.AsDecimal(item.HouseWL);
                    ws.Cells[indexRow, 5].Style = exlStyle.GetStyleNumber(true, CastAide.AsDecimal(item.HouseWL), "", r, g, b);

                    ws.Cells[indexRow, 6].Value = CastAide.AsDecimal(item.SMATO);
                    ws.Cells[indexRow, 6].Style = exlStyle.GetStyleNumber(true, 0, "", r, g, b);

                    ws.Cells[indexRow, 7].Value = CastAide.AsDecimal(item.SMAWL);
                    ws.Cells[indexRow, 7].Style = exlStyle.GetStyleNumber(true, 0, "", r, g, b);


                    if (reportType == ReportType.Member)
                    {
                        ws.Cells[indexRow, 8].Value = item.Currency;
                        ws.Cells[indexRow, 8].Style = exlStyle.GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                    }
                }
                #endregion

                #region Total

                r = 196;
                g = 217;
                b = 238;
                indexRow++;

                ws.Cells[indexRow, 0].Value = "TOTAL";
                ws.Cells[indexRow, 0].Style = exlStyle.GetStyleTextTotal(r, g, b);

                ws.Cells[indexRow, 1].Value = CastAide.AsDecimal(match.Total.MemberTO);
                ws.Cells[indexRow, 1].Style = exlStyle.GetStyleNumberTotal(true, 0, "", r, g, b);

                ws.Cells[indexRow, 2].Value = CastAide.AsDecimal(match.Total.MemberWL);
                ws.Cells[indexRow, 2].Style = exlStyle.GetStyleNumberTotal(true, 0, "", r, g, b);

                ws.Cells[indexRow, 3].Value = CastAide.AsDecimal(match.Total.BetCount.Replace(".00", ""));
                ws.Cells[indexRow, 3].Style = exlStyle.GetStyleIntNumberPunterAnalysis("", Color.FromArgb(r, g, b), true, false);
                ws.Cells[indexRow, 3].Style.HorizontalAlignment = HorizontalAlignmentStyle.Right;

                ws.Cells[indexRow, 4].Value = CastAide.AsDecimal(match.Total.HouseTO);
                ws.Cells[indexRow, 4].Style = exlStyle.GetStyleNumberTotal(true, 0, "", r, g, b);

                ws.Cells[indexRow, 5].Value = CastAide.AsDecimal(match.Total.HouseWL);
                ws.Cells[indexRow, 5].Style = exlStyle.GetStyleNumberTotal(true, CastAide.AsDecimal(match.Total.HouseWL), "", r, g, b);

                ws.Cells[indexRow, 6].Value = CastAide.AsDecimal(match.Total.SMATO);
                ws.Cells[indexRow, 6].Style = exlStyle.GetStyleNumberTotal(true, 0, "", r, g, b);

                ws.Cells[indexRow, 7].Value = CastAide.AsDecimal(match.Total.SMAWL);
                ws.Cells[indexRow, 7].Style = exlStyle.GetStyleNumberTotal(true, 0, "", r, g, b);


                if (reportType == ReportType.Member)
                {
                    ws.Cells[indexRow, 8].Value = "";
                    ws.Cells[indexRow, 8].Style = exlStyle.GetStyleTextTotal(r, g, b);
                }
                #endregion

                for (int i = 0; i <= colspan; i++)
                {
                    ws.Columns[i].Width = 120 * 40;
                }

                #region ResponseFile
                var response = HttpContext.Current.Response;
                response.Clear();
                response.ContentType = "Application/x-msexcel";
                string filename = string.Format("SpecialMatchAnalysis_{0}", DateTime.Today.ToShortDateString());
                filename = filename.Replace("/", "_");
                response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ".xls");
                excelFile.SaveXls(response.OutputStream);
                #endregion ResponseFile
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return excelFile;
        }
        #endregion ExportMatchAnalysis
        #region MatchSelection

        public List<Sporttype> SelLeague(MatchAnalysisParam param)
        {
            var result = new List<Sporttype>();
            try
            {
                var rs = _analysisDao.SelLeague(param);

                if (rs != null && rs.Rows.Count > 0)
                {
                    List<int> sportTypes = rs.AsEnumerable().Select(s => CastAide.AsInt32(s["sporttype"])).Distinct().ToList();
                    foreach (var sportType in sportTypes)
                    {
                        var sportLeagueX = new Sporttype
                        {
                            Id = sportType,
                            Name = ExecuteCacheRefWithRetry(sportType, CacheRef.GetSportName),
                            Leagues = new List<League>()
                        };
                        DataRow[] rows = rs.Select("sporttype='" + sportType + "'");
                        foreach (DataRow row in rows)
                        {
                            var leagueX = new League
                            {
                                Id = CastAide.AsInt32(row["leagueid"]),
                                Name = ExecuteCacheRefWithRetry(CastAide.AsString(row["leagueid"]), CacheRef.GetLeagueName)
                            };
                            sportLeagueX.Leagues.Add(leagueX);
                        }
                        result.Add(sportLeagueX);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public List<League> SelMatchByLeagueids(MatchAnalysisParam param)
        {
            var result = new List<League>();
            if (string.IsNullOrEmpty(param.LeagueIds))
            {
                return result;
            }
            try
            {
                var rs = _analysisDao.SelMatchByLeagueids(param);
                if (rs != null && rs.Rows.Count > 0)
                {
                    List<int> leagueIds = rs.AsEnumerable().Select(s => CastAide.AsInt32(s["leagueid"])).Distinct().ToList();
                    foreach (var leagueId in leagueIds)
                    {
                        var matchLeagueX = new League
                        {
                            Id = leagueId,
                            Name = ExecuteCacheRefWithRetry(leagueId, CacheRef.GetLeagueName),
                            Matches = new List<Match>()
                        };
                        DataRow[] rows = rs.Select("leagueid='" + leagueId + "'");
                        foreach (DataRow row in rows)
                        {
                            var matchX = new Match
                            {
                                Id = CastAide.AsInt32(row["matchid"]),
                                Name = CastAide.AsString(row["matchcode"]),
                                HomeId = CastAide.AsString(row["homeid"]),
                                AwayId = CastAide.AsString(row["awayid"]),
                                HomeTeamName = ExecuteCacheRefWithRetry(CastAide.AsString(row["homeid"]), CacheRef.GetTeamName),
                                AwayTeamName = ExecuteCacheRefWithRetry(CastAide.AsString(row["awayid"]), CacheRef.GetTeamName),

                                ShowTime = CastAide.AsString(row["showtime"]),
                                SportTypeId = CastAide.AsInt32(row["sporttype"]),
                                SportTypeName = ExecuteCacheRefWithRetry(CastAide.AsString(row["sporttype"]), CacheRef.GetSportName),

                            };
                            matchLeagueX.Matches.Add(matchX);
                        }
                        result.Add(matchLeagueX);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        public List<Bettype> SelBettypeByMatchId(MatchAnalysisParam param)
        {
            var result = new List<Bettype>();
            try
            {
                var rs = _analysisDao.SelBettypeByMatchId(param);
                if (rs != null && rs.Rows.Count > 0)
                {
                    foreach (DataRow row in rs.Rows)
                    {
                        var betTypeX = new Bettype
                        {
                            Id = CastAide.AsInt32(row["bettype"]),
                            Name = ExecuteCacheRefWithRetry(CastAide.AsString(row["bettype"]), CacheRef.GetBettype)
                        };
                        result.Add(betTypeX);
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return result;
        }

        private string GetMatchDetail(string homeTeamName, string vsDisplay, string awayTeamName,
            int sportTypeId, string showTime, string matchCode)
        {
            string matchDetail = "";
            if (sportTypeId == Constants.Sport.NumberGame)
            {
                matchDetail = SpecialMatchAnalysisSection.Instance.DisplayText["NumberGamePrefix"].Value + " " + matchCode;
                return matchDetail;
            }

            if (sportTypeId == Constants.Sport.LiveCasino)
            {
                matchDetail = SpecialMatchAnalysisSection.Instance.DisplayText["LiveCasinoPrefix"].Value + " " + showTime;
                return matchDetail;
            }

            if (string.IsNullOrEmpty(homeTeamName) && string.IsNullOrEmpty(awayTeamName))
            {
                matchDetail = "";
            }
            else if (string.IsNullOrEmpty(homeTeamName))
            {
                matchDetail = awayTeamName;
            }
            else if (string.IsNullOrEmpty(awayTeamName))
            {
                matchDetail = homeTeamName;
            }
            else
            {
                matchDetail = homeTeamName + vsDisplay + awayTeamName;
            }
            return matchDetail;
        }

        private static string ExecuteCacheRefWithRetry(object param, Func<object, string> op)
        {
            int retry = MaxRetryNumber;
            string result = "";
            try
            {
                while (retry > 0)
                {
                    result = op(param);
                    if (!string.IsNullOrEmpty(result))
                    {
                        retry = -1;
                        break;
                    }
                    retry--;
                }
            }
            catch (Exception e)
            {
                LogExtensions.ExceptionHandler(e);
            }
            return result;
        }

        #endregion MatchSelection
    }
}
