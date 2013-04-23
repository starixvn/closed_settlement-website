using System;
using System.Runtime.Serialization;
using DNAide.Core;
using Norm.Attributes;
using Sunplus.Data.DataAbstract.Utils;
using System.Collections.Generic;
using DNAide.Utils;

namespace Sunplus.Data.DataAbstract.Entity
{
    [Serializable]
    [DataContract]
    public class BABetList
    {
        [DataMember]
        public List<BetListDetail> Items { get; set; }

        [DataMember]
        public int totalItem{get;set;}
    }

    [Serializable]
    [DataContract]
    public class BetListDetail
    {
        [DataMember]
        public string Transid { get; set; }

        [DataMember]
        public string Serial { get; set; }

        #region TransDate

        private string _code;
        [MongoIgnore, DataMember]
        public string Code
        {
            get
            {
                if (_bettran.Bettype == 71 || _bettran.Bettype == 72 || _bettran.Bettype == 73 || _bettran.Bettype == 113 || _bettran.Bettype == 114 || _bettran.Bettype == 112)
                {
                    return _bettran.MatchCode.ToString();
                }
                return _bettran.Transid;
            }
            set { _code = value; }
        }

        private string _username;
        [MongoIgnore, DataMember]
        public string Username
        {
            get
            {
                return CastAide.AsString(_bettran.UserName);
            }
            set { _username = value; }
        }
        private string _transDate;
        [MongoIgnore, DataMember]
        public string TransDate
        {
            get
            {
                DateTime transDate = CastAide.AsDateTime(_bettran.Transdate);
                return transDate.ToShortDateString() + "&nbsp;" +
                        transDate.ToShortTimeString();

            }
            set { _transDate = value; }
        }

        private string _transShortDate;
        [MongoIgnore, DataMember]
        public string TransShortDate
        {
            get {
                DateTime transDate = CastAide.AsDateTime(_bettran.Transdate);
                return transDate.ToShortDateString();
            }
            set { _transShortDate = value; }
        }

        #endregion
       
        #region Choice

        private SportEventDTO _sportEventDTO;

        [DataMember]
        public virtual SportEventDTO SportEvent
        {
            get
            {
                var sportEvent = new SportEvent(_bettran);
                return sportEvent.GetSportEvent();
            }
            set { _sportEventDTO = value; }
        }


        #endregion

        #region Combination Detail

        [DataMember]
        public List<CombinationDetalDTO> CombinationDetails { get; set; }

        #endregion

        #region Odds
            private string _odds;
            [MongoIgnore, DataMember]
            public string Odds
            {
                get
                {
                    _odds = _bettran.Odds;
                    var result = string.Empty;
                    if (_bettran.Bettype == 9)
                    {
                        result = FormatHandler.FormatNumber4(CastAide.AsDecimal(_odds),"en-EN");
                        return result;
                    }
                    if (_bettran.Bettype == 71 || _bettran.Bettype == 72 || _bettran.Bettype == 73 || _bettran.Bettype == 113 || _bettran.Bettype == 114 || _bettran.Bettype == 112)
                    {
                        return "";
                    }
                    //Lack case of HR bettype
                    result = FormatHandler.FormatNumber3(CastAide.AsDecimal(_odds),"en-EN");

                    return result;
                }
                set { _odds = value; }
            }


            [MongoIgnore]
            public string OddType { get; set; }
            
            private string _oddName;
            [MongoIgnore, DataMember]
            public string OddName
            {
                get
                {
                    return SportBookUtils.GetOddName(_bettran.OddsType.ToString());
                }
                set { _oddName = value; }
            }   
          
        #endregion

        #region mOdds
            private string _modds;
            [MongoIgnore, DataMember]
            public string Modds
            { 
                get{
                    return FormatHandler.FormatNumber4(CastAide.AsDecimal(_bettran.Modds), "en-EN");
                }
                set { _modds = value; }
                
            }
            #endregion

