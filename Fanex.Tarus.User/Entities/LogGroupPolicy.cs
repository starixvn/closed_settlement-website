using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.User.Entities
{
    public class LogGroupPolicy
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<Account> Members { get; set; }
        public string PoliciesOld { get; set; }
        public string PoliciesNew { get; set; }
        public string GroupNameOld { get; set; }
        public string GroupNameNew { get; set; }
        public int GroupIdOld { get; set; }
        public int GroupIdNew { get; set; }
    }
}
