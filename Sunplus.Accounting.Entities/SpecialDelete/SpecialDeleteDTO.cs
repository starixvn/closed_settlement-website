using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.SpecialDelete
{
    [Serializable]
    [DataContract]
    public class SpecialDeleteDTO
    {
        [DataMember]
        public string MatchCode { get; set; }

        [DataMember]
        public string HomeName { get; set; }

        [DataMember]
        public string AwayName { get; set; }

        [DataMember]
        public string EventDate{ get; set; }

        [DataMember]
        public int ErrorCode { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public IEnumerable<SpecialDeleteBO> rows { get; set; }

        [DataMember]
        public List<SpecialDeleteByType> Items { get; set; }

        [DataMember]
        public List<string> TransIdNormalBet { get; set; }
        
    }


    [Serializable]
    [DataContract]
    public class SpecialDeleteByType
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string OptionType { get; set; }

        [DataMember]
        public bool IsAllowOptionDelete { get; set; }

        [DataMember]
        public List<SpecialDeleteBO> Items { get; set; }

        [DataMember]
        public List<SpecialDeleteBetEarlyKO> KoItems { get; set; }
    }
}
