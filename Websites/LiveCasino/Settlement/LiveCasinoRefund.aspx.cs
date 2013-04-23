using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fanex.Tarus.Settlement.Business;
using Sunplus.Accounting.Core.Cache;

public partial class LiveCasino_Settlement_LiveCasinoRefund : PageBase
{
    public string LeagueName = string.Empty;
    public string Code = string.Empty;
    public string KickOffTime = string.Empty;
    public string Event = string.Empty;
    public string Result = string.Empty;
    public string MatchID = string.Empty;
    public string AdminName = string.Empty;
    public string AdminID = string.Empty;
    public string ShowDate = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        var leagueId = DNAide.Core.CastAide.AsInt32(Request.QueryString["lid"]);
        LeagueName = CacheRef.GetLeagueName(leagueId);

        ShowDate = DNAide.Core.CastAide.AsString(Request.QueryString["showdate"]);
        var betType = DNAide.Core.CastAide.AsInt32(Request.QueryString["bettype"]);
        var matchId = DNAide.Core.CastAide.AsInt32(Request.QueryString["matchid"]);

        var match = (new LiveCasinoManager()).GetMatch(matchId, betType);
        if (match != null)
        {
            Code = match.MatchCode;
            KickOffTime = match.KickOffTime;
            Event = match.ShowTime;
            Result = match.BetStatus;
        }
        MatchID = matchId.ToString();
        AdminID = Sunplus.Accounting.Core.Common.AppSession.AccId.ToString();
        AdminName = Sunplus.Accounting.Core.Common.AppSession.AccUserName;
    }
}
