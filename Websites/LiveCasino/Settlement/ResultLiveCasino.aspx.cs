using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Tarus.Settlement.Business;
using System.Text;
using Fanex.Tarus.Settlement.Entity;

public partial class LiveCasino_Settlement_ResultLiveCasino : PageBase
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