using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sunplus.Accounting.Entities.Customer;

namespace Sunplus.Accounting.Entities.PositionTaking.Poker
{
    [DataContract]
    public class PokerSetting
    {
        [DataMember]
        public CustDetail CustInfo { get; set; }

        [DataMember]
        public BetSetting BetSetting { get; set; }

        [DataMember]
        public List<GameTypeSetting> GameSettings { get; set; }
        
        [DataMember]
        public int errCode { get; set; }

        [DataMember]
        public string errMessage { get; set; }

        [DataMember]
        public List<GameComm> GamesComm { get; set; }
    }
}
