using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.DownLinceBalance
{
    [Serializable]
    [DataContract]
    public class DownLineBalanceDTO
    {
        [DataMember]
        public List<DownLineBalaneRow> DownLineBalane { get; set; }
        [DataMember]
        public DownLineBalaneRow Total { get; set; }
        [DataMember]
        public DownLineBalaneRow BaseTotal { get; set; }
    }

    [Serializable]
    [DataContract]
    public class DownLineBalaneRow
    {
        [DataMember]
        public string CustId { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string RoleId { get; set; }
        [DataMember]
        public string CurrencyId { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string Balance { get; set; }
        [DataMember]
        public string BaseBalance { get; set; }
    }

    [Serializable]
    [DataContract]
    public class DifferenceBalanceInfo
    {
        [DataMember]
        public string DiffDate { get; set; }
        [DataMember]
        public string DiffBalance { get; set; }

    }
}
