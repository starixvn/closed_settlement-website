using System;
using System.Data;
using System.Globalization;
using System.Text;
using DNAide.Core;
using DNAide.Web;

using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Settle;
using Fanex.Tarus.Settlement.Entity;

//using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Business.Settlement
{
    /// <summary>
    /// Summary description for OverOnly
    /// </summary>matkhau
    /// 
    public static class OverOnly
    {
        private static readonly CultureInfo cult = CultureInfo.CurrentCulture;
        private static string nl = Environment.NewLine;
        //private static string cURL = "Calculation.aspx?mid=";
        private static readonly string displayBetCount = string.Empty;
        private static string displayMultiple = " class='hide'";
        private static readonly string calLink = WebUtils.LoadTemplate("Settle/CalculateLink.htm");
        private static string calLink2 = WebUtils.LoadTemplate("Settle/CalculateLink2.htm");

        private static string GetStringLink(string _params, string permissions)
        {
            //_matchid + "&sid=" + sportID.ToString()
            //string _sLink = (SettleBgColor.HasHostPermission) ? calLink : calLink2;

            string _sLink = calLink;

            string[] arrparams = _params.Split('_');
            TemplateHandler.Assign(ref _sLink, "{params}", arrparams[0]);
            TemplateHandler.Assign(ref _sLink, "{params_cls}", arrparams[1] + "_" + arrparams[2]);
            TemplateHandler.Assign(ref _sLink, "{params_id}", _params.Replace("|", "_"));

            // set background color                 
            TemplateHandler.Assign(ref _sLink, "{bgCalculate}", SettleBgColor.DEFAULTCOLOR);
            TemplateHandler.Assign(ref _sLink, "{bgMultiple}", displayMultiple);
            TemplateHandler.Assign(ref _sLink, "{bgRefundMulttiple}", displayMultiple);

            //Check sub-function permission
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

        private static string GetStringLink_Soccer(string _params, bool isHTCal, string permissions)
        {
            if (isHTCal)
            {
                //string _sLink = (SettleBgColor.HasHostPermission) ? calLink : calLink2;
                string _sLink = calLink;

                string[] arrparams = _params.Split('_');
                TemplateHandler.Assign(ref _sLink, "{params}", arrparams[0]);
                TemplateHandler.Assign(ref _sLink, "{params_cls}", arrparams[1] + "_" + arrparams[2]);
                TemplateHandler.Assign(ref _sLink, "{params_id}", _params.Replace("|", "_"));

                TemplateHandler.Assign(ref _sLink, "{bgCalculate}", SettleBgColor.HTCALONLY);
                TemplateHandler.Assign(ref _sLink, "{bgMultiple}", displayMultiple);
                TemplateHandler.Assign(ref _sLink, "{bgRefundMulttiple}", displayMultiple);

                //Check sub-function permissions
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
            return GetStringLink(_params, permissions); // Not calculate HT yet
        }

        public static string CommonSportType(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate,
                                             string permissions)
        {
            try
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainCommon.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowCommon.htm");
                string _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                if (permissions.IndexOf("[C]") > -1)
                {

                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 4).ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                }
                else
                {

                    TemplateHandler.Assign(ref _strLeague, "{colspan}", totalColumns.ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                }

                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple);
                TemplateHandler.Assign(ref _sMain, "{classDisplay}", displayBetCount);
                TemplateHandler.Assign(ref _strRow, "{classDisplay}", displayBetCount);
                string curLeagueId = "";
                string bgClass = " rowEven";
                var _s = new StringBuilder(); // content
                //------------

                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();

                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isUndecided"]))
                    {
                        bgClass = "undecided";
                    }

                    if (!curLeagueId.Equals(_leagueid))
                    {
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(dr["leagueid"]));
                        TemplateHandler.Assign(ref _sleague, "{leagueid}", _leagueid);
                        TemplateHandler.Assign(ref _sleague, "{params}", sportID.ToString());
                        TemplateHandler.Assign(ref _sleague, "{event}", _eventstatus);

                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }

                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");

                    TemplateHandler.Assign(ref _sRow, "{confirmht}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderFT"]));

                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["kickofftime"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{hometeam}",
                                           CacheRef.GetTeamName(dr["homeid"]) +
                                           (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayteam}", CacheRef.GetTeamName(dr["awayid"]));
                    string _time = string.Format(cult, "{0:MM/dd/yy hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeHT}",
                                           (!Convert.IsDBNull(dr["hthomescore"])) ? dr["hthomescore"].ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{homeFT}",
                                           (!Convert.IsDBNull(dr["finalhomescore"])) ? dr["finalhomescore"].ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayHT}",
                                           (!Convert.IsDBNull(dr["htawayscore"])) ? dr["htawayscore"].ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayFT}",
                                           (!Convert.IsDBNull(dr["finalawayscore"])) ? dr["finalawayscore"].ToString() : "-");

                    var isPending = false;
                    var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (string.Compare(_eventstatus, "postponed", true) == 0) ? SettleBgColor.POSTPONED :
                        (isPending ? SettleBgColor.PendingStatus : SettleBgColor.DEFAULTCOLOR));

                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + _leagueid.ToString() + "_" + _eventstatus;
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink(_params, permissions));
                    TemplateHandler.Assign(ref _sRow, "{betcnt}", dr["betcount"].ToString());
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

        public static string Soccer(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate,
                                    string permissions, bool IsHT)
        {
            try
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainSoccer.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowSoccer.htm");
                string _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                if (permissions.IndexOf("[C]") > -1)
                {
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 1).ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                }
                else
                {
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", totalColumns.ToString());
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                }

                if (IsHT)
                {
                    TemplateHandler.RemoveBlock(ref _sMain, "confirmft");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmft");
                }

                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple);
                TemplateHandler.Assign(ref _sMain, "{classDisplay}", displayBetCount);
                TemplateHandler.Assign(ref _strRow, "{classDisplay}", displayBetCount);
                string curLeagueId = "";
                string bgClass = " rowEven";
                var _s = new StringBuilder(); // content
                //------------


                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();

                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();
                    if (!curLeagueId.Equals(_leagueid))
                    {
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(dr["leagueid"]));
                        TemplateHandler.Assign(ref _sleague, "{leagueid}", _leagueid);
                        TemplateHandler.Assign(ref _sleague, "{params}", sportID.ToString());
                        TemplateHandler.Assign(ref _sleague, "{event}", _eventstatus);

                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isUndecided"]))
                    {
                        bgClass = "undecided";
                    }
                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");

                    TemplateHandler.Assign(ref _sRow, "{confirmht}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderHT"]));

                    if (!IsHT)
                    {
                        TemplateHandler.Assign(ref _sRow, "{confirmft}",
                                               string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"]))
                                                   ? "-"
                                                   : CastAide.AsString(dr["suspenderFT"]));
                    }

                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["kickofftime"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{hometeam}",
                                           CacheRef.GetTeamName(dr["homeid"]) +
                                           (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayteam}", CacheRef.GetTeamName(dr["awayid"]));
                    string _time = string.Format(cult, "{0:MM/dd/yy hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeHT}",
                                           (!Convert.IsDBNull(dr["hthomescore"])) ? dr["hthomescore"].ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{homeFT}",
                                           (!Convert.IsDBNull(dr["finalhomescore"])) ? dr["finalhomescore"].ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayHT}",
                                           (!Convert.IsDBNull(dr["htawayscore"])) ? dr["htawayscore"].ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayFT}",
                                           (!Convert.IsDBNull(dr["finalawayscore"])) ? dr["finalawayscore"].ToString() : "-");

                    string goalrecord = dr["goalrecord"].ToString();
                    string betstatus = dr["betstatus"].ToString();
                    var isExistFGLG = Convert.ToBoolean(dr["isExistFGLG"]);
                    string finalhomescore = dr["finalhomescore"].ToString();
                    string finalawayscore = dr["finalawayscore"].ToString();
                    var match = new Match();
                    match.GoalRecord = goalrecord;
                    match.BetStatus = betstatus;
                    match.FinalHomeScore = finalhomescore;
                    match.FinalAwayScore = finalawayscore;
                    var goalHistory = match.GoalHistory;
                    string homeIcon = string.Empty;
                    string awayIcon = string.Empty;
                    if (isExistFGLG)
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


                    TemplateHandler.Assign(ref _sRow, "{fgHome}", string.Empty);
                    TemplateHandler.Assign(ref _sRow, "{fgAway}", string.Empty);
                    TemplateHandler.Assign(ref _sRow, "{lgHome}", string.Empty);
                    TemplateHandler.Assign(ref _sRow, "{lgAway}", string.Empty);

                    var isPending = false;
                    var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (string.Compare(_eventstatus, "postponed", true) == 0) ? SettleBgColor.POSTPONED :
                        (isPending ? SettleBgColor.PendingStatus : SettleBgColor.DEFAULTCOLOR));

                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + _leagueid.ToString() + "_" + _eventstatus;
                    bool _isHTCal = CastAide.AsBoolean(dr["ishtcal"], false);
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink_Soccer(_params, _isHTCal, permissions));
                    TemplateHandler.Assign(ref _sRow, "{betcnt}", dr["betcount"].ToString());
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

        public static string Basketball(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate,
                                        string permissions)
        {
            int totalSets = 4; // for Basketball
            try
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainBasketball.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowBasketball.htm");

                string _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                if (permissions.IndexOf("[C]") > -1)
                {


                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 1).ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                }
                else
                {

                    TemplateHandler.Assign(ref _strLeague, "{colspan}", totalColumns.ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                }

                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple);
                TemplateHandler.Assign(ref _strRow, "{classDisplay}", displayBetCount);

                string curLeagueId = "";
                string bgClass = " rowEven";
                var _s = new StringBuilder(); // content
                //------------

                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();
                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isUndecided"]))
                    {
                        bgClass = "undecided";
                    }
                    if (!curLeagueId.Equals(_leagueid))
                    {
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(dr["leagueid"]));
                        TemplateHandler.Assign(ref _sleague, "{leagueid}", _leagueid);
                        TemplateHandler.Assign(ref _sleague, "{params}", sportID.ToString());
                        TemplateHandler.Assign(ref _sleague, "{event}", _eventstatus);

                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }

                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");

                    TemplateHandler.Assign(ref _sRow, "{confirmht}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderFT"]));
                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["kickofftime"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{homeTeam}",
                                           CacheRef.GetTeamName(dr["homeid"]) +
                                           (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayTeam}", CacheRef.GetTeamName(dr["awayid"]));
                    int i = 0;
                    string _homeOT = "-", _awayOT = "-";
                    if (!Convert.IsDBNull(dr["betstatus"]) && !string.IsNullOrEmpty(dr["betstatus"].ToString()))
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
                            TemplateHandler.Assign(ref _sRow, "{home" + (i + 1) + "Q}", _betStatus[i].Trim().Split('-')[0]);
                            TemplateHandler.Assign(ref _sRow, "{away" + (i + 1) + "Q}", _betStatus[i].Trim().Split('-')[1]);
                            if (i == totalSets)
                            {
                                _homeOT = _betStatus[i].Trim().Split('-')[0];
                                _awayOT = _betStatus[i].Trim().Split('-')[1];
                            }
                        }
                        i--;
                    }
                    for (; i < totalSets; i++)
                    {
                        TemplateHandler.Assign(ref _sRow, "{home" + (i + 1) + "Q}", "-");
                        TemplateHandler.Assign(ref _sRow, "{away" + (i + 1) + "Q}", "-");
                    }

                    TemplateHandler.Assign(ref _sRow, "{homeOT}", _homeOT);
                    TemplateHandler.Assign(ref _sRow, "{awayOT}", _awayOT);
                    string _time = string.Format(cult, "{0:MM/dd/yy hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeHT}",
                                           (!Convert.IsDBNull(dr["hthomescore"])) ? dr["hthomescore"].ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayHT}",
                                           (!Convert.IsDBNull(dr["htawayscore"])) ? dr["htawayscore"].ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{homeFT}",
                                           (!Convert.IsDBNull(dr["finalhomescore"])) ? dr["finalhomescore"].ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayFT}",
                                           (!Convert.IsDBNull(dr["finalawayscore"])) ? dr["finalawayscore"].ToString() : "-");

                    var isPending = false;
                    var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (string.Compare(_eventstatus, "postponed", true) == 0) ? SettleBgColor.POSTPONED :
                        (isPending ? SettleBgColor.PendingStatus : SettleBgColor.DEFAULTCOLOR));

                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + _leagueid.ToString() + "_" + _eventstatus;

                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink(_params, permissions));
                    TemplateHandler.Assign(ref _sRow, "{betcnt}", dr["betcount"].ToString());
                    _s.Append(_sRow);
                }
                TemplateHandler.Assign(ref _sMain, "{classDisplay}", displayBetCount);
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

        public static string Hockey(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate,
                                    string permissions)
        {
            int totalSets = 3; // for Hockey
            try
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainHockey.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowHockey.htm");
                string _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                if (permissions.IndexOf("[C]") > -1)
                {
                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns).ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                }
                else
                {

                    TemplateHandler.Assign(ref _strLeague, "{colspan}", totalColumns.ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                }

                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple);
                TemplateHandler.Assign(ref _sMain, "{classDisplay}", displayBetCount);
                TemplateHandler.Assign(ref _strRow, "{classDisplay}", displayBetCount);
                string curLeagueId = "";
                string bgClass = " rowEven";
                var _s = new StringBuilder(); // content
                //------------

                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();

                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isUndecided"]))
                    {
                        bgClass = "undecided";
                    }
                    if (!curLeagueId.Equals(_leagueid))
                    {
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(dr["leagueid"]));
                        TemplateHandler.Assign(ref _sleague, "{leagueid}", _leagueid);
                        TemplateHandler.Assign(ref _sleague, "{params}", sportID.ToString());
                        TemplateHandler.Assign(ref _sleague, "{event}", _eventstatus);

                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }

                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");

                    TemplateHandler.Assign(ref _sRow, "{confirmht}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderFT"]));

                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["kickofftime"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{homeTeam}",
                                           CacheRef.GetTeamName(dr["homeid"]) +
                                           (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayTeam}", CacheRef.GetTeamName(dr["awayid"]));
                    int i = 0;
                    string _homeOT = "-", _awayOT = "-";
                    if (!Convert.IsDBNull(dr["betstatus"]) && !string.IsNullOrEmpty(dr["betstatus"].ToString()))
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
                                TemplateHandler.Assign(ref _sRow, "{home" + (i + 1) + "Q}",
                                                       _betStatus[i].Trim().Split('-')[0]);
                                TemplateHandler.Assign(ref _sRow, "{away" + (i + 1) + "Q}",
                                                       _betStatus[i].Trim().Split('-')[1]);
                                if (i == totalSets)
                                {
                                    _homeOT = _betStatus[i].Trim().Split('-')[0];
                                    _awayOT = _betStatus[i].Trim().Split('-')[1];
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
                        TemplateHandler.Assign(ref _sRow, "{home" + (i + 1) + "Q}", "-");
                        TemplateHandler.Assign(ref _sRow, "{away" + (i + 1) + "Q}", "-");
                    }

                    string _time = string.Format(cult, "{0:MM/dd/yy hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeOT}", _homeOT);
                    TemplateHandler.Assign(ref _sRow, "{awayOT}", _awayOT);
                    TemplateHandler.Assign(ref _sRow, "{homeFT}",
                                           (!Convert.IsDBNull(dr["finalhomescore"])) ? dr["finalhomescore"].ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayFT}",
                                           (!Convert.IsDBNull(dr["finalawayscore"])) ? dr["finalawayscore"].ToString() : "-");

                    var isPending = false;
                    var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (string.Compare(_eventstatus, "postponed", true) == 0) ? SettleBgColor.POSTPONED :
                        (isPending ? SettleBgColor.PendingStatus : SettleBgColor.DEFAULTCOLOR));

                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + _leagueid.ToString() + "_" + _eventstatus;
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink(_params, permissions));
                    TemplateHandler.Assign(ref _sRow, "{betcnt}", dr["betcount"].ToString());
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

        public static string Tennis(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate,
                                    string permissions)
        {
            int totalSets = 5; // for Tennis
            try
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainTennis.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowTennis.htm");

                string _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                if (permissions.IndexOf("[C]") > -1)
                {


                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 1).ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                }
                else
                {

                    TemplateHandler.Assign(ref _strLeague, "{colspan}", totalColumns.ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                }


                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple);
                TemplateHandler.Assign(ref _sMain, "{classDisplay}", displayBetCount);
                TemplateHandler.Assign(ref _strRow, "{classDisplay}", displayBetCount);
                string curLeagueId = "";
                string bgClass = " rowEven";
                var _s = new StringBuilder(); // content
                //------------

                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();

                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isUndecided"]))
                    {
                        bgClass = "undecided";
                    }
                    if (!curLeagueId.Equals(_leagueid))
                    {
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(dr["leagueid"]));
                        TemplateHandler.Assign(ref _sleague, "{leagueid}", _leagueid);
                        TemplateHandler.Assign(ref _sleague, "{params}", sportID.ToString());
                        TemplateHandler.Assign(ref _sleague, "{event}", _eventstatus);

                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }
                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");

                    TemplateHandler.Assign(ref _sRow, "{confirmht}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderFT"]));

                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["kickofftime"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{homeTeam}",
                                           CacheRef.GetTeamName(dr["homeid"]) +
                                           (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayTeam}", CacheRef.GetTeamName(dr["awayid"]));
                    int i = 0;
                    int totalGameHome = 0, totalGameAway = 0;
                    if (!Convert.IsDBNull(dr["betstatus"]) && !string.IsNullOrEmpty(dr["betstatus"].ToString()))
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
                                string homeSet = _betStatus[i].Trim().Split('-')[0];
                                string awaySet = _betStatus[i].Trim().Split('-')[1];
                                totalGameHome += Convert.ToInt32(homeSet.Trim());
                                totalGameAway += Convert.ToInt32(awaySet.Trim());
                                TemplateHandler.Assign(ref _sRow, "{home" + (i + 1) + "Q}", homeSet.Trim());
                                TemplateHandler.Assign(ref _sRow, "{away" + (i + 1) + "Q}", awaySet.Trim());
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
                        TemplateHandler.Assign(ref _sRow, "{home" + (i + 1) + "Q}", "-");
                        TemplateHandler.Assign(ref _sRow, "{away" + (i + 1) + "Q}", "-");
                    }

                    string _time = string.Format(cult, "{0:MM/dd/yy hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeTG}", (totalGameHome > 0) ? totalGameHome.ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayTG}", (totalGameAway > 0) ? totalGameAway.ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{homeTS}",
                                           (!Convert.IsDBNull(dr["finalhomescore"])) ? dr["finalhomescore"].ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayTS}",
                                           (!Convert.IsDBNull(dr["finalawayscore"])) ? dr["finalawayscore"].ToString() : "-");

                    var isPending = false;
                    var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (string.Compare(_eventstatus, "postponed", true) == 0) ? SettleBgColor.POSTPONED :
                        (isPending ? SettleBgColor.PendingStatus : SettleBgColor.DEFAULTCOLOR));

                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + _leagueid.ToString() + "_" + _eventstatus;
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink(_params, permissions));
                    TemplateHandler.Assign(ref _sRow, "{betcnt}", dr["betcount"].ToString());
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

        public static string Billiard(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate,
                                      string permissions)
        {
            try
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainBilliard.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowBilliard.htm");
                string _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                if (permissions.IndexOf("[C]") > -1)
                {


                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 1).ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                }
                else
                {

                    TemplateHandler.Assign(ref _strLeague, "{colspan}", totalColumns.ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                }
                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple);
                TemplateHandler.Assign(ref _sMain, "{classDisplay}", displayBetCount);
                TemplateHandler.Assign(ref _strRow, "{classDisplay}", displayBetCount);
                string curLeagueId = "";
                string bgClass = " rowEven";
                var _s = new StringBuilder(); // content
                //------------

                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();

                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isUndecided"]))
                    {
                        bgClass = "undecided";
                    }
                    if (!curLeagueId.Equals(_leagueid))
                    {
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(dr["leagueid"]));
                        TemplateHandler.Assign(ref _sleague, "{leagueid}", _leagueid);
                        TemplateHandler.Assign(ref _sleague, "{params}", sportID.ToString());
                        TemplateHandler.Assign(ref _sleague, "{event}", _eventstatus);
                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }
                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");
                    TemplateHandler.Assign(ref _sRow, "{confirmht}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderFT"]));

                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["kickofftime"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{homeTeam}",
                                           CacheRef.GetTeamName(dr["homeid"]) +
                                           (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayTeam}", CacheRef.GetTeamName(dr["awayid"]));
                    string _time = string.Format(cult, "{0:MM/dd/yy hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeFT}",
                                           (!Convert.IsDBNull(dr["finalhomescore"])) ? dr["finalhomescore"].ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayFT}",
                                           (!Convert.IsDBNull(dr["finalawayscore"])) ? dr["finalawayscore"].ToString() : "-");

                    var isPending = false;
                    var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (string.Compare(_eventstatus, "postponed", true) == 0) ? SettleBgColor.POSTPONED :
                        (isPending ? SettleBgColor.PendingStatus : SettleBgColor.DEFAULTCOLOR));


                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + _leagueid.ToString() + "_" + _eventstatus;
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink(_params, permissions));
                    TemplateHandler.Assign(ref _sRow, "{betcnt}", dr["betcount"].ToString());
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

        public static string Badminton(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate,
                                       string permissions)
        {
            int totalSets = 5; // for Tennis
            try
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainBadminton.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowBadminton.htm");
                string _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                if (permissions.IndexOf("[C]") > -1)
                {


                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 1).ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                }
                else
                {

                    TemplateHandler.Assign(ref _strLeague, "{colspan}", totalColumns.ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                }
                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple);
                TemplateHandler.Assign(ref _sMain, "{classDisplay}", displayBetCount);
                TemplateHandler.Assign(ref _strRow, "{classDisplay}", displayBetCount);
                string curLeagueId = "";
                string bgClass = " rowEven";
                var _s = new StringBuilder(); // content
                //------------

                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();

                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isUndecided"]))
                    {
                        bgClass = "undecided";
                    }
                    if (!curLeagueId.Equals(_leagueid))
                    {
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(dr["leagueid"]));
                        TemplateHandler.Assign(ref _sleague, "{leagueid}", _leagueid);
                        TemplateHandler.Assign(ref _sleague, "{params}", sportID.ToString());
                        TemplateHandler.Assign(ref _sleague, "{event}", _eventstatus);
                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }
                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");
                    TemplateHandler.Assign(ref _sRow, "{confirmht}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderFT"]));

                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["kickofftime"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{homeTeam}",
                                           CacheRef.GetTeamName(dr["homeid"]) +
                                           (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayTeam}", CacheRef.GetTeamName(dr["awayid"]));
                    int i = 0;
                    int totalGameHome = 0, totalGameAway = 0;
                    if (!Convert.IsDBNull(dr["betstatus"]) && !string.IsNullOrEmpty(dr["betstatus"].ToString()))
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
                                string homeSet = _betStatus[i].Trim().Split('-')[0];
                                string awaySet = _betStatus[i].Trim().Split('-')[1];
                                totalGameHome += Convert.ToInt32(homeSet.Trim());
                                totalGameAway += Convert.ToInt32(awaySet.Trim());
                                TemplateHandler.Assign(ref _sRow, "{home" + (i + 1) + "Q}", homeSet.Trim());
                                TemplateHandler.Assign(ref _sRow, "{away" + (i + 1) + "Q}", awaySet.Trim());
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
                        TemplateHandler.Assign(ref _sRow, "{home" + (i + 1) + "Q}", "-");
                        TemplateHandler.Assign(ref _sRow, "{away" + (i + 1) + "Q}", "-");
                    }

                    string _time = string.Format(cult, "{0:MM/dd/yy hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeTG}", (totalGameHome > 0) ? totalGameHome.ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayTG}", (totalGameAway > 0) ? totalGameAway.ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{homeTS}",
                                           (!Convert.IsDBNull(dr["finalhomescore"])) ? dr["finalhomescore"].ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayTS}",
                                           (!Convert.IsDBNull(dr["finalawayscore"])) ? dr["finalawayscore"].ToString() : "-");

                    var isPending = false;
                    var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (string.Compare(_eventstatus, "postponed", true) == 0) ? SettleBgColor.POSTPONED :
                        (isPending ? SettleBgColor.PendingStatus : SettleBgColor.DEFAULTCOLOR));

                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + _leagueid.ToString() + "_" + _eventstatus;
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink(_params, permissions));
                    TemplateHandler.Assign(ref _sRow, "{betcnt}", dr["betcount"].ToString());
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

        public static string BaseBall(int sportID, DataRow[] rows, int totalColumns, string fdate, string tdate,
                                      string permissions)
        {
            int totalSets = 9; // for Hockey
            try
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainBaseBall.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowBaseBall.htm");
                string _strLeague = WebUtils.LoadTemplate("Settle/LeagueName.htm");
                if (permissions.IndexOf("[C]") > -1)
                {


                    TemplateHandler.Assign(ref _strLeague, "{colspan}", (totalColumns - 1).ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultRead");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuRead");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendingread");
                }
                else
                {

                    TemplateHandler.Assign(ref _strLeague, "{colspan}", totalColumns.ToString());
                    TemplateHandler.RemoveBlock(ref _sMain, "MultHost");
                    TemplateHandler.RemoveBlock(ref _sMain, "RefuHost");
                    TemplateHandler.RemoveBlock(ref _strRow, "confirmpendinghost");
                }
                TemplateHandler.Assign(ref _sMain, "{displayMul}", displayMultiple); //displayMultiple
                TemplateHandler.Assign(ref _sMain, "{classDisplay}", displayBetCount);
                TemplateHandler.Assign(ref _strRow, "{classDisplay}", displayBetCount);
                string curLeagueId = "";
                string bgClass = " rowEven";
                var _s = new StringBuilder(); // content
                //------------

                var undecided = new Sunplus.Accounting.Business.Settle.Settlement();

                foreach (DataRow dr in rows)
                {
                    bgClass = (bgClass == " rowEven") ? string.Empty : " rowEven";
                    string _sRow = _strRow;
                    string _leagueid = dr["leagueid"].ToString();
                    string _eventstatus = dr["eventstatus"].ToString();

                    undecided.AssignDataUnDecided(ref _sRow, permissions, dr);
                    if (CastAide.AsBoolean(dr["isUndecided"]))
                    {
                        bgClass = "undecided";
                    }
                    if (!curLeagueId.Equals(_leagueid))
                    {
                        curLeagueId = _leagueid;
                        string _sleague = _strLeague;
                        TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(dr["leagueid"]));
                        TemplateHandler.Assign(ref _sleague, "{leagueid}", _leagueid);
                        TemplateHandler.Assign(ref _sleague, "{params}", sportID.ToString());
                        TemplateHandler.Assign(ref _sleague, "{event}", _eventstatus);
                        bgClass = string.Empty;
                        _s.Append(_sleague);
                    }
                    string displayPending = (_eventstatus.ToLower() == "running" || _eventstatus.ToLower() == "closed") ? "" : "disabled='disabled'";
                    TemplateHandler.Assign(ref _sRow, "{displaypending}", displayPending);
                    TemplateHandler.Assign(ref _sRow, "{matchid}", dr["matchid"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{ispending}", CastAide.AsBoolean(dr["ispending"]) ? "checked='true'" : "");
                    TemplateHandler.Assign(ref _sRow, "{confirmht}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderHT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderHT"]));
                    TemplateHandler.Assign(ref _sRow, "{confirmft}",
                                           string.IsNullOrEmpty(CastAide.AsString(dr["suspenderFT"]))
                                               ? "-"
                                               : CastAide.AsString(dr["suspenderFT"]));

                    TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                    TemplateHandler.Assign(ref _sRow, "{code}", dr["matchcode"].ToString());
                    TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", dr["kickofftime"]));
                    TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    TemplateHandler.Assign(ref _sRow, "{homeTeam}",
                                           CacheRef.GetTeamName(dr["homeid"]) +
                                           (CastAide.AsBoolean(dr["isNeutral"]) ? ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "IsNeutral") : string.Empty));
                    TemplateHandler.Assign(ref _sRow, "{awayTeam}", CacheRef.GetTeamName(dr["awayid"]));
                    int i = 0;
                    string _homeEI = "-", _awayEI = "-";
                    if (!Convert.IsDBNull(dr["betstatus"]) && !string.IsNullOrEmpty(dr["betstatus"].ToString()))
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
                                TemplateHandler.Assign(ref _sRow, "{home" + (i + 1) + "}",
                                                       _betStatus[i].Trim().Split('-')[0]);
                                TemplateHandler.Assign(ref _sRow, "{away" + (i + 1) + "}",
                                                       _betStatus[i].Trim().Split('-')[1]);
                                if (i == totalSets)
                                {
                                    _homeEI = _betStatus[i].Trim().Split('-')[0];
                                    _awayEI = _betStatus[i].Trim().Split('-')[1];
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
                        TemplateHandler.Assign(ref _sRow, "{home" + (i + 1) + "}", "-");
                        TemplateHandler.Assign(ref _sRow, "{away" + (i + 1) + "}", "-");
                    }

                    string _time = string.Format(cult, "{0:MM/dd/yy<br />hh:mm:ss tt}", dr["kickofftime"]);
                    TemplateHandler.Assign(ref _sRow, "{date}", _time);
                    TemplateHandler.Assign(ref _sRow, "{homeEI}", _homeEI);
                    TemplateHandler.Assign(ref _sRow, "{awayEI}", _awayEI);
                    TemplateHandler.Assign(ref _sRow, "{homeHT}",
                                           (!Convert.IsDBNull(dr["hthomescore"]))
                                               ? CastAide.AsInt32(dr["hthomescore"]).ToString()
                                               : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayHT}",
                                           (!Convert.IsDBNull(dr["htawayscore"]))
                                               ? CastAide.AsInt32(dr["htawayscore"]).ToString()
                                               : "-");
                    TemplateHandler.Assign(ref _sRow, "{homeFT}",
                                           (!Convert.IsDBNull(dr["finalhomescore"])) ? dr["finalhomescore"].ToString() : "-");
                    TemplateHandler.Assign(ref _sRow, "{awayFT}",
                                           (!Convert.IsDBNull(dr["finalawayscore"])) ? dr["finalawayscore"].ToString() : "-");

                    var isPending = false;
                    var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
                    TemplateHandler.Assign(ref _sRow, "{eventstatus}", settle.SelStatusConfirming(dr, ref isPending));
                    TemplateHandler.Assign(ref _sRow, "{bgEventStatus}", (string.Compare(_eventstatus, "postponed", true) == 0) ? SettleBgColor.POSTPONED :
                        (isPending ? SettleBgColor.PendingStatus : SettleBgColor.DEFAULTCOLOR));


                    string _params = dr["matchid"].ToString() + "|" + sportID.ToString() + "_" + _leagueid.ToString() + "_" + _eventstatus;
                    TemplateHandler.Assign(ref _sRow, "{calLink}", GetStringLink(_params, permissions));
                    TemplateHandler.Assign(ref _sRow, "{betcnt}", dr["betcount"].ToString());
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
    }
}
