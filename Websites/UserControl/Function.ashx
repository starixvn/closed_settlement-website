<%@ WebHandler Language="C#" Class="Function" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Business.Security;
using Sunplus.Core.LoggingLib;


public class Function : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        string[] strListUser = AccCommon.FuncMgr.Split(';');
        bool isMgr = false;
        for (int i = 0; i < strListUser.Length; i++)
            if (string.Compare(context.Session["Username"].ToString().Trim(), strListUser[i], true) == 0)
            {
                isMgr = true;
                break;
            }
        if (!isMgr)
        {
            context.Response.Write("<script type='text/javascript'>parent.location.href = '../AccessDenied.aspx';</script>");
            return;
        }
        string strName = context.Request.Form["txtName"];
        string strDesc = context.Request.Form["txtDesc"];
        string strFileNames = context.Request.Form["txtFileNames"];
        strFileNames = string.IsNullOrEmpty(strFileNames) ? "" : strFileNames.Replace("\r\n", "");
        
        //var oldParent = context.Request.Form["hdParentId"];
        string strParent = context.Request.Form["slParent"];
        //strParent = string.IsNullOrEmpty(strParent) ? oldParent : strParent;
        
        string system = context.Request.Form["slsystem"];
        string strAdd = context.Request.Form["hdAdd"];
        try
        {
            int userId = CastAide.AsInt32(context.Session["AccId"]);

            IBCUtils.IbcError error = null;
            int funcId = CastAide.AsInt32(context.Request["hdFuncId"], 0);
            switch (strAdd)
            {
                case "0"://Edit
                    string Upsystem = context.Request.Form["hdsystem"];
                    if (funcId == 0)
                        context.Response.Write("<script type='text/javascript'>parent.UpdateErr('You must choose at least one funcion to update.');</script>");
                    if (!string.IsNullOrEmpty(strName) && !string.IsNullOrEmpty(strFileNames) && !string.IsNullOrEmpty(strAdd) && !string.IsNullOrEmpty(strDesc))
                        error = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.UpdateFunction(funcId, userId, strName, strDesc, strFileNames, Upsystem, strParent);
                    break;
                case "1"://Add
                    if (!string.IsNullOrEmpty(strName) && !string.IsNullOrEmpty(strFileNames) && !string.IsNullOrEmpty(strAdd) && !string.IsNullOrEmpty(strDesc))
                        error = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.InsertFunction(strName, strDesc, strFileNames, strParent, userId, system);
                    break;
                case "2"://Delete
                    if (funcId == 0)
                        context.Response.Write("<script type='text/javascript'>parent.UpdateErr('You must choose at least one funcion to update.');</script>");
                    error = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.DeleteFunction(funcId, userId);
                    break;
            }
            Permissions.InitAccessPermission();
            if (error.ErrorCode != 0)
            {
                context.Response.Write("<script type='text/javascript'>parent.UpdateErr(\"" + error.ErrorMsg + "\");</script>");
                return;
            }
            else
            {
                context.Response.Write("<script type='text/javascript'>parent.UpdateSucc();</script>");
                return;
            }

        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            context.Response.Write("<script type='text/javascript'>parent.UpdateErr(\"" + ex.Message + "\");</script>");
            return;
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}
