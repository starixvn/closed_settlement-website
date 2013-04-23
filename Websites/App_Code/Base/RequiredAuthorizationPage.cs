using System;
using System.Web;
using DNAide.Core;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Presentation;

    /// <summary>
    /// Summary description for RequiredAuthorizationPage
    /// </summary>
    public class RequiredAuthorizationPage : PageBase
    {
        private bool _isValidRequest = true;

        protected virtual int CustId
        {
            get { return CastAide.AsInt32(Request[Constants.QueryStringKey.CustomerId]); }
        }

        protected virtual int UplineId
        {
            get { return CastAide.AsInt32(Request[Constants.QueryStringKey.UplineId]); }
        }

        protected virtual int CurrencyId
        {
            get { return CastAide.AsInt32(Request[Constants.QueryStringKey.CurrencyId]); }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ProcessOnInit();
            if (!_isValidRequest)
            {
                RedirectError();
                Response.End();
            }
        }

        private void ProcessOnInit()
        {
            
        }

        protected virtual void RedirectError()
        {
            HttpContext.Current.Server.Transfer(Constants.Page.Error);
        }
    }
