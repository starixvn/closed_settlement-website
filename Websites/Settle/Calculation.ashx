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
                catch (Exception ex)
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex); 
                }


            }

            if (_isSuccess)
            {
                _s = "success"; // close and refesh

                //sync score to apollo

                this.SyncScoreToApollo(type.ToLower(), matchID, sportID, context);

                //endsync score to apollo

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

                string _infoMsg = msg + "," +_s + ", matchid:" + matchID + "," + SyncScoreToApollo(type.ToLower(), sportID, context);
                Fanex.Logging.Logger.Log.Info(_infoMsg, "Apollo-Settlement");
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

        context.Response.Write("{\"responseText\":\"" + _s + "\",\"successMatchIds\":\"" + successMatchIds + "\"}");
    }

    private int ActionKey(string type)
    {
        var rs = 0;
        switch (type)
        {
            case "unsettle": // 0: Unsettle
                rs = 0;
                break;
            case "calculate": // 1: Calculate FG/NG
                rs = 1;
                break;
            case "billht": // 2: Calculate HT
                rs = 2;
                break;
            case "billft": // 3: Calculate FT
                rs = 3;
                break;
            case "bill": // 4: Calculate All
                rs = 4;
                break;
            case "m": // 5: Calculate Parlay
                rs = 5;
                break;
            case "refundhalftime": // 11: Refund HT
                rs = 11;
                break;
            case "refundfulltime": // 12: Refund FT
                rs = 12;
                break;
            case "refund": // 13: Refund All
                rs = 13;
                break;
            case "rht": // 14: Refund Parlay HT
                rs = 14;
                break;
            case "rft": // 15: Refund Parlay FT
                rs = 15;
                break;
            case "r": // 16: Refund Parlay All
                rs = 16;
                break;
            case "refundfirstnogoal": // 17: Refund All FGLG
                rs = 17;
                break;
            case "refundalllastgoal": // 18: Refund All LG
                rs = 18;
                break;
        }
        return rs;
    }

    private void SyncScoreToApollo(string type, int matchId, int sportId, HttpContext context)
    {
        string score = string.Empty;
        try
        {
            //ref int homeHT, ref int awayHT, ref int homeFT, ref int awayFT, ref string betStatus
            int homeHT = 0;
            int awayHT = 0;
            int homeFT = 0;
            int awayFT = 0;
            string betStatus = string.Empty;
            string goalrecord = string.Empty;
            bool isHTCal = type == "billht";
            bool isScoreDirect = false;

            if (context.Request.Form["isSD"] != null)
            {
                isScoreDirect = (string.Compare(context.Request.Form["isSD"], "1", true) == 0);
            }

            DoSettle.CalculationParams(sportId, context, ref homeHT, ref awayHT, ref homeFT, ref awayFT, ref betStatus, isScoreDirect, isHTCal);

            if (sportId == 1)
            {
                goalrecord = CastAide.AsString(context.Request.Form["GR"]);
                bool IsFGLG = context.Request.Form["sHasFGLG"].ToString() == "1";
                betStatus = DoSettle.UpdateBetStatusForBill(context, IsFGLG);
            }

            var service = new Fanex.Tarus.Apollo.Settlement.Business.SettlementService();
            service.Set_ConfirmedAction_Single(matchId.ToString(), this.ActionKey(type),
                homeFT, awayFT, homeHT, awayHT, goalrecord, betStatus);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
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

    private string SyncScoreToApollo(string type, int sportId, HttpContext context)
    {
        string score = string.Empty;
        try
        {
            //ref int homeHT, ref int awayHT, ref int homeFT, ref int awayFT, ref string betStatus 
            int homeHT = 0;
            int awayHT = 0;
            int homeFT = 0;
            int awayFT = 0;
            string betStatus = string.Empty;
            string goalrecord = string.Empty;
            bool isHTCal = type.ToLower() == "billht";
            bool isScoreDirect = false;

            if (context.Request.Form["isSD"] != null)
            {
                isScoreDirect = (string.Compare(context.Request.Form["isSD"], "1", true) == 0);
            }

            DoSettle.CalculationParams(sportId, context, ref homeHT, ref awayHT, ref homeFT, ref awayFT, ref betStatus, isScoreDirect, isHTCal);

            if (sportId == 1)
            {
                goalrecord = CastAide.AsString(context.Request.Form["GR"]);
                bool IsFGLG = context.Request.Form["sHasFGLG"].ToString() == "1";
                betStatus = DoSettle.UpdateBetStatusForBill(context, IsFGLG);
            }


            score = "\"homeHT\":\"" + homeHT + "\",";
            score += "\"awayHT\":\"" + awayHT + "\",";
            score += "\"homeFT\":\"" + homeFT + "\",";
            score += "\"awayFT\":\"" + awayFT + "\",";
            score += "\"betStatus\":\"" + betStatus + "\",";
            score += "\"goalrecord\":\"" + goalrecord + "\",";
            score += "\"type\":\"" + type.ToLower();

        }
        catch (Exception ex)
        {

        }
        return score;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
