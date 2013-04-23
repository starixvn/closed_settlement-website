using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Security
{
    [DataContract]
    [Serializable]
    public class LogFailedInformation
    {
        [DataMember]
        public int LogID { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public DateTime LoginTime { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public DateTime UnLockTime { get; set; }
    }
}
