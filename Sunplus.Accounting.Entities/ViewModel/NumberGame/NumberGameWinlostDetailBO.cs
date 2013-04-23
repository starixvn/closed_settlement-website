using System.Runtime.Serialization;
using System.Collections.Generic;
using System;

/// <summary>
/// Summary description for NumberGameWinLostByMatchBO
/// </summary>
/// 

namespace Sunplus.Accounting.Entities.ViewModel
{
    [Serializable]
    [DataContract]
    public class NumberGameWinLostDetailBO
    {

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string TurnOver { get; set; }

        [DataMember]
        public string Commission { get; set; }

        [DataMember]
        public string MemberWinlost { get; set; }
        
        [DataMember]
        public string MemberComm { get; set; }
        
        [DataMember]
        public string MemberTotal { get; set; }

        [DataMember]
        public string AgentWinlost { get; set; }

        [DataMember]
        public string AgentComm { get; set; }

        [DataMember]
        public string AgentTotal { get; set; }

        [DataMember]
        public string MasterWinlost { get; set; }

        [DataMember]
        public string MasterComm { get; set; }

        [DataMember]
        public string MasterTotal { get; set; }

        [DataMember]
        public string SuperWinlost { get; set; }

        [DataMember]
        public string SuperComm { get; set; }

        [DataMember]
        public string SuperTotal { get; set; }

        [DataMember]
        public string CompanyTotal { get; set; }


    }
}