<%@ WebHandler Language="C#" Class="ChangeSport" %>

using System;
using System.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Web.SessionState;
using System.Text;
using System.Data;
using DNAide.Core;


public class ChangeSport : IHttpHandler, IRequiresSessionState
{
    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    private string nl = Environment.NewLine;
    private string fromDate, toDate;
    int sportID;   
    public void ProcessRequest (HttpContext context)
    {
        try
        {
            context.Response.ContentType = "text/html";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            fromDate = IBCUtils.FormatHandler.FormatDateEn(context.Request.Form["fdate"] ?? DateTime.Now.AddDays(-1).ToString(cult));
            toDate = IBCUtils.FormatHandler.FormatDateEn(context.Request.Form["tdate"] ?? DateTime.Now.ToString(cult));
            sportID = Convert.ToInt32(context.Request.Form["sportID"] ?? "1");
            StringBuilder _s = new StringBuilder("<select id='ddlLeagues' name='ddlLeagues' class='fullwidth'>");
            _s.Append("<option value='0' selected='selected'>*..::¤ Show All Leagues  ¤::..*</option>");
            using (IDataReader dr = Settle.SettleSelectLeague(CastAide.AsDateTime(fromDate, DateTime.Now), CastAide.AsDateTime(toDate, DateTime.Now), sportID))
            {
                while (dr.Read())
                {
                    if (CastAide.AsString(dr["sporttype"]).Equals("151") && WebSettings.disabledHR)
                    {
                        ;
                    }
                    else
                    {
                        string _leaguename = (string)dr["leaguename"];
                        string _leagueid = dr["leagueid"].ToString();
                        _s.Append("<option value='" + _leagueid + "'>" + _leaguename.Replace("\"", "'") + "</option>");
                    }
                }
            }

            _s.Append("</select>");
            _s = _s.Replace(Environment.NewLine, string.Empty).Replace("  ", string.Empty);
            context.Response.Write(_s.ToString());
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }     
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
