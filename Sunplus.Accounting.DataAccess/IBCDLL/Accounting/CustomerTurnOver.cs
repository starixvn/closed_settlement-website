using System;
using System.Collections;
using DNAide.Data;
using IBCUtils;

using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    /// <summary>
    /// Nguyen Thi Minh Thu code
    /// </summary>
    public sealed class CustomerTurnOver
    {
        private CustomerTurnOver()
        {
        }

        /// <summary>
        /// call store procedure "Rpt_CustomerTurnOver"
        /// </summary>
        /// Phuong.Nguyen modified
        /// <param name="fromdate">fromdate</param>
        /// <param name="todate">todate</param>
        /// <returns>dataresult </returns>
        public static DbResult RptCustomerTurnOver(DateTime fromdate, DateTime todate)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("fromdate", FormatHandler.ResetStartDate(fromdate));
                p.Add("todate", FormatHandler.ResetEndDate(todate));

                result = DbHelper.Execute("Acc_Rpt_CustomerTurnover", p, DBType.WAREHOUSE, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }
    }
}