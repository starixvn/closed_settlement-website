<%@ WebHandler Language="C#" Class="OnlineUpd" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.DataAccess.IBCDLL;
using Sunplus.Core.LoggingLib;



public class OnlineUpd : IHttpHandler , IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        try
        {
            context.Response.ContentType = "text/html";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            int onlineId = AppSession.OnAccId;
            bool isOnline = false;
            context.Response.Write("<html><head></head><body>");
            string appPath = context.Request.ApplicationPath;
            if (appPath != "/") appPath += "/";
            if (onlineId != 0)
                isOnline = UserController.OnlineUserUpdate(onlineId);
            if (!isOnline)
            {
                //#region Remove Cache Item from Cache
                //((ASP.global_asax)context.ApplicationInstance).RemoveSessionCacheItem(
                //    DNAide.Core.CastAide.AsString(AppSession.AccId), AppSession.Username, DNAide.Core.CastAide.AsString(AppSession.RoleID));
                //#endregion
                context.Response.Write("<script type='text/javascript'>alert('Another session has been logged in or your session is expired. Forced to logout!');top.location.href='" + appPath + "PreLogIn/Logout.ashx';</script>");
            }
            //else
            //{
            //    #region Access Cache Item
            //    string msSessionCacheValue =
            //        ((ASP.global_asax)context.ApplicationInstance).GetSessionCacheItem(
            //        DNAide.Core.CastAide.AsString(AppSession.OnAccId), AppSession.Username, DNAide.Core.CastAide.AsString(AppSession.RoleID));
            //    #endregion
            //}

            
            
            context.Response.Write("</body></html>");
        }
        catch(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);

            context.Response.Write("<html><head></head><body><script type='text/javascript'>top.location.href='../PreLogIn/LogOut.ashx';</script></body></html>");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
