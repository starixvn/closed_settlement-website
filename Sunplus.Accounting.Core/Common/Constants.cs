
using System.Collections.Generic;
using System.Globalization;
using Sunplus.Accounting.Core.Configurations;

namespace Sunplus.Accounting.Core.Common
{
    /// <summary>
    /// Summary description for Constant
    /// </summary>
    public static class Constants
    {
        public static CultureInfo CurrentCulture
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }

        public static class QueryStringKey
        {
            public const string CustomerId = "custid";
            public const string UplineId = "uplineid";
            public const string SuperId = "superid";
            public const string MasterId = "masterid";
            public const string AgentId = "agentid";
            public const string Username = "username";
            public const string CID = "cid";
            public const string User = "user";
            public const string CurrencyId = "Currency";
            public const string ListCurrency = "listCurrency";
        }
        public static int MaxLimitPerIP = 10;

        public static readonly string ATTR_DISABLED = "disabled";

#region Global  Resource
        public static class Resources
        {
            public const string LanguageResources = "Language";

            public static class Language
            {
                public const string UserInternal = "userInternal";
                public const string UserBetting = "userBetting";
            }
        }
#endregion

        public static class SessionKey
        {
            public const string UserId = "AccId";
        }

        public static class Page
        {
            /// <summary>
            /// ~/PageNotFound.aspx
            /// </summary>
            public const string NotFound = "~/PageNotFound.aspx";
            public const string SignIn = "~/Signin.aspx";
            /// <summary>
            /// ~/ErrorPage.aspx
            /// </summary>
            public const string Error = "~/ErrorPage.aspx";
            public const string ErrorPage = "/ErrorPage.aspx";

            /// <summary>
            /// "~/AccessDenied.aspx"
            /// </summary>
            public const string AccessDenied = "~/AccessDenied.aspx";
            public const string LogOut = "~/LogOut.aspx";

            public const string UMPage = "~/UnderMaintenance.aspx";
        }

        public static class System
        {
           
            public const string Accounting = "ACCOUNTING";
            
          
        }

        public static class SystemCurrency
        {
            public const int Thai = 43;
        }

        public static class Domain
        {
            public static readonly Dictionary<string, string> External = WebSettings.ExternalSite;
        }

        #region Sport Constance
        public static class Sport
        {
            
            public const int Soccer = 1;
            public const int Basketball = 2;
            public const int Football = 3;
            public const int IceHockey = 4;
            public const int Tennis = 5;
            public const int Volleyball = 6;
            public const int Billiard = 7;
            public const int Baseball = 8;
            public const int Badminton = 9;
            public const int Golf = 10;
            public const int Motorsports = 11;
            public const int Swimming = 12;
            public const int Politics = 13;
            public const int WaterPolo = 14;
            public const int Diving = 15;
            public const int Boxing = 16;
            public const int Archery = 17;
            public const int TableTennis = 18;
            public const int Weightlifting = 19;
            public const int Canoeing = 20;
            public const int Gymnastics = 21;
            public const int Athletics = 22;
            public const int Equestrian = 23;
            public const int Handball = 24;
            public const int Darts = 25;
            public const int Rugby = 26;
            public const int Cricket = 27;
            public const int FieldHockey = 28;
            public const int WinterSports = 29;
            public const int Squash = 30;
            public const int OtherSports = 99;
            public const int HorseRacing = 151;
            public const int Greyhound = 152;
            public const int Harness = 153;
            public const int Financials = 201;
            public const int Casino = 251;
            public const int BingoGame = 252;
            public const int NumberGame = 161;
            public const int LiveCasino = 162;
            public static readonly IEnumerable<int> Sports = new int[] { Soccer, Basketball, Football, Tennis, Baseball, Golf, Motorsports, OtherSports };
            public static string GetSportName(int sportId)
            {
                switch (sportId)
                {
                    case 1:
                        return "Soccer";
                    case 2:
                        return "BasketBall";
                    case 3:
                        return "FootBall";
                    case 4:
                        return "Hockey";
                    case 5:
                        return "Tennis";
                    case 6:
                        return "VolleyBall";
                    case 7:
                        return "Billiard";
                    case 8:
                        return "Baseball";
                    case 9:
                        return "Badminton";
                    case 10:
                        return "Golf";
                    case 11:
                        return "MotorSports";
                    case 12:
                        return "Swimming";
                    case 13:
                        return "SynchronizedSwimming";
                    case 14:
                        return "WaterPolo";
                    case 15:
                        return "Diving";
                    case 16:
                        return "Boxing";
                    case 17:
                        return "Archery";
                    case 18:
                        return "TableTennis";
                    case 19:
                        return "WeightLifting";
                    case 20:
                        return "Canoeing";
                    case 21:
                        return "Gymnastics";
                    case 22:
                        return "Athletics";
                    case 23:
                        return "Equestrian";
                    case 24:
                        return "Handball";
                    case 25:
                        return "Darts";
                    case 99:
                        return "OtherSports";
                    default:
                        return string.Empty;
                }
            }
        }
        #endregion

