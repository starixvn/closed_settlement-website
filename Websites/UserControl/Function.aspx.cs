using System;
using System.Text;
using System.Data;
using DNAide.Web;
using Sunplus.Accounting.Business.UserControl;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;

using Sunplus.Accounting.Business;

public partial class UserControl_Function : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        var userControll = new UserControlManager();
        var data = userControll.GetFunctionList();

        string []strListUser = AccCommon.FuncMgr.Split(';');
        bool isMgr = false;
        for (int i = 0; i < strListUser.Length; i++)
            if (string.Compare(Session["Username"].ToString(), strListUser[i], true) == 0)
            {
                isMgr = true;
                break;
            }
        if (!isMgr)
        {
            Response.Redirect("../AccessDenied.aspx");
           
        }
        htmlAdd.Text = GetListFunction();
    }
    string GetListFunction()
    {
        string strTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/Function.htm");
      
        StringBuilder strListRow = new StringBuilder();
        
        //Get list Parent function in order to bind dropdownlist
        StringBuilder strListParent = new StringBuilder();
        try
        {
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.FunctionParent("ACCOUNTING"))
            {
                strListParent.Append("<option value=''>None</option>");
                while (dr.Read())
                {
                    strListParent.Append("<option value='" + dr["FuncId"].ToString() + "'>" + dr["FuncName"].ToString() + "</option>");
                }
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex); 
        }
        TemplateHandler.Assign(ref strTpl, "slListParent", strListParent.ToString()); 
        return strTpl;
    }
}
