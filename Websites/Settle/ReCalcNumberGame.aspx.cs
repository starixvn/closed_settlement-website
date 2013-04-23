using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Settle;
using Sunplus.Accounting.Entities.Settle;
using System.Text;

public partial class Settle_ReCalcNumberGame : PageBase
{
    public string Balls = "";
    public int matchid;
    public string matchcode = "";
    public string kickoffdate = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        matchid = DNAide.Core.CastAide.AsInt32(Request.QueryString["matchid"]);
        matchcode = DNAide.Core.CastAide.AsString(Request.QueryString["matchcode"]);
        kickoffdate = DNAide.Core.CastAide.AsString(Request.QueryString["kickoffdate"]);

        var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
        var listBalls = settle.GetNumberGameBalls(matchid);
        Balls = "var balls = [{0}];";
        List<string> sb = new List<string>();
        foreach (NumberGameBalls ball in listBalls)
        {
            sb.Add("{MatchId: " + ball.MatchId + ", Seq: " + ball.Seq + ", Number: " + ball.Number + "}");
        }
        Balls = string.Format(Balls, string.Join(",", sb.ToArray()));
    }
}