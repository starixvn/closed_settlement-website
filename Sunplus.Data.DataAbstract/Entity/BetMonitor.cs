using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using DNAide.Core;
using Norm.Attributes;
using Sunplus.Data.DataAbstract.Utils;
using DNAide.Utils;

namespace Sunplus.Data.DataAbstract.Entity
{
    [Serializable]
    [DataContract]
    public class BetMonitor
    {

        [DataMember]
        public string Transid { get; set; }

        #region TransDate

        private string _code;
        [MongoIgnore, DataMember]
        public string Code
        {
            get
            {
                if (_bettran.Bettype == 71 || _bettran.Bettype == 72 || _bettran.Bettype == 73 || _bettran.Bettype == 113 || _bettran.Bettype == 114 || _bettran.Bettype == 112)
                {
                    return _bettran.MatchCode;
                }
                return _bettran.Transid;
            }
            set { _code = value; }
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
        #endregion

        #region Choice

        private SportEventDTO _sportEventDTO;
        [DataMember]
        public SportEventDTO SportEvent
        {
            get
            {
                var sportEvent = new BASportEvent(_bettran);
                return sportEvent.GetSportEvent();
            }
            set { _sportEventDTO = value; }
        }


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
                    result =  FormatHandler.FormatNumber4(CastAide.AsDecimal(_odds),"en-EN");
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

        #region Stake
        private string _stake;
        [MongoIgnore, DataMember]
        public string Stake
        {
            get { return FormatHandler.FormatNumber2(CastAide.AsDecimal(_bettran.Stake),"en-EN"); }
            set { _stake = value; }
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
}
