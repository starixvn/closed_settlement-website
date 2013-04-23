using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.SpecialDelete.LiveCasino
{
    [Serializable]
    [DataContract]
    public class Choice
    {
        [DataMember]
        public string Betteam { get; set; }

        [DataMember]
        public string Stake { get; set; }

        [DataMember]
        public string Odds { get; set; }
    }
}
