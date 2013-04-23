using System;
using System.Collections;
using System.Data;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.AgentReport
{
    public static class WinLoss
    {
        
        public static IDataReader CustomerSelAllWinLoss(int custId, DateTime fdate, DateTime tdate)
        {
            IDataReader dr = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custId);
                param.Add("fromdate", fdate);
                param.Add("todate", tdate);
                var spElement = SpConfig.GetReportStoreProcedure(ReportSession.StoreCollection.Main, "CustomerSelAllWinLoss");
                dr = DbHelper.ExecuteReader(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dr;
        }

        private class SpConfig
        {
            public static Core.Configurations.StoreProcedureElement GetReportStoreProcedure(ReportSession.StoreCollection storeCollection, string aliasName)
            {
                var sps = ReportSession.Instance[storeCollection];
                return sps != null ? sps[aliasName] : null;

            }
        }
    }
}