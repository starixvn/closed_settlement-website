<%@ WebHandler Language="C#" Class="UpdatePolicy" %>

using System;
using System.Web;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using IBCUtils;
using DNAide.Core;

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public class UpdatePolicy : IHttpHandler,IRequiresSessionState {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        context.Response.Write("<script type=\"text/javascript\">");        
        try
        {
            string custId = context.Request["custId"] as string ?? string.Empty;
            string groupId = context.Request["groupId"] as string ?? string.Empty;
            string dept = context.Request["dept"] as string ?? string.Empty;
            if (!custId.Equals(""))
            {
                UpdateAdmin(context, custId,dept);
            }
            else if (custId.Equals("") && !groupId.Equals(""))
            {
                UpdateGroup(context, groupId);
            }
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            context.Response.Write("parent.accountManagement.utils.showAlertMessage('errfunc','" + ex.Message + "',1);");
            context.Response.Write("parent.parent.jQuery.unblockUI();");
        }
        context.Response.Write("</script>");
        
    }
    private void UpdateAdmin(HttpContext context,string custId,string dept)
    {
        //bool isUpdated = false;
        string memberPolicies = string.Empty;
        string[] id = null;                
        IbcError error = null;        
        string funcId = context.Request["arrFuncId"] as string ?? string.Empty;
        string arrUser = context.Request["user"] as string ?? string.Empty;
        arrUser = arrUser.Trim();
        int userId = CastAide.AsInt32(context.Session["AccId"]);
        string[] arrFuncId = null;
        if (funcId.Length > 0)
        {
            arrFuncId = funcId.Split(',');
        }       
        id = custId.Split(',');
        if (id.Length > 0)
        {
            for (int k = 0; k < id.Length; k++)
            {
                if (id[k] != "")
                {
                    var dict = new Dictionary<string, string>();
                    for (int i = 0; i < arrFuncId.Length; i++)
                    {
                        var val = "0"; var func = arrFuncId[i];
                        if(!dict.ContainsKey(func))
                            dict.Add(func, val);

                        if (context.Request["chkR" + arrFuncId[i]] != null && context.Request["chkH" + arrFuncId[i]] != null)
                        {
                            val = "3";
                        }
                        else if (context.Request["chkR" + arrFuncId[i]] != null)
                        {
                            val = "1";
                        }
                        else if (context.Request["chkH" + arrFuncId[i]] != null)
                        {
                            val = "2";
                        }
                        // reset val
                        dict[func] = val;
                    }
                    memberPolicies = ToStringValue(dict);
                    error = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.UpdateAdminPolicy(memberPolicies, userId, CastAide.AsInt32(id[k]));
                }
            }
            if (error.ErrorCode != 0)
            {
                context.Response.Write("parent.accountManagement.utils.showAlertMessage('errfunc','" + error.ErrorMsg + "',1);");
            }
            else
            {
                context.Response.Write("parent.accountManagement.utils.showAlertMessage('errfunc','Update role policy successfully.',0);");
            }
            context.Response.Write("parent.parent.jQuery.unblockUI();");
        }
    }

    private void UpdateGroup(HttpContext context, string groupId)
    {        
        string groupPolicies = string.Empty;        
        IbcError error = null;
        string funcId = context.Request["arrFuncId"] as string ?? string.Empty;        
        int userId = CastAide.AsInt32(context.Session["AccId"]);
        string[] arrFuncId = null;
        if (funcId.Length > 0)
        {
            arrFuncId = funcId.Split(',');
        }
        var dict = new Dictionary<string, string>();
        for (int i = 0; i < arrFuncId.Length; i++)
        {
            if (arrFuncId[i] != "")
            {
                var val = "0"; var func = arrFuncId[i];
                if (!dict.ContainsKey(func))
                    dict.Add(func, val);
                
                if (context.Request["chkR" + arrFuncId[i]] != null && context.Request["chkH" + arrFuncId[i]] != null)
                {
                    val = "3";
                }
                else if (context.Request["chkR" + arrFuncId[i]] != null)
                {
                    val = "1";
                }
                else if (context.Request["chkH" + arrFuncId[i]] != null)
                {
                    val = "2";
                }
                // reset val
                dict[func] = val;
            }
        }
        groupPolicies = ToStringValue(dict);
        error = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.UpdateGroupPolicy(groupPolicies, userId, CastAide.AsInt32(groupId));                   
        if (error.ErrorCode != 0)
        {
            context.Response.Write("parent.accountManagement.utils.showAlertMessage('errfunc','" + error.ErrorMsg + "',1);");
        }
        else
        {            
            context.Response.Write("parent.accountManagement.utils.showAlertMessage('errfunc','Update role policy successfully.',0);");
        }
        context.Response.Write("parent.parent.jQuery.unblockUI();");
    }

    private string ToStringValue(Dictionary<string, string> dict)
    {
        string strValue = string.Empty;
        List<string> arrVal = new List<string>();
        foreach (var item in dict) 
        {
            arrVal.Add(item.Key + "," + item.Value);
        }
        strValue = string.Join(";", arrVal.ToArray<string>());
        return strValue;
    }
    public bool IsReusable {
        get {
            return false;
        }
    }
}
