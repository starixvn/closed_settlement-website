using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.CreditTerm
{
    [DataContract]
    [Serializable]
    public class Group
    {
        [DataMember]
        public int GroupId { get; set; }
        [DataMember]
        public string HeadUsername { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string Checker { get; set; }
        [DataMember]
        public bool Permisssion { get; set; }
    }
}
