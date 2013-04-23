using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Infrastructure.Helper.SportHelper
{
    public static class BetTeam
    {
        private static string GetHA(string ha, string lang)
        {
            string rs = string.Empty;
            switch (ha)
            {
                case "1":
                    switch (lang)
                    {
                        case "cs":
                            rs = "主";
                            break;
                        default:
                            rs = "Home";
                            break;
                    }
                    break;
                case "2":
                    switch (lang)
                    {
                        case "cs":
                            rs = "客";
                            break;
                        default:
                            rs = "Away";
                            break;
                    }
                    break;
                default:
                    switch (lang)
                    {
                        case "cs":
                            rs = "無";
                            break;
                        default:
                            rs = "No";
                            break;
                    }
                    break;
            }
            return rs;
        }
        private  static string GetFGLG(string fglg, string lang)
        {
            string rs = string.Empty;

            switch (fglg)
            {
                case "1":
                    switch (lang)
                    {
                        case "cs":
                            rs = "先";
                            break;
                        default:
                            rs = " FG";
                            break;
                    }
                    break;
                case "2":
                    switch (lang)
                    {
                        case "cs":
                            rs = "後";
                            break;
                        default:
                            rs = " LG";
                            break;
                    }
                    break;
                default:
                    switch (lang)
                    {
                        case "cs":
                            rs = "進球";
                            break;
                        default:
                            rs = " Goal";
                            break;
                    }
                    break;
            }

            return rs;
        }


        private static string GetHome(string home, string lang)
        {
            string rs = string.Empty;
            
            switch (home)
            {
                case "1":
                    switch (lang)
                    {
                        case "cs":
                            rs = "主";
                            break;
                        default:
                            rs = "H";
                            break;
                    }
                    break;
                case "2":
                    switch (lang)
                    {
                        case "cs":
                            rs = "客";
                            break;
                        default:
                            rs = "A";
                            break;
                    }
                    break;
                default:
                    switch (lang)
                    {
                        case "cs":
                            rs = "和";
                            break;
                        default:
                            rs = "D";
                            break;
                    }
                    break;
            }
            return rs;
        }
        private static string GetAway(string away, string lang)
        {
            string rs = string.Empty;

            switch (away)
            {
                case "1":
                    switch (lang)
                    {
                        case "cs":
                            rs = "主";
                            break;
                        default:
                            rs = "H";
                            break;
                    }
                    break;
                case "2":
                    switch (lang)
                    {
                        case "cs":
                            rs = "客";
                            break;
                        default:
                            rs = "A";
                            break;
                    }
                    break;
                default:
                    switch (lang)
                    {
                        case "cs":
                            rs = "和";
                            break;
                        default:
                            rs = "D";
                            break;
                    }
                    break;
            }
            return rs;
        }

        /// <summary>
        /// f_GetChoice
        /// </summary>
        /// <param name="betteam"></param>
        /// <param name="bettype"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string GetBetTeam(string betteam, int bettype, string language)
        {
            string rs = betteam;
            if (string.IsNullOrEmpty(language))
                language = "e";

            string lang = language.ToLower();

            if (lang.Equals("c"))
                lang = "cs";

            if (string.IsNullOrEmpty(betteam))
                return rs;

            if (betteam.Length < 2)
                return rs;

            if(bettype == 14) //FGLG
            {
                string ha, fglg;
              
                ha = betteam.Substring(0, 1);
                fglg = betteam.Substring(betteam.Length - 1, 1);

                string strHA, strFGLG;

                strHA = GetHA(ha, lang);
                strFGLG = GetFGLG(fglg, lang);

                rs = strHA + strFGLG;
            }
            else if(bettype == 16)//HTFT
            {
                string home, away;
                home = betteam.Substring(0, 1);
                away = betteam.Substring(betteam.Length - 1, 1);

                string strHome, strAway;
                strHome = GetHome(home, lang);
                strAway = GetAway(away, lang);

                rs = strHome + strAway;
            }
           
            
            return rs;
        }
    }
}
