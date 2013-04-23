using System;
using System.Data;
using System.Collections;
using System.Web;
using System.Text;
using DNAide.Web;
using Sunplus.Accounting.Business.Settlement;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using DNAide.Core;
using DNAide.Data;

using Sunplus.Accounting.Core.Web;

public partial class Settle_Result : PageBase
{
    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    private string nl = Environment.NewLine;
    private string fromDate;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.sTitle.Text = "Result";
            DateTime fDate = DateTime.Now.AddDays(-1);
            fromDate = IBCUtils.FormatHandler.FormatDateEn(fDate);            

            string s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/Result.htm");
            TemplateHandler.Assign(ref s, "{fromdate}", Calendar.GenCalendarControl("fdate", "../", "fdateTrigger", fromDate, false, false, 0, 0, "DateSubmit()"));//PHuong.Nguyen, show others day of next month

            StringBuilder _sLeague = new StringBuilder();
            _sLeague.Append("<option value='0|0' selected='selected'>*..::¤ Show All Leagues  ¤::..*</option>" + nl);
            using (IDataReader r = Settle.SettleSelectLeague(fDate, fDate, 0))
            {
                while (r.Read())
                {
                    if (!CastAide.AsString(r["sporttype"]).Equals("151") && !WebSettings.disabledHR)
                    {
                        string _leaguename = (string)r["leaguename"];
                        string _leagueid = r["leagueid"].ToString();
                        _sLeague.Append("<option value='" + _leagueid + "|" + r["sporttype"].ToString() + "'>" + _leaguename.Replace("\"", "'") + "</option>" + nl);
                    }
                }
            }
            TemplateHandler.Assign(ref s, "{leagueOptions}", _sLeague.ToString());
            ArrayList _sportList = new ArrayList();
            Hashtable _sport = new Hashtable();
            using (IDataReader drSport = Settle.SettleSelectSport())
            {
                while (drSport.Read())
                {
                    int sportID = CastAide.AsInt32(drSport["sporttype"], -1);
                    _sportList.Add(sportID);
                    _sport.Add(sportID, drSport["sportname"].ToString());
                }
            }
            var dr = Settle.SettleSelectMatch(0, fDate, fDate, 0, false, false);
            //TemplateHandler.Assign(ref s, "{content}", GenerateResultHTML.GenerateHTML(dr,_sportList, _sport, fromDate));
            this.sSearch.Text = s;
            this.sContent.Text = GenerateResultHTML.GenerateHTML(dr, _sportList,0, _sport, fromDate);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }
}
