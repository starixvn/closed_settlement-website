using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Sunplus.Accounting.Core.Extensions;

namespace Fanex.Tarus.Settlement.Entity
{
    [Serializable]
    [DataContract]
    public class Match
    {
        [DataMember]
        public decimal Index { get; set; }

        [DataMember]
        public int MatchId { get; set; }

        [DataMember]
        public int LeagueId { get; set; }

        [DataMember]
        public string MatchCode { get; set; }

        [DataMember]
        public string EventDate { get; set; }

        [DataMember]
        public string KickOffTime { get; set; }

        [DataMember]
        public string ShowTime { get; set; }

        [DataMember]
        public string FinalHomeScore { get; set; }

        [DataMember]
        public string FinalAwayScore { get; set; }

        [DataMember]
        public string BetStatus { get; set; }

        [DataMember]
        public string EventStatus { get; set; }

        [DataMember]
        public bool EnableCalculate { get; set; }

        [DataMember]
        public bool IsSettled { get; set; }

        [DataMember]
        public string FTStatus { get; set; }

        [DataMember]
        public int BetType { get; set; }

        public string GoalRecord { get; set; }

        public GoalInfo GoalHistory { get {return new GoalInfo(this);} }

        public bool HasFirstGoalLastGoal { get; set; }

        public bool IsHT = false;

        public bool IsFT = false;

        /// <summary>
        /// Generate match status base on FinalScore, GoalRecord
        /// </summary>
        /// <returns></returns>
        public string GenerateMatchStatus()
        {
            string firstGoal = "0", firstGoalHT = "0";
            string lastGoal = "0", lastGoalHT = "0";
            string betStatus = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(GoalRecord))
                {
                    var goalRecords = GoalRecord.ToLower().Split(';');
                    var index = goalRecords.Length > 1 ? 1 : 0;

                    firstGoal = goalRecords[0].ToLower().StartsWith("h") ? "1" : "2";
                    firstGoalHT = goalRecords[0].ToLower().StartsWith("h") ? "1" : "2";

                    if (!string.IsNullOrEmpty(FinalHomeScore) && !string.IsNullOrEmpty(FinalAwayScore))
                    {
                        lastGoal = (goalRecords[index] == string.Empty ? goalRecords[0] : goalRecords[index]).ToLower().EndsWith("h") ? "1" : "2";
                        lastGoalHT = goalRecords[0].ToLower().EndsWith("h") ? "1" : "2";
                    }
                }

                firstGoal = string.IsNullOrWhiteSpace(firstGoal) ? "0" : firstGoal;
                lastGoal = string.IsNullOrWhiteSpace(lastGoal) ? "0" : lastGoal;

                firstGoalHT = string.IsNullOrWhiteSpace(firstGoalHT) ? "0" : firstGoalHT;
                lastGoalHT = string.IsNullOrWhiteSpace(lastGoalHT) ? "0" : lastGoalHT;

                var score =  firstGoal + "-" + lastGoal;
                var scoreHT = firstGoalHT + "-" + lastGoalHT;
                betStatus = IsHT ? string.Concat("1FL:", scoreHT, ";") : 
                    IsFT ? string.Concat("FL:", score) :
                    string.IsNullOrWhiteSpace(GoalRecord) ? "1FL:0-0;FL:0-0" :
                    string.Concat("1FL:", scoreHT, ";FL:", score);
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
            }
            return betStatus;
        }
    }
}
