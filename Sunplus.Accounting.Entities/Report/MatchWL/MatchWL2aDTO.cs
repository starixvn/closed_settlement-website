using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.MatchWL
{
    [Serializable]
    [DataContract]
    public class MatchWL2aDTO
    {
        [DataMember]
        public IEnumerable<MatchWL2aBO> rows { get; set; }

        [DataMember]
        public IEnumerable<MatchWL2aBO> rows3rd { get; set; }

        [DataMember]
        public IEnumerable<MatchWL2aBO> rowsMatchWLTotal { get; set; }

        [DataMember]
        public IEnumerable<WLbyBettype> rowsWLbyBettype { get; set; }

        [DataMember]
        public IEnumerable<WLbyBettype> rowsWLbyBettypeTotal { get; set; }

        [DataMember]
        public string wl8rm { get; set; }

        [DataMember]
        public int currpage { get; set; }

        [DataMember]
        public bool IsFinish { get; set; }

        [DataMember]
        public string FI_3rdShare { get; set; }
    }
}
