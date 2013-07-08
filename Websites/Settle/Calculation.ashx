<%@ WebHandler Language="C#" Class="Calculation" %>

using System;
using System.Web;

using DNAide.Core;
using DNAide.Web;
using Fanex.Tarus.Settlement.Entity;
using Sunplus.Accounting.Business.Settlement;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Web.SessionState;
using DNAide.Data;

public class Calculation : IHttpHandler, IRequiresSessionState
{
    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    private string nl = Environment.NewLine;
    private int custID = -1, sportID = -1, matchID = -1;
    private string type = string.Empty;
    private string permissions = "";
    private string _matchIds_Hedging = string.Empty;
    private string _errMsg = string.Empty;
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        string _s = "false";
        var successMatchIds = string.Empty;
        string msg = "";
        int delaytime = 0;
        var lastGoalOnly = false;
        //context.Request.Form["scoreDirect"];


        #region score confirm

        string hiddenScoreConfirm_Sport = context.Request.Form["hiddenScoreConfirm_Sport"];

        #region soccer
        string homeHScore = string.Empty;
        string awayHScore = string.Empty;
        string homeFScore = string.Empty;
        string awayFScore = string.Empty;
        #endregion

        #region BasketballFootbal
        string scoreConfirm_Betstatus = string.Empty;
        #endregion

        homeHScore = context.Request.Form["hiddenScoreConfirm_homeHScore"];
        awayHScore = context.Request.Form["hiddenScoreConfirm_awayHScore"];
        homeFScore = context.Request.Form["hiddenScoreConfirm_homeFScore"];
        awayFScore = context.Request.Form["hiddenScoreConfirm_awayFScore"];

        switch (hiddenScoreConfirm_Sport)
        {
            case "soccer":
            case "Common":
                break;
            case "BasketballFootbal":
            case "Hockey":
            case "Tennis":
            case "Badminton":
            case "Baseball":
                scoreConfirm_Betstatus = context.Request.Form["hiddenBetStatus_ConfirmScore"];
                break;
        }

        if (context.Request["chkIsSettleDeterminedTickets"] == null || context.Request["chkIsSettleDeterminedTickets"] != "on")
        {
            homeHScore = null;
            awayHScore = null;
            homeFScore = null;
            awayFScore = null;
            scoreConfirm_Betstatus = null;
        }
        #endregion

