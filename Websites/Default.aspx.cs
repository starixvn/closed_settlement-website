using System;
//using IBC.Core.LoggingLib;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Business.Security;
using System.Configuration;

public partial class _Default : System.Web.UI.Page
{
    public string accUsername = "";
    public string listMenuIgnore = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                AccCommon.LeaveSSL();
                accUsername = AppSession.AccUserName;
                listMenuIgnore = ConfigurationManager.AppSettings["ListIgnoreMenu"].ToString();
                //if (AppSession.CurrentSystem == Enumerations.AccountingSystem.Thai)
                //{
                //    mainMenu = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplateBySystem("MainMenu/MainMenu.htm", true);
                //    isThai = "true";
                //}
                //else
                //{
                //    mainMenu = "";
                //}
            }
            catch (Exception ex)
            {
               
            }
            //News news = new News();
            //LastestNewsList = news.ShowLastNewsRotaion();
        }
    }

    //public string GetUTCOffSet()
    //{
    //    if (TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours > 0)
    //    {
    //        return "+" + TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours.ToString();
    //    }
    //    else
    //    {
    //        return TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours.ToString();
    //    }
    //}
}
