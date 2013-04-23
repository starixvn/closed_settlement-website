using System.Collections.Generic;
using Fanex.Tarus.Entities.User.AccountManagement;

namespace Fanex.Tarus.Web.Models.User.AccountManagement
{
    public class ViewData
    {
        public List<Department> Departments { get; set; }
        public List<Account> Accounts { get; set; }
    }
}