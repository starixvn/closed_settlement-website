using System;
using System.Runtime.Serialization;
using DNAide.Utils;

namespace Sunplus.Accounting.Entities.Customer
{
    /// <summary>
    /// Summary description for CustDetail
    /// </summary>

    [Serializable]

    [DataContract]
    public class CustDetail
    {
        private string _firstName;
        private string _lastName;

        [DataMember]
        public int CustId { get; set; }
        [DataMember]
        public int RoleId { get; set; }
        [DataMember]
        public string UserName { get; set; }

        public string Password { get; set; }
        [DataMember]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = Encryption.RemoveSpecialChars(value); }
        }
        [DataMember]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = Encryption.RemoveSpecialChars(value); }
        }
        [DataMember]
        public string Phone { get; set; }

        [DataMember]
        public string Site { get; set; }
        [DataMember]
        public string MobilePhone { get; set; }

        public string MaxCredit { get; set; }
        
        [DataMember]
        public string CurrencyName { get; set; }

        public bool InternalUser { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string NickName { get; set; }
        public string Email { get; set; }

        [DataMember]
        public int CurrencyId { get; set; }
        public int SuperId { get; set; }
        public int MasterId { get; set; }
        public int AgentId { get; set; }

        public int UpLineId { get; set; }


        public string Address { get; set; }
        public string Postal { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string BirthDay { get; set; }
        public string Remark { get; set; }
        public bool Closed { get; set; }
        public bool Suspend { get; set; }
        [DataMember]
        public string LastLoginIp { get; set; }
        public bool IsUplineSyncCasino { get; set; }
        public bool IsUplineDisableCasino { get; set; }
        public bool IsUplineSyncBingo { get; set; }
        public bool IsUplineSyncLiveCasino { get; set; }
        public bool IsUplineDisableLiveCasino { get; set; }
    }
}
