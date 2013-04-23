using System;
using System.Collections;
using System.Collections.Generic;
using Sunplus.Accounting.DataAccess.Provider.Report;

/// <summary>
/// Summary description for MatchWLProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.Entity.SqlServer.Report
{
    public class MatchWLProviderClient : MatchWLProvider
    {
        public override List<MatchWLRow> MatchWL14(DateTime wldate, ref Hashtable Outputs)
        {
            var param = new Hashtable();
            param.Add("winlostdate", wldate);
            param.Add("winlost_8rm", 0);
            param.Add("fi_3rdshare", 0);
            List<MatchWLRow> lst = SqlDataContext.ExecuteReader<MatchWLRow>("Acc_Rpt_MatchWinLoss214", param,
                                                                            DBType.WAREHOUSE, 1000, ref Outputs);
            return lst;
        }

        public override List<MatchWLRow> MatchWL(DateTime wldate, ref Hashtable Outputs, string db)
        {
            return null;
        }
    }

    #region MatchWLRow

    [Serializable]
    public class MatchWLRow
    {
        //TODO: Integrate this class with any existing data object class

        private int isneutral;
        private decimal linetotal;
        private int m_awayid;
        private int m_bettype;
        private decimal m_comm;
        private string m_eventdate;
        private int m_homeid;
        private string m_kickofftime;
        private string m_leaguenametc;
        private int m_liveindicator;
        private string m_matchcode;
        private decimal m_pos;
        private decimal m_winlost;
        private decimal virtualcomm;
        private decimal virtuallinetotal;

        public MatchWLRow(string matchcode, string eventdate, string kickofftime, int liveindicator, int bettype,
                          int homeid, int awayid, decimal winlost, decimal pos, decimal comm, decimal VirtualComm,
                          decimal LineTotal, decimal VirtualLineTotal, string leaguenametc, int IsNeutral)
        {
            m_matchcode = matchcode;
            m_eventdate = eventdate;
            m_kickofftime = kickofftime;
            m_liveindicator = liveindicator;
            m_bettype = bettype;
            m_homeid = homeid;
            m_awayid = awayid;
            m_winlost = winlost;
            m_pos = pos;
            m_comm = comm;
            virtualcomm = VirtualComm;
            linetotal = LineTotal;
            virtuallinetotal = VirtualLineTotal;
            m_leaguenametc = leaguenametc;
            isneutral = IsNeutral;
        }

        public MatchWLRow()
        {
            m_matchcode = null;
            m_eventdate = "";
            m_kickofftime = "";
            m_liveindicator = 0;
            m_bettype = 0;
            m_homeid = 0;
            m_awayid = 0;
            m_winlost = 0;
            m_pos = 0;
            m_comm = 0;
            virtualcomm = 0;
            linetotal = 0;
            virtuallinetotal = 0;
            m_leaguenametc = null;
            isneutral = 0;
        }

        ////[DataMapping("matchcode")]
        public string matchcode
        {
            get { return m_matchcode; }
            set { m_matchcode = value; }
        }

        ////[DataMapping("eventdate")]
        public string eventdate
        {
            get { return m_eventdate; }
            set { m_eventdate = value; }
        }

        ////[DataMapping("kickofftime")]
        public string kickofftime
        {
            get { return m_kickofftime; }
            set { m_kickofftime = value; }
        }

        ////[DataMapping("liveindicator")]
        public int liveindicator
        {
            get { return m_liveindicator; }
            set { m_liveindicator = value; }
        }

        ////[DataMapping("bettype")]
        public int bettype
        {
            get { return m_bettype; }
            set { m_bettype = value; }
        }

        ////[DataMapping("homeid")]
        public int homeid
        {
            get { return m_homeid; }
            set { m_homeid = value; }
        }

        ////[DataMapping("awayid")]
        public int awayid
        {
            get { return m_awayid; }
            set { m_awayid = value; }
        }

        ////[DataMapping("winlost")]
        public decimal winlost
        {
            get { return m_winlost; }
            set { m_winlost = value; }
        }

        ////[DataMapping("pos")]
        public decimal pos
        {
            get { return m_pos; }
            set { m_pos = value; }
        }

        ////[DataMapping("comm")]
        public decimal comm
        {
            get { return m_comm; }
            set { m_comm = value; }
        }

        ////[DataMapping("VirtualComm")]
        public decimal VirtualComm
        {
            get { return virtualcomm; }
            set { virtualcomm = value; }
        }

        ////[DataMapping("LineTotal")]
        public decimal LineTotal
        {
            get { return linetotal; }
            set { linetotal = value; }
        }

        ////[DataMapping("VirtualLineTotal")]
        public decimal VirtualLineTotal
        {
            get { return virtuallinetotal; }
            set { virtuallinetotal = value; }
        }

        ////[DataMapping("leaguenametc")]
        public string leaguenametc
        {
            get { return m_leaguenametc; }
            set { m_leaguenametc = value; }
        }

        ////[DataMapping("IsNeutral")]
        public int IsNeutral
        {
            get { return isneutral; }
            set { isneutral = value; }
        }
    }

    #endregion
}