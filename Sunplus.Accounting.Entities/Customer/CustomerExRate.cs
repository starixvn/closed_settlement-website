using System;
using System.Web;
using DNAide.Core;
using DNAide.Utils;
using Sunplus.Accounting.Entities.Casino;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Customer
{
    /// <summary>
    /// Summary description for CustDetail
    /// </summary>

    [Serializable]
    [DataContract]
    public class CustomerExRate
    {
        [DataMember]
        public int custid;

        [DataMember]
        public string username;

        [DataMember]
        public string currency;

        [DataMember]
        public double actualrate;
    }
}
