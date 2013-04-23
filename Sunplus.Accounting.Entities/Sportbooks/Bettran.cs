using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Norm;
using Norm.Attributes;
using Sunplus.Accounting.DataAbstract.Data;
using Sunplus.Accounting.DataAbstract.Utils;

namespace Sunplus.Accounting.DataAbstract.Entity
{
    [DataContract]
    public class Bettran
    {

        #region public properties

        [MongoIdentifier, DataMember]
        public string Transid { get; set; }

        [DataMember]
        public int Custid { get; set; }

        [DataMember]
        public DateTime Transdate { get; set; }

        [DataMember]
        public int Oddsid { get; set; }

        [DataMember]
        public long Hdp1 { get; set; }

        [DataMember]
        public long Hdp2 { get; set; }

        [DataMember]
        public long Odds { get; set; }

        [DataMember]
        public long Stake { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public long Winlost { get; set; }

        [DataMember]
        public long Awinlost { get; set; }

        [DataMember]
        public long Mwinlost { get; set; }

        [DataMember]
        public long Playercomm { get; set; }

        [DataMember]
        public long Comm { get; set; }

        [DataMember]
        public long Acomm { get; set; }

        [DataMember]
        public long Apositiontaking { get; set; }

        [DataMember]
        public long Mpositiontaking { get; set; }

        [DataMember]
        public long Tpositiontaking { get; set; }

        [DataMember]
        public long Playerdiscount { get; set; }

        [DataMember]
        public long Discount { get; set; }

        [DataMember]
        public long Adiscount { get; set; }

        [DataMember]
        public short Livehomescore { get; set; }

        [DataMember]
        public short Liveawayscore { get; set; }

        [DataMember]
        public bool Liveindicator { get; set; }

        [DataMember]
        public string Betteam { get; set; }

        [DataMember]
        public int Creator { get; set; }

        [DataMember]
        public long Refno { get; set; }

        [DataMember]
        public string Comstatus { get; set; }

        [DataMember]
        public DateTime Winlostdate { get; set; }

        [DataMember]
        public string Betfrom { get; set; }

        [DataMember]
        public string Betcheck { get; set; }

        [DataMember]
        public DateTime Checktime { get; set; }

        [DataMember]
        public long Oddsspread { get; set; }

        [DataMember]
        public double Actualrate { get; set; }

        [DataMember]
        public int Matchid { get; set; }

        [DataMember]
        public int Recommend { get; set; }

        [DataMember]
        public int Mrecommend { get; set; }

        [DataMember]
        public byte Ruben { get; set; }

        [DataMember]
        public long Betid { get; set; }

        [DataMember]
        public int Statuswinlost { get; set; }

        [DataMember]
        public long Modds { get; set; }

        [DataMember]
        public string Bettype { get; set; }

        //[DataMember]
        //public long? Betdaqid { get; set; }

        [DataMember]
        public long ActualStake { get; set; }

        [DataMember]
        public int Currency { get; set; }

        [DataMember]
        public string Ip { get; set; }

        [DataMember]
        public string Transdesc { get; set; }

        [DataMember]
        public long Sdiscount { get; set; }

        [DataMember]
        public long Scomm { get; set; }

        [DataMember]
        public long Spositiontaking { get; set; }

        [DataMember]
        public long Swinlost { get; set; }

        [DataMember]
        public int Srecommend { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string CurrencyName { get; set; }

        [DataMember]
        public double Virtualrate { get; set; }

        [DataMember]
        public int Oddstype { get; set; }

        [DataMember]
        public int StatusId { get; set; }

        [DataMember]
        public int Currency2 { get; set; }

        [DataMember]
        public string LeagueId { get; set; }

        [DataMember]
        public string HomeId { get; set; }

        [DataMember]
        public string AwayId { get; set; }

        [DataMember]
        public string SportId { get; set; }
        
        [MongoIgnore]
        [DataMember]
        public string LeagueName
        {
            get
            {
                //Get base on LeagueId
                return SportBookUtils.GetLeagueName(this.LeagueId);
            }
        }

        [MongoIgnore]
        [DataMember]
        public string AwayName
        {
            get { return SportBookUtils.GetTeamName(this.AwayId); }
        }

        [MongoIgnore]
        [DataMember]
        public string HomeName
        {
            get { return SportBookUtils.GetTeamName(this.HomeId); }
        }

        [MongoIgnore]
        [DataMember]
        public string SportName
        {
            get { return SportBookUtils.GetSportName(this.SportId); }
        }

        [MongoIgnore]
        [DataMember]
        public string BettypeName
        {
            get { return SportBookUtils.GetBetTypeName(this.Bettype); }
        }

        [MongoIgnore]
        [DataMember]
        public string OddName
        {
            get { return string.Empty; }
        }
        
        #endregion public properties

    }
}
