using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunplus.Accounting.Entities
{
    public class Site
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsLicense { get; set; }
    }

    public class SiteDTO
    {
        public string Name { get; set; }
        public List<Site> Sites { get; set; }
    }
}
