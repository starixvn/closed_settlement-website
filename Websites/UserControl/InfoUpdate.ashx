<%@ WebHandler Language="C#" Class="InfoUpdate" %>

using System;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using IBCUtils.Web;
using IBCUtils;
using Sunplus.Accounting.DataAccess.IBCDLL;
using IBCUtils.DAL;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using System.Web.SessionState;


public class InfoUpdate : IHttpHandler, IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        try
        {            
            int accId = Convert.ToInt32(context.Session["AccId"] as object ?? -1);
            if (accId == -1) throw new AccException(IbcError.ACCESS_DENIED_MSG);
            
            int custId = Convert.ToInt32(context.Request["custid"] as object ?? -1);
            if (custId == -1) throw new AccException(IbcError.USERNAME_UNAVAILABLE_MSG);
            string type = context.Request["type"] as string ?? string.Empty;
            if (string.Compare(type, "status", true) == 0)
            {
                string active = context.Request["active"] as string ?? string.Empty;
                bool isClosed = (string.Compare(active, "true", true) == 0) ? false : true;                
                Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.AccountAdminStatusUpdate(custId, accId, isClosed);
                context.Response.Write("<script type='text/javascript'>parent.location.replace(parent.location);</script>");
            }
            else if (string.Compare(type, "access", true) == 0)
            {
                string access = context.Request["access"] as string ?? string.Empty;
                bool role = Convert.ToBoolean(context.Request["role"] as object ?? false);
                Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.AccountAdminPageAccessUpdate(custId, accId, role, access);                
            }
        }
        catch (AccException ex)
        {
            ShowError(context, ex.Message.ToString()); 
        }  
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }                        
    }

    private void ShowError(HttpContext context, string msg)
    {        
        context.Response.Write("<script type='text/javascript'>alert('" + msg + "');</script>");
        context.Response.Write("<script type='text/javascript'>parent.location.replace(parent.location);</script>");
        return;
    }
  
    public bool IsReusable {
        get {
            return false;
        }
    }
}