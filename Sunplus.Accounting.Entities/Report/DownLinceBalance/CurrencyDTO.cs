using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.DownLinceBalance
{
    [Serializable]
    [DataContract]
    public class CurrencyDTO
    {
        [DataMember]
        public List<Currency> Currencies { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Currency
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
}
