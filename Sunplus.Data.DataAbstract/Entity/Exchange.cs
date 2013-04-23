using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Norm;
using Norm.Attributes; 

namespace Sunplus.Data.DataAbstract.Entity
{
    [Serializable]
    public class Exchange
    {
        public string Exchangeid { get; set; }

        public string Currency { get; set; }

        public string Stakerate { get; set; }

        public string Actualrate { get; set; }

        public string Commaxpayout { get; set; }

        public string Creator { get; set; }

        public string Minbet { get; set; }

        public string Minbetlive { get; set; }

        public string AudRate { get; set; }

        public string ZarRate { get; set; }

        public string GbpRate { get; set; }

        public string EuroRate { get; set; }

        public string UsRate { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string MultipliedBy { get; set; }
    }
}
