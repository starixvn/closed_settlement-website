using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Others
{
    [Serializable]
    [DataContract]
    public class DuplicateTicketsDTO
    {
         [DataMember]
        public List<DuplicateTickets> Items { get; set; }
    }

    [Serializable]
    [DataContract]
    public class DuplicateTickets
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string CustId { get; set; }

        [DataMember]
        public string Bettype { get; set; }

        [DataMember]
        public string WinLostDate { get; set; }

        [DataMember]
        public string BalanceBeforePlaceBet { get; set; }

        [DataMember]
        public string Stake { get; set; }

        [DataMember]
        public string Odds { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string RefNo1stTicket { get; set; }

        [DataMember]
        public string RefNo2ndTicket { get; set; }

        [DataMember]
        public string BetTime1stTicket { get; set; }

        [DataMember]
        public string BetTime2ndTicket { get; set; }

        [DataMember]
        public string TimeDiff { get; set; }
    }
}
