using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Racebook
{
    [Serializable]
    [DataContract]
    public class RacingDailyWinLossRow
    {
        [DataMember]
        public string Meeting { get; set; }

        [DataMember]
        public string MeetingId { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public string CurrencyId { get; set; }

        [DataMember]
        public string Race { get; set; }

        [DataMember]
        public string AgWinTO { get; set; }

        [DataMember]
        public string AgPlaceTO { get; set; }

        [DataMember]
        public string AgWPTO { get; set; }

        [DataMember]
        public string AgTotal { get; set; }

        [DataMember]
        public string AgCommIncome { get; set; }

        [DataMember]
        public string AgPTWL{ get; set; }


        [DataMember]
        public string IBCWinTO { get; set; }

        [DataMember]
        public string IBCPlaceTO { get; set; }

        [DataMember]
        public string IBCWPTO { get; set; }


        [DataMember]
        public string IBCTotal { get; set; }

        [DataMember]
        public string IBCCommIncome { get; set; }

        [DataMember]
        public string IBCCommExpense { get; set; }

        [DataMember]
        public string IBCAmountFee { get; set; }

        [DataMember]
        public string IBCPT { get; set; }

        [DataMember]
        public string IBCNetIncome { get; set; }

        [DataMember]
        public string PhuWinTO { get; set; }

        [DataMember]
        public string PhuPlaceTO { get; set; }

        [DataMember]
        public string PhuWPTO { get; set; }

        [DataMember]
        public string PhuTotal { get; set; }

        [DataMember]
        public string PhuDividendWon { get; set; }

        [DataMember]
        public string PhuAmountDue{ get; set; }

        [DataMember]
        public string PhuCommExpense { get; set; }

        [DataMember]
        public string PhuAmountFee{ get; set; }
    }

    [Serializable]
    [DataContract]
    public class RacingDailyWinLossByMeetingCurrency
    {
        [DataMember]
        public List<RacingDailyWinLossRow> Items { get; set; }

        [DataMember]
        public List<RacingDailyWinLossRow> Totals { get; set; }


    }

    [Serializable]
    [DataContract]
    public class RacingDailyWinLossByCurrency
    {
        [DataMember]
        public List<RacingDailyWinLossByMeetingCurrency> Items { get; set; }

        [DataMember]
        public List<RacingDailyWinLossRow> Totals { get; set; }

        [DataMember]
        public List<RacingDailyWinLossRow> GroupTotals { get; set; }

    }

    [Serializable]
    [DataContract]
    public class RacingDailyWinLossDTO
    {
        [DataMember]
        public List<RacingDailyWinLossByCurrency> Items { get; set; }

        [DataMember]
        public List<RacingDailyWinLossRow> Totals { get; set; }
    }
}
