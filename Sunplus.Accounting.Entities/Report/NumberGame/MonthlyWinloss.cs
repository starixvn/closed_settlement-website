using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.NumberGame
{
    [Serializable]
    [DataContract]
    public class MonthlyWinloss
    {
        //select -1371933.4172 winlost,
        //-1025750.1216 position,
        //0 commission,
        //346183.2956 linetotal,
        //9306134.0896 turnover,
        //3.71 margin

        [DataMember]
        public double Winlost { get; set; }
        [DataMember]
        public double Position { get; set; }
        [DataMember]
        public double Commission { get; set; }
        [DataMember]
        public double LineTotal { get; set; }
        [DataMember]
        public double TurnOver { get; set; }
        [DataMember]
        public double Margin { get; set; }
    }
}
