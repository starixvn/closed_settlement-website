using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IBCUtils;
using IBCUtils.Web;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Business.Security;

public partial class SignIn : System.Web.UI.Page
{
    protected string submitUrl;

    public string LoginTheme = "Login";

    protected void Page_PreInit(object sender, EventArgs e)
    {
        LoginTheme = AuthManager.IsRequestFromExternalSite(Request.Url.Host) ? "LoginEx" : "Login";
        //Page.Theme = LoginTheme;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Username"] != null) FormsAuthentication.RedirectFromLoginPage(DNAide.Core.CastAide.AsString(Session["Username"]), false);

        AccCommon.Redirect2SSL();

        int endPos = Request.Url.AbsoluteUri.IndexOf("/SignIn.aspx", StringComparison.CurrentCultureIgnoreCase);

        submitUrl = Request.Url.AbsoluteUri.Substring(0, endPos).ToLower();
        if (!WebSettings.IsLocalTest)
        {
            submitUrl = submitUrl.Replace("http://", "https://");
        }


    }
}
