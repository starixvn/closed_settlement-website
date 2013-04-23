using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.MatchWL
{


    [Serializable]
    [DataContract]
    public class WLbyBettypeDTO
    {
        [DataMember]
        public List<WLbyBettype> Items { get; set; }

        [DataMember]
        public WLbyBettype Total { get; set; }
    }
   

    [Serializable]
    [DataContract]
    public class MatchWLBySportDTO
    {
        public List<MatchWL2aBySport> Items { get; set; }

        [DataMember]
        public MatchWL2aBySport Item { get; set; }

        [DataMember]
        public int NextIndex { get; set; }

        [DataMember]
        public int NextBtypeIndex { get; set; }
    }

    [Serializable]
    [DataContract]
    public class MatchWLFooterDTO
    {
        [DataMember]
        public MatchWL2aBONEW GrandTotal1 { get; set; }

        [DataMember]
        public string wl8rm { get; set; }

        [DataMember]
        public string wlBARM { get; set; }

        [DataMember]
        public List<MatchWL2aBONEW> Items { get; set; }

        [DataMember]
        public MatchWL2aBONEW GrandTotal2 { get; set; }


    }

    /*----------------------------------------------------------*/

    

    [Serializable]
    [DataContract]
    public class MatchWL2aByLiveIndicator
    {
        [DataMember]
        public List<MatchWL2aBONEW> Items { get; set; }

        [DataMember]
        public MatchWL2aBONEW Total { get; set; }
    }

    [Serializable]
    [DataContract]
    public class MatchWL2aByBettype
    {
        [DataMember]
        public List<MatchWL2aByLiveIndicator> Items { get; set; }

        [DataMember]
        public int TotalRow { get; set; }
    }

   
    [Serializable]
    [DataContract]
    public class MatchWL2aBySport
    {
        [DataMember]
        public List<MatchWL2aByBettype> Items { get; set; }

        [DataMember]
        public MatchWL2aBONEW Total { get; set; } 
    }



   
}
