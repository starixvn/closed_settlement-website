using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Customer
{
    [Serializable]

    [DataContract]
    public class BetSettingNG
    {
        [DataMember]
        public string minbet { get; set; }

        [DataMember]
        public string maxbet { get; set; }

        [DataMember]
        public string maxperball { get; set; }

        [DataMember]
        public string maxpermach { get; set; }

        [DataMember]
        public int isEnable { get; set; }

        [DataMember]
        public int errCode { get; set; }

        [DataMember]
        public string errMsg { get; set; }

       
    }
}