        try
        {
            custID = Convert.ToInt32(context.Session["AccId"] as object ?? -1);
            if (custID == -1) throw new AccException(IBCUtils.IbcError.ACCESS_DENIED_MSG);
            permissions = Sunplus.Accounting.Business.Security.Permissions.CheckHostPermissionSettle(custID);
            try
            {
                type = context.Request.Form["sAction"];
                //MatchID|SportID
                string _params = context.Request.Form["sParams"];
                matchID = Convert.ToInt32(_params.Split('|')[0]);
                sportID = Convert.ToInt32(_params.Split('|')[1]);

                _matchIds_Hedging = context.Request["txtMatchIds_Hedging"];

                _matchIds_Hedging = CastAide.AsString(_matchIds_Hedging, string.Empty);
                _matchIds_Hedging = _matchIds_Hedging + "^" + matchID;

            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                //Logs.Write(ex.Message);
                _s = "<br />" + AlertMessage.GetMsg("Sorry, Invalid session!", AlertMessage.MsgType.Error);
                //throw ex;
            }

            var arrIds = _matchIds_Hedging.Split('^');
            bool _isSuccess = false;
            bool _isDeny = false;
            bool IsExcludeBetdaq = context.Request["chkIsExcludeBetdaq"] != null;
            bool IsSettleDeterminedTickets = false;
            if (context.Request["chkIsSettleDeterminedTickets"] != null && context.Request["chkIsSettleDeterminedTickets"] == "on")
            {
                IsSettleDeterminedTickets = true;
            }

            foreach (string _matchId in arrIds)
            {
                if (_matchId.Length == 0) continue;

                matchID = CastAide.AsInt32(_matchId);
                try
                {
                    switch (type.ToLowerInvariant())
                    {
                        case "bill":
                        case "billht":
                        case "billft":
                            if (permissions.IndexOf("[C]") > -1)
                            {
                                bool _isScoreDirect = false;
                                if (context.Request.Form["isSD"] != null)
                                {
                                    _isScoreDirect = (string.Compare(context.Request.Form["isSD"], "1", true) == 0);
                                }
                                _isSuccess = DoSettle.Calculation(custID, type, sportID, matchID, context, _isScoreDirect, ref msg, ref delaytime);
                            }
                            else
                            {
                                _isDeny = true;
                            }
                            break;
                        case "keyscore":
                            if (permissions.IndexOf("[K]") > -1)
                            {
                                bool _isScoreDirect = false;
                                if (context.Request.Form["isSD"] != null)
                                {
                                    _isScoreDirect = (string.Compare(context.Request.Form["isSD"], "1", true) == 0);
                                }
                                _isSuccess = DoSettle.Calculation(custID, type, sportID, matchID, context, _isScoreDirect, ref msg, ref delaytime);
                            }
                            else
                            {
                                _isDeny = true;
                            }
                            break;
                        case "calculate": // for FGLG only
                            if (permissions.IndexOf("[C]") > -1)
                            {
                                if (sportID != 1) break;

                                string goalrecord = context.Request.Form["GR"];
                                string oldbetstatus = context.Request.Form["statusFGLG"];
                                string finalHomeScore = context.Request.Form["homeF"];
                                string finalAwayScore = context.Request.Form["awayF"];
                                string betstatus = string.Empty;
                                var match = new Match();
                                match.GoalRecord = goalrecord;
                                match.FinalHomeScore = finalHomeScore;
                                match.FinalAwayScore = finalAwayScore;
                                match.BetStatus = oldbetstatus;
                                betstatus = match.GenerateMatchStatus();

                                var errCode = 0;
                                Sunplus.Accounting.DataAccess.IBCDLL.Accounting.Settle.SettlementSoccerFGLG(custID, matchID, betstatus, false, goalrecord, ref errCode, ref msg);
                                if (errCode == 0)
                                    _isSuccess = true;
                                else
                                    _isSuccess = false;
                            }
                            else
                            {
                                _isDeny = true;
                            }
                            break;
                        case "refundhalftime":
                            if (permissions.IndexOf("[C]") > -1)
                            {
                                _isSuccess = DoSettle.RefundHalfTime(custID, matchID, IsExcludeBetdaq, IsSettleDeterminedTickets, ref msg, homeHScore, awayHScore);
                            }
                            break;
                        case "refund":
                            if (permissions.IndexOf("[C]") > -1)
                            {
                                _isSuccess = DoSettle.RefundOrCancel(custID, matchID, IsExcludeBetdaq, IsSettleDeterminedTickets, ref msg, homeHScore, awayHScore, homeFScore, awayFScore, scoreConfirm_Betstatus);
                            }
                            break;
                        case "unsettle": //undo settlement
                            if (permissions.IndexOf("[U]") > -1)
                            {
                                // _isSuccess = DoSettle.UndoSettlement(custID, matchID);                        
                                _isSuccess = DoSettle.Calculation(custID, type, sportID, matchID, context, false, ref msg, ref delaytime);
                                if (_isSuccess)
                                {
                                    successMatchIds += _matchId + "|";
                                }
                            }
                            else
                            {
                                _isDeny = true;
                            }
                            break;
                        case "refundfulltime":
                            if (permissions.IndexOf("[C]") > -1)
                            {
                                _isSuccess = DoSettle.RefundFullTime(custID, matchID, IsExcludeBetdaq, IsSettleDeterminedTickets, ref msg, homeFScore, awayFScore, scoreConfirm_Betstatus);
                            }
                            else
                            {
                                _isDeny = true;
                            }
                            break;
                        case "refundfirstnogoal":
                            if (permissions.IndexOf("[C]") > -1)
                            {
                                _isSuccess = DoSettle.RefundSoccerFGLG(custID, matchID, IsExcludeBetdaq, ref msg);
                            }
                            else
                            {
                                _isDeny = true;
                            }
                            break;
                        case "refundalllastgoal":
                            lastGoalOnly = true;
                            if (permissions.IndexOf("[C]") > -1)
                            {
                                _isSuccess = DoSettle.RefundSoccerAllLastGoal(custID, matchID, IsExcludeBetdaq, ref msg);
                            }
                            else
                            {
                                _isDeny = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch { }


            }

            //Set action confirm from IBC to Apollo
            var apollo = new Fanex.Tarus.Apollo.Settlement.Business.SettlementService();
            int _homeHT = 0, _awayHT = 0, _homeFT = 0, _awayFT = 0;
            string _betStatus = string.Empty, _goalrecord = string.Empty;
            bool _isSD = false, _isHTCal = false;

            if (context.Request.Form["isSD"] != null)
            {
                _isSD = (string.Compare(context.Request.Form["isSD"], "1", true) == 0);
            }
            if (string.Compare(type, "billHT", true) == 0)
            {
                _isHTCal = true;
            }

            apollo.CalculationParams(sportID, context, ref _homeHT, ref _awayHT, ref _homeFT, ref _awayFT, ref _betStatus, _isSD, _isHTCal);
            if (sportID == 1)
            {
                bool IsFGLG = context.Request.Form["sHasFGLG"].ToString() == "1";
                if (IsFGLG)
                {
                    _goalrecord = CastAide.AsString(context.Request.Form["GR"]);
                    var _match = new Match();
                    _match.GoalRecord = _goalrecord;
                    _match.FinalHomeScore = context.Request.Form["homeF"];
                    _match.FinalAwayScore = context.Request.Form["awayF"];
                    _betStatus = _match.GenerateMatchStatus();
                }
            }

            int ActionId = apollo.ParseActionType(type.ToLower());
            matchID = Convert.ToInt32(context.Request.Form["sParams"].Split('|')[0]);
            var result = apollo.Set_ConfirmedAction_Single(matchID.ToString(), ActionId, _homeFT, _awayFT, _homeHT, _awayHT, _goalrecord, _betStatus);
            _errMsg = result.ErrMsg;
            //End Set action confirm from IBC to Apollo

            if (_isSuccess)
            {
                _s = "success"; // close and refesh

                if (sportID == 1 && context.Request.Form["sHasFGLG"] == "1" && !lastGoalOnly) _s = "soccerSuccess";
                if (lastGoalOnly) _s = "success-refundalllastgoal";
                if (!string.IsNullOrEmpty(msg)) _s = "no|" + msg;
            }
            else
            {
                if (_isDeny)
                {
                    _s = "Access Denied";
                }
            }

            if (!string.IsNullOrEmpty(msg)) _s = "no|" + msg;
            //if (delaytime > 0) _s = "delay|" + delaytime.ToString();
        }
        catch (DNAide.Utils.AccException ex)
        {
            _s = "<br />" + AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            _s = "<br />" + AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
            //throw ex;
        }

        context.Response.Write("{\"responseText\":\"" + _s + "\",\"successMatchIds\":\"" + successMatchIds + "\",\"status\":\"" + _errMsg + "\"}");
    }

    private string BetStatusBuilder(int matchID, string betstatus)
    {
        string _betstatus = string.Empty;
        var outputParams = new System.Collections.Hashtable();
        var dr = Settle.SelectMatchForKeyScore(matchID, ref outputParams);
        _betstatus = dr.Tables[0].Rows[0]["betstatus"].ToString();
        if (string.IsNullOrEmpty(_betstatus))
        {
            _betstatus = betstatus;
        }
        else if (!_betstatus.Contains("FL:"))
        {
            _betstatus += betstatus;
        }
        else
        {
            int start = _betstatus.IndexOf("FL:");
            int cnt = _betstatus.Length - start;
            _betstatus = _betstatus.Remove(start, cnt);
            _betstatus += betstatus;
        }
        return _betstatus;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
