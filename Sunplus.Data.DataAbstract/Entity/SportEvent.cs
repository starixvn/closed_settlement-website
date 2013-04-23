using System;
using DNAide.Core;
using Sunplus.Data.DataAbstract.Utils;
using DNAide.Utils;


namespace Sunplus.Data.DataAbstract.Entity
{
    public class SportEvent
    {
        private readonly Bettran _bettran = null;

        public SportEvent(Bettran bettran)
        {
            _bettran = bettran;
        }

        public void GetSportEventVoidStatus(ref SportEventDTO sportEvent)
        {
            if (_bettran.StatusId == 102)
            {
                sportEvent.Choice = string.Format("<s>{0}</s>", sportEvent.Choice);
                sportEvent.BetType = string.Format("<s>{0}</s>", sportEvent.BetType);
                sportEvent.Event = string.Format("<s>{0}</s>", sportEvent.Event);
                sportEvent.SportType = string.Format("<s>{0}</s>", sportEvent.SportType);
                sportEvent.LeagueName = string.Format("<s>{0}</s>", sportEvent.LeagueName);
            }
        }

        public  SportEventDTO GetSportEvent()
        {
            var sportEvent = new SportEventDTO();
            switch (_bettran.Bettype)
            {
                case 1:
                case 7:
                case 20:
                case 22: //Next Goal bet type
                case 21: //Next Goal bet type
                case 124://FT 1x2 AH  
                case 125://1st 1x2 AH
                    AssignHdp(ref sportEvent);
                    break;
                case 2:
                case 12:
                    AssignOE(ref sportEvent);
                    break;
                case 3:
                case 8:
                    AssignOU(ref sportEvent);
                    break;
                case 4:
                case 30:
                    AssignCS(ref sportEvent);
                    break;
                case 5:
                    Assign1x2(ref sportEvent, true);
                    break;
                case 15:
                    Assign1x2(ref sportEvent, false);
                    break;
                case 6:
                case 14:
                case 16:
                    AssignTG(ref sportEvent);
                    break;
                case 10:
                    AssignOutRight(ref sportEvent);
                    break;
                //Phuong.Nguyen, HR
                case 31:
                case 32:
                case 33:
                case 41:
                case 42:
                case 43:
                    //GenHREvent(ref eventItem);
                    break;
                case 9:
                    AssignMP(ref sportEvent);
                    break;
                case 29:
                    AssignCombinationMP(ref sportEvent);
                    break;
                case 51: //Betting of Finance
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                case 66:
                    //_home = CacheRef.GetFncIndexName(row["homeid"]);
                    //GenFinancialRow(ref eventItem);
                    break;
                case 71:
                case 72:
                case 73:
                case 112:
                case 113:
                case 114:
                    //GenCasino(ref eventItem, bettype, CastAide.AsString(row["betid"]));
                    //GenCasino(ref eventItem, _bettype, _transId);
                  
                case 81://Number Game
                    Assign1stBallOU(ref sportEvent);
                    break;
                case 82:
                    AssignLastBallOU(ref sportEvent);
                    break;
                case 83:
                    Assign1stBallOE(ref sportEvent);
                    break;
                case 84:
                    AssignLastBallOE(ref sportEvent);
                    break;
                case 85:
                    AssignOverUnder(ref sportEvent);
                    break;
                case 86:
                    AssignOddEven(ref sportEvent);
                    break;
                case 90:
                    //GenEvent_NumberGame(ref eventItem, listType, false);
                    AssignNextHighLow(ref sportEvent);
                    break;
                case 88:
                    AssignWarrior(ref sportEvent);
                    break;
                case 89:
                    AssignNextCombo(ref sportEvent);
                    break;
                case 24:
                    AssignDoubleChance(ref sportEvent);
                    break;
                case 25:
                    AssignDrawNoBet(ref sportEvent);
                    break;
                case 26:
                    AssignBothOneNeither(ref sportEvent);
                    break;
                case 27:
                    AssignToWinToNil(ref sportEvent);
                    break;
                case 13:
                    AssignCleanSheet(ref sportEvent);
                    break;
                case 28:
                    AssignThreeWayHandicap(ref sportEvent);
                    break;
                default:
                    break;
            }
            //sportEvent.MoreBetInfo = oddMP(CastAide.AsInt32(_bettran.Odds));
            return sportEvent;
        }

