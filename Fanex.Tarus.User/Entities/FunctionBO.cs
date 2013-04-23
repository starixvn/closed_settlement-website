using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.User.Entities
{
    public class FunctionBO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public string Desc { get; set; }

        public string Files { get; set; }

        public int ParentId { get; set; }

        public int TotalSub { get; set; }

        public string System { get; set; }

        public List<FunctionBO> SubFuncs { get; set; }
    }
}
