<%@ WebHandler Language="C#" Class="NewsDelete" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Business.Others;

public class NewsDelete : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {

        int newsID = 0;
        int appID = 0;
        String htmlData="";
        string strNewsID =  CastAide.AsString(context.Request["NewsID"]);
        string strAppID = CastAide.AsString(context.Request["AppID"]);

        Int32.TryParse(strNewsID, out newsID);
        Int32.TryParse(strAppID, out appID);
        
        // Delete News
        if (newsID > 0)
        {
             var news = new News();
            htmlData = news.DeleteNews(newsID, appID);
        }
        
        // Response
        context.Response.Write(htmlData);        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}