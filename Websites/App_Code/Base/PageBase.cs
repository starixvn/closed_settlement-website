using System;
using System.Web;
using System.Web.UI;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Presentation;


    /// <summary>
    /// Summary description for PageBase
    /// </summary>
    public class PageBase : Page
    {
        private const string NotFoundPageUrl = Constants.Page.NotFound;

        public virtual string RedirectUrl
        {
            get { return NotFoundPageUrl; }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (AppSession.AccId == 0 || AppSession.RoleAccess == null || AppSession.PageAccess == null ||
                AppSession.UserMenu == null)
            {
                LogOut();
            }

            if (!HasPermission())
            {
                RedirectDeny();
            }
        }

        protected virtual void RedirectDeny()
        {
            HttpContext.Current.Server.Transfer(Constants.Page.AccessDenied);
        }

        private static void LogOut()
        {
            HttpContext.Current.Server.Transfer(Constants.Page.LogOut);
        }

        protected virtual bool HasPermission()
        {
            string url = Request.CurrentExecutionFilePath.ToLower();
            if (Request.ApplicationPath.Equals("/"))
                url = "/" + url;
            url = url.Replace(Request.ApplicationPath.ToLower() + "/", "").ToLower();
            ;
            // url = WebSettings.VirtualDir ? Request.RawUrl.Substring(Request.RawUrl.IndexOf("/", 1) + 1) : Request.RawUrl.Substring(1);
            //url = url.Substring(0, url.IndexOf(".aspx") + 5).ToLower();
            return AppSession.PageAccess.ContainsKey(url);
        }
    }
