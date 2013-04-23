using System;
using System.Collections;
using System.Collections.Generic;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.Provider.Report;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for LeagueWL
/// </summary>

namespace Sunplus.Accounting.DataAccess.Entity.SqlServer.Report
{
    public class LeagueWLProviderClient : LeagueWLProvider
    {
        public override List<LeagueSportWLDetails> LeagueSportWL(string winlostdate, ref double? winlost_8rm, string db)
        {
            var p = new Hashtable();
            p.Add("winlostdate", winlostdate);

            var output = new Hashtable();
            output.Add("winlost_8rm", winlost_8rm);

            List<LeagueSportWLDetails> lsSportWL =
                SqlDataContext.ExecuteReader<LeagueSportWLDetails>("Acc_Rpt_LeagueWL_Sporttype", p, db, ref output);
            return lsSportWL;
        }

        #region Acc_Rpt_LeagueWL_Sporttype14

        public override DbResult LeagueWL_Sporttype14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("winlost_8rm", 0);

                string _DBType = DBType.REPORT;

                result = DbHelper.Execute("Acc_Rpt_LeagueWL_Sporttype14", p, _DBType, 300);
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return result;
        }

        #endregion

        #region LeagueWL_Details14

        public override DbResult LeagueWL_Details14(DateTime wldate, int sporttype, int leagueid, int bettype,
                                                    bool liveindicator, bool ishedge)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("leagueid", leagueid);
                p.Add("sporttype", sporttype);
                p.Add("bettype", bettype);
                p.Add("liveindicator", liveindicator);
                p.Add("ishedge", ishedge);
                p.Add("winlost_8rm", 0);

                string dbtype = DBType.REPORT;

                result = DbHelper.Execute("Acc_Rpt_LeagueWL_Details14", p, dbtype, 300);
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return result;
        }

        #endregion

        #region LeagueWL_Bettype14

        public override DbResult LeagueWL_Bettype14(DateTime wldate, int sporttype, int leagueid)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("sporttype", sporttype);
                p.Add("leagueid", leagueid);
                p.Add("winlost_8rm", 0);

                string dbtype = DBType.REPORT;

                result = DbHelper.Execute("Acc_Rpt_LeagueWL_Bettype14", p, dbtype, 300);
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return result;
        }

        #endregion
    }

    [Serializable]
    public class LeagueSportWLDetails
    {
        //TODO: Integrate this class with any existing data object class

        private decimal commission;
        private decimal linetotal;
        private int m_leagueid;
        private int m_sporttype;
        private decimal position;
        private decimal turnover;
        private decimal winlost;

        public LeagueSportWLDetails(int sporttype, int leagueid, decimal WinLost, decimal Position, decimal Commission,
                                    decimal LineTotal, decimal Turnover)
        {
            m_sporttype = sporttype;
            m_leagueid = leagueid;
            winlost = WinLost;
            position = Position;
            commission = Commission;
            linetotal = LineTotal;
            turnover = Turnover;
        }

        public LeagueSportWLDetails()
        {
            m_sporttype = 0;
            m_leagueid = 0;
            winlost = 0;
            position = 0;
            commission = 0;
            linetotal = 0;
            turnover = 0;
        }

        //[DataMapping("sporttype")]
        public int sporttype
        {
            get { return m_sporttype; }
            set { m_sporttype = value; }
        }

        //[DataMapping("leagueid")]
        public int leagueid
        {
            get { return m_leagueid; }
            set { m_leagueid = value; }
        }

        //[DataMapping("WinLost")]
        public decimal WinLost
        {
            get { return winlost; }
            set { winlost = value; }
        }

        //[DataMapping("Position")]
        public decimal Position
        {
            get { return position; }
            set { position = value; }
        }

        //[DataMapping("Commission", 10)]
        public decimal Commission
        {
            get { return commission; }
            set { commission = value; }
        }

        //[DataMapping("LineTotal")]
        public decimal LineTotal
        {
            get { return linetotal; }
            set { linetotal = value; }
        }

        //[DataMapping("Turnover")]
        public decimal Turnover
        {
            get { return turnover; }
            set { turnover = value; }
        }
    }
}