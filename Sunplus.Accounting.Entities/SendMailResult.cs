using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities
{
    [Serializable]
    [DataContract]
    public class SendMailToReciverResult
    {
        [DataMember]
        public string Reciver { get; set; }
        [DataMember]
        public bool IsSuccess { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }

    [Serializable]
    [DataContract]
    public class SendMailResult
    {
        [DataMember]
        public bool IsFail { get; set; }
        [DataMember]
        public List<SendMailToReciverResult> Recivers { get; set; }
    }

    [Serializable]
    [DataContract]
    public class MailFormat
    {
        [DataMember]
        public string Reciver { get; set; }
        [DataMember]
        public string To { get; set; }
        [DataMember]
        public string Cc { get; set; }
        [DataMember]
        public string Bcc { get; set; }
    }

    [Serializable]
    [DataContract]
    public class Mails
    {
        [DataMember]
        public List<MailFormat> Recivers { get; set; }
    }
}
