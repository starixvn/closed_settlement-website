<%@ WebHandler Language="C#" Class="GetCountDownTime" %>

using System;
using System.Web;

public class GetCountDownTime : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        context.Response.ContentType = "text/plain";
        context.Response.Write(LoginFailedHandler.GetRemainingTime(context.Request.QueryString["username"]));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}