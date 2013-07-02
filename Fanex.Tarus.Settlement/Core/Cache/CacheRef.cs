// -----------------------------------------------------------------------
// <copyright file="CacheRef.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Tarus.Settlement.Core.Cache
{
    using System.Linq;
    using DNAide.Core;
    using RemotingRef;
    using Fanex.Tarus.Settlement.Core.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class CacheRef
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
                case (int)Enumerations.Bettype.PartyBaccarat:
                    result = RemotingRef.RefData.GetCasinoBetItem(resultId.ToString(), TWLang("en-US"));;
                    break;
                case (int)Enumerations.Bettype.PartyRoutle:
                    result = resultId == 0 ? "-" : RemotingRef.RefData.GetRouletteBetItem(resultId.ToString(), TWLang("en-US"));                    
                    break;
                case (int)Enumerations.Bettype.LiveBaccarat:
                    result = RemotingRef.RefData.GetCasinoBetItem(resultId.ToString(), TWLang("en-US")); ;
                    break;
                case (int)Enumerations.Bettype.LiveRoutle:
                    result = resultId == 0 ? "-" : RemotingRef.RefData.GetRouletteBetItem(resultId.ToString(), TWLang("en-US"));
                    break;
                default:
                    break;
                
            }
            return result;
        }    
    }
}
