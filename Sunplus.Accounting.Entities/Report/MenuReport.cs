using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunplus.Accounting.Entities.Report
{
    public class MenuReport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public MenuReport Parent { get; set; }
        public string Target { get; set; }
        public int Order { get; set; }

        public IEnumerable<MenuReport> GetChildList(IEnumerable<MenuReport> menuItemCollection)
        {
            if (menuItemCollection != null && menuItemCollection.Count() > 0)
            {
                return menuItemCollection
                    .Select(menu => menu)
                    .Where(menuItem => (menuItem.Parent != null && menuItem.Parent.Id == this.Id));
            }

            return new List<MenuReport>();
        }
    }
}
