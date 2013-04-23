using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.PositionTaking.Poker
{
    [DataContract]
    public class GameComm
    {
        [DataMember]
        public string _Name { get; set; }
      
        [DataMember]
        public string Comm { get; set; }

        [DataMember]
        public string MaxComm { get; set; }

        [DataMember]
        public string UplineComm { get; set; }

    }
}
