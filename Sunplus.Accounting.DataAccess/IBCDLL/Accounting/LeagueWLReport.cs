using System;
using System.Collections;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    /// <summary>
    /// Summary description for ReportHR
    /// </summary>
    public sealed class LeagueWLReport
    {
        #region Acc_Rpt_LeagueWL_Sporttype

        public static DbResult LeagueWL_Sporttype(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("winlost_8rm", 0);
                string _DBType = DBType.MAIN;
                if (db.Equals("rep")) _DBType = DBType.REPORT;

                result = DbHelper.Execute("Acc_Rpt_LeagueWL_Sporttype", p, DBType.LOCALTEST, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        #endregion

        #region Acc_Rpt_LeagueWL_Sporttype14

        public static DbResult LeagueWL_Sporttype14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("winlost_8rm", 0);

                string _DBType = DBType.REPORT;

                result = DbHelper.Execute("Acc_Rpt_LeagueWL_Sporttype14", p, DBType.LOCALTEST, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        #endregion
    }
}