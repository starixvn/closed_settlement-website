using System;
using System.Collections;
using System.Data;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using Fanex.Tarus.Settlement.Entity;
using IBCUtils.Web;
using Sunplus.Accounting.Business.Constants;
using Sunplus.Accounting.Business.Settle;
using Sunplus.Accounting.Business.Settlement;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;


public partial class Settle_Calculation : System.Web.UI.Page
{
    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    private string nl = Environment.NewLine;
    //private string type = string.Empty;
    bool isBill = false;
    private int sportID = -1, matchID = -1;
    protected string sportName = "Soccer";
    /* Element
     *      type (bill, keyscore) 
     *      p (matchID | sportID)
     *      calType 
     * */
    int custID = 0;
    string permissions = "";
    public bool IsAlreadyRefundHT = false;
    public string settleDelay = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        string s = string.Empty;
        bool isRefund = false;
        try
        {
            //Get list permissions
            custID = Convert.ToInt32(Session["AccId"] as object ?? -1);
            if (custID == -1) throw new AccException(IBCUtils.IbcError.ACCESS_DENIED_MSG);
            permissions = Sunplus.Accounting.Business.Security.Permissions.CheckHostPermissionSettle(custID);

            if (Request["p"] == null || Request["type"] == null) throw new AccException("Sorry, Invalid session!");
            string type = Request["type"];
            string _params = Request["p"];
            sportID = Convert.ToInt32(_params.Split('|')[1]);
            matchID = Convert.ToInt32(_params.Split('|')[0]);
            //sportName = AccCommon.GetSportType(sportID);
            sportName = CacheRef.GetSportName(sportID);
            if (string.Compare(type, "b", true) != 0 && string.Compare(type, "k", true) != 0 && string.Compare(type, "r", true) != 0)
                throw new AccException("Sorry, Invalid type!");
            if (string.Compare(type, "b", true) == 0) isBill = true;
            if (string.Compare(type, "r", true) == 0) isRefund = true;

            int delaytime = 0;
            s = LoadCalHTML(isRefund, ref IsAlreadyRefundHT, ref delaytime);
            if (type == "b" || type == "r")
            {
                if (delaytime > 0)
                {
                    s += "<script>SettleDelay(" + delaytime.ToString() + ");</script>";
                }
            }
        }
        catch (AccException ex)
        {
            s = "<br />" + AlertMessage.GetMsg(ex.Message.ToString(), AlertMessage.MsgType.Error);
        }
        catch (Exception ex)
        {

        }
        this.sContent.Text = s.Replace(Environment.NewLine, string.Empty).Replace("  ", string.Empty);
    }

    private string LoadCalHTML(bool isRefund, ref bool IsAlreadyRefundHT, ref int delaytime)
    {
        try
        {
            bool IsExistsNG = false;
            bool IsLive = false;
            var outputPrams = new Hashtable();
            var dr = (isBill) ? Settle.SelectMatchForBill(matchID, ref outputPrams) : Settle.SelectMatchForKeyScore(matchID, ref outputPrams);
            bool IsMovebetdaq = false, IsExcludeBetdaq = false, IsSettleAbandonedMatch = false, IsRefundRunningBetDaq = false;
            bool IsMovebetfair = false, IsExcludeBetfair = false, IsSettleAbandonedBetFairMatch = false, IsRefundRunningBetFair = false;

            if (isRefund)
            {
                DbResult rsBetdaq = Settle.BetDaq_Checking(matchID);
                IsMovebetdaq = CastAide.AsBoolean(rsBetdaq.Outputs["ismovebetdaq"]);
                IsExcludeBetdaq = CastAide.AsBoolean(rsBetdaq.Outputs["isexcludebetdaq"]);
                IsSettleAbandonedMatch = CastAide.AsBoolean(rsBetdaq.Outputs["issettleabandonedmatch"]);
                IsRefundRunningBetDaq = CastAide.AsBoolean(rsBetdaq.Outputs["isrefundrunningbetdaq"]);

                DbResult rsBetfair = Settle.BetFair_Checking(matchID);
                IsMovebetfair = CastAide.AsBoolean(rsBetfair.Outputs["ismovebetfair"]);
                IsExcludeBetfair = CastAide.AsBoolean(rsBetfair.Outputs["isexcludebetfair"]);
                IsSettleAbandonedBetFairMatch = CastAide.AsBoolean(rsBetfair.Outputs["issettleabandonedmatchbetfair"]);
                IsRefundRunningBetFair = CastAide.AsBoolean(rsBetfair.Outputs["isrefundrunningbetfair"]);
            }

            if ((int)outputPrams["errcode"] > 0)
                throw new AccException(outputPrams["errmsg"].ToString());
            delaytime = CastAide.AsInt16(outputPrams["delaytime"]);
            if (isBill)
            {
                IsExistsNG = CastAide.AsBoolean(outputPrams["isexistsng"]);
                IsLive = CastAide.AsBoolean(outputPrams["islive"]);
            }

            DataRow r = dr.Tables[0].Rows[0];
            if (string.Compare(r["sporttype"].ToString(), sportID.ToString(), true) != 0)
                throw new AccException("Sorry, Invalid Session!");

            string betstatus = CastAide.AsString(r["betstatus"]);
            IsAlreadyRefundHT = betstatus.ToLower().IndexOf("refund ht") > -1;

            switch (sportID)
            {
                case 1: // soccer
                    if (string.Compare(Request["calType"], "FGLG", true) == 0)
                        return SoccerFGLGHTML(r, Convert.ToBoolean(outputPrams["isexistfglg"]), IsMovebetdaq);
                    var isExistFGLG = Convert.ToBoolean(outputPrams["isexistfglg"]);
                    var isExistLG = Convert.ToBoolean(outputPrams["isexistlg"]);

                    return SoccerHTML(r, isExistFGLG, isExistLG, CastAide.AsBoolean(outputPrams["ishtonly"], false), isRefund, IsExistsNG, IsLive, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq,
                        IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
                case 2: // basketball
                    return BasketBallHTML(r, CastAide.AsBoolean(outputPrams["ishtonly"], false), isRefund, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq,
                        IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
                case 3: // football
                case 26: // rugby
                case 32: // netball
                    return FootbalHTML(r, CastAide.AsBoolean(outputPrams["ishtonly"], false), isRefund, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq,
                        IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
                case 4: //ice hockey
                case 28: //field hockey
                    return HockeyHTML(r, isRefund, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq,
                        IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
                case 5: // tennis
                    return TennisHTML(r, isRefund, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq,
                        IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
                case 10: // golf
                case 11: // Motorsports
                case 7: // billiard 
                case 25: // Darts 
                case 29://Winter Sport
                case 33://     Cycling
                case 34://     Fencing
                case 35://     Judo
                case 36://     Modern Pentathlon
                case 37://     Rowing
                case 38://     Sailing
                case 39://     Shooting
                case 40://     Taekwondo
                case 41://     Triathlon
                case 42://     Wrestling
                case SportConstants.HorseRacingFixedOdds:
                    return BilliardHTML(r, isRefund, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq,
                        IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair); // not finish yet
                case 8: // baseball
                    return BaseballHTML(r, isRefund, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq,
                        IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
                case 9: // badminton
                case 6: // volleyball
                case 30://Squash
                    return BadmintonHTML(r, isRefund, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq,
                        IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
                default: return CommonHTML(r, isRefund, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq,
                        IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
            }

        }
        catch (AccException ex)
        {
            throw ex;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return string.Empty;
        }
    }

    #region CommonHTML
    private string CommonHTML(DataRow r, bool isRefund, bool IsMovebetdaq, bool IsExcludeBetdaq, bool IsSettleAbandonedMatch, bool IsRefundRunningBetDaq,
        bool IsMovebetfair = false, bool IsExcludeBetfair = false, bool IsSettleAbandonedBetFairMatch = false, bool IsRefundRunningBetFair = false)
    {
        try
        {
            string _s = string.Empty;

            if (isRefund)
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/RefundCommon.htm");
                string tplRefund = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/Refund.htm");
                TemplateHandler.RemoveBlock(ref tplRefund, "RefundFT");
                TemplateHandler.RemoveBlock(ref tplRefund, "RefundMHT");
                TemplateHandler.Assign(ref _s, "{Refund}", tplRefund);

                CheckExcludeMoveBetDaq(ref _s, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq);
                CheckExcludeMoveBetFair(ref _s, IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
            }
            else
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/BillCommon.htm");

                TemplateController.RemoveBlock(ref _s, "RefundBlock");
            }

            var _applyHedging = new Settlement();
            _applyHedging.AssignApplyHedgingMatch(ref _s, matchID);


            TemplateController.Assign(ref _s, "{leaguename}", CacheRef.GetLeagueName(r["leagueid"]));
            TemplateController.Assign(ref _s, "{hometeam}", CacheRef.GetTeamName(r["homeid"]) + (CastAide.AsBoolean(r["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty));
            TemplateController.Assign(ref _s, "{awayteam}", CacheRef.GetTeamName(r["awayid"]));

            TemplateHandler.Assign(ref _s, "{matchcode}", ((!Convert.IsDBNull(r["matchcode"])) ? r["matchcode"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{date}", string.Format(cult, "{0:MM/dd/yyyy}", r["eventdate"]));
            TemplateHandler.Assign(ref _s, "{kickoffBetdaq}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateHandler.Assign(ref _s, "{kickoffBetfair}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateHandler.Assign(ref _s, "{homeHScore}", (!Convert.IsDBNull(r["hthomescore"]) ? r["hthomescore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{awayHScore}", (!Convert.IsDBNull(r["htawayscore"]) ? r["htawayscore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{homeFScore}", (!Convert.IsDBNull(r["finalhomescore"]) ? r["finalhomescore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{awayFScore}", (!Convert.IsDBNull(r["finalawayscore"]) ? r["finalawayscore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{type}", (isBill) ? "bill" : "keyscore");
            //TemplateHandler.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            TemplateHandler.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            TemplateHandler.Assign(ref _s, "{sportID}", sportID.ToString());
            TemplateHandler.Assign(ref _s, "{params}", r["matchid"].ToString() + "|" + sportID.ToString());

            if (isBill)
            {
                TemplateHandler.Assign(ref _s, "{btnSubmitValue}", "CALCULATE");
                TemplateHandler.Assign(ref _s, "{disableUS}", ((string.Compare(r["htstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["ftstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["fglgstatus"].ToString(), "settled", true) == 0)) ? string.Empty : "disabled=\"disabled\"");

                if (permissions.IndexOf("[U]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "UnSeRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "UnSeHost");
                }

                if (permissions.IndexOf("[C]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcRead");

                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcHost");

                }
            }
            else
            {
                TemplateHandler.Assign(ref _s, "{btnSubmitValue}", "KEY SCORE");
                TemplateHandler.RemoveBlock(ref _s, "RefundBlock");
                TemplateHandler.RemoveBlock(ref _s, "Unsettle");

                if (permissions.IndexOf("[K]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcRead");

                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcHost");

                }
            }


            return _s;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return string.Empty;
        }
    }
    #endregion

    #region SoccerHTML

    private string SoccerFGLGHTML(DataRow r, bool isExistFGLG, bool IsMovebetdaq)
    {
        try
        {
            string _s = string.Empty;
            _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/BillSoccerFGLG.htm");

            bool _isRefund = Convert.ToBoolean(r["IsRefund"]);
            bool _isCalRefund = Convert.ToBoolean(r["IsCalRefund"]);
            var _applyHedging = new Settlement();
            _applyHedging.AssignApplyHedgingMatch(ref _s, matchID);

            TemplateHandler.Assign(ref _s, "{leaguename}", CacheRef.GetLeagueName(r["leagueid"]));
            TemplateHandler.Assign(ref _s, "{hometeam}", CacheRef.GetTeamName(r["homeid"]) + (CastAide.AsBoolean(r["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty));
            TemplateHandler.Assign(ref _s, "{awayteam}", CacheRef.GetTeamName(r["awayid"]));
            TemplateHandler.Assign(ref _s, "{matchcode}", ((!Convert.IsDBNull(r["matchcode"])) ? r["matchcode"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{date}", string.Format(cult, "{0:MM/dd/yyyy}", r["eventdate"]));
            if (!isExistFGLG)
            {
                TemplateHandler.RemoveBlock(ref _s, "FGLG");
            }
            else
            {
                if (r["betstatus"].ToString().IndexOf("FL:") != -1)
                {
                    try
                    {
                        string[] _betStatus = r["betstatus"].ToString().Split(':');
                        if (!Convert.IsDBNull(r["betstatus"]) && !string.IsNullOrEmpty(r["betstatus"].ToString()))
                        {
                            string _fg = _betStatus[1].Split('-')[0];
                            string _lg = _betStatus[1].Split('-')[1];
                            if (_fg == "1") TemplateHandler.Assign(ref _s, "{fgHomeChecked}", "checked='checked'");
                            if (_fg == "2") TemplateHandler.Assign(ref _s, "{fgAwayChecked}", "checked='checked'");
                            if (_lg == "1") TemplateHandler.Assign(ref _s, "{lgHomeChecked}", "checked='checked'");
                            if (_lg == "2") TemplateHandler.Assign(ref _s, "{lgAwayChecked}", "checked='checked'");
                            if (_fg == "0" && _lg == "0")
                            {
                                TemplateHandler.Assign(ref _s, "{fgHomeChecked}", "disabled='disabled'");
                                TemplateHandler.Assign(ref _s, "{fgAwayChecked}", "disabled='disabled'");
                                TemplateHandler.Assign(ref _s, "{lgHomeChecked}", "disabled='disabled'");
                                TemplateHandler.Assign(ref _s, "{lgAwayChecked}", "disabled='disabled'");
                                TemplateHandler.Assign(ref _s, "{_NoGoalChecked_}", "checked='checked'");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                    }
                }
                TemplateHandler.Assign(ref _s, "{fgHomeChecked}", string.Empty);
                TemplateHandler.Assign(ref _s, "{fgAwayChecked}", string.Empty);
                TemplateHandler.Assign(ref _s, "{lgHomeChecked}", string.Empty);
                TemplateHandler.Assign(ref _s, "{lgAwayChecked}", string.Empty);
            }

            TemplateHandler.Assign(ref _s, "{type}", "calculate");
            //TemplateHandler.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            TemplateHandler.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            TemplateHandler.Assign(ref _s, "{sportID}", sportID.ToString());
            TemplateHandler.Assign(ref _s, "{params}", r["matchid"].ToString() + "|" + sportID.ToString());


            if (permissions.IndexOf("[C]") > -1)
            {
                TemplateHandler.RemoveBlock(ref _s, "CalcRead");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref _s, "CalcHost");
            }

            return _s;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return string.Empty;
        }
    }

    private string SoccerHTML(DataRow r, bool isExistFGLG, bool isExistLG, bool isHalfTimeOnly, bool isRefund, bool IsExistsNG, bool IsLive, bool IsMovebetdaq, bool IsExcludeBetdaq, bool IsSettleAbandonedMatch, bool IsRefundRunningBetDaq,
        bool IsMovebetfair = false, bool IsExcludeBetfair = false, bool IsSettleAbandonedBetFairMatch = false, bool IsRefundRunningBetFair = false)
    {
        //string permissions = "";
        //permissions=Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.CheckHostPermissionSettle(cus

        try
        {
            string _s = string.Empty;
            string htHome = (!Convert.IsDBNull(r["hthomescore"]) ? r["hthomescore"].ToString().Trim() : string.Empty);
            string htAway = (!Convert.IsDBNull(r["htawayscore"]) ? r["htawayscore"].ToString().Trim() : string.Empty);
            string ftHome = (!Convert.IsDBNull(r["finalhomescore"]) ? r["finalhomescore"].ToString().Trim() : string.Empty);
            string ftAway = (!Convert.IsDBNull(r["finalawayscore"]) ? r["finalawayscore"].ToString().Trim() : string.Empty);
            string betstatus = r["betstatus"].ToString();
            string goalrecord = CastAide.AsString(r["goalrecord"]);
            var match = new Match();
            match.BetStatus = betstatus;
            match.GoalRecord = goalrecord;
            match.FinalHomeScore = ftHome;
            match.FinalAwayScore = ftAway;
            var goalHistory = match.GoalHistory;
            var fglgStatus = r["fglgstatus"].ToString().ToLower();
            if (isRefund)
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/RefundSoccer.htm");
                string tplRefund = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/RefundSoccer_btn.htm");


                if (!string.IsNullOrEmpty(betstatus) && betstatus.Contains("Refund HT"))
                {
                    TemplateController.RemoveBlock(ref tplRefund, "RefundFT");
                }

                TemplateHandler.Assign(ref _s, "{hfgoaldisplay}", goalHistory.HomeScoreFirstGoal ? "inline" : "none");
                TemplateHandler.Assign(ref _s, "{hlgoaldisplay}", goalHistory.HomeScoreLastGoal ? "inline" : "none");
                TemplateHandler.Assign(ref _s, "{afgoaldisplay}", goalHistory.AwayScoreFirstGoal ? "inline" : "none");
                TemplateHandler.Assign(ref _s, "{algoaldisplay}", goalHistory.AwayScoreLastGoal ? "inline" : "none");
                TemplateHandler.Assign(ref _s, "{Refund}", tplRefund);

                if (!isExistFGLG && fglgStatus == "no ticket")
                {
                    TemplateController.RemoveBlock(ref _s, "FGLG");
                    // TemplateController.RemoveBlock(ref _s, "RefunFGLG");
                    //TemplateController.RemoveBlock(ref _s, "RefundAllLastGoal");
                }
                CheckExcludeMoveBetDaq(ref _s, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq);
                CheckExcludeMoveBetFair(ref _s, IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
            }
            else
            {

                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/BillSoccer.htm");

                TemplateHandler.RemoveBlock(ref _s, "RefundButton1");
                TemplateHandler.RemoveBlock(ref _s, "RefundButton2");
                TemplateHandler.RemoveBlock(ref _s, "RefundButton3");

                #region GoalHistory
                string type = Request["type"];

                if (type == "b")// only for bill
                {
                    //string goalrecord = CastAide.AsString(r["goalrecord"]);
                    string data = Calculate.ShowGoalHistory(goalrecord);

                    if (string.IsNullOrEmpty(goalrecord)) data = "";


                    TemplateHandler.Assign(ref _s, "{hfgoaldisplay}", goalHistory.HomeScoreFirstGoal ? "inline" : "none");
                    TemplateHandler.Assign(ref _s, "{hlgoaldisplay}", goalHistory.HomeScoreLastGoal ? "inline" : "none");
                    TemplateHandler.Assign(ref _s, "{afgoaldisplay}", goalHistory.AwayScoreFirstGoal ? "inline" : "none");
                    TemplateHandler.Assign(ref _s, "{algoaldisplay}", goalHistory.AwayScoreLastGoal ? "inline" : "none");
                    TemplateHandler.Assign(ref _s, "{statusFGLG}", r["betstatus"]);

                    TemplateHandler.Assign(ref _s, "ShowGoalHistory", data);
                    TemplateHandler.Assign(ref _s, "GoalRecord", goalrecord);
                    TemplateHandler.Assign(ref _s, "IsExistsNG", IsExistsNG);
                    TemplateHandler.Assign(ref _s, "IsLive", IsLive);
                }
                else
                {
                    TemplateHandler.Assign(ref _s, "ShowGoalHistory", string.Empty);
                    TemplateHandler.RemoveBlock(ref _s, "goalrecord");
                }
                if (!isExistFGLG)
                {
                    TemplateController.RemoveBlock(ref _s, "FGLG");
                    TemplateController.RemoveBlock(ref _s, "RefunFGLG");
                    TemplateController.RemoveBlock(ref _s, "RefundAllLastGoal");
                    TemplateHandler.Assign(ref _s, "{disabledGR}", "disabled=\"disabled\"");
                }
                else
                {
                    TemplateHandler.Assign(ref _s, "{disabledGR}", "");
                }

                #endregion
            }

            var _applyHedging = new Settlement();
            _applyHedging.AssignApplyHedgingMatch(ref _s, matchID);

            TemplateHandler.Assign(ref _s, "{leaguename}", CacheRef.GetLeagueName(r["leagueid"]));
            TemplateHandler.Assign(ref _s, "{hometeam}", CacheRef.GetTeamName(r["homeid"]) + (CastAide.AsBoolean(r["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty));
            TemplateHandler.Assign(ref _s, "{awayteam}", CacheRef.GetTeamName(r["awayid"]));
            TemplateHandler.Assign(ref _s, "{matchcode}", ((!Convert.IsDBNull(r["matchcode"])) ? r["matchcode"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{date}", string.Format(cult, "{0:MM/dd/yyyy}", r["eventdate"]));
            TemplateHandler.Assign(ref _s, "{kickoffBetdaq}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateHandler.Assign(ref _s, "{kickoffBetfair}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));

            if (!isExistLG)
                TemplateController.RemoveBlock(ref _s, "RefundAllLastGoal");
            //if (isExistFGLG)
            //{

            //    TemplateHandler.Assign(ref _s, "{disabledGR}", "");
            //}
            //else
            //{

            //    TemplateHandler.Assign(ref _s, "{disabledGR}", "disabled=\"disabled\"");
            //}
            TemplateController.Assign(ref _s, "{hasFGLG}", (isExistFGLG) ? "1" : "0");
            TemplateController.Assign(ref _s, "{homeHScore}", htHome);
            TemplateController.Assign(ref _s, "{awayHScore}", htAway);

            TemplateController.Assign(ref _s, "{params}", r["matchid"].ToString() + "|" + sportID.ToString());
            TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());

            if (isHalfTimeOnly)
            {
                TemplateHandler.Assign(ref _s, "{homeFScore}", "-");
                TemplateHandler.Assign(ref _s, "{awayFScore}", "-");
                TemplateHandler.Assign(ref _s, "{disableFT}", "disabled=\"disabled\"");
                TemplateHandler.Assign(ref _s, "{disableFTCal}", "disabled=\"disabled\"");
            }
            else
            {
                TemplateHandler.Assign(ref _s, "{homeFScore}", ftHome);
                TemplateHandler.Assign(ref _s, "{awayFScore}", ftAway);
                TemplateHandler.Assign(ref _s, "{disableFT}", string.Empty);
            }


            if (isBill)
            {
                TemplateController.RemoveBlock(ref _s, "KeyScore");
                TemplateController.Assign(ref _s, "{HTStatus}", r["htstatus"].ToString());
                TemplateController.Assign(ref _s, "{FTStatus}", r["ftstatus"].ToString());
                TemplateController.Assign(ref _s, "{FGLGStatus}", r["fglgstatus"].ToString());
                TemplateController.Assign(ref _s, "{HTbgcolor}", (string.Compare(r["htstatus"].ToString(), "settled", true) == 0) ? SettleBgColor.BILL : string.Empty);
                TemplateController.Assign(ref _s, "{FTbgcolor}", (string.Compare(r["ftstatus"].ToString(), "settled", true) == 0) ? SettleBgColor.BILL : string.Empty);
                TemplateController.Assign(ref _s, "{FGLGbgcolor}", (string.Compare(r["fglgstatus"].ToString(), "settled", true) == 0) ? SettleBgColor.BILL : string.Empty);
                TemplateController.Assign(ref _s, "{disableFTCal}", (string.Compare(r["htstatus"].ToString(), "running", true) == 0) ? "disabled=\"disabled\"" : string.Empty);
                //TemplateController.Assign(ref _s, "{disableUS}", ((string.Compare(r["htstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["ftstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["fglgstatus"].ToString(), "settled", true) == 0)) ? string.Empty : "disabled=\"disabled\"");
                TemplateController.Assign(ref _s, "{disableUS}", "");
            }
            else
            {
                // TemplateController.RemoveBlock(ref _s, "FGLG");
                TemplateController.RemoveBlock(ref _s, "RefundBlock");
            }

            //Check peemission

            if (permissions.IndexOf("[U]") > -1)
            {
                TemplateHandler.RemoveBlock(ref _s, "UnSeRead");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref _s, "UnSeHost");
            }

            if (permissions.IndexOf("[C]") > -1)
            {
                TemplateController.RemoveBlock(ref _s, "CalcHTRead");
                TemplateController.RemoveBlock(ref _s, "CalcFTRead");
                TemplateController.RemoveBlock(ref _s, "CalcNGRead");
                TemplateController.RemoveBlock(ref _s, "CalcAllRead");
            }
            else
            {

                TemplateController.RemoveBlock(ref _s, "CalcHTHost");
                TemplateController.RemoveBlock(ref _s, "CalcFTHost");
                TemplateController.RemoveBlock(ref _s, "CalcAllHost");
                TemplateController.RemoveBlock(ref _s, "CalcNGHost");
            }



            return _s;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return string.Empty;
        }
    }

    #endregion

    #region BasketBallFootbalHTML
    private string FootbalHTML(DataRow r, bool isHalfTimeOnly, bool isRefund, bool IsMovebetdaq, bool IsExcludeBetdaq, bool IsSettleAbandonedMatch, bool IsRefundRunningBetDaq,
        bool IsMovebetfair = false, bool IsExcludeBetfair = false, bool IsSettleAbandonedBetFairMatch = false, bool IsRefundRunningBetFair = false)
    {
        try
        {
            int totalSets = 4; // for Basketball & footballl

            string _s = string.Empty;

            if (isRefund)
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/RefundBaskballFootbal.htm");
                string tplRefund = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/Refund.htm");
                TemplateHandler.RemoveBlock(ref tplRefund, "RefundMHT");
                TemplateController.Assign(ref _s, "{Refund}", tplRefund);
                CheckExcludeMoveBetDaq(ref _s, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq);
                CheckExcludeMoveBetFair(ref _s, IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
            }
            else
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/BillBaskballFootbal.htm");

                TemplateHandler.RemoveBlock(ref _s, "RefundButton1");
                TemplateHandler.RemoveBlock(ref _s, "RefundButton2");
                TemplateHandler.RemoveBlock(ref _s, "RefundButton3");
            }

            var _applyHedging = new Settlement();
            _applyHedging.AssignApplyHedgingMatch(ref _s, matchID);

            TemplateController.Assign(ref _s, "{leaguename}", CacheRef.GetLeagueName(r["leagueid"]));
            TemplateController.Assign(ref _s, "{hometeam}", CacheRef.GetTeamName(r["homeid"]) + (CastAide.AsBoolean(r["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty));
            TemplateController.Assign(ref _s, "{awayteam}", CacheRef.GetTeamName(r["awayid"]));
            TemplateController.Assign(ref _s, "{matchcode}", ((!Convert.IsDBNull(r["matchcode"])) ? r["matchcode"].ToString() : string.Empty));
            TemplateController.Assign(ref _s, "{date}", string.Format(cult, "{0:MM/dd/yyyy}", r["eventdate"]));
            TemplateHandler.Assign(ref _s, "{kickoffBetdaq}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateHandler.Assign(ref _s, "{kickoffBetfair}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateController.Assign(ref _s, "{IsHTOnly}", ((isHalfTimeOnly) ? "1" : "0"));

            //TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            TemplateController.Assign(ref _s, "{sportID}", sportID.ToString());
            TemplateController.Assign(ref _s, "{params}", r["matchid"].ToString() + "|" + sportID.ToString());


            bool _isRefund = Convert.ToBoolean(r["IsRefund"]);
            bool _isCalRefund = Convert.ToBoolean(r["IsCalRefund"]);
            int i = 0;
            int totalGameHome = 0, totalGameAway = 0;
            string _homeOT = string.Empty, _awayOT = string.Empty;
            if (!_isRefund && !_isCalRefund && !Convert.IsDBNull(r["betstatus"]) && !string.IsNullOrEmpty(r["betstatus"].ToString()))
            {
                try
                {
                    string[] _betStatus;

                    if (r["betstatus"].ToString().IndexOf(';') > -1)
                    {
                        _betStatus = r["betstatus"].ToString().Split(';')[1].Split(',');
                    }
                    else
                    {
                        _betStatus = r["betstatus"].ToString().Split(',');
                    }
                    for (; i < _betStatus.Length; i++)
                    {
                        var arr_betStatus = _betStatus[i].Trim().Split('-');
                        if (arr_betStatus.Length > 1)
                        {
                            string homeSet = _betStatus[i].Trim().Split('-')[0];
                            string awaySet = _betStatus[i].Trim().Split('-')[1];
                            totalGameHome += Convert.ToInt32(homeSet.Trim());
                            totalGameAway += Convert.ToInt32(awaySet.Trim());

                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "qHome}", homeSet.Trim());
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "qAway}", awaySet.Trim());
                        }
                        else
                        {
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "qHome}", string.Empty);
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "qAway}", string.Empty);
                        }
                        if (i == totalSets && arr_betStatus.Length > 1)
                        {
                            _homeOT = _betStatus[i].Trim().Split('-')[0];
                            _awayOT = _betStatus[i].Trim().Split('-')[1];
                        }
                    }
                    i--;
                }
                catch (Exception ex)
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            for (; i < totalSets; i++)
            {
                TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "qHome}", string.Empty);
                TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "qAway}", string.Empty);
            }
            TemplateHandler.Assign(ref _s, "{valueOTHome}", _homeOT);
            TemplateHandler.Assign(ref _s, "{valueOTAway}", _awayOT);
            TemplateHandler.Assign(ref _s, "{homeHScore}", (!Convert.IsDBNull(r["hthomescore"]) ? r["hthomescore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{awayHScore}", (!Convert.IsDBNull(r["htawayscore"]) ? r["htawayscore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{homeFScore}", (!Convert.IsDBNull(r["finalhomescore"]) ? r["finalhomescore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{awayFScore}", (!Convert.IsDBNull(r["finalawayscore"]) ? r["finalawayscore"].ToString() : string.Empty));


            if (isHalfTimeOnly)
            {
                TemplateHandler.Assign(ref _s, "{homeFScore}", "-");
                TemplateHandler.Assign(ref _s, "{awayFScore}", "-");
                TemplateHandler.Assign(ref _s, "{disableFT}", "disabled=\"disabled\"");
                TemplateHandler.Assign(ref _s, "{disableFTCal}", "disabled=\"disabled\"");
            }
            else
            {
                TemplateHandler.Assign(ref _s, "{disableFT}", "");
                TemplateHandler.Assign(ref _s, "{disableFTCal}", "");
            }


            if (isBill)
            {
                TemplateController.RemoveBlock(ref _s, "KeyScore");
                TemplateController.Assign(ref _s, "{HTStatus}", r["htstatus"].ToString());
                TemplateController.Assign(ref _s, "{FTStatus}", r["ftstatus"].ToString());
                TemplateController.Assign(ref _s, "{FGLGStatus}", r["fglgstatus"].ToString());
                TemplateController.Assign(ref _s, "{HTbgcolor}", (string.Compare(r["htstatus"].ToString(), "settled", true) == 0) ? SettleBgColor.BILL : string.Empty);
                TemplateController.Assign(ref _s, "{FTbgcolor}", (string.Compare(r["ftstatus"].ToString(), "settled", true) == 0) ? SettleBgColor.BILL : string.Empty);
                TemplateController.Assign(ref _s, "{FGLGbgcolor}", (string.Compare(r["fglgstatus"].ToString(), "settled", true) == 0) ? SettleBgColor.BILL : string.Empty);
                TemplateController.Assign(ref _s, "{disableFTCal}", (string.Compare(r["htstatus"].ToString(), "running", true) == 0) ? "disabled=\"disabled\"" : string.Empty);
                TemplateController.Assign(ref _s, "{disableUS}", ((string.Compare(r["htstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["ftstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["fglgstatus"].ToString(), "settled", true) == 0)) ? string.Empty : "disabled=\"disabled\"");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref _s, "RefundBlock");
            }

            if (permissions.IndexOf("[U]") > -1)
            {
                TemplateHandler.RemoveBlock(ref _s, "UnSeRead");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref _s, "UnSeHost");
            }

            if (permissions.IndexOf("[C]") > -1)
            {
                TemplateHandler.RemoveBlock(ref _s, "CalcHTRead");
                TemplateHandler.RemoveBlock(ref _s, "CalcFTRead");
                TemplateHandler.RemoveBlock(ref _s, "CalcAllRead");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref _s, "CalcHTHost");
                TemplateHandler.RemoveBlock(ref _s, "CalcFTHost");
                TemplateHandler.RemoveBlock(ref _s, "CalcAllHost");
            }

            return _s;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return string.Empty;
        }
    }

    private string BasketBallHTML(DataRow r, bool isHalfTimeOnly, bool isRefund, bool IsMovebetdaq, bool IsExcludeBetdaq, bool IsSettleAbandonedMatch, bool IsRefundRunningBetDaq,
        bool IsMovebetfair = false, bool IsExcludeBetfair = false, bool IsSettleAbandonedBetFairMatch = false, bool IsRefundRunningBetFair = false)
    {
        try
        {
            int totalSets = 4; // for Basketball & footballl

            string _s = string.Empty;

            if (isRefund)
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/RefundBaskballFootbal.htm");
                string tplRefund = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/Refund.htm");

                if (!r.IsNull("betstatus") && r["betstatus"].ToString().Contains("Refund HT"))
                {
                    TemplateController.RemoveBlock(ref tplRefund, "RefundFT");
                }

                TemplateController.Assign(ref _s, "{Refund}", tplRefund);

                CheckExcludeMoveBetDaq(ref _s, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq);
                CheckExcludeMoveBetFair(ref _s, IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
            }
            else
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/BillBaskballFootbal.htm");

                TemplateHandler.RemoveBlock(ref _s, "RefundButton1");
                TemplateHandler.RemoveBlock(ref _s, "RefundButton2");
                TemplateHandler.RemoveBlock(ref _s, "RefundButton3");
            }

            var _applyHedging = new Settlement();
            _applyHedging.AssignApplyHedgingMatch(ref _s, matchID);

            TemplateController.Assign(ref _s, "{leaguename}", CacheRef.GetLeagueName(r["leagueid"]));
            TemplateController.Assign(ref _s, "{hometeam}", CacheRef.GetTeamName(r["homeid"]) + (CastAide.AsBoolean(r["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty));
            TemplateController.Assign(ref _s, "{awayteam}", CacheRef.GetTeamName(r["awayid"]));
            TemplateController.Assign(ref _s, "{matchcode}", ((!Convert.IsDBNull(r["matchcode"])) ? r["matchcode"].ToString() : string.Empty));
            TemplateController.Assign(ref _s, "{date}", string.Format(cult, "{0:MM/dd/yyyy}", r["eventdate"]));
            TemplateHandler.Assign(ref _s, "{kickoffBetdaq}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateHandler.Assign(ref _s, "{kickoffBetfair}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateController.Assign(ref _s, "{IsHTOnly}", ((isHalfTimeOnly) ? "1" : "0"));

            //TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            TemplateController.Assign(ref _s, "{sportID}", sportID.ToString());
            TemplateController.Assign(ref _s, "{params}", r["matchid"].ToString() + "|" + sportID.ToString());


            bool _isRefund = Convert.ToBoolean(r["IsRefund"]);
            bool _isCalRefund = Convert.ToBoolean(r["IsCalRefund"]);
            int i = 0;
            int totalGameHome = 0, totalGameAway = 0;
            string _homeOT = string.Empty, _awayOT = string.Empty;
            if (!_isRefund && !_isCalRefund && !Convert.IsDBNull(r["betstatus"]) && !string.IsNullOrEmpty(r["betstatus"].ToString()))
            {
                try
                {
                    string[] _betStatus;

                    if (r["betstatus"].ToString().IndexOf(';') > -1)
                    {
                        _betStatus = r["betstatus"].ToString().Split(';')[1].Split(',');
                    }
                    else
                    {
                        _betStatus = r["betstatus"].ToString().Split(',');
                    }
                    for (; i < _betStatus.Length; i++)
                    {
                        var arr_betStatus = _betStatus[i].Trim().Split('-');
                        if (arr_betStatus.Length > 1)
                        {
                            string homeSet = _betStatus[i].Trim().Split('-')[0];
                            string awaySet = _betStatus[i].Trim().Split('-')[1];
                            totalGameHome += Convert.ToInt32(homeSet.Trim());
                            totalGameAway += Convert.ToInt32(awaySet.Trim());

                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "qHome}", homeSet.Trim());
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "qAway}", awaySet.Trim());
                        }
                        else
                        {
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "qHome}", string.Empty);
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "qAway}", string.Empty);
                        }
                        if (i == totalSets && arr_betStatus.Length > 1)
                        {
                            _homeOT = _betStatus[i].Trim().Split('-')[0];
                            _awayOT = _betStatus[i].Trim().Split('-')[1];
                        }
                    }
                    i--;
                }
                catch (Exception ex)
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            for (; i < totalSets; i++)
            {
                TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "qHome}", string.Empty);
                TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "qAway}", string.Empty);
            }
            TemplateHandler.Assign(ref _s, "{valueOTHome}", _homeOT);
            TemplateHandler.Assign(ref _s, "{valueOTAway}", _awayOT);
            TemplateHandler.Assign(ref _s, "{homeHScore}", (!Convert.IsDBNull(r["hthomescore"]) ? r["hthomescore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{awayHScore}", (!Convert.IsDBNull(r["htawayscore"]) ? r["htawayscore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{homeFScore}", (!Convert.IsDBNull(r["finalhomescore"]) ? r["finalhomescore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{awayFScore}", (!Convert.IsDBNull(r["finalawayscore"]) ? r["finalawayscore"].ToString() : string.Empty));


            if (isHalfTimeOnly)
            {
                TemplateHandler.Assign(ref _s, "{homeFScore}", "-");
                TemplateHandler.Assign(ref _s, "{awayFScore}", "-");
                TemplateHandler.Assign(ref _s, "{disableFT}", "disabled=\"disabled\"");
                TemplateHandler.Assign(ref _s, "{disableFTCal}", "disabled=\"disabled\"");
            }
            else
            {
                TemplateHandler.Assign(ref _s, "{disableFT}", "");
                TemplateHandler.Assign(ref _s, "{disableFTCal}", "");
            }


            if (isBill)
            {
                TemplateController.RemoveBlock(ref _s, "KeyScore");
                TemplateController.Assign(ref _s, "{HTStatus}", r["htstatus"].ToString());
                TemplateController.Assign(ref _s, "{FTStatus}", r["ftstatus"].ToString());
                TemplateController.Assign(ref _s, "{FGLGStatus}", r["fglgstatus"].ToString());
                TemplateController.Assign(ref _s, "{HTbgcolor}", (string.Compare(r["htstatus"].ToString(), "settled", true) == 0) ? SettleBgColor.BILL : string.Empty);
                TemplateController.Assign(ref _s, "{FTbgcolor}", (string.Compare(r["ftstatus"].ToString(), "settled", true) == 0) ? SettleBgColor.BILL : string.Empty);
                TemplateController.Assign(ref _s, "{FGLGbgcolor}", (string.Compare(r["fglgstatus"].ToString(), "settled", true) == 0) ? SettleBgColor.BILL : string.Empty);
                TemplateController.Assign(ref _s, "{disableFTCal}", (string.Compare(r["htstatus"].ToString(), "running", true) == 0) ? "disabled=\"disabled\"" : string.Empty);
                TemplateController.Assign(ref _s, "{disableUS}", ((string.Compare(r["htstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["ftstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["fglgstatus"].ToString(), "settled", true) == 0)) ? string.Empty : "disabled=\"disabled\"");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref _s, "RefundBlock");
            }

            if (permissions.IndexOf("[U]") > -1)
            {
                TemplateHandler.RemoveBlock(ref _s, "UnSeRead");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref _s, "UnSeHost");
            }

            if (permissions.IndexOf("[C]") > -1)
            {
                TemplateHandler.RemoveBlock(ref _s, "CalcHTRead");
                TemplateHandler.RemoveBlock(ref _s, "CalcFTRead");
                TemplateHandler.RemoveBlock(ref _s, "CalcAllRead");
            }
            else
            {
                TemplateHandler.RemoveBlock(ref _s, "CalcHTHost");
                TemplateHandler.RemoveBlock(ref _s, "CalcFTHost");
                TemplateHandler.RemoveBlock(ref _s, "CalcAllHost");
            }

            return _s;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return string.Empty;
        }
    }
    #endregion

    #region HockeyHTML
    private string HockeyHTML(DataRow r, bool isRefund, bool IsMovebetdaq, bool IsExcludeBetdaq, bool IsSettleAbandonedMatch, bool IsRefundRunningBetDaq,
        bool IsMovebetfair = false, bool IsExcludeBetfair = false, bool IsSettleAbandonedBetFairMatch = false, bool IsRefundRunningBetFair = false)
    {
        int totalSets = 3; // for Hockey [1 Period, 2 Period, 3 Period]
        try
        {
            string _s = string.Empty;

            if (isRefund)
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/RefundHockey.htm");
                string tplRefund = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/Refund.htm");
                TemplateHandler.RemoveBlock(ref tplRefund, "RefundFT");
                TemplateHandler.RemoveBlock(ref tplRefund, "RefundMHT");
                TemplateHandler.Assign(ref _s, "{Refund}", tplRefund);
                CheckExcludeMoveBetDaq(ref _s, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq);
                CheckExcludeMoveBetFair(ref _s, IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
            }
            else
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/BillHockey.htm");

                TemplateController.RemoveBlock(ref _s, "RefundBlock");
            }


            var _applyHedging = new Settlement();
            _applyHedging.AssignApplyHedgingMatch(ref _s, matchID);

            TemplateController.Assign(ref _s, "{leaguename}", CacheRef.GetLeagueName(r["leagueid"]));
            TemplateController.Assign(ref _s, "{hometeam}", CacheRef.GetTeamName(r["homeid"]) + (CastAide.AsBoolean(r["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty));
            TemplateController.Assign(ref _s, "{awayteam}", CacheRef.GetTeamName(r["awayid"]));
            TemplateController.Assign(ref _s, "{matchcode}", ((!Convert.IsDBNull(r["matchcode"])) ? r["matchcode"].ToString() : string.Empty));
            TemplateController.Assign(ref _s, "{date}", string.Format(cult, "{0:MM/dd/yyyy}", r["eventdate"]));
            TemplateHandler.Assign(ref _s, "{kickoffBetdaq}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateHandler.Assign(ref _s, "{kickoffBetfair}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateController.Assign(ref _s, "{type}", (isBill) ? "bill" : "keyscore");
            TemplateController.Assign(ref _s, "{params}", r["matchid"].ToString() + "|" + sportID.ToString());
            //TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());

            bool _isRefund = Convert.ToBoolean(r["IsRefund"]);
            bool _isCalRefund = Convert.ToBoolean(r["IsCalRefund"]);
            int i = 0;
            int totalGameHome = 0, totalGameAway = 0;

            string _homeOT = string.Empty, _awayOT = string.Empty;
            if (!_isRefund && !_isCalRefund && !Convert.IsDBNull(r["betstatus"]) && !string.IsNullOrEmpty(r["betstatus"].ToString()))
            {
                try
                {
                    string[] _betStatus;

                    if (r["betstatus"].ToString().IndexOf(';') > -1)
                    {
                        _betStatus = r["betstatus"].ToString().Split(';')[1].Split(',');
                    }
                    else
                    {
                        _betStatus = r["betstatus"].ToString().Split(',');
                    }
                    for (; i < _betStatus.Length; i++)
                    {
                        string homeSet = _betStatus[i].Trim().Split('-')[0];
                        string awaySet = _betStatus[i].Trim().Split('-')[1];
                        totalGameHome += Convert.ToInt32(homeSet.Trim());
                        totalGameAway += Convert.ToInt32(awaySet.Trim());
                        TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "pHome}", homeSet.Trim());
                        TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "pAway}", awaySet.Trim());
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
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            for (; i < totalSets; i++)
            {
                TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "pHome}", string.Empty);
                TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "pAway}", string.Empty);
            }
            TemplateHandler.Assign(ref _s, "{valueOTHome}", _homeOT);
            TemplateHandler.Assign(ref _s, "{valueOTAway}", _awayOT);
            TemplateHandler.Assign(ref _s, "{homeFScore}", (!Convert.IsDBNull(r["finalhomescore"]) ? r["finalhomescore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{awayFScore}", (!Convert.IsDBNull(r["finalawayscore"]) ? r["finalawayscore"].ToString() : string.Empty));
            if (isBill)
            {
                TemplateHandler.Assign(ref _s, "{btnSubmitValue}", "CALCULATE");
                TemplateHandler.Assign(ref _s, "{disableUS}", ((string.Compare(r["htstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["ftstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["fglgstatus"].ToString(), "settled", true) == 0)) ? string.Empty : "disabled=\"disabled\"");

                if (permissions.IndexOf("[U]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "UnSeRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "UnSeHost");
                }

                if (permissions.IndexOf("[C]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcHost");
                }

            }
            else
            {
                TemplateHandler.Assign(ref _s, "{btnSubmitValue}", "KEY SCORE");
                TemplateHandler.RemoveBlock(ref _s, "RefundBlock");
                TemplateHandler.RemoveBlock(ref _s, "Unsettle");

                if (permissions.IndexOf("[K]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcHost");
                }
            }



            return _s;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return string.Empty;
        }
    }
    #endregion

    #region TennisHTML
    private string TennisHTML(DataRow r, bool isRefund, bool IsMovebetdaq, bool IsExcludeBetdaq, bool IsSettleAbandonedMatch, bool IsRefundRunningBetDaq,
        bool IsMovebetfair = false, bool IsExcludeBetfair = false, bool IsSettleAbandonedBetFairMatch = false, bool IsRefundRunningBetFair = false)
    {
        int totalSets = 5; // for Tennis
        try
        {
            string _s = string.Empty;

            if (isRefund)
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/RefundTennis.htm");
                string tplRefund = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/Refund.htm");
                TemplateHandler.RemoveBlock(ref tplRefund, "RefundFT");
                TemplateHandler.RemoveBlock(ref tplRefund, "RefundMHT");
                TemplateHandler.Assign(ref _s, "{Refund}", tplRefund);

                CheckExcludeMoveBetDaq(ref _s, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq);
                CheckExcludeMoveBetFair(ref _s, IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
            }
            else
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/BillTennis.htm");

                TemplateController.RemoveBlock(ref _s, "RefundBlock");
            }

            var _applyHedging = new Settlement();
            _applyHedging.AssignApplyHedgingMatch(ref _s, matchID);

            TemplateController.Assign(ref _s, "{leaguename}", CacheRef.GetLeagueName(r["leagueid"]));
            TemplateController.Assign(ref _s, "{hometeam}", CacheRef.GetTeamName(r["homeid"]) + (CastAide.AsBoolean(r["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty));
            TemplateController.Assign(ref _s, "{awayteam}", CacheRef.GetTeamName(r["awayid"]));
            TemplateController.Assign(ref _s, "{matchcode}", ((!Convert.IsDBNull(r["matchcode"])) ? r["matchcode"].ToString() : string.Empty));
            TemplateController.Assign(ref _s, "{date}", string.Format(cult, "{0:MM/dd/yyyy}", r["eventdate"]));
            TemplateHandler.Assign(ref _s, "{kickoffBetdaq}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateHandler.Assign(ref _s, "{kickoffBetfair}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateController.Assign(ref _s, "{type}", (isBill) ? "bill" : "keyscore");
            //TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            TemplateController.Assign(ref _s, "{sportID}", sportID.ToString());
            TemplateController.Assign(ref _s, "{params}", r["matchid"].ToString() + "|" + sportID.ToString());

            bool _isRefund = Convert.ToBoolean(r["IsRefund"]);
            bool _isCalRefund = Convert.ToBoolean(r["IsCalRefund"]);
            int i = 0;
            int totalGameHome = 0, totalGameAway = 0;
            if (!_isRefund && !_isCalRefund && !Convert.IsDBNull(r["betstatus"]) && !string.IsNullOrEmpty(r["betstatus"].ToString()))
            {
                try
                {
                    string[] _betStatus;

                    if (r["betstatus"].ToString().IndexOf(';') > -1)
                    {
                        _betStatus = r["betstatus"].ToString().Split(';')[1].Split(',');
                    }
                    else
                    {
                        _betStatus = r["betstatus"].ToString().Split(',');
                    }
                    for (; i < _betStatus.Length; i++)
                    {
                        var arr_betStatus = _betStatus[i].Trim().Split('-');
                        if (arr_betStatus.Length > 1)
                        {
                            string homeSet = _betStatus[i].Trim().Split('-')[0];
                            string awaySet = _betStatus[i].Trim().Split('-')[1];
                            totalGameHome += Convert.ToInt32(homeSet.Trim());
                            totalGameAway += Convert.ToInt32(awaySet.Trim());

                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "sHome}", homeSet.Trim());
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "sAway}", awaySet.Trim());
                        }
                        else
                        {
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "sHome}", string.Empty);
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "sAway}", string.Empty);
                        }
                    }
                    i--;
                }
                catch (Exception ex)
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            for (; i < totalSets; i++)
            {
                TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "sHome}", string.Empty);
                TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "sAway}", string.Empty);
            }
            TemplateController.Assign(ref _s, "{valueTGHome}", (totalGameHome > 0) ? totalGameHome.ToString() : string.Empty);
            TemplateController.Assign(ref _s, "{valueTGAway}", (totalGameAway > 0) ? totalGameAway.ToString() : string.Empty);
            TemplateController.Assign(ref _s, "{valueFSHome}", (!Convert.IsDBNull(r["finalhomescore"]) ? r["finalhomescore"].ToString() : string.Empty));
            TemplateController.Assign(ref _s, "{valueFSAway}", (!Convert.IsDBNull(r["finalawayscore"]) ? r["finalawayscore"].ToString() : string.Empty));
            if (isBill)
            {
                TemplateHandler.Assign(ref _s, "{btnSubmitValue}", "CALCULATE");
                TemplateHandler.Assign(ref _s, "{disableUS}", ((string.Compare(r["htstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["ftstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["fglgstatus"].ToString(), "settled", true) == 0)) ? string.Empty : "disabled=\"disabled\"");

                if (permissions.IndexOf("[U]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "UnSeRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "UnSeHost");
                }

                if (permissions.IndexOf("[C]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcHost");
                }

            }
            else
            {
                TemplateHandler.Assign(ref _s, "{btnSubmitValue}", "KEY SCORE");
                TemplateHandler.RemoveBlock(ref _s, "RefundBlock");
                TemplateHandler.RemoveBlock(ref _s, "Unsettle");

                if (permissions.IndexOf("[K]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcHost");
                }
            }


            return _s;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return string.Empty;
        }
    }
    #endregion

    #region BilliardHTML
    private string BilliardHTML(DataRow r, bool isRefund, bool IsMovebetdaq, bool IsExcludeBetdaq, bool IsSettleAbandonedMatch, bool IsRefundRunningBetDaq,
        bool IsMovebetfair = false, bool IsExcludeBetfair = false, bool IsSettleAbandonedBetFairMatch = false, bool IsRefundRunningBetFair = false)
    {
        try
        {
            string _s = string.Empty;

            if (isRefund)
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/RefundBilliard.htm");
                string tplRefund = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/Refund.htm");
                TemplateHandler.RemoveBlock(ref tplRefund, "RefundFT");
                TemplateHandler.RemoveBlock(ref tplRefund, "RefundMHT");
                TemplateHandler.Assign(ref _s, "{Refund}", tplRefund);

                CheckExcludeMoveBetDaq(ref _s, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq);
                CheckExcludeMoveBetFair(ref _s, IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
            }
            else
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/BillBilliard.htm");

                TemplateHandler.RemoveBlock(ref _s, "RefundBlock");
            }

            var _applyHedging = new Settlement();
            _applyHedging.AssignApplyHedgingMatch(ref _s, matchID);
            TemplateController.Assign(ref _s, "{leaguename}", CacheRef.GetLeagueName(r["leagueid"]));
            TemplateController.Assign(ref _s, "{hometeam}", CacheRef.GetTeamName(r["homeid"]) + (CastAide.AsBoolean(r["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty));
            TemplateController.Assign(ref _s, "{awayteam}", CacheRef.GetTeamName(r["awayid"]));
            TemplateController.Assign(ref _s, "{matchcode}", ((!Convert.IsDBNull(r["matchcode"])) ? r["matchcode"].ToString() : string.Empty));
            TemplateController.Assign(ref _s, "{date}", string.Format(cult, "{0:MM/dd/yyyy}", r["eventdate"]));
            TemplateHandler.Assign(ref _s, "{kickoffBetdaq}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateHandler.Assign(ref _s, "{kickoffBetfair}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateController.Assign(ref _s, "{homeFScore}", (!Convert.IsDBNull(r["finalhomescore"]) ? r["finalhomescore"].ToString() : string.Empty));
            TemplateController.Assign(ref _s, "{awayFScore}", (!Convert.IsDBNull(r["finalawayscore"]) ? r["finalawayscore"].ToString() : string.Empty));
            TemplateController.Assign(ref _s, "{type}", (isBill) ? "bill" : "keyscore");

            TemplateController.Assign(ref _s, "{params}", r["matchid"].ToString() + "|" + sportID.ToString());
            //TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            if (isBill)
            {
                TemplateHandler.Assign(ref _s, "{btnSubmitValue}", "CALCULATE");
                TemplateHandler.Assign(ref _s, "{disableUS}", ((string.Compare(r["htstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["ftstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["fglgstatus"].ToString(), "settled", true) == 0)) ? string.Empty : "disabled=\"disabled\"");

                //Check permission list
                if (permissions.IndexOf("[U]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "UnSeRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "UnSeHost");
                }

                if (permissions.IndexOf("[C]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcHost");
                }

            }
            else
            {
                TemplateHandler.Assign(ref _s, "{btnSubmitValue}", "KEY SCORE");
                TemplateHandler.RemoveBlock(ref _s, "RefundBlock");
                TemplateHandler.RemoveBlock(ref _s, "Unsettle");

                if (permissions.IndexOf("[K]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcHost");
                }

            }



            return _s;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return string.Empty;
        }
    }
    #endregion

    #region BadmintonHTML
    private string BadmintonHTML(DataRow r, bool isRefund, bool IsMovebetdaq, bool IsExcludeBetdaq, bool IsSettleAbandonedMatch, bool IsRefundRunningBetDaq,
        bool IsMovebetfair = false, bool IsExcludeBetfair = false, bool IsSettleAbandonedBetFairMatch = false, bool IsRefundRunningBetFair = false)
    {
        int totalSets = 5; // for Tennis
        try
        {
            string _s = string.Empty;

            if (isRefund)
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/RefundBadminton.htm");
                string tplRefund = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/Refund.htm");
                TemplateHandler.RemoveBlock(ref tplRefund, "RefundFT");
                TemplateHandler.RemoveBlock(ref tplRefund, "RefundMHT");
                TemplateHandler.Assign(ref _s, "{Refund}", tplRefund);

                CheckExcludeMoveBetDaq(ref _s, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq);
                CheckExcludeMoveBetFair(ref _s, IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
            }
            else
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/BillBadminton.htm");

                TemplateHandler.RemoveBlock(ref _s, "RefundBlock");
            }
            var _applyHedging = new Settlement();
            _applyHedging.AssignApplyHedgingMatch(ref _s, matchID);

            TemplateController.Assign(ref _s, "{leaguename}", CacheRef.GetLeagueName(r["leagueid"]));
            TemplateController.Assign(ref _s, "{hometeam}", CacheRef.GetTeamName(r["homeid"]) + (CastAide.AsBoolean(r["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty));
            TemplateController.Assign(ref _s, "{awayteam}", CacheRef.GetTeamName(r["awayid"]));
            TemplateController.Assign(ref _s, "{matchcode}", ((!Convert.IsDBNull(r["matchcode"])) ? r["matchcode"].ToString() : string.Empty));
            TemplateController.Assign(ref _s, "{date}", string.Format(cult, "{0:MM/dd/yyyy}", r["eventdate"]));
            TemplateHandler.Assign(ref _s, "{kickoffBetdaq}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateHandler.Assign(ref _s, "{kickoffBetfair}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateController.Assign(ref _s, "{type}", (isBill) ? "bill" : "keyscore");
            TemplateController.Assign(ref _s, "{params}", r["matchid"].ToString() + "|" + sportID.ToString());
            //TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());

            bool _isRefund = Convert.ToBoolean(r["IsRefund"]);
            bool _isCalRefund = Convert.ToBoolean(r["IsCalRefund"]);
            int i = 0;
            int totalGameHome = 0, totalGameAway = 0;
            if (!_isRefund && !_isCalRefund && !Convert.IsDBNull(r["betstatus"]) && !string.IsNullOrEmpty(r["betstatus"].ToString()))
            {
                try
                {
                    string[] _betStatus;

                    if (r["betstatus"].ToString().IndexOf(';') > -1)
                    {
                        _betStatus = r["betstatus"].ToString().Split(';')[1].Split(',');
                    }
                    else
                    {
                        _betStatus = r["betstatus"].ToString().Split(',');
                    }
                    for (; i < _betStatus.Length; i++)
                    {
                        var arr_betStatus = _betStatus[i].Trim().Split('-');
                        if (arr_betStatus.Length > 1)
                        {
                            string homeSet = _betStatus[i].Trim().Split('-')[0];
                            string awaySet = _betStatus[i].Trim().Split('-')[1];
                            totalGameHome += Convert.ToInt32(homeSet.Trim());
                            totalGameAway += Convert.ToInt32(awaySet.Trim());
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "sHome}", homeSet.Trim());
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "sAway}", awaySet.Trim());
                        }
                        else
                        {
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "sHome}", string.Empty);
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "sAway}", string.Empty);
                        }

                    }
                    i--;
                }
                catch (Exception ex)
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            for (; i < totalSets; i++)
            {
                TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "sHome}", string.Empty);
                TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "sAway}", string.Empty);
            }
            TemplateHandler.Assign(ref _s, "{valueTPHome}", (totalGameHome > 0) ? totalGameHome.ToString() : string.Empty);
            TemplateHandler.Assign(ref _s, "{valueTPAway}", (totalGameAway > 0) ? totalGameAway.ToString() : string.Empty);
            TemplateHandler.Assign(ref _s, "{valueTSHome}", (!Convert.IsDBNull(r["finalhomescore"]) ? r["finalhomescore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{valueTSAway}", (!Convert.IsDBNull(r["finalawayscore"]) ? r["finalawayscore"].ToString() : string.Empty));
            if (isBill)
            {
                TemplateHandler.Assign(ref _s, "{btnSubmitValue}", "CALCULATE");
                TemplateHandler.Assign(ref _s, "{disableUS}", ((string.Compare(r["htstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["ftstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["fglgstatus"].ToString(), "settled", true) == 0)) ? string.Empty : "disabled=\"disabled\"");

                if (permissions.IndexOf("[U]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "UnSeRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "UnSeHost");
                }

                if (permissions.IndexOf("[C]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcHost");
                }

            }
            else
            {
                TemplateHandler.Assign(ref _s, "{btnSubmitValue}", "KEY SCORE");
                TemplateHandler.RemoveBlock(ref _s, "RefundBlock");
                TemplateHandler.RemoveBlock(ref _s, "Unsettle");


                if (permissions.IndexOf("[K") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcHost");
                }
            }


            return _s;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return string.Empty;
        }
    }
    #endregion

    #region BaseballHTML
    private string BaseballHTML(DataRow r, bool isRefund, bool IsMovebetdaq, bool IsExcludeBetdaq, bool IsSettleAbandonedMatch, bool IsRefundRunningBetDaq,
        bool IsMovebetfair = false, bool IsExcludeBetfair = false, bool IsSettleAbandonedBetFairMatch = false, bool IsRefundRunningBetFair = false)
    {
        int totalSets = 9; // for Hockey [1 Period, 2 Period, 3 Period]
        try
        {
            string _s = string.Empty;

            if (isRefund)
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/RefundBaseball.htm");
                string tplRefund = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/Refund.htm");
                //TemplateHandler.RemoveBlock(ref tplRefund, "RefundFT");
                TemplateHandler.RemoveBlock(ref tplRefund, "RefundMHT");
                TemplateHandler.Assign(ref _s, "{Refund}", tplRefund);

                CheckExcludeMoveBetDaq(ref _s, IsMovebetdaq, IsExcludeBetdaq, IsSettleAbandonedMatch, IsRefundRunningBetDaq);
                CheckExcludeMoveBetFair(ref _s, IsMovebetfair, IsExcludeBetfair, IsSettleAbandonedBetFairMatch, IsRefundRunningBetFair);
            }
            else
            {
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/BillBaseball.htm");
            }

            var _applyHedging = new Settlement();
            _applyHedging.AssignApplyHedgingMatch(ref _s, matchID);
            TemplateController.Assign(ref _s, "{leaguename}", CacheRef.GetLeagueName(r["leagueid"]));
            TemplateController.Assign(ref _s, "{hometeam}", CacheRef.GetTeamName(r["homeid"]) + (CastAide.AsBoolean(r["isNeutral"]) ? Resources.Language.IsNeutral : string.Empty));
            TemplateController.Assign(ref _s, "{awayteam}", CacheRef.GetTeamName(r["awayid"]));
            TemplateController.Assign(ref _s, "{matchcode}", ((!Convert.IsDBNull(r["matchcode"])) ? r["matchcode"].ToString() : string.Empty));
            TemplateController.Assign(ref _s, "{date}", string.Format(cult, "{0:MM/dd/yyyy}", r["eventdate"]));
            TemplateHandler.Assign(ref _s, "{kickoffBetdaq}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateHandler.Assign(ref _s, "{kickoffBetfair}", string.Format(cult, "{0:MMM dd, yyyy HH:mm tt} GMT-4", ((DateTime)r["eventdate"]).AddHours(24)));
            TemplateController.Assign(ref _s, "{type}", (isBill) ? "bill" : "keyscore");

            TemplateController.Assign(ref _s, "{params}", r["matchid"].ToString() + "|" + sportID.ToString());
            //TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());
            TemplateController.Assign(ref _s, "{matchid}", r["matchid"].ToString());

            bool _isRefund = Convert.ToBoolean(r["IsRefund"]);
            bool _isCalRefund = Convert.ToBoolean(r["IsCalRefund"]);
            int i = 0;
            int totalGameHome = 0, totalGameAway = 0;
            string _homeEI = string.Empty, _awayEI = string.Empty;
            if (!_isRefund && !_isCalRefund && !Convert.IsDBNull(r["betstatus"]) && !string.IsNullOrEmpty(r["betstatus"].ToString()))
            {
                try
                {
                    string[] _betStatus;

                    if (r["betstatus"].ToString().IndexOf(';') > -1)
                    {
                        _betStatus = r["betstatus"].ToString().Split(';')[1].Split(',');
                    }
                    else
                    {
                        _betStatus = r["betstatus"].ToString().Split(',');
                    }
                    for (; i < _betStatus.Length; i++)
                    {
                        var arr_betStatus = _betStatus[i].Trim().Split('-');
                        if (arr_betStatus.Length > 1)
                        {
                            string homeSet = _betStatus[i].Trim().Split('-')[0];
                            string awaySet = _betStatus[i].Trim().Split('-')[1];
                            totalGameHome += Convert.ToInt32(homeSet.Trim());
                            totalGameAway += Convert.ToInt32(awaySet.Trim());
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "Home}", homeSet.Trim());
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "Away}", awaySet.Trim());
                        }
                        else
                        {
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "Home}", string.Empty);
                            TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "Away}", string.Empty);
                        }
                        if (i == totalSets && arr_betStatus.Length > 1)
                        {
                            _homeEI = _betStatus[i].Trim().Split('-')[0];
                            _awayEI = _betStatus[i].Trim().Split('-')[1];
                        }
                    }
                    i--;
                }
                catch (Exception ex)
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            for (; i < totalSets; i++)
            {
                TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "Home}", string.Empty);
                TemplateHandler.Assign(ref _s, "{value" + (i + 1).ToString() + "Away}", string.Empty);
            }
            TemplateHandler.Assign(ref _s, "{valueEIHome}", _homeEI);
            TemplateHandler.Assign(ref _s, "{valueEIAway}", _awayEI);
            TemplateHandler.Assign(ref _s, "{homeHScore}", (!Convert.IsDBNull(r["hthomescore"]) ? r["hthomescore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{awayHScore}", (!Convert.IsDBNull(r["htawayscore"]) ? r["htawayscore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{homeFScore}", (!Convert.IsDBNull(r["finalhomescore"]) ? r["finalhomescore"].ToString() : string.Empty));
            TemplateHandler.Assign(ref _s, "{awayFScore}", (!Convert.IsDBNull(r["finalawayscore"]) ? r["finalawayscore"].ToString() : string.Empty));
            if (isBill)
            {
                TemplateHandler.RemoveBlock(ref _s, "KeyScore");
                TemplateHandler.Assign(ref _s, "{btnSubmitValue}", "CALCULATE");
                TemplateHandler.Assign(ref _s, "{disableUS}", ((string.Compare(r["htstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["ftstatus"].ToString(), "settled", true) == 0) || (string.Compare(r["fglgstatus"].ToString(), "settled", true) == 0)) ? string.Empty : "disabled=\"disabled\"");
                TemplateHandler.Assign(ref _s, "{HTStatus}", r["htstatus"].ToString());
                TemplateHandler.Assign(ref _s, "{FTStatus}", r["ftstatus"].ToString());
                TemplateHandler.Assign(ref _s, "{HTbgcolor}", (string.Compare(r["htstatus"].ToString(), "settled", true) == 0) ? SettleBgColor.BILL : string.Empty);
                TemplateHandler.Assign(ref _s, "{FTbgcolor}", (string.Compare(r["ftstatus"].ToString(), "settled", true) == 0) ? SettleBgColor.BILL : string.Empty);

                if (permissions.IndexOf("[U]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "UnSeRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "UnSeHost");
                }

                if (permissions.IndexOf("[C]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcRead");
                    TemplateHandler.RemoveBlock(ref _s, "CalcAllRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcHost");
                    TemplateHandler.RemoveBlock(ref _s, "CalcAllHost");
                }

            }
            else
            {
                TemplateHandler.Assign(ref _s, "{btnSubmitValue}", "KEY SCORE");
                TemplateHandler.RemoveBlock(ref _s, "RefundBlock");
                TemplateHandler.RemoveBlock(ref _s, "Unsettle");

                if (permissions.IndexOf("[K]") > -1)
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcRead");
                }
                else
                {
                    TemplateHandler.RemoveBlock(ref _s, "CalcHost");
                }
            }

            return _s;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return string.Empty;
        }

    }
    #endregion

    #region CheckExcludeMoveBetDaq
    static void CheckExcludeMoveBetDaq(ref string _s, bool IsMovebetdaq, bool IsExcludeBetdaq, bool IsSettleAbandonedMatch, bool IsRefundRunningBetDaq)
    {
        if (!IsMovebetdaq)
        {
            TemplateHandler.RemoveBlock(ref _s, "MoveBetdaq");
            TemplateHandler.RemoveBlock(ref _s, "RefundRunningBetDaqTickets");
            TemplateHandler.RemoveBlock(ref _s, "Betdaq");
        }
        if (!IsRefundRunningBetDaq)
        {
            TemplateHandler.RemoveBlock(ref _s, "RefundRunningBetDaqTickets");
        }
        if (!IsExcludeBetdaq)
        {
            TemplateHandler.RemoveBlock(ref _s, "ExcludeBetdaq");
        }
        if (!IsSettleAbandonedMatch)
        {
            TemplateHandler.RemoveBlock(ref _s, "SettleDeterminedTickets");
        }
    }
    #endregion

    #region CheckExcludeMoveBetFair
    static void CheckExcludeMoveBetFair(ref string _s, bool IsMovebetfair, bool IsExcludeBetfair, bool IsSettleAbandonedMatch, bool IsRefundRunningBetFair)
    {
        if (!IsMovebetfair)
        {
            TemplateHandler.RemoveBlock(ref _s, "MoveBetfair");
            TemplateHandler.RemoveBlock(ref _s, "RefundRunningBetFairTickets");
            TemplateHandler.RemoveBlock(ref _s, "Betfair");
        }
        if (!IsRefundRunningBetFair)
        {
            TemplateHandler.RemoveBlock(ref _s, "RefundRunningBetFairTickets");
        }
        if (!IsExcludeBetfair)
        {
            TemplateHandler.RemoveBlock(ref _s, "ExcludeBetfair");
        }
        if (!IsSettleAbandonedMatch)
        {
            TemplateHandler.RemoveBlock(ref _s, "SettleDeterminedBetfairTickets");
        }
    }
    #endregion
}
