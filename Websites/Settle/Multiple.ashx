<%@ WebHandler Language="C#" Class="Multiple" %>

using System;
using System.Web;
using Sunplus.Accounting.Business.Settlement;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Web.SessionState;
using System.Text;
using System.Data;
using IBCUtils.DAL;
using IBCUtils.Web;
using DNAide.Core;
using DNAide.Data;


public class Multiple : IHttpHandler, IRequiresSessionState
{
    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    private string nl = Environment.NewLine;
    private int custID = -1, matchID = -1, sportID = -1;
    private string type = string.Empty;
    private string permissions = "";

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        string _s = "Internal server error!";
        try
        {
            custID = Convert.ToInt32(context.Session["AccId"] as object ?? -1);
            if (custID == -1) throw new AccException(IBCUtils.IbcError.ACCESS_DENIED_MSG);
            permissions = Sunplus.Accounting.Business.Security.Permissions.CheckHostPermissionSettle(custID);                      
            type = context.Request.Form["type"];
            string _params = context.Request.Form["p"];
            string matchIDs = "";
            matchID = CastAide.AsInt32(_params.Split('|')[0]);
            sportID = CastAide.AsInt32(_params.Split('|')[1]);
           
            if (permissions.IndexOf("[C]") > -1)
            {
                Sunplus.Accounting.Entities.ErrorDetail rs = null;                
                if (string.Compare(type, "m", true) == 0) // multiple
                {
                   
                    if (sportID == 0)
                    {
                        matchIDs = _params.Split('|')[0];
                        rs = DoSettle.Multiple1(custID, matchIDs);                        
                        if (rs.ErrorCode > 0)
                            _s = rs.ErrorMessage;
                        else _s = "Calculate Multiple Successfully!";
                        
                    }
                    else {
                        //if (DoSettle.Multiple(custID, matchID)) _s = "Calculate Multiple Successfully!";
                        rs = DoSettle.Multiple1(custID, matchIDs);
                        if (rs.ErrorCode > 0)
                            _s = rs.ErrorMessage;
                        else _s = "Calculate Multiple Successfully!";
                    }
                    
                }
                else if (string.Compare(type, "r", true) == 0) // refund multiple
                {
                    rs = DoSettle.RefundMultiple(custID, matchID);                    
                    if (rs.ErrorCode > 0)
                        _s = rs.ErrorMessage;
                    else _s = "Refund Multiple Successfully!";
                }
                else if (string.Compare(type, "rht", true) == 0) // refund multiple FT
                {
                    rs = DoSettle.RefundMultipleHT(custID, matchID);                    
                    if (rs.ErrorCode > 0)
                        _s = rs.ErrorMessage;
                    else _s = "Refund Multiple HT Successfully!";
                }    
                    
                else if (string.Compare(type, "rft", true) == 0) // refund multiple FT
                {
                    rs = DoSettle.RefundMultipleFT(custID, matchID);
                    if (rs.ErrorCode > 0)
                        _s = rs.ErrorMessage;
                    else _s = "Refund Multiple FT Successfully!";
                }
                
                try
                {
                    var service = new Fanex.Tarus.Apollo.Settlement.Business.SettlementService();
                    string _matchids = matchID.ToString();
                    if (type.ToLower() == "m")
                        _matchids = matchIDs;


                    service.Set_ConfirmedAction_Parlay(_matchids, ActionKey(type.ToLower()));                       
                }
                catch (Exception ex)
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
               
            }
        }
        catch (AccException ex)
        {
            _s = ex.Message;
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        context.Response.Write(_s);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private int ActionKey(string type)
    {
        var rs = 0;
        switch (type)
        {
            case "unsettle": // 0: Unsettle
                rs = 0;
                break;
            case "calculate": // 1: Calculate FG/NG
                rs = 1;
                break;
            case "billht": // 2: Calculate HT
                rs = 2;
                break;
            case "billft": // 3: Calculate FT
                rs = 3;
                break;
            case "bill": // 4: Calculate All
                rs = 4;
                break;
            case "m": // 5: Calculate Parlay
                rs = 5;
                break;
            case "refundhalftime": // 11: Refund HT
                rs = 11;
                break;
            case "refundfulltime": // 12: Refund FT
                rs = 12;
                break;
            case "refund": // 13: Refund All
                rs = 13;
                break;
            case "rht": // 14: Refund Parlay HT
                rs = 14;
                break;
            case "rft": // 15: Refund Parlay FT
                rs = 15;
                break;
            case "r": // 16: Refund Parlay All
                rs = 16;
                break;
            case "refundfirstnogoal": // 17: Refund All FGLG
                rs = 17;
                break;
            case "refundalllastgoal": // 18: Refund All LG
                rs = 18;
                break;
        }
        return rs;
    }

}
