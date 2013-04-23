<%@ WebHandler Language="C#" Class="LastestNewsList" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Business.Others;

public class LastestNewsList : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        
        // Generate news list
        News news = new News();
        String lastNewsList = news.ShowLastNewsRotaion();
        context.Response.Write(lastNewsList);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}