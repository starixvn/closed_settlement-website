using System;
using System.Data;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Core.LoggingLib;
using IBCUtils.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;


public partial class EditPersonalInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string _s = string.Empty;
        try
        {
            string accname = Session["Username"] as string ?? string.Empty;
            int accid = Convert.ToInt32(Session["AccId"] as object ?? -1);
            if (string.IsNullOrEmpty(accname) || accid == -1) throw new AccException("Invalid Session!");
            _s = LoadTemplate(accid, accname);
        }
        catch (AccException ex)
        {            
            Response.Write("<script type='text/javascript'>alert('" + ex.Message.ToString() + "');</script>");
            Response.Write("<script type='text/javascript'>document.location.replace('" + Request.UrlReferrer.ToString() + "');</script>");            
        }
        catch (Exception eex)
        {

            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(eex);
        }
        this.sBody.Text = _s;
    }

    private string LoadTemplate(int accid, string accname)
    {
        string s = string.Empty;
        try
        {            
            s = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplateBySystem("EditPersonalInfo.htm", true);
            TemplateHandler.Assign(ref s, "{resetpassParam}", "uname=" +accname +"&uid=" +accid.ToString());
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.AccPersonalInfo.AccInformationSelect(accid))
            {
                dr.Read();
                string sBirthday = Calendar.GenCalendarControl("birthday", "", "trigger_from", Convert.ToDateTime(dr["birthday"]).ToString("MM/dd/yyyy"), false, false, 0, 0, ""); //Phuong.Nguyen, show others day of next month
                TemplateHandler.Assign(ref s, "{birthday}", sBirthday);
                TemplateHandler.Assign(ref s, "{username}", accname);
                TemplateHandler.Assign(ref s, "{firstname}", dr["firstname"].ToString());
                TemplateHandler.Assign(ref s, "{lastname}", dr["lastname"].ToString());
                TemplateHandler.Assign(ref s, "{email}", dr["email"].ToString());
                TemplateHandler.Assign(ref s, "{address}", dr["address"].ToString());
                TemplateHandler.Assign(ref s, "{postal}", dr["postal"].ToString());
                TemplateHandler.Assign(ref s, "{state}", dr["state"].ToString());
                TemplateHandler.Assign(ref s, "{phone}", dr["phone"].ToString());
                TemplateHandler.Assign(ref s, "{mobile}", dr["mobilephone"].ToString());
                TemplateHandler.Assign(ref s, "{fax}", dr["fax"].ToString());
                TemplateHandler.Assign(ref s, "{city}", dr["city"].ToString());
                TemplateHandler.Assign(ref s, "{country}", dr["country"].ToString());                
            }
        }
        catch(Exception ex)
        {
            LogExtensions.ExceptionHandler(ex); 
        }
        return s;
    }   
}
