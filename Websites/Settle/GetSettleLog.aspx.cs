using System;
using System.Text;
using DNAide.Core;
using Sunplus.Accounting.Business.Settle.Log;
using Sunplus.Accounting.Core.Web.Controls;

public partial class Settle_GetSettleLog : PageBase
{
    public StringBuilder html = new StringBuilder();
    protected string FromDate = "";
    protected string ToDate = "";
    protected string Matchcode = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        var log = new MatchSettleLog();
        Matchcode = Request["matchcode"];
        Matchcode = CastAide.AsString(Matchcode).Replace("^", "&");

        var fdate = Request["fdate"] ?? DateTime.Now.AddDays(-3).ToShortDateString();
        var tdate = Request["tdate"] ?? DateTime.Now.ToShortDateString();
        fdate = CastAide.AsDateTime(fdate, DateTime.Today.AddDays(-3)).ToShortDateString();
        tdate = CastAide.AsDateTime(tdate).ToShortDateString();

        FromDate = Calendar.GenCalendarControl("fromdate", "../", "fromdateTrigger", fdate, false, false, 0, 0, "");
        ToDate = Calendar.GenCalendarControl("todate", "../", "todateTrigger", tdate, true, false, 0, 0, "");


        if (Request["t"] == "m")
        {
            html.Append(log.LoadMenu());
            //html.Append(log.LoadHeaderForm(matchcode, fdate, tdate));
            //html.Append(log.LoadMatchInfo(matchcode, fdate, tdate));
        }
        else
        {
            //html.Append(log.LoadHeaderForm(matchcode, fdate, tdate));
            //html.Append(log.LoadMatchInfo(matchcode, fdate, tdate));
        }
    }


}