        #region Stake
            private string _stake;
            [MongoIgnore, DataMember]
            public string Stake
            {
                get { return FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.Stake),"en-EN"); }
                set { _stake = value; }
            }
        #endregion

        #region Actual Stake
            private string _actualStake;
            [MongoIgnore, DataMember]
            public string actualStake
            {
                get { return FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.actualStake), "en-EN"); }
                set { _actualStake = value; }
            }
        #endregion

            #region Vitual
            private string _virtualrate;
            [MongoIgnore, DataMember]
            public string Virtualrate
            {
                get {
                    var virtualrate = "";
                    if(_bettran.Status != "running")
                        virtualrate = FormatHandler.FormatNumber2(CastAide.AsDouble(_bettran.Virtualrate, 0), "en-EN");
                    return virtualrate;
                }
                set { _virtualrate = value; }
            }

            private string _virtual;
            [MongoIgnore, DataMember]
            public string Virtual
            {
                get {
                    var virtualVal = "";
                    if(_bettran.Status != "running")
                        virtualVal =  FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.Virtual), "en-EN");
                    return virtualVal;
                }
                set { _virtual = value; }
            }
            #endregion


            #region Real
            private string _real;
            [MongoIgnore, DataMember]
            public string Real
            {
                get {
                    var real = "";
                    if(_bettran.Status != "running")
                        real = FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.Real), "en-EN");
                    return real;
                }
                set { _real = value; }
            }

            #endregion

            #region Status
            //Status get from BettranDetail
            private string _ip;
            [MongoIgnore, DataMember]
            public string Ip
            {
                get { return _bettran.Ip; } //View IPVN
                set { _ip = value; }
            }

            private int _statusId;
            [MongoIgnore, DataMember]
            public int statusId
            {
                get { return _bettran.StatusId; }
                set { _statusId = value; }
            }
        #endregion

        #region Comm Winloss


          
            private string _playerwinlost;
            [MongoIgnore, DataMember]
            public string PlayerWinloss
            {

                get { return FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.PlayerWinloss),"en-EN"); }
                set { _playerwinlost = value; }
            }

          


        private string _agentDiscount;
        [MongoIgnore, DataMember]
        public string AgentDiscount
        {
            get { return FormatHandler.FormatNumber2(CastAide.AsDouble(_bettran.Adiscount, 0) * 100,"en-EN") + "%"; }
            set { _agentDiscount = value; }
        }

        private string _awinlost;
        [MongoIgnore, DataMember]
        public string AgentWinloss
         {
            
            get 
            {
                var winloss = "";
                if (_bettran.Status != "running" && _bettran.Status != "waiting")
                    winloss = FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.Winlost), "en-EN");
                return winloss;
            }
            set { _awinlost = value; }
        }

        private string _agentComm;
        [MongoIgnore, DataMember]
        public string AgentComm
        {
            get { return FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.Acomm),"en-EN"); }
            set { _agentComm = value; }
        }

        private string _companywinlost;
        [MongoIgnore, DataMember]
        public string CompanyWinloss
        {
            get { return FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.CompanyWinloss),"en-EN"); }
            set { _companywinlost = value; }
        }

        #endregion

        protected Bettran _bettran;

        [MongoIgnore, DataMember]
        public Bettran BettranDetail
        {
            get { return _bettran; }
            set { _bettran = value; }
        }
    }

    [Serializable]
    [DataContract]
    public class SportEventDTO
    {
        [MongoIgnore, DataMember]
        public string Choice { get; set; }

        [MongoIgnore, DataMember]
        public string Hdp { get; set; }

        [MongoIgnore, DataMember]
        public string LiveScore { get; set; }

        [MongoIgnore, DataMember]
        public string BetType { get; set; }

        [MongoIgnore, DataMember]
        public string Event { get; set; }

        [MongoIgnore, DataMember]
        public string SportType { get; set; }

        [MongoIgnore, DataMember]
        public string LeagueName { get; set; }

        [MongoIgnore, DataMember]
        public string MoreBetInfo { get; set; }
    }

    [Serializable]
    [DataContract]
    public class CombinationDetalDTO
    {
        [MongoIgnore, DataMember]
        public string HomeName { get; set; }

        [MongoIgnore, DataMember]
        public string AwayName { get; set; }
    }

    public class CombinationDetailMP
    {
        [MongoIgnore, DataMember]
        public List<BetListDetail> items { get; set; }

        [MongoIgnore, DataMember]
        public string outStanding { get; set; }

        [MongoIgnore, DataMember]
        public string TotalWLComm { get; set; }
    }


    [Serializable]
    [DataContract]
    public class BetListSystemParlayDetail
    {
        [DataMember]
        public string Transid { get; set; }

        #region TransDate
        [DataMember]
        public string TransDate { get; set; }        
        #endregion

        #region Choice
        [DataMember]
        public string Choice { get; set; }
        [DataMember]
        public string Event { get; set; }
        #endregion

        #region Odds
        private string _odds;
        [DataMember]
        public string Odds { get; set; }
       #endregion

        #region Stake
        [DataMember] 
        public string Stake { get; set; }
        #endregion

        #region Actual Stake
        public string ActualStake { get; set; }
        #endregion

        #region Status        
        [DataMember]
        public string IP { get; set; }

        [DataMember]
        public string Status { get; set; }
        #endregion

        #region Comm - Winloss        
        [DataMember]
        public string PlayerWinLoss { get; set; }

        [DataMember]
        public string SuperPT { get; set; }
        [DataMember]
        public string SuperWinLost { get; set; }
        [DataMember]
        public string SuperComm { get; set; }
        [DataMember]
        public string SuperDiscount { get; set; }

        [DataMember]
        public string MasterPT { get; set; }
        [DataMember]
        public string MasterWinLost { get; set; }
        [DataMember]
        public string MasterComm { get; set; }
        [DataMember]
        public string MasterDiscount { get; set; }

        [DataMember]
        public string AgentPT { get; set; }
        [DataMember]
        public string AgentWinLost { get; set; }
        [DataMember]
        public string AgentComm { get; set; }
        [DataMember]
        public string AgentDiscount { get; set; }

        
        #endregion

    }



    [Serializable]
    [DataContract]
    public class SystemParlayDetail
    {
        [DataMember]
        public List<BetListSystemParlayDetail> BetList { get; set; }

        [DataMember]
        public CombinationDetailMP DetailMp { get; set; }

        [DataMember]
        public List<SystemParlayDetailGroup> SystemParlayCombinationDetailList { get; set; }
    }
    
    //class the hien object tra ve thong tin SystemParlayCombinationDetail
    [Serializable]
    [DataContract]
    public class SystemParlayCombinationDetail
    {
        [DataMember]
        public string WinLostDate { get; set; }

        [DataMember]
        public string Event { get; set; }

        [DataMember]
        public string Odds { get; set; }

        [DataMember]
        public string Stake { get; set; }

        [DataMember]
        public string WinLoss { get; set; }

        [DataMember]
        public string Status { get; set; }
    }

    [Serializable]
    [DataContract]
    public class SystemParlayDetailGroup
    {
        [DataMember]
        public List<SystemParlayCombinationDetail> Items { get; set; }
        [DataMember]
        public SystemParlayCombinationDetail SubTotal { get; set; }
        [DataMember]
        public int TotalItems { get; set; }
    }
}
