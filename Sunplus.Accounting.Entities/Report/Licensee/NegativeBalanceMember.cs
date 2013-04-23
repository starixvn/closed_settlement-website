using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Licensee
{
    [Serializable]
    [DataContract]
    public class NegativeBalanceMemberRow
    {
        
        public string Licensee { get; set; }

        
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string Balance { get; set; }
    }

    
    [Serializable]
    [DataContract]
    public class NegativeBalanceMemberByLicensee
    {
        [DataMember]
        public List<NegativeBalanceMemberRow> Items { get; set; }

        [DataMember]
        public NegativeBalanceMemberRow Total { get; set; }
    }

    [Serializable]
    [DataContract]
    public class NegativeBalanceMemberDTO
    {
        [DataMember]
        public List<NegativeBalanceMemberByLicensee> Items { get; set; }
        
        [DataMember]
        public List<NegativeBalanceMemberMailFormat> MailList { get; set; }

        [DataMember]
        public NegativeBalanceMemberRow Total { get; set; }

        [DataMember]
        public string totalitem { get; set; }

        public IEnumerator<object> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }


    [Serializable]
    [DataContract]
    public class NegativeBalanceMemberMailFormat 
    {
        [DataMember]
        public string Site { get; set; }
        [DataMember]
        public string To { get; set; }
        [DataMember]
        public string Cc { get; set; }
        [DataMember]
        public string Bcc { get; set; }
    }

    [Serializable]
    [DataContract]
    public class NegativeBalanceMemberSendMailResult
    {
        [DataMember]
        public string Site { get; set; }
        [DataMember]
        public bool IsSuccess { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }

    [Serializable]
    [DataContract]
    public class NegativeBalanceMemberSendMailFail
    {
        [DataMember]
        public bool IsFail { get; set; }
        [DataMember]
        public List<NegativeBalanceMemberSendMailResult> Licensees { get; set; }
    }
}