        public class BetType
        {
            /// <summary>
            /// Plus 1000 to convert Bet
            /// </summary>
            public static int ToLiveBettype = 1000;


            public static readonly int Handicap = 1;
            public static readonly int OddEven = 2;
            public static readonly int OverUnder = 3;
            public static readonly int CorrectScore = 4;
            public static readonly int FT1X2 = 5;
            public static readonly int TotalGoal = 6;
            public static readonly int FirstHandicap = 7;
            public static readonly int FirstOverUnder = 8;
            public static readonly int MixParlay = 9;
            public static readonly int Outright = 10;
            public static readonly int TotalCorners = 11;
            public static readonly int FirstOddEven = 12;
            public static readonly int CleanSheet = 13;
            public static readonly int FirstGoalLastGoal = 14;
            public static readonly int First1X2 = 15;
            public static readonly int HTFT = 16;
            public static readonly int SecondHDP = 17;
            public static readonly int SecondOverUnder = 18;
            public static readonly int Substitutes = 19;
            public static readonly int MoneyLine = 20;
            public static readonly int NextGoal = 22;
            public static readonly int NextCorner = 23;
            public static readonly int Win = 31;
            public static readonly int Place = 32;
            public static readonly int WinPlace = 33;
            public static readonly int WinFOdds = 41;
            public static readonly int PlaceFOdds = 42;
            public static readonly int WinPlaceFOdds = 43;
            public static readonly int FiveMinOU = 51;
            public static readonly int FifteenMinOU = 52;
            public static readonly int ThirtyMinOU = 53;
            public static readonly int FortyFiveMinOU = 54;
            public static readonly int SixtyMinOU = 55;
            public static readonly int EndofdayOU = 56;
            public static readonly int FiveMinOE = 61;
            public static readonly int FifteenMinOE = 62;
            public static readonly int ThirtyMinOE = 63;
            public static readonly int FortyFiveMinOE = 64;
            public static readonly int SixtyMinOE = 65;
            public static readonly int EndofdayOE = 66;
            public static readonly int CasinoGames = 71;
            public static readonly int P2PGames = 72;
            public static readonly int CasinoMiniGame = 73;
            public static readonly int FirstLastOU = 81;
            public static readonly int NextOU = 85;
            public static readonly int Warrior = 88; 
            public static readonly int NextCombo = 89;
            public static readonly int NumberGroup = 90;
            //Live Bettype

            public static readonly int HandicapLive = 1 + ToLiveBettype;
            public static readonly int OverUnderLive = 3 + ToLiveBettype;
            public static readonly int FirstHandicapLive = 7 + ToLiveBettype;
            public static readonly int FirstOverUnderLive = 8 + ToLiveBettype;


            public static IEnumerable<int> SoccerBettypes = new int[] { Handicap, OverUnder, FirstHandicap, FirstOverUnder, OddEven, FT1X2, CorrectScore, TotalGoal, MixParlay, Outright, HandicapLive, OverUnderLive, FirstHandicapLive, FirstOverUnderLive };

            public static string GetBetTypeName(int bettype)
            {
                string typename = "";
                switch (bettype)
                {
                    case 1:
                        typename = "Handicap";
                        break;
                    case 2:
                        typename = "OddEven";
                        break;
                    case 12:
                        typename = "FirstOddEven";
                        break;
                    case 3:
                        typename = "OverUnder";
                        break;
                    case 4:
                        typename = "CorrectScore";
                        break;
                    case 5:
                        typename = "FT1X2";
                        break;
                    case 6:
                        typename = "TotalGoal";
                        break;
                    case 7:
                        typename = "FirstHandicap";
                        break;
                    case 8:
                        typename = "FirstOverUnder";
                        break;
                    case 14:
                        typename = "FirstGoalLastGoal";
                        break;
                    case 15:
                        typename = "First1X2";
                        break;
                    case 16:
                        typename = "HTFT";
                        break;
                    case 20:
                        typename = "MoneyLine";
                        break;
                    default:
                        typename = "NA";
                        break;
                }
                return typename;
            }


        }

        public class Error
        {
            public static int NotAutherized = 500;
        }
        public static string GetNameByRoleId(int roleId)
        {
            if (roleId == 4)
            {
                return "Super";;
            }
            else if (roleId == 3)
            {
                return "Master";
            }
            else if (roleId == 2)
            {
                return "Agent";
            }
            else
            {
                return"Member";
            }
        }


        public static class Config
        {
            public static readonly string ConfigFolder = "Configurations";
        }
        
    }
}
