using System;
using Sunplus.Accounting.Core.Extensions;

namespace Fanex.Tarus.Settlement.Entity
{
    public class GoalInfo
    {
        private readonly Match _matchInfo;

        private int _firstGoalValue;

        private int _lastGoalValue;

        private int _lastGoalValueHT;

        public bool HomeScoreFirstGoal { get; private set; }

        public bool HomeScoreLastGoal { get; private set; }

        public bool HomeScoreHTFirstGoal { get; private set; }

        public bool HomeScoreHTLastGoal { get; private set; }

        public bool AwayScoreFirstGoal { get; private set; }

        public bool AwayScoreLastGoal { get; private set; }

        public bool AwayScoreHTFirstGoal { get; private set; }

        public bool AwayScoreHTLastGoal { get; private set; }

        public string FirstGoalLastGoalInfo { get; private set; }

        public GoalInfo(Match matchInfo)
        {
            _matchInfo = matchInfo;

            if (_matchInfo != null)
            {
                Init();
                DetermineFirstGoalLastGoal();
            }
        }

        public GoalInfo(string matchStatus, string finalHomeScore, string finalAwayScore)
        {
            _matchInfo = new Match
            {
                BetStatus = matchStatus,
                FinalHomeScore = finalHomeScore,
                FinalAwayScore = finalAwayScore
            };

            DetermineFirstGoalLastGoal();
        }

        private void Init()
        {
            try
            {
                if (string.IsNullOrEmpty(_matchInfo.BetStatus))
                    FirstGoalLastGoalInfo = string.Empty;
                else
                {
                    string[] matchStatus = _matchInfo.BetStatus.Split(';');

                    foreach (var status in matchStatus)
                    {
                        if (status.Contains("FL:") || status.Contains("1FL:"))
                        {                            
                            FirstGoalLastGoalInfo = status.Split(':')[1];

                            var goalInfoArray = FirstGoalLastGoalInfo.Split('-');

                            _firstGoalValue = Convert.ToInt32(goalInfoArray[0]);
                            _lastGoalValue = _lastGoalValueHT = Convert.ToInt32(goalInfoArray[1]);

                            if (status.IndexOf("1FL:") != -1 || !string.IsNullOrWhiteSpace(_matchInfo.GoalRecord))
                            {
                                _lastGoalValueHT = string.IsNullOrWhiteSpace(_matchInfo.GoalRecord) ? Convert.ToInt32(goalInfoArray[1]) : (_matchInfo.GoalRecord.Substring(0, _matchInfo.GoalRecord.IndexOf(";")).EndsWith("h") ? 1 : 2);
                                _lastGoalValue = string.IsNullOrWhiteSpace(_matchInfo.GoalRecord) ? Convert.ToInt32(goalInfoArray[1]) : (_matchInfo.GoalRecord.Replace(";", string.Empty).ToLower().EndsWith("h") ? 1 : 2);
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogExtensions.ExceptionHandler(e);
            }
        }

        private void DetermineFirstGoalLastGoal()
        {
            try
            {
                //Match have not been settled yet
                if (string.IsNullOrEmpty(FirstGoalLastGoalInfo))
                {
                    if (!string.IsNullOrEmpty(_matchInfo.GoalRecord))
                    {
                        // Detect First Goal and HT First Goal
                        if (_matchInfo.GoalRecord.Split(';')[0].ToLower().StartsWith("h"))
                        {
                            HomeScoreFirstGoal = true;
                            HomeScoreHTFirstGoal = true;
                        }
                        else
                        {
                            AwayScoreFirstGoal = true;
                            AwayScoreHTFirstGoal = true;
                        }

                        //Detect Last Goal
                        if (!string.IsNullOrEmpty(_matchInfo.FinalAwayScore) &&
                            !string.IsNullOrEmpty(_matchInfo.FinalHomeScore))
                        {
                            if (_matchInfo.GoalRecord.Replace(";",  string.Empty).ToLower().EndsWith("h"))
                            {
                                HomeScoreLastGoal = true;
                            }
                            else
                            {
                                AwayScoreLastGoal = true;
                            }
                        }

                        // Detect HT Last Goal
                        if (_matchInfo.GoalRecord.Split(';')[0].ToLower().EndsWith("h"))
                        {
                            HomeScoreHTLastGoal = true;
                        }
                        else
                        {
                            AwayScoreHTLastGoal = true;
                        }
                    }
                }
                else
                {
                    //Detect First Goal and HT First Goal
                    switch (_firstGoalValue)
                    {
                        case 1: 
                            HomeScoreFirstGoal = true;
                            HomeScoreHTFirstGoal = true;
                            break;
                        case 2: 
                            AwayScoreFirstGoal = true;
                            AwayScoreHTFirstGoal = true;
                            break;
                    }

                    //Detect Last Goal
                    if (!string.IsNullOrEmpty(_matchInfo.FinalAwayScore) &&
                        !string.IsNullOrEmpty(_matchInfo.FinalHomeScore))
                    {
                        if (_matchInfo.GoalRecord.Replace(";", string.Empty).ToLower().EndsWith("h"))
                        {
                            HomeScoreLastGoal = true;
                        }
                        else
                        {
                            AwayScoreLastGoal = true;
                        }
                    }

                    // Detect HT Last Goal

                    switch (_lastGoalValueHT)
                    {
                        case 1:
                            HomeScoreHTLastGoal = true;
                            break;
                        case 2:
                            AwayScoreHTLastGoal = true;
                            break;
                    }
                }
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
            }
        }
    }
}