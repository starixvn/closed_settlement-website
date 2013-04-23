using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Fanex.Tarus.User.DataAccess
{
    public interface IUserLogAccess
    {
        DataTable UserControlLogSelect(DateTime fromDate, DateTime toDate, int type, int custId, string userName, int pageIndex, int pageSize);
        int GetPagesTotal(DateTime fromDate, DateTime toDate, int type, int custId, string userName);
        int GetNameFromId(int typeId, string name, out string message);
    }
}
