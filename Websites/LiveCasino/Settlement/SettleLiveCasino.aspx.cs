using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using Sunplus.Accounting.Core.Web.Controls;
using Fanex.Tarus.Settlement.Business;
using Fanex.Tarus.Settlement.Entity;
using System.Text;
public partial class LiveCasino_Settlement_SettleLiveCasino : PageBase
{
    public string Leagues = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        var listLeagues = (new LiveCasinoManager()).GetLeagues(DateTime.Now.ToShortDateString(), DateTime.Now.ToShortDateString());
        StringBuilder sb = new StringBuilder();
        sb.Append("var leagues = [];");
        foreach (League item in listLeagues)
        {
            sb.Append("leagues.push({LeagueId: " + item.LeagueId.ToString() + ", LeagueName: '" + item.LeagueName + "'});");
        }
        Leagues = sb.ToString();
    }
}