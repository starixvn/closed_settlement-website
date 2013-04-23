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
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using IBCUtils.Web;
using IBCUtils;
using Sunplus.Accounting.DataAccess.IBCDLL;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using IBCUtils.DAL;


public partial class VerifyPassword : System.Web.UI.Page
{
    public string accname = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            accname = Session["Username"] as string ?? string.Empty;            
            if (string.IsNullOrEmpty(accname)) throw new AccException(IbcError.ACCESS_DENIED_MSG);
            
        }
        catch (AccException ex)
        {
            Logs.Write(ex.Message);
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex); 
        }
    }     
}
