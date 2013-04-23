using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Norm;

namespace Sunplus.Data.DataAbstract.Entity
{
    public class IpVn
    {
        public IpVn()
        {
            //Id = new System.Guid().ToString();
        }

        [MongoIdentifier]
        public string Id { get; set; }

        public string IpFrom { get; set; }
        public string IpTo { get; set; }
        public bool IsManual { get; set; }
    }
}
