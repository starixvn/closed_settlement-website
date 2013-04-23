using System.Collections.Generic;
using System.Runtime.Serialization;
using Sunplus.Accounting.Entities.Customer;

namespace Sunplus.Accounting.Entities.PositionTaking.Poker
{
    [DataContract]
    public class PokerStatus
    {
        [DataMember]
        public CustDetail CustInfo { get; set; }

        [DataMember]
        public bool disableDLP2P { get; set; }

        [DataMember]
        public bool disableP2P { get; set; }

        [DataMember]
        public bool disableUplineP2P { get; set; }

        [DataMember]
        public bool disableDLFunP2P { get; set; }

        [DataMember]
        public bool disableFunP2P { get; set; }

        [DataMember]
        public bool disableUplineFunP2P { get; set; }

        [DataMember]
        public int errCode { get; set; }

        [DataMember]
        public string errMessage { get; set; }

       
    }
}
