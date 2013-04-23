using System;

namespace Sunplus.Accounting.Entities.Report
{
    /// <summary>
    /// Summary description for TurnoverPositionTaking
    /// </summary>
    [Serializable]
    public class TurnoverPositionTakingWinlossMargin
    {
        public float _positionTaking;
        public double Turnover { get; set; }

        public double Winloss { get; set; }

        public float PositionTaking
        {
            get { return _positionTaking; }
        }

        public float Margin
        {
            get
            {
                if (Turnover == 0)
                    return 0;
                return (float) (Winloss/Turnover)*100;
            }
        }

        public float GetPositionTaking(double totalTurnover)
        {
            if (totalTurnover == 0)
                return 0;
            _positionTaking = (float) (Turnover/totalTurnover)*100;
            return _positionTaking;
        }
    }
}