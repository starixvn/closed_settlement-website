<%@ WebHandler Language="C#" Class="SwapVersion" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Core.LoggingLib;


public class SwapVersion : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        try
        {
            context.Response.Write(WebSettings.SwapVersionUrl);
            System.Web.Security.FormsAuthentication.SignOut();
            context.Session.Clear();
            
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
            
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
