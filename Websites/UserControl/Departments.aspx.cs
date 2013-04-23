using System;
using System.Data;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;
using DNAide.Core;



public partial class UserControl_Departments : PageBase
{

    private bool hasData = false;
    private string strContent = string.Empty;
    protected string strRe = "";
    protected void Page_Load(object sender, EventArgs e)
    {    
        Page.Title = "Department";                
        //this script to reload Account List by group after update account successfully
        strRe =Request["re"] as string ?? "" ;
        strRe = "var re='" + strRe + "';";
        string str = Request["gId"] as string ?? "";        
        strRe += " id='"+ str +"';";
        string dept = Request["dept"] as string ?? "";
        strRe += " dept='"+ dept +"'";

        literalGroupList.Text = GetGroupList();
        literalDepartmentList.Text = GetGroupList();
        literalDepartmentListTemp.Text = literalDepartmentList.Text;
    }
    private static string GetGroupList()
    {
        string content = string.Empty;        
        try
        {
            using (IDataReader dr = Sunplus.Accounting.DataAccess.IBCDLL.Accounting.UserControl.GroupInfoSelect())
            {                
                content += "<option value=\"0\">[Select]</option>";
                while (dr.Read())
                {
                    content += "<option value=\"" + CastAide.AsString(dr["groupid"]) + "\">" + CastAide.AsString(dr["groupname"]) + "</option>";
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
