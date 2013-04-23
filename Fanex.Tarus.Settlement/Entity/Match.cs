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

        /// <summary>
        /// Generate match status base on FinalScore, GoalRecord
        /// </summary>
        /// <returns></returns>
        public string GenerateMatchStatus()
        {
            string firstGoal = "0";
            string lastGoal = "0";
            string betStatus = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(GoalRecord))
                {
                    firstGoal = GoalRecord.ToLower().StartsWith("h") ? "1" : "2";

                    if (!string.IsNullOrEmpty(FinalHomeScore) && !string.IsNullOrEmpty(FinalAwayScore))
                    {
                        lastGoal = GoalRecord.ToLower().EndsWith("h") ? "1" : "2";
                    }
                }

                betStatus = "FL:" + firstGoal + "-" + lastGoal;
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
            }
            return betStatus;
        }
    }
}
