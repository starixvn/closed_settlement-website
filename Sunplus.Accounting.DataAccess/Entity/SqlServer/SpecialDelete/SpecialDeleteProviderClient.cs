using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DNAide.Core;
using Sunplus.Accounting.DataAccess.Provider.SpecialDelete;

/// <summary>
/// Summary description for SpecialDeleteProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Entity.SqlServer.SpecialDelete
{
    public class SpecialDeleteProviderClient : SpecialDeleteProvider
    {

        public override List<MatchInfoRow> MatchSeById(int MatchId)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("matchid", MatchId);
            var spElement =
                GetStoreProcedure(
                    Core.Configurations.StoreProcedureGroup.SpecialDeleteGroupSection.StoreCollection.Sportbook, "SelMatchInfo");
            IDataReader dataReader = DNAide.Data.DbHelper.ExecuteReader(spElement.Name, hashtable, spElement.DbName, 600);

            List<MatchInfoRow> lst = new List<MatchInfoRow>();
            while (dataReader.Read())
            {
                MatchInfoRow infoRow = new MatchInfoRow();
                infoRow.awayid = CastAide.AsInt32(dataReader["awayid"]);
                infoRow.homeid = CastAide.AsInt32(dataReader["homeid"]);
                infoRow.eventdate = CastAide.AsDateTime(dataReader["eventdate"]);
                infoRow.IsNeutral = CastAide.AsBoolean(dataReader["IsNeutral"]);
                infoRow.leagueid = CastAide.AsInt32(dataReader["leagueid"]);
                infoRow.matchcode = CastAide.AsString(dataReader["matchcode"]);

                lst.Add(infoRow);
            }

            dataReader.Close();

            return lst;
        }
    }

    #region SpecialDeleteRow

    [Serializable]
    public class SpecialDeleteRow
    {
        //TODO: Integrate this class with any existing data object class

        private string m_betteam;
        private int m_bettype;
        private string m_creator;
        private int m_currency;
        private decimal m_hdp1;
        private decimal m_hdp2;
        private int m_leagueid;
        private int m_liveawayscore;
        private int m_livehomescore;
        private decimal m_odds;
        private int m_oddstype;
        private string m_status;
        private int m_teamid;
        private DateTime m_transdate;
        private string m_transid;
        private string m_username;
        private decimal stakepositiontaking;

        public SpecialDeleteRow(string username, string transid, int bettype, string betteam, decimal odds, int oddstype,
                                decimal StakePositiontaking, int currency, decimal hdp1, decimal hdp2,
                                DateTime transdate, int livehomescore, int liveawayscore, string creator, string status,
                                int leagueid, int teamid)
        {
            m_username = username;
            m_transid = transid;
            m_bettype = bettype;
            m_betteam = betteam;
            m_odds = odds;
            m_oddstype = oddstype;
            stakepositiontaking = StakePositiontaking;
            m_currency = currency;
            m_hdp1 = hdp1;
            m_hdp2 = hdp2;
            m_transdate = transdate;
            m_livehomescore = livehomescore;
            m_liveawayscore = liveawayscore;
            m_creator = creator;
            m_status = status;
            m_leagueid = leagueid;
            m_teamid = teamid;
        }

        public SpecialDeleteRow()
        {
            m_username = null;
            m_transid = null;
            m_bettype = 0;
            m_betteam = null;
            m_odds = 0;
            m_oddstype = 0;
            stakepositiontaking = 0;
            m_currency = 0;
            m_hdp1 = 0;
            m_hdp2 = 0;
            m_transdate = DateTime.Today;
            m_livehomescore = 0;
            m_liveawayscore = 0;
            m_creator = null;
            m_status = null;
        }

        //////[DataMapping("username")]
        public string username
        {
            get { return m_username; }
            set { m_username = value; }
        }

        //////[DataMapping("transid")]
        public string transid
        {
            get { return m_transid; }
            set { m_transid = value; }
        }

        ////[DataMapping("bettype")]
        public int bettype
        {
            get { return m_bettype; }
            set { m_bettype = value; }
        }

        ////[DataMapping("betteam")]
        public string betteam
        {
            get { return m_betteam; }
            set { m_betteam = value; }
        }

        ////[DataMapping("odds")]
        public decimal odds
        {
            get { return m_odds; }
            set { m_odds = value; }
        }

        ////[DataMapping("oddstype")]
        public int oddstype
        {
            get { return m_oddstype; }
            set { m_oddstype = value; }
        }

        ////[DataMapping("StakePositiontaking")]
        public decimal StakePositiontaking
        {
            get { return stakepositiontaking; }
            set { stakepositiontaking = value; }
        }

        ////[DataMapping("currency")]
        public int currency
        {
            get { return m_currency; }
            set { m_currency = value; }
        }

        ////[DataMapping("hdp1")]
        public decimal hdp1
        {
            get { return m_hdp1; }
            set { m_hdp1 = value; }
        }

        ////[DataMapping("hdp2")]
        public decimal hdp2
        {
            get { return m_hdp2; }
            set { m_hdp2 = value; }
        }

        ////[DataMapping("transdate")]
        public DateTime transdate
        {
            get { return m_transdate; }
            set { m_transdate = value; }
        }

        ////[DataMapping("livehomescore")]
        public int livehomescore
        {
            get { return m_livehomescore; }
            set { m_livehomescore = value; }
        }

        ////[DataMapping("liveawayscore")]
        public int liveawayscore
        {
            get { return m_liveawayscore; }
            set { m_liveawayscore = value; }
        }

        ////[DataMapping("creator")]
        public string creator
        {
            get { return m_creator; }
            set { m_creator = value; }
        }

        ////[DataMapping("status")]
        public string status
        {
            get { return m_status; }
            set { m_status = value; }
        }

        ////[DataMapping("teamid")]
        public int Teamid
        {
            get { return m_teamid; }
            set { m_teamid = value; }
        }

        ////[DataMapping("leagueid")]
        public int Leagueid
        {
            get { return m_leagueid; }
            set { m_leagueid = value; }
        }
    }

    #endregion

    #region MatchInfoRow

    [Serializable]
    public class MatchInfoRow
    {
        //TODO: Integrate this class with any existing data object class

        private bool isneutral;
        private int m_awayid;
        private DateTime m_eventdate;
        private int m_homeid;
        private int m_leagueid;
        private string m_matchcode;

        public MatchInfoRow(string matchcode, int homeid, int awayid, DateTime eventdate, int leagueid, bool IsNeutral)
        {
            m_matchcode = matchcode;
            m_homeid = homeid;
            m_awayid = awayid;
            m_eventdate = eventdate;
            m_leagueid = leagueid;
            isneutral = IsNeutral;
        }

        public MatchInfoRow()
        {
            m_matchcode = null;
            m_homeid = 0;
            m_awayid = 0;
            m_eventdate = DateTime.Today;
            m_leagueid = 0;
            isneutral = false;
        }

        ////[DataMapping("matchcode")]
        public string matchcode
        {
            get { return m_matchcode; }
            set { m_matchcode = value; }
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

        ////[DataMapping("eventdate")]
        public DateTime eventdate
        {
            get { return m_eventdate; }
            set { m_eventdate = value; }
        }

        ////[DataMapping("leagueid")]
        public int leagueid
        {
            get { return m_leagueid; }
            set { m_leagueid = value; }
        }

        ////[DataMapping("IsNeutral")]
        public bool IsNeutral
        {
            get { return isneutral; }
            set { isneutral = value; }
        }
    }

    #endregion
}