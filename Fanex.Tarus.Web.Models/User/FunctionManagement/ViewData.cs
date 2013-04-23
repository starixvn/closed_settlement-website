using System.Collections.Generic;
using Sunplus.Accounting.Entities;

namespace Fanex.Tarus.Web.Models.User.FunctionManagement
{
    public class ViewData
    {
        public List<FunctionBO> FunctionList { get; set; }
        public List<FunctionBO> ParentFunctionList { get; set; }
    }
}