using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Deposit
{
    [Serializable]
    [DataContract]
    public class ConcurrentUser
    {
        [DataMember]
        public string  Date { get; set; }

        [DataMember]
        public int TotalUser { get; set;}
    }

    [Serializable]
    [DataContract]
    public class ConcurrentUserDTO
    {
        [DataMember]
        public List<ConcurrentUser> ListConcurrentUser { get; set; }

        [DataMember]
        public double AverageUser { get; set; }

        [DataMember]
        public double Total { get; set; }

        [DataMember]
        public int MaxUser { get; set; }

        [DataMember]
        public int MinUser { get; set; }
    }
}
