using System;
using DNAide.Core;
using Sunplus.Bingo.Core;
using Sunplus.Data.DataAbstract.Utils;
using DNAide.Utils;

namespace Sunplus.Data.DataAbstract.Entity
{
    public class BASportEvent
    {
        private readonly Bettran _bettran = null;

        public BASportEvent(Bettran bettran)
        {
            _bettran = bettran;
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
                    break;
                case 81://Number Game
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                    AssignNumberGame(ref sportEvent);
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

            sportEvent.BetType = SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event =  home + " -vs- " + away;
        }

        private  void AssignOE(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";
            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);

            sportEvent.Choice = _bettran.Betteam == "h" ? "ODD" : "EVEN";
            sportEvent.Hdp = "";
            sportEvent.BetType =  SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = home + " -vs- " + away;
        }

        private  void Assign1x2(ref SportEventDTO sportEvent, bool isFT)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";

            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);
            sportEvent.Choice = isFT ? "FT." + _bettran.Betteam : "HT." + _bettran.Betteam;

            sportEvent.Hdp = "";
            sportEvent.BetType = SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = home + " -vs- " + away;
       
        }

        private void AssignNumberGame(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Betid + "]";

            if (_bettran.Bettype == 81 || _bettran.Bettype == 82 || _bettran.Bettype == 85)
            {
                var maxBall = (CastAide.AsDouble(BingoSettings.MaxBall.ToString()) / 2);
                sportEvent.Choice = _bettran.Betteam.ToLower() == "h" ? " OVER " : " UNDER ";
                sportEvent.Choice += !_bettran.Liveindicator ? string.Empty :  NumberFormat.DecFormat(maxBall,1);
             
            }
            else if (_bettran.Bettype == 83 || _bettran.Bettype == 84 || _bettran.Bettype == 86)
            {
                sportEvent.Choice = _bettran.Betteam.ToLower() == "h" ? "ODD " : "EVEN ";
            }
            else if (_bettran.Bettype == 87)
            {
                sportEvent.Choice = _bettran.Betteam.ToLower() == "h" ? "HIGH " : "LOW ";

            }

            sportEvent.Hdp = "";
            sportEvent.BetType = _bettran.Bettype.Equals(87)
                                       ? SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString())
                                       : (!_bettran.Liveindicator ? "FT. " : "Next ") + SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());

            sportEvent.Event = "Number Game No " + _bettran.MatchCode;
        }

        private  void AssignTG(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";

            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);
            sportEvent.Choice = _bettran.Betteam;

            sportEvent.Hdp = "";
            sportEvent.BetType = SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = home + " -vs- " + away;
          
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
            sportEvent.BetType = SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event =  home + " -vs- " + away;
        }

        private  void AssignOutRight(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = "";

            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);

            sportEvent.Choice = away;

            sportEvent.Hdp = "";
            sportEvent.BetType = SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = SportBookUtils.GetLeagueName(_bettran.LeagueId); ;
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
            sportEvent.Hdp = "&nbsp;" + FormatHandler.FormatNumber2(Math.Abs(CastAide.AsDecimal(_bettran.Hdp1)),"en-EN");
            sportEvent.BetType = SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event =  home + " -vs- " + away;
         
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

            sportEvent.Hdp = FormatHandler.FormatNumber2(Math.Abs(CastAide.AsDecimal(_bettran.Hdp1)),"en-EN");
            sportEvent.BetType =  SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = home + " -vs- " + away;
            
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
            sportEvent.BetType = SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = home + " -vs- " + away;
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
            sportEvent.BetType = SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event =  home + " -vs- " + away;
         
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
            sportEvent.BetType = SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event =  home + " -vs- " + away;
          
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
            sportEvent.BetType =  SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event = home + " -vs- " + away;
           
        }

        private  void AssignThreeWayHandicap(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = !_bettran.Liveindicator ? string.Empty : "&nbsp;[" + _bettran.Livehomescore + "-" + _bettran.Liveawayscore + "]";
            string home = SportBookUtils.GetTeamName(_bettran.HomeId);
            string away = SportBookUtils.GetTeamName(_bettran.AwayId);
            decimal hdpResult = 0;
            if (_bettran.Betteam.ToLower().Equals("1"))
            {
                //hdpResult = CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2) > 0 ? -(CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2)) : Math.Abs(CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2));
                hdpResult = (int)(CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2)) > 0 ? -((int)(CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2))) : Math.Abs((int)(CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2)));
                sportEvent.Choice = home;
                sportEvent.Hdp = "<span style='color:#000;'>" + (hdpResult > 0 ? "+&nbsp;" : string.Empty) + hdpResult + "</span>";

            }
            else if (_bettran.Betteam.ToLower().Equals("2"))
            {
                //hdpResult = CastAide.AsDecimal(_bettran.Hdp2) - CastAide.AsDecimal(_bettran.Hdp1) > 0 ? -(CastAide.AsDecimal(_bettran.Hdp2) - CastAide.AsDecimal(_bettran.Hdp1)) : Math.Abs(CastAide.AsDecimal(_bettran.Hdp2) - CastAide.AsDecimal(_bettran.Hdp1));
                hdpResult = CastAide.AsDecimal(_bettran.Hdp2) - CastAide.AsDecimal(_bettran.Hdp1) > 0 ? -((int)(CastAide.AsDecimal(_bettran.Hdp2) - CastAide.AsDecimal(_bettran.Hdp1))) : Math.Abs((int)(CastAide.AsDecimal(_bettran.Hdp2) - CastAide.AsDecimal(_bettran.Hdp1)));
                sportEvent.Choice = away;
                sportEvent.Hdp = "<span style='color:#000;'>" + (hdpResult > 0 ? "+&nbsp;" : string.Empty) + hdpResult + "</span>";

            }
            else if (_bettran.Betteam.ToLower().Equals("x"))
            {
                sportEvent.Choice = "Draw";
                sportEvent.Hdp = string.Format("<span style='color:#000;'>(<span style='color:#ff0000;'>{0}</span> +{1})</span>",
                    CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2) > 0 ? "A" : "H", Math.Abs((int)(CastAide.AsDecimal(_bettran.Hdp1) - CastAide.AsDecimal(_bettran.Hdp2))));
            }

            sportEvent.BetType =  SportBookUtils.GetBetTypeName(_bettran.Bettype.ToString());
            sportEvent.Event =  home + " -vs- " + away;
          
        }
        private  void AssignMP(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = "";
            sportEvent.Choice = "-";
            sportEvent.Hdp = "";
            sportEvent.BetType = "<span class='lnk' onclick='LatestBet.showMP(\"" + _bettran.Refno + "\",\"" + _bettran.Transid  + "\")' >" + "Mix Parlay</span>";
            sportEvent.Event = "-";
        }

        private void AssignCombinationMP(ref SportEventDTO sportEvent)
        {
            sportEvent.LiveScore = "";
            sportEvent.Choice = "-";
            sportEvent.Hdp = "";
            sportEvent.BetType = "<span class='lnk' onclick='LatestBet.showCombinationMP(\"" + _bettran.Refno + "\",\"" + _bettran.Transid + "\")' >" + " " + RemotingRef.RefData.GetBetTypeName(_bettran.Bettype.ToString(),"en-EN") + "- <span class='MPType'>" + getCombinationType(_bettran.Betteam) + "</span></span>";
            sportEvent.Event = "-";
        }

        private  string oddMP(int odds)
        {
            return "<span class='style_12BB'><b> @ " + FormatHandler.FormatNumber3(odds, "en-EN") + "</b></span>";
        }

        private string getCombinationType(string betteam)
        {
            var result = string.Empty;
            switch (betteam)
            {
                case "110":
                    result= "Trixie";
                    break;
                case "1110":
                    result = "Yankee";
                    break;
                case "11110":
                    result = "Canadian";
                    break;
                case "111110":
                    result = "Heinz";
                    break;
                case "1111110":
                    return "Super Heinz";
                    break;
                case "11111110":
                    result = "Goliath";
                    break;
            }
            return result;
        }
    }
}
