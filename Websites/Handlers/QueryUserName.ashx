<%@ WebHandler Language="C#" Class="QueryUserName" %>

using System;
using System.Web;
using System.Collections.Specialized;
using System.Data;
using DNAide.Core;
using DNAide.JSon;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Common;

public class JsonData
{
    public string query;
    public StringCollection suggestions;
    public string[] data = new string[] { "" };
}

public class QueryUserName : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        //context.Response.ContentType = "text/plain";
        context.Response.ContentType = "application/json";
        
        string query = CastAide.AsString(context.Request.QueryString["query"], String.Empty);
        int userid = AppSession.AccId;

        DNAide.Data.DbResult result = Sunplus.Accounting.DataAccess.IBCDLL.Customer.SelectCustomer.QueryUserName(userid, query, AppSession.SYSTEM, Permissions.HasPermission
                (Enumerations.Functions.FinancialManagement.ViewFinanceAccount));

        JsonData userNameList = new JsonData();
        userNameList.query = query;
        userNameList.suggestions = DataTable2Collection(result.ResultSet.Tables[0]);

        string jsonUsername = JsonMapper.ToJson(userNameList);
        context.Response.Write(jsonUsername);
    }

    public StringCollection DataTable2Collection(DataTable tbl)
    {
        StringCollection suggestion = new StringCollection();
        foreach (DataRow row in tbl.Rows)
        {
            suggestion.Add(CastAide.AsString(row[0]));
        }
        return suggestion;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}