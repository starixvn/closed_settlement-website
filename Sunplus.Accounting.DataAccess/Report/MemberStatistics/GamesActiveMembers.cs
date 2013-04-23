using System;
using DNAide.Data;

using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Common;
using System.Collections;

namespace Sunplus.Accounting.DataAccess.Report.MemberStatistics
{
    public class GamesActiveMembers :DataAccess, IGamesActiveMembers
    {
        private static Core.Configurations.StoreProcedureElement GetReportStoreProcedure(ReportSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = ReportSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;

        }

        public DbResult GetNumberOfActiveMembers(DateTime fromDate, DateTime toDate, DateTime balanceUpDay)
        {
            DbResult dbResult = null;
            try
            {
                var param = new Hashtable();
                param.Add("fromdate", fromDate);
                param.Add("todate", toDate);
                param.Add("balanceupday", balanceUpDay);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Gaming, "GamesActiveMember");
                dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dbResult;
        }
    }
}
