using System;
using System.Data;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using DNAide.Core;


public partial class UserControl_EditGroup : PageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Title = "Edit Department";
        int id = CastAide.AsInt32(Request["groupId"]);
        htmlEdit.Text= GetGroupInfo(id);
    }
    private string GetGroupInfo(int groupId)
    {
        string editTpl = Sunplus.Accounting.Core.Web.WebUtils.LoadTemplate("UserControl/EditGroup.htm");
        try
        {
            TemplateHandler.Assign(ref editTpl, "{groupid}", groupId.ToString());
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.GroupInfoSelect(groupId))
            {
                while (dr.Read())
                {
                    TemplateHandler.Assign(ref editTpl, "{deptname}", CastAide.AsString(dr["groupname"]));
                    TemplateHandler.Assign(ref editTpl, "{deptdesc}", CastAide.AsString(dr["groupdesc"]));
                }
            }
        }
        catch (Exception ex)
        {

            Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex); 
        }  
        return editTpl;
    }
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
