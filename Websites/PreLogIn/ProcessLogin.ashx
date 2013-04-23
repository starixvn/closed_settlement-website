<%@ WebHandler Language="C#" Class="ProcessLogin" %>

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Core.LoggingLib;
using IBCUtils;
using System.Data;


public class ProcessLogin : IHttpHandler,IRequiresSessionState 
{
    
    public void ProcessRequest (HttpContext context) 
    {
        try
        {

            context.Response.ContentType = "text/html";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string username = context.Request.Form["TxtUsername"];
            string password = context.Request.Form["TxtPassword"];

            context.Response.Write("<html><head><script type='text/javascript'>\n");
            int custId;
            string system;
            int roleId;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                context.Response.Write("parent.signIn.loginError('Username and Password must not be empty!');\n");

            if (LoginFailedHandler.IsLocked(username))
            {
                context.Response.Write(LoginFailedHandler.WriteJS(username));
                goto end;
            }
            //string countryName = IpManagement.ReplaceCountryName("202.78.230.82");
            //var ipManager = new Sunplus.Accounting.Core.Helper.IP.IpManager();
            //string countryName = ipManager.GetCountryNameByIp(context.Request.UserHostAddress);

            AppSession.CurrentSystemName = WebSettings.SystemName;
            
            string browserName = context.Request.Browser.Browser + " " + context.Request.Browser.Version;
            IbcError error = Sunplus.Accounting.DataAccess.IBCDLL.UserController.AccLoginAdmin(ref username, password, 
                context.Request.UserHostAddress , context.Request.UrlReferrer.AbsoluteUri, 
                out custId, out roleId, out system, context.Request.Url.Host, browserName, 
                context.Request.Browser.Platform, "");
            
            if (error.IsSuccess())
            {
            
                LoginFailedHandler.LoginSuccess(username);

                FormsAuthentication.SetAuthCookie(username, false);
                AppSession.Username = username;
                AppSession.AccUserName = username;
                AppSession.AccId = custId;

                

                AppSession.OnAccId = Sunplus.Accounting.DataAccess.IBCDLL.UserController.AccLoginAdminOnuser(custId, WebSettings.ServerId, context.Request.UserHostAddress);
                AppSession.RoleID = roleId;
                AppSession.AccPwd = IBCUtils.Encryption.CfsEnCode(password);
                

                
                AuthManager.AssignRoles(custId);                
                var hasSettlementPermission = Permissions.HasPermission(AppSession.CurrentSystemName);
                if (AppSession.RoleAccess == null || AppSession.PageAccess == null || AppSession.RoleAccess.Count == 0
                    || AppSession.PageAccess.Count == 0 || !hasSettlementPermission)
                {
                    context.Session.Clear();
                    FormsAuthentication.SignOut();
                    context.Response.Write("parent.signIn.loginError('" + Resources.ErrorMsg.DenyLoginAccount +  "');\n");
                    goto end;
                }
                
                
                context.Response.Write("top.location.href='" + FormsAuthentication.DefaultUrl.Replace("https", "http") + "';\n");
                IMenuManager menuService = CommonBootStrapper.Locator.GetInstance<IMenuManager>();
                
                menuService.GetUserMenu();
            }
            else
            {
                if (LoginFailedHandler.IsLocked(username))
                {
                    context.Response.Write(LoginFailedHandler.WriteJS(username));
                }
                else
                {
                    LoginFailedHandler.LoginFailed(username, error.ErrorCode);

                    //-----------------------------------------------------------------------

                    if (LoginFailedHandler.IsLocked(username))
                    {
                        context.Response.Write(LoginFailedHandler.WriteJS(username));
                    }
                    else
                    {
                        context.Response.Write("parent.signIn.loginError('" + IBCUtils.Encryption.RemoveSpecialChars(error.ErrorMsg) + "');\n");
                    }
                }
            }
        end:
            context.Response.Write("\n</script></head><body></body>");
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}
