using System;
using System.Data;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Web;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;

public partial class Settle_Index : PageBase
{
    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    private string nl = Environment.NewLine;
    private string fromDate, toDate;
    protected string title = "";




    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            fromDate = IBCUtils.FormatHandler.FormatDateEn(DateTime.Now.AddDays(-1));
            toDate = IBCUtils.FormatHandler.FormatDateEn(DateTime.Now);

            string s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/SelectType.htm");

            string mode = Request.QueryString["m"];
            bool isCalulate = true;
            if (string.Compare(mode, "overonly", true) == 0) // over only
            {
                isCalulate = false;
                TemplateHandler.Assign(ref s, "{mode}", "OVER");
                title = "Over Only";
            }
            else if (string.Compare(mode, "htoveronly", true) == 0) // HT over only
            {
                isCalulate = false;
                TemplateHandler.Assign(ref s, "{mode}", "HTOVER");
                title = "HT Over Only";
            }
            else
            {
                TemplateHandler.Assign(ref s, "{mode}", "CALC");
                title = "Calculate";
            }

            TemplateHandler.Assign(ref s, "{sLeagues}", LeagueString(fromDate, toDate, isCalulate, mode));
            this.searchContent.Text = s.Replace(Environment.NewLine, string.Empty).Replace("  ", string.Empty);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }

    private string LeagueString(string fromDate, string toDate, bool isCalculate, string mode)
    {
        string s = string.Empty;
        try
        {
            s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/SelectLeague.htm");
            if (mode != null && (mode.ToLower().Equals("overonly") || mode.ToLower().Equals("htoveronly")))
                s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/SelectLeagueOverOnly.htm");

            if (!isCalculate) TemplateHandler.RemoveBlock(ref s, "calc");
            TemplateHandler.Assign(ref s, "{fromdate}", Calendar.GenCalendarControl("fdate", "../", "fdateTrigger", fromDate, false, false, 0, 0, "ChangeSport();"));
            TemplateHandler.Assign(ref s, "{todate}", Calendar.GenCalendarControl("tdate", "../", "tdateTrigger", toDate, true, false, 0, 0, "ChangeSport();"));
            TemplateHandler.Assign(ref s, "{selectLeague}", Resources.Language.selectLeague);
            TemplateHandler.Assign(ref s, "{ThemePath}", WebSettings.ThemePath);
            StringBuilder _sSport = new StringBuilder();
            StringBuilder _sLeague = new StringBuilder();
            if (string.Compare(mode, "htoveronly", true) == 0)
            {
                _sSport.Append("<option value='1'>Soccer</option>" + nl);
                _sSport.Append("<option value='2'>Basketball</option>" + nl);
                _sSport.Append("<option value='3'>Football</option>" + nl);
                _sSport.Append("<option value='32'>Netball</option>" + nl);
            }
            else
            {
                using (IDataReader dr = Settle.SettleSelectSport())
                {
                    while (dr.Read())
                    {
                        string _sportName = CastAide.AsString(dr["sportname"]);
                        string _sportType = CastAide.AsString(dr["sporttype"]);
                        if (!_sportType.Equals("201") && !_sportType.Equals("251") && !_sportType.Equals("252"))
                        {
                            _sSport.Append("<option value='" + _sportType + "'>" + _sportName + "</option>" + nl);
                        }
                    }
                }
                _sLeague.Append("<option value='0' selected='selected'>*..::¤ Show All Leagues  ¤::..*</option>" + nl);
                using (IDataReader dr = Settle.SettleSelectLeague(CastAide.AsDateTime(fromDate), CastAide.AsDateTime(toDate), 1))
                {
                    while (dr.Read())
                    {
                        if (!CastAide.AsString(dr["sporttype"]).Equals("151") && !CastAide.AsString(dr["sporttype"]).Equals("201") && !CastAide.AsString(dr["sporttype"]).Equals("251") && !CastAide.AsString(dr["sporttype"]).Equals("252"))
                        {
                            string _leaguename = (string)dr["leaguename"];
                            string _leagueid = dr["leagueid"].ToString();
                            _sLeague.Append("<option value='" + _leagueid + "'>" + _leaguename.Replace("\"", "'") + "</option>" + nl);
                        }
                    }
                }
            }
            TemplateHandler.Assign(ref s, "{sportOptions}", _sSport.ToString());
            TemplateHandler.Assign(ref s, "{leagueOptions}", _sLeague.ToString());
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return s;
    }
}