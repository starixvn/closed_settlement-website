using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Sunplus.Accounting.DataAccess.Report
{
    public class ReportUtils : DataAccess
    {
        private static StoreProcedureElement GetReportStoreProcedure(ReportSession.StoreCollection storeCollection, string aliasName)
        {
            var sps = ReportSession.Instance[storeCollection];
            return sps != null ? sps[aliasName] : null;

        }
        #region IsRepDB
        /// <summary>
        /// if isRep = true then get data from Replication(DatabaseType.ARCHIVE_1)
        /// else then get data from Main(DatabaseType.MAIN)
        /// </summary>
        public static bool IsRepDB
        {
            get
            {

                string IsRep = HttpContext.Current.Request["db"];
                IsRep = string.IsNullOrEmpty(IsRep) ? "Rep" : IsRep;
                if (IsRep.Equals("Rep"))
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Rep or Main
        /// </summary>
        public static string DBType
        {
            get
            {
                if (IsRepDB) return "Rep";
                else return "Main";
            }
        }
        #endregion




        public static class BalanceUp
        {
            private static DateTime _balanceUpDay;
            private static DateTime _moveBetsDay;

            private static ICacheManager _cache;

            static BalanceUp()
            {
                _cache = EnterpriseLibraryContainer.Current.GetInstance<ICacheManager>();
            }

            private static void GetData()
            {
                if (_cache.GetData("BalanceUpDay") == null || _cache.GetData("MoveBetsDay") == null)
                {
                    DbResult dr = Age_BalDaySel();
                    DataRow r = dr.ResultSet.Tables[0].Rows[0];
                    _balanceUpDay = CastAide.AsDateTime(r["BalanceUpDay"]);
                    _moveBetsDay = CastAide.AsDateTime(r["MoveBetsDay"]);

                    _cache.Add("BalanceUpDay", _balanceUpDay, CacheItemPriority.None, null, new AbsoluteTime(TimeSpan.FromHours(2)));
                    _cache.Add("MoveBetsDay", _moveBetsDay, CacheItemPriority.None, null, new AbsoluteTime(TimeSpan.FromHours(2)));
                }
                else
                {
                    _balanceUpDay = CastAide.AsDateTime(_cache.GetData("BalanceUpDay"));
                    _moveBetsDay = CastAide.AsDateTime(_cache.GetData("MoveBetsDay"));
                }
            }

            public static DateTime BalanceUpDay
            {
                get
                {
                    GetData();
                    return _balanceUpDay;
                }
            }

            public static DateTime MoveBetsDay
            {
                get
                {
                    GetData();
                    return _moveBetsDay;
                }
            }

            private static DbResult Age_BalDaySel()
            {
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Others, "SelBalanceUpDay");
                return DbHelper.Execute(spElement.Name, spElement.DbName);
            }
        }
    }
}
