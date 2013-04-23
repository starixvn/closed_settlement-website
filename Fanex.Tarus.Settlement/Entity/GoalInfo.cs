using System;
using Sunplus.Accounting.Core.Extensions;

namespace Fanex.Tarus.Settlement.Entity
{
    public class GoalInfo
    {
        private readonly Match _matchInfo;

        private int _firstGoalValue;
        private int _lastGoalValue;

        public bool HomeScoreFirstGoal { get; private set; }

        public bool HomeScoreLastGoal { get; private set; }

        public bool AwayScoreFirstGoal { get; private set; }

        public bool AwayScoreLastGoal { get; private set; }


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
                        if (status.Contains("FL:"))
                        {
                            FirstGoalLastGoalInfo = status.Split(':')[1];

                            var goalInfoArray = FirstGoalLastGoalInfo.Split('-');

                            _firstGoalValue = Convert.ToInt32(goalInfoArray[0]);
                            _lastGoalValue = Convert.ToInt32(goalInfoArray[1]);
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
                        if (_matchInfo.GoalRecord.ToLower().StartsWith("h"))
                            HomeScoreFirstGoal = true;
                        else
                            AwayScoreFirstGoal = true;

                        //Detect Last Goal
                        if (!string.IsNullOrEmpty(_matchInfo.FinalAwayScore) &&
                            !string.IsNullOrEmpty(_matchInfo.FinalHomeScore))
                        {
                            if (_matchInfo.GoalRecord.ToLower().EndsWith("h"))
                                HomeScoreLastGoal = true;
                            else
                                AwayScoreLastGoal = true;

                        }
                    }
                }
                else
                {
                    //Detect 1st Goal

                    switch (_firstGoalValue)
                    {
                        case 1: HomeScoreFirstGoal = true;
                            break;
                        case 2: AwayScoreFirstGoal = true;
                            break;
                    }



                    //Detect Last Goal
                    if (!string.IsNullOrEmpty(_matchInfo.FinalAwayScore) &&
                        !string.IsNullOrEmpty(_matchInfo.FinalHomeScore))
                    {
                        switch (_lastGoalValue)
                        {
                            case 1: HomeScoreLastGoal = true;
                                break;
                            case 2: AwayScoreLastGoal = true;
                                break;
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
