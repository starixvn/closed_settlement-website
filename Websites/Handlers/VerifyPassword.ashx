<%@ WebHandler Language="C#" Class="VerifyPassword" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils.DAL;
using IBCUtils;


public class VerifyPassword : IHttpHandler, IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        try
        {
            context.Response.ContentType = "text/html";
            string accname = AppSession.AccUserName;
            int accid = AppSession.AccId;
            if (string.IsNullOrEmpty(accname) || accid == -1) throw new AccException(IbcError.ACCESS_DENIED_MSG);
            string password = context.Request["password"] as string ?? string.Empty;
            if (string.IsNullOrEmpty(password) || password.Length < 4)
                throw new AccException("Password must have at least 4 characters!");
            DNAide.Data.DbResult dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccPersonalInfo.AccPasswordVerify(accname, accid, IBCUtils.Encryption.CfsEnCode(password));
            if ((int)dr.Outputs["errcode"] > 0) 
                context.Response.Write("<script type='text/javascript'>parent.alert_error('" + dr.Outputs["errmsg"].ToString() + "','errMsg');</script>");
            else
                context.Response.Write("<script type='text/javascript'>parent.location.replace('../EditPersonalInfo.aspx');</script>");
        }
        catch (AccException ex)
        {
            context.Response.Write("<script type='text/javascript'>parent.alert_error('" + AlertMessage.GetMsg(ex.Message.ToString(), AlertMessage.MsgType.Error) + "','errMsg');</script>");
        }
        catch (Exception ex) {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
    }
      
    public bool IsReusable {
        get {
            return false;
        }
    }

}
