using System;
using System.Collections;
using System.Data;
using DNAide.Core;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class LicenseDAO : DataAccess, ILicenseDAO
    {
        public LicenseDAO()
        {
        }

        protected StoreProcedureElement GetStoreProcedure(string site)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Licensee, site);
        }

        public DataTable GetLicenseeList()
        {
            DataTable dataTable = new DataTable();

            StoreProcedureElement procedureElement = GetStoreProcedure("LicenseeSelAll");

            string spName = procedureElement.Name;
            string db = procedureElement.DbName;
            DbResult dbResult = DbHelper.Execute(spName, db);
            if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
            {
                dataTable = dbResult.ResultSet.Tables[0];
            }

            return dataTable;
        }

        public DbResult SelCurrencyByLicensee(string site)
        {
            DbResult dbResult = null;
            try
            {
                string sp = "", db = "";

                var spElement = GetStoreProcedure("SelCurrencyByLicensee");

                sp = spElement.Name;
                db = spElement.DbName;

                Hashtable h = new Hashtable();
                h.Add("licenseeid", site);
                 
                dbResult = DbHelper.Execute(sp, h, db, 200);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dbResult;
        }


        public DbResult SelReportByLicensee(int LicenseeID, int Currency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, string MoveBetsDay, bool IsOld, int drawfilter, int excludeTest, int BAFilter)
        {
            DbResult dbResult = null;
            try
            {
                Hashtable param = new Hashtable();
                string sp = "", db = "", alias = "";

                var spElement = GetStoreProcedure("LicWinloss");

                db = spElement.DbName;
                sp = spElement.Name;

                param.Add("licenseeid", LicenseeID);
                param.Add("currency", Math.Abs( Currency));
                param.Add("fromdate", FromDate);
                param.Add("todate", ToDate);
                param.Add("showsb", showSB);
                param.Add("showrb", ShowRB);
                param.Add("showng", ShowNG);
                param.Add("reporttype", ReportType);
                param.Add("movebetsday", MoveBetsDay);
                param.Add("isold", IsOld);
                param.Add("drawfilter", drawfilter);
                param.Add("excludetest",excludeTest);
                param.Add("bafilter", BAFilter);
                dbResult = DbHelper.Execute(sp, param, db, 600);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dbResult;
        }
        
        public DbResult SelBAWinLossByLincensee(int licenseeId, int currency, string fromDate, string toDate, int reportType, string moveBetsDay, int drawfilter, int excludeTest)
        {
            DbResult dbResult = null;
            try
            {
                var param = new Hashtable();
                string sp = "", db = "", alias = "";

                var spElement = GetStoreProcedure("BAWinLossByLicensee");

                db = spElement.DbName;
                sp = spElement.Name;

                param.Add("licenseeid", licenseeId);
                param.Add("currency", Math.Abs(currency));
                param.Add("fromdate", fromDate);
                param.Add("todate", toDate);
                param.Add("reporttype", reportType);
                param.Add("movebetsday", moveBetsDay);
                param.Add("drawfilter", drawfilter);
                param.Add("excludetest", excludeTest);
                dbResult = DbHelper.Execute(sp, param, db, 600);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dbResult;
        }
        

        public DataTable SelConcurrentUser(DateTime fromDate, DateTime toDate, string licenseSite)
        {
            var param = new Hashtable();
            param.Add("fromdate",fromDate);
            param.Add("todate",toDate);
            param.Add("licenseename", licenseSite);
            DataTable dtResult = null;
            var spElement = GetStoreProcedure("ConcurrentUserReport");
            var ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            if (ds != null && ds.ResultSet != null && ds.ResultSet.Tables.Count > 0)
                dtResult = ds.ResultSet.Tables[0];
            return dtResult;
        }




        public DbResult SelReportByLicenseeDetail_Ag(int LicenseeID, int Currency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ReportType, string MoveBetsDay, bool IsOld, int drawfilter, int excludeTest, int BAFilter)
        {
            DbResult dbResult = null;
            try
            {
                Hashtable param = new Hashtable();
                string sp = "", db = "", alias = "";

                var spElement = GetStoreProcedure("LicWinlossDetail_Ag");

                db = spElement.DbName;
                sp = spElement.Name;

                param.Add("licenseeid", LicenseeID);
                param.Add("currency", Math.Abs(Currency));
                param.Add("fromdate", FromDate);
                param.Add("todate", ToDate);
                param.Add("showsb", showSB);
                param.Add("showrb", ShowRB);
                param.Add("showng", ShowNG);
                param.Add("reporttype", ReportType);
                param.Add("movebetsday", MoveBetsDay);
                param.Add("isold", IsOld);
                param.Add("drawfilter", drawfilter);
                param.Add("excludetest", excludeTest);
                param.Add("bafilter", BAFilter);
                dbResult = DbHelper.Execute(sp, param, db, 600);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dbResult;
        }


        public DbResult SelReportByLicenseeDetail(int LicenseeID, int Currency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB,int ShowNG, int ReportType, string MoveBetsDay, bool IsOld, int drawfilter, int excludeTest, int BAFilter)
        {
            DbResult dbResult = null;
            try
            {
                Hashtable param = new Hashtable();
                string sp = "", db = "", alias = "";

                var spElement = GetStoreProcedure("LicWinlossDetail");

                db = spElement.DbName;
                sp = spElement.Name;

                param.Add("licenseeid", LicenseeID);
                param.Add("currency", Math.Abs(Currency));
                param.Add("custid", CustId);
                param.Add("fromdate", FromDate);
                param.Add("todate", ToDate);
                param.Add("showsb", showSB);
                param.Add("showrb", ShowRB);
                param.Add("showng", ShowNG);
                param.Add("reporttype", ReportType);
                param.Add("movebetsday", MoveBetsDay);
                param.Add("isold", IsOld);
                param.Add("drawfilter", drawfilter);
                param.Add("excludetest", excludeTest);
                param.Add("bafilter", BAFilter);
                dbResult = DbHelper.Execute(sp, param, db, 600);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dbResult;
        }


        public DbResult SelBetlistByLicensee(int CustId, string FromDate, string ToDate, string MoveBetsDay, string BalanceUpDay, bool ShowSB, bool ShowRB, bool ShowNG, int drawFilter, int baFilter)
        {
            DbResult dbResult = null;
            try
            {
                Hashtable param = new Hashtable();
                string sp = "", db = "";

                var spElement = GetStoreProcedure("LicWinlossDetailBetlist");

                db = spElement.DbName;
                sp = spElement.Name;

                Hashtable h = new Hashtable();
                h.Add("custid", CustId);
                h.Add("fromdate", FromDate);
                h.Add("todate", ToDate);
                h.Add("movebetsday", MoveBetsDay);
                h.Add("balanceupday", BalanceUpDay);
                h.Add("showsb", ShowSB);
                h.Add("showrb", ShowRB);
                h.Add("showng", ShowNG);
                h.Add("bafilter", baFilter);
               
                h.Add("drawfilter", drawFilter);
                h.Add("totalrows", 0);
                h.Add("sumwinlost", 0);
                h.Add("sumawinlost", 0);
                h.Add("sumcomm", 0);
                h.Add("sumwinlostusd", 0);
                h.Add("sumaWinlostusd", 0);
                h.Add("sumcommusd", 0);
                h.Add("sumwinlosteur", 0);
                h.Add("sumawinlosteur", 0);
                h.Add("sumcommeur", 0);

                

                dbResult = DbHelper.Execute(sp, h, db);

            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dbResult;
        }

        public DataTable SelLicenseeByReseller()
        {
            DataTable dataTable = null;
            try
            {
                Hashtable param = new Hashtable();
                string sp = "", db = "";

                var spElement = GetStoreProcedure("LicGetReseller");

                db = spElement.DbName;
                sp = spElement.Name;

                DbResult dbResult = DbHelper.Execute(sp, db);
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];

            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dataTable;
        } 

        public DataTable SelTotalTicket(int resellerid, int licenseeid, string date, string MoveBetsDay, string BalanceUpDay, bool showSB, bool showNG, bool showRB)
        {
            DataTable dataTable = null;
            try
            {
                Hashtable param = new Hashtable();
                string sp = "", db = "";

                var spElement = GetStoreProcedure("LicGetTotalTicket");

                db = spElement.DbName;
                sp = spElement.Name;

                Hashtable h = new Hashtable();

                bool IsOld = CastAide.AsDateTime(date) < new DateTime(2009, 01, 01);
                h.Add("resellerid", resellerid);
                h.Add("licenseeid", licenseeid);
                h.Add("showsb", showSB);
                h.Add("showng", showNG);
                h.Add("inputdate", date);
                h.Add("movebetsday", MoveBetsDay);
                h.Add("balanceupday", BalanceUpDay);
                h.Add("showrb", showRB);
                h.Add("isold", IsOld);

                DbResult dbResult = DbHelper.Execute(sp, h, db);
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];

            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dataTable;
        }


        public DataTable GetRates(string date)
        {
            DataTable dataTable = null;
            try
            {

                var spElement = GetStoreProcedure("LicGetResellerRates");

                var param = new Hashtable { { "inputdate", date } };

                var dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName);

                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0) dataTable = dbResult.ResultSet.Tables[0];

            }
            catch (Exception ex)
            {

                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dataTable;
        }

        public DataTable SelNegativeBalanceMember(string site)
        {
            DataTable dataTable = null;
            try
            {
                Hashtable param = new Hashtable();
                string sp = "", db = "";

                var spElement = GetStoreProcedure("LicNegativeBalanceMember");

                db = spElement.DbName;
                sp = spElement.Name;

                Hashtable h = new Hashtable();

                if(site.Length>0)
                    h.Add("site",  site);

                DbResult dbResult = DbHelper.Execute(sp, h, db);
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];

            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dataTable;
        }

        

        public DbResult SelReportByLicenseeV2(int LicenseeID, int Currency, string FromDate, string ToDate, int ShowSB, int ShowRB, int ShowNG, int ShowBA, int ReportType, string MoveBetsDay, bool IsOld, int drawfilter, int excludeTest)
        {
            DbResult dbResult = null;
            try
            {
                Hashtable param = new Hashtable();
                string sp = "", db = "", alias = "";

                var spElement = GetStoreProcedure("LicWinloss_new");

                db = spElement.DbName;
                sp = spElement.Name;

                param.Add("licenseeid", LicenseeID);
                param.Add("currency", Math.Abs(Currency));
                param.Add("fromdate", FromDate);
                param.Add("todate", ToDate);
                param.Add("showsb", ShowSB);
                param.Add("showrb", ShowRB);
                param.Add("showng", ShowNG);
                param.Add("showba", ShowBA);
                param.Add("reporttype", ReportType);
                param.Add("movebetsday", MoveBetsDay);
                param.Add("isold", IsOld);
                param.Add("drawfilter", drawfilter);
                param.Add("excludetest", excludeTest);
               
                dbResult = DbHelper.Execute(sp, param, db, 600);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dbResult;
        }

        public DbResult SelReportByLicenseeDetail_AgV2(int LicenseeID, int Currency, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ReportType, string MoveBetsDay, bool IsOld, int drawfilter, int excludeTest)
        {
            DbResult dbResult = null;
            try
            {
                Hashtable param = new Hashtable();
                string sp = "", db = "", alias = "";

                var spElement = GetStoreProcedure("LicWinlossDetail_AgV2");

                db = spElement.DbName;
                sp = spElement.Name;

                param.Add("licenseeid", LicenseeID);
                param.Add("currency", Math.Abs(Currency));
                param.Add("fromdate", FromDate);
                param.Add("todate", ToDate);
                param.Add("showsb", showSB);
                param.Add("showrb", ShowRB);
                param.Add("showng", ShowNG);
                param.Add("showba", ShowBA);
                param.Add("reporttype", ReportType);
                param.Add("movebetsday", MoveBetsDay);
                param.Add("isold", IsOld);
                param.Add("drawfilter", drawfilter);
                param.Add("excludetest", excludeTest);

                dbResult = DbHelper.Execute(sp, param, db, 600);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dbResult;
        }

        public DbResult SelReportByLicenseeDetailV2(int LicenseeID, int Currency, int CustId, string FromDate, string ToDate, int showSB, int ShowRB, int ShowNG, int ShowBA, int ReportType, string MoveBetsDay, bool IsOld, int drawfilter, int excludeTest)
        {
            DbResult dbResult = null;
            try
            {
                Hashtable param = new Hashtable();
                string sp = "", db = "", alias = "";

                var spElement = GetStoreProcedure("LicWinlossDetailV2");

                db = spElement.DbName;
                sp = spElement.Name;

                param.Add("licenseeid", LicenseeID);
                param.Add("currency", Math.Abs(Currency));
                param.Add("custid", CustId);
                param.Add("fromdate", FromDate);
                param.Add("todate", ToDate);
                param.Add("showsb", showSB);
                param.Add("showrb", ShowRB);
                param.Add("showng", ShowNG);
                param.Add("showba", ShowBA);
                param.Add("reporttype", ReportType);
                param.Add("movebetsday", MoveBetsDay);
                param.Add("isold", IsOld);
                param.Add("drawfilter", drawfilter);
                param.Add("excludetest", excludeTest);

                dbResult = DbHelper.Execute(sp, param, db, 600);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dbResult;
        }

        public DbResult SelBetlistByLicenseeV2(int CustId, string FromDate, string ToDate, string MoveBetsDay, string BalanceUpDay, bool ShowSB, bool ShowRB, bool ShowNG, int ShowBA, int drawFilter)
        {
            DbResult dbResult = null;
            try
            {
                Hashtable param = new Hashtable();
                string sp = "", db = "";

                var spElement = GetStoreProcedure("LicWinlossDetailBetlist2");

                db = spElement.DbName;
                sp = spElement.Name;

                Hashtable h = new Hashtable();
                h.Add("custid", CustId);
                h.Add("fromdate", FromDate);
                h.Add("todate", ToDate);
                h.Add("movebetsday", MoveBetsDay);
                h.Add("balanceupday", BalanceUpDay);
                h.Add("showsb", ShowSB);
                h.Add("showrb", ShowRB);
                h.Add("showng", ShowNG);
                h.Add("showba", ShowBA);

                h.Add("drawfilter", drawFilter);
                h.Add("totalrows", 0);
                h.Add("sumwinlost", 0);
                h.Add("sumwinlostusd", 0);
                h.Add("sumwinlosteur", 0);
                h.Add("sumlicenseewl", 0);
                h.Add("sumlicenseewlusd", 0);
                h.Add("sumlicenseewleur", 0);
                h.Add("sumcomm", 0);
                h.Add("sumcommusd", 0);
                h.Add("sumcommeur", 0);



                dbResult = DbHelper.Execute(sp, h, db, 1000);

            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dbResult;
        }


        public DbResult SelBABetListByLicensee(int custId, string fromDate, string toDate, string moveBetsDay, string balanceUpDay, int drawFilter)
        {
            DbResult dbResult = null;
            try
            {
                var param = new Hashtable();
                string sp = "", db = "";

                var spElement = GetStoreProcedure("BAWinLossDetailBetListByLicensee");

                db = spElement.DbName;
                sp = spElement.Name;

                var h = new Hashtable();
                h.Add("custid", custId);
                h.Add("fromdate", fromDate);
                h.Add("todate", toDate);
                h.Add("movebetsday", moveBetsDay);
                h.Add("balanceupday", balanceUpDay);

                h.Add("drawfilter", drawFilter);
                h.Add("totalrows", 0);
                h.Add("sumwinlost", 0);
                h.Add("sumwinlostusd", 0);
                h.Add("sumwinlosteur", 0);
                h.Add("sumlicenseewl", 0);
                h.Add("sumlicenseewlusd", 0);
                h.Add("sumlicenseewleur", 0);
                h.Add("sumcomm", 0);
                h.Add("sumcommusd", 0);
                h.Add("sumcommeur", 0);

                dbResult = DbHelper.Execute(sp, h, db, 1000);

            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dbResult;
        }

        public DbResult SelBAWinLossReportByLicenseeAgent(int licenseeId, int currency, string fromDate, string toDate, string moveBetsDay, int drawfilter, int excludeTest)
        {
            DbResult dbResult = null;
            try
            {
                var param = new Hashtable();
                string sp = "", db = "";

                var spElement = GetStoreProcedure("LicWinlossDetailAg");

                db = spElement.DbName;
                sp = spElement.Name;

                param.Add("licenseeid", licenseeId);
                param.Add("currency", Math.Abs(currency));
                param.Add("fromdate", fromDate);
                param.Add("todate", toDate);
                param.Add("movebetsday", moveBetsDay);
                param.Add("drawfilter", drawfilter);
                param.Add("excludetest", excludeTest);

                dbResult = DbHelper.Execute(sp, param, db, 600);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dbResult;
        }

        public DbResult SelBAWinLossByLicenseeDetail(int licenseeId, int currency, int custId, string fromDate, string toDate, string moveBetsDay, int drawfilter, int excludeTest)
        {
            DbResult dbResult = null;
            try
            {
                var param = new Hashtable();
                string sp = "", db = "";

                var spElement = GetStoreProcedure("BAWinLossByLicenseeDetail");

                db = spElement.DbName;
                sp = spElement.Name;

                param.Add("licenseeid", licenseeId);
                param.Add("currency", Math.Abs(currency));
                param.Add("custid", custId);
                param.Add("fromdate", fromDate);
                param.Add("todate", toDate);
                param.Add("movebetsday", moveBetsDay);
                param.Add("drawfilter", drawfilter);
                param.Add("excludetest", excludeTest);

                dbResult = DbHelper.Execute(sp, param, db, 600);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return dbResult;
        }


    }
}
