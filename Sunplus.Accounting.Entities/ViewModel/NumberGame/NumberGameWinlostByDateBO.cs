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
    public class NumberGameWinLostByDateBO
    {

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

       
        [DataMember]
        public bool IsLive { get; set; }

        [DataMember]
        public int BetType { get; set; }

        [DataMember]
        public string BetTypeName { get; set; }
        
        [DataMember]
        public string Winlost { get; set; }
        
        [DataMember]
        public string Position { get; set; }
        
        [DataMember]
        public string Commission { get; set; }

        [DataMember]
        public string LineTotal { get; set; }

        [DataMember]
        public string TurnOver { get; set; }

        [DataMember]
        public string Margin { get; set; }

        [DataMember]
        public string VirtualLineTotal { get; set; }

        [DataMember]
        public string Variance { get; set; }

        [DataMember]
        public List<NumberGameWinLostByDateBO> children { get; set; }
    }
}