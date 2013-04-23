<%@ WebHandler Language="C#" Class="EditUsername" %>

using System;
using System.Web;
using System.Web.SessionState;
using CasinoLibrary;
using CasinoLibrary.SBAgent;
using System.Data;

using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;


public class EditUsername : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        if (context.Session["AccId"] != null)
        {
            context.Response.Write("<html><head></head><body>");
            context.Response.Write("<script type=\"text/javascript\">");
            try
            {
                string oldUser = context.Request["txtOldUname"] as string ?? string.Empty;
                oldUser = oldUser.Trim();
                string newUser = context.Request["txtNewUname"] as string ?? string.Empty;
                newUser = newUser.Trim();
                string accId = context.Session["AccId"].ToString();

                int custid = 0;
                 CustomerDetail custcs = Sunplus.Accounting.DataAccess.APIClient.IBCLibProvider.Instance.GetCustomerDetail(oldUser);
                 if (WebSettings.disableCS && custcs.IsCasinoSync)
                 {

                     string errCS = string.Format(Resources.ErrorMsg.error_LockUpdateStatus, "CHANGE USERNAME");
                     context.Response.Write("parent.alert_error('" + IBCUtils.Encryption.RemoveSpecialChars(errCS) + "','errMsg');");
                 }
                 else
                 {
                     IBCUtils.IbcError err = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.CustomerUpdUserNameRpt(oldUser, newUser, accId, out custid);

                     bool success = err.IsSuccess();
                     if (custid != 0 && success) // Have been synchronized casino
                     {
                        
                         
                         if (!WebSettings.disableCS)
                         {
                             CasinoLibrary.SBAgent.Customer csCustomer = new CasinoLibrary.SBAgent.Customer();
                             success = csCustomer.UpdateUserName(custid, newUser, context.Request.UserHostAddress, int.Parse(accId), AppSession.Username, 3);
                         }
                     }
                     if (success)
                     {
                         context.Response.Write("parent.parent.jQuery.unblockUI();parent.alert_error('" + AlertMessage.GetMsg("Update CustId successfully.", AlertMessage.MsgType.Success) + "','errMsg');");
                     }
                     else
                     {
                         context.Response.Write("parent.parent.jQuery.unblockUI();parent.alert_error('" + AlertMessage.GetMsg(err.ErrorMsg, AlertMessage.MsgType.Error) + "','errMsg');");
                     }
                 }
            }
            catch(Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                context.Response.Write("parent.parent.jQuery.unblockUI();parent.alert_error('" + AlertMessage.GetMsg(Resources.Language.thereisnodata, AlertMessage.MsgType.Error) + "','errMsg');");
            }
            finally
            {
                context.Response.Write("</script>");
                context.Response.Write("</body></html>");
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}
