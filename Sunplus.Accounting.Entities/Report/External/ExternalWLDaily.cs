using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.External
{
    [Serializable]
    [DataContract]
    public class ExternalWLDailyBO
    {
        [DataMember]
        public List<ExternalWLDailyRow> Items { get;set; }

        [DataMember]
        public ExternalWLDailyRow Total { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ExternalWLDailyRow
    {
      
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public int CustId { get; set; }

        [DataMember]
        public string TotalTO { get; set; }

        [DataMember]
        public string CompanyTO { get; set; }

        [DataMember]
        public string CompanyPT { get; set; }

        [DataMember]
        public string SuperTO { get; set; }

        [DataMember]
        public string SuperPT { get; set; }

        [DataMember]
        public string MasterTO { get; set; }

        [DataMember]
        public string MasterPT { get; set; }

        [DataMember]
        public string AgentTO { get; set; }

        [DataMember]
        public string AgentPT { get; set; }

        [DataMember]
        public string CompanyWL { get; set; }

        [DataMember]
        public string CompanyMargin { get; set; }

        [DataMember]
        public string SuperWL { get; set; }

        [DataMember]
        public string SuperMargin { get; set; }

        [DataMember]
        public string MasterWL { get; set; }

        [DataMember]
        public string MasterMargin { get; set; }

        [DataMember]
        public string AgentWL { get; set; }

        [DataMember]
        public string AgentMargin { get; set; }
    }

}
