using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Fanex.Tarus.Entities
{
    public class AgentAccess
    {
        public int Index { get; set; }

        public int AccountId { get; set; }

        public string Account { get; set; }

        public string Role { get; set; }

        public bool GrantAccess { get; set; }

        public string AddedDate { get; set; }

        public string AddedBy { get; set; }

        public string LastModified { get; set; }

        public string LastModifiedBy { get; set; }

        public string Remark { get; set; }
    }
}
