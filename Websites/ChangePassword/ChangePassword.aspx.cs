using System;
using System.Web.Security;
using System.Text;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Entities.Casino;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Core.LoggingLib;
using IBCUtils;


/// <summary>
/// CITS
/// </summary>
public partial class ChangePassword_ChangePassword : RequiredAuthorizationPage
{

    string str = "";
    string errMsg = "";
    public string msg = "";
    public string strscript = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Page.Title = "Change Password";
            StringBuilder html = new StringBuilder();
            if (Session["AccId"] == null || (Session["CustId"] == null && Request["custId"] == null))
            {

                html.Append(AlertMessage.GetMsg("Session is expired, please re log-in ", AlertMessage.MsgType.Error));
                content.Text = html.ToString();
                return;
            }

            int custid = DNAide.Core.CastAide.AsInt32(Request["custId"]);
            int userid = Convert.ToInt32(Session["AccId"]);
            string strUsername=Convert.ToString(Session["CustUname"]);
            bool isRequest = false;
            
            string strCustID = "";
            if (Request["custId"] !=null)
            {
                custid = Convert.ToInt32(Request["custId"]);
                strCustID +=Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("Hidden.htm", new string[] { "{name}", "{strValue}" }, new string[] { "custId", custid.ToString() });                                
            }
            if(Request["uname"] !=null)
            {
                strUsername=Convert.ToString(Request["uname"]);
                strCustID += Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("Hidden.htm", new string[] { "{name}", "{strValue}" }, new string[] { "uname", strUsername });
                isRequest = true;
            }                       
            string language = "E";
            string txtPass = Request["txtPass"];
            string txtConfirmPass = Request["txtConfirmPass"];
            if (txtPass == null)
            {
                html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("ChangePassword/ChangePassword.htm", new string[] { "{username}", "{hidden}" }, new string[] { strUsername, strCustID }));
                content.Text = html.ToString();
                return;
            }
            string encodepwd = IBCUtils.Encryption.CfsEnCode(txtPass);
            if (txtPass.Length == 0)
                str = AlertMessage.GetMsg("Sorry, password cannot be empty.", AlertMessage.MsgType.Error);
            else if (txtPass.Length < 4)
                str = AlertMessage.GetMsg("Password must have at least 4 characters.",AlertMessage.MsgType.Error);
            else if(!txtPass.Equals(txtConfirmPass))
                str = AlertMessage.GetMsg("Password was not correctly confirmed.",AlertMessage.MsgType.Error);
            else if (changePWD(custid, encodepwd, userid, language))
            {
                if (!isRequest)
                {
                    str = AlertMessage.GetMsg("Password has been changed successfully.", AlertMessage.MsgType.Success);
                }
                else
                {
                    if (DNAide.Core.CastAide.AsInt32(Session["AccId"]) == DNAide.Core.CastAide.AsInt32(Request["custId"]))
                    {
                        Session["AccPwd"] = encodepwd;

                        string rndStr = Encryption.RandomString(6);
                        string accKey = FormsAuthentication.HashPasswordForStoringInConfigFile(Session["AccPwd"] + rndStr, "MD5");

                        string script = "";
                        script += "<script type='text/javascript'>";
                        script += "parent.window.top.leftPanel.document.getElementById('accKey').value='" + accKey + "';";
                        script += "parent.window.top.leftPanel.document.getElementById('chalengeStr').value='" + rndStr + "';";
                        script += "window.close();";
                        script += "</script>";
                        strscript=script;
                    }
                    else
                    {
                        str = AlertMessage.GetMsg("Password has been changed successfully.", AlertMessage.MsgType.Success);
                        strscript="<script type=\"text/javascript\">afterUpdate(false,null);</script>";

                    }
                }
            }
            else if(errMsg !="")
            {
                str = AlertMessage.GetMsg(errMsg, AlertMessage.MsgType.Error);
                str += "<script>afterUpdate(false,2000);</script>";
            }
            string strUname = strUsername;
            html.Append(Sunplus.Accounting.Core.Web.WebUtils.LoadHtml("ChangePassword/ChangePassword.htm", new string[] { "{username}", "{hidden}" }, new string[] { strUname, strCustID }));
            content.Text = html.ToString();
            msg = str;
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
        }
    }

    bool changePWD(int custid, string userpwd, int userid, string language)
    {        
        try
        {
            CSCustomerDetail custcs = Sunplus.Accounting.DataAccess.APIClient.IBCLibProvider.Instance.GetBasicCustInfoIBC(custid);
            if (WebSettings.disableCS && custcs.CasinoSync)
            {
                errMsg = string.Format(Resources.ErrorMsg.error_LockUpdateStatus, "CHANGE PASSWORD");
                return false;
            }
            else
            {
                // Notes: should be put into 1 transaction later!!!
                //1
                IbcError error = Sunplus.Accounting.DataAccess.IBCDLL.Customer.UpdateCustomer.Acc_CustomerPwdUpd(custid, userpwd, userid, language);
                //2

                if (Request["type"] != "User" && custcs.CasinoSync)
                {
                   
                    try // Casino cheat
                    {
                        bool status = CasinoManager.UpdateCustPwd(custid, userpwd, Request.ServerVariables["REMOTE_ADDR"]);
                    }
                    catch { }
                }

                if (error.ErrorCode != 0)
                {
                    errMsg = error.ErrorMsg;
                    return false;
                }
                else
                {
                    
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            LogExtensions.ExceptionHandler(ex);
            return false;
        }
    }
}
