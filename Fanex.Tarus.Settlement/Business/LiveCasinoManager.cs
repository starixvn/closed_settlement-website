using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Settlement.Entity;
using Fanex.Tarus.Settlement.DataAccess;
using System.Data;
using DNAide.Core;
using LVCasinoSetting;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Fanex.Tarus.Settlement.Core.Common;
using Fanex.Tarus.Settlement.Core.Cache;

namespace Fanex.Tarus.Settlement.Business
{
    public class LiveCasinoManager : BaseSettlement, ILiveCasinoManager
    {
        private ILiveCasinoDataAccess _liveCasinoAccess;
        private bool IsHadHostPermission = true;
        public LiveCasinoManager()
            : this(null)
        {
            
            SettingMgr.GetInstance().Initialize(SiteEnum.AgentSite);
        }

        public LiveCasinoManager(ILiveCasinoDataAccess liveCasinoDataAccess)
        {
            _liveCasinoAccess = liveCasinoDataAccess ?? new LiveCasinoDataAccess();
            
        }

        public string GetMatchesHtml(string fdate, string tdate, string LeagueIds, int statusId)
        {
            var html = string.Empty;
            try
            {
                html = RenderMatchesHTML(GetMatches(fdate, tdate, LeagueIds, statusId));
                if (string.IsNullOrEmpty(html))
                {
                    html = "<tr><td><div style='text-align:center;width:961px'>There is no information!</div></td></tr>";
                }
            }
            catch (Exception ex)
            {
                html = "<tr><td><div style='text-align:center;width:961px'>" + ex.Message + "</div></td></tr>";
            }

            return html;
        }

        public List<League> GetMatches(string fdate, string tdate, string LeagueIds, int statusId)
        {
            int LeagueId = 0;
            if (!LeagueIds.Contains(",") && LeagueIds != "") LeagueId = DNAide.Core.CastAide.AsInt32(LeagueIds);
            var ds = _liveCasinoAccess.GetMatches(fdate, tdate, LeagueId, statusId);

            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count <= 0)
                return new List<League>();

            var leagues = new List<League>();
            var listLeagueId = new List<int>();
            foreach (string lid in LeagueIds.Split(new char[] { ',' }))
            {
                if (string.IsNullOrEmpty(lid)) continue;
                listLeagueId.Add(DNAide.Core.CastAide.AsInt32(lid));
            }

            decimal index = 0;
            foreach (int _leagueid in listLeagueId)
            {
                var league = new League();
                league.LeagueId = _leagueid;
                var matches = new List<Match>();
                
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (_leagueid != CastAide.AsInt32(row["leagueid"])) continue;
                    ++index;
                    var kickOffTime = CastAide.AsDateTime(row["kickofftime"]);
                    var eventDate = CastAide.AsDateTime(row["eventdate"]);
                    var eventStatus = CastAide.AsString(row["eventstatus"]);
                    var betstatus = CastAide.AsString(row["betstatus"]);
                    bool isEnableCalc = false;
                    bool isShowFinalScore = !(eventStatus.Trim().ToLower() == "refund" && string.IsNullOrEmpty(betstatus));

                    //if (eventStatus.Trim().ToLower() == "completed" || (eventStatus.Trim().ToLower() == "refund" && string.IsNullOrEmpty(betstatus)))
                    //{
                    if (eventDate.Day == DateTime.Now.Day && eventDate.Month == DateTime.Now.Month && eventDate.Year == DateTime.Now.Year) isEnableCalc = true;
                    //}

                    var bettypeId = CastAide.AsInt32(row["bettype"]);
                    var betResults = (bettypeId == (int)Enumerations.Bettype.Routle && string.IsNullOrEmpty(betstatus)) ? "-" : ParseResult(CastAide.AsString(row["betstatus"]), bettypeId);

                    matches.Add(new Match
                    {
                        LeagueId = CastAide.AsInt32(row["leagueid"]),
                        MatchId = CastAide.AsInt32(row["matchid"]),
                        MatchCode = CastAide.AsString(row["matchcode"]),
                        EventDate = eventDate.ToShortDateString(),
                        KickOffTime = kickOffTime.ToString("MM/dd/yyyy hh:mm:ss tt"),
                        ShowTime = CastAide.AsString(row["gameevent"]),
                        FinalHomeScore = isShowFinalScore ? CastAide.AsString(row["finalhomescore"]) : string.Empty,
                        FinalAwayScore = isShowFinalScore ? CastAide.AsString(row["finalawayscore"]) : string.Empty,
                        BetStatus = betResults,
                        EventStatus = UppercaseFirst(eventStatus),
                        EnableCalculate = isEnableCalc,
                        BetType = bettypeId,
                        Index = index
                    });
                }

