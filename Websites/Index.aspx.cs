using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Index : System.Web.UI.Page
{
    public string ToParams = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        ToParams = Request.QueryString.ToString();
        ToParams = string.IsNullOrEmpty(ToParams) ? "" : "?" + ToParams;
    }
}
