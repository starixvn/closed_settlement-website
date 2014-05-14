using System;
using System.Web;
using DNAide.Core;
using DNAide.Data;

using Sunplus.Accounting.Entities;
using System.Collections;
using Fanex.Tarus.Settlement.Entity;
using Sunplus.Accounting.Business.Constants;


namespace Sunplus.Accounting.Business.Settlement
{
    /// <summary>
    /// Summary description for DoSettle
    /// </summary>
    public static class DoSettle
    {
        private static System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
        public static bool Calculation(int custID, string type, int sportID, int matchID, HttpContext hc, bool isScoreDirect, ref string msg, ref int errorCode)
        {
            try
            {
                int _homeHT = 0, _awayHT = 0, _homeFT = 0, _awayFT = 0;
                string _betStatus = string.Empty, goalrecord = string.Empty;
                bool _isHTCal = false;
                string _action = "CAL"; // default: BILL ALL
                if (string.Compare(type, "billHT", true) == 0)
                {
                    _action = "CAL-HT";
                    _isHTCal = true;
                }
                else if (string.Compare(type, "billFT", true) == 0) _action = "CAL-FT";
                else if (string.Compare(type, "unsettle", true) == 0) _action = "UNDO";

                CalculationParams(sportID, hc, ref _homeHT, ref _awayHT, ref _homeFT, ref _awayFT, ref _betStatus, isScoreDirect, _isHTCal);

                if (string.Compare(type, "keyscore", true) == 0) // key score
                {
                    if (sportID == 1) _betStatus = BetStatusForKEYSCORE(matchID);
                    DataAccess.IBCDLL.Accounting.Settle.KeyScore(custID, matchID, _homeHT, _awayHT, _homeFT, _awayFT, _betStatus);
                    return true;
                }
                // bill
                if (sportID == 1)
                {
                    goalrecord = CastAide.AsString(hc.Request.Form["GR"]);
                    bool IsFGLG = hc.Request.Form["sHasFGLG"].ToString() == "1";
                    bool has1HFGLG = hc.Request.Form["sHas1HFGLG"] == "1";
                    _betStatus = UpdateBetStatusForBill(hc, IsFGLG, has1HFGLG, _isHTCal);
                }

                if (_action != "UNDO")
                {
                    var rs = DataAccess.IBCDLL.Accounting.Settle.SettlementChecking(matchID, _action);
                    if (rs.ErrorCode > 0)
                    {
                        msg = CastAide.AsString(rs.ErrorMessage);
                        return false;
                    }
                }

                DataAccess.IBCDLL.Accounting.Settle.Settlement(custID, matchID, _homeHT, _awayHT, _homeFT, _awayFT, _betStatus, _action, _isHTCal, goalrecord, ref errorCode, ref msg);
                if (errorCode > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                msg = ex.Message;
                //throw;
            }
            return false;
        }

        public static string UpdateBetStatusForBill(HttpContext hc, bool isFGLG, bool has1HFGLG = false, bool isHT = false, bool isApollo = false)
        {
            string _betstatus = string.Empty;
            var match = new Match();

            string goalrecord = hc.Request.Form["GR"];
            string finalHomeScore = hc.Request.Form["homeF"];
            string finalAwayScore = hc.Request.Form["awayF"];

            ////BetStatus
            if (isFGLG || has1HFGLG)
            {
                match.GoalRecord = goalrecord;
                match.FinalHomeScore = finalHomeScore;
                match.FinalAwayScore = finalAwayScore;
                match.IsHT = isHT;
                match.IsHTFGLG = has1HFGLG;
                match.HasFTFGLG = isFGLG;

                _betstatus = isApollo ?
                    match.GenerateMatchStatusOld() :
                    match.GenerateMatchStatus();
            }

            return _betstatus;
        }

        private static string BetStatusForKEYSCORE(int matchID)
        {
            var ouputParams = new Hashtable();
            var dr = DataAccess.IBCDLL.Accounting.Settle.SelectMatchForKeyScore(matchID, ref ouputParams);
            return dr.Tables[0].Rows[0]["betstatus"].ToString();
        }

        public static void CalculationParams(int sportID, HttpContext hc, ref int homeHT, ref int awayHT, ref int homeFT, ref int awayFT, ref string betStatus, bool isScoreDirect, bool isHTCal)
        {
            if (sportID == 1) // soccer 
            {
                homeHT = CastAide.AsInt32(hc.Request.Form["homeH"], 0);
                awayHT = CastAide.AsInt32(hc.Request.Form["awayH"], 0);
                homeFT = CastAide.AsInt32(hc.Request.Form["homeF"], 0);
                awayFT = CastAide.AsInt32(hc.Request.Form["awayF"], 0);


                // betStatus = "FL:" + (hc.Request.Form["chkFg"] ?? "0") + "-" + (hc.Request.Form["chkLg"] ?? "0");            
            }
            else if (sportID == 2 || sportID == 3 || sportID == 26 || sportID == 32) // Basketball, football, rugby, netball
            {
                if (isScoreDirect)
                {
                    homeHT = CastAide.AsInt32(hc.Request.Form["homeH"], 0);
                    awayHT = CastAide.AsInt32(hc.Request.Form["awayH"], 0);
                    homeFT = CastAide.AsInt32(hc.Request.Form["homeF"], 0);
                    awayFT = CastAide.AsInt32(hc.Request.Form["awayF"], 0);
                }
                else
                {
                    int _home = 0, _away = 0;
                    for (int i = 1; i < 5; i++)
                    {
                        _home = CastAide.AsInt32(hc.Request.Form["txt" + i.ToString() + "qHome"], 0);
                        _away = CastAide.AsInt32(hc.Request.Form["txt" + i.ToString() + "qAway"], 0);
                        if (isHTCal) // this for betstatus when HT Calculate 
                        {
                            if (i < 3)
                            {
                                betStatus += (!string.IsNullOrEmpty(betStatus) ? "," : "") + _home.ToString() + "-" + _away.ToString();
                            }
                        }
                        else
                        {
                            betStatus += (!string.IsNullOrEmpty(betStatus) ? "," : "") + _home.ToString() + "-" + _away.ToString();
                        }
                        if (i < 3)
                        {
                            homeHT += _home;
                            awayHT += _away;
                        }
                        homeFT += _home;
                        awayFT += _away;
                    }
                    if (!string.IsNullOrEmpty(hc.Request.Form["txtOTHome"]) || !string.IsNullOrEmpty(hc.Request.Form["txtOTAway"]))
                    {
                        int _otHome = CastAide.AsInt32(hc.Request.Form["txtOTHome"], 0);
                        int _otAway = CastAide.AsInt32(hc.Request.Form["txtOTAway"], 0);
                        if (!isHTCal) betStatus += "," + _otHome.ToString() + "-" + _otAway.ToString();
                        homeFT += _otHome;
                        awayFT += _otAway;
                    }
                }
            }
            else if (sportID == 4 || sportID == 28) // Hockey, filed Hockey
            {
                if (isScoreDirect)
                {
                    homeFT = CastAide.AsInt32(hc.Request.Form["homeF"], 0);
                    awayFT = CastAide.AsInt32(hc.Request.Form["awayF"], 0);
                }
                else
                {
                    int _home = 0, _away = 0;
                    for (int i = 1; i < 4; i++)
                    {
                        _home = CastAide.AsInt32(hc.Request.Form["txt" + i.ToString() + "pHome"], 0);
                        _away = CastAide.AsInt32(hc.Request.Form["txt" + i.ToString() + "pAway"], 0);
                        betStatus += (!string.IsNullOrEmpty(betStatus) ? "," : "") + _home.ToString() + "-" + _away.ToString();
                        homeFT += _home;
                        awayFT += _away;
                    }
                    if (!string.IsNullOrEmpty(hc.Request.Form["txtOTHome"]) || !string.IsNullOrEmpty(hc.Request.Form["txtOTAway"]))
                    {
                        int _otHome = CastAide.AsInt32(hc.Request.Form["txtOTHome"], 0);
                        int _otAway = CastAide.AsInt32(hc.Request.Form["txtOTAway"], 0);
                        betStatus += "," + _otHome.ToString() + "-" + _otAway.ToString();
                        homeFT += _otHome;
                        awayFT += _otAway;
                    }
                }
            }
            else if (sportID == 5) // Tennis
            {
                if (isScoreDirect)
                {
                    homeFT = CastAide.AsInt32(hc.Request.Form["txtFSHome"], 0);
                    awayFT = CastAide.AsInt32(hc.Request.Form["txtFSAway"], 0);
                }
                else
                {
                    int _home = 0, _away = 0;
                    for (int i = 1; i < 6; i++)
                    {
                        _home = CastAide.AsInt32(hc.Request.Form["txt" + i.ToString() + "sHome"], 0);
                        _away = CastAide.AsInt32(hc.Request.Form["txt" + i.ToString() + "sAway"], 0);
                        betStatus += (!string.IsNullOrEmpty(betStatus) ? "," : "") + _home.ToString() + "-" + _away.ToString();
                        if (_home > _away) homeFT++;
                        if (_home < _away) awayFT++;
                    }
                }
            }
            else if (sportID == 6 || sportID == 9 || sportID == 30) // Badminton, Volleyball, Squash
            {
                if (isScoreDirect)
                {
                    homeFT = CastAide.AsInt32(hc.Request.Form["txtTSHome"], 0);
                    awayFT = CastAide.AsInt32(hc.Request.Form["txtTSAway"], 0);
                }
                else
                {
                    int _home = 0, _away = 0;
                    for (int i = 1; i < 6; i++)
                    {
                        _home = CastAide.AsInt32(hc.Request.Form["txt" + i.ToString() + "sHome"], 0);
                        _away = CastAide.AsInt32(hc.Request.Form["txt" + i.ToString() + "sAway"], 0);
                        betStatus += (!string.IsNullOrEmpty(betStatus) ? "," : "") + _home.ToString() + "-" + _away.ToString();
                        if (_home > _away) homeFT++;
                        if (_home < _away) awayFT++;
                    }
                }
            }
            else if (sportID == 7 ||
                sportID == 10 ||
                sportID == 11 ||
                sportID == 29 ||
                sportID == SportConstants.HorseRacingFixedOdds) // Billiard, Golf, Motorsports, Winter Sports, HorseRacingFixedOdds
            {
                homeFT = CastAide.AsInt32(hc.Request.Form["homeF"], 0);
                awayFT = CastAide.AsInt32(hc.Request.Form["awayF"], 0);
            }
            else if (sportID == 8) // Baseball       
            {
                if (isScoreDirect)
                {
                    homeHT = CastAide.AsInt32(hc.Request.Form["homeH"], 0);
                    awayHT = CastAide.AsInt32(hc.Request.Form["awayH"], 0);
                    homeFT = CastAide.AsInt32(hc.Request.Form["homeF"], 0);
                    awayFT = CastAide.AsInt32(hc.Request.Form["awayF"], 0);
                }
                else
                {
                    int _home = 0, _away = 0;
                    for (int i = 1; i < 10; i++)
                    {
                        _home = CastAide.AsInt32(hc.Request.Form["txt" + i.ToString() + "Home"], 0);
                        _away = CastAide.AsInt32(hc.Request.Form["txt" + i.ToString() + "Away"], 0);
                        if (isHTCal) // this for betstatus when HT Calculate 
                        {
                            if (i < 6)
                            {
                                betStatus += (!string.IsNullOrEmpty(betStatus) ? "," : "") + _home.ToString() + "-" + _away.ToString();
                            }
                        }
                        else
                        {
                            betStatus += (!string.IsNullOrEmpty(betStatus) ? "," : "") + _home.ToString() + "-" + _away.ToString();
                        }
                        if (i < 6)
                        {
                            homeHT += _home;
                            awayHT += _away;
                        }

                        homeFT += _home;
                        awayFT += _away;
                    }
                    if (!string.IsNullOrEmpty(hc.Request.Form["txtEIHome"]) || !string.IsNullOrEmpty(hc.Request.Form["txtEIAway"]))
                    {
                        int _eiHome = CastAide.AsInt32(hc.Request.Form["txtEIHome"], 0);
                        int _eiAway = CastAide.AsInt32(hc.Request.Form["txtEIAway"], 0);
                        if (!isHTCal)
                            betStatus += "," + _eiHome.ToString() + "-" + _eiAway.ToString();
                        homeFT += _eiHome;
                        awayFT += _eiAway;
                    }
                }
            }
            else // Financial, OtherSports
            {
                homeHT = CastAide.AsInt32(hc.Request.Form["homeH"], 0);
                awayHT = CastAide.AsInt32(hc.Request.Form["awayH"], 0);
                homeFT = CastAide.AsInt32(hc.Request.Form["homeF"], 0);
                awayFT = CastAide.AsInt32(hc.Request.Form["awayF"], 0);
            }
        }

        public static bool RefundHalfTime(int custID, int matchID, bool IsExcludeBetdaq, bool IsSettleDeterminedTickets, ref string msg
            , string homeHScore, string awayHScore, bool IsExcludeBetfair = false, bool IsSettleDeterminedBetFairTickets = false)
        {
            try
            {
                return DataAccess.IBCDLL.Accounting.Settle.RefundHalfTime(custID, matchID, IsExcludeBetdaq, IsSettleDeterminedTickets, ref msg, homeHScore, awayHScore, IsExcludeBetfair, IsSettleDeterminedBetFairTickets);

            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return false;
        }

        public static bool RefundOrCancel(int custID, int matchID, bool IsExcludeBetdaq, bool IsSettleDeterminedTickets, ref string msg
            , string homeHScore, string awayHScore, string homeFScore, string awayFScore, string scoreConfirm_Betstatus, bool IsExcludeBetfair = false, bool IsSettleDeterminedBetFairTickets = false, bool IsSettleAbandonedMatchIBC = false)
        {
            try
            {
                return DataAccess.IBCDLL.Accounting.Settle.Refund(custID, matchID, IsExcludeBetdaq, IsSettleDeterminedTickets, ref msg, homeHScore, awayHScore, homeFScore, awayFScore, scoreConfirm_Betstatus, IsExcludeBetfair, IsSettleDeterminedBetFairTickets, IsSettleAbandonedMatchIBC);
            }
            catch (Exception ex)
            {


                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

                msg = ex.Message;
                return false;
            }
        }

        public static bool RefundFullTime(int custID, int matchID, bool IsExcludeBetdaq, bool IsSettleDeterminedTickets, ref string msg
            , string homeFScore, string awayFScore, string scoreConfirm_Betstatus, bool IsExcludeBetfair = false, bool IsSettleDeterminedBetFairTickets = false, bool IsSettleAbandonedMatchIBC = false)
        {
            try
            {
                return DataAccess.IBCDLL.Accounting.Settle.RefundFullTime(custID, matchID, IsExcludeBetdaq, IsSettleDeterminedTickets, ref msg, homeFScore, awayFScore, scoreConfirm_Betstatus, IsExcludeBetfair, IsSettleDeterminedBetFairTickets, IsSettleAbandonedMatchIBC);
            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return false;
        }

        public static bool RefundSoccerFGLG(int custID, int matchID, ref string errMsg, string action = null)
        {
            try
            {
                return DataAccess.IBCDLL.Accounting.Settle.RefundSoccerFGLG(custID, matchID, ref errMsg, action);

            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return false;
            }
            return false;
        }

        public static bool RefundSoccerAllLastGoal(int custID, int matchID, ref string errMsg, string action = null)
        {
            try
            {
                return DataAccess.IBCDLL.Accounting.Settle.RefundSoccerAllLastGoal(custID, matchID, ref errMsg, action);
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
            return false;
        }

        public static bool Multiple(int custID, int matchID)
        {
            try
            {
                DataAccess.IBCDLL.Accounting.Settle.Multiple(custID, matchID);
                return true;
            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
            return false;
        }

        public static ErrorDetail Multiple1(int custID, string matchID)
        {
            try
            {
                return DataAccess.IBCDLL.Accounting.Settle.Multiple1(custID, matchID);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new ErrorDetail()
                {
                    ErrorCode = 1,
                    ErrorMessage = ex.Message
                };
            }
        }

        public static ErrorDetail RefundMultiple(int custID, int matchID)
        {
            try
            {
                return DataAccess.IBCDLL.Accounting.Settle.RefundMultiple(custID, matchID);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new ErrorDetail()
                {
                    ErrorCode = 1,
                    ErrorMessage = ex.Message
                };
            }
        }

        public static ErrorDetail RefundMultipleHT(int custID, int matchID)
        {
            try
            {
                return DataAccess.IBCDLL.Accounting.Settle.RefundMultipleHT(custID, matchID);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new ErrorDetail()
                {
                    ErrorCode = 1,
                    ErrorMessage = ex.Message
                };
            }
        }

        public static ErrorDetail RefundMultipleFT(int custID, int matchID)
        {
            try
            {
                return DataAccess.IBCDLL.Accounting.Settle.RefundMultipleFT(custID, matchID);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return new ErrorDetail()
                {
                    ErrorCode = 1,
                    ErrorMessage = ex.Message
                };
            }
        }

        public static bool Settlement_BetDaq_Move(int matchid, ref string errmsg, ref string NewMatchcode)
        {
            bool f = false;
            try
            {
                var rs = DataAccess.IBCDLL.Accounting.Settle.Settlement_BetDaq_Move(matchid);
                if (rs.ErrorCode > 0)
                    errmsg = rs.ErrorMessage;
                else
                {
                    NewMatchcode = rs.ErrCode;//errcode is newmatchode
                    f = true;
                }

            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                errmsg = ex.Message;
            }
            return f;
        }

        public static bool Settlement_BetDaq_Move(int matchid, int newMatchId, ref string errmsg, ref string newMatchcode)
        {
            bool f = false;
            try
            {
                var rs = DataAccess.IBCDLL.Accounting.Settle.Settlement_BetDaq_Move(matchid, newMatchId);
                if (rs.ErrorCode > 0)
                    errmsg = rs.ErrorMessage;
                else
                {
                    newMatchcode = rs.ErrCode;//errcode is newmatchode
                    f = true;
                }
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                errmsg = ex.Message;
            }
            return f;
        }

        public static bool Settlement_BetFair_Move(int matchid, ref string errmsg, ref string NewMatchcode)
        {
            bool f = false;
            try
            {
                var rs = DataAccess.IBCDLL.Accounting.Settle.Settlement_BetFair_Move(matchid);
                if (rs.ErrorCode > 0)
                    errmsg = rs.ErrorMessage;
                else
                {
                    NewMatchcode = rs.ErrCode;//errcode is newmatchode
                    f = true;
                }

            }
            catch (Exception ex)
            {

                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                errmsg = ex.Message;
            }
            return f;
        }

        public static bool Settlement_BetFair_Move(int matchid, int newMatchId, ref string errmsg, ref string newMatchcode)
        {
            bool f = false;
            try
            {
                var rs = DataAccess.IBCDLL.Accounting.Settle.Settlement_BetFair_Move(matchid, newMatchId);
                if (rs.ErrorCode > 0)
                    errmsg = rs.ErrorMessage;
                else
                {
                    newMatchcode = rs.ErrCode;//errcode is newmatchode
                    f = true;
                }
            }
            catch (Exception ex)
            {
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                errmsg = ex.Message;
            }
            return f;
        }
    }
}
