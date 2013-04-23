using System;
using System.Web.Security;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Core.LoggingLib;
using IBCUtils;



public partial class UserControl_ResetPassword : System.Web.UI.Page
{

    private System.Globalization.CultureInfo cult = System.Globalization.CultureInfo.CurrentCulture;
    protected string userName = string.Empty;
    private int custId = -1;
    private int accId = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        string _s = string.Empty;
        try
        {
            accId = Convert.ToInt32(Session["AccId"] as object ?? -1);
            if (accId == -1) throw new AccException(IbcError.ACCESS_DENIED_MSG);  
            string _accept = Request["accept"] as string ?? string.Empty;
            if (!string.IsNullOrEmpty(_accept))
            {                                                
                custId = Convert.ToInt32(Request["custId"] as object ?? -1);
                string newPass = Request["password"] as string ?? string.Empty;
                string confirm = Request["confirm"] as string ?? string.Empty;
                newPass = newPass.Trim();
                confirm = confirm.Trim();
                if (custId == -1 || string.IsNullOrEmpty(newPass) || (string.Compare(newPass, confirm) != 0))
                    throw new AccException(IbcError.INVALID_INPUT_VALUE_MSG);                
                _s = ResetPassword(newPass);
            }
            else
            {
                userName = Request.QueryString["uname"] as string ?? string.Empty;
                custId = Convert.ToInt32(Request.QueryString["uid"] as object ?? -1);
                if (string.IsNullOrEmpty(userName) || custId == -1)
                    throw new AccException(IbcError.INVALID_INPUT_VALUE_MSG);                
                _s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/ResetPassword.htm");
                TemplateHandler.Assign(ref _s, "{username}", userName);
                TemplateHandler.Assign(ref _s, "{custid}", custId.ToString(cult));
            }            
        }
        catch (AccException ex)
        {
            _s = AlertMessage.GetMsg(ex.Message.ToString(), AlertMessage.MsgType.Error);   
        }
        catch (Exception eex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(eex);
        }
        this.sBody.Text = _s;
    }

    private string ResetPassword(string newpass)
    {
        string s = string.Empty;        
        try
        {
            string password = IBCUtils.Encryption.CfsEnCode(newpass);
            Session["AccPwd"] = password;

            string rndStr = Encryption.RandomString(6);
            string accKey = FormsAuthentication.HashPasswordForStoringInConfigFile(Session["AccPwd"] + rndStr, "MD5");

            Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.AccountAdminPasswordUpdate(custId, accId, password);

            //string script = "";
            s += "<script type='text/javascript'>showAlertMessage('errMsg','Update Password Successfully!',0); ";
            s += "parent.leftPanel.document.getElementById('accKey').value='" + accKey + "';";
            s += "parent.leftPanel.document.getElementById('chalengeStr').value='" + rndStr + "';";
            s += "</script>";            
            //Response.Write(script);
            s += "<script type='text/javascript'>afterUpdate(false,2000);</script>";
        }
        catch(Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return s;                
    }
}
