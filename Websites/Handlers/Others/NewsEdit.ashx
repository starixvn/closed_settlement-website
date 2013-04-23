<%@ WebHandler Language="C#" Class="NewsEdit" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Business.Others;

public class NewsEdit : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {

        // Get Page Index
        String strNewsID = DNAide.Core.CastAide.AsString(context.Request["NewsID"]);
        int newsID = 1;
        Int32.TryParse(strNewsID, out newsID);

        // Generate html data        
        News news = new News();
        String newsList = news.ShowNewsEditing(newsID);
        context.Response.Write(newsList);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}