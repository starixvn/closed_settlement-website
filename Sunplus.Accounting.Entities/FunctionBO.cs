using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities
{
    [DataContract]
    public class FunctionBO
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Order { get; set; }

        [DataMember]
        public string Desc { get; set; }

        [DataMember]
        public string Files { get; set; }

        [DataMember]
        public int ParentId { get; set; }

        [DataMember]
        public int TotalSub { get; set; }

        [DataMember]
        public string System { get; set; }

        [DataMember]
        public List<FunctionBO> SubFuncs { get; set; }
    }
}