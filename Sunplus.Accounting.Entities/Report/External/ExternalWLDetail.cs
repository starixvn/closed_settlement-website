using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.External
{
    [Serializable]
    [DataContract]
    public class ExternalWLDetail
    {
        [DataMember]
        public List<ExternalWLDetailRow> Items { get; set; }

        [DataMember]
        public ExternalWLDetailRow Total { get; set; }

        public IEnumerator<object> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    [DataContract]
    public class ExternalWLDetailRow
    {
        [DataMember]
        public int CustId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Turnover { get; set; }

        [DataMember]
        public string GrossComm { get; set; }

        [DataMember]
        public string WinLoss { get; set; }

        [DataMember]
        public string Comm { get; set; }

        [DataMember]
        public string Total { get; set; }

        [DataMember]
        public string AWinLoss { get; set; }

        [DataMember]
        public string AComm { get; set; }

        [DataMember]
        public string ATotal { get; set; }

        [DataMember]
        public string MWinLoss { get; set; }

        [DataMember]
        public string MComm { get; set; }

        [DataMember]
        public string MTotal { get; set; }

        [DataMember]
        public string SWinLoss { get; set; }

        [DataMember]
        public string SComm { get; set; }

        [DataMember]
        public string STotal { get; set; }

        [DataMember]
        public string Company { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Param
    {
        [DataMember]
        public string custid { get; set; }

        [DataMember]
        public string fromdate { get; set; }

        [DataMember]
        public string todate { get; set; }

        [DataMember]
        public string showSB { get; set; }

        [DataMember]
        public string showCasino { get; set; }

        [DataMember]
        public string showP2P { get; set; }

        [DataMember]
        public string showRB { get; set; }

        [DataMember]
        public string showFI { get; set; }

        [DataMember]
        public string showNG { get; set; }

        [DataMember]
        public string showBI { get; set; }

        [DataMember]
        public string showLC { get; set; }

        [DataMember]
        public Sorting Sorting { get; set; }

    }

    [Serializable]
    [DataContract]
    public class Sorting
    {
        [DataMember]
        public string fieldName { get; set; }
        [DataMember]
        public bool IsDecimal { get; set; }
        [DataMember]
        public int sortType { get; set; }
    }
}
