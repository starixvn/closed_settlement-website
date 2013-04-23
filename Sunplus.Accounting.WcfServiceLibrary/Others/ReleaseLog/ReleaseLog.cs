using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Sunplus.Accounting.Business.Others;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities.Others;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Extensions;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Net.Mime;
using System.Text;
using Sunplus.Accounting.Core.Web;
using DNAide.Web;

namespace Sunplus.Accounting.WcfServiceLibrary.Others
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ReleaseLogService : IReleaseLogService
    {
        public NewsData GetNewsList(int page, int appId)
        {
            var news = new News();
            var newsData = news.GetNewsPage(page, appId);
            return newsData;
        }

        public NewsItem GetNewsDetail(int newsId, int appId)
        {
            var news = new News();
            var newsData = news.GetNewsDetail(newsId, appId);
            return newsData;
        }

        public string InsNews(NewsItem newsItem)
        {
            var newsId = 0;
            var news = new News();
            newsItem.OwnerID = AppSession.AccId;
            if (newsItem.NewsID != 0)
            {
                return UpdNews(newsItem);
            }
            var errorMessage = news.InsertNews(FormatUnicode(newsItem.Title), FormatUnicode(newsItem.NewsContent), Convert.ToDateTime(newsItem.Date),
                                           newsItem.Func, newsItem.Feature, newsItem.Thumbnail, newsItem.AppID,
                                           newsItem.OwnerID, out newsId);
            var isError = errorMessage.Length > 0 ? true : false;
            if (!isError)
            {
                SendNotification(FormatUnicode(newsItem.Recipient), FormatUnicode(newsItem.Title), FormatUnicode(newsItem.NewsContent));
            }
            return string.Format("{0}|{1}|{2}", isError, errorMessage, newsId);
        }

        public string UpdNews(NewsItem newsItem)
        {
            var news = new News();
            newsItem.OwnerID = AppSession.AccId;
            var errorMessage = news.UpdateNews(newsItem.NewsID, FormatUnicode(newsItem.Title), FormatUnicode(newsItem.NewsContent), Convert.ToDateTime(newsItem.Date), newsItem.Func, newsItem.Feature, newsItem.Thumbnail, newsItem.AppID, newsItem.OwnerID);
            var isError = errorMessage.Length > 0 ? true : false;
            if (!isError)
            {
                SendNotification(FormatUnicode(newsItem.Recipient), FormatUnicode(newsItem.Title), FormatUnicode(newsItem.NewsContent));
            }
            return string.Format("{0}|{1}|{2}", isError, errorMessage, newsItem.NewsID);
        }

        public string DelNews(int newsId, int appId)
        {
            var news = new News();
            var errorMessage = news.DeleteNews(newsId, appId);
            return string.IsNullOrEmpty(errorMessage) ? "Success" : errorMessage;
        }

        public string HotNews(int newsId, int feature, int appId)
        {
            var news = new News();
            var errorMessage = news.HotFeature(appId, newsId, feature);
            return string.IsNullOrEmpty(errorMessage) ? "Success" : errorMessage;
        }

        private void SendNotification(string recipient, string subject, string content)
        {
            var mailRelease = MailSection.Instance.Send.Cast<MailElement>().Where(mail => mail.Name == "ReleaseLog").SingleOrDefault<MailElement>();
            if (!string.IsNullOrEmpty(recipient) && mailRelease != null)
            {
                var contentHtml = WebUtils.LoadTemplate("Others/MailReleaseLog.htm");
                TemplateHandler.Assign(ref contentHtml, "{content}", content);
                //var contentHtml = "<html><body>" + content + "</body></html>";
                Regex rex = new Regex("<img[^<>]+>");
                AlternateView altView = null;
                var linkedList = new List<LinkedResource>();
                var matches = rex.Matches(contentHtml);
                foreach (var match in matches)
                {
                    var matchTmp = match.ToString().Replace("/>", ">").Replace(">", "/>");
                    contentHtml = contentHtml.Replace(match.ToString(), matchTmp);
                     try 
                     {
                        var xmlLoad = XElement.Parse(matchTmp);
                        var imgUrl = (from attr in xmlLoad.Attributes() 
                                      where attr.Name.ToString().IndexOf("src") != -1 && attr.Value.IndexOf("http") == -1
                                      select attr.Value).SingleOrDefault<string>();
                        if (!string.IsNullOrEmpty(imgUrl))
                        {
                            var fullPath = HttpContext.Current.Server.MapPath("~/Others/ReleaseLog/") + imgUrl;
                            var hashImg = System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.Default.GetBytes(imgUrl));
                            var sBuilder = (from hash in hashImg select hash.ToString("x2")).ToArray<string>();
                            var contentId = string.Join("", sBuilder);
                       
                            // replace image src
                            LinkedResource linkedImage = new LinkedResource(fullPath, MediaTypeNames.Image.Gif);
                            linkedImage.ContentId = contentId;
                            linkedList.Add(linkedImage);
                            contentHtml = contentHtml.Replace(matchTmp, "<img src=\"cid:" + contentId + "\"/>");
                        
                        }
                    }
                    catch (Exception ex) { }
                }
                if (linkedList.Count > 0)
                {
                    altView = AlternateView.CreateAlternateViewFromString(contentHtml, Encoding.UTF8, MediaTypeNames.Text.Html);
                    foreach (var item in linkedList)
                    {
                        altView.LinkedResources.Add(item);
                    }
                }
                SendMailExtension.AccSendMail(MailSection.HostName.Gmail, recipient, mailRelease.Cc, string.Empty, mailRelease.Title, subject, contentHtml, null , altView);
            }
        }

        private string FormatUnicode(string text) 
        {
            return HttpUtility.UrlDecode(text, Encoding.UTF8);
        }
    }
}
