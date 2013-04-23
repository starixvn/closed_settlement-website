using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.User.Entities;
using Sunplus.Accounting.Entities;
using FunctionBO = Fanex.Tarus.User.Entities.FunctionBO;

namespace Fanex.Tarus.User.DataAccess
{
    public interface IFunctionAccess
    {
        List<FunctionBO> GetFunctionParentList(string systemName);
        List<FunctionBO> GetFunctionList();
        Result UpdateFunction(int intFuncId, int intUserId, string strName, string strDesc, string strFileNames, string System, string parentId);
        Result InsertFunction(string strName, string strDesc, string strFileNames, string strParent, int intUserId, string system);
        Result DeleteFunction(int intFuncId, int intUserId);
    }
}
