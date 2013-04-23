<%@ WebHandler Language="C#" Class="ProcessSwapVersion" %>

using System;
using System.Web;
using System.Web.Security;
using System.Collections;
using System.Web.SessionState;
using DNAide.Data;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess;
using Sunplus.Core.LoggingLib;


public class ProcessSwapVersion : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        context.Response.Write("<html><head><script type='text/javascript'>\n");
        try
        {
            string custId = DNAide.Core.CastAide.AsString(context.Request["uid"], "0");
            string sessionId = DNAide.Core.CastAide.AsString(context.Request["sid"], "0");
            Hashtable param = new Hashtable();
            param.Add("custid", custId);
            param.Add("sessionid", sessionId);
            param.Add("result", false);
            bool succ = false;

            DbResult ds = DbHelper.ExecuteCmd("Acc_LoginClaimed_SwitchVersion", param, DBType.ON_USER);
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
                    int onUserId = Sunplus.Accounting.DataAccess.IBCDLL.UserController.AccLoginAdminOnuser(custIdInt, WebSettings.ServerId, context.Request.UserHostAddress);
                    FormsAuthentication.SetAuthCookie(username, false);
                    AppSession.Username = username;//context.Session["Username"] = username;
                    AppSession.AccId = custIdInt;//context.Session["AccId"] = custIdInt;
                    AppSession.AccUserName = username;
                    AppSession.OnAccId = onUserId;//context.Session["OnAccId"] = onUserId;
                    AppSession.RoleID = roleId;//context.Session["roleId"] = roleId;
                    AppSession.AccPwd = pwd;
                    AuthManager.AssignRoles(custIdInt);
                    succ = true;
                    //AppCache.AccessPermission = null;
                }
            }
            if (succ)
            {
                context.Response.Write("top.location.href='" + FormsAuthentication.DefaultUrl + "';\n");
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

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
