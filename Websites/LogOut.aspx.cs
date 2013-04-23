using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class LogOut : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Clear();
        FormsAuthentication.SignOut();
        
        Response.Write("<script type='text/javascript'>top.location.href='" + FormsAuthentication.LoginUrl + "';</script>");
    }
}