                if (matches.Count == 0) continue;

                league.Matches = matches;
                league.LeagueName = CacheRef.GetLeagueName(league.LeagueId) + " (" + DNAide.Utils.NumberFormat.DecFormat(matches.Count,0) + ")";
                leagues.Add(league);
            }
            return leagues;
        }

        private string RenderMatchesHTML(List<League> leagues)
        {
            StringBuilder sb = new StringBuilder();
            foreach (League league in leagues)
            {
                RenderLeagueRow(sb, league);
                if (league.Matches != null)
                {
                    foreach (Match match in league.Matches)
                    {
                        RenderMatchRow(sb, match);
                    }
                }
            }
            return sb.ToString();
        }

        private void RenderLeagueRow(StringBuilder sb, League league)
        {
            string template = @"<tr><td colspan='3' style='font-weight:bold; color:black;background-color:#E1EFFA'>
                                        ${LeagueName}
                                    </td><td style='font-weight:bold; color:black;text-align:center;background-color:#E1EFFA'>
                                        ${FinalScore}
                                    </td><td colspan='4' style=';background-color:#E1EFFA'>
                                    </td>
                                </tr>";
            template = template.Replace("${LeagueName}", league.LeagueName);
            string FinalScore = string.Empty;
            if (league.LeagueName.ToLower().Contains("baccarat"))
            {
                FinalScore = "(P - B)";
            }
            template = template.Replace("${FinalScore}", FinalScore);
            sb.Append(template);
        }

        private void RenderMatchRow(StringBuilder sb, Match match)
        {
            string template = @"<tr ${Background}><td>
                                        <div class='col140 c'>
                                            <table width='100%' border='0' cellpadding='0' cellspacing='0'>
                                                <tr><td style='border:0px'>
                                                        ${MatchCode}
                                                    </td>            
                                                </tr><tr><td style='border:0px'>
                                                        <div class='viewhis' onclick='settlelc.showLog(" + Convert.ToChar(34) + match.MatchCode + Convert.ToChar(34) + @");'></div>
                                                    </td>            
                                                </tr>
                                            </table>
                                            
                                        </div>
                                    </td><td>
                                        <div class='col140 c'>
                                            ${KickOffTime}
                                        </div>
                                    </td><td>
                                        <div class='col200 l b'>
                                            ${ShowTime}
                                        </div>
                                    </td><td>
                                        <div class='col80 c'>
                                            ${finalscore}
                                        </div>
                                    </td><td>
                                        <div class='col120 c'>
                                            ${BetStatus}
                                        </div>
                                    </td><td>
                                        <div class='col100 c'>
                                            ${EventStatus}
                                        </div>
                                    </td><td>
                                        <div class='col80 c'>
                                            ${Calculate}
                                        </div>
                                    </td><td>
                                        <div class='col80 c'>
                                            ${Refund}
                                        </div>
                                    </td>
                                </tr>";
            if (match.Index % 2 != 0)
            {
                template = template.Replace("${Background}", "style='background-color:#DFEAFF'");
            }
            else
            {
                template = template.Replace("${Background}", string.Empty);
            }

            if (match.BetType == (int)Enumerations.Bettype.Baccarat)
            {
                template = template.Replace("${finalscore}", "<span>${FinalHomeScore} - ${FinalAwayScore}</span>");

            }
            else
            {
                template = template.Replace("${finalscore}", "<span>${FinalHomeScore}</span>");
            }

            template = template.Replace("${MatchCode}", match.MatchCode);
            template = template.Replace("${Background}", match.MatchCode);
            template = template.Replace("${KickOffTime}", match.KickOffTime);
            template = template.Replace("${ShowTime}", match.ShowTime);
            template = template.Replace("${FinalHomeScore}", match.FinalHomeScore);
            template = template.Replace("${FinalAwayScore}", match.FinalAwayScore);
            template = template.Replace("${BetStatus}", string.IsNullOrEmpty(match.BetStatus) ? "-" : match.BetStatus);
            template = template.Replace("${EventStatus}", match.EventStatus);
            if (match.EnableCalculate && IsHadHostPermission)
            {
                template = template.Replace("${Calculate}", "<span class='canchange' onclick='javascript:settlelc.reCalc(${MatchId}, ${LeagueId}, ${BetType}, \"" + GetCurrentDate() + "\");'>Re-Calc</span>");
            }
            else
            {
                template = template.Replace("${Calculate}", "Re-Calc");
            }

            if (match.EnableCalculate && IsHadHostPermission)
            {
                template = template.Replace("${Refund}", "<span class='canchange' onclick='javascript:settlelc.refund(${MatchId}, ${LeagueId}, ${BetType}, \"" + GetCurrentDate() + "\");'>Refund</span>");
            }
            else
            {
                template = template.Replace("${Refund}", "Refund");
            }
            template = template.Replace("${MatchId}", match.MatchId.ToString());
            template = template.Replace("${LeagueId}", match.LeagueId.ToString());
            template = template.Replace("${BetType}", match.BetType.ToString());
            sb.Append(template);
        }

        private string GetCurrentDate()
        {
            return DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString();
        }

        private string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            if (s.Length == 1) return char.ToUpper(s[0]).ToString();
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public Score GetScore(int MatchId)
        {
            var ds = _liveCasinoAccess.GetLiveCasinoScore(MatchId);

            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count <= 0)
                return null;
            DataRow row = ds.Tables[0].Rows[0];
            var scoreDetails = new List<ScoreDetail>();
            scoreDetails.Add(new ScoreDetail { Name = "BResult2", Value = GetValidScore(CastAide.AsString(row["b_result2"])) });
            scoreDetails.Add(new ScoreDetail { Name = "BResult4", Value = GetValidScore(CastAide.AsString(row["b_result4"])) });
            scoreDetails.Add(new ScoreDetail { Name = "BResult6", Value = GetValidScore(CastAide.AsString(row["b_result6"])) });
            scoreDetails.Add(new ScoreDetail { Name = "PResult1", Value = GetValidScore(CastAide.AsString(row["p_result1"])) });
            scoreDetails.Add(new ScoreDetail { Name = "PResult3", Value = GetValidScore(CastAide.AsString(row["p_result3"])) });
            scoreDetails.Add(new ScoreDetail { Name = "PResult5", Value = GetValidScore(CastAide.AsString(row["p_result5"])) });
            return new Score
            {
                MatchId = CastAide.AsInt32(row["matchid"]),
                ScoreDetails = scoreDetails
            };
        }

        private string GetValidScore(string Value)
        {
            if (string.IsNullOrEmpty(Value)) return string.Empty;
            if (Value.Length < 2) return string.Empty;
            if (Value.ToUpper() == "SA") Value = "S1";
            else if (Value.ToUpper() == "HA") Value = "H1";
            else if (Value.ToUpper() == "CA") Value = "C1";
            else if (Value.ToUpper() == "DA") Value = "D1";
            else if (Value.ToUpper() == "S1") Value = "S0";
            else if (Value.ToUpper() == "H1") Value = "H0";
            else if (Value.ToUpper() == "C1") Value = "C0";
            else if (Value.ToUpper() == "D1") Value = "D0";
            return Value;
        }

        private string GetCardResult(string BankerCard, string PlayerCard)
        {
            string result = string.Empty;
            string[] arrPlayerCard = PlayerCard.Split(',');
            string[] arrBankerCard = BankerCard.Split(',');
            result += arrPlayerCard[0];
            result += "," + arrBankerCard[0];
            result += "," + arrPlayerCard[1];
            result += "," + arrBankerCard[1];
            if (arrPlayerCard.Length > 2) result += "," + arrPlayerCard[2]; else result += ",-";
            if (arrBankerCard.Length > 2) result += "," + arrBankerCard[2]; else result += ",-";
            return result;
        }

        public string ReCalc(int MatchID, int IBCBetTypeID, string BankerCard, string PlayerCard, int AdminID, string AdminName, string IP, string ShowDate)
        {
            if (IBCBetTypeID == (int)Enumerations.Bettype.Baccarat)
                return ReCalcBaccarat(MatchID, IBCBetTypeID, BankerCard, PlayerCard, AdminID, AdminName, IP, ShowDate);
            else
                return ReCalcRoutle(MatchID, IBCBetTypeID, BankerCard, PlayerCard, AdminID, AdminName, IP, ShowDate);
        }
        public string ReCalcBaccarat(int MatchID, int IBCBetTypeID, string BankerCard, string PlayerCard, int AdminID, string AdminName, string IP, string ShowDate)
        {
            try
            {
                if (!IsHadHostPermission) return ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500");

                //Check is current day
                if (ShowDate != null)
                {
                    var arrShowDate = ShowDate.Split('-');
                    if (arrShowDate.Length == 3)
                    {
                        var showDate = new DateTime(CastAide.AsInt32(arrShowDate[0]), CastAide.AsInt32(arrShowDate[1]), CastAide.AsInt32(arrShowDate[2]));
                        if (showDate.Year != DateTime.Now.Year || showDate.Month != DateTime.Now.Month || showDate.Day != DateTime.Now.Day)
                        {
                            return ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "match04"); ;
                        }
                    }
                }

                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("MatchID", MatchID.ToString());
                param.Add("IBCBetTypeID", IBCBetTypeID.ToString());
                param.Add("BankerCard", BankerCard);
                param.Add("PlayerCard", PlayerCard);
                param.Add("AdminID", AdminID.ToString());
                param.Add("AdminName", AdminName);
                param.Add("GameTypeNo", Enumerations.GameType.Baccarat);
                //Update result/bet tickets Game Server by API
                LVAPILibrary.IBCMgr.ReSettlement(param);

                int Banker = DNAide.Core.CastAide.AsInt32(param["Banker"]);
                int Player = DNAide.Core.CastAide.AsInt32(param["Player"]);
                string IBCResultID = DNAide.Core.CastAide.AsString(param["IBCResultID"]);
                decimal CasinoCommRate = DNAide.Core.CastAide.AsDecimal(param["CasinoCommRate"]);
                //update match status in IBC DB
                bool resultUpdateStatus = _liveCasinoAccess.UpdateStatus(MatchID, Banker, Player, IBCResultID, IBCBetTypeID);

                if (!resultUpdateStatus)
                {
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "match01");
                }

                //call Settlement in IBC DB
                string msgReCal = _liveCasinoAccess.RefundAtIBCDB(MatchID, "CAL", AdminID, IBCResultID, CasinoCommRate, IP, GetCardResult(BankerCard, PlayerCard));
                if (!string.IsNullOrEmpty(msgReCal))
                {
                    if (msgReCal.Equals("Error"))
                        return ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "match02");
                    else
                        return msgReCal;
                }

                //Call API to check the result between LiveCasio and IBC DBs
                param = new Dictionary<string, string>();
                param.Add("Matchid", MatchID.ToString());
                param.Add("AdminID", AdminID.ToString());
                param.Add("AdminName", AdminName);
                try
                {
                    LVAPILibrary.IBCMgr.CheckSettle(param);
                }
                catch (LVAPILibrary.Model.UserException.ErrorException e)
                {
                    return e.ErrorCode + ": " + GetMsgError(e.ErrorCode);
                }
                return string.Empty;
            }
            catch (LVAPILibrary.Model.UserException.ErrorException e)
            {
                return e.ErrorCode + ": " + GetMsgError(e.ErrorCode);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return ex.Message;
            }
        }
        public string ReCalcRoutle(int MatchID, int IBCBetTypeID, string BankerCard, string PlayerCard, int AdminID, string AdminName, string IP, string ShowDate)
        {
            try
            {
                if (!IsHadHostPermission) return ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500");

                //Check is current day
                if (ShowDate != null)
                {
                    var arrShowDate = ShowDate.Split('-');
                    if (arrShowDate.Length == 3)
                    {
                        var showDate = new DateTime(CastAide.AsInt32(arrShowDate[0]), CastAide.AsInt32(arrShowDate[1]), CastAide.AsInt32(arrShowDate[2]));
                        if (showDate.Year != DateTime.Now.Year || showDate.Month != DateTime.Now.Month || showDate.Day != DateTime.Now.Day)
                        {
                            return ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "match04"); ;
                        }
                    }
                }

                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("MatchID", MatchID.ToString());
                param.Add("IBCBetTypeID", IBCBetTypeID.ToString());
                param.Add("BankerCard", string.Empty);
                param.Add("PlayerCard", PlayerCard);
                param.Add("AdminID", AdminID.ToString());
                param.Add("AdminName", AdminName);
                param.Add("GameTypeNo", Enumerations.GameType.Routle);
                //Update result/bet tickets Game Server by API
                LVAPILibrary.IBCMgr.ReSettlement(param);
                                
                decimal CasinoCommRate = DNAide.Core.CastAide.AsDecimal(param["CasinoCommRate"]);
                //update match status in IBC DB
                bool resultUpdateStatus = _liveCasinoAccess.UpdateStatus(MatchID, 0, CastAide.AsInt32(PlayerCard), string.Empty, IBCBetTypeID);

                if (!resultUpdateStatus)
                {
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "match01");
                }

                //call Settlement in IBC DB
                string msgReCal = _liveCasinoAccess.RefundAtIBCDB(MatchID, "CAL", AdminID, PlayerCard, CasinoCommRate, IP, string.Empty);

                if (!string.IsNullOrEmpty(msgReCal))
                {
                    if (msgReCal.Equals("Error"))
                        return ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "match02");
                    else
                        return msgReCal;
                }

                //Call API to check the result between LiveCasio and IBC DBs
                param = new Dictionary<string, string>();
                param.Add("Matchid", MatchID.ToString());
                param.Add("AdminID", AdminID.ToString());
                param.Add("AdminName", AdminName);
                try
                {
                    LVAPILibrary.IBCMgr.CheckSettle(param);
                }
                catch (LVAPILibrary.Model.UserException.ErrorException e)
                {
                    return e.ErrorCode + ": " + GetMsgError(e.ErrorCode);
                }
                return string.Empty;
            }
            catch (LVAPILibrary.Model.UserException.ErrorException e)
            {
                return e.ErrorCode + ": " + GetMsgError(e.ErrorCode);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return ex.Message;
            }
        }

        private string GetMsgError(string Code)
        {
            switch (Code.Trim())
            {
                case "E0001":
                    return "MatchID is required";
                case "E0002":
                    return "MatchID not in Game";
                case "E0003":
                    return "BankerCard is required";
                case "E0004":
                    return "PlayerCard is required";
                case "E0005":
                    return "Only completed game can be resettled";
                case "E0006":
                    return "GameID is required";
                case "E0007":
                    return "IBCBetTypeID is required";
                case "E0008":
                    return "Wrong card data";
                case "E0009":
                    return "IBCBetTypeID XXX not in GameType";
                default:
                    return string.Empty;
            }
        }

        public override Match GetMatch(int MatchId, int bettypeId)
        {
            var ds = _liveCasinoAccess.GetMatch(MatchId);

            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count <= 0)
                return null;
            DataRow row = ds.Tables[0].Rows[0];
            var kickOffTime = CastAide.AsDateTime(row["kickofftime"]);
            var eventDate = CastAide.AsDateTime(row["eventdate"]);
            var eventStatus = CastAide.AsString(row["eventstatus"]);
            var finalHomeScore = CastAide.AsString(row["finalhomescore"]);
            var betStatusResult = finalHomeScore == "0" && bettypeId == (int)Enumerations.Bettype.Routle ? "-" : ParseResult(CastAide.AsString(row["betstatus"]), bettypeId);
            return new Match
            {
                LeagueId = CastAide.AsInt32(row["leagueid"]),
                MatchId = CastAide.AsInt32(row["matchid"]),
                MatchCode = CastAide.AsString(row["matchcode"]),
                EventDate = eventDate.ToShortDateString(),
                KickOffTime = kickOffTime.ToString("MM/dd/yyyy hh:mm:ss tt"),
                ShowTime = CastAide.AsString(row["gameevent"]),
                FinalHomeScore = finalHomeScore,
                FinalAwayScore = CastAide.AsString(row["finalawayscore"]),
                BetStatus = betStatusResult,
                EventStatus = UppercaseFirst(eventStatus),
                IsSettled = CastAide.AsBoolean(row["issettled"]),
                FTStatus = CastAide.AsString(row["ftstatus"])
            };
        }

        private string ParseResult(string BetStatus, int bettypeId)
        {
            if (string.IsNullOrEmpty(BetStatus)) return string.Empty;

            string[] arr = BetStatus.Split(',');
            var rs = new StringBuilder();
            var index = 0;
            foreach (string s in arr)
            {                
                rs.Append(string.Format("{0}{1}", CacheRef.GetLiveCasinoResult(CastAide.AsInt32(s.Trim()), bettypeId), (index == 2 && bettypeId == (int)Fanex.Tarus.Settlement.Core.Common.Enumerations.Bettype.Routle ? "; <br>" : "; ")));
                index++;
            }
            var result = rs.ToString();
            return result.Remove(result.Length - 2, 2);
        }

        public string RefundAtLiveCasioDB(int MatchID, int AdminID, string AdminName, string IP, string ShowDate)
        {
            try
            {
                if (!IsHadHostPermission) return ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "_500");

                //Check is current day
                if (ShowDate != null)
                {
                    var arrShowDate = ShowDate.Split('-');
                    if (arrShowDate.Length == 3)
                    {
                        var showDate = new DateTime(CastAide.AsInt32(arrShowDate[0]), CastAide.AsInt32(arrShowDate[1]), CastAide.AsInt32(arrShowDate[2]));
                        if (showDate.Year != DateTime.Now.Year || showDate.Month != DateTime.Now.Month || showDate.Day != DateTime.Now.Day)
                        {
                            return ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "match05"); ;
                        }
                    }
                }

                //Update status/bet tickets Live Casino DB by SPs
                var msg = _liveCasinoAccess.RefundAtLiveCasioDB(MatchID, AdminID, AdminName);
                if (!string.IsNullOrEmpty(msg)) return msg;
                //Update status/bet tickets in IBC DB
                msg = _liveCasinoAccess.RefundAtIBCDB(MatchID, "Refund", AdminID, "refund", 0, IP, string.Empty);
                if (msg.Equals("Error"))
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "match03");
                else
                    return msg;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                return AlertMessage.GetMsg(ex.Message, AlertMessage.MsgType.Error);
            }
        }
    }
}
