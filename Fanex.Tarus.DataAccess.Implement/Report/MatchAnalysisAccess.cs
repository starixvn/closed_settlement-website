
using System;
using System.Data;
using Fanex.Data;
using Fanex.Tarus.DataAccess.Interface.Report;
using Fanex.Tarus.Entities.Report;
using Sunplus.Accounting.Core.Extensions;

namespace Fanex.Tarus.DataAccess.Implement.Report
{
    public class MatchAnalysisAccess : IMatchAnalysisAccess
    {
        public ReportType GetReportType(int reportType)
        {
            ReportType _reportType = ReportType.Currency;

            switch (reportType)
            {
                case 0:
                    _reportType = ReportType.Currency;
                    break;
                case 1:
                    _reportType = ReportType.Bettype;
                    break;
                case 2:
                    _reportType = ReportType.Super;
                    break;
                case 3:
                    _reportType = ReportType.Member;
                    break;
                case 4:
                    _reportType = ReportType.Country;
                    break;
            }
            return _reportType;
        }

        public DataTable SelMatchAnalysisByType(MatchAnalysisParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                string spName = "Acc_Rpt_MatchAnalysisByCurrency";

                switch (GetReportType(param.ReportType))
                {
                    case ReportType.Currency:
                        spName = "Acc_Rpt_MatchAnalysisByCurrency";
                        break;
                    case ReportType.Bettype:
                        spName = "Acc_Rpt_MatchAnalysisByBettype";
                        break;
                    case ReportType.Super:
                        spName = "Acc_Rpt_MatchAnalysisBySuper";
                        break;
                    case ReportType.Member:
                        spName = "Acc_Rpt_MatchAnalysisByMember";
                        break;
                    case ReportType.Country:
                        spName = "Acc_Rpt_MatchAnalysisByCountry";
                        break;
                }

                using (ObjectDb db = new ObjectDb(spName))
                {
                    db.AddInParameter("eventdate", DbType.DateTime, param.EventDate);
                    db.AddInParameter("matchid", DbType.Int32, param.MatchId);
                    if (param.Liveindicator != -1)
                    {
                        db.AddInParameter("liveindicator", DbType.Int32, param.Liveindicator);
                    }
                   
                    if (!string.IsNullOrEmpty(param.Bettypes))
                    {
                        db.AddInParameter("bettypes", DbType.String, param.Bettypes);
                    }
                    
                    db.AddInParameter("statusid", DbType.Int32, param.StatusId);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, param.MoveBetsDay);
                    db.AddInParameter("BalanceUpDay", DbType.DateTime, param.BalanceUpDay);
                    var result = db.ExecuteDataSet();
                    if (result.Tables.Count > 0)
                        dt = result.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }

            return dt;
        }

        public DataTable SelLeague(MatchAnalysisParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_LeagueSel"))
                {
                    db.AddInParameter("eventdate", DbType.DateTime, param.EventDate);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, param.MoveBetsDay);
                    db.AddInParameter("BalanceUpDay", DbType.DateTime, param.BalanceUpDay);
                    var result = db.ExecuteDataSet();
                    if (result.Tables.Count > 0)
                    {
                        dt = result.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }

        public DataTable SelMatchByLeagueids(MatchAnalysisParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_MatchSel"))
                {
                    db.AddInParameter("eventdate", DbType.DateTime, param.EventDate);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, param.MoveBetsDay);
                    db.AddInParameter("BalanceUpDay", DbType.DateTime, param.BalanceUpDay);
                    db.AddInParameter("leagueids", DbType.String, param.LeagueIds);
                    var result = db.ExecuteDataSet();
                    if (result.Tables.Count > 0)
                    {
                        dt = result.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }
        
        public DataTable SelBettypeByMatchId(MatchAnalysisParam param)
        {
            DataTable dt = new DataTable();
            try
            {
                using (ObjectDb db = new ObjectDb("Acc_BettypeSelFromMatch"))
                {
                    db.AddInParameter("eventdate", DbType.DateTime, param.EventDate);
                    db.AddInParameter("MoveBetsDay", DbType.DateTime, param.MoveBetsDay);
                    db.AddInParameter("BalanceUpDay", DbType.DateTime, param.BalanceUpDay);
                    db.AddInParameter("matchid", DbType.Int32, param.MatchId);
                    var result = db.ExecuteDataSet();
                    if (result.Tables.Count > 0)
                        dt = result.Tables[0];
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }
       
    }
}
