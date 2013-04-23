using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Monitor
{
    [Serializable]
    [DataContract]
    public class Filter
    {
        [DataMember]
        public string FromDate { get; set; }
        
        [DataMember]
        public string ToDate { get; set; }

        [DataMember]
        public bool IsHistory { get; set; }

        [DataMember]
        public List<string> InYears { get; set; }

        [DataMember]
        public int InMonth { get; set; }

        [DataMember]
        public int FromDay { get; set; }

        [DataMember]
        public int ToDay { get; set; }

        [DataMember]
        public List<Items> Currency { get; set; }

        [DataMember]
        public List<Items> Product { get; set; }

        [DataMember]
        public List<Items> Sport { get; set; }

        [DataMember]
        public List<Items> BetType { get; set; }

        [DataMember]
        public List<string> LiveStatus { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public List <Items> Leagues{ get; set; }

        [DataMember]
        public List<Items> ReportBy { get; set; }

        [DataMember]
        public string BeforeBalanceUpDay { get; set; }

        
    }

    [Serializable]
    [DataContract]
    public class Items
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name{ get; set; }

        [DataMember]
        public string Key { get; set; }

        [DataMember]
        public string Value { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Param
    {
        [DataMember]
        public string fromdate { get; set; }

        [DataMember]
        public string todate { get; set; }

        [DataMember]
        public int currency { get; set; }

        [DataMember]
        public List<Items> product { get; set; }

        [DataMember]
        public int sporttype { get; set; }

        [DataMember]
        public int bettype { get; set; }

        [DataMember]
        public int live { get; set; }

        [DataMember]
        public string usernames { get; set; }

        [DataMember]
        public string leagues { get; set; }

        [DataMember]
        public int reportby { get; set; }

        [DataMember]
        public int pageSize { get; set; }

        [DataMember]
        public int pageIndex { get; set; }

        [DataMember]
        public string Turnover1 { get; set; }

        [DataMember]
        public string Turnover2 { get; set; }

        [DataMember]
        public string Winloss1 { get; set; }

        [DataMember]
        public string Winloss2 { get; set; }
    }

}
