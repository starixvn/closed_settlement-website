using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Customer.PT.ViewPT
{

    [DataContract]
    public class Bettype
    {
        [DataMember]
        public int BettypeId { get; set; }
        [DataMember]
        public string BettypeName { get; set; }
        [DataMember]
        public List<Bettype> Children { get; set; }
    }

    [DataContract]
    public class BettypeDTO
    {
        [DataMember]
        public List<Bettype> BettypeList { get; set; }   
    }

    public class BettypeInfo
    {
        public enum Bettype
        {
            SAToteWin = 31,
            SATotePlace = 32,
            UKToteWin = 41,
            UKTotePlace = 42,
            AUToteWin = 51,
            AUTotePlace = 52
        }
    }
}
