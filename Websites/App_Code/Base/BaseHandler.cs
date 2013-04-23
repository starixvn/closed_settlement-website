using System;
using System.Web;
using System.Web.SessionState;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;


/// <summary>
    /// Summary description for EmptyHandler
    /// This handler do nothing, it use to prevent user access hidden handler
    /// Lewin: IRP #411
    /// Fron controller 
    /// </summary>
    public class BaseHandler : IHttpHandler, IRequiresSessionState
    {
        protected HttpContext _context;

        protected virtual int CurrencyId
        {
            get { return CastAide.AsInt32(_context.Request[Constants.QueryStringKey.CurrencyId]); }
        }

        protected virtual int CustId
        {
            get { return CastAide.AsInt32(_context.Request[Constants.QueryStringKey.CustomerId]); }
        }

        protected virtual int UplineId
        {
            get { return CastAide.AsInt32(_context.Request[Constants.QueryStringKey.UplineId]); }
        }

        protected virtual string Username
        {
            get { return CastAide.AsString(_context.Request[Constants.QueryStringKey.Username]); }
        }

        #region IHttpHandler Members

        public virtual void ProcessRequest(HttpContext context)
        {
            _context = context;
            if (!HasPermission() || !IsValidRequest())
            {
                RedirectError();
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }

        #endregion

        private bool IsValidRequest()
        {
            

            return true;
        }

        protected virtual bool HasPermission()
        {
            return true;
        }

        protected virtual void RedirectError()
        {
            _context.Response.Write(
                "<html><head></head><body><script type='text/javascript'> var wnd; if (top.content.centerFr!=null) wnd=top.content.centerFr; else wnd=window;  wnd.location.href='" +
                FullApplicationPath() + Constants.Page.ErrorPage +
                "';if (top.content.jQuery.unblockUI) top.content.jQuery.unblockUI();</script></body></html>");
        }

        protected void GotoErroPage()
        {
            _context.Response.Redirect(Constants.Page.Error);
        }

        protected void GotoErroPage(HttpContext context)
        {
            context.Response.Redirect(Constants.Page.Error);
        }


        private string FullApplicationPath()
        {
            return _context.Request.Url.GetLeftPart(UriPartial.Authority) + _context.Request.ApplicationPath;
        }
    }
