using System.Collections.Generic;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Admin;
using Sunplus.Accounting.Entities.Customer;

namespace Sunplus.Accounting.Business.Admin
{
    public interface IExternalSite
    {
        List<string> GetGroupName();
        ExternalSiteGridBO PagerCustomers(string groupName, int pageIndex, int pageSize, bool fromcache, string username);
        Result DeleteCustGroupName(int custId);
        Result AddCustGroupName(string username, string groupName);
        List<CustDetail> GetInfoCusts(string groupName, string username);
    }
}