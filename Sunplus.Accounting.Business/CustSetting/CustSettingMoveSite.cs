using System.Data;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Web.TplCore.App_Templates.Customer.App_Templates.Customer;
using Sunplus.Accounting.DataAccess.AdoNet.SqlServer.CustSetting;

/// <summary>
/// Summary description for CustSettingMoveSite
/// </summary>
namespace Sunplus.Accounting.Business.CustSetting
{
    public class CustSettingMoveSite
    {
        #region GetSiteNameByCustId
        public string GetSiteNameByCustId(int custId, int roleId, string siteName, string userName)
        {
            var cust = new CustSettingProviderClient();
            bool isAllow = false;
            if (AppSession.SYSTEM.Equals(Enumerations.AccountingSystem.Accounting))
                isAllow = Permissions.GetHostPermission(Enumerations.Functions.Customer.MoveSite) && roleId != 1;
            
            if (isAllow)
                siteName = "<div id='dvSite'><a target='centerFr' href='Customer/ChangeSite.aspx?CustId=" + custId +
                           "&Site=" + siteName + "&UserName=" + userName + "&RoleId=" + roleId +
                           "' onclick='parent.resetMenu();'>" + siteName + "</a></div>";
            else
                siteName = "<div id='dvSite'>" + siteName + "</div>";

            return siteName;
        }
        #endregion
        #region LoadChangSiteForm
        public string LoadChangSiteForm(string CurrentSite, string UserName, int RoleId, int CustId)
        {
            var html = new ChangeSite_Template();
            html.CurrentSite = CurrentSite;
            string level = "";
            switch (RoleId)
            {
                case 1:
                    level = "Member";
                    break;
                case 2:
                    level = "Agent";
                    break;
                case 3:
                    level = "Master";
                    break;
                case 4:
                    level = "Super";
                    break;
            }
            html.Level = level;

            if (string.IsNullOrEmpty(UserName))
            {
                UserName =
                    "<input style='width:100px' title='Please input user name' type='text' id='txtUserName' name='txtUserName' onkeypress='OnEnter(event);' onchange='changeUserName();' />";
                UserName +=
                    "  <input type='button' class='btn' id='btnGetSite' name='btnGetSite' value='Submit' onclick='GetSiteInfo();'/>";
                html.disabled = "disabled";
            }
            else
                html.disabled = "";

            html.UserName = UserName;

            html.NewSite = GetAllSiteNames();
            html.CustId = CustId.ToString();
            html.UserName = UserName;
            if (CustId == 0) html.IsSameLine = "0";
            else html.IsSameLine = "1";
            return html.ToString();
        }
        #endregion

        #region GetAllSiteNames
        public string GetAllSiteNames()
        {
            string site = "";
            if (CacheUtils.GetAllSiteNames == null)
            {
                var cust = new CustSettingProviderClient();
                DbResult rs = cust.GetAllSiteNames();
                DataRow[] Rows = rs.ResultSet.Tables[0].Select();
                site = "<select id='Site' name='Site' style='width:100px;'>";
                foreach (DataRow dr in Rows)
                {
                    site += "<option value='" + dr["Site"] + "'>" + dr["Site"] + "</option>";
                }
                site += "</select>";

                CacheUtils.GetAllSiteNames = site;
            }
            else
                site = CacheUtils.GetAllSiteNames;

            return site;
        }
        #endregion

        #region UpdateMoveSite
        public string UpdateMoveSite(int CustId, string NewSite)
        {
            var cust = new CustSettingProviderClient();
            DbResult rs = cust.CustomerUdpMoveSite(AppSession.AccId, CustId, NewSite);
            int ErrCode = CastAide.AsInt32(rs.Outputs["errcode"]);
            string Msg = "no|" + CastAide.AsString(rs.Outputs["errmsg"]);
            if (ErrCode == 0)
                Msg = "ok|Move site for this customer and downlines successfully";

            return Msg;
        }
        #endregion
    }
}
