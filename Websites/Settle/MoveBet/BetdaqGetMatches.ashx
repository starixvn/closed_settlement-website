<%@ WebHandler Language="C#" Class="BetdaqGetMatches" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class BetdaqGetMatches : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        var dtMatches = new System.Data.DataTable();
        var error = new ErrorDetail();
        var outData = new JObject();
        var matchid = DNAide.Core.CastAide.AsInt32(context.Request["matchid"], 0);
        if(AppSession.AccId == 0)
        {
            error.ErrorCode = -1;
            error.ErrorMessage = "Session Timeout";
            outData["Error"] = new JValue(JsonConvert.SerializeObject(error));
            context.Response.Write(JsonConvert.SerializeObject(outData));
            return;
        }
        dtMatches = Settle.Settlement_BetDaq_SelectMatches(matchid);
        
        if (dtMatches.Rows.Count == 0)
        {
            error.ErrorCode = 1;
            error.ErrorMessage = "<span style='color:#ff0000'>" + Resources.ErrorMsg.betdaq_NoMatch + "</span>";
        }
        else
        {
            error.ErrorCode = 0;
        }
        
        outData["Error"] = new JValue(JsonConvert.SerializeObject(error));
        outData["Matches"] = new JValue(JsonConvert.SerializeObject(dtMatches));

        context.Response.Write(JsonConvert.SerializeObject(outData));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}