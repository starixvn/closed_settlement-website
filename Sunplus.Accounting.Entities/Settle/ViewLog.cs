using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Settle
{
    public class ViewLog
    {
    }

    [DataContract]
    public class SettleLogDetail
    {
        [DataMember]
        public int No { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Action { get; set; }
        [DataMember]
        public string HTScore { get; set; }
        [DataMember]
        public string FTScore { get; set; }
        [DataMember]
        public string GoalRecord { get; set; }
        [DataMember]
        public string ResultString { get; set; }
        [DataMember]
        public string Remark { get; set; }
        [DataMember]
        public string Ip { get; set; }
    }

    [DataContract]
    public class SettleLog
    {
        [DataMember]
        public string LeagueName { get; set; }
        [DataMember]
        public string HomeName { get; set; }
        [DataMember]
        public string AwayName { get; set; }
        [DataMember]
        public string EventDate { get; set; }
        [DataMember]
        public string MatchId { get; set; }

        [DataMember]
        public string ShowTime { get; set; }

        [DataMember]
        public int SportType { get; set; }

        [DataMember]
        public List<SettleLogDetail> SettleLogDetails { get; set; }
    }

    [DataContract]
    public class SettleLogResult
    {
        [DataMember]
        public List<SettleLog> Rows { get; set; }
    }
}
