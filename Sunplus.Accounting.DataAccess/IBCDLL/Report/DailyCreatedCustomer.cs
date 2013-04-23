using System;
using System.Collections;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Report
{
    public sealed class DailyCreatedCustomer
    {
        /// <summary>
        /// @Author: Simon20091210
        /// Content: Get Daily Created Customers List
        /// </summary>
        public static DbResult GetDailyCreatedCustomer(Hashtable hash)
        {
            DbResult rs = null;
            try
            {
                rs = DbHelper.Execute("Acc_DailyCreatedCustomer", hash, DBType.REPORT);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// @Author: Simon20091210
        /// Content: View downline of Daily Created Customers List
        /// </summary>
        public static DbResult GetDailyCreatedCustomerDetails(Hashtable hash)
        {
            DbResult rs = null;
            try
            {
                rs = DbHelper.Execute("Acc_DailyCreatedCustomer_Details", hash, DBType.REPORT);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }
    }
}