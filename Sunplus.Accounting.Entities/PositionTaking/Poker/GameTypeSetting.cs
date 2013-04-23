using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.PositionTaking.Poker
{
    [DataContract]
    public class GameTypeSetting
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public List<StakeLevel> StakeLevels { get; set; }
    }
}
