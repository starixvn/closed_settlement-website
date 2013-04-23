using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Customer
{
    [Serializable]
    [DataContract]
    public class ForexAccount
    {
        [DataMember]
        public string username { get; set; }
    }
}
