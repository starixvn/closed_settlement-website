<%@ WebHandler Language="C#" Class="ProcessData" %>

using System;
using System.Web;
using DNAide.Web;
using DNAide.Utils;
using DNAide.Core;
using System.Web.SessionState;
using Sunplus.Accounting.Business.Settle.Log;
using Sunplus.Accounting.Business.Settlement;

public class ProcessData : IHttpHandler, IRequiresSessionState{
    
    public void ProcessRequest (HttpContext context) {
       context.Response.ContentType = "text/html";
        int mode = CastAide.AsInt32(context.Request["mode"], 0);
        switch (mode)
        { 
            case 1:
                string goalrecord = CastAide.AsString(context.Request["goalrecord"]);
                context.Response.Write(Calculate.ShowGoalHistory(goalrecord));
                break;
            case 2:
                int matchid = CastAide.AsInt32(context.Request["matchid"],0);                
                string matchcode = CastAide.AsString(context.Request["matchcode"]);
                string errmsg = "", NewMatchcode = "", msg = "";
                bool f = DoSettle.Settlement_BetDaq_Move(matchid,ref errmsg, ref NewMatchcode);
                if (f) msg = "ok|You have moved Betdaq tickets from the old match <b>" + matchcode + "</b> to the new created match <b>" + NewMatchcode + "</b> successfully.";
                else msg = "no|" + errmsg;
                context.Response.Write(msg);
                break;
            case 3:
                var log = new MatchSettleLog();
                context.Response.Write(log.LoadMatchInfo(DNAide.Core.CastAide.AsString(context.Request["matchcode"]).Replace("^", "&"), context.Request["fdate"], context.Request["tdate"]));
                break;
            default:
                context.Response.Write("Invalid data!!!");
                break;
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}