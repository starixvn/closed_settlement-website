using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.PositionTaking.Racing
{
    [Serializable]
    [DataContract]
    public class Sport
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool Visible { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public List<Tote> Totes { get; set; }
      
    }
}
