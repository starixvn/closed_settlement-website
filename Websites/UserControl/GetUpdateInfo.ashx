<%@ WebHandler Language="C#" Class="GetUpdateInfo" %>

using System;
using System.Web;
using System.Data;
using System.Web.SessionState;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using IBCUtils.Web;
using IBCUtils.DAL;
using DNAide.Core;
using Sunplus.Accounting.DataAccess.IBCDLL;
using System.Data.SqlClient;


public class GetUpdateInfo : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";        
        context.Response.Write(GetGroupList());
        
    }   
    private string GetGroupList()
    {
        string content = string.Empty;
        try
        {
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.GroupInfoSelect())
            {                
                content += "0,[Select]|";
                while (dr.Read())
                {             
                    content += CastAide.AsString(dr["groupid"]) + "," + CastAide.AsString(dr["groupname"]) + "|";
                }                
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex); 
        }
        return content;

    }
    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }      
}