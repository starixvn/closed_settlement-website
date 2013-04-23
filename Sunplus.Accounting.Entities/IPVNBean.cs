using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunplus.Accounting.Entities
{
    [Serializable]
    public class IPVNBean
    {
        private double _IPFrom;
        private double _IPTo;

        public IPVNBean(double ipFrom, double ipTo)
        {
            _IPFrom = ipFrom;
            _IPTo = ipTo;
        }

        public double IPFrom
        {
            get { return _IPFrom; }
            set { _IPFrom = value; }
        }

        public double IPTo
        {
            get { return _IPTo; }
            set { _IPTo = value; }
        }
    }
}
