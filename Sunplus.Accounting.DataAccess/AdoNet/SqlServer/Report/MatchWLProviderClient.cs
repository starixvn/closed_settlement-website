using System;
using System.Collections;
using DNAide.Data;
using IBCUtils;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.Provider.Report;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for MatchWLProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.Report
{
    using System.Data;

    using Fanex.Data;

    using Sunplus.Accounting.Core.Extensions;

    public class MatchWLProviderClient : MatchWLProvider
    {
        public override DbResult MatchWL14(DateTime wldate)
        {
            var param = new Hashtable();
            param.Add("winlostdate", wldate);
            param.Add("winlost_8rm", 0);
            param.Add("fi_3rdshare", 0);
            return DbHelper.Execute("Acc_Rpt_MatchWinLoss214", param, DBType.WAREHOUSE, 10000);
        }

        public override DbResult MatchWL_WH(DateTime wldate)
        {
            var param = new Hashtable();
            param.Add("fromdate", wldate);
            param.Add("todate", wldate);
            param.Add("winlost_8rm", 0);
            param.Add("fi_3rdshare", 0);
            return DbHelper.Execute("Acc_Rpt_MatchWinLoss2_Old", param, DBType.PUNTER, 10000);
        }

        public override DbResult MatchWL(DateTime wldate, string db)
        {
            string dbtype = DBType.MAIN;
            if (dbtype == "rep")
                dbtype = DBType.WAREHOUSE;
            else
                dbtype = DBType.ARCHIVE;

            var param = new Hashtable();
            param.Add("winlostdate", wldate);
            param.Add("winlost_8rm", 0);
            param.Add("fi_3rdshare", 0);

            return DbHelper.Execute("Acc_Rpt_MatchWinLoss2", param, DBType.WAREHOUSE, 1000);
        }

        public override DataSet MatchWLSingleBet(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var p = new Hashtable();
                p.Add("fromdate", FormatHandler.ResetStartDate(fromDate));
                p.Add("todate", FormatHandler.ResetEndDate(toDate));
                
                using (ObjectDb obj = new ObjectDb("MatchWinLossSingleBet", 600))
                {
                    obj.AssignParamValues(p);
                    return obj.ExecuteDataSet();
                }
            }
            catch (InvalidOperationException invalidOex)
            {
                LogExtensions.ExceptionHandler(invalidOex);
                return new DataSet();
            }
        }

        public override DbResult WinLossByBettype(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();

                p.Add("winlostdate", wldate);
                p.Add("fi_3rdshare", 0);

                if (db.Equals("rep"))
                    result = DbHelper.Execute("Acc_Rpt_WinLossByBettype", p, DBType.REPORT, 600);
                        //DatabaseType.ARCHIVE_1
                else if (db.Equals("main"))
                    result = DbHelper.Execute("Acc_Rpt_WinLossByBettype", p, DBType.MAIN, 600); //DatabaseType.MAINs
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public override DbResult WinLossByBettype14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();

                p.Add("winlostdate", wldate);

                result = DbHelper.Execute("Acc_Rpt_WinLossByBettype14", p, DBType.REPORT, 600); 
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public override DbResult WinLossByBettype_WH(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();

                p.Add("fromdate", wldate);
                p.Add("todate", wldate);

                result = DbHelper.Execute("Acc_Rpt_WinlossByBettype_Old", p, DBType.PUNTER, 600); 
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }
    }
}