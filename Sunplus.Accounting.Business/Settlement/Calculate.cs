using System;
using System.Data;
using System.Text;
using System.Collections;
using DNAide.Web;
using Fanex.Tarus.Settlement.Entity;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Settle;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using DNAide.Core;
using Sunplus.Accounting.Core.Extensions;
using IBCUtils;

namespace Sunplus.Accounting.Business.Settlement
{
    /// <summary>
    /// Summary description for Calculate
    /// </summary>
    public static class Calculate
    {
        private static System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
        private static string nl = Environment.NewLine;
        //private static string cURL = "Calculation.aspx?mid=";
        private static string displayMultiple = string.Empty;
        private static string calLink = WebUtils.LoadTemplate("Settle/CalculateLink.htm");
        private static string calLink2 = WebUtils.LoadTemplate("Settle/CalculateLink2.htm");

        public static string GetStatusStr(string _eventstatus)
        {
            switch (_eventstatus.ToLowerInvariant())
            {
                case "running": return "Running";
                case "completed": return "Completed";
                case "closed": return "Closed";
                case "postponed": return "Postponed";
                case "internal": return "Internal";
                default: return _eventstatus;
            }
        }
        private static string GetStringLink(string _params, string _eventstatus, bool _isRefund, bool _isCalRefund, bool _isCalculated, bool _isMultiCaled, bool isHTCal, bool isKeyScore, string permissions, string date = null)
        {
            //string _sLink = (SettleBgColor.HasHostPermission) ? calLink : calLink2;
            string _sLink = calLink;

            string[] arrparams = _params.Split('_');
            TemplateHandler.Assign(ref _sLink, "{params}", arrparams[0]);
            if (!string.IsNullOrWhiteSpace(date)) TemplateHandler.Assign(ref _sLink, "{date}", date);
            TemplateHandler.Assign(ref _sLink, "{params_cls}", arrparams[1]);
            TemplateHandler.Assign(ref _sLink, "{params_id}", _params.Replace("|", "_"));
            // set background color  
            string _bgCalculate = SettleBgColor.DEFAULTCOLOR;
            var bgMultipleCal = SettleBgColor.DEFAULTCOLOR;

            if (_isCalRefund) // refund already
            {
                _bgCalculate = SettleBgColor.REFUND;
            }
            else if (_isCalculated) // bill already
            {
                _bgCalculate = SettleBgColor.BILL;
            }
            else if (isHTCal) // bill already
            {
                _bgCalculate = SettleBgColor.HTCALONLY;
            }

            if (_isRefund)
            {
                bgMultipleCal = SettleBgColor.REFUND;
            }
            else if (_isMultiCaled)
            {
                bgMultipleCal = SettleBgColor.BILL;
            }

            TemplateHandler.Assign(ref _sLink, "{bgCalculate}", _bgCalculate);
            TemplateHandler.Assign(ref _sLink, "{disabled}", isKeyScore ? "" : "disabled=\"disabled\"");
            TemplateHandler.Assign(ref _sLink, "{bgMultiple}", bgMultipleCal);
            string _bgRefundMultiple = SettleBgColor.DEFAULTCOLOR;
            if (string.Compare(_eventstatus, "postponed", true) == 0 && !_isRefund) _bgRefundMultiple = SettleBgColor.REFUNDMULTIPLE;
            TemplateHandler.Assign(ref _sLink, "{bgRefundMulttiple}", _bgRefundMultiple);

            if (permissions.IndexOf("[K]") > -1)
            {
                TemplateHandler.RemoveBlock(ref _sLink, "KeyRead");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref _sLink, "KeyHost");
            }

            if (permissions.IndexOf("[C]") > -1 || permissions.IndexOf("[U]") > -1)
            {
                TemplateHandler.RemoveBlock(ref _sLink, "CalcRead");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref _sLink, "CalcHost");

            }

