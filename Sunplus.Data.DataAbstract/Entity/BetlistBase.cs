using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DNAide.Core;
using DNAide.Utils;
using Norm.Attributes;
using Sunplus.Data.DataAbstract.Utils;

namespace Sunplus.Data.DataAbstract.Entity
{
    [Serializable]
    [DataContract]
    public class BetListBase
    {
        [DataMember]
        public virtual List<BetListDetailBase> Items { get; set; }

        [DataMember]
        public int totalItem { get; set; }
    }

    [Serializable]
    [DataContract]
    public class BetListDetailBase
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
            get
            {
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
        public List<CombinationDetalBaseDTO> CombinationDetails { get; set; }

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
                    result = FormatHandler.FormatNumber4(CastAide.AsDecimal(_odds), "en-EN");
                    return result;
                }
                if (_bettran.Bettype == 71 || _bettran.Bettype == 72 || _bettran.Bettype == 73 || _bettran.Bettype == 113 || _bettran.Bettype == 114 || _bettran.Bettype == 112)
                {
                    return "";
                }
                //Lack case of HR bettype
                result = FormatHandler.FormatNumber3(CastAide.AsDecimal(_odds), "en-EN");

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
            get
            {
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
            get { return FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.Stake), "en-EN"); }
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

        #region Status
        //Status get from BettranDetail
        private string _ip;
        [MongoIgnore, DataMember]
        public string Ip
        {
            get { return _bettran.Ip; } //View IPVN
            set { _ip = value; }
        }
        #endregion

        #region Comm Winloss



        private string _playerwinlost;
        [MongoIgnore, DataMember]
        public string PlayerWinloss
        {

            get { return FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.PlayerWinloss), "en-EN"); }
            set { _playerwinlost = value; }
        }




        private string _agentDiscount;
        [MongoIgnore, DataMember]
        public string AgentDiscount
        {
            get { return FormatHandler.FormatNumber2(CastAide.AsDouble(_bettran.Adiscount, 0) * 100, "en-EN") + "%"; }
            set { _agentDiscount = value; }
        }

        private string _awinlost;
        [MongoIgnore, DataMember]
        public string AgentWinloss
        {

            get
            {
                var winloss = "";
                if (_bettran.Status == "running" || _bettran.Status == "waiting")
                    winloss = "";
                else
                    winloss = FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.Winlost), "en-EN");
                return winloss;
            }
            set { _awinlost = value; }
        }

        private string _agentComm;
        [MongoIgnore, DataMember]
        public string AgentComm
        {
            get { return FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.Acomm), "en-EN"); }
            set { _agentComm = value; }
        }

        private string _companywinlost;
        [MongoIgnore, DataMember]
        public string CompanyWinloss
        {
            get { return FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.CompanyWinloss), "en-EN"); }
            set { _companywinlost = value; }
        }

        #endregion

        private Bettran _bettran;
        [MongoIgnore, DataMember]
        public Bettran BettranDetail
        {
            get { return _bettran; }
            set { _bettran = value; }
        }
    }

    [Serializable]
    [DataContract]
    public class SportEventBaseDTO
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
    public class CombinationDetalBaseDTO
    {
        [MongoIgnore, DataMember]
        public string HomeName { get; set; }

        [MongoIgnore, DataMember]
        public string AwayName { get; set; }
    }

    [Serializable]
    [DataContract]
    public class CombinationDetailMPBase
    {
        [MongoIgnore, DataMember]
        public List<BetListDetail> items { get; set; }

        [MongoIgnore, DataMember]
        public string outStanding { get; set; }

        [MongoIgnore, DataMember]
        public string TotalWLComm { get; set; }
    }
}
