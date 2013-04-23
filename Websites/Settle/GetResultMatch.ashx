<%@ WebHandler Language="C#" Class="GetResultMatch" %>

using System;
using System.Web;
using Sunplus.Accounting.Business.Settlement;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Web.SessionState;
using System.Text;
using System.Collections;
using System.Data;
using IBCUtils.DAL;
using IBCUtils.Web;
using DNAide.Core;
using DNAide.Data;


public class GetResultMatch : IHttpHandler, IRequiresSessionState
{
    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    private string nl = Environment.NewLine;
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            string _s = string.Empty;
            context.Response.ContentType = "text/xml";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            int custID = Convert.ToInt32(context.Session["AccId"] as object ?? -1);
            if (custID == -1) throw new AccException(IBCUtils.IbcError.ACCESS_DENIED_MSG);
            DateTime dtFdate = CastAide.AsDateTime(context.Request.Form["fdate"]);
            string fdate = IBCUtils.FormatHandler.FormatDateEn(dtFdate);
            string lid = context.Request.Form["lid"];
            int leagueID = 0, sportID = 0;                        
            string type = context.Request.Form["type"];
            if (string.Compare(type, "getMatchs", true) == 0)
            {
                // return Matchs
                if (!string.IsNullOrEmpty(lid))
                {
                    leagueID = CastAide.AsInt32(lid.Split('|')[0]);
                    sportID = CastAide.AsInt32(lid.Split('|')[1]);
                }
                var dr = Settle.SettleSelectMatch(sportID, dtFdate, dtFdate, leagueID, false, false);
                if (leagueID == 0)
                {
                    ArrayList _sportList = new ArrayList();
                    Hashtable _sport = new Hashtable();
                    using (IDataReader drSport = Settle.SettleSelectSport())
                    {
                        while (drSport.Read())
                        {
                            sportID = CastAide.AsInt32(drSport["sporttype"], -1);
                            if (sportID == 151 && WebSettings.disabledHR)
                            {
                                ;
                            }
                            else
                            {
                                _sportList.Add(sportID);
                                //_sport.Add(sportID, CacheRef.GetSportName(drSport["sporttype"]));
                                _sport.Add(sportID, drSport["sportname"].ToString());
                            }
                            
                        }
                    }
                    _s = GenerateResultHTML.GenerateHTML(dr, _sportList,leagueID, _sport, fdate);
                }
                else
                {
                    string _sportName = CacheRef.GetSportName(sportID);//GetSportName(sportID);
                    _s = GenerateResultHTML.GenerateHTMLBySportID(dr.Tables[0].Select(), sportID, leagueID, _sportName, fdate);
                }
            }
            else
            {
                StringBuilder _sLeague = new StringBuilder("<select id='ddlLeagues' name='ddlLeagues' class='_fullwidth' onchange='ChangeLeague();'>");
                _sLeague.Append("<option value='0|0' selected='selected'>*..::¤ Show All Leagues  ¤::..*</option>" + nl);
                using (IDataReader r = Settle.SettleSelectLeague(dtFdate, dtFdate, 0))
                {
                    while (r.Read())
                    {
                        if (CastAide.AsString(r["sporttype"]).Equals("151") && WebSettings.disabledHR)
                        {
                            ;
                        }
                        else
                        {
                            string _leaguename = (string)r["leaguename"];
                            string _leagueid = r["leagueid"].ToString();
                            _sLeague.Append("<option value='" + _leagueid + "|" + r["sporttype"].ToString() + "'>" + _leaguename.Replace("\"", "'") + "</option>" + nl);
                        }
                    }
                }
                _sLeague.Append("</select>");
                _s = _sLeague.ToString();
            }

            context.Response.Write(_s);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }

    private string GetSportName(int sportID)
    {
        switch (sportID)
        {
            case 1: return "Soccer";
            case 2: return "Basketball";
            case 3: return "Football";
            case 4: return "Hockey";
            case 5: return "Tennis";
            case 6: return "Volleyball";
            case 7: return "Billiard";
            case 8: return "Baseball";
            case 9: return "Badminton";
            case 10: return "Golf";
            case 11: return "Motorsports";
            case 12: return "Financial";
            case 99: return "Other Sports";
            case 32: return "Netball";
            default: return string.Empty;
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
