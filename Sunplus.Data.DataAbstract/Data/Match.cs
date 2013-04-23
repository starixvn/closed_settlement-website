using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Sunplus.Accounting.DataAbstract.Data
{
    [DataContract]
    public class Match : AbstractModel, IDataModel
    {

        #region db fields
        [BsonId]
        public ObjectId _id { get; set; }

        #region public properties

        public int Matchid
        {
            get { return _matchid; }
            set { _matchid = value; }
        }

        public int Leagueid
        {
            get { return _leagueid; }
            set { _leagueid = value; }
        }

        public int Homeid
        {
            get { return _homeid; }
            set { _homeid = value; }
        }

        public int Awayid
        {
            get { return _awayid; }
            set { _awayid = value; }
        }

        public DateTime? Eventdate
        {
            get { return _eventdate; }
            set { _eventdate = value; }
        }

        public string Eventstatus
        {
            get { return _eventstatus; }
            set { _eventstatus = value; }
        }

        public string Betstatus
        {
            get { return _betstatus; }
            set { _betstatus = value; }
        }

        public int? Livehomescore
        {
            get { return _livehomescore; }
            set { _livehomescore = value; }
        }

        public int? Liveawayscore
        {
            get { return _liveawayscore; }
            set { _liveawayscore = value; }
        }

        public int? Finalhomescore
        {
            get { return _finalhomescore; }
            set { _finalhomescore = value; }
        }

        public int? Finalawayscore
        {
            get { return _finalawayscore; }
            set { _finalawayscore = value; }
        }

        public int Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }

        public string Matchcode
        {
            get { return _matchcode; }
            set { _matchcode = value; }
        }

        public DateTime? Kickofftime
        {
            get { return _kickofftime; }
            set { _kickofftime = value; }
        }

        public DateTime? Closedtime
        {
            get { return _closedtime; }
            set { _closedtime = value; }
        }

        public string Showtime
        {
            get { return _showtime; }
            set { _showtime = value; }
        }

        public int? Hthomescore
        {
            get { return _hthomescore; }
            set { _hthomescore = value; }
        }

        public int? Htawayscore
        {
            get { return _htawayscore; }
            set { _htawayscore = value; }
        }

        public byte? Ruben
        {
            get { return _ruben; }
            set { _ruben = value; }
        }

        public byte? Multiple
        {
            get { return _multiple; }
            set { _multiple = value; }
        }

        public bool? Yenbet
        {
            get { return _yenbet; }
            set { _yenbet = value; }
        }

        public double? Exrisk
        {
            get { return _exrisk; }
            set { _exrisk = value; }
        }

        public byte Sporttype
        {
            get { return _sporttype; }
            set { _sporttype = value; }
        }

        public int Homeid2
        {
            get { return _homeid2; }
            set { _homeid2 = value; }
        }

        public int Awayid2
        {
            get { return _awayid2; }
            set { _awayid2 = value; }
        }

        public byte DisplayMode
        {
            get { return _DisplayMode; }
            set { _DisplayMode = value; }
        }

        public short LeaguePatternId
        {
            get { return _LeaguePatternID; }
            set { _LeaguePatternID = value; }
        }

        public short HomeDisplayPatternId
        {
            get { return _HomeDisplayPatternID; }
            set { _HomeDisplayPatternID = value; }
        }

        public short AwayDisplayPatternId
        {
            get { return _AwayDisplayPatternID; }
            set { _AwayDisplayPatternID = value; }
        }

        public bool IsNeutral
        {
            get { return _IsNeutral; }
            set { _IsNeutral = value; }
        }

        public byte? InjuryTime
        {
            get { return _InjuryTime; }
            set { _InjuryTime = value; }
        }

        public string GoalRecord
        {
            get { return _GoalRecord; }
            set { _GoalRecord = value; }
        }

        public int? ParentId
        {
            get { return _ParentID; }
            set { _ParentID = value; }
        }

        public byte? ChildMatchType
        {
            get { return _ChildMatchType; }
            set { _ChildMatchType = value; }
        }

        public DateTime? GlobalShowTime
        {
            get { return _GlobalShowTime; }
            set { _GlobalShowTime = value; }
        }

        public bool HasLive
        {
            get { return _HasLive; }
            set { _HasLive = value; }
        }

        public DateTime? Changetime
        {
            get { return _changetime; }
            set { _changetime = value; }
        }

        public DateTime? Settletime
        {
            get { return _settletime; }
            set { _settletime = value; }
        }
        #endregion

        #region private properties
        private int _matchid;

        private int _leagueid;

        private int _homeid;

        private int _awayid;

        private System.Nullable<System.DateTime> _eventdate;

        private string _eventstatus;

        private string _betstatus;

        private System.Nullable<int> _livehomescore;

        private System.Nullable<int> _liveawayscore;

        private System.Nullable<int> _finalhomescore;

        private System.Nullable<int> _finalawayscore;

        private int _creator;

        private string _matchcode;

        private System.Nullable<System.DateTime> _kickofftime;

        private System.Nullable<System.DateTime> _closedtime;

        private string _showtime;

        private System.Nullable<int> _hthomescore;

        private System.Nullable<int> _htawayscore;

        private System.Nullable<byte> _ruben;

        private System.Nullable<byte> _multiple;

        private System.Nullable<bool> _yenbet;

        private System.Nullable<double> _exrisk;

        private byte _sporttype;

        private int _homeid2;

        private int _awayid2;

        private byte _DisplayMode;

        private short _LeaguePatternID;

        private short _HomeDisplayPatternID;

        private short _AwayDisplayPatternID;

        private bool _IsNeutral;

        private System.Nullable<byte> _InjuryTime;

        private string _GoalRecord;

        private System.Nullable<int> _ParentID;

        private System.Nullable<byte> _ChildMatchType;

        private System.Nullable<System.DateTime> _GlobalShowTime;

        private bool _HasLive;

        private System.Nullable<System.DateTime> _changetime;

        private System.Nullable<System.DateTime> _settletime;
        #endregion
        #endregion

        #region constructors
        public Match()
            : base("Match")
        {
        }
        ~Match() { }
        #endregion

        #region IDataModel members
        public object Get(int key)
        {
            throw new NotImplementedException();
        }

        public bool Insert(string key, object value)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int key)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
