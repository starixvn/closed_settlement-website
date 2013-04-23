<%@ WebHandler Language="C#" Class="UpdateAccInformation" %>

using System;
using System.Web;
using System.Collections;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;


public class UpdateAccInformation : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";        
        try
        {
            int accid = Convert.ToInt32(context.Session["AccId"] as object ?? -1);
            if (accid == -1) throw new AccException("Invalid Session!");                                    
            Hashtable param = new Hashtable();
            param.Add("custid", accid);
            param.Add("firstname", context.Request["firstname"]);
            param.Add("lastname", context.Request["lastname"]);
            param.Add("address", context.Request["address"]);
            param.Add("postal", context.Request["postal"]);
            param.Add("city", context.Request["city"]);
            param.Add("state", context.Request["state"]);
            param.Add("country", context.Request["listCountry"]);
            param.Add("phone", context.Request["phone"]);
            param.Add("mobilephone", context.Request["mobile"]);
            param.Add("fax", context.Request["fax"]);
            param.Add("email", context.Request["email"]);
            param.Add("birthday", context.Request["birthday"]);
            param.Add("userid", accid);
            Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccPersonalInfo.AccInformationUpdate(param);            
            context.Response.Write("<script type='text/javascript'>parent.alert_error('" + AlertMessage.GetMsg("Your information is updated successfully!", AlertMessage.MsgType.Success) + "', 'errMsg');</script>");            
        }
        catch (AccException ex)
        {
            LogExtensions.ExceptionHandler(ex); 
            context.Response.Write("<script type='text/javascript'>alert('" + ex.Message.ToString() + "');</script>");
        }
            
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
