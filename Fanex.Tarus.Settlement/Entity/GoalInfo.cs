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
                        if (status.Contains("1FL:"))
                        {
                            FirstGoalLastGoalInfo = status.Split(':')[1];
                            var goalInfoArray = FirstGoalLastGoalInfo.Split('-');

                            _firstGoalValue = Convert.ToInt32(goalInfoArray[0]);
                            _lastGoalValue = _lastGoalValueHT = Convert.ToInt32(goalInfoArray[1]);

                            var goalRecord2T = string.IsNullOrWhiteSpace(_matchInfo.GoalRecord) ? string.Empty : _matchInfo.GoalRecord.Replace(";", string.Empty).ToLower();
                            _lastGoalValueHT = Convert.ToInt32(goalInfoArray[1]);
                            _lastGoalValue =
                                Convert.ToInt32(goalInfoArray[1]) == 0 ?
                                Convert.ToInt32(goalInfoArray[1]) :
                                (goalRecord2T.EndsWith("h") ? 1 : 2);

                            if (!string.IsNullOrWhiteSpace(_matchInfo.GoalRecord) && _matchInfo.GoalRecord.Contains(";"))
                            {
                                HomeScoreHTFirstGoal = _firstGoalValue == 1;
                                AwayScoreHTFirstGoal = _firstGoalValue == 2;
                                HomeScoreHTLastGoal = _lastGoalValueHT == 1;
                                AwayScoreHTLastGoal = _lastGoalValueHT == 2;
                                HomeScoreFirstGoal = HomeScoreHTFirstGoal;
                                AwayScoreFirstGoal = AwayScoreHTFirstGoal;
                            }
                            else
                            {
                                HomeScoreFirstGoal = _firstGoalValue == 1;
                                AwayScoreFirstGoal = _firstGoalValue == 2;
                            }

                            continue;
                        }

                        if (status.Contains("FL:"))
                        {
                            FirstGoalLastGoalInfo = status.Split(':')[1];
                            var firstGoalLastGoalInfo = status.Split(':')[1];
                            var goalInfoArray = firstGoalLastGoalInfo.Split('-');
                            _firstGoalValue = Convert.ToInt32(goalInfoArray[0]) == 0 ?
                                0 :
                                (_firstGoalValue == 0 ? Convert.ToInt32(goalInfoArray[0]) : _firstGoalValue);

                            _lastGoalValue = _lastGoalValueHT = Convert.ToInt32(goalInfoArray[1]);

                            if (!string.IsNullOrWhiteSpace(_matchInfo.GoalRecord))
                            {
                                var goalRecord2T = string.IsNullOrWhiteSpace(_matchInfo.GoalRecord) ? string.Empty : _matchInfo.GoalRecord.Replace(";", string.Empty).ToLower();

                                _lastGoalValue =
                                    Convert.ToInt32(goalInfoArray[1]) == 0 ?
                                    Convert.ToInt32(goalInfoArray[1]) :
                                    (goalRecord2T.EndsWith("h") ? 1 : 2);
                            }

                            HomeScoreFirstGoal = (_firstGoalValue != 0) && (HomeScoreFirstGoal || _firstGoalValue == 1);
                            AwayScoreFirstGoal = (_firstGoalValue != 0) && (AwayScoreFirstGoal || _firstGoalValue == 2);

                            HomeScoreLastGoal = _lastGoalValue == 1;
                            AwayScoreLastGoal = _lastGoalValue == 2;
                            continue;
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
                        var hasSemiColon = _matchInfo.GoalRecord.IndexOf(";") != -1;
                        // Detect First Goal and HT First Goal
                        if (_matchInfo.GoalRecord.Split(';')[0].ToLower().StartsWith("h"))
                        {
                            HomeScoreFirstGoal = true;
                            HomeScoreHTFirstGoal = hasSemiColon;
                        }
                        else
                        {
                            AwayScoreFirstGoal = true;
                            AwayScoreHTFirstGoal = hasSemiColon;
                        }
                        
                        // Detect HT Last Goal
                        if (_matchInfo.GoalRecord.Split(';')[0].ToLower().EndsWith("h"))
                        {
                            HomeScoreHTLastGoal = hasSemiColon;
                        }
                        else
                        {
                            AwayScoreHTLastGoal = hasSemiColon;
                        }
                        
                        //Detect Last Goal
                        if (!string.IsNullOrEmpty(_matchInfo.FinalAwayScore) &&
                            !string.IsNullOrEmpty(_matchInfo.FinalHomeScore) &&
                            _matchInfo.FinalAwayScore != "0" && _matchInfo.FinalHomeScore != "0")
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