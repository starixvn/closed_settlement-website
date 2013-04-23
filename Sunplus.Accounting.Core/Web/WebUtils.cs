using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;

namespace Sunplus.Accounting.Core.Web
{
    /// <summary>
    /// Summary description for WebUtils
    /// </summary>
    public static class WebUtils
    {
        #region Common

        public static string CurrentIp
        {
            get { return HttpContext.Current.Request.UserHostAddress; }
        }

        public static void SetNoCache()
        {
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now;
            HttpContext.Current.Response.Expires = -1441;
            HttpContext.Current.Response.CacheControl = "no-cache";
            HttpContext.Current.Response.AddHeader("Pragma", "no-cache");
            HttpContext.Current.Response.AddHeader("Pragma", "no-store");
            HttpContext.Current.Response.AddHeader("cache-control", "no-cache");
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
            HttpContext.Current.Response.Cache.SetNoServerCaching();
        }

        /// <summary>
        /// get template folder of system in a user session
        /// </summary>
        public static string GetTemplateFolder(string accountingSystem)
        {
            string templateFolder = string.Empty;
            //parallel work arround
            if (HttpContext.Current != null)
            {
                if(AppSession.CurrentSystemNAME.ToString().ToLower().Equals(Enumerations.AccountingSystem.Accounting.ToString().ToLower()))
                    templateFolder = HttpContext.Current.Session["TemplateFolderDefault"] as string;
                else
                {
                    if (AppSession.SYSTEM.ToLower().Equals(accountingSystem.ToLower()))
                        templateFolder = HttpContext.Current.Session["TemplateFolder"] as string;
                    else
                        templateFolder = HttpContext.Current.Session["TemplateFolderDefault"] as string;
                }

            }

            if (string.IsNullOrEmpty(templateFolder))
            {
                switch (accountingSystem.ToLower())
                {
                    case "accounting":
                        templateFolder = "Default";
                        templateFolder = Path.Combine(WebSettings.TemplateFolder, templateFolder);
                        if (HttpContext.Current != null)
                        {
                            HttpContext.Current.Session["TemplateFolderDefault"] = templateFolder;
                        }

                        break;

                    default:
                        templateFolder = "External";
                        templateFolder = Path.Combine(WebSettings.TemplateFolder, templateFolder);
                        if (HttpContext.Current != null)
                        {
                            HttpContext.Current.Session["TemplateFolder"] = templateFolder;
                        }

                        break;
                        
                }

                

                //parallel work arround
               
            }

            return templateFolder;
        }

        /// <summary>
        /// get current system template folder
        /// </summary>
        /// <returns></returns>
        public static string GetTemplateFolder()
        {
            return GetTemplateFolder(AppSession.SYSTEM);
        }

        #endregion

        #region Load template

        /// <summary>
        /// Load templates from default folder or current authenticated system
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="hasShrinkHtml">indicate whether shrink html or not</param>
        /// <param name="hasLoadBySystem">whether load from local folder or by system type</param>
        /// <returns>html string</returns>
        private static string LoadTemplate(string templateName, bool hasShrinkHtml, bool hasLoadBySystem)
        {
            return (hasLoadBySystem)
                       ? TemplateHandler.LoadTemplate(templateName, hasShrinkHtml,
                                                      GetTemplateFolder(AppSession.SYSTEM))
                       : TemplateHandler.LoadTemplate(templateName, hasShrinkHtml,
                                                      GetTemplateFolder(Enumerations.AccountingSystem.Accounting.ToString()));
        }

        /// <summary>
        /// load template and assign value
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="tags"></param>
        /// <param name="values"></param>
        /// <param name="hasLoadBySystem"></param>
        /// <returns></returns>
        private static string LoadHtml(string templateName, string[] tags, string[] values, bool hasLoadBySystem)
        {
            return (hasLoadBySystem)
                       ? TemplateHandler.LoadHtml(templateName, tags, values,
                                                  GetTemplateFolder(AppSession.SYSTEM))
                       : TemplateHandler.LoadHtml(templateName, tags, values,
                                                  GetTemplateFolder(Enumerations.AccountingSystem.Accounting.ToString()));
        }

        private static string LoadHtml(string templateName, Dictionary<string, string> tags, bool hasLoadBySystem)
        {
            return (hasLoadBySystem)
                       ? TemplateHandler.LoadHtml(templateName, tags,
                                                  GetTemplateFolder(AppSession.SYSTEM))
                       : TemplateHandler.LoadHtml(templateName, tags,
                                                  GetTemplateFolder(Enumerations.AccountingSystem.Accounting.ToString()));
        }

        /// <summary>
        /// Load template file and assign value
        /// </summary>
        /// <param name="templateFile"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static string LoadHtml(string templateFile, Dictionary<string, string> tags)
        {
            return LoadHtml(templateFile, tags, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="tags"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string LoadHtml(string templateName, string[] tags, string[] values)
        {
            return LoadHtml(templateName, tags, values, true);
        }

        /// <summary>
        /// load template from default folder
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns>html string</returns>
        public static string LoadTemplate(string templateName)
        {
            return LoadTemplate(templateName, true, false);
        }

        /// <summary>
        /// load template from default folder
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="hasShrinkHtml">indicate whether shrink html or not</param>
        /// <returns></returns>
        public static string LoadTemplate(string templateName, bool hasShrinkHtml)
        {
            return LoadTemplate(templateName, hasShrinkHtml, false);
        }

        /// <summary>
        /// Load template from current system
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns>html string</returns>
        public static string LoadTemplateBySystem(string templateName)
        {
            return LoadTemplate(templateName, true, true);
        }

        /// <summary>
        /// Load template from current system
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="hasShrinkHtml">indicate whether shrink html or not</param>
        /// <returns></returns>
        public static string LoadTemplateBySystem(string templateName, bool hasShrinkHtml)
        {
            return LoadTemplate(templateName, hasShrinkHtml, true);
        }

        /// <summary>
        /// Get query string value of a specific key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetQueryStringValue<T>(string key, T defaultValue) where T : class
        {
            Check.Argument.IsNotNull(key, "key");
            T returnValue = null;
            if (HttpContext.Current != null
                && HttpContext.Current.Request.QueryString[key] != null)
            {
                returnValue = HttpContext.Current.Request.QueryString[key] as T;
            }

            return (returnValue == null) ? defaultValue : returnValue;
        }

        #endregion
    }
}