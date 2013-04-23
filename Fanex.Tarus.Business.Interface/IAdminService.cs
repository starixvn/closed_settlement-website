using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunplus.Accounting.Entities;

using Fanex.Tarus.Entities;


namespace Fanex.Tarus.Business.Interface
{
    public interface IAdminService
    {
        List<AgentAccess> GetListAgentAcess();
        Result AddUser(string userName, int creator, string remark);
        Result Update(int custId, bool isAllowed, string remark);
        Result Delete(int custId);
        List<QuickLink> GetSearchMenu(string keyWord, bool isSuggestion, List<MenuBO> listUserMenu, List<MenuBO> listReportMenu);
    }
}
