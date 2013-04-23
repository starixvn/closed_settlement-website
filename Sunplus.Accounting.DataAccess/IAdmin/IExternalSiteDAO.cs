using System.Data;
using Sunplus.Accounting.Entities;

namespace Sunplus.Accounting.DataAccess.IAdmin
{
    public interface IExternalSiteDAO
    {
         DataTable SelGroupName();
        DataTable SelInfoCust(string groupName);
        Result DelCust(int custId, int userId);
        Result AddExistCust(string username, string groupName, int userId);
    }
}