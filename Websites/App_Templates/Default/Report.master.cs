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

public partial class App_Templates_Report : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Session["AccId"] !=null)
        //{
        //    FormsAuthentication.Initialize();
        //    String strRole = AssignRoles(Session["AccId"]);

        //    //The AddMinutes determines how long the user will be logged in after leaving
        //    //the site if he doesn't log off.
        //    FormsAuthenticationTicket fat = new FormsAuthenticationTicket(1,
        //       DNAide.Core.CastAide.AsString( Session["AccId"]), DateTime.Now,
        //        DateTime.Now.AddMinutes(30), false, strRole,
        //        FormsAuthentication.FormsCookiePath);
        //    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName,
        //        FormsAuthentication.Encrypt(fat)));            
        //}        
    }   
}
