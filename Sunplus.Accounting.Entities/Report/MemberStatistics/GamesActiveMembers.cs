using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.MemberStatistics
{
    [Serializable]
    [DataContract]
    public class GamesActiveMembers
    {
        [DataMember]
        public string GameName { get; set; }
        [DataMember]
        public int NoActives { get; set; }
    }

    [Serializable]
    [DataContract]
    public class GamesActiveMembersBO
    {
        [DataMember]
        public List<GamesActiveMembers> ActiveMembersList { get; set; }
    }
}
