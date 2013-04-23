<%@ WebHandler Language="C#" Class="NewsUpdate" %>

using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Business.Others;
using Sunplus.Accounting.Core.Common;

public class NewsUpdate : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {

        String errorMessage = "";
        String htmlData = "";
        
        int newsID = 0;
        int appID = 0;
        int hotFeature = CastAide.AsInt32(context.Request["HotFeature"], 0);
        Int32 feature = CastAide.AsInt32(context.Request["Feature"]);

        DateTime newsDate = DateTime.Now;
        String strNewsID = CastAide.AsString(context.Request["NewsID"]);
        String strNewsDate = CastAide.AsString(context.Request["NewsDateTextBox"]);
        String strNewsContent = CastAide.AsString(context.Request["NewsContentTextArea"]);
        String strNewsTitle = CastAide.AsString(context.Request["NewsTitleTextBox"]);
        String strAppID = CastAide.AsString(context.Request["AppID"]);
        String func = CastAide.AsString(context.Request["Func"], "");
        String thumbnail = CastAide.AsString(context.Request["Thumbnail"], "");


        Int32.TryParse(strNewsID, out newsID);
        DateTime.TryParse(strNewsDate, out newsDate);
        Int32.TryParse(strAppID, out appID);


        bool isError = false;
        
        if (hotFeature == 1)
        {
            News news = new News();
            htmlData = news.HotFeature(appID, newsID, feature);
            htmlData = string.IsNullOrEmpty(htmlData) ? "Success" : htmlData;
            context.Response.Write(htmlData);
        }
        else
        {
            var ownerID = AppSession.AccId;
            News news = new News();
            // Update News
            if (newsID > 0)
            {
                errorMessage = news.UpdateNews(newsID, strNewsTitle, strNewsContent, newsDate, func, feature, thumbnail, appID, ownerID);
            }
            else
            {
                errorMessage = news.InsertNews(strNewsTitle, strNewsContent, newsDate, func, feature, thumbnail, appID, ownerID, out newsID);
            }

            // Response
            isError = errorMessage.Length > 0 ? true : false;
            htmlData = String.Format("{0}|{1}|{2}", isError, errorMessage, newsID);
            context.Response.Write(htmlData);  
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