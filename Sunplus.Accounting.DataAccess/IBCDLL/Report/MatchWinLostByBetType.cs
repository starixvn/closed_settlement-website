using System;
using System.Collections;
using DNAide.Data;
using DNAide.Utils;
//using IBCUtils;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Report
{
    using System.Data;

    using Fanex.Data;

    public  class MatchWinLostByBetType
    {
        /// <summary>
        /// @Author: Simon20091210
        /// Content: 
        /// </summary>       
        public static DataSet RptMatchWinLostByBettype(DateTime fromdate, DateTime todate)
        {
            //DataResult rs = null;
            var rs = new DataSet();
            try
            {
                var param = new Hashtable();
                param.Add("fromdate", FormatHandler.ResetStartDate(fromdate)); //datetime
                param.Add("todate", FormatHandler.ResetEndDate(todate)); //int
                using (var obj = new ObjectDb("MatchWinLossByBetType", 600))
                {
                    obj.AssignParamValues(param);
                    rs = obj.ExecuteDataSet();
                }
                
            }
            catch (InvalidOperationException InvalidOEx)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(InvalidOEx);
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }


        public static DbResult NumberGameDailyBettypeWinloss(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                string _DBType = (ReportUtils.IsRepDB) ? DBType.REPORT : DBType.MAIN; //DatabaseType.ARCHIVE_1 : DatabaseType.MAIN
                result = DbHelper.Execute("Acc_Rpt_DailyWLDirect_NumberGame", p, _DBType, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult NumberGameDailyBettypeWinloss_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                string _DBType = DBType.REPORT;
                result = DbHelper.Execute("Acc_Rpt_DailyWLDirect_NumberGame14", p, _DBType, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult NumberGameDailyBettypeWinloss_WH(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                string _DBType = DBType.PUNTER;
                result = DbHelper.Execute("Acc_Rpt_DailyWLDirect_NumberGame", p, _DBType, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        
    }
}