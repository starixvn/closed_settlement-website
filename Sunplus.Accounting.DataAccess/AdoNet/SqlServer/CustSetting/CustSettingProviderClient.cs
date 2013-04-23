using System.Collections;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.Provider.CustSetting;

/// <summary>
/// Summary description for CustSetting
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.CustSetting
{
    public class CustSettingProviderClient : CustSettingProvider
    {
        public override bool IsAllowCustomerMoveSite(int custid, ref int RoleId)
        {
            var hash = new Hashtable();
            hash.Add("custid", custid);
            hash.Add("isallow", 0);
            hash.Add("roleid", 0);
            hash.Add("userid", AppSession.AccId);
            var spElement = new SpConfig().GetStoreProcedure("CustomerSelCheckMoveSite");
            DbResult rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 200);
            bool IsAllow = CastAide.AsBoolean(rs.Outputs["isallow"]);
            RoleId = CastAide.AsInt32(rs.Outputs["roleid"]);

            return IsAllow;
        }

        public override DbResult CustomerUdpMoveSite(int UserId, int CustId, string NewSite)
        {
            var hash = new Hashtable();
            hash.Add("userid", UserId);
            hash.Add("custid", CustId);
            hash.Add("site", CastAide.AsString(NewSite));
            hash.Add("errcode", 0);
            hash.Add("errmsg", "");
            hash.Add("ip", HttpContext.Current.Request.UserHostAddress);

            DbResult rs = DbHelper.Execute("Acc_CustomerUpdMoveSite", hash, DBType.MAIN, 1000);
            return rs;
        }

        public override DbResult GetAllSiteNames()
        {
            var spElement = new SpConfig().GetStoreProcedure("SiteSel");
            return DbHelper.Execute(spElement.Name, spElement.DbName);
        }

        private class SpConfig:DataAccess
        {
            public Core.Configurations.StoreProcedureElement GetStoreProcedure(string aliasName)
            {
                return GetStoreProcedure(Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl, aliasName);
            }
        }
    }
}