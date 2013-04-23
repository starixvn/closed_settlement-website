using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Settle_SelectMatchMoveBet : System.Web.UI.Page
{
    public string sysPath = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        sysPath = Sunplus.Accounting.Core.Web.WebUtils.GetTemplateFolder().Replace("\\", "/");
    }
}
