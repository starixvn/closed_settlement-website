using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class UserControl_AddMenu : System.Web.UI.Page
{
    public string ActionTypeMenu = "";
    public string strChecked = "";
    public string strUrl = "";
    //public string _default = Sunplus.Accounting.Core.Common.Enumerations.AccountingSystem.Accounting.ToString();
    //public string _mini = Sunplus.Accounting.Core.Common.Enumerations.AccountingSystem.Mini.ToString();
    //public string _thai = Sunplus.Accounting.Core.Common.Enumerations.AccountingSystem.AccThai.ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["edit"] == "0")
        {
            ActionTypeMenu = "Add New Menu for " + Request["funcName"];
        }
        else
        {
            ActionTypeMenu = "Edit Menu for " + Request["funcName"];
            strUrl = HttpUtility.UrlDecode(Request["Url"]);
        }

        if (Request["IsHide"] == "true")
        {
            strChecked = "checked='checked'";
        }

        
    }
}
