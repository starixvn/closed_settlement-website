
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DNAide.Core;
using DNAide.Data;

using Sunplus.Accounting.Core.Cache.Extension;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Web.TplCore.App_Templates.Others.App_Templates.Others;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.Provider.Others;
using Sunplus.Accounting.Entities.Others;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for News
/// </summary>
namespace Sunplus.Accounting.Business.Others
{
    public class News : BaseManager
    {
        #region Const
        private const String VIEW_PERMISSION_DENY = "You do not have permission to view news.";
        private const String UPDATE_PERMISSION_DENY = "You do not have permission to view news.";
        private const String INSERT_PERMISSION_DENY = "You do not have permission to insert news.";
        private const String DELETE_PERMISSION_DENY = "You do not have permission to delete news.";
        private const String UPLOAD_PERMISSION_DENY = "You do not have permission to upload file on this page.";
        private const int PAGE_SIZE = 15;
        private const int LAST_NEWS_ROTATION_SIZE = 5;
        private const String KEY_CACHE_FEATURE  = "news.a{0}.feature";
        private const String KEY_CACHE_DETAIL   = "news.a{0}.i{1}.detail";
        private const String KEY_CACHE_BYPAGE   = "news.a{0}.p{1}.list";
        private const string KEY_CACHE_ALLPAGE  = "news.a{0}.p";
        #endregion

        #region Constructor

        #endregion

        public NewsProvider NewsProvider
        {
            get { throw new NotImplementedException(); }
            set { }
        }

