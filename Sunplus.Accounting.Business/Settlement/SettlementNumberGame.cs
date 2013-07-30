using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Web;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web;

namespace Sunplus.Accounting.Business.Settlement
{
    /// <summary>
    /// Summary description for SettlementNumberGame
    /// </summary>
    public class SettlementNumberGame
    {
        public static string GetNumberGameForSettlement(string fromdate, string todate, int leagueId, string permissions)
        {
            DataTable dt = DataAccess.IBCDLL.Accounting.Settle.GetNumberGameSettlement(CastAide.AsDateTime(fromdate), CastAide.AsDateTime(todate), leagueId);
            if (dt.Rows.Count == 0)
            {
                string _s = WebUtils.LoadTemplate("Settle/NoInfo.htm");
                TemplateHandler.Assign(ref _s, "{fdate}", fromdate);
                TemplateHandler.Assign(ref _s, "{tdate}", todate);
                return _s;
            }
            else
            {
                string _sMain = WebUtils.LoadTemplate("Settle/MainNumberGame.htm");
                string _strRow = WebUtils.LoadTemplate("Settle/RowNumberGame.htm");
                string _strLeague = TemplateHandler.CopyBlock(_sMain, "leaguename");
                TemplateHandler.RemoveBlock(ref _sMain, "leaguename");
                StringBuilder _s = new StringBuilder(); // content

                List<int> leagueids = GetLeagues(dt);
                DateTime now = DateTime.Now;
                DateTime yesterday = now.AddDays(-1);
                foreach (int _leagueid in leagueids)
                {
                    string _sleague = _strLeague;
                    TemplateHandler.Assign(ref _sleague, "{leaguename}", CacheRef.GetLeagueName(_leagueid));
                    _s.Append(_sleague);
                    List<int> matchids = GetMatchsByLeagueId(dt, _leagueid);
                    string bgClass = "";
                    foreach (int _matchid in matchids)
                    {
                        string EventStatus = "";
                        bool _isRefund = false;
                        string KickOffTime = "";
                        string Code = "";
                        string Event = "";

                        string bgRefund = "";

                        DateTime eventDate = DateTime.Now;

                        string Result = "";
                        string resultNumber = "";

                        int totalseq = 0;
                        int number;
                        string imgnumber;

                        string _sRow = _strRow;
                        TemplateHandler.Assign(ref _sRow, "{bgClass}", bgClass);
                        bgClass = (bgClass == "evenrow") ? "" : "evenrow";

                        DataRow[] rows = GetSeqNumberByMatchId(dt, _matchid);
                        totalseq = rows.Length;
                        if (rows.Length > 0)
                        {
                            var seqNumber = rows[0];
                            Code = seqNumber["matchcode"].ToString();
                            KickOffTime = seqNumber["kickofftime"].ToString();

                            EventStatus = seqNumber["eventstatus"].ToString();

                            eventDate = CastAide.AsDateTime(seqNumber["eventdate"]);

                            Event = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblNumberGameNo") + " " + Code;
                            _isRefund = Convert.ToBoolean(seqNumber["IsRefund"]);
                            if (_isRefund) bgRefund = SettleBgColor.REFUND;
                            if (!_isRefund && EventStatus.ToLower().Trim() == "refund ft") bgRefund = SettleBgColor.REFUND;

                            int i = 1;
                            string imgtd = "";
                            foreach (DataRow row in rows)
                            {
                                number = CastAide.AsInt16(row["number"], 0);
                                if (rows.Count() == 1 && number == 0) break;
                                imgnumber = number < 10 ? "0" + number.ToString() + ".jpg" : number.ToString() + ".jpg";

                                if (i == 1)
                                {
                                    imgtd = "<tr><td><img src='Images/" + imgnumber + "' border='0' hspace='1' vspace='1'></td>";
                                    i++;
                                }
                                else if (i == 15)
                                {
                                    imgtd = "<td><img src='Images/" + imgnumber + "' border='0' hspace='1' vspace='1'></td></tr>";
                                    i = 1;
                                }
                                else
                                {
                                    imgtd = "<td><img src='Images/" + imgnumber + "' border='0' hspace='1' vspace='1'></td>";
                                    i++;
                                }

                                resultNumber += imgtd;
                            }
                            if (i > 1 && i < 15)
                            {
                                for (int j = i; j < 14; j++)
                                {
                                    resultNumber += "<td></td>";
                                }
                                resultNumber += "</tr>";
                            }
                            Result = resultNumber == "" ? resultNumber : "<table class=\"noneborder\" cellspacing=\"0\">" + resultNumber + "</table>";
                        }
                        TemplateHandler.Assign(ref _sRow, "{date}", KickOffTime);
                        TemplateHandler.Assign(ref _sRow, "{code}", Code);
                        TemplateHandler.Assign(ref _sRow, "{result}", Result);
                        TemplateHandler.Assign(ref _sRow, "{fdate}", string.Format("{0:MM/dd/yyyy}", eventDate.ToShortDateString()));
                        TemplateHandler.Assign(ref _sRow, "{tdate}", string.Format("{0:MM/dd/yyyy}", now));

                        //hightlight Calculation column
                        TemplateHandler.Assign(ref _sRow, "{eventstatus}", EventStatus);
                        if (EventStatus.ToLower().Trim() == "completed")
                        {
                            TemplateHandler.Assign(ref _sRow, "{bgcolor_cell_eventstatus}", "style='background-color:#EFE3AF'");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref _sRow, "{bgcolor_cell_eventstatus}", string.Empty);
                        }

                        //Calculation column
                        
                        if (permissions.IndexOf("[C]") > -1 && ((eventDate.Day == now.Day && eventDate.Month == now.Month
                            && eventDate.Year == now.Year) || (eventDate.Day == yesterday.Day && eventDate.Month == yesterday.Month
                            && eventDate.Year == yesterday.Year)))
                        {
                            TemplateHandler.Assign(ref _sRow, "{Calculation}", "<span class='canchange' onclick=\"getNumberGameBalls(" + _matchid.ToString() + ", '" + Code + "', '" + KickOffTime.Split(' ')[0] + "');\">Re-Calc</span>");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref _sRow, "{Calculation}", "Re-Calc");
                        }

                        TemplateHandler.Assign(ref _sRow, "{event}", Event);
                        TemplateHandler.Assign(ref _sRow, "{classrefund}", bgRefund);

                        if (permissions.IndexOf("[C]") > -1)
                        {
                            TemplateHandler.RemoveBlock(ref _sRow, "RefuRead");
                            if ((eventDate.Day == now.Day && eventDate.Month == now.Month
                            && eventDate.Year == now.Year) || (eventDate.Day == yesterday.Day && eventDate.Month == yesterday.Month
                            && eventDate.Year == yesterday.Year))
                            {
                                TemplateHandler.Assign(ref _sRow, "{_onclickft}", "class='canchange' onclick=\"refundNumberGameFt('{matchid}')\"");
                                TemplateHandler.Assign(ref _sRow, "{_onclick}", "class='canchange' onclick=\"refundNumberGame('{matchid}')\"");
                            }
                            else
                            {
                                TemplateHandler.Assign(ref _sRow, "{_onclickft}", string.Empty);
                                TemplateHandler.Assign(ref _sRow, "{_onclick}", string.Empty);
                            }
                        }
                        else
                        {
                            TemplateHandler.RemoveBlock(ref _sRow, "RefuHost");
                        }
                        TemplateHandler.Assign(ref _sRow, "{matchid}", _matchid);
                        _s.Append(_sRow);
                    }
                }

                TemplateHandler.Assign(ref _sMain, "{fdate}", fromdate);
                TemplateHandler.Assign(ref _sMain, "{tdate}", todate);
                TemplateHandler.Assign(ref _sMain, "{content}", _s.ToString());
                return _sMain;
            }
        }

        private static List<int> GetLeagues(DataTable tb)
        {
            return (from row in tb.AsEnumerable()
                    select row.Field<int>("leagueid")).Distinct().ToList();
        }
        private static List<int> GetMatchsByLeagueId(DataTable tb, int leagueId)
        {
            return (from row in tb.AsEnumerable()
                    where row.Field<int>("leagueid") == leagueId
                    select row.Field<int>("matchid")).Distinct().ToList();
        }
        private static DataRow[] GetSeqNumberByMatchId(DataTable tb, int matchId)
        {
            DataRow[] rows = tb.Select("matchid=" + matchId.ToString());
            return rows;
        }
    }
}
