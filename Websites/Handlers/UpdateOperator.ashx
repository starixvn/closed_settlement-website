<%@ WebHandler Language="C#" Class="UpdateOperator" %>

using System;
using System.Web;
using System.Web.SessionState;
using System.Collections;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;


public class UpdateOperator : BaseHandler {
    
    public override void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        context.Response.Write("<script type=\"text/javascript\">");
        if (context.Session["AccId"] == null)
        {

        }
        else
        {
            try
            {
                Hashtable resultUpdated = new Hashtable();
                Hashtable hash = new Hashtable();
                hash.Add("custid", context.Request["operatorId"]); //int
                hash.Add("firstname", context.Request["txtFirstname"]); //nvarchar
                hash.Add("lastname", context.Request["txtLastname"]); //nvarchar
                hash.Add("address", context.Request["txtAddress"]); //nvarchar
                hash.Add("state", context.Request["txtState"]); //nvarchar
                hash.Add("country", context.Request["listCountry"]); //nvarchar
                hash.Add("phone", context.Request["txtPhone"]); //nvarchar
                hash.Add("mobilephone", context.Request["txtMobile"]); //nvarchar
                hash.Add("fax", context.Request["txtFax"]); //nvarchar
                hash.Add("email", context.Request["txtEmail"]); //nvarchar
                hash.Add("roleid", context.Request["listRole"]); //int
                hash.Add("postal", context.Request["txtPCode"]); //nvarchar
                hash.Add("city", context.Request["txtCity"]); //nvarchar
                hash.Add("userid", context.Session["AccId"]); //int
                resultUpdated = UpdateCustomer.CustomerUpdOperatorRpt(hash);
                if (Convert.ToString(resultUpdated["errCode"]) == "0")
                {
                    context.Response.Write("parent.afterUpdate(false, 2000);");
                }
                else
                {
                    string errMsg=resultUpdated["errMsg"] as string ?? string.Empty;
                    context.Response.Write("parent.alert_error('" + AlertMessage.GetMsg(errMsg, AlertMessage.MsgType.Error) + "','errMsg')");
                }
            }
            catch(Exception ex)
            {
               
                GotoErroPage(context);
            }
            finally
            {
                context.Response.Write("</script>");
            }
        }    
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
