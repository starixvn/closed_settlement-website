<%@ WebHandler Language="C#" Class="FunctionParent" %>

using System;
using System.Web;
using System.Text;
using System.Web.SessionState;
using DNAide.Web;
using DNAide.Core;
using System.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;

using System.Linq;

public class FunctionParent : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

        string system = context.Request.Form["system"];
        int parentid = CastAide.AsInt32(context.Request.Form["parentid"]);
        StringBuilder strListParent = new StringBuilder();
        try
        {
            using (var dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.FunctionParent(system))
            {
                strListParent.Append("<option value=''>None</option>");
                while (dr.Read())
                {                    
                    if (parentid == 0)
                    {
                        strListParent.Append("<option value=\"" + dr["FuncId"].ToString() + "\">" + dr["FuncName"].ToString() + "</option>");
                    }
                    else
                    {
                        if (CastAide.AsInt32(dr["FuncId"]) == parentid)
                        {
                            strListParent.Append("<option selected value=\"" + dr["FuncId"].ToString() + "\">" + dr["FuncName"].ToString() + "</option>");
                        }
                        else
                        {
                            strListParent.Append("<option value=\"" + dr["FuncId"].ToString() + "\">" + dr["FuncName"].ToString() + "</option>");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }

        context.Response.Write("<select id=\"slParent\" name=\"slParent\" style=\"width: 150px;\">" + strListParent.ToString() + "</select>");
        
        //if (parentid == 0)
        //{
        //    context.Response.Write("<select id=\"slParent\" name=\"slParent\" style=\"width: 150px;\">" + strListParent.ToString() + "</select>");
        //}
        //else
        //{
        //    context.Response.Write("<select disabled id=\"slParent\" name=\"slParent\" style=\"width: 150px;\">" + strListParent.ToString() + "</select>");
        //}
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}