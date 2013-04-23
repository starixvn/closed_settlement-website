using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.User.Entities
{
    public class UserControlBO
    {
        public int ID { get; set; }
        
        public int ParentId { get; set; }

        public bool IsGroupRead { get; set; }

        public bool IsGroupHost { get; set; }

        public bool IsRead { get; set; }

        public bool IsHost { get; set; }

        public string FuncName { get; set; }

        public string FuncDesc { get; set; }

        public string GroupName { get; set; }

        public int TotalSub { get; set; }

        public List<UserControlBO> SubFuncs { get; set; }
    }
}
