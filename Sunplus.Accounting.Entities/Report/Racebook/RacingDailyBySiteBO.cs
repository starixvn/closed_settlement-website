using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sunplus.Accounting.Entities.Report.Racebook
{
    #region WinlostRacingLicensee_Details
    #region RacingDailyBySiteItem
    [Serializable]
    [DataContract]
    public class RacingDailyBySiteItem
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool HasPt { get; set; }
    }
    #endregion

    #region RacingDailyBySiteRow
    [Serializable]
    [DataContract]
    public class RacingDailyBySiteRow
    {
        [DataMember]
        public int CurrencyId { get; set; }

        [DataMember]
        public string CurrencyName { get; set; }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Race { get; set; }

        [DataMember]
        public string WinTO { get; set; }

        [DataMember]
        public string PlaceTO { get; set; }

        [DataMember]
        public string WinPlaceTO { get; set; }

        [DataMember]
        public string TotalTO { get; set; }


        #region Agent WL
        [DataMember]
        public string AgentWinTO { get; set; }

        [DataMember]
        public string AgentPlaceTO { get; set; }

        [DataMember]
        public string AgentWinPlaceTO { get; set; }

        [DataMember]
        public string AgentTotalTO { get; set; }

        [DataMember]
        public string AgentPtWinLoss{ get; set; }
        #endregion

        [DataMember]
        public string IBCPTWL { get; set; }

        [DataMember]
        public string IBCCommIncomm { get; set; }

        [DataMember]
        public string DividendWon { get; set; }

        [DataMember]
        public string CommIncome { get; set; }

        [DataMember]
        public string Amountdueto  { get; set; }
    }
    #endregion

    #region RacingDailyBySiteMeeting
    [Serializable]
    [DataContract]
    public class RacingDailyBySiteMeeting
    {
        [DataMember]
        public List<RacingDailyBySiteRow> Items { get; set; }

        [DataMember]
        public RacingDailyBySiteRow Total { get; set; }
    }
    #endregion

    #region RacingDailyBySiteCurrency
    [Serializable]
    [DataContract]
    public class RacingDailyBySiteCurrency
    {
        [DataMember]
        public List<RacingDailyBySiteMeeting> Items { get; set; }

        [DataMember]
        public List<RacingDailyBySiteRow> Totals { get; set; }
    }
    #endregion

    #region RacingDailyBySiteBO
    [Serializable]
    [DataContract]
    public class RacingDailyBySiteBO
    {
        [DataMember]
        public List<RacingDailyBySiteItem> SiteItems { get; set; }

        [DataMember]
        public List<RacingDailyBySiteCurrency> Items { get; set; }

        [DataMember]
        public RacingDailyBySiteRow Total { get; set; }
        
    }
    #endregion
    #endregion

    #region WinlostRacingLicensee_BetList_Details

    #region RacingDailyBySiteBetlistRow
    [Serializable]
    [DataContract]
    public class RacingDailyBySiteBetlistRow
    {
        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public string Member { get; set; }

        [DataMember]
        public string UserName2 { get; set; }

        [DataMember]
        public string TranTime { get; set; }

        [DataMember]
        public string HorseBettype { get; set; }

        [DataMember]
        public string Odds { get; set; }

        [DataMember]
        public string WinOdds { get; set; }

        [DataMember]
        public string PlaceOdds { get; set; }

        [DataMember]
        public string Stake { get; set; }

        [DataMember]
        public string ActualRate { get; set; }

        [DataMember]
        public string ExRate2 { get; set; }

        [DataMember]
        public string Winloss { get; set; }

        [DataMember]
        public string Comm { get; set; }

        [DataMember]
        public string AWinloss { get; set; }

        [DataMember]
        public string AComm { get; set; }

        [DataMember]
        public string PTWLLicensee { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string WinStatus { get; set; }

        [DataMember]
        public string PlaceStatus { get; set; }

        [DataMember]
        public string Dividend { get; set; }

        [DataMember]
        public bool IsShowOtExRate { get; set; }

        [DataMember]
        public string TransId { get; set; }

        [DataMember]
        public string Bettype { get; set; }

        [DataMember]
        public bool IsCombineBettype { get; set; }

        [DataMember]
        public string IBCPTWL { get; set; }

        [DataMember]
        public string IBCCommIncomm { get; set; }
    }
    #endregion

    #region RacingDailyBySiteBetlistBO
    [Serializable]
    [DataContract]
    public class RacingDailyBySiteBetlistBO
    {
        [DataMember]
        public List<RacingDailyBySiteBetlistRow> Items { get; set; }

        [DataMember]
        public RacingDailyBySiteBetlistRow Total { get; set; }
    }
    #endregion
    #endregion
}
