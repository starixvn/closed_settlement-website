using System;
using System.Collections;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.Provider.Report;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for LeagueWL
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.Report
{
    public class LeagueWLProviderClient : LeagueWLProvider
    {
        #region Acc_Rpt_LeagueWL_Sporttype

        public override DbResult LeagueWL_Sporttype(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("winlost_8rm", 0);

                result = DbHelper.Execute("Acc_Rpt_LeagueWL_Sporttype", p, DBType.REPORT, 300);
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

        #region Acc_Rpt_LeagueWL_Sporttype14

        public override DbResult LeagueWL_Sporttype14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("winlost_8rm", 0);

                result = DbHelper.Execute("Acc_Rpt_LeagueWL_Sporttype14", p, DBType.REPORT, 300);
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

        #region LeagueWL_Details

        public override DbResult LeagueWL_Details(DateTime wldate, int sporttype, int leagueid, int bettype,
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

                result = DbHelper.Execute("Acc_Rpt_LeagueWL_Details", p, DBType.REPORT, 300);
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

                result = DbHelper.Execute("Acc_Rpt_LeagueWL_Details14", p, DBType.REPORT, 300);
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

        #region LeagueWL_Bettype

        public override DbResult LeagueWL_Bettype(DateTime wldate, int sporttype, int leagueid)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("sporttype", sporttype);
                p.Add("leagueid", leagueid);
                p.Add("winlost_8rm", 0);

                result = DbHelper.Execute("Acc_Rpt_LeagueWL_Bettype", p, DBType.REPORT, 300);
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

                result = DbHelper.Execute("Acc_Rpt_LeagueWL_Bettype14", p, DBType.REPORT, 300);
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
}