using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sunplus.Accounting.Core.Cache;
using Fanex.Tarus.Settlement.Business;
using Sunplus.Accounting.Core.Common;
using Fanex.Tarus.Settlement.Entity;

public partial class LiveCasino_Settlement_LiveCasinoReCalc : PageBase
{
    public string LeagueName = string.Empty;
    public string Code = string.Empty;
    public string KickOffTime = string.Empty;
    public string Event = string.Empty;
    public string FinalScore = string.Empty;
    public string Result = string.Empty;
    public string Settled = string.Empty;
    public string FTStatus = string.Empty;

    public string P1 = string.Empty, P3 = string.Empty, P5 = string.Empty;
    public string B2 = string.Empty, B4 = string.Empty, B6 = string.Empty;

    public string MatchID = string.Empty;

    public string AdminName = string.Empty;
    public string AdminID = string.Empty;

    public string BetType = string.Empty;

    public string ShowDate = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        var leagueId = DNAide.Core.CastAide.AsInt32(Request.QueryString["lid"]);
        LeagueName = CacheRef.GetLeagueName(leagueId);

        var betType = DNAide.Core.CastAide.AsInt32(Request.QueryString["bettype"]);
        BetType = betType.ToString();

        ShowDate = DNAide.Core.CastAide.AsString(Request.QueryString["showdate"]);

        var matchId = DNAide.Core.CastAide.AsInt32(Request.QueryString["matchid"]);
        MatchID = matchId.ToString();
        var match = (new LiveCasinoManager()).GetMatch(matchId, betType);
        if (match != null)
        {
            Code = match.MatchCode;
            KickOffTime = match.KickOffTime;
            Event = match.ShowTime;
            FinalScore = betType == (int)Fanex.Tarus.Settlement.Core.Common.Enumerations.Bettype.Baccarat ? (match.FinalHomeScore + " - " + match.FinalAwayScore) : match.FinalHomeScore;
            Result = match.BetStatus;
            if (match.IsSettled) Settled = "SETTLED";
            FTStatus = match.FTStatus;
        }
        var scores = (new LiveCasinoManager()).GetScore(matchId);
        if (scores != null)
        {
            ScoreDetail scoreDetail = scores.ScoreDetails.Find(sd => sd.Name == "PResult1");
            if (scoreDetail != null) P1 = scoreDetail.Value;
            scoreDetail = scores.ScoreDetails.Find(sd => sd.Name == "PResult3");
            if (scoreDetail != null) P3 = scoreDetail.Value;
            scoreDetail = scores.ScoreDetails.Find(sd => sd.Name == "PResult5");
            if (scoreDetail != null) P5 = scoreDetail.Value;

            scoreDetail = scores.ScoreDetails.Find(sd => sd.Name == "BResult2");
            if (scoreDetail != null) B2 = scoreDetail.Value;
            scoreDetail = scores.ScoreDetails.Find(sd => sd.Name == "BResult4");
            if (scoreDetail != null) B4 = scoreDetail.Value;
            scoreDetail = scores.ScoreDetails.Find(sd => sd.Name == "BResult6");
            if (scoreDetail != null) B6 = scoreDetail.Value;
        }

        AdminID = AppSession.AccId.ToString();
        AdminName = AppSession.AccUserName;
    }
}
