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
    public sealed class ReportHR
    {
        #region WinlostHR12BET_Details

        public static DbResult WinlostHR12BET_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("winlostdate", wldate);
                string _DBType = DBType.MAIN;
                if (db.Equals("rep")) _DBType = DBType.REPORT;

                result = DbHelper.Execute("Acc_Rpt_WinlostHR12BET_Details", p, _DBType, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        #endregion

        #region WinlostHR12BET_Details_14

        public static DbResult WinlostHR12BET_Details_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("winlostdate", wldate);
                string _DBType = DBType.REPORT;
                result = DbHelper.Execute("Acc_Rpt_WinlostHR12BET_Details14", p, _DBType, 300);
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