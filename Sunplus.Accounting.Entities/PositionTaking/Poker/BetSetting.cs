using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.PositionTaking.Poker
{
    [DataContract]
    public class BetSetting
    {
        [DataMember]
        public decimal TransferCredit { get; set; }

        [DataMember]
        public decimal MinTransferCredit { get; set; }

        [DataMember]
        public decimal MaxTransferCredit { get; set; }

        [DataMember]
        public bool IsAutoReset { get; set; }

        [DataMember]
        public bool IsUplineAutoReset { get; set; }
    }
}
