using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Customer.PT.ViewPT
{
    [DataContract]
    public class Sport
    {
        [DataMember]
        public int SportId { get; set; }
        [DataMember]
        public string SportName { get; set; }
        [DataMember]
        public List<Sport> Children { get; set; }
        [DataMember]
        public BettypeDTO Bettype { get; set; }
    }

    [DataContract]
    public class SportDTO
    {
        [DataMember]
        public List<Sport> SportsList { get; set; }
    }
}