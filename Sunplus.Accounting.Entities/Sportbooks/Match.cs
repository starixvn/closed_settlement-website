using System;
using System.Runtime.Serialization;
using Norm;

namespace Sunplus.Accounting.DataAbstract.Entity
{
    [DataContract]
    public class Match
    {

      
        
        #region public properties

        [MongoIdentifier]
        public string Matchid { get; set; }

        public string Leagueid { get; set; }

        public string Homeid { get; set; }

        public string Awayid { get; set; }

        public DateTime Eventdate { get; set; }

        public string Eventstatus { get; set; }

        public string Betstatus { get; set; }

        public int Livehomescore { get; set; }

        public int Liveawayscore { get; set; }

        public int Finalhomescore { get; set; }

        public int Finalawayscore { get; set; }

        public int Creator { get; set; }

        public string Matchcode { get; set; }

        public DateTime Kickofftime { get; set; }

        public DateTime Closedtime { get; set; }

        public string Showtime { get; set; }

        public int Hthomescore { get; set; }

        public int Htawayscore { get; set; }

        public int Ruben { get; set; }

        public int Multiple { get; set; }

        public bool Yenbet { get; set; }

        public long Exrisk { get; set; }

        public string Sporttype { get; set; }

        public int Homeid2 { get; set; }

        public int Awayid2 { get; set; }

        public int DisplayMode { get; set; }

        public short LeaguePatternId { get; set; }

        public short HomeDisplayPatternId { get; set; }

        public short AwayDisplayPatternId { get; set; }

        public bool IsNeutral { get; set; }

        public int InjuryTime { get; set; }

        public string GoalRecord { get; set; }

        public int ParentId { get; set; }

        public int ChildMatchType { get; set; }

        public DateTime GlobalShowTime { get; set; }

        public bool HasLive { get; set; }

        public DateTime Changetime { get; set; }

        public DateTime Settletime { get; set; }

        #endregion

        
      

       
        
    }
}
