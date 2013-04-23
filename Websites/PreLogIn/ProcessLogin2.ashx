<%@ WebHandler Language="C#" Class="ProcessLogin2" %>

using System;
using System.Web;
using System.Web.Security;
using System.Collections;
using System.Web.SessionState;
using DNAide.Data;

using IBCUtils;

using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess;
using Sunplus.Core.LoggingLib;


public class ProcessLogin2 : IHttpHandler,IRequiresSessionState {

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        context.Response.Write("<html><head><script type='text/javascript'>\n");
        try
        {
            string custId = DNAide.Core.CastAide.AsString(context.Request["uid"], "0");
            string logId = DNAide.Core.CastAide.AsString(context.Request["lid"], "0");
            //Set CustID=0 to select by username

            string sessionId = DNAide.Core.CastAide.AsString(context.Request["sid"], "0");
            Hashtable param = new Hashtable();
            param.Add("custid", custId);
            param.Add("sessionid", sessionId);
            param.Add("logid", logId);
            param.Add("result", false);
            bool succ = false;
            string system;

            AppSession.CurrentSystemName = WebSettings.SystemName;

            DbResult ds = DbHelper.ExecuteCmd("UserLoginClaimed_out", param, DBType.ON_USER);
            if (Convert.ToBoolean(ds.Outputs["result"]))
            {
                param.Clear();
                param.Add("custid", custId);

                ds = DbHelper.Execute("Acc_Con_AdminSelByCustID", param, DBType.DB03);

                if (ds.ResultSet.Tables[0].Rows.Count > 0)
                {
                    string username = ds.ResultSet.Tables[0].Rows[0]["username"] + "";
                    int custIdInt = Convert.ToInt32(custId);

                    int roleId = Convert.ToInt32(ds.ResultSet.Tables[0].Rows[0]["roleid"]);
                    string pwd = ds.ResultSet.Tables[0].Rows[0]["userpwd"] + "";
                    int onUserId = Sunplus.Accounting.DataAccess.IBCDLL.UserController.AccLoginAdminOnuser(
                        custIdInt, WebSettings.ServerId, context.Request.UserHostAddress);


                    string browserName = context.Request.Browser.Browser + " " + context.Request.Browser.Version;
                    IbcError error = Sunplus.Accounting.DataAccess.IBCDLL.UserController.AccLoginAdmin(
                        ref username,
                        IBCUtils.Encryption.CfsCode(pwd),
                        context.Request.UserHostAddress,
                        context.Request.UrlReferrer.AbsoluteUri,
                        out custIdInt,
                        out roleId,
                        out system,
                        context.Request.Url.Host,
                        browserName,
                        context.Request.Browser.Platform,
                        "");


                    if (error.IsSuccess())
                    {

                        if (!AuthManager.IsValidSystem(system))
                        {
                            context.Session.Clear();
                            succ = false;
                        }
                        else
                        {
                            FormsAuthentication.SetAuthCookie(username, false);
                            AppSession.Username = username; //context.Session["Username"] = username;
                            AppSession.AccUserName = username;
                            AppSession.AccId = custIdInt; //context.Session["AccId"] = custIdInt;
                            AppSession.CurrentSystemNAME = Enumerations.AccountingSystem.Accounting;
                            AppSession.SYSTEM = Enumerations.AccountingSystem.Accounting.ToString();
                            AppSession.OnAccId = onUserId; //context.Session["OnAccId"] = onUserId;
                            AppSession.RoleID = roleId; //context.Session["roleId"] = roleId;
                            AppSession.AccPwd = pwd;
                            AuthManager.AssignRoles(custIdInt);

                            var hasSettlementPermission = Permissions.HasPermission(AppSession.CurrentSystemName);
                            if (AppSession.RoleAccess == null || AppSession.PageAccess == null || AppSession.RoleAccess.Count == 0
                                || AppSession.PageAccess.Count == 0 || !hasSettlementPermission)
                            {
                                context.Session.Clear();
                                FormsAuthentication.SignOut();
                                succ = false;

                            }
                            else
                            {
                                succ = true;
                            }


                        }

                    }







                    IMenuManager menuService = CommonBootStrapper.Locator.GetInstance<IMenuManager>();
                    menuService.GetUserMenu();
                }
            }

            if (succ)
            {
                context.Response.Write(
                    "top.location.href='" + FormsAuthentication.DefaultUrl
                    +
                    (!String.IsNullOrEmpty(context.Request.QueryString["uname"])
                         ? "?uname=" + DNAide.Core.CastAide.AsString(context.Request.QueryString["uname"])
                         : string.Empty) + "';\n");
            }
            else
            {
                context.Response.Write("top.location.href='" + FormsAuthentication.LoginUrl + "';\n");
            }

        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        context.Response.Write("\n</script></head><body></body>");
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}
