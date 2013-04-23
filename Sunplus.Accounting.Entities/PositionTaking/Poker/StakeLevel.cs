using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.PositionTaking.Poker
{
    [DataContract]
    public class StakeLevel
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool IsChecked { get; set; }
        [DataMember]
        public bool IsAllow { get; set; }
    }
}
