using System;
using System.Collections;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    /// <summary>
    /// @Author: Hoang, Minh Anh Tu
    /// </summary>
    public sealed class CompanyReport:DataAccess
    {
        private CompanyReport()
        {
        }
        public static StoreProcedureElement GetReportStoreProcedure(ReportSession.StoreCollection storeCollection, string alias)
        {
            var sps = ReportSession.Instance[storeCollection];
            return sps != null ? sps[alias] : null;
        }
        //public static StoreProcedureElement GetBetListStoreProcedure(string alias)
        //{
        //    var sps = BetListSection.Instance[BetListSection.StoreCollection.BetListStores];
        //    return sps != null ? sps[alias] : null;
        //}

        /// <summary>
        /// retrieve customer id and role id through 
        /// </summary>
        /// Phuong.Nguyen updated
        /// <param name="username">username string</param>
        /// <param name="custid">output parameters: customer id</param>
        /// <param name="roleid"></param>
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "2#"),
         SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        public static void CustomerSelByUserName(string username, out int custid, out int roleid)
        {
            custid = -1;
            roleid = -1;
            try
            {
                var p = new Hashtable();
                p.Add("username", username);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "CustomerSelByUserName");
                DbResult rs = DbHelper.Execute(spElement.Name, p, spElement.DbName);

                if (rs.ResultSet.Tables.Count > 0)
                {
                    if (rs.ResultSet.Tables[0].Rows.Count > 0)
                    {
                        DataRow r = rs.ResultSet.Tables[0].Rows[0];
                        custid = Convert.ToInt32(r["custid"], CultureInfo.CurrentCulture);
                        roleid = Convert.ToInt16(r["roleid"], CultureInfo.CurrentCulture);
                    }
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
        }

        public static DataTable GetHRMeetingList()
        {
            DbResult rs = null;
            try
            {

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Racing, "SelHRComm_Company");
                rs = DbHelper.Execute(spElement.Name, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            if (rs != null)
                return rs.ResultSet.Tables[0];
            return null;
        }

        public static DataTable GetHRMeetingPartnerList()
        {
            DbResult rs = null;
            try
            {
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Racing, "SelHRCommbyPartner");
                rs = DbHelper.Execute(spElement.Name, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw;
            }
            if (rs != null)
                return rs.ResultSet.Tables[0];
            return null;
        }

        public static DataTable GetHRMeetingPartnerList_Details(string PartnerType)
        {
            DbResult rs = null;
            var p = new Hashtable();
            p.Add("partnertype", PartnerType);
            try
            {
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Racing, "SelHRCommbyPartner_Details");
                rs = DbHelper.Execute(spElement.Name, p, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw;
            }
            if (rs != null)
                return rs.ResultSet.Tables[0];
            return null;
        }
    }
}