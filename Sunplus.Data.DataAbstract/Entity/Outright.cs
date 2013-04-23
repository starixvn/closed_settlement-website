using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Norm;

namespace Sunplus.Data.DataAbstract.Entity
{
    [DataContract]
    public class Outright
    {
        [DataMember, MongoIdentifier]
        public string Orid { get; set; }

        [DataMember]
        public string Leagueid { get; set; }

        [DataMember]
        public string Teamid { get; set; }

        [DataMember]
        public string Eventdate { get; set; }

        [DataMember]
        public string Kickofftime { get; set; }

        [DataMember]
        public string Oddsstatus { get; set; }

        [DataMember]
        public string Odds { get; set; }

        [DataMember]
        public string Maxbet { get; set; }

        [DataMember]
        public string Gapodds { get; set; }

        [DataMember]
        public string Matchcode { get; set; }

        [DataMember]
        public string Groupmatch { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string Creator { get; set; }

        [DataMember]
        public string Bal { get; set; }

        [DataMember]
        public string Risk { get; set; }

        [DataMember]
        public string DisplayMode { get; set; }

        [DataMember]
        public string ProgramId { get; set; }

        [DataMember]
        public string HorseJockey { get; set; }

        [DataMember]
        public string HorseDraw { get; set; }

        [DataMember]
        public string HorseWgt { get; set; }

        [DataMember]
        public string PlaceOdds { get; set; }

        [DataMember]
        public string Placing { get; set; }

        [DataMember]
        public bool IsFavor { get; set; }

        [DataMember]
        public bool IsScr { get; set; }

        [DataMember]
        public string Colours { get; set; }

        [DataMember]
        public string HorseTrainer { get; set; }

        [DataMember]
        public string FOddsStatus { get; set; }

        [DataMember]
        public string ScrTime { get; set; }

        [DataMember]
        public string Changetime { get; set; }

        [DataMember]
        public string LeaguePatternId { get; set; }

        [DataMember]
        public string TeamPatternId { get; set; }

        [DataMember]
        public string ProgramDate { get; set; }
    }
}
