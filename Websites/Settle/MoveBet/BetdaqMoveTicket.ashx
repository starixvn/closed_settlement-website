<%@ WebHandler Language="C#" Class="BetdaqMoveTicket" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Business.Settlement;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Helper;

public class BetdaqMoveTicket : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        int matchid = CastAide.AsInt32(context.Request["matchid"], 0);
        int newMatchid = CastAide.AsInt32(context.Request["newmatchid"], 0);
        string matchcode = CastAide.AsString(context.Request["matchcode"]);
        string errmsg = "", msg = "";
        string newMatchcode = "";
        bool f = DoSettle.Settlement_BetDaq_Move(matchid, newMatchid, ref errmsg, ref newMatchcode);
        var error = new ErrorDetail();
        if(!f)
        {
            error.ErrorCode = 101;
            error.ErrorMessage = "<span style='color:#ff0000'>" + errmsg + "</span>";
            context.Response.Write(JsonUtil.ToJson("Result", error));
            return;
            
        }
        error.ErrorCode = 0;
        error.ErrorMessage = "<span style='color:#0000ff'> Betdaq tickets have been moved from the old match <b>" + matchcode +
                             "</b> to the new created match <b>" + newMatchcode + "</b> successfully. </span>";
        context.Response.Write(JsonUtil.ToJson("Result", error));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}