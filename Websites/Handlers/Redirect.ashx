<%@ WebHandler Language="C#" Class="Redirect" %>

using System;
using System.Web;
using System.Web.SessionState;

public class Redirect : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        try
        {

            context.Response.ContentType = "text/html";
            if (context.Request.Url.AbsoluteUri.Contains("https"))
            {


                context.Response.Write("<html><head></head><body><script type='text/javascript'>top.location.href='" +
                                       HttpContext.Current.Request.Url.AbsoluteUri.Replace("https", "http") + "';</script></body></html>");
            }
        }
        catch (Exception ex)
        {

        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}