using System.Collections.Generic;
using System.Linq;
using DNAide.Core;
using RemotingRef;

namespace Sunplus.Accounting.Core.Cache
{
    /// <summary>
    /// Summary description for CacheRef
    /// </summary>
    public static class CacheRef
    {
        private static string TWLang(string lang)
        {
            switch (lang)
            {
                case "en-US": return "en";
                case "zh-TW": return "ch"; // Traditional
                case "zh-CN": return "cs"; // Simplified
                case "ja-JP": return "jp";
                case "th-TH": return "th"; // Thai
                default: return "en"; 
            }
        }
        public static string GetBettype(object bettypeID)
        {
            return RefData.GetBetTypeName(bettypeID.ToString(), TWLang("en-US"), string.Empty);
        }

        public static string GetSportName(object sportID)
        {
            return RefData.GetSportName(sportID.ToString(), TWLang("en-US"), string.Empty);
        }

        public static string GetTeamName(object teamID)
        {
            return RefData.GetTeamName(teamID.ToString(), TWLang("en-US"), string.Empty);
        }

        public static string GetLeagueName(object leagueID)
        {
            return RefData.GetLeagueName(leagueID.ToString(), TWLang("en-US"), string.Empty);
        }

        public static string GetFncIndexName(object IndexId)
        {
            return RefData.GetFncIndexName(CastAide.AsString(IndexId), TWLang("en-US"));
        }

        public static string GetHorseTeamName(object CodeID)
        {
            return RefData.GetHorseTeamName(CastAide.AsString(CodeID), TWLang("en-US"));
        }

        public static string GetOddsTypeName(object OddsType)
        {
            return RefData.GetOddsTypeName(CastAide.AsString(OddsType), TWLang("en-US"));
        }

        public static string GetCombinationName(string betteam)
        {
            string name = string.Empty;
            switch (betteam)
            {
                case "110":
                    name = "Trixie";
                    break;
                case "1110":
                    name = "Yankee";
                    break;
                case "11110":
                    name = "Canadian";
                    break;
                case "111110":
                    name = "Heinz";
                    break;
                case "1111110":
                    name = "Super Heinz";
                    break;
                case "11111110":
                    name = "Goliath";
                    break;
            }
            return name;
        }

        public static string GetLiveCasinoResult(int resultId, int bettypeId)
        {
            string result = string.Empty;
            switch (bettypeId)
            {
                case 1101:
                case 1103:
                    result = RemotingRef.RefData.GetCasinoBetItem(resultId.ToString(), TWLang("en-US"));;
                    break;
                case 1102:
                    result = RemotingRef.RefData.GetRouletteBetItem(resultId.ToString(), TWLang("en-US"));
                    break;
                default:
                    break;
                
            }
            return result;
        }        
    }
}
