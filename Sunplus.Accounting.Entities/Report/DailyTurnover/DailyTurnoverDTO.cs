using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report.DailyTurnover
{
    [Serializable]
    [DataContract]
    public class DailyTurnoverDTO
    {
        [DataMember]
        public string StrDate { get; set; }

        [DataMember]
        public string Turnover { get; set; }

        [DataMember]
        public string STurnover { get; set; }

        [DataMember]
        public string MTurnover { get; set; }

        [DataMember]
        public string ATurnover { get; set; }

        [DataMember]
        public string IBCTurnover { get; set; }

        [DataMember]
        public string CNT { get; set; }

        [DataMember]
        public string TypeName { get; set; }
    }

    [Serializable]
    [DataContract]
    public class DetailTurnoverDTO
    {
        [DataMember]
        public string StrDateTime { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public List<DailyTurnoverDTO> TurnoverChanged { get; set; }

        [DataMember]
        public string CNT { get; set; }
    }


    [Serializable]
    [DataContract]
    public class DetailTurnoverActionDTO
    {
        [DataMember]
        public string StrDateTime { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string ActionType { get; set; }

        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public List<DailyTurnoverDTO> TurnoverChanged { get; set; }

        [DataMember]
        public string Turnover { get; set; }

        [DataMember]
        public string STurnover { get; set; }

        [DataMember]
        public string MTurnover { get; set; }

        [DataMember]
        public string ATurnover { get; set; }

        [DataMember]
        public string IBCTurnover { get; set; }

        [DataMember]
        public string CNT { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ActionDetail
    {

        [DataMember]
        public string Detail { get; set; }

        [DataMember]
        public string TurnoverActual { get; set; }

        [DataMember]
        public string TurnoverVirtual { get; set; }

        [DataMember]
        public string STurnoverActual { get; set; }

        [DataMember]
        public string STurnoverVirtual { get; set; }

        [DataMember]
        public string MTurnoverActual { get; set; }

        [DataMember]
        public string MTurnoverVirtual { get; set; }

        [DataMember]
        public string ATurnoverActual { get; set; }

        [DataMember]
        public string ATurnoverVirtual { get; set; }

        [DataMember]
        public string IBCTurnoverActual { get; set; }

        [DataMember]
        public string IBCTurnoverVirtual { get; set; }

        [DataMember]
        public string TicketCNT { get; set; }

        [DataMember]
        public string NormalCNT { get; set; }

        [DataMember]
        public string RejectCNT { get; set; }

    }

    [Serializable]
    [DataContract]
    public class ActionTurnover
    {
        [DataMember]
        public string StrDateTime { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string ActionName { get; set; }


        [DataMember]
        public ActionDetail ActionDetail { get; set; }

    }

}
