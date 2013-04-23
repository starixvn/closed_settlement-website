<%@ WebHandler Language="C#" Class="UpdateDept" %>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using Newtonsoft.Json;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;
using IBCUtils;
using DNAide.Core;


public class UpdateDept : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        context.Response.Write("<script type=\"text/javascript\">");
        try
        {
            string type = context.Request["type"] as string ?? "";
            int userId = CastAide.AsInt32(context.Session["AccId"]);            
            IbcError error=null;
            if (type.Equals("add"))
            {                
                string groupN = CastAide.AsString(context.Request["txtDept"]);
                string groupD = CastAide.AsString(context.Request["txtDesc"]);
                error= Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.InsertGroup(groupN, groupD, userId);
                if (error.ErrorCode != 0)
                {
                    context.Response.Write("parent.accountManagement.utils.showAlertMessage('errfunc','" + error.ErrorMsg + "', 1);");                    
                }
                else
                {
                    context.Response.Write("parent.accountManagement.viewAllDepartments.viewModelBackup = null;");
                    context.Response.Write("parent.accountManagement.viewAllDepartments.LoadDepartmentInfomation();");
                    context.Response.Write("parent.accountManagement.utils.showAlertMessage('errfunc','" + groupN + " was inserted!.', 0);");                    
                }                
            }
            else if (type.Equals("edit"))
            {
                var json = (new System.Web.Script.Serialization.JavaScriptSerializer()).Deserialize<Department>(context.Request.Form[0].ToString());

                int groupId = json.groupId;
                string groupN = json.txtDept;
                string groupD = json.txtDesc;
                
                Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.UpdateGroup(groupId,groupN,groupD,userId);
            }
            else if (type.Equals("del"))
            {
                int groupId = CastAide.AsInt32(context.Request["groupId"]);
                error = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.DeleteGroup(groupId, userId);
                if (error.ErrorCode != 0)
                {
                    context.Response.Write("parent.accountManagement.utils.showAlertMessage('errfunc','" + AlertMessage.GetMsg(error.ErrorMsg, AlertMessage.MsgType.Error) + "', 1);");
                }
                else
                {
                    context.Response.Write("parent.accountManagement.viewAllDepartments.deleteDepartment('" + groupId.ToString() + "');");
                }
                context.Response.Write("parent.parent.jQuery.unblockUI();");
            }            
        }
        catch (Exception ex)
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

[Serializable]
public class Department
{
    public int groupId { get; set; }
    public string txtDept { get; set; }
    public string txtDesc { get; set; }
}