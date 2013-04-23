using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Tarus.Settlement.Business;

public partial class LiveCasino_Settlement_GridDataSettlment : PageBase
{
    public string HTML = string.Empty;
    public string Title = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {

        string fdate = Request.QueryString["fdate"];
        string tdate = Request.QueryString["tdate"];
        string LeagueIds = Request.QueryString["LeagueIds"];        
        int statusId = DNAide.Core.CastAide.AsInt32(Request.QueryString["status"]);
        HTML = (new LiveCasinoManager()).GetMatchesHtml(fdate, tdate, LeagueIds, statusId);
        Title = "EVENT FROM " + fdate + " 12:00:00 AM TO " + tdate + " 12:00:00 AM";
    }
}