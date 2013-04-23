using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Entities
{
    [Serializable]
    public class QuickLink
    {
        public int Id { get; set; }

        public int AncestorId { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int NumOfAppear { get; set; }

        public bool IsReportMenu { get; set; }

        public string ServerPath { get; set; }

        public string ParentName { get; set; }

        public string Target { get; set; }
    }
}
