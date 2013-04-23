using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.ViewModel
{
    /// <summary>
    /// Summary description for DailyTransaction
    /// </summary>
    /// 

    [Serializable]
    [DataContract]
    public class DailyTransaction
    {
        [DataMember]
        private string _checkerName;

        [DataMember]
        public int RowIndex { get; set; }

        [DataMember]
        public string TransId { get; set; }

        [DataMember]
        public string RefNo { get; set; }

        [DataMember]
        public string transDate { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public double Amount { get; set; }

        [DataMember]
        public string Creator { get; set; }

        [DataMember]
        public bool IsVerified { get; set; }

        [DataMember]
        public string CheckerName
        {
            get
            {
                //by default data return CheckerName 0 for null checkerid 
                //I do not know why 
                return (_checkerName == "0") ? string.Empty : _checkerName;
            }
            set { _checkerName = value; }
        }

        [DataMember]
        public int CheckerId { get; set; }

        [DataMember]
        public string CheckedTime { get; set; }
    }

    [Serializable]
    [DataContract]
    public class DailyTransactionDTO
    {
        [DataMember]
        public List<DailyTransaction> items { get; set; }

        [DataMember]
        public int totalRows { get; set; }


    }
}