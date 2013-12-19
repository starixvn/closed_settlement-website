<%@ WebHandler Language="C#" Class="DirectLogin" %>

using System;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.IBCDLL;

public class DirectLogin : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        const int dwKeySize = 1024;
        var param = string.Empty;
        string strParams = string.Empty;
        param = context.Request["loginInfo"];
        var strPath = context.Request.PhysicalApplicationPath + "/IbcKeys/ibckeydec.snk";
        var encryption = new RSACryptoPad.EncryptionThread();

        if (string.IsNullOrEmpty(param)) this.LogOut();


        param = param.Replace(' ', '+');
        strParams = encryption.DecryptString(param, dwKeySize, strPath);

        if (!string.IsNullOrEmpty(strParams))
        {

            string[] ps = strParams.Split('&');
            if (ps.Length > 0)
            {

                string username = ps[0].Trim();
                int accId = Convert.ToInt32(ps[1]);
                string onAccId = ps[2];
                var isOnline = UserController.OnlineUserUpdate(Convert.ToInt32(onAccId));
                if (!isOnline)
                {
                    this.LogOut();
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(username, false);
                    AppSession.CurrentSystemName = WebSettings.SystemName;
                    AppSession.AccUserName = username;
                    AppSession.Username = username;
                    AppSession.AccId = accId;
                    AppSession.OnAccId = Sunplus.Accounting.DataAccess.IBCDLL.UserController.AccLoginAdminOnuser(accId, WebSettings.ServerId, context.Request.UserHostAddress);
                    AuthManager.AssignRoles(accId);
                    var hasSettlementPermission = Permissions.HasPermission(AppSession.CurrentSystemName);
                    if (AppSession.RoleAccess == null || AppSession.PageAccess == null || AppSession.RoleAccess.Count == 0
                        || AppSession.PageAccess.Count == 0 || !hasSettlementPermission)
                    {
                        context.Session.Clear();
                        FormsAuthentication.SignOut();
                        this.LogOut();
                    }



                    var menuService = CommonBootStrapper.Locator.GetInstance<IMenuManager>();

                    menuService.GetUserMenu();

                    context.Response.Redirect(FormsAuthentication.DefaultUrl.Replace("https", "http"));
                }

            }
            else
            {
                this.LogOut();
            }
        }
        else
        {
            this.LogOut();
        }

    }


    private void LogOut()
    {
        HttpContext.Current.Session.Clear();
        HttpContext.Current.Response.Redirect(System.Web.Security.FormsAuthentication.LoginUrl);
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}