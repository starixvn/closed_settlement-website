using System.Collections.Generic;

namespace Fanex.Tarus.User.Entities
{
    public class FuncManagementViewData
    {
        public List<FunctionBO> FunctionList { get; set; }
        public List<FunctionBO> ParentFunctionList { get; set; }
    }
}