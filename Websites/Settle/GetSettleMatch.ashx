<%@ WebHandler Language="C#" Class="GetSettleMatch" %>

using System;
using System.Web;
using DNAide.Web;
using Sunplus.Accounting.Business.Settlement;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Web.SessionState;
using System.Globalization;
using System.Data;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Business.Constants;

public class GetSettleMatch : IHttpHandler, IRequiresSessionState
{
    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    private string nl = Environment.NewLine;
    private string fdate, tdate;
    int custID = 0, sportID = 1, leagueID = 0;
    bool isUncal = false, isUncalMul = false;
    string mode = string.Empty;
    string permissions = "";
    //isCalculate | fdate | tdate | sportID | leagueID | isUncal | isUncalMul
    public void ProcessRequest(HttpContext context)
    {
        try
        {
            context.Response.ContentType = "text/html";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            custID = Convert.ToInt32(context.Session["AccId"] as object ?? -1);
            if (custID == -1) throw new AccException(IBCUtils.IbcError.ACCESS_DENIED_MSG);
            //fdate = IBCUtils.FormatHandler.FormatDateEn(context.Request.Form["fdate"]);
            CultureInfo culture = new CultureInfo("en-US");
            DateTime dtFDate = CastAide.AsDateTime(context.Request.Form["fdate"], DateTime.Now, culture);
            DateTime dtTDate = CastAide.AsDateTime(context.Request.Form["tdate"], DateTime.Now, culture);
            fdate = dtFDate.ToString("MM/dd/yyyy");
            tdate = dtTDate.ToString("MM/dd/yyyy");
            sportID = Convert.ToInt32(context.Request.Form["sportID"]);
            mode = context.Request.Form["mode"];


            DbResult dr = Settle.GetSettlePermission(custID);
            SettleBgColor.HasHostPermission = (Convert.ToInt32(dr.Outputs["errcode"]) == 0) ? true : false;

            // Get string permissions
            permissions = Sunplus.Accounting.Business.Security.Permissions.CheckHostPermissionSettle(custID);

            string _s = string.Empty;
            if (sportID == 161)//Hardcode for Number Game
            {
                leagueID = Convert.ToInt32(context.Request.Form["leagueID"]);
                _s = SettlementNumberGame.GetNumberGameForSettlement(dtFDate.ToString(), dtTDate.ToString(), leagueID, permissions);
            }
            else
            {
                DataSet dsResult = null;
                if (string.Compare(mode, "CALC", true) == 0) // Calculate
                {
                    leagueID = Convert.ToInt32(context.Request.Form["leagueID"]);
                    isUncal = (context.Request.Form["isUncal"] == "1") ? true : false;
                    isUncalMul = (context.Request.Form["isUncalMul"] == "1") ? true : false;
                    dsResult = Settle.SettleSelectMatch(sportID, dtFDate, dtTDate, leagueID, isUncal, isUncalMul);
                }
                else if (string.Compare(mode, "HTOVER", true) == 0) // HT Over ONLY
                {
                    dsResult = Settle.SettleSelectCompleteHTMatch_soccer(dtFDate, sportID);
                }
                else if (string.Compare(mode, "OVER", true) == 0) // Over Only
                {
                    dsResult = Settle.SettleSelectCompleteMatch(sportID, dtFDate);
                }

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    _s = GenerateHTML(dsResult.Tables[0].Select());
                    _s = _s.Replace(Environment.NewLine, string.Empty).Replace("  ", " ");
                }
                else
                {
                    _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("Settle/NoInfo.htm");
                    TemplateHandler.Assign(ref _s, "{fdate}", fdate);
                    TemplateHandler.Assign(ref _s, "{tdate}", tdate);
                }

            }

            context.Response.Write(_s);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }

    private string GenerateHTML(DataRow[] rows)
    {
        if (string.Compare(mode, "CALC", true) == 0) // Calculate
        {
            switch (sportID)
            {
                case 1: // Soccer
                    return Calculate.Soccer(sportID, rows, 12, fdate, tdate, permissions);
                case 2: // basketball
                case 3: // football
                case 26: // rugby
                case 32: // NetBall
                    return Calculate.Basketball(sportID, rows, 17, fdate, tdate, permissions);
                case 4: // ice hockey
                case 28: //filed hockey
                    return Calculate.Hockey(sportID, rows, 16, fdate, tdate, permissions);
                case 5: // tennis
                    return Calculate.Tennis(sportID, rows, 18, fdate, tdate, permissions);
                case 10: // golf
                case 11: // Motorsports
                case 7: // billiard
                case 25: // Darts
                case 29://Winter Sport
                case 33://     Cycling
                case 34://     Fencing
                case 35://     Judo
                case 36://     Modern Pentathlon
                case 37://     Rowing
                case 38://     Sailing
                case 39://     Shooting
                case 40://     Taekwondo
                case 41://     Triathlon
                case 42://     Wrestling
                case SportConstants.HorseRacingFixedOdds:
                    return Calculate.Billiard(sportID, rows, 12, fdate, tdate, permissions);
                case 8: // baseball
                    return Calculate.BaseBall(sportID, rows, 23, fdate, tdate, permissions);
                case 6: // volleyball
                    return Calculate.Badminton(sportID, rows, 18, fdate, tdate, permissions);
                case 9: // badminton
                case 30://Squash
                    return Calculate.Badminton(sportID, rows, 17, fdate, tdate, permissions);
                case 151://Racing
                    return Calculate.HorseRacing(rows, fdate, tdate, sportID.ToString(), permissions);
                case 161://Number Game
                    return Calculate.NumberGame(sportID, rows, 17, fdate, tdate, permissions);
                default: return Calculate.CommonSportType(sportID, rows, 13, fdate, tdate, permissions);
            }
        }
        else if (string.Compare(mode, "HTOVER", true) == 0) // HT Over ONLY
        {
            return OverOnly.Soccer(sportID, rows, 12, fdate, tdate, permissions, true);
        }
        else if (string.Compare(mode, "OVER", true) == 0) // Over Only
        {
            switch (sportID)
            {
                case 1: // soccer
                    return OverOnly.Soccer(sportID, rows, 13, fdate, tdate, permissions, false);
                case 2: // basketball
                case 3: // football
                case 26: // rugby
                case 32: // netball
                    return OverOnly.Basketball(sportID, rows, 17, fdate, tdate, permissions);
                case 4: // ice hockey
                case 28: //filed hockey
                    return OverOnly.Hockey(sportID, rows, 14, fdate, tdate, permissions);
                case 5: // tennis
                    return OverOnly.Tennis(sportID, rows, 18, fdate, tdate, permissions);
                case 10: // golf
                case 11: // Motorsports
                case 7: // billiard
                case 25: // Darts
                case SportConstants.HorseRacingFixedOdds:
                    return OverOnly.Billiard(sportID, rows, 12, fdate, tdate, permissions);
                case 8: // baseball
                    return OverOnly.BaseBall(sportID, rows, 23, fdate, tdate, permissions);
                case 9: // badminton
                case 6: // volleyball
                    return OverOnly.Badminton(sportID, rows, 18, fdate, tdate, permissions);
                default: return OverOnly.CommonSportType(sportID, rows, 13, fdate, tdate, permissions);
            }
        }
        return string.Empty; // Unknown :-o
    }



    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
