using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.User.Entities
{
    public class LogAdminPolicy
    {
        public int AccId { get; set; }
        public string AccName { get; set; }
        public string PoliciesOld { get; set; }
        public string PoliciesNew { get; set; }
    }
}
