using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities.Others;

namespace Sunplus.Accounting.WcfServiceLibrary.Others
{
    [ServiceContract]
    public interface IReleaseLogService
    {
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        NewsData GetNewsList(int page, int appId);

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        NewsItem GetNewsDetail(int newsId, int appId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string InsNews(NewsItem newsItem);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string UpdNews(NewsItem newsItem);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string DelNews(int newsId, int appId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string HotNews(int newsId, int feature, int appId);
    }
}
