using System;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

/// <summary>
/// Summary description for CustSettingProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.CustSetting
{
    public abstract class CustSettingProvider : DataAccess
    {
        public static CustSettingProvider Instance(DAOClient client)
        {
            return
                (CustSettingProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.CustSetting.CustSettingProviderType,
                                               GetDAOClient(client), GetDataSource(DataSource.SqlServer))));
        }

        public virtual bool IsAllowCustomerMoveSite(int custid, ref int RoleId)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult CustomerUdpMoveSite(int UserId, int CustId, string NewSite)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult GetAllSiteNames()
        {
            throw new NotImplementedException();
        }
    }
}