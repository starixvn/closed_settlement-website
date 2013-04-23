using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.MatchWL
{
    [Serializable]
    [DataContract]
    public class MatchWL2aBONEW
    {

        [DataMember]
        public string matchcode { get; set; }

        [DataMember]
        public string eventdate { get; set; }

        [DataMember]
        public string kickofftime { get; set; }

        [DataMember]
        public bool liveindicator { get; set; }

        [DataMember]
        public string bettype { get; set; }

        [DataMember]
        public string homename { get; set; }

        [DataMember]
        public string awayname { get; set; }

        [DataMember]
        public string winlost { get; set; }

        [DataMember]
        public string pos { get; set; }

        [DataMember]
        public string comm { get; set; }

        [DataMember]
        public string VirtualComm { get; set; }

        [DataMember]
        public string LineTotal { get; set; }

        [DataMember]
        public string VirtualLineTotal { get; set; }

        [DataMember]
        public string leaguenametc { get; set; }

        [DataMember]
        public bool IsNeutral { get; set; }

        [DataMember]
        public string variance { get; set; }

        /// <summary>
        /// 0: default - render
        /// 1: Live or bettype or leaguenametc total
        /// 2: Sport total
        /// 3: Grand total
        /// </summary>
        [DataMember]
        public int typeTotal { get; set; }

        /// <summary>
        /// 0: default - render
        /// 1: 3rd (colspan='3' ast home-vs-away)
        /// 2: display link(used for ToteHR and casino)
        /// </summary>
        [DataMember]
        public int type3rd { get; set; }
    }



    public class MatchWL2aBONEWCalc
    {

        public decimal comm { get; set; }

        public decimal VirtualComm { get; set; }

        public decimal LineTotal { get; set; }

        public decimal VirtualLineTotal { get; set; }

        public decimal variance { get; set; }

    }
}
