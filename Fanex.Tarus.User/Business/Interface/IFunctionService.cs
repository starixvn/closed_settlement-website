using System.Collections.Generic;
using Fanex.Tarus.User.Entities;
using Sunplus.Accounting.Entities;
using FunctionBO = Fanex.Tarus.User.Entities.FunctionBO;
namespace Fanex.Tarus.User.Business.Interface
{
    public interface IFunctionService
    {
        List<FunctionBO> GetAccountingFunctionList();
        List<FunctionBO> GetFunctionParentList(string systemName);
        Result EditFunction(int funcId, string name, string desc, string fileNames, string parent, string systemName);
        Result AddFunction(string name, string desc, string fileNames, string parent, string systemName);
        Result DeleteFunction(int funcId);
    }
}
