using System;
using System.Collections;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public static class Transactions
    {
        /// <summary>
        /// Get Daily Transactions 
        /// store procedure: Acc_RptStatementDaily
        /// @viet hoang
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DbResult DailyTransactions(DateTime d, int pageIndex, ref int totalPages, string Type,
                                                 string Creator)
        {
            DbResult dr = null;
            try
            {
                var p = new Hashtable();
                p.Add("fromdate", d);
                p.Add("pageindex", pageIndex);
                p.Add("totalpages", totalPages);
                p.Add("type", Type);
                p.Add("creator", Creator);
                p.Add("pagesize", 100);

                dr = DbHelper.Execute("Acc_RptStatementDaily", p, DBType.WAREHOUSE, 120);
                totalPages = (int) dr.Outputs["totalpages"];
                return dr;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }
    }
}