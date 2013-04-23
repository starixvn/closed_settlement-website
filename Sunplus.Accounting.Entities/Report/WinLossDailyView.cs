using System;

namespace Sunplus.Accounting.Entities.Report
{
    /// <summary>
    /// Summary description for WinLossDaily
    /// </summary>
    [Serializable]
    public class WinLossDailyView
    {
        private TurnoverPositionTakingWinlossMargin _agent;
        private TurnoverPositionTakingWinlossMargin _company;
        private TurnoverPositionTakingWinlossMargin _ma;
        private TurnoverPositionTakingWinlossMargin _sa;

        public WinLossDailyView()
        {
            _company = new TurnoverPositionTakingWinlossMargin();
            _sa = new TurnoverPositionTakingWinlossMargin();
            _ma = new TurnoverPositionTakingWinlossMargin();
            _agent = new TurnoverPositionTakingWinlossMargin();
        }

        public int No { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public double TotalTO { get; set; }

        public TurnoverPositionTakingWinlossMargin Company
        {
            get { return _company; }
            set { _company = value; }
        }

        public TurnoverPositionTakingWinlossMargin SA
        {
            get { return _sa; }
            set { _sa = value; }
        }

        public TurnoverPositionTakingWinlossMargin MA
        {
            get { return _ma; }
            set { _ma = value; }
        }

        public TurnoverPositionTakingWinlossMargin Agent
        {
            get { return _agent; }
            set { _agent = value; }
        }
    }
}