using System;
using System.Data;
using System.Web.UI;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using DNAide.Core;


public partial class UserControl_EditAccount : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Edit Account";
        htmlEditAcc.Text = GetAccountInfo();
    }
    private string GetAccountInfo()
    {
        string editTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/EditAccount.htm");
        string custId=Request["custId"] as string?? "";
        string dept = Request["dept"] as string ?? "";
        TemplateHandler.Assign(ref editTpl, "{dept}", dept);
        TemplateHandler.Assign(ref editTpl, "{custid}", custId);
        try
        {
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.SelectAdminInfo(custId))
            {                
                while (dr.Read())
                {             
                    TemplateHandler.Assign(ref editTpl, "{user}",CastAide.AsString(dr["username"]));
                    TemplateHandler.Assign(ref editTpl, "{fname}", CastAide.AsString(dr["firstname"]));
                    TemplateHandler.Assign(ref editTpl, "{lname}", CastAide.AsString(dr["lastname"]));
                    TemplateHandler.Assign(ref editTpl, "{groupid}", CastAide.AsString(dr["groupid"]));
                    TemplateHandler.Assign(ref editTpl, "{dept-val}", GetGroupList(CastAide.AsString(dr["groupid"])));
                }
            }
        }
        catch (Exception ex)
        {
            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }
        return editTpl;
    }
    private string GetGroupList(string groupId)
    {
        string content = string.Empty;
        try
        {
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.GroupInfoSelect())
            {                
                while (dr.Read())
                {
                    if (groupId == CastAide.AsString(dr["groupid"]))
                    {
                        content += "<option selected=\"true\" value=\"" + CastAide.AsString(dr["groupid"]) + "\">" + CastAide.AsString(dr["groupname"]) + "</option>";
                    }
                    else
                    {
                        content += "<option value=\"" + CastAide.AsString(dr["groupid"]) + "\">" + CastAide.AsString(dr["groupname"]) + "</option>";
                    }                    
                }
            }
        }
        catch (Exception ex)
        {

            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
        }   
        return content;

    }
}
