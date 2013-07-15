using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Sunplus.Accounting.Core.Extensions;
using Fanex.Tarus.Apollo.Settlement.DataAccess;
using Fanex.Tarus.Apollo.Settlement.Entity;
using System.Web;

namespace Fanex.Tarus.Apollo.Settlement.Business
{
    public class SettlementService : ISettlementService
    {
        ISettlementAccess _access;
        public SettlementService()
        {
            _access = new SettlementAccess();
        }

        public DataTable SelectMatchForBill(string matchId)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = _access.SelectMatchForBill(matchId);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dt;
        }


        public ActionResult Set_ConfirmedAction(string matchIds, int action)
        {
            ActionResult result = new ActionResult();
            try
            {
                if (action == 5)//settle for parlay
                {
                    result = _access.Set_ConfirmedAction_Parlay(matchIds);
                    if (result.ErrCode == 0) {
                        result.ErrMsg = "Set_ConfirmedActionParlay for matchid " + matchIds + " to apollo DB successfully."; 
                    }
                }
                else//refund for parlay
                {
                    result = _access.Set_ConfirmedAction(matchIds, action, 0, 0, 0, 0, string.Empty, string.Empty);
                    if (result.ErrCode == 0)
                    {
                        result.ErrMsg = "Set_ConfirmedAction for matchid " + matchIds + " to apollo DB successfully."; 
                    }
                }
                
            }
            catch (Exception ex)
            {
                result = new ActionResult() { 
                    ErrCode = 100,
                    ErrMsg = ex.Message
                };

                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }


        private int ParseNInt(string val)
        {
            if (string.IsNullOrEmpty(val))
                val = "0";

            int i;
            int.TryParse(val, out i);
            return i;
        } 

        public void CalculationParams(int sportID, HttpContext hc, ref int homeHT, ref int awayHT, ref int homeFT, ref int awayFT, ref string betStatus, bool isScoreDirect, bool isHTCal)
        {
            if (sportID == 1) // soccer 
            {
                homeHT = ParseNInt(hc.Request.Form["homeH"]);
                awayHT = ParseNInt(hc.Request.Form["awayH"]);
                homeFT = ParseNInt(hc.Request.Form["homeF"]);
                awayFT = ParseNInt(hc.Request.Form["awayF"]);


                // betStatus = "FL:" + (hc.Request.Form["chkFg"] ?? "0") + "-" + (hc.Request.Form["chkLg"] ?? "0");            
            }
            else if (sportID == 2 || sportID == 3 || sportID == 26 || sportID == 32) // Basketball, football, rugby, netball
            {
                if (isScoreDirect)
                {
                    homeHT = ParseNInt(hc.Request.Form["homeH"]);
                    awayHT = ParseNInt(hc.Request.Form["awayH"]);
                    homeFT = ParseNInt(hc.Request.Form["homeF"]);
                    awayFT = ParseNInt(hc.Request.Form["awayF"]);
                }
                else
                {
                    int _home = 0, _away = 0;
                    for (int i = 1; i < 5; i++)
                    {
                        _home = ParseNInt(hc.Request.Form["txt" + i.ToString() + "qHome"]);
                        _away = ParseNInt(hc.Request.Form["txt" + i.ToString() + "qAway"]);
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
                        int _otHome = ParseNInt(hc.Request.Form["txtOTHome"]);
                        int _otAway = ParseNInt(hc.Request.Form["txtOTAway"]);
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
                    homeFT = ParseNInt(hc.Request.Form["homeF"]);
                    awayFT = ParseNInt(hc.Request.Form["awayF"]);
                }
                else
                {
                    int _home = 0, _away = 0;
                    for (int i = 1; i < 4; i++)
                    {
                        _home = ParseNInt(hc.Request.Form["txt" + i.ToString() + "pHome"]);
                        _away = ParseNInt(hc.Request.Form["txt" + i.ToString() + "pAway"]);
                        betStatus += (!string.IsNullOrEmpty(betStatus) ? "," : "") + _home.ToString() + "-" + _away.ToString();
                        homeFT += _home;
                        awayFT += _away;
                    }
                    if (!string.IsNullOrEmpty(hc.Request.Form["txtOTHome"]) || !string.IsNullOrEmpty(hc.Request.Form["txtOTAway"]))
                    {
                        int _otHome = ParseNInt(hc.Request.Form["txtOTHome"]);
                        int _otAway = ParseNInt(hc.Request.Form["txtOTAway"]);
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
                    homeFT = ParseNInt(hc.Request.Form["txtFSHome"]);
                    awayFT = ParseNInt(hc.Request.Form["txtFSAway"]);
                }
                else
                {
                    int _home = 0, _away = 0;
                    for (int i = 1; i < 6; i++)
                    {
                        _home = ParseNInt(hc.Request.Form["txt" + i.ToString() + "sHome"]);
                        _away = ParseNInt(hc.Request.Form["txt" + i.ToString() + "sAway"]);
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
                    homeFT = ParseNInt(hc.Request.Form["txtTSHome"]);
                    awayFT = ParseNInt(hc.Request.Form["txtTSAway"]);
                }
                else
                {
                    int _home = 0, _away = 0;
                    for (int i = 1; i < 6; i++)
                    {
                        _home = ParseNInt(hc.Request.Form["txt" + i.ToString() + "sHome"]);
                        _away = ParseNInt(hc.Request.Form["txt" + i.ToString() + "sAway"]);
                        betStatus += (!string.IsNullOrEmpty(betStatus) ? "," : "") + _home.ToString() + "-" + _away.ToString();
                        if (_home > _away) homeFT++;
                        if (_home < _away) awayFT++;
                    }
                }
            }
            else if (sportID == 7 || sportID == 10 || sportID == 11 || sportID == 29) // Billiard, Golf, Motorsports, Winter Sports
            {
                homeFT = ParseNInt(hc.Request.Form["homeF"]);
                awayFT = ParseNInt(hc.Request.Form["awayF"]);
            }
            else if (sportID == 8) // Baseball       
            {
                if (isScoreDirect)
                {
                    homeHT = ParseNInt(hc.Request.Form["homeH"]);
                    awayHT = ParseNInt(hc.Request.Form["awayH"]);
                    homeFT = ParseNInt(hc.Request.Form["homeF"]);
                    awayFT = ParseNInt(hc.Request.Form["awayF"]);
                }
                else
                {
                    int _home = 0, _away = 0;
                    for (int i = 1; i < 10; i++)
                    {
                        _home = ParseNInt(hc.Request.Form["txt" + i.ToString() + "Home"]);
                        _away = ParseNInt(hc.Request.Form["txt" + i.ToString() + "Away"]);
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
                        int _eiHome = ParseNInt(hc.Request.Form["txtEIHome"]);
                        int _eiAway = ParseNInt(hc.Request.Form["txtEIAway"]);
                        if (!isHTCal)
                            betStatus += "," + _eiHome.ToString() + "-" + _eiAway.ToString();
                        homeFT += _eiHome;
                        awayFT += _eiAway;
                    }
                }
            }
            else // Financial, OtherSports
            {
                homeHT = ParseNInt(hc.Request.Form["homeH"]);
                awayHT = ParseNInt(hc.Request.Form["awayH"]);
                homeFT = ParseNInt(hc.Request.Form["homeF"]);
                awayFT = ParseNInt(hc.Request.Form["awayF"]);
            }
        }

        public ActionResult Set_ConfirmedAction_Single(string matchId, int action, int fth, int fta, int hth, int hta, string goalrecord, string betstatus)
        {
            ActionResult result = new ActionResult();
            try
            {
                if (action == 13)//refund All
                {
                    betstatus = "Refund;FL:2-0";
                }

                result = _access.Set_ConfirmedAction(matchId, action, fth, fta, hth, hta, goalrecord, betstatus);
                if (result.ErrCode == 0)
                {
                    result.ErrMsg = "Set_ConfirmedAction for matchid " + matchId + " to apollo DB successfully."; 
                }
            }
            catch (Exception ex)
            {
                result = new ActionResult()
                {
                    ErrCode = 100,
                    ErrMsg = ex.Message
                };

                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public ActionResult Set_ConfirmedAction_Parlay(string matchIds)
        {
            ActionResult result = new ActionResult();
            try
            {
                result = _access.Set_ConfirmedAction_Parlay(matchIds);
                if (result.ErrCode == 0)
                {
                    result.ErrMsg = "Set_ConfirmedAction for matchid " + matchIds + " to apollo DB successfully.";
                }
            }
            catch (Exception ex)
            {
                result = new ActionResult()
                {
                    ErrCode = 100,
                    ErrMsg = ex.Message
                };

                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }


        public int ParseActionType(string type)
        {
            var rs = 0;
            switch (type) {
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
    }
}