        #region InsertNews
        /// <summary>        
        /// Check host permission
        /// Insert news
        /// </summary>
        /// <param name="content"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public String InsertNews(String title, String content, DateTime date, string func, int feature, string thumbnail, int appId, int ownerID,out int newsID)
        {
            newsID = 0;
            String resultMessage = "";
            // Check permission 
            bool hostPermission = false;
            try
            {
                hostPermission = Permissions.GetHostPermission(Enumerations.Functions.Others.WhatNew);
            }
            catch
            {
            }

            if (hostPermission)
            {
                #region Insert Data
                try
                {
                    var param = new Hashtable();
                    param.Add("title", title);
                    param.Add("newscontent", content);
                    param.Add("date", date);
                    param.Add("func", func);
                    param.Add("appid", appId);
                    param.Add("feature", feature);
                    param.Add("thumbnail", thumbnail);
                    param.Add("ownerid", ownerID);
                    param.Add("newsid", 0);
                    param.Add("errcode", 0);
                    param.Add("errmsg", "");


                    DbResult rs = OtherProvider.News.NewsIns(param);
                    resultMessage = CastAide.AsString(rs.Outputs["errmsg"]);
                    newsID = CastAide.AsInt32(rs.Outputs["newsid"]);

                    //delete cache news detail
                    CacheExtension.RegexDelete(string.Format(KEY_CACHE_ALLPAGE, appId));
                    if (feature==1)
                    {
                        var newsFeature = CacheExtension.Get<NewsItem>(string.Format(KEY_CACHE_FEATURE, appId));
                        CacheExtension.Delete(string.Format(KEY_CACHE_FEATURE, appId));
                        if (newsFeature!=null)
                        {
                            CacheExtension.Delete(string.Format(KEY_CACHE_DETAIL, appId, newsFeature.NewsID));
                        }
                    }
                }
                catch (SqlException exception)
                {
                    resultMessage = exception.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                catch (Exception exception)
                {
                    resultMessage = exception.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                #endregion
            }
            else
            {
                resultMessage = INSERT_PERMISSION_DENY;
            }
            return resultMessage;
        }
        #endregion

        #region UpdateNews
        /// <summary>
        /// Check host permission
        /// Update news
        /// </summary>
        /// <param name="newsID"></param>
        /// <param name="content"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public String UpdateNews(int newsID, String title, String content, DateTime date, string func, int feature, string thumbnail, int appId, int ownerID)
        {
            String resultMessage = "";
            // Check permission 
            bool hostPermission = false;
            try
            {
                hostPermission = Permissions.GetHostPermission(Enumerations.Functions.Others.WhatNew);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            if (hostPermission)
            {
                #region Update data
                try
                {
                    var param = new Hashtable();
                    param.Add("newsid", newsID);
                    param.Add("title", title);
                    param.Add("newscontent", content);
                    param.Add("date", date);
                    param.Add("func", func);
                    param.Add("appid", appId);
                    param.Add("feature", feature);
                    param.Add("thumbnail", thumbnail);
                    param.Add("ownerid", ownerID);
                    param.Add("errcode", 0);
                    param.Add("errmsg", "");

                    DbResult rs = OtherProvider.News.NewsUpd(param);
                    resultMessage = CastAide.AsString(rs.Outputs["errmsg"]);

                    //delete cache news detail
                    CacheExtension.Delete(string.Format(KEY_CACHE_DETAIL, appId, newsID));
                    CacheExtension.RegexDelete(string.Format(KEY_CACHE_ALLPAGE, appId));
                    if (feature == 1)
                    {
                        var newsFeature = CacheExtension.Get<NewsItem>(string.Format(KEY_CACHE_FEATURE, appId));
                        CacheExtension.Delete(string.Format(KEY_CACHE_FEATURE, appId));
                        if (newsFeature != null)
                        {
                            CacheExtension.Delete(string.Format(KEY_CACHE_DETAIL, appId, newsFeature.NewsID));
                        }
                    }
                }
                catch (SqlException exception)
                {
                    resultMessage = exception.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                catch (Exception exception)
                {
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                    resultMessage = exception.Message;
                }
                #endregion
            }
            else
            {
                resultMessage = UPDATE_PERMISSION_DENY;
            }
            return resultMessage;
        }
        #endregion

        #region DeleteNews
        public String DeleteNews(int newsID, int appId)
        {
            String resultMessage = "";

            // Check permission 
            bool hostPermission = false;
            try
            {
                hostPermission = Permissions.GetHostPermission(Enumerations.Functions.Others.WhatNew);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            if (hostPermission)
            {
                #region Delete Data
                try
                {
                    var hash = new Hashtable();
                    hash.Add("newsid", newsID);
                    hash.Add("appid", appId);
                    hash.Add("errcode", 0);
                    hash.Add("errmsg", "");

                    DbResult rsResult = OtherProvider.News.NewsDel(hash);

                    // Check error message 
                    resultMessage = rsResult.Outputs["errmsg"].ToString();
                    resultMessage = string.IsNullOrEmpty(resultMessage) ? "Success" : resultMessage;

                    //delete cache news detail
                    CacheExtension.Delete(string.Format(KEY_CACHE_DETAIL, appId, newsID));
                    CacheExtension.Delete(string.Format(KEY_CACHE_FEATURE, appId));
                    CacheExtension.RegexDelete(string.Format(KEY_CACHE_ALLPAGE, appId));
                }
                catch (SqlException exception)
                {
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                    resultMessage = exception.Message;
                }
                catch (Exception exception)
                {
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                    resultMessage = exception.Message;
                }
                #endregion
            }
            else
            {
                resultMessage = DELETE_PERMISSION_DENY;
            }
            return resultMessage;
        }
        #endregion

        #region ShowAllNews
        /// <summary>
        /// Get all news and generate their content
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public String ShowAllNews(int pageIndex)
        {
            String htmlData = "";

            // Check permission 
            bool viewPermission = false, hostPermission = false;
            try
            {
                viewPermission = Permissions.HasPermission(Enumerations.Functions.Others.WhatNew);
                hostPermission = Permissions.GetHostPermission(Enumerations.Functions.Others.WhatNew);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            if (viewPermission)
            {
                #region  Render the html
                try
                {
                    // Generate new list
                    pageIndex = pageIndex <= 0 ? 1 : pageIndex;
                    var param = new Hashtable();
                    param.Add("pageindex", pageIndex);
                    param.Add("pagesize", PAGE_SIZE);
                    param.Add("totalrows", 0);

                    DbResult rsNews = OtherProvider.News.NewsSelAll(param);
                    DataRow[] Rows = rsNews.ResultSet.Tables[0].Select();
                    var newsTemplate = new News_Template();

                    newsTemplate.Rows.Clear();
                    foreach (DataRow dr in Rows)
                    {
                        DateTime archiveDate = Convert.IsDBNull(dr["Date"]) ? DateTime.Now : (DateTime) dr["Date"];
                        var block = new News_Rows_Block();
                        block.ArchiveDate = archiveDate.ToShortDateString();
                        block.NewsTitle = dr["Title"].ToString();
                        block.NewsID = dr["NewsID"].ToString();

                        newsTemplate.Rows.Append(block);
                    }

                    // Generate Pager Control
                    int totalRows = Convert.IsDBNull(rsNews.Outputs["totalrows"])
                                        ? 0
                                        : (int) rsNews.Outputs["totalrows"];
                    int totalPages = totalRows%PAGE_SIZE == 0 ? totalRows/PAGE_SIZE : totalRows/PAGE_SIZE + 1;
                    newsTemplate.PagerControl = AccCommon.Acc_GenPager(totalPages, pageIndex, "");
                    newsTemplate.AddNewsButton.Clear();
                    if (hostPermission)
                    {
                        var addMewsBlock = new News_AddNewsButton_Block();
                        newsTemplate.AddNewsButton.Append(addMewsBlock);
                    }

                    // Return data
                    htmlData = newsTemplate.ToString();
                    // var data = {row: [], total: 100};
                }
                catch (SqlException exception)
                {
                    htmlData = exception.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                catch (Exception exception)
                {
                    htmlData = exception.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                #endregion
            }
            else
            {
                htmlData = VIEW_PERMISSION_DENY;
            }
            return htmlData;
        }
        #endregion

        #region ShowNewsDetail
        /// <summary>
        /// Generate html for news detail
        /// </summary>
        /// <param name="newsID"></param>
        /// <returns></returns>
        public String ShowNewsDetail(int newsID)
        {
            String htmlData = "";

            // Check permission 
            bool viewPermission = false, hostPermission = false;
            try
            {
                viewPermission = Permissions.HasPermission(Enumerations.Functions.Others.WhatNew);
                hostPermission = Permissions.GetHostPermission(Enumerations.Functions.Others.WhatNew);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            if (viewPermission)
            {
                #region  Render the html
                try
                {
                    var param = new Hashtable();
                    param.Add("newsid", newsID);
                    DbResult rsResult = OtherProvider.News.NewsSel(param);

                    if (rsResult.ResultSet.Tables.Count > 0)
                    {
                        DataTable dtData = rsResult.ResultSet.Tables[0];
                        if (dtData.Rows.Count > 0)
                        {
                            DataRow firstRow = dtData.Rows[0];

                            // Render html by template
                            var detailTemplate = new NewsDetail_Template();
                            detailTemplate.DeleteButton.Clear();
                            detailTemplate.EditButton.Clear();
                            if (hostPermission)
                            {
                                var deleteBlock = new NewsDetail_DeleteButton_Block();
                                var editBlock = new NewsDetail_EditButton_Block();
                                deleteBlock.NewsId = newsID.ToString();
                                editBlock.NewsId = newsID.ToString();
                                detailTemplate.DeleteButton.Append(deleteBlock);
                                detailTemplate.EditButton.Append(editBlock);
                            }
                            
                            detailTemplate.NewsTitle = Convert.IsDBNull(firstRow["Title"]) ? "" : firstRow["Title"].ToString();
                            detailTemplate.NewsDate = Convert.IsDBNull(firstRow["Date"]) ? "" : ((DateTime)firstRow["Date"]).ToShortDateString();
                            detailTemplate.NewsContent = Convert.IsDBNull(firstRow["NewsContent"]) ? "" : firstRow["NewsContent"].ToString();

                            detailTemplate.NewsTitle = Convert.IsDBNull(firstRow["Title"])
                                                           ? ""
                                                           : firstRow["Title"].ToString();
                            detailTemplate.NewsDate = Convert.IsDBNull(firstRow["Date"])
                                                          ? ""
                                                          : ((DateTime) firstRow["Date"]).ToShortDateString();
                            detailTemplate.NewsContent = Convert.IsDBNull(firstRow["NewsContent"])
                                                             ? ""
                                                             : firstRow["NewsContent"].ToString();

                            htmlData = detailTemplate.ToString();
                        }
                    }
                }
                catch (SqlException exception)
                {
                    htmlData = exception.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                catch (Exception exception)
                {
                    htmlData = exception.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                #endregion
            }
            else
            {
                htmlData = VIEW_PERMISSION_DENY;
            }
            return htmlData;
        }
        #endregion

        #region ShowNewsEditing
        public String ShowNewsEditing(int newsID)
        {
            String htmlData = "";

            // Check permission 
            bool hostPermission = false;
            try
            {
                hostPermission = Permissions.GetHostPermission(Enumerations.Functions.Others.WhatNew);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            if (hostPermission)
            {
                #region  Render the html
                try
                {
                    String newsTitle = "", newsDate = DateTime.Now.ToShortDateString(), newsContent = "";

                    // If editting, get current data
                    if (newsID > 0)
                    {
                        #region Get News Detail Data

                        var param = new Hashtable();
                        param.Add("newsid", newsID);
                        DbResult rsResult = OtherProvider.News.NewsSel(param);

                        if (rsResult.ResultSet.Tables.Count > 0)
                        {
                            DataTable dtData = rsResult.ResultSet.Tables[0];
                            if (dtData.Rows.Count > 0)
                            {
                                DataRow firstRow = dtData.Rows[0];

                                newsTitle = Convert.IsDBNull(firstRow["Title"]) ? "" : firstRow["Title"].ToString();
                                newsDate = Convert.IsDBNull(firstRow["Date"])
                                               ? ""
                                               : ((DateTime) firstRow["Date"]).ToShortDateString();
                                newsContent = Convert.IsDBNull(firstRow["NewsContent"])
                                                  ? ""
                                                  : firstRow["NewsContent"].ToString();
                            }
                        }
                        #endregion
                    }

                    // Render html
                    var editTemplate = new NewsEdit_Template();
                    editTemplate.NewsID = newsID.ToString();
                    editTemplate.NewsTitle = newsTitle;
                    editTemplate.NewsContent = newsContent;
                    editTemplate.NewsDate = newsDate;

                    htmlData = editTemplate.ToString();
                }
                catch (SqlException exception)
                {
                    htmlData = exception.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                catch (Exception exception)
                {
                    htmlData = exception.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                #endregion
            }
            else
            {
                htmlData = newsID > 0 ? UPDATE_PERMISSION_DENY : INSERT_PERMISSION_DENY;
            }
            return htmlData;
        }
        #endregion

        #region ShowLastNewsRotation
        public String ShowLastNewsRotaion()
        {
            String htmlData = "";

            // Check permission 
            bool viewPermission = false;
            try
            {
                viewPermission = Permissions.HasPermission(Enumerations.Functions.Others.WhatNew);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            if (viewPermission)
            {
                #region  Render the html
                try
                {
                    // Generate new list                    
                    var param = new Hashtable();
                    param.Add("pageindex", 1);
                    param.Add("pagesize", LAST_NEWS_ROTATION_SIZE);
                    param.Add("totalrows", 0);

                    DbResult rsNews = OtherProvider.News.NewsSelAll(param);
                    DataRow[] Rows = rsNews.ResultSet.Tables[0].Select();
                    var newsTemplate = new LastNews_Template();

                    newsTemplate.Rows.Clear();
                    foreach (DataRow dr in Rows)
                    {
                        DateTime archiveDate = Convert.IsDBNull(dr["Date"]) ? DateTime.Now : (DateTime) dr["Date"];
                        var block = new LastNews_Rows_Block();
                        block.NewsID = Convert.IsDBNull(dr["NewsID"]) ? "0" : dr["NewsID"].ToString();
                        block.NewsTitle = Convert.IsDBNull(dr["Title"]) ? "" : dr["Title"].ToString();
                        newsTemplate.Rows.Append(block);
                    }

                    // Return data
                    htmlData = newsTemplate.ToString();
                }
                catch (SqlException exception)
                {
                    htmlData = exception.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                catch (Exception exception)
                {
                    htmlData = exception.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                #endregion
            }
            else
            {
                htmlData = "";
            }
            return htmlData;
        }
        #endregion

        #region GetNewsDetail

        public String GetNewsDetail(int newsID, out String newsTitle, out String newsDate, out String newsContent)
        {
            String resultMessage = "";
            newsDate = "";
            newsContent = "";
            newsTitle = "";

            // Check permission 
            bool viewPermission = false;
            try
            {
                viewPermission = Permissions.HasPermission(Enumerations.Functions.Others.WhatNew);
            }
            catch (Exception ex)
            {
                resultMessage = ex.Message;
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            if (viewPermission)
            {
                #region Get Data
                try
                {
                    var param = new Hashtable();
                    param.Add("newsid", newsID);
                    DbResult rsResult = OtherProvider.News.NewsSel(param);

                    if (rsResult.ResultSet.Tables.Count > 0)
                    {
                        DataTable dtData = rsResult.ResultSet.Tables[0];
                        if (dtData.Rows.Count > 0)
                        {
                            DataRow firstRow = dtData.Rows[0];

                            newsTitle = Convert.IsDBNull(firstRow["Title"]) ? "" : firstRow["Title"].ToString();
                            newsDate = Convert.IsDBNull(firstRow["Date"])
                                           ? ""
                                           : ((DateTime) firstRow["Date"]).ToShortDateString();
                            newsContent = Convert.IsDBNull(firstRow["NewsContent"])
                                              ? ""
                                              : firstRow["NewsContent"].ToString();
                        }
                    }
                }
                catch (SqlException exception)
                {
                    resultMessage = exception.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                catch (Exception exception)
                {
                    resultMessage = exception.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                #endregion
            }
            else
            {
                resultMessage = VIEW_PERMISSION_DENY;
            }
            return resultMessage;
        }

        #endregion
    
        #region GetNewsPage
        /// <summary>
        /// Get all news by page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public NewsData GetNewsPage(int page, int appId)
        {
            NewsData listNews = null;
            // Check permission 
            var viewPermission = false;
            var hostPermission = false;
            try
            {
                viewPermission = Permissions.HasPermission(Enumerations.Functions.Others.WhatNew);
                hostPermission = Permissions.GetHostPermission(Enumerations.Functions.Others.WhatNew);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            if (viewPermission)
            {

                try
                {
                    var pageIndex = page <= 0 ? 1 : page;

                    string keyList = string.Format(KEY_CACHE_BYPAGE, appId, pageIndex);
                    listNews = CacheExtension.Get<NewsData>(keyList);
                    if (listNews == null)
                    {

                        // Generate new list
                        var param = new Hashtable();
                        param.Add("pageindex", pageIndex);
                        param.Add("appid", appId);
                        param.Add("pagesize", PAGE_SIZE);
                        param.Add("totalrows", 0);

                        DbResult rsNews = OtherProvider.News.NewsSelAll(param);
                        DataRow[] rows = rsNews.ResultSet.Tables[0].Select();

                        listNews = new NewsData();
                        if (rows.Length > 0)
                        {
                            listNews.Rows = new List<NewsItem>();
                            foreach (DataRow dr in rows)
                            {
                                DateTime archiveDate = Convert.IsDBNull(dr["Date"])
                                                           ? DateTime.Now
                                                           : (DateTime) dr["Date"];
                                var newsItem = new NewsItem
                                                   {
                                                       NewsID = (int) dr["NewsID"],
                                                       Date = archiveDate.ToShortDateString(),
                                                       Title = dr["Title"].ToString(),
                                                       AppID = appId,
                                                       Func = dr["Func"].ToString(),
                                                       Thumbnail = dr["Thumbnail"].ToString()
                                                   };
                                listNews.Rows.Add(newsItem);
                            }
                            int totalRows = Convert.IsDBNull(rsNews.Outputs["totalrows"])
                                                ? 0
                                                : (int) rsNews.Outputs["totalrows"];

                            listNews.Total = totalRows;
                            listNews.PagerControl = "";

                            //set cache by page
                            CacheExtension.Set(keyList, listNews);
                        }
                    }
                    listNews.HostRole = hostPermission;
                }
                catch (SqlException exception)
                {
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                catch (Exception exception)
                {
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
            }
            return listNews;
        }
        #endregion

        #region Get News Detail
        /// <summary>
        /// Get News Detail
        /// </summary>
        /// <param name="newsID"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public NewsItem GetNewsDetail(int newsID, int appId)
        {
            NewsItem newsItem = null;
            // Check permission 
            var viewPermission = false;
            var hostPermission = false;
            try
            {
                viewPermission = Permissions.HasPermission(Enumerations.Functions.Others.WhatNew);
                hostPermission = Permissions.GetHostPermission(Enumerations.Functions.Others.WhatNew);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            if (viewPermission)
            {

                try
                {
                    newsItem = CacheExtension.Get<NewsItem>(string.Format(KEY_CACHE_DETAIL, appId, newsID));
                    if (newsItem == null)
                    {
                        var param = new Hashtable();
                        param.Add("newsid", newsID);
                        param.Add("appid", appId);
                        DbResult rsResult = OtherProvider.News.NewsSel(param);

                        if (rsResult.ResultSet.Tables.Count > 0)
                        {
                            DataTable dtData = rsResult.ResultSet.Tables[0];
                            if (dtData.Rows.Count > 0)
                            {
                                newsItem = new NewsItem();

                                DataRow firstRow = dtData.Rows[0];

                                newsItem.AppID = appId;

                                newsItem.NewsID = (int) firstRow["NewsID"];

                                newsItem.Title = Convert.IsDBNull(firstRow["Title"])
                                                     ? ""
                                                     : firstRow["Title"].ToString();
                                newsItem.Date = Convert.IsDBNull(firstRow["Date"])
                                                    ? ""
                                                    : ((DateTime) firstRow["Date"]).ToShortDateString();
                                newsItem.NewsContent = Convert.IsDBNull(firstRow["NewsContent"])
                                                           ? ""
                                                           : firstRow["NewsContent"].ToString();

                                newsItem.Func = Convert.IsDBNull(firstRow["Func"])
                                                    ? ""
                                                    : firstRow["Func"].ToString();

                                newsItem.Thumbnail = Convert.IsDBNull(firstRow["Thumbnail"])
                                                         ? ""
                                                         : firstRow["Thumbnail"].ToString();

                                newsItem.Feature = Convert.IsDBNull(firstRow["Feature"])
                                                       ? 0
                                                       : CastAide.AsInt32(firstRow["Feature"].ToString(), 0);

                                //set cache news detail
                                CacheExtension.Set(string.Format(KEY_CACHE_DETAIL, appId, newsID), newsItem);
                            }
                        }
                    }
                    if (newsItem != null)
                    {
                        newsItem.AppID = appId;
                        newsItem.HostRole = hostPermission;
                    }
                }
                catch (SqlException exception)
                {
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                catch (Exception exception)
                {
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
            }
            return newsItem;
        }
        #endregion

        #region Get News Feature
        /// <summary>
        /// Get News Feature
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        public NewsItem GetNewsFeature(int appId)
        {
            NewsItem newsItem = null;
            var viewPermission = false;
            viewPermission = Permissions.HasPermission(Enumerations.Functions.Others.WhatNew);
            if (viewPermission)
            {
                try
                {
                    string keyFeature = string.Format(KEY_CACHE_FEATURE, appId);
                    newsItem = CacheExtension.Get<NewsItem>(keyFeature);
                    if (newsItem != null)
                    {
                        return newsItem;
                    }
                    var param = new Hashtable();
                    param.Add("appid", appId);
                    DbResult rsResult = OtherProvider.News.NewsSelFeature(param);

                    if (rsResult.ResultSet.Tables.Count > 0)
                    {
                        DataTable dtData = rsResult.ResultSet.Tables[0];
                        if (dtData.Rows.Count > 0)
                        {
                            newsItem = new NewsItem();

                            DataRow firstRow = dtData.Rows[0];

                            newsItem.NewsID = (int) firstRow["NewsID"];

                            newsItem.Title = Convert.IsDBNull(firstRow["Title"])
                                                 ? ""
                                                 : firstRow["Title"].ToString();
                            newsItem.Date = Convert.IsDBNull(firstRow["Date"])
                                                ? ""
                                                : ((DateTime) firstRow["Date"]).ToShortDateString();
                            newsItem.NewsContent = Convert.IsDBNull(firstRow["NewsContent"])
                                                       ? ""
                                                       : firstRow["NewsContent"].ToString();

                            newsItem.Func = Convert.IsDBNull(firstRow["Func"])
                                                ? ""
                                                : firstRow["Func"].ToString();

                            newsItem.Thumbnail = Convert.IsDBNull(firstRow["Thumbnail"])
                                                         ? ""
                                                         : firstRow["Thumbnail"].ToString();

                            newsItem.Feature = Convert.IsDBNull(firstRow["Feature"])
                                                   ? 0
                                                   : CastAide.AsInt32(firstRow["Feature"].ToString(), 0);

                            newsItem.AppID = appId;

                            //set cache news feature
                            CacheExtension.Set(keyFeature, newsItem);
                        }
                    }
                }
                catch (SqlException exception)
                {
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                catch (Exception exception)
                {
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
            }
            return newsItem;
        }
        #endregion

        #region Hot Feature News
        /// <summary>
        /// Hot Feature News
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="newsId"></param>
        /// <param name="feature"></param>
        /// <returns></returns>
        public string HotFeature(int appId, int newsId, int feature)
        {
            string resultMessage = "";
            // Check permission 
            bool hostPermission = false;
            try
            {
                hostPermission = Permissions.GetHostPermission(Enumerations.Functions.Others.WhatNew);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            #region Update data
            if (hostPermission)
            {
                try
                {
                    var param = new Hashtable();
                    param.Add("newsid", newsId);
                    param.Add("appid", appId);
                    param.Add("feature", feature);
                    param.Add("errcode", 0);
                    param.Add("errmsg", "");

                    DbResult rs = OtherProvider.News.NewsUpdFeature(param);
                    resultMessage = CastAide.AsString(rs.Outputs["errmsg"]);
                    
                    // delete cache news feature
                    var newsFeature = CacheExtension.Get<NewsItem>(string.Format(KEY_CACHE_FEATURE, appId));
                    CacheExtension.Delete(string.Format(KEY_CACHE_FEATURE, appId));
                    CacheExtension.Delete(string.Format(KEY_CACHE_DETAIL, appId, newsId)); 
                    if (newsFeature != null)
                    {
                        CacheExtension.Delete(string.Format(KEY_CACHE_DETAIL, appId, newsFeature.NewsID));    
                    }
                    
                }
                catch (SqlException exception)
                {
                    resultMessage = exception.Message;
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                }
                catch (Exception exception)
                {
                    
                        Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(exception);
                    resultMessage = exception.Message;
                }
            }
            else
            {
                resultMessage = UPDATE_PERMISSION_DENY;
            }
            #endregion
            return resultMessage;
        }
        #endregion
    }

}
