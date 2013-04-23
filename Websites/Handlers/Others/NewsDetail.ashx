<%@ WebHandler Language="C#" Class="NewsDetail" %>

using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Business.Others;
using Sunplus.Accounting.Entities.Others;

public class NewsDetail : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {

        // Get Page Index
        int newsID = 0;
        int appID = 0;
        
        String strNewsID = DNAide.Core.CastAide.AsString(context.Request["NewsID"]);
        String strAppID = CastAide.AsString(context.Request["AppID"]);

        Int32.TryParse(strAppID, out appID);
        Int32.TryParse(strNewsID, out newsID);

        // Generate html data
        var newsDetail = new NewsItem();
        if (newsID > 0)
        {
            var news = new News();
            newsDetail = news.GetNewsDetail(newsID, appID);            
        }
        
        context.Response.ContentType = "application/json";
        
        var newsItem = newsDetail ?? new NewsItem();
        
        context.Response.Write(ToJson(newsItem));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    public string ToJson(Object obj)
    {
        string jsonString = "";
        if (obj != null)
        {
            var oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            jsonString = oSerializer.Serialize(obj);
        }
        return jsonString;
    }
}