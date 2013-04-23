using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sunplus.Accounting.Entities.Customer.PT.ViewPT
{
    [DataContract]
    public class CustomerInfo
    {
        [DataMember]
        public int CustId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public int RoleId { get; set; }
        [DataMember]
        public string RoleName { get; set; }
    }

    [DataContract]
    public class CustomerRelation
    {
        [DataMember]
        public CustomerInfo Customer { get; set; }
        [DataMember]
        public List<CustomerInfo> Upline { get; set; }
    }

    [DataContract]
    public class IbcAutoPtForRacebook
    {
        [DataMember]
        public int ToteId { get; set; }
        [DataMember]
        public string ToteName { get; set; }
        [DataMember]
        public string IbcAutoPt { get; set; }
        [DataMember]
        public ActualIbcPtOnMember ActualIbcPtOnMember { get; set; }
    }

    [DataContract]
    public  class ActualIbcPtOnMember
    {
        [DataMember]
        public string Win { get; set; }
        [DataMember]
        public string Place { get; set; }
        [DataMember]
        public string EWin { get; set; }
        [DataMember]
        public string EPlace { get; set; }
    }

    [DataContract]
    public class RacebookIbcPt
    {
        [DataMember]
        public List<IbcAutoPtForRacebook> IbcPtForRacebook { get; set; }
    }

    [DataContract]
    public class ViewPtDTO
    {
        [DataMember]
        public CustomerRelation CustomerInfo { get; set; }
        [DataMember]
        public bool IsCasino { get; set; }
        [DataMember]
        public bool IsLiveCasino { get; set; }
        [DataMember]
        public RacebookIbcPt IBCPt { get; set; }
        [DataMember]
        public List<PositionTakingDTO> PotionTakingDetail { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }

    [DataContract]
    public class PositionTakingInfo
    {
        [DataMember]
        public string Super { get; set; }
        [DataMember]
        public string Master { get; set; }
        [DataMember]
        public string Agent { get; set; }
        [DataMember]
        public string SuperAuto { get; set; }
        [DataMember]
        public string SuperIsAuto { get; set; }
        [DataMember]
        public string MasterAuto { get; set; }
        [DataMember]
        public string MasterIsAuto { get; set; }
        [DataMember]
        public string MinPt { get; set; }
        [DataMember]
        public string MaxPt { get; set; }
        [DataMember]
        public string Com { get; set; }
    }

    [DataContract]
    public class AllowView
    {
        [DataMember]
        public AllowViewSport OnexTwo { get; set; }
        [DataMember]
        public AllowViewSport HorseRacing { get; set; }
        [DataMember]
        public AllowViewSport Greyhound { get; set; }
        [DataMember]
        public AllowViewSport Harness { get; set; }
        [DataMember]
        public AllowViewSport Finance { get; set; }
        [DataMember]
        public AllowViewSport GameComm { get; set; }
    }

    [Serializable]
    [DataContract]
    public class AllowViewSport
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool Status { get; set; }
        [DataMember]
        public string Comm { get; set; }
        [DataMember]
        public List<AllowViewSport> AllowViewTotes { get; set; }
    }

    [DataContract]
    public class PositionTakingDTO
    {
        [DataMember]
        public string SportName { get; set; }
        [DataMember]
        public string TypeName { get; set; }
        [DataMember]
        public string SportType { get; set; }
        [DataMember]
        public string Bettype { get; set; }
        [DataMember]
        public string LiveGame { get; set; }
        [DataMember]
        public bool EnableAutoMaster { get; set; }
        [DataMember]
        public bool PermistionEnableAutoMaster { get; set; }
        [DataMember]
        public string DisableFlagCss { get; set; }
        [DataMember]
        public string DisableAutoCss { get; set; }
        [DataMember]
        public PositionTakingInfo Super { get; set; }
        [DataMember]
        public PositionTakingInfo Master { get; set; }
        [DataMember]
        public PositionTakingInfo Agent { get; set; }
        [DataMember]
        public PositionTakingInfo Member { get; set; }
    }

    [DataContract]
    public class Commission
    {
        [DataMember]
        public string RoleName { get; set; }

        [DataMember]
        public AllowViewSport GroupA { get; set; }
        //[DataMember]
        //public string GroupA { get; set; }
        [DataMember]
        public AllowViewSport GroupB { get; set; }
        //[DataMember]
        //public string GroupB { get; set; }
        [DataMember]
        public AllowViewSport GroupC { get; set; }
        //[DataMember]
        //public string GroupC { get; set; }
        [DataMember]
        public AllowViewSport GroupD { get; set; }
        //[DataMember]
        //public string GroupD { get; set; }
        [DataMember]
        public AllowViewSport Other { get; set; }
        //[DataMember]
        //public string Other { get; set; }
        [DataMember]
        public AllowViewSport OneXTwo { get; set; }
        //[DataMember]
        //public string OneXTwo { get; set; }
        [DataMember]
        public AllowViewSport HorseRacing { get; set; }
        //[DataMember]
        //public string RacingSa { get; set; }
        //[DataMember]
        //public AllowViewTote RacingSa { get; set; }
        //[DataMember]
        //public string RacingUk { get; set; }
        //[DataMember]
        //public string RacingAu { get; set; }
        [DataMember]
        public AllowViewSport Greyhound { get; set; }
        //[DataMember]
        //public string GreyhoundUk { get; set; }
        //[DataMember]
        //public string GreyhoundAu { get; set; }
        [DataMember]
        public AllowViewSport Harness { get; set; }
        //[DataMember]
        //public string HarnessAu { get; set; }
        [DataMember]
        public AllowViewSport FiComm { get; set; }
        //[DataMember]
        //public string FiComm { get; set; }
        [DataMember]
        public AllowViewSport P2PGameComm { get; set; }
        [DataMember]
        public AllowViewSport P2PPrivateGameComm { get; set; }
        [DataMember]
        public AllowViewSport GameComm { get; set; }
        //[DataMember]
        //public string GameComm { get; set; }
        [DataMember]
        public AllowViewSport LiveCasinoComm { get; set; }

    }

    [DataContract]
    public class ViewCommissionDTO
    {
        [DataMember]
        public CustomerRelation CustomerInfo { get; set; }
        [DataMember]
        public List<Commission> Comm { get; set; }
        [DataMember]
        public AllowView AllowViewSport { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