        private  void AssignHdp(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";
            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);
            sportEvent.Choice = _bettran.Betteam == "h" ? home : away;
            if (_bettran.Bettype == 20)
            {
                sportEvent.LiveScore = string.Empty;
                sportEvent.Hdp = string.Empty;
            }
            else
            {
                sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";
                if (_bettran.Betteam == "h")
                {
                    sportEvent.Hdp = "&nbsp;" + FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.Hdp2) - CastAide.AsDecimal(_bettran.Hdp1), "en-EN");
                }
                else
                {
                    sportEvent.Hdp = "&nbsp;" + FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2), "en-EN");
                }
            }


            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />" + home + " -VS- " + away;
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private  void AssignOE(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";
            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);

            sportEvent.Choice = _bettran.Betteam == "h" ? "ODD" : "EVEN";
            sportEvent.Hdp = "";
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />" + home + " -VS- " + away;
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
           // GetSportEventVoidStatus(ref sportEvent);
        }

        private  void Assign1x2(ref SportEventDTO sportEvent, bool isFT)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";

            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);
            sportEvent.Choice = isFT ? "FT." + _bettran.Betteam : "HT." + _bettran.Betteam;

            //if (listType == 1) { if (isMP) choice += oddMP(); }
            //if ((listType == 2 || listType == 3) && isMP) choice += " <span class='style_12BB'> @ " + IBCUtils.FormatHandler.FormatNumber20(_odds) + "</span>";

            sportEvent.Hdp = "";
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />" + home + " -VS- " + away;
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private  void AssignTG(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";

            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);
            sportEvent.Choice = _bettran.Betteam;

            sportEvent.Hdp = "";
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />" + home + " -VS- " + away;
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private  void AssignCS(ref SportEventDTO sportEvent)
        {
            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);
            sportEvent.Choice = _bettran.Betteam;
            if (_bettran.Betteam == "5:0" || _bettran.Betteam == "0:5")
            {
                _bettran.Betteam += " UP";
            }

            sportEvent.LiveScore = "";
            
            sportEvent.Hdp = ""; 
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />" + home + " -VS- " + away;
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private  void AssignOutRight(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = "";

            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);

            sportEvent.Choice = away;

            sportEvent.Hdp = "";
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "";
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private  void AssignOU(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";
            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);

            if (_bettran.Betteam == "h")
            {
                sportEvent.Choice = "OVER";
            }
            else
            {
                sportEvent.Choice = "UNDER";
            }
            sportEvent.Hdp = FormatHandler.FormatNumber2(Math.Abs(CastAide.AsDecimal(_bettran.Hdp1)),"en-EN");
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />" + home + " -VS- " + away;
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private  void AssignDoubleChance(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";
            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);

            if (_bettran.Betteam.ToLower().Equals("1x"))
                sportEvent.Choice = string.Format("{0} <span class='or'>or</span> Draw", home);
            else if (_bettran.Betteam.ToLower().Equals("2x"))
                sportEvent.Choice = string.Format("{0} <span class='or'>or</span> Draw", away);
            else if (_bettran.Betteam.ToLower().Equals("12"))
                sportEvent.Choice = string.Format("{0} <span class='or'>or</span> {1}", home, away);

            sportEvent.Hdp = FormatHandler.FormatNumber2(Math.Abs(CastAide.AsDecimal(_bettran.Hdp1)), "en-EN");
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />" + home + " -VS- " + away;
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }
        private  void AssignDrawNoBet(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";
            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);

            if (_bettran.Betteam == "h")
            {
                sportEvent.Choice = home;
            }
            else
            {
                sportEvent.Choice = away;
            }

            sportEvent.Hdp = string.Empty;
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />" + home + " -VS- " + away;
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
           // GetSportEventVoidStatus(ref sportEvent);
        }

        private  void AssignBothOneNeither(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";
            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);

            if (_bettran.Betteam.ToLower().Equals("b"))
                sportEvent.Choice = "Both";
            else if (_bettran.Betteam.ToLower().Equals("o"))
                sportEvent.Choice = "One";
            else if (_bettran.Betteam.ToLower().Equals("n"))
                sportEvent.Choice = "No Goal";

            sportEvent.Hdp = string.Empty;
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />" + home + " -VS- " + away;
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private  void AssignToWinToNil(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";
            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);

            if (_bettran.Betteam == "h")
            {
                sportEvent.Choice = home;
            }
            else
            {
                sportEvent.Choice = away;
            }

            sportEvent.Hdp = string.Empty;
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />" + home + " -VS- " + away;
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private  void AssignCleanSheet(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";
            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);

            if (_bettran.Betteam.ToLower().Equals("hy"))
                sportEvent.Choice = string.Format("{0} <span style='color:#000;'>{1}</span>", home, "YES");
            else if (_bettran.Betteam.ToLower().Equals("hn"))
                sportEvent.Choice = string.Format("{0} <span style='color:#000;'>{1}</span>", home, "NO");
            else if (_bettran.Betteam.ToLower().Equals("ay"))
                sportEvent.Choice = string.Format("{0} <span style='color:#000;'>{1}</span>", away, "YES");
            else if (_bettran.Betteam.ToLower().Equals("an"))
                sportEvent.Choice = string.Format("{0} <span style='color:#000;'>{1}</span>", away, "NO");

            sportEvent.Hdp = string.Empty;
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />" + home + " -VS- " + away;
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private  void AssignThreeWayHandicap(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";
            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);
            decimal hdpResult = 0;
            if (_bettran.Betteam.ToLower().Equals("1"))
            {
                hdpResult = (int)(CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2)) > 0 ? -((int)(CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2))) : Math.Abs((int)(CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2)));
                //hdpResult = (int)(CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2) > 0 ? -((int)CastAide.AsDecimal(_bettran.Hdp2) - _hdp1) : Math.Abs(_hdp2 - _hdp1));
                sportEvent.Choice = home;
                //sportEvent.Hdp = "<span style='color:#000;'>" + (hdpResult > 0 ? "+&nbsp;" : string.Empty) + hdpResult + "</span>";
                sportEvent.Hdp = string.Format("<span style='color:#000;'>({0}{1})</span>", (hdpResult > 0 ? "+" : string.Empty), FormatHandler.FormatNumber2(hdpResult, "en-EN"));
            }
            else if (_bettran.Betteam.ToLower().Equals("2"))
            {
                hdpResult = CastAide.AsDecimal(_bettran.Hdp2) - CastAide.AsDecimal(_bettran.Hdp1) > 0 ? -((int)(CastAide.AsDecimal(_bettran.Hdp2) - CastAide.AsDecimal(_bettran.Hdp1))) : Math.Abs((int)(CastAide.AsDecimal(_bettran.Hdp2) - CastAide.AsDecimal(_bettran.Hdp1)));
                sportEvent.Choice = away;
                //sportEvent.Hdp = "<span style='color:#000;'>" + (hdpResult > 0 ? "+&nbsp;" : string.Empty) + hdpResult + "</span>";
                sportEvent.Hdp = string.Format("<span style='color:#000;'>({0}{1})</span>", (hdpResult > 0 ? "+" : string.Empty), FormatHandler.FormatNumber2(hdpResult, "en-EN"));

            }
            else if (_bettran.Betteam.ToLower().Equals("x"))
            {
                sportEvent.Choice = "Draw";
                sportEvent.Hdp = string.Format("<span style='color:#000;'>(<span style='color:#ff0000;'>{0}</span> +{1})</span>",
                    CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2) > 0 ? "A" : "H", Math.Abs((int)(CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2))));
            }

            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />" + home + " -VS- " + away;
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
          
        }
        private  void AssignMP(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = "";
            sportEvent.Choice = "";
            sportEvent.Hdp = "";
            sportEvent.BetType = "<span class='lnk' onclick='LatestBet.showMP(\"" + _bettran.Refno + "\",\"" + _bettran.Transid + "\")' >" + "Mix Parlay</span>";
            sportEvent.Event = "";
            sportEvent.SportType = "";
            sportEvent.LeagueName = "";
        }

        private void AssignCombinationMP(ref SportEventDTO sportEvent)
        {
            string selection = "";
            switch (_bettran.Betteam)
            {
                case "110":
                    selection = "Trixie";
                    break;
                case "1110":
                    selection = "Yankee";
                    break;
                case "11110":
                    selection = "Canadian";
                    break;
                case "111110":
                    selection = "Heinz";
                    break;
                case "1111110":
                    selection = "Super Heinz";
                    break;
                case "11111110":
                    selection = "Goliath";
                    break;
                default:
                    selection = "Trixie";
                    break;
            }
           
            sportEvent.LiveScore = "";
            sportEvent.Choice = "";
            sportEvent.Hdp = "";
            sportEvent.BetType = "<span class='lnk' onclick='LatestBet.showCombinationMP(\"" + _bettran.Refno + "\",\"" + _bettran.Transid + "\")' >" + RemotingRef.RefData.GetBetTypeName(_bettran.Bettype.ToString(), "en-EN") + " - <span class='MPType'>" + selection + "</span></span><br />";
            sportEvent.Event = "<span class='detailMP' onclick='LatestBet.showCombinationDetailMP(\"" + _bettran.Refno + "\",\"" + selection + "\")' >" + "Details</span>"; ;
            sportEvent.SportType = "";
            sportEvent.LeagueName = "";
        }

        private void AssignWarrior(ref SportEventDTO sportEvent)
        {
            sportEvent.Hdp = string.Empty;
            sportEvent.LiveScore = string.Empty;
            sportEvent.Choice = _bettran.Betteam == "h" ? "2nd ball win" : "3rd ball win";
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />Number Game No " + _bettran.MatchCode.ToString();
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);

        }

        private void AssignNextCombo(ref SportEventDTO sportEvent)
        {
            
            if (_bettran.Betteam == "1:1")
                sportEvent.Choice = "Over/Odd";
            else if (_bettran.Betteam == "1:2")
                sportEvent.Choice = "Over/Even";
            else if (_bettran.Betteam == "2:1")
                sportEvent.Choice = "Under/Odd";
            else if (_bettran.Betteam == "2:2")
                sportEvent.Choice = "Under/Even";
            sportEvent.Hdp = "&nbsp;" + (double.Parse(Bingo.Core.BingoSettings.MaxBall.ToString()) / 2);
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Betid+ "]";
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />Number Game No " + _bettran.MatchCode.ToString();
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);

        }

        private void Assign1stBallOU(ref SportEventDTO sportEvent)
        {
            sportEvent.Choice = _bettran.Betteam.ToLower() == "h" ? "OVER" : "UNDER";
            sportEvent.Hdp = "&nbsp;" + (double.Parse(Bingo.Core.BingoSettings.MaxBall.ToString()) / 2);
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />Number Game No " + _bettran.MatchCode.ToString();
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private void AssignLastBallOU(ref SportEventDTO sportEvent)
        {
            sportEvent.Choice = _bettran.Betteam.ToLower() == "h" ? "OVER" : "UNDER";
            sportEvent.Hdp = "&nbsp;" + (double.Parse(Bingo.Core.BingoSettings.MaxBall.ToString()) / 2);
            sportEvent.LiveScore = string.Empty;
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />Number Game No " + _bettran.MatchCode.ToString();
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private void Assign1stBallOE(ref SportEventDTO sportEvent)
        {
            sportEvent.Choice = _bettran.Betteam.ToLower() == "h" ? "ODD" : "EVEN";
            sportEvent.Hdp = string.Empty;
            sportEvent.LiveScore = string.Empty;
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />Number Game No " + _bettran.MatchCode.ToString();
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private void AssignLastBallOE(ref SportEventDTO sportEvent)
        {
            sportEvent.Choice = _bettran.Betteam.ToLower() == "h" ? "ODD" : "EVEN";
            sportEvent.Hdp = string.Empty;
            sportEvent.LiveScore = string.Empty;
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />Number Game No " + _bettran.MatchCode.ToString();
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private void AssignOverUnder(ref SportEventDTO sportEvent)
        {
            sportEvent.Choice = _bettran.Betteam.ToLower() == "h" ? "OVER" : "UNDER";
            sportEvent.Hdp = "&nbsp;" + (double.Parse(Bingo.Core.BingoSettings.MaxBall.ToString()) / 2);
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Betid + "]";
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />Number Game No " + _bettran.MatchCode.ToString();
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private void AssignOddEven(ref SportEventDTO sportEvent)
        {
            sportEvent.Choice = _bettran.Betteam.ToLower() == "h" ? "ODD" : "EVEN";
            sportEvent.Hdp = string.Empty;
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Betid + "]";
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />Number Game No " + _bettran.MatchCode.ToString();
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
            //GetSportEventVoidStatus(ref sportEvent);
        }

        private void AssignNextHighLow(ref SportEventDTO sportEvent)
        {
            sportEvent.Choice = _bettran.Betteam.ToLower() == "h" ? "HIGH" : "LOW";
            sportEvent.Hdp = "&nbsp;" + (double.Parse(Bingo.Core.BingoSettings.MaxBall.ToString()) / 2);
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Betid + "]";
            sportEvent.BetType = "<br />" + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = "<br />Number Game No " + _bettran.MatchCode.ToString();
            sportEvent.SportType = "<br />" + SportBookUtils.GetSportName(_bettran.SportId);
            sportEvent.LeagueName = "&nbsp;" + SportBookUtils.GetLeagueName(_bettran.LeagueId);
           // GetSportEventVoidStatus(ref sportEvent);
        }

        /*private void AssignCasino(ref SportEventDTO sportEvent, int betType)
        {
            string choice = "";
            if (betType == 71 || betType == 113)
                choice = "Casino Game";
            else if (betType == 72 || betType == 114)
                choice = "P2P Game";
            else if (betType == 112)
                choice = "Casino Game - Transfer";
            else if (betType == 73)
                //choice = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblIBCbetBingo");
                choice = "IBCbet Bingo";
            sportEvent.Choice = choice;

        }*/
        private  string oddMP(int odds)
        {
            return "<span class='style_12BB'><b> @ " + FormatHandler.FormatNumber3(odds, "en-EN") + "</b></span>";
        }
    }
}
