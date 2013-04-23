<%@ WebHandler Language="C#" Class="NewsList" %>

using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Business.Others;
using System.Collections.Generic;
using Sunplus.Accounting.Entities.Others;

public class NewsList : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {

        // Get Page Index
        int appID = 0;
        int pageIndex = 1;
        String strPageIndex = DNAide.Core.CastAide.AsString(context.Request["PageIndex"]);
        String strAppID = CastAide.AsString(context.Request["AppID"]);

        Int32.TryParse(strAppID, out appID);
        Int32.TryParse(strPageIndex, out pageIndex);

        // Generate html data
        
        var news = new News();

        var newsData = news.GetNewsPage(pageIndex, appID);
        newsData = newsData ?? new Sunplus.Accounting.Entities.Others.NewsData();
        context.Response.ContentType = "application/json";

        var jsoncallback = CastAide.AsString(context.Request["jsoncallback"], "");

        if (string.IsNullOrEmpty(jsoncallback))
        {
            context.Response.Write(ToJson(newsData));
        }
        else
        {
            context.Response.Write(jsoncallback + "(" + ToJson(newsData) + ")");
        }
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    
    public string ToJson(Object obj)
    {
        string jsonString = "";
        if (obj!=null)
        {
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            jsonString = oSerializer.Serialize(obj);	
        }
        return jsonString;
    }
}