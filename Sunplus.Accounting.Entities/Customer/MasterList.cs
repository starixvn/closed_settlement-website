using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Customer
{
    [Serializable]

    [DataContract]
    public class MasterList
    {
        [DataMember]
        public string No { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string MaxEx { get; set; }

        [DataMember]
        public string VirExrate { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string CustId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string NickName { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string GroupA { get; set; }

        [DataMember]
        public string GroupB { get; set; }

        [DataMember]
        public string GroupC { get; set; }

        [DataMember]
        public string GroupD { get; set; }

        [DataMember]
        public string _1X2 { get; set; }

        [DataMember]
        public string OtherCom { get; set; }

        [DataMember]
        public string ngcom { get; set; }

        [DataMember]
        public string LoginIp { get; set; }
    }
}
