using System;
using Sunplus.Accounting.Core.Common;

public partial class _Footer : System.Web.UI.Page
{
    public string WrapVertionStr = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (AppSession.SYSTEM.ToLower() != Enumerations.AccountingSystem.Accounting.ToString().ToLower())
        {
            WrapVertionStr = "";
        }
        else
        {
            WrapVertionStr = "<a href=\"javascript:SwapVersion();\">Classic Version</a>";
        }
    }
}
