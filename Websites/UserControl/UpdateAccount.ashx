<%@ WebHandler Language="C#" Class="UpdateAccount" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using IBCUtils;
using DNAide.Core;
using System.Collections;


public class UpdateAccount : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        context.Response.Write("<script type=\"text/javascript\">");
        try
        {
            string type = context.Request["type"] as string ?? "";
            int userId = CastAide.AsInt32(context.Session["AccId"]);            
            IbcError error = null;
            Hashtable param = new Hashtable();            
            
            param.Add("language", "E");
            param.Add("errcode", 0);
            param.Add("errmsg", "");
            if (type.Equals("add"))
            {
                param.Add("groupid", context.Request["selGroup"]);
                param.Add("firstname", context.Request["txtFName"]);
                param.Add("lastname", context.Request["txtLName"]);
                param.Add("userid", context.Session["AccId"]);
                param.Add("username", context.Request["txtUName"].Trim());
                param.Add("userpwd", IBCUtils.Encryption.CfsEnCode(context.Request["txtPwd"].Trim()));
                param.Add("custid", 0);
                error = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.InsertAdmin(param);
                if (error.ErrorCode != 0)
                {
                    context.Response.Write("parent.accountManagement.utils.showAlertMessage('errfunc','" + error.ErrorMsg + "', 1);");
                }
                else
                {
                    context.Response.Write("parent.$.msg();");
                    context.Response.Write("parent.location = parent.location.href+'?eraseCache=true';");
                }
                context.Response.Write("parent.parent.jQuery.unblockUI();");
            }
            else if (type.Equals("edit"))
            {
                var json = (new System.Web.Script.Serialization.JavaScriptSerializer()).Deserialize<Customer>(context.Request.Form[0].ToString());

                int custId = json.custid;
                
                param.Add("custid", custId);
                param.Add("groupid", json.groupid);
                param.Add("firstname", json.firstname);
                param.Add("lastname", json.lastname);
                param.Add("userid", context.Session["AccId"]);
                error = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.UpdateAdmin(param);
                if (error.ErrorCode != 0)
                {
                }
                else
                {
                }                
            }
            else if (type.Equals("exist"))
            {                
                param.Add("username", context.Request["txtEUName"]);                
                param.Add("creator", context.Session["AccId"]);
                param.Add("groupid", context.Request["selEGroup"]);
                error = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.AddExistedAdmin(param);
                if (error.ErrorCode != 0)
                {
                    context.Response.Write("parent.accountManagement.utils.showAlertMessage('errfunc','" + error.ErrorMsg + "', 1);");                    
                }
                else
                {
                    context.Response.Write("parent.$.msg();");
                    context.Response.Write("parent.location = parent.location.href+'?eraseCache=true';");
                }
                context.Response.Write("parent.parent.jQuery.unblockUI();");
            }
            else if (string.Compare(type, "status", true) == 0)
            {
                string active = context.Request["active"] as string ?? string.Empty;
                bool isClosed = (string.Compare(active, "true", true) == 0) ? false : true;
                int custId = CastAide.AsInt32(context.Request["custId"]);
                int accId = CastAide.AsInt32(context.Session["AccId"]);
                string oldGroupId = CastAide.AsString(context.Request["groupId"]);
                string gname = CastAide.AsString(context.Request["dept"]);
                error= Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.AccountAdminStatusUpdate(custId, accId, isClosed);
                if (error.ErrorCode != 0)
                {
                    context.Response.Write("parent.accountManagement.utils.showAlertMessage('errfunc','" + error.ErrorMsg + "', 1);");                    
                }
                else
                {
                    context.Response.Write("parent.$.msg();");
                    context.Response.Write("parent.location = parent.location.href+'?eraseCache=true';");
                }
                context.Response.Write("parent.parent.jQuery.unblockUI();");              
            }      
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        context.Response.Write("</script>");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}

public class Customer
{
    public int custid { get; set; }
    public string groupid { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
}