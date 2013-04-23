using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Entities;
using Sunplus.Accounting.Entities;

namespace Fanex.Tarus.DataAccess.Interface
{

    public interface IAdminAccess
    {
        List<AgentAccess> GetListAgentAcess();
        Result AddUser(string userName, int creator, string remark);
        Result Update(int custId, bool isAllowed, string remark);
        Result Delete(int custId);
    }
}