            if (permissions.IndexOf("[C]") > -1)
            {
                TemplateHandler.RemoveBlock(ref _sLink, "MultRead");
                TemplateHandler.RemoveBlock(ref _sLink, "RefuRead");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref _sLink, "MultHost");
                TemplateHandler.RemoveBlock(ref _sLink, "RefuHost");
            }

            return _sLink;
        }

        private static string GetStringLink_Soccer(string _params, string _eventstatus, bool _isRefund, bool _isCalRefund, bool _isCalculated, bool _isMultiCaled, bool isHTCal, bool isKeyScore, string permissions, string date = null)
        {
            if (isHTCal)
            {
                //string _sLink = (SettleBgColor.HasHostPermission) ? calLink : calLink2;
                string _sLink = calLink;
                string[] arrparams = _params.Split('_');
                TemplateHandler.Assign(ref _sLink, "{params}", arrparams[0]);
                if (!string.IsNullOrWhiteSpace(date)) TemplateHandler.Assign(ref _sLink, "{date}", date);
                TemplateHandler.Assign(ref _sLink, "{params_cls}", arrparams[1]);
                TemplateHandler.Assign(ref _sLink, "{params_id}", _params.Replace("|", "_"));
                TemplateHandler.Assign(ref _sLink, "{disabled}", isKeyScore ? "" : "disabled=\"disabled\"");
                // set background color  
                string _bgCalculate = SettleBgColor.DEFAULTCOLOR;
                var bgMultipleCal = SettleBgColor.DEFAULTCOLOR;

                if (_isCalRefund) // refund already
                {
                    _bgCalculate = SettleBgColor.REFUND;
                }
                else if (_isCalculated) // bill already
                {
                    _bgCalculate = SettleBgColor.BILL;
                }
                else if (isHTCal) // bill already
                {
                    _bgCalculate = SettleBgColor.HTCALONLY;
                }

                if (_isRefund)
                {
                    bgMultipleCal = SettleBgColor.REFUND;
                }
                else if (_isMultiCaled)
                {
                    bgMultipleCal = SettleBgColor.BILL;
                }

                TemplateHandler.Assign(ref _sLink, "{bgCalculate}", _bgCalculate);
                TemplateHandler.Assign(ref _sLink, "{bgMultiple}", bgMultipleCal);
                string _bgRefundMultiple = SettleBgColor.DEFAULTCOLOR;
                if (string.Compare(_eventstatus, "postponed", true) == 0 && !_isRefund) _bgRefundMultiple = SettleBgColor.REFUNDMULTIPLE;
                TemplateHandler.Assign(ref _sLink, "{bgRefundMulttiple}", _bgRefundMultiple);
                if (permissions.IndexOf("[K]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _sLink, "KeyRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _sLink, "KeyHost");
                }


                if (permissions.IndexOf("[C]") > -1 || permissions.IndexOf("[U]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _sLink, "CalcRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _sLink, "CalcHost");

                }

                if (permissions.IndexOf("[C]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _sLink, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sLink, "RefuRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _sLink, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sLink, "RefuHost");
                }

                return _sLink;
            }
            return GetStringLink(_params, _eventstatus, _isRefund, _isCalRefund, _isCalculated, _isMultiCaled, isHTCal, isKeyScore, permissions, date); // Not calculate HT yet
        }
        public static string NumberGame(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate, string permissions)
        {
            try
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainNumberGame.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowNumberGame.htm");
                string _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");


                TemplateHandler.Assign(ref _sMain, "{fdate}", fdate);
                TemplateHandler.Assign(ref _sMain, "{tdate}", tdate);

                return _sMain;
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }

                throw ex;
            }
        }

        public static string CommonSportType(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate, string permissions)
        {
            try
            {
                var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();

                string _sMain = WebUtils.LoadTemplate("Settle/MainCommon.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowCommon.htm");
                string _strLeague = string.Empty;
                if (permissions.IndexOf("[C]") > -1)
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueNameMultiple.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 6).ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                }
                else
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                    TemplateHandler.RemoveBlock(ref _strLeague, "MultHost");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 1).ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");

                }
                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple);
                string curLeagueId = "";
                string bgClass = " rowEven";
                StringBuilder _s = new StringBuilder(); // content
                //------------
                int leagueCount = 0;
                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();
                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();
                    string _time1 = string.Format(cult, "{0:MMddyy}", dr["kickofftime"]);

                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");

                    if (!curLeagueId.Equals(_leagueid))
                    {
                        leagueCount++;
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(dr["leagueid"]));
                        TemplateHandler.Assign(ref _sleague, "{leagueCount}", leagueCount);
                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isundecided"]))
                    {
                        bgClass = "undecided";
                    }
                    TemplateHandler.Assign(ref _sRow, "{confirmht}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"])) ? "-" : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"])) ? "-" : CastAide.AsString(dr["suspenderFT"]));

                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["eventdate"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{hometeam}", CacheRef.GetTeamName(dr["homeid"]) + (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayteam}", CacheRef.GetTeamName(dr["awayid"]));
                    string _time = string.Format(cult, "{0:MM/dd/yy hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeHT}", (!Convert.IsDBNull(dr["hthomescore"])) ? CastAide.AsInt32(dr["hthomescore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{homeFT}", (!Convert.IsDBNull(dr["finalhomescore"])) ? CastAide.AsInt32(dr["finalhomescore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayHT}", (!Convert.IsDBNull(dr["htawayscore"])) ? CastAide.AsInt32(dr["htawayscore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayFT}", (!Convert.IsDBNull(dr["finalawayscore"])) ? CastAide.AsInt32(dr["finalawayscore"]).ToString() : "-");

                    var isPending = false;
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (string.Compare(_eventstatus, "postponed", true) == 0) ? SettleBgColor.POSTPONED : (isPending ? SettleBgColor.PendingStatus : SettleBgColor.DEFAULTCOLOR));
                    bool _isRefund = Convert.ToBoolean(dr["IsRefund"]);
                    bool _isCalRefund = Convert.ToBoolean(dr["IsCalRefund"]);
                    bool _isCalculated = Convert.ToBoolean(dr["IsCalculated"]);
                    bool _isMultiCaled = Convert.ToBoolean(dr["IsMultiCaled"]);
                    bool isHTCal = Convert.ToBoolean(dr["IsHTCal"]);

                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + leagueCount + "_" + dr["matchcode"].ToString();
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink(_params, _eventstatus, _isRefund, _isCalRefund, _isCalculated, _isMultiCaled, isHTCal, CastAide.AsBoolean(dr["iskeyscore"]), permissions, _time));
                    _s.Append(_sRow);
                }

                TemplateHandler.Assign(ref _sMain, "{fdate}", fdate);
                TemplateHandler.Assign(ref _sMain, "{tdate}", tdate);
                TemplateHandler.Assign(ref _sMain, "{content}", _s.ToString());
                return _sMain;
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }

                throw ex;
            }
        }

        public static string Soccer(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate, string permissions)
        {
            try
            {
                var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();

                string _sMain = WebUtils.LoadTemplate("Settle/MainSoccer.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowSoccer.htm");
                TemplateHandler.RemoveBlock(ref _strRow, "betcount");
                TemplateHandler.RemoveBlock(ref _sMain, "betcount");
                string _strLeague = string.Empty;
                if (permissions.IndexOf("[C]") > -1)
                {

                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueNameMultiple.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 5).ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                }
                else
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", totalColumns.ToString());
                    TemplateHandler.RemoveBlock(ref _strLeague, "MultHost");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                }

                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();

                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple);

                string curLeagueId = "";
                string bgClass = "";
                StringBuilder _s = new StringBuilder(); // content
                //------------
                int leagueCount = 0;
                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == "evenrow") ? string.Empty : "evenrow";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();

                    if (!curLeagueId.Equals(_leagueid))
                    {
                        leagueCount++;
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(_leagueid));
                        TemplateHandler.Assign(ref _sleague, "{leagueCount}", leagueCount);
                        bgClass = string.Empty;
                        _s.Append(_sleague);

                    }

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isundecided"]))
                    {
                        bgClass = "undecided";
                    }

                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");

                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["eventdate"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));

                    TemplateHandler.Assign(ref _sRow, "{hometeam}", CacheRef.GetTeamName(dr["homeid"]) + (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayteam}", CacheRef.GetTeamName(dr["awayid"]));
                    string _time = string.Format(cult, "{0:MM/dd/yy hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeHT}", (!Convert.IsDBNull(dr["hthomescore"])) ? CastAide.AsInt32(dr["hthomescore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{homeFT}", (!Convert.IsDBNull(dr["finalhomescore"])) ? CastAide.AsInt32(dr["finalhomescore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayHT}", (!Convert.IsDBNull(dr["htawayscore"])) ? CastAide.AsInt32(dr["htawayscore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayFT}", (!Convert.IsDBNull(dr["finalawayscore"])) ? CastAide.AsInt32(dr["finalawayscore"]).ToString() : "-");

                    TemplateHandler.Assign(ref _sRow, "{confirmht}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"])) ? "-" : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"])) ? "-" : CastAide.AsString(dr["suspenderFT"]));

                    bool _isRefund = Convert.ToBoolean(dr["IsRefund"]);
                    bool _isCalRefund = Convert.ToBoolean(dr["IsCalRefund"]);
                    bool _isFGLG = Convert.ToBoolean(dr["IsFGLG"]);
                    bool _isHTFGLG = Convert.ToBoolean(dr["IsHTFGLG"]);


                    string goalrecord = dr["goalrecord"].ToString();
                    string betstatus = dr["betstatus"].ToString();
                    string finalhomescore = dr["finalhomescore"].ToString();
                    string finalawayscore = dr["finalawayscore"].ToString();
                    string homeIcon = string.Empty;
                    string awayIcon = string.Empty;

                    var match = new Match();
                    match.GoalRecord = goalrecord;
                    match.BetStatus = betstatus;
                    match.FinalHomeScore = finalhomescore;
                    match.FinalAwayScore = finalawayscore;

                    bool isShowRefundHTLG = IsShowRefundLG(match.BetStatus, "HT");
                    bool isShowRefundLG = IsShowRefundLG(match.BetStatus, "FT");

                    var goalHistory = match.GoalHistory;
                    var isMissingSemicolonCase = !string.IsNullOrEmpty(match.GoalRecord) &&
                        match.GoalRecord.IndexOf(";") == -1;
                    if (!isShowRefundLG)
                    {
                        TemplateHandler.Assign(ref _sRow, "{hlgoaldisplay}", "none");
                        TemplateHandler.Assign(ref _sRow, "{algoaldisplay}", "none");
                    }

                    if (_isFGLG)
                    {
                        TemplateHandler.Assign(ref _sRow, "{hfgoaldisplay}", goalHistory.HomeScoreFirstGoal ? "inline" : "none");
                        TemplateHandler.Assign(ref _sRow, "{hlgoaldisplay}", goalHistory.HomeScoreLastGoal ? "inline" : "none");
                        TemplateHandler.Assign(ref _sRow, "{afgoaldisplay}", goalHistory.AwayScoreFirstGoal ? "inline" : "none");
                        TemplateHandler.Assign(ref _sRow, "{algoaldisplay}", goalHistory.AwayScoreLastGoal ? "inline" : "none");
                    }
                    else
                    {
                        TemplateHandler.RemoveBlock(ref _sRow, "FGLG");
                    }

                    if (!isShowRefundHTLG)
                    {
                        TemplateHandler.Assign(ref _sRow, "{1hlgoaldisplay}", "none");
                        TemplateHandler.Assign(ref _sRow, "{1algoaldisplay}", "none");
                    }

                    if (_isHTFGLG)
                    {
                        TemplateHandler.Assign(ref _sRow, "{1hfgoaldisplay}", goalHistory.HomeScoreHTFirstGoal && !isMissingSemicolonCase ? "inline" : "none");
                        TemplateHandler.Assign(ref _sRow, "{1hlgoaldisplay}", goalHistory.HomeScoreHTLastGoal && !isMissingSemicolonCase ? "inline" : "none");
                        TemplateHandler.Assign(ref _sRow, "{1afgoaldisplay}", goalHistory.AwayScoreHTFirstGoal && !isMissingSemicolonCase ? "inline" : "none");
                        TemplateHandler.Assign(ref _sRow, "{1algoaldisplay}", goalHistory.AwayScoreHTLastGoal && !isMissingSemicolonCase ? "inline" : "none");
                    }
                    else
                    {
                        TemplateHandler.RemoveBlock(ref _sRow, "FL");
                    }

                    var isPending = false;
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (string.Compare(_eventstatus, "postponed", true) == 0) ? SettleBgColor.POSTPONED : (isPending ? SettleBgColor.PendingStatus : SettleBgColor.DEFAULTCOLOR));

                    bool _isCalculated = Convert.ToBoolean(dr["IsCalculated"]);
                    bool _isMultiCaled = Convert.ToBoolean(dr["IsMultiCaled"]);
                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + leagueCount + "_" + dr["matchcode"].ToString();
                    bool _isHTCal = CastAide.AsBoolean(dr["ishtcal"], false);
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink_Soccer(_params, _eventstatus, _isRefund, _isCalRefund, _isCalculated, _isMultiCaled, _isHTCal, CastAide.AsBoolean(dr["iskeyscore"]), permissions, _time));

                    _s.Append(_sRow);
                }

                TemplateHandler.Assign(ref _sMain, "{fdate}", fdate);
                TemplateHandler.Assign(ref _sMain, "{tdate}", tdate);
                TemplateHandler.Assign(ref _sMain, "{content}", _s.ToString());
                return _sMain;
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }

        public static string Basketball(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate, string permissions)
        {
            int totalSets = 4; // for Basketball
            var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
            try
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainBasketball.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowBasketball.htm");
                string _strLeague = string.Empty;
                if (permissions.IndexOf("[C]") > -1)
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueNameMultiple.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 5).ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                }
                else
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", totalColumns.ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                    TemplateHandler.RemoveBlock(ref _strLeague, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                }

                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple);

                TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns).ToString());
                string curLeagueId = "";
                string bgClass = " rowEven";
                StringBuilder _s = new StringBuilder(); // content
                //------------
                int leagueCount = 0;
                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();
                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();

                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");

                    if (!curLeagueId.Equals(_leagueid))
                    {
                        leagueCount++;
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(_leagueid));
                        TemplateHandler.Assign(ref _sleague, "{leagueCount}", leagueCount);
                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }
                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isundecided"]))
                    {
                        bgClass = "undecided";
                    }
                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["eventdate"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{homeTeam}", CacheRef.GetTeamName(dr["homeid"]) + (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayTeam}", CacheRef.GetTeamName(dr["awayid"]));
                    bool _isCalculated = Convert.ToBoolean(dr["IsCalculated"]);
                    bool _isMultiCaled = Convert.ToBoolean(dr["IsMultiCaled"]);
                    bool _isRefund = Convert.ToBoolean(dr["IsRefund"]);
                    bool _isCalRefund = Convert.ToBoolean(dr["IsCalRefund"]);
                    bool isHTCal = Convert.ToBoolean(dr["IsHTCal"]);


                    int i = 0;
                    string _homeOT = "-", _awayOT = "-";
                    if (!_isCalRefund && !Convert.IsDBNull(dr["betstatus"]) && !string.IsNullOrEmpty(dr["betstatus"].ToString()))
                    {
                        try
                        {
                            string[] _betStatus = null;

                            if (dr["betstatus"].ToString().IndexOf(';') > -1)
                            {
                                if (dr["betstatus"].ToString().Split(';').Length >= 2)
                                    _betStatus = dr["betstatus"].ToString().Split(';')[1].Split(',');
                            }
                            else
                            {
                                _betStatus = dr["betstatus"].ToString().Split(',');
                            }
                            if (_betStatus != null)
                            {
                                for (; i < _betStatus.Length; i++)
                                {
                                    var _arr = _betStatus[i].Trim().Split('-');
                                    if (_arr.Length >= 2)
                                    {
                                        string _cHome = _arr[0];
                                        string _cAway = _arr[1];
                                        TemplateHandler.Assign(ref _sRow, "{home" + (i + 1).ToString() + "Q}", _cHome);
                                        TemplateHandler.Assign(ref _sRow, "{away" + (i + 1).ToString() + "Q}", _cAway);
                                        if (i == totalSets)
                                        {
                                            _homeOT = _cHome;
                                            _awayOT = _cAway;
                                        }
                                    }
                                }
                            }
                            i--;
                        }
                        catch (Exception ex)
                        {

                            {
                                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                            }
                        }
                    }
                    for (; i < totalSets; i++)
                    {
                        TemplateHandler.Assign(ref _sRow, "{home" + (i + 1).ToString() + "Q}", "-");
                        TemplateHandler.Assign(ref _sRow, "{away" + (i + 1).ToString() + "Q}", "-");
                    }

                    TemplateHandler.Assign(ref _sRow, "{confirmht}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"])) ? "-" : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"])) ? "-" : CastAide.AsString(dr["suspenderFT"]));

                    TemplateHandler.Assign(ref _sRow, "{homeOT}", _homeOT);
                    TemplateHandler.Assign(ref _sRow, "{awayOT}", _awayOT);
                    string _time = string.Format(cult, "{0:MM/dd/yy hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeHT}", (!Convert.IsDBNull(dr["hthomescore"])) ? CastAide.AsInt32(dr["hthomescore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayHT}", (!Convert.IsDBNull(dr["htawayscore"])) ? CastAide.AsInt32(dr["htawayscore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{homeFT}", (!Convert.IsDBNull(dr["finalhomescore"])) ? CastAide.AsInt32(dr["finalhomescore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayFT}", (!Convert.IsDBNull(dr["finalawayscore"])) ? CastAide.AsInt32(dr["finalawayscore"]).ToString() : "-");

                    var isPending = false;
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (isPending ? SettleBgColor.PendingStatus : ""));
                    //string _params = dr["matchid"].ToString() + "|" + sportID.ToString();

                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + leagueCount + "_" + dr["matchcode"].ToString();
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink(_params, _eventstatus, _isRefund, _isCalRefund, _isCalculated, _isMultiCaled, isHTCal, CastAide.AsBoolean(dr["iskeyscore"]), permissions, _time));
                    _s.Append(_sRow);
                }
                TemplateHandler.Assign(ref _sMain, "{fdate}", fdate);
                TemplateHandler.Assign(ref _sMain, "{tdate}", tdate);
                TemplateHandler.Assign(ref _sMain, "{content}", _s.ToString());
                return _sMain;
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }

        public static string Hockey(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate, string permissions)
        {
            int totalSets = 3; // for Hockey
            var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
            try
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainHockey.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowHockey.htm");
                string _strLeague = string.Empty;
                if (permissions.IndexOf("[C]") > -1)
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueNameMultiple.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 6).ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                }
                else
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 1).ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                    TemplateHandler.RemoveBlock(ref _strLeague, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                }

                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple);
                string curLeagueId = "";
                string bgClass = " rowEven";
                StringBuilder _s = new StringBuilder(); // content
                //------------
                int leagueCount = 0;
                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();
                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();
                    string _time1 = string.Format(cult, "{0:MMddyy}", dr["kickofftime"]);
                    if (!curLeagueId.Equals(_leagueid))
                    {
                        leagueCount++;
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(dr["leagueid"]));
                        TemplateHandler.Assign(ref _sleague, "{leagueCount}", leagueCount);

                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isundecided"]))
                    {
                        bgClass = "undecided";
                    }
                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");

                    TemplateHandler.Assign(ref _sRow, "{confirmht}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"])) ? "-" : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"])) ? "-" : CastAide.AsString(dr["suspenderFT"]));

                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["eventdate"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{homeTeam}", CacheRef.GetTeamName(dr["homeid"]) + (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayTeam}", CacheRef.GetTeamName(dr["awayid"]));
                    bool _isCalculated = Convert.ToBoolean(dr["IsCalculated"]);
                    bool _isMultiCaled = Convert.ToBoolean(dr["IsMultiCaled"]);
                    bool _isRefund = Convert.ToBoolean(dr["IsRefund"]);
                    bool _isCalRefund = Convert.ToBoolean(dr["IsCalRefund"]);
                    bool isHTCal = Convert.ToBoolean(dr["IsHTCal"]);

                    int i = 0;
                    string _homeOT = "-", _awayOT = "-";
                    if (!_isCalRefund && !Convert.IsDBNull(dr["betstatus"]) && !string.IsNullOrEmpty(dr["betstatus"].ToString()))
                    {
                        try
                        {

                            string[] _betStatus = null;

                            if (dr["betstatus"].ToString().IndexOf(';') > -1)
                            {
                                if (dr["betstatus"].ToString().Split(';').Length >= 2)
                                    _betStatus = dr["betstatus"].ToString().Split(';')[1].Split(',');
                            }
                            else
                            {
                                _betStatus = dr["betstatus"].ToString().Split(',');
                            }
                            if (_betStatus != null)
                            {
                                for (; i < _betStatus.Length; i++)
                                {
                                    var _arr = _betStatus[i].Trim().Split('-');
                                    if (_arr.Length >= 2)
                                    {
                                        TemplateHandler.Assign(ref _sRow, "{home" + (i + 1).ToString() + "Q}", _arr[0]);
                                        TemplateHandler.Assign(ref _sRow, "{away" + (i + 1).ToString() + "Q}", _arr[1]);
                                        if (i == totalSets)
                                        {
                                            _homeOT = _arr[0];
                                            _awayOT = _arr[1];
                                        }
                                    }
                                }
                            }
                            i--;
                        }
                        catch (Exception ex)
                        {

                            {
                                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                            }
                        }
                    }
                    for (; i < totalSets; i++)
                    {
                        TemplateHandler.Assign(ref _sRow, "{home" + (i + 1).ToString() + "Q}", "-");
                        TemplateHandler.Assign(ref _sRow, "{away" + (i + 1).ToString() + "Q}", "-");
                    }

                    string _time = string.Format(cult, "{0:MM/dd/yy hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeOT}", _homeOT);
                    TemplateHandler.Assign(ref _sRow, "{awayOT}", _awayOT);
                    TemplateHandler.Assign(ref _sRow, "{homeFT}", (!Convert.IsDBNull(dr["finalhomescore"])) ? CastAide.AsInt32(dr["finalhomescore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayFT}", (!Convert.IsDBNull(dr["finalawayscore"])) ? CastAide.AsInt32(dr["finalawayscore"]).ToString() : "-");

                    var isPending = false;
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (isPending ? SettleBgColor.PendingStatus : ""));
                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + leagueCount + "_" + dr["matchcode"].ToString();
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink(_params, _eventstatus, _isRefund, _isCalRefund, _isCalculated, _isMultiCaled, isHTCal, CastAide.AsBoolean(dr["iskeyscore"]), permissions, _time));
                    _s.Append(_sRow);
                }
                TemplateHandler.Assign(ref _sMain, "{fdate}", fdate);
                TemplateHandler.Assign(ref _sMain, "{tdate}", tdate);
                TemplateHandler.Assign(ref _sMain, "{content}", _s.ToString());
                return _sMain;
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }

        public static string Tennis(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate, string permissions)
        {
            int totalSets = 5; // for Tennis
            var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
            try
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainTennis.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowTennis.htm");
                string _strLeague = string.Empty;
                if (permissions.IndexOf("[C]") > -1)
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueNameMultiple.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 6).ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                }
                else
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 1).ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                    TemplateHandler.RemoveBlock(ref _strLeague, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                }
                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple);
                string curLeagueId = "";
                string bgClass = " rowEven";
                StringBuilder _s = new StringBuilder(); // content
                //------------
                int leagueCount = 0;
                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();
                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();
                    string _time1 = string.Format(cult, "{0:MMddyy}", dr["kickofftime"]);
                    if (!curLeagueId.Equals(_leagueid))
                    {
                        leagueCount++;
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(dr["leagueid"]));
                        TemplateHandler.Assign(ref _sleague, "{leagueCount}", leagueCount);
                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isundecided"]))
                    {
                        bgClass = "undecided";
                    }
                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");

                    TemplateHandler.Assign(ref _sRow, "{confirmht}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"])) ? "-" : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"])) ? "-" : CastAide.AsString(dr["suspenderFT"]));

                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["eventdate"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{homeTeam}", CacheRef.GetTeamName(dr["homeid"]) + (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayTeam}", CacheRef.GetTeamName(dr["awayid"]));
                    bool _isCalculated = Convert.ToBoolean(dr["IsCalculated"]);
                    bool _isMultiCaled = Convert.ToBoolean(dr["IsMultiCaled"]);
                    bool _isRefund = Convert.ToBoolean(dr["IsRefund"]);
                    bool _isCalRefund = Convert.ToBoolean(dr["IsCalRefund"]);
                    bool isHTCal = Convert.ToBoolean(dr["IsHTCal"]);

                    int i = 0;
                    int totalGameHome = 0, totalGameAway = 0;
                    if (!_isCalRefund && !Convert.IsDBNull(dr["betstatus"]) && !string.IsNullOrEmpty(dr["betstatus"].ToString()))
                    {
                        try
                        {
                            string[] _betStatus;

                            if (dr["betstatus"].ToString().IndexOf(';') > -1)
                            {
                                _betStatus = dr["betstatus"].ToString().Split(';')[1].Split(',');
                            }
                            else
                            {
                                _betStatus = dr["betstatus"].ToString().Split(',');
                            }
                            for (; i < _betStatus.Length; i++)
                            {
                                var _arr = _betStatus[i].Trim().Split('-');
                                string homeSet = _arr.Length >= 1 ? _betStatus[i].Trim().Split('-')[0] : string.Empty;
                                string awaySet = _arr.Length >= 2 ? _betStatus[i].Trim().Split('-')[1] : string.Empty;
                                if (_arr.Length >= 1)
                                    totalGameHome += Convert.ToInt32(homeSet.Trim());
                                if (_arr.Length >= 2)
                                    totalGameAway += Convert.ToInt32(awaySet.Trim());
                                TemplateHandler.Assign(ref _sRow, "{home" + (i + 1).ToString() + "Q}", homeSet.Trim());
                                TemplateHandler.Assign(ref _sRow, "{away" + (i + 1).ToString() + "Q}", awaySet.Trim());
                            }
                            i--;
                        }
                        catch (Exception ex)
                        {

                            {
                                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                            }
                        }
                    }
                    for (; i < totalSets; i++)
                    {
                        TemplateHandler.Assign(ref _sRow, "{home" + (i + 1).ToString() + "Q}", "-");
                        TemplateHandler.Assign(ref _sRow, "{away" + (i + 1).ToString() + "Q}", "-");
                    }

                    string _time = string.Format(cult, "{0:MM/dd/yy hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeTG}", (totalGameHome > 0) ? totalGameHome.ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayTG}", (totalGameAway > 0) ? totalGameAway.ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{homeTS}", (!Convert.IsDBNull(dr["finalhomescore"])) ? CastAide.AsInt32(dr["finalhomescore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayTS}", (!Convert.IsDBNull(dr["finalawayscore"])) ? CastAide.AsInt32(dr["finalawayscore"]).ToString() : "-");

                    var isPending = false;
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (isPending ? SettleBgColor.PendingStatus : ""));

                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + leagueCount + "_" + dr["matchcode"].ToString();
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink(_params, _eventstatus, _isRefund, _isCalRefund, _isCalculated, _isMultiCaled, isHTCal, CastAide.AsBoolean(dr["iskeyscore"]), permissions, _time));
                    _s.Append(_sRow);
                }
                TemplateHandler.Assign(ref _sMain, "{fdate}", fdate);
                TemplateHandler.Assign(ref _sMain, "{tdate}", tdate);
                TemplateHandler.Assign(ref _sMain, "{content}", _s.ToString());
                return _sMain;
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }

        public static string Billiard(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate, string permissions)
        {
            try
            {
                var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();

                string _sMain = WebUtils.LoadTemplate("Settle/MainBilliard.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowBilliard.htm");
                string _strLeague = string.Empty;
                if (permissions.IndexOf("[C]") > -1)
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueNameMultiple.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 6).ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                }
                else
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 1).ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                    TemplateHandler.RemoveBlock(ref _strLeague, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                }
                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple);
                string curLeagueId = "";
                string bgClass = " rowEven";
                StringBuilder _s = new StringBuilder(); // content
                //------------
                int leagueCount = 0;
                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();
                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();

                    if (!curLeagueId.Equals(_leagueid))
                    {
                        leagueCount++;
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(dr["leagueid"]));
                        TemplateHandler.Assign(ref _sleague, "{leagueCount}", leagueCount);
                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isundecided"]))
                    {
                        bgClass = "undecided";
                    }
                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");

                    TemplateHandler.Assign(ref _sRow, "{confirmht}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"])) ? "-" : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"])) ? "-" : CastAide.AsString(dr["suspenderFT"]));

                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["eventdate"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{homeTeam}", CacheRef.GetTeamName(dr["homeid"]) + (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayTeam}", CacheRef.GetTeamName(dr["awayid"]));
                    bool _isCalculated = Convert.ToBoolean(dr["IsCalculated"]);
                    bool _isMultiCaled = Convert.ToBoolean(dr["IsMultiCaled"]);
                    bool _isRefund = Convert.ToBoolean(dr["IsRefund"]);
                    bool _isCalRefund = Convert.ToBoolean(dr["IsCalRefund"]);
                    bool isHTCal = Convert.ToBoolean(dr["IsHTCal"]);

                    string _time = string.Format(cult, "{0:MM/dd/yy hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeFT}", (!Convert.IsDBNull(dr["finalhomescore"])) ? CastAide.AsInt32(dr["finalhomescore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayFT}", (!Convert.IsDBNull(dr["finalawayscore"])) ? CastAide.AsInt32(dr["finalawayscore"]).ToString() : "-");

                    var isPending = false;
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (isPending ? SettleBgColor.PendingStatus : ""));

                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + leagueCount + "_" + dr["matchcode"].ToString();
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink(_params, _eventstatus, _isRefund, _isCalRefund, _isCalculated, _isMultiCaled, isHTCal, CastAide.AsBoolean(dr["iskeyscore"]), permissions, _time));
                    _s.Append(_sRow);
                }
                TemplateHandler.Assign(ref _sMain, "{fdate}", fdate);
                TemplateHandler.Assign(ref _sMain, "{tdate}", tdate);
                TemplateHandler.Assign(ref _sMain, "{content}", _s.ToString());
                return _sMain;
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }

        public static string Badminton(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate, string permissions)
        {
            int totalSets = 5; // for Tennis
            var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
            try
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainBadminton.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowBadminton.htm");
                string _strLeague = string.Empty;
                if (permissions.IndexOf("[C]") > -1)
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueNameMultiple.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 6).ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                }
                else
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 1).ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                    TemplateHandler.RemoveBlock(ref _strLeague, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                }
                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple);

                string curLeagueId = "";
                string bgClass = " rowEven";
                StringBuilder _s = new StringBuilder(); // content
                //------------
                int leagueCount = 0;
                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();
                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();
                    if (!curLeagueId.Equals(_leagueid))
                    {
                        leagueCount++;
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(dr["leagueid"]));
                        TemplateHandler.Assign(ref _sleague, "{leagueCount}", leagueCount);
                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isundecided"]))
                    {
                        bgClass = "undecided";
                    }
                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");

                    TemplateHandler.Assign(ref _sRow, "{confirmht}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"])) ? "-" : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"])) ? "-" : CastAide.AsString(dr["suspenderFT"]));

                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["eventdate"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{homeTeam}", CacheRef.GetTeamName(dr["homeid"]) + (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayTeam}", CacheRef.GetTeamName(dr["awayid"]));
                    bool _isCalculated = Convert.ToBoolean(dr["IsCalculated"]);
                    bool _isMultiCaled = Convert.ToBoolean(dr["IsMultiCaled"]);
                    bool _isRefund = Convert.ToBoolean(dr["IsRefund"]);
                    bool _isCalRefund = Convert.ToBoolean(dr["IsCalRefund"]);
                    bool isHTCal = Convert.ToBoolean(dr["IsHTCal"]);

                    int i = 0;
                    int totalGameHome = 0, totalGameAway = 0;
                    if (!_isCalRefund && !Convert.IsDBNull(dr["betstatus"]) && !string.IsNullOrEmpty(dr["betstatus"].ToString()))
                    {
                        try
                        {
                            string[] _betStatus = null;

                            if (dr["betstatus"].ToString().IndexOf(';') > -1)
                            {
                                if (dr["betstatus"].ToString().Split(';').Length >= 2)
                                    _betStatus = dr["betstatus"].ToString().Split(';')[1].Split(',');
                            }
                            else
                            {
                                _betStatus = dr["betstatus"].ToString().Split(',');
                            }
                            if (_betStatus != null)
                            {
                                for (; i < _betStatus.Length; i++)
                                {
                                    var _arr = _betStatus[i].Trim().Split('-');
                                    if (_arr.Length >= 2)
                                    {
                                        string homeSet = _arr[0];
                                        string awaySet = _arr[1];
                                        totalGameHome += Convert.ToInt32(homeSet.Trim());
                                        totalGameAway += Convert.ToInt32(awaySet.Trim());
                                        TemplateHandler.Assign(ref _sRow, "{home" + (i + 1).ToString() + "Q}", homeSet.Trim());
                                        TemplateHandler.Assign(ref _sRow, "{away" + (i + 1).ToString() + "Q}", awaySet.Trim());
                                    }
                                }
                            }
                            i--;
                        }
                        catch (Exception ex)
                        {

                            {
                                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                            }
                        }
                    }
                    for (; i < totalSets; i++)
                    {
                        TemplateHandler.Assign(ref _sRow, "{home" + (i + 1).ToString() + "Q}", "-");
                        TemplateHandler.Assign(ref _sRow, "{away" + (i + 1).ToString() + "Q}", "-");
                    }

                    string _time = string.Format(cult, "{0:MM/dd/yy hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeTG}", (totalGameHome > 0) ? totalGameHome.ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayTG}", (totalGameAway > 0) ? totalGameAway.ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{homeTS}", (!Convert.IsDBNull(dr["finalhomescore"])) ? CastAide.AsInt32(dr["finalhomescore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayTS}", (!Convert.IsDBNull(dr["finalawayscore"])) ? CastAide.AsInt32(dr["finalawayscore"]).ToString() : "-");

                    var isPending = false;
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (isPending ? SettleBgColor.PendingStatus : ""));

                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + leagueCount + "_" + dr["matchcode"].ToString();
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink(_params, _eventstatus, _isRefund, _isCalRefund, _isCalculated, _isMultiCaled, isHTCal, CastAide.AsBoolean(dr["iskeyscore"]), permissions, _time));
                    _s.Append(_sRow);
                }
                TemplateHandler.Assign(ref _sMain, "{fdate}", fdate);
                TemplateHandler.Assign(ref _sMain, "{tdate}", tdate);
                TemplateHandler.Assign(ref _sMain, "{content}", _s.ToString());
                return _sMain;
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }

        public static string BaseBall(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate, string permissions)
        {
            int totalSets = 9; // for Hockey
            var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
            try
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainBaseBall.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowBaseBall.htm");
                string _strLeague = string.Empty;
                if (permissions.IndexOf("[C]") > -1)
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueNameMultiple.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 6).ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                }
                else
                {
                    _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 1).ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                    TemplateHandler.RemoveBlock(ref _strLeague, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                }
                TemplateHandler.Assign(ref _sMain, "{displayMul}", "");

                string curLeagueId = "";
                string bgClass = " rowEven";
                StringBuilder _s = new StringBuilder(); // content
                //------------
                int leagueCount = 0;
                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();
                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();

                    if (!curLeagueId.Equals(_leagueid))
                    {
                        leagueCount++;
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(dr["leagueid"]));
                        TemplateHandler.Assign(ref _sleague, "{leagueCount}", leagueCount);
                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isundecided"]))
                    {
                        bgClass = "undecided";
                    }
                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");

                    TemplateHandler.Assign(ref _sRow, "{confirmht}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"])) ? "-" : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}", string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"])) ? "-" : CastAide.AsString(dr["suspenderFT"]));

                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["eventdate"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{homeTeam}", CacheRef.GetTeamName(dr["homeid"]) + (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayTeam}", CacheRef.GetTeamName(dr["awayid"]));
                    bool _isCalculated = Convert.ToBoolean(dr["IsCalculated"]);
                    bool _isMultiCaled = Convert.ToBoolean(dr["IsMultiCaled"]);
                    bool _isRefund = Convert.ToBoolean(dr["IsRefund"]);
                    bool _isCalRefund = Convert.ToBoolean(dr["IsCalRefund"]);
                    bool isHTCal = Convert.ToBoolean(dr["IsHTCal"]);

                    int i = 0;
                    string _homeEI = "-", _awayEI = "-";
                    if (!_isCalRefund && !Convert.IsDBNull(dr["betstatus"]) && !string.IsNullOrEmpty(dr["betstatus"].ToString()))
                    {
                        try
                        {
                            string[] _betStatus = null;

                            if (dr["betstatus"].ToString().IndexOf(';') > -1)
                            {
                                if (dr["betstatus"].ToString().Split(';').Length >= 2)
                                    _betStatus = dr["betstatus"].ToString().Split(';')[1].Split(',');
                            }
                            else
                            {
                                _betStatus = dr["betstatus"].ToString().Split(',');
                            }
                            if (_betStatus != null)
                            {
                                for (; i < _betStatus.Length; i++)
                                {
                                    TemplateHandler.Assign(ref _sRow, "{home" + (i + 1).ToString() + "}", _betStatus[i].Trim().Split('-')[0]);
                                    TemplateHandler.Assign(ref _sRow, "{away" + (i + 1).ToString() + "}", _betStatus[i].Trim().Split('-')[1]);
                                    if (i == totalSets)
                                    {
                                        var _arr = _betStatus[i].Trim().Split('-');
                                        if (_arr.Length >= 2)
                                        {
                                            _homeEI = _arr[0];
                                            _awayEI = _arr[1];
                                        }
                                    }
                                }
                            }
                            i--;
                        }
                        catch (Exception ex)
                        {

                            {
                                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                            }
                        }
                    }
                    for (; i < totalSets; i++)
                    {
                        TemplateHandler.Assign(ref _sRow, "{home" + (i + 1).ToString() + "}", "-");
                        TemplateHandler.Assign(ref _sRow, "{away" + (i + 1).ToString() + "}", "-");
                    }

                    string _time = string.Format(cult, "{0:MM/dd/yy<br />hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeEI}", _homeEI);
                    TemplateHandler.Assign(ref _sRow, "{awayEI}", _awayEI);
                    TemplateHandler.Assign(ref _sRow, "{homeHT}", (!Convert.IsDBNull(dr["hthomescore"])) ? CastAide.AsInt32(dr["hthomescore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayHT}", (!Convert.IsDBNull(dr["htawayscore"])) ? CastAide.AsInt32(dr["htawayscore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{homeFT}", (!Convert.IsDBNull(dr["finalhomescore"])) ? CastAide.AsInt32(dr["finalhomescore"]).ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayFT}", (!Convert.IsDBNull(dr["finalawayscore"])) ? CastAide.AsInt32(dr["finalawayscore"]).ToString() : "-");

                    var isPending = false;
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (isPending ? SettleBgColor.PendingStatus : ""));

                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + leagueCount + "_" + dr["matchcode"].ToString();
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink(_params, _eventstatus, _isRefund, _isCalRefund, _isCalculated, _isMultiCaled, isHTCal, CastAide.AsBoolean(dr["iskeyscore"]), permissions, _time));
                    _s.Append(_sRow);
                }
                TemplateHandler.Assign(ref _sMain, "{fdate}", fdate);
                TemplateHandler.Assign(ref _sMain, "{tdate}", tdate);
                TemplateHandler.Assign(ref _sMain, "{content}", _s.ToString());
                return _sMain;
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }

        #region HorseRacing
        public static string HorseRacing(DataRow[] rows, string fdate, string tdate, string sportID, string permissions)
        {
            var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
            string mainContent = WebUtils.LoadTemplate("Settle/RowHorseRacing.htm");
            string SubContent = "";

            TemplateHandler.Assign(ref mainContent, "{fdate}", fdate);
            TemplateHandler.Assign(ref mainContent, "{tdate}", tdate);

            //Assign Title
            TemplateHandler.Assign(ref mainContent, "{lblRace}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Race"));
            TemplateHandler.Assign(ref mainContent, "{lblResult}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblResult"));
            TemplateHandler.Assign(ref mainContent, "{lblStatus}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Status"));
            TemplateHandler.Assign(ref mainContent, "{lblEventStatus}", "Event Status");
            TemplateHandler.Assign(ref mainContent, "lblWin", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblWin"));
            TemplateHandler.Assign(ref mainContent, "lblPlace", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblPlace"));
            TemplateHandler.Assign(ref mainContent, "lblCalculate", "Calculate");

            string league = TemplateHandler.CopyBlock(mainContent, "league");
            string running = TemplateHandler.CopyBlock(mainContent, "running");
            string running_info = TemplateHandler.CopyBlock(mainContent, "running-info");
            string info1 = TemplateHandler.CopyBlock(mainContent, "info1");
            string info2 = TemplateHandler.CopyBlock(mainContent, "info2");
            TemplateHandler.RemoveBlock(ref mainContent, "league");
            TemplateHandler.RemoveBlock(ref mainContent, "row");

            ArrayList arrleagueid = new ArrayList();
            foreach (DataRow row in rows)
            {
                if (!arrleagueid.Contains(row["leagueid"].ToString()))
                {
                    arrleagueid.Add(row["leagueid"].ToString());
                }
            }
            string _league = "";
            string _running = "";
            string _running_info = "";
            string _info1 = "";
            string _info2 = "";
            int limithorse = 5;


            for (int i = 0; i < arrleagueid.Count; i++) // filter by League
            {
                _league = (string)league.Clone();
                TemplateHandler.Assign(ref _league, "{leagueName}", CacheRef.GetLeagueName(arrleagueid[i]));
                SubContent += _league;

                //Get list match in a league
                ArrayList arrraceid = new ArrayList();
                foreach (DataRow row1 in rows)
                {
                    if (!arrraceid.Contains(row1["leagueid"].ToString() + "|" + row1["matchid"].ToString() + "|" + row1["eventdate"].ToString()) && row1["leagueid"].ToString() == arrleagueid[i].ToString())
                    {
                        arrraceid.Add(row1["leagueid"].ToString() + "|" + row1["matchid"].ToString() + "|" + row1["eventdate"].ToString());
                    }
                }

                int totalhorse = 0;
                //Get list horse

                DataRow[] hl;
                bool even = true;
                for (int j = 0; j < arrraceid.Count; j++) //filter by Race
                {
                    string _params = "'" + arrraceid[j].ToString() + "'";
                    hl = GetListHorseByRace(rows, arrraceid[j].ToString());
                    totalhorse = hl.Length;
                    if (CheckNoInfoRace(hl)) // NO Info Found
                    {
                        #region no info found
                        _running = (string)running.Clone();
                        TemplateHandler.Assign(ref _running, "{raceNumber}", arrraceid[j].ToString().Split('|')[1].ToString());
                        TemplateHandler.Assign(ref _running, "{totalHorses}", "Total horses: " + totalhorse.ToString());
                        TemplateHandler.Assign(ref _running, "{eventStatus}", hl[0]["eventstatus"].ToString());
                        TemplateHandler.Assign(ref _running, "{eventDate}", CastAide.AsDateTimeWithFormat(hl[0]["eventDate"].ToString(), "mm/dd/yyyy").ToShortDateString());
                        TemplateHandler.Assign(ref _running, "{bgEven}", even == true ? "" : "rowEven");

                        //Check Permission
                        if (permissions.IndexOf("[K]") > -1 || permissions.IndexOf("[C]") > -1)
                        {

                            TemplateHandler.RemoveBlock(ref _running, "ReadPermission");
                            TemplateHandler.Assign(ref _running, "{Params}", _params);
                        }
                        else
                        {
                            TemplateHandler.RemoveBlock(ref _running, "HostPermission");
                        }


                        //End check Permission
                        SubContent += _running;
                        #endregion
                    }
                    else
                    {
                        DataRow[] hpl;
                        hpl = GetHorsePlace(hl, limithorse);
                        if (hpl.Length == 0) //no placing
                        {
                            _running_info = (string)running_info.Clone();

                            TemplateHandler.Assign(ref _running_info, "{raceNumber}", arrraceid[j].ToString().Split('|')[1].ToString());
                            TemplateHandler.Assign(ref _running_info, "{totalHorses}", "Total horses: " + totalhorse.ToString());
                            TemplateHandler.Assign(ref _running_info, "{eventStatus}", hl[0]["eventstatus"].ToString());
                            TemplateHandler.Assign(ref _running_info, "{eventDate}", CastAide.AsDateTimeWithFormat(hl[0]["eventDate"].ToString(), "mm/dd/yyyy").ToShortDateString());
                            TemplateHandler.Assign(ref _running_info, "{lblSCR}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSCR") + ": ");
                            TemplateHandler.Assign(ref _running_info, "{lblTF}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTF") + ": ");
                            string SCR_TF = "";
                            SCR_TF = GetScratchAndTFHorses(hl);
                            TemplateHandler.Assign(ref _running_info, "{SCR}", SCR_TF.Split('|')[0].ToString());
                            TemplateHandler.Assign(ref _running_info, "{TF}", SCR_TF.Split('|')[1].ToString());
                            TemplateHandler.Assign(ref _running_info, "{bgEven}", even == true ? "" : "rowEven");
                            TemplateHandler.Assign(ref _running_info, "{bgBill}", (CastAide.AsString(hl[0]["eventstatus"]).Equals(Status.Completed)) ? "bgBill" : "");

                            if (!CastAide.AsString(hl[0]["eventstatus"]).Equals(Status.Completed))
                            {
                                TemplateHandler.RemoveBlock(ref _running_info, "Status");
                            }
                            else
                            {
                                TemplateHandler.Assign(ref _running_info, "{Status}", Status.Official);
                            }


                            //Check Permission

                            if (permissions.IndexOf("[K]") > -1 || permissions.IndexOf("[C]") > -1)
                            {

                                TemplateHandler.RemoveBlock(ref _running_info, "ReadPermission");
                                TemplateHandler.Assign(ref _running_info, "{Params}", _params);
                            }
                            else
                            {
                                TemplateHandler.RemoveBlock(ref _running_info, "HostPermission");
                            }


                            //End check Permission

                            SubContent += _running_info;
                        }
                        else
                        {
                            for (int z = 0; z < hpl.Length; z++)
                            {
                                if (z == 0)
                                {
                                    _info1 = (string)info1.Clone();
                                    TemplateHandler.Assign(ref _info1, "{totalPlace}", hpl.Length.ToString());
                                    TemplateHandler.Assign(ref _info1, "{raceNumber}", arrraceid[j].ToString().Split('|')[1].ToString());
                                    TemplateHandler.Assign(ref _info1, "{placeNumber}", AccCommon.GetPlaceFormat(int.Parse(hpl[z]["betstatus"].ToString())));
                                    TemplateHandler.Assign(ref _info1, "{horseNumber}", hpl[z]["matchcode"].ToString());
                                    TemplateHandler.Assign(ref _info1, "{horseName}", CacheRef.GetTeamName(hpl[z]["homeid"]));
                                    TemplateHandler.Assign(ref _info1, "{winOdd}", int.Parse(hpl[z]["betstatus"].ToString()) == 1 ? FormatHandler.FormatNumber20(hpl[z]["finalhomescore"]) : "");
                                    TemplateHandler.Assign(ref _info1, "{placeOdd}", FormatHandler.FormatNumber20(hpl[z]["finalawayscore"]));
                                    TemplateHandler.Assign(ref _info1, "{Status}", (CastAide.AsString(hpl[z]["eventstatus"]).Equals(Status.Completed)) ? Status.Official : "");
                                    TemplateHandler.Assign(ref _info1, "{eventStatus}", hpl[0]["eventstatus"].ToString());
                                    TemplateHandler.Assign(ref _info1, "{lblSCR}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblSCR") + ": ");
                                    TemplateHandler.Assign(ref _info1, "{lblTF}", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblTF") + ": ");
                                    TemplateHandler.Assign(ref _info1, "{eventDate}", CastAide.AsDateTimeWithFormat(hpl[0]["eventDate"].ToString(), "mm/dd/yyyy").ToShortDateString());
                                    TemplateHandler.Assign(ref _info1, "{bgBill}", (CastAide.AsString(hl[0]["eventstatus"]).Equals(Status.Completed)) ? "bgBill" : "");
                                    string SCR_TF = "";
                                    SCR_TF = GetScratchAndTFHorses(hl);
                                    TemplateHandler.Assign(ref _info1, "{SCR}", SCR_TF.Split('|')[0].ToString());
                                    TemplateHandler.Assign(ref _info1, "{TF}", SCR_TF.Split('|')[1].ToString());
                                    TemplateHandler.Assign(ref _info1, "{bgEven}", even == true ? "" : "rowEven");
                                    if (!CastAide.AsString(hl[0]["eventstatus"]).Equals(Status.Completed))
                                    {
                                        TemplateHandler.RemoveBlock(ref _info1, "Status");
                                    }

                                    //Check Permission
                                    if (permissions.IndexOf("[K]") > -1 || permissions.IndexOf("[C]") > -1)
                                    {

                                        TemplateHandler.RemoveBlock(ref _info1, "ReadPermission");
                                        TemplateHandler.Assign(ref _info1, "{Params}", _params);
                                    }
                                    else
                                    {
                                        TemplateHandler.RemoveBlock(ref _info1, "HostPermission");
                                    }


                                    //End check Permission
                                    SubContent += _info1;
                                }
                                else
                                {
                                    _info2 = (string)info2.Clone();

                                    TemplateHandler.Assign(ref _info2, "{placeNumber}", AccCommon.GetPlaceFormat(int.Parse(hpl[z]["betstatus"].ToString())));
                                    TemplateHandler.Assign(ref _info2, "{horseNumber}", hpl[z]["matchcode"].ToString());
                                    TemplateHandler.Assign(ref _info2, "{horseName}", CacheRef.GetTeamName(hpl[z]["homeid"]));
                                    TemplateHandler.Assign(ref _info2, "{placeOdd}", int.Parse(hpl[z]["betstatus"].ToString()) == 4 ? "" : FormatHandler.FormatNumber20(hpl[z]["finalawayscore"]));
                                    TemplateHandler.Assign(ref _info2, "{bgEven}", even == true ? "" : "rowEven");
                                    TemplateHandler.Assign(ref _info2, "{Params}", _params);
                                    //Check Permission
                                    if (permissions.IndexOf("[K]") > -1 || permissions.IndexOf("[C]") > -1)
                                    {

                                        TemplateHandler.RemoveBlock(ref _info2, "ReadPermission");
                                        TemplateHandler.Assign(ref _info2, "{Params}", _params);
                                    }
                                    else
                                    {
                                        TemplateHandler.RemoveBlock(ref _info2, "HostPermission");
                                    }
                                    SubContent += _info2;
                                }
                            }
                        }

                    }

                    if (even == true)
                    { even = false; }
                    else { even = true; }
                }
            }

            TemplateHandler.Assign(ref mainContent, "{content}", SubContent);
            return mainContent;
        }

        public static DataRow[] GetListHorseByRace(DataRow[] rows, string raceid_eventdate)
        {
            int i = 0;
            DataRow[] horses = new DataRow[0];
            foreach (DataRow horse in rows)
            {
                if (horse["leagueid"].ToString() + "|" + horse["matchid"].ToString() + "|" + horse["eventdate"].ToString() == raceid_eventdate)
                {
                    Array.Resize(ref horses, i + 1);
                    horses[i] = horse;
                    i++;
                }
            }
            return horses;
        }

        public static bool CheckNoInfoRace(DataRow[] rows)
        {
            bool check = true;
            foreach (DataRow row in rows)
            {

                if (CastAide.AsInt32(row["hthomescore"], 0) != 0)
                {
                    check = false;
                    return check;
                }
                if (CastAide.AsInt32(row["htawayscore"], 0) != 0)
                {
                    check = false;
                    return check;
                }
                if (CastAide.AsInt32(row["betstatus"], 0) > 0 && CastAide.AsInt32(row["betstatus"].ToString(), 0) < 5)
                {
                    check = false;
                    return check;
                }
            }
            return check;
        }

        public static DataRow[] GetHorsePlace(DataRow[] rows, int max)
        {
            int i = 0;
            DataRow[] horses = new DataRow[0];
            foreach (DataRow horse in rows)
            {
                if (CastAide.AsInt32(horse["betstatus"].ToString(), 0) > 0 && CastAide.AsInt32(horse["betstatus"].ToString()) < max)
                {
                    Array.Resize(ref horses, i + 1);
                    horses[i] = horse;
                    i++;
                }
            }
            return horses;
        }

        public static DataRow[] OrderHorseList(DataRow[] rows)
        {
            int length;
            length = rows.Length;
            DataRow[] sortedRows = new DataRow[length];
            string[] sortedstr = new string[length];
            for (int i = 0; i < length; i++)
            {
                sortedstr[i] = "0";
            }
            foreach (DataRow row in rows)
            {
                if (CastAide.AsInt32(row["placing"]) < 5)
                {
                    sortedstr[CastAide.AsInt32(row["placing"]) - 1] = row["Horseid"].ToString() + "|1";
                    sortedRows[CastAide.AsInt32(row["placing"]) - 1] = row;
                }
                else
                {
                    for (int j = 0; j < length; j++)
                    {
                        if (sortedstr[j] == "0")
                        {
                            sortedstr[j] = row["Horseid"].ToString() + "|0";
                            sortedRows[j] = row;
                            break;
                        }
                    }
                }
            }
            return sortedRows;
        }

        private static string GetScratchAndTFHorses(DataRow[] rows)
        {
            string strTF = "";
            string strSCR = "";
            //try
            //{
            foreach (DataRow row in rows)
            {
                if (CastAide.AsInt32(row["htawayscore"], 0) == 1)
                    strSCR += CastAide.AsString(row["matchcode"]) + ", ";
                if (CastAide.AsInt32(row["hthomescore"], 0) == 1)
                    strTF += CastAide.AsString(row["matchcode"]) + ", ";


            }
            strSCR = (!string.IsNullOrEmpty(strSCR)) ? strSCR.Substring(0, strSCR.LastIndexOf(",")).Trim() : "";
            strTF = (!string.IsNullOrEmpty(strTF)) ? strTF.Substring(0, strTF.LastIndexOf(",")).Trim() : "";
            //}
            //catch (Exception ex)
            //{
            //    strSCR = "";
            //    strTF = "";
            //}
            return strSCR + "|" + strTF;
        }
        #endregion

        #region ShowGoalHistory
        public static string ShowGoalHistory(string goalrecord)
        {
            string tpl = WebUtils.LoadTemplate("Settle/GoalHistory.htm");
            string data = string.Empty, row = string.Empty;
            row = TemplateHandler.CopyBlock(tpl, "rowsGoalRecord");
            int homeScore = 0, awayScore = 0;
            var records = goalrecord.Split(';');

            if (records[0].Length == 0)
            {
                TemplateHandler.RemoveBlock(ref tpl, "goalRecord");
            }
            else
            {
                for (int i = 0; i < records[0].Length; i++)
                {

                    string tempRow = row;
                    string teamScore = "";
                    if (records[0].Substring(i, 1).ToLower().Equals("h"))
                    {
                        homeScore += 1;
                        teamScore = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblHome");
                    }
                    else
                    {
                        awayScore += 1;
                        teamScore = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAway");
                    }
                    TemplateHandler.Assign(ref tempRow, "score", homeScore.ToString() + " - " + awayScore.ToString());
                    TemplateHandler.Assign(ref tempRow, "team", teamScore);
                    data += tempRow;
                }

                TemplateHandler.ReplaceBlock(ref tpl, "rowsGoalRecord", data);
            }

            if (records.Length > 1)
            {
                data = string.Empty;
                homeScore = 0;
                awayScore = 0;
                row = TemplateHandler.CopyBlock(tpl, "rowsGoalRecordFT");
                var score = string.Concat(records[0], records[1]);

                if (score.Length == 0)
                {
                    TemplateHandler.RemoveBlock(ref tpl, "goalRecordFT");
                }
                else
                {
                    for (int i = 0; i < score.Length; i++)
                    {

                        string tempRow = row;
                        string teamScore = "";
                        if (score.Substring(i, 1).ToLower().Equals("h"))
                        {
                            homeScore += 1;
                            teamScore = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblHome");
                        }
                        else
                        {
                            awayScore += 1;
                            teamScore = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblAway");
                        }
                        TemplateHandler.Assign(ref tempRow, "scoreFT", homeScore.ToString() + " - " + awayScore.ToString());
                        TemplateHandler.Assign(ref tempRow, "teamFT", teamScore);
                        data += tempRow;
                    }
                }

                TemplateHandler.ReplaceBlock(ref tpl, "rowsGoalRecordFT", data);
            }
            else
            {
                TemplateHandler.RemoveBlock(ref tpl, "goalRecordFT");
            }

            if (goalrecord.Length == 0)
            {
                TemplateHandler.Assign(ref tpl, "Hide-GoalHistory", "hide");
                TemplateHandler.Assign(ref tpl, "Hide-GoalHistoryFT", "hide");
            }
            else
            {
                if (records.Length == 1)
                {
                    TemplateHandler.Assign(ref tpl, "Hide-GoalHistory", string.Empty);
                    TemplateHandler.Assign(ref tpl, "Hide-GoalHistoryFT", "hide");
                }
                else
                {
                    TemplateHandler.Assign(ref tpl, "Hide-GoalHistory", string.Empty);
                    TemplateHandler.Assign(ref tpl, "Hide-GoalHistoryFT", string.Empty);
                }
            }

            return tpl;
        }
        #endregion

        private static bool IsShowRefundLG(string betStatus, string timeType)
        {
            var isHalfTime = string.Equals(timeType, "HT", StringComparison.OrdinalIgnoreCase);
            var isFullTime = string.Equals(timeType, "FT", StringComparison.OrdinalIgnoreCase);
            var isShowRefundHTLG = false;
            if (!string.IsNullOrEmpty(betStatus) &&
                (betStatus.Contains("FL:") || betStatus.Contains("1FL:")))
            {
                string[] matchStatus = betStatus.Split(';');
                foreach (var status in matchStatus)
                {
                    if ((isHalfTime && status.Contains("1FL:")) ||
                        (isFullTime && status.Contains("FL:")))
                    {
                        var firstGoalLastGoalInfo = status.Split(':')[1];
                        var firstGoalBet = firstGoalLastGoalInfo.Split('-')[0];
                        var lastGoalBet = firstGoalLastGoalInfo.Split('-')[1];
                        isShowRefundHTLG = lastGoalBet != "0";
                    }
                }
            }

            return isShowRefundHTLG;
        }
    }
}
