<%@ WebHandler Language="C#" Class="LogOut" %>

using System;
using System.Web;
using System.Web.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;


public class LogOut : IHttpHandler,System.Web.SessionState.IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        try
        {

            context.Response.ContentType = "text/html";
            int accOnId = 0;
            if (context.Session["OnAccId"] != null)
            {
                accOnId = (int)context.Session["OnAccId"];
            }
            //#region Remove Cache Item from Cache
            //((ASP.global_asax)context.ApplicationInstance).RemoveSessionCacheItem(
            //    DNAide.Core.CastAide.AsString(AppSession.OnAccId),
            //    AppSession.Username, DNAide.Core.CastAide.AsString(AppSession.RoleID));
            //#endregion
            Sunplus.Accounting.DataAccess.IBCDLL.UserController.AccLogOut(accOnId, AppSession.Username, Sunplus.Accounting.Core.Configurations.WebSettings.ServerId);
            context.Session.Clear();
            FormsAuthentication.SignOut();
            if (context.Request.Cookies != null)
            {
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName) {Expires = DateTime.Now.AddDays(-1)};
                context.Response.Cookies.Add(cookie);
                FormsAuthentication.SignOut();
            }

            context.Response.Write("<html><head></head><body><script type='text/javascript'>top.location.href='" + FormsAuthentication.LoginUrl + "';</script></body></html>");
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
