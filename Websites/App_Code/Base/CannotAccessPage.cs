using System;
using System.Web;


    /// <summary>
    /// Summary description for CannotAccessPage
    /// </summary>
    public class CannotAccessPage : PageBase
    {
        public override string RedirectUrl
        {
            get { return "~/ErrorPage.aspx"; }
        }

        protected override void OnInit(EventArgs e)
        {
            HttpContext.Current.Server.Transfer(RedirectUrl);
        }
    }
