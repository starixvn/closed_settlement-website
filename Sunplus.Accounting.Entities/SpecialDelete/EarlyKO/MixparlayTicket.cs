using Sunplus.Accounting.Entities.SpecialDelete;
using System.Runtime.Serialization;
using System.Collections.Generic;
namespace Sunplus.Accounting.Entities.SpecialDelete.EarlyKO
{
    [DataContract]
    public class MixparlayTicket
    {
        [DataMember]
        public string Account { get; set; }

        [DataMember]
        public string Refno { get; set; }

        [DataMember]
        public string TransId { get; set; }

        [DataMember]
        public string TransTime { get; set; }

        [DataMember]
        public List<MixparlayChoice> Choices { get; set; }

        [DataMember]
        public string Stake { get; set; }

        [DataMember]
        public string Currency { get; set; }

        
    }

    [DataContract]
    public class MixparlayChoice
    {
        [DataMember]
        public string Refno { get; set; }

        [DataMember]
        public string Creator { get; set; }

        [DataMember]
        public string TransId { get; set; }

        [DataMember]
        public string SubTransId { get; set; }
        
        [DataMember]
        public string IsDelete { get; set; }

        [DataMember]
        public string Choice { get; set; }
    }
}
