using System.Runtime.Serialization;
using DNAide.Core;
using Norm;
using System;

namespace Sunplus.Data.DataAbstract.Entity
{
    [Serializable]
    [DataContract]
    public class Bettran
    {
        #region public properties
        public string SystemParlaySelection_Bettran { get; set; }

        #region User
        
        public int Custid { get; set; }

        public int Creator { get; set; }

        public int Recommend { get; set; }

        public string Ip { get; set; }

        [DataMember]
        public string UserName { get; set; }
        #endregion

        #region Trans

        [DataMember]
        public string Refno { get; set; }

        [MongoIdentifier]
        public string Transid { get; set; }

        public string Transdate { get; set; }
               
        public string Transdesc { get; set; }
        #endregion

        #region Bettype

        public string Betfrom { get; set; }

        public string Betcheck { get; set; }
  
        public string Betid { get; set; }
      
        [DataMember]
        public int Bettype { get; set; }
        #endregion

        #region Match
        public int Matchid { get; set; }
        
        public string MatchCode { get; set; }
        
        public string Hdp1 { get; set; }

        public string Hdp2  { get; set; }
     
        public string Livehomescore { get; set; }

        public string Liveawayscore { get; set; }

        public string EventDate { get; set; }

        [DataMember]  
        public bool Liveindicator { get; set; }


        public string Betteam { get; set; }


        public string LeagueId { get; set; }

       
        public string HomeId { get; set; }

      
        public string AwayId { get; set; }


        public string SportId { get; set; }

        public bool IsNeutral
        {
            get;
            set;
        }


        #endregion

        #region Odds

        public int OddsId { get; set; }

        public string Odds { get; set; }

        public string OddsSpread { get; set; }

        public int OddsType { get; set; }

        public string Modds { get; set; }
        #endregion

        #region WinLost

        public string Stake{get; set;}

        public string actualStake { get; set; }

        public string Winlost{get; set; }

        public int Statuswinlost { get; set; }


        public string Winlostdate { get; set; }
     
       public int Currency { get; set; }

       [DataMember]
       public string CurrencyName { get; set; }

       public int Currency2 { get; set; }

        public string Virtualrate
        {
            get; set; }

        public string Virtual
        { get; set; }

        public string Real { get; set; }
        //isnumeric(odds.programID)
        //b.matchid=odds.orid  

        public bool IsShowOtExRate
        {
            get;
            set;
        }


        public int StatusId { get; set; }

        [DataMember]
        public string Status { get; set; }
        #endregion

        #region Super
        private string _sdiscount;

        public string Sdiscount
        {
            get { return (CastAide.AsDouble(_sdiscount, 0) * 100).ToString(); }
            set { _sdiscount = value; }
        }

        private string _scomm;

        public string Scomm
        {
            get { return CastAide.AsDecimal(_scomm).ToString(); }
            set { _scomm = value; }
        }

        private string _spositiontaking;

        public string Spositiontaking
        {
            get { return (CastAide.AsDecimal(_spositiontaking) * 100).ToString(); }
            set { _spositiontaking = value; }
        }

        private string _swinlost;

        public string Swinlost
        {
            get { return CastAide.AsDecimal(_swinlost).ToString(); }
            set { _swinlost = value; }
        }


        public int Srecommend { get; set; }
        #endregion

        #region Master

        private string _acomm;

        public string Acomm
        {
            get { return CastAide.AsDecimal(_acomm).ToString(); }
            set { _acomm = value; }
        }

        private string _adiscount;

        public string Adiscount
        {
            get { return (CastAide.AsDecimal(_adiscount)).ToString(); }
            set { _adiscount = value; }
        }

        private string _mpositiontaking;

        public string Mpositiontaking
        {
            get { return (CastAide.AsDecimal(_mpositiontaking) * 100).ToString(); }
            set { _mpositiontaking = value; }
        }

        private string _mwinlost;

        public string Mwinlost
        {
            get { return CastAide.AsDecimal(_mwinlost).ToString(); }
            set { _mwinlost = value; }
        }


        public int Mrecommend { get; set; }
        #endregion

        #region Agent
        private string _awinlost;
        public string Awinlost
        {
            get { return CastAide.AsDecimal(_awinlost).ToString(); }
            set { _awinlost = value; }
        }
     
        public string Discount { get; set; }

        public string Comm { get; set; }

        private string _apositiontaking;
        public string Apositiontaking
        {
            get { return (CastAide.AsDecimal(this._apositiontaking) * 100).ToString(); }
            set { _apositiontaking = value; }
        }
        #endregion

        #region Member

        public string Playercomm { get; set;}

        public string Playerdiscount { get; set; }

        public string PlayerWinloss { get; set; }
       
        #endregion
            public string CompanyWinloss { get; set; }
        #region Company

        #endregion

        #region Other

        public string Tpositiontaking { get; set; }

        public string Comstatus { get; set; }

        public string Checktime { get; set; }

        public int Ruben { get; set; }

        #endregion

        #region SA Ticket
        [DataMember]
        public int SATicket { get; set; }
        #endregion

        #endregion public properties

    }
}
