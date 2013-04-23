using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sunplus.Accounting.Entities.LogDisplay.SettingPT;

namespace Sunplus.Accounting.Entities.LogDisplay
{
    public enum ELogType { 
        Setting = 1,
        Status = 2,
        Credit = 3,
        Login = 4
    } 

    public interface IResultLog
    {
        int Total { get; set; }
        List<IRecordLog> Rows { get; set; }
    }

    public interface IRecordLog
    {
        int ID { get; set; } // log code
        string Date { get; set; } // log date
        string UEdit { get; set; } // edit by
        int UEId { get; set; } // edit by
        string UName { get; set; } // username
        int UNId { get; set; } // username
        int Aid { get; set; } // action ID
        string Act { get; set; } // action Name
    }

    [Serializable]
    [DataContract]
    public class LogParam
    {
        //int custId, string custName, DateTime fromDate, DateTime toDate, int viewBy, int page, int pageSize;
        public int Type { get; set; }
        public int CustId { get; set; }
        public string CustName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int ViewBy { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
    [Serializable]
    [DataContract]
    public class ResultLog
    {
        [DataMember]
        public int Total { get; set; }
        [DataMember]
        public int Type { get; set; }
        [DataMember]
        public List<RecordSp> Setting { get; set; }
        [DataMember]
        public List<RecordSp> Status { get; set; }
        [DataMember]
        public List<RecordCredit> Credit { get; set; }
        [DataMember]
        public List<RecordLogin> Login { get; set; }
    }

    [Serializable]
    [DataContract]
    public class RecordLog : IRecordLog
    {
        [DataMember]
        public int ID { get; set; } // log code
        [DataMember]
        public string Date { get; set; } // log date
        [DataMember]
        public string UEdit { get; set; } // edit by
        [DataMember]
        public int UEId { get; set; } // edit by
        [DataMember]
        public string UName { get; set; } // username
        [DataMember]
        public int UNId { get; set; } // username
        [DataMember]
        public int Aid { get; set; } // action ID
        [DataMember]
        public string Act { get; set; } // action Name
    }
    #region log setting & status
    [Serializable]
    [DataContract]
    public class ResultSp
    {
        [DataMember]
        public int Total { get; set; }
        [DataMember]
        public List<RecordSp> Rows { get; set; }
    }
    [Serializable]
    [DataContract]
    public class RecordSp : RecordLog
    {
        [DataMember]
        public TrackingLog Detail { get; set; } // log detail*/
        [DataMember]
        public List<TrackingValue> Diffs { get; set; }
        [DataMember]
        public TrackingLog Old { get; set; } // log detail*/
    }
    #endregion log setting & status
    #region log credit
    [Serializable]
    [DataContract]
    public class ResultCredit
    {
        [DataMember]
        public int Total { get; set; }
        [DataMember]
        public List<RecordCredit> Rows { get; set; }
    }
    [Serializable]
    [DataContract]
    public class RecordCredit : RecordLog
    {
        [DataMember]
        public string SP { get; set; } // sp Name
        [DataMember]
        public string OldVal { get; set; } // log old value
        [DataMember]
        public string NewVal { get; set; } // log new value
        [DataMember]
        public string IP { get; set; } // log ip
        [DataMember]
        public string Detail { get; set; } // log detail
    }
    #endregion log credit

    #region log login
    [Serializable]
    [DataContract]
    public class ResultLogin
    {
        [DataMember]
        public int Total { get; set; }
        [DataMember]
        public List<RecordLogin> Rows { get; set; }
    }
    [Serializable]
    [DataContract]
    public class RecordLogin : RecordLog
    {
        [DataMember]
        public string SP { get; set; } // sp Name
        [DataMember]
        public string IP { get; set; } // log ip
        [DataMember]
        public string Desc { get; set; } // log desc
        [DataMember]
        public string Domain { get; set; } // log domain
        [DataMember]
        public string Browser { get; set; }// log broswer
        [DataMember]
        public string Country { get; set; }// log country
    }
    #endregion log login
}
