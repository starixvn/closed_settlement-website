using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Report
{
    [Serializable]
    [DataContract]
    public class TraderPerformanceDetail
    {
        [DataMember]
        public int CustId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string TransDate { get; set; }

        [DataMember]
        public string League { get; set; }
        [DataMember]
        public string Match { get; set; }
        [DataMember]
        public string HT { get; set; }
        [DataMember]
        public string FT { get; set; }

        #region TurnOver
        [DataMember]
        public string TurnOver1stHdp { get; set; }
        [DataMember]
        public string TurnOverHdp { get; set; }
        [DataMember]
        public string TurnOver1stOu { get; set; }
        [DataMember]
        public string TurnOverOu { get; set; }
        [DataMember]
        public string TurnOver1st1x2 { get; set; }
        [DataMember]
        public string TurnOver1x2 { get; set; }
        [DataMember]
        public string TurnOver1stOe { get; set; }
        [DataMember]
        public string TurnOverOe { get; set; }
        [DataMember]
        public string TurnOver1stNoOfCorners { get; set; }
        [DataMember]
        public string TurnOverNoOfCorners { get; set; }
        [DataMember]
        public string TurnOver15minOu { get; set; }
        [DataMember]
        public string TurnOver1stNoOfYellowCards { get; set; }
        [DataMember]
        public string TurnOverNoOfYellowCards { get; set; }
        [DataMember]
        public string TurnOverNextCorner { get; set; }
        [DataMember]
        public string TurnOverOverall { get; set; }
        #endregion

        #region WinLoss
        [DataMember]
        public string WinLost1stOu { get; set; }
        [DataMember]
        public string WinLostOu { get; set; }
        [DataMember]
        public string WinLost1stOe { get; set; }
        [DataMember]
        public string WinLostOe { get; set; }
        [DataMember]
        public string WinLost1stHdp { get; set; }
        [DataMember]
        public string WinLostHdp { get; set; }
        [DataMember]
        public string WinLost1st1x2 { get; set; }
        [DataMember]
        public string WinLost1x2 { get; set; }
        [DataMember]
        public string WinLost1stNoOfCorners { get; set; }
        [DataMember]
        public string WinLostNoOfCorners { get; set; }
        [DataMember]
        public string WinLost15minOu { get; set; }
        [DataMember]
        public string WinLost1stNoOfYellowCards { get; set; }
        [DataMember]
        public string WinLostNoOfYellowCards { get; set; }
        [DataMember]
        public string WinLostNextCorner { get; set; }
        [DataMember]
        public string WinLostOverall { get; set; }
        #endregion

        #region Comm
        [DataMember]
        public string Comm1stOu { get; set; }
        [DataMember]
        public string CommOu { get; set; }
        [DataMember]
        public string Comm1stOe { get; set; }
        [DataMember]
        public string CommOe { get; set; }
        [DataMember]
        public string Comm1stHdp { get; set; }
        [DataMember]
        public string CommHdp { get; set; }
        [DataMember]
        public string Comm1st1x2 { get; set; }
        [DataMember]
        public string Comm1x2 { get; set; }
        [DataMember]
        public string Comm1stNoOfCorners { get; set; }
        [DataMember]
        public string CommNoOfCorners { get; set; }
        [DataMember]
        public string Comm15minOu { get; set; }
        [DataMember]
        public string Comm1stNoOfYellowCards { get; set; }
        [DataMember]
        public string CommNoOfYellowCards { get; set; }
        [DataMember]
        public string CommNextCorner { get; set; }
        [DataMember]
        public string CommOverall { get; set; }
        #endregion

        #region Margin
        [DataMember]
        public string Margin1stOu { get; set; }
        [DataMember]
        public string MarginOu { get; set; }
        [DataMember]
        public string Margin1stOe { get; set; }
        [DataMember]
        public string MarginOe { get; set; }
        [DataMember]
        public string Margin1stHdp { get; set; }
        [DataMember]
        public string MarginHdp { get; set; }
        [DataMember]
        public string Margin1st1x2 { get; set; }
        [DataMember]
        public string Margin1x2 { get; set; }
        [DataMember]
        public string Margin1stNoOfCorners { get; set; }
        [DataMember]
        public string MarginNoOfCorners { get; set; }
        [DataMember]
        public string Margin15minOu { get; set; }
        [DataMember]
        public string Margin1stNoOfYellowCards { get; set; }
        [DataMember]
        public string MarginNoOfYellowCards { get; set; }
        [DataMember]
        public string MarginNextCorner { get; set; }
        [DataMember]
        public string MarginOverall { get; set; }
        #endregion

        #region Live Bet Count
        [DataMember]
        public string LiveBetCount1stOu { get; set; }
        [DataMember]
        public string LiveBetCountOu { get; set; }
        [DataMember]
        public string LiveBetCount1stOe { get; set; }
        [DataMember]
        public string LiveBetCountOe { get; set; }
        [DataMember]
        public string LiveBetCount1stHdp { get; set; }
        [DataMember]
        public string LiveBetCountHdp { get; set; }
        [DataMember]
        public string LiveBetCount1st1x2 { get; set; }
        [DataMember]
        public string LiveBetCount1x2 { get; set; }
        [DataMember]
        public string LiveBetCount1stNoOfCorners { get; set; }
        [DataMember]
        public string LiveBetCountNoOfCorners { get; set; }
        [DataMember]
        public string LiveBetCount15minOu { get; set; }
        [DataMember]
        public string LiveBetCount1stNoOfYellowCards { get; set; }
        [DataMember]
        public string LiveBetCountNoOfYellowCards { get; set; }
        [DataMember]
        public string LiveBetCountNextCorner { get; set; }
        [DataMember]
        public string LiveBetCountOverall { get; set; }
        #endregion
    }

    [Serializable]
    [DataContract]
    public class TraderPerformanceDetailBO
    {
        [DataMember]
        public TraderPerformanceDetail total { get; set; }

        [DataMember]
        public IEnumerable<TraderPerformanceDetail> children { get; set; }
    }

    [Serializable]
    [DataContract]
    public class TraderPerformanceDetailDTO
    {
        [DataMember]
        public IEnumerable<TraderPerformanceDetailBO> rows { get; set; }

        [DataMember]
        public TraderPerformanceDetail footer { get; set; }

        [DataMember]
        public int total { get; set; }
        [DataMember]
        public int pageindex { get; set; }
    }
}
