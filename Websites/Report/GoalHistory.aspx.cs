using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DNAide.Web;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;


public partial class Report_GoalHistory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int leagueid = CastAide.AsInt32(Request.QueryString["leagueid"], 0);
        int sporttype = CastAide.AsInt32(Request.QueryString["sporttype"], 0);
        int homeid = CastAide.AsInt32(Request.QueryString["homeid"], 0);
        int awayid = CastAide.AsInt32(Request.QueryString["awayid"], 0); 
        string goalrecord = CastAide.AsString(Request.QueryString["goalrecord"], string.Empty);
        bool isNeuTral = CastAide.AsBoolean(Request.QueryString["isneutral"], false);
        lGoalHistory.Text = GenHTMLGoalHistory(sporttype, leagueid, homeid, awayid, goalrecord, isNeuTral);
    }

    private string GenHTMLGoalHistory(int sporttype, int leagueid, int homeid, int awayid, string goalrecord, bool isNeuTral)
    {
        string html = "";
        try
        {
            if (0 == leagueid || string.IsNullOrEmpty(goalrecord))
            {
                //Assign No Information
                html = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/GoalHistoryNoInfo.htm");
                TemplateHandler.Assign(ref html, "noino", Resources.Language.thereisnodata);
                //Assign header
            }
            else
            {
             
                html = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Report/GoalHistory.htm");
                string row = TemplateHandler.CopyBlock(html, "row");
                TemplateHandler.Assign(ref html, "sport", CacheRef.GetSportName(sporttype));
                TemplateHandler.Assign(ref html, "league", CacheRef.GetLeagueName(leagueid));
                TemplateHandler.Assign(ref html, "home", (!isNeuTral) ? CacheRef.GetTeamName(homeid) : CacheRef.GetTeamName(homeid) + Resources.Language.IsNeutral);
                TemplateHandler.Assign(ref html, "away", CacheRef.GetTeamName(awayid));
                string data = "";
                int homeScore = 0;
                int awayScore = 0;
                for (int i = 0; i < goalrecord.Length; i++)
                {
                    string tempRow = row;
                    string teamScore = "";
                    if (goalrecord.Substring(i, 1).ToLower().Equals("h"))
                    {
                        homeScore += 1;
                        teamScore = Resources.Language.lblHome;
                    }
                    else
                    {
                        awayScore += 1;
                        teamScore = Resources.Language.lblAway;
                    }
                    TemplateHandler.Assign(ref tempRow, "score", homeScore.ToString() + " - " + awayScore.ToString());
                    TemplateHandler.Assign(ref tempRow, "teamscore", teamScore);
                    data += tempRow;
                }

                TemplateHandler.ReplaceBlock(ref html, "row", data);

            }
            return html;
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            return string.Empty;
        }
        
    }
}
