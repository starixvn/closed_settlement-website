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

}
