using System;
using System.Collections;
using DNAide.Data;
using DNAide.Utils;
//using IBCUtils;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Accounting.Core.Extensions;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public sealed class MatchWinLostReport : DataAccessReport
    {
        public static StoreProcedureElement GetReportStoreProcedure(ReportSession.StoreCollection storeCollection, string alias)
        {
            var sps = ReportSession.Instance[storeCollection];
            return sps != null ? sps[alias] : null;
        }
        private MatchWinLostReport() { }
        /// <summary>
        /// Get Match Win/Loss content for Account MatchWinLoss report
        /// @PhuongDN
        /// Phuong.Nguyen updated
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DbResult RptMatchWinLoss(DateTime fromDate, DateTime toDate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("fromdate", FormatHandler.ResetStartDate(fromDate));
                p.Add("todate", FormatHandler.ResetEndDate(toDate));
                //Rpt_MatchWinLossDetailByMatch
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "Rpt_MatchWinLoss");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 120);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        /// <summary>
        /// Get Commission content for Account MatchWinLoss report
        /// @PhuongDN
        /// Phuong.Nguyen updated
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DbResult RptMatchWinLoss2(DateTime fromDate, DateTime toDate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("fromdate", FormatHandler.ResetStartDate(fromDate));
                p.Add("todate", FormatHandler.ResetEndDate(toDate));

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "Rpt_MasterMatchWinLoss2");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 120);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        /// <summary>
        /// Get Match PositionTalking content for Account MatchWinLoss report
        /// @PhuongDN
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DbResult MatchWinLostDetail(DateTime fromDate, DateTime toDate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("fromdate", FormatHandler.ResetStartDate(fromDate));
                p.Add("todate", FormatHandler.ResetEndDate(toDate));
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "MatchWinLostDetail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 120);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        /// <summary>
        /// Get currency Currency Win/Los Detail
        /// @PhuongDN
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DbResult RptCurrencyWinLossDetail(DateTime fromDate, DateTime toDate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("fromdate", FormatHandler.ResetStartDate(fromDate));
                p.Add("todate", FormatHandler.ResetEndDate(toDate));
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "CurrencyWinLossDetail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 120);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        /// <summary>
        ///Execute Rpt_MatchWinlossReportByBetType pocedure
        /// @PhuongDN
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DbResult RptMatchWinLoss2a(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);                
                p.Add("winlost_8rm", -1);
                p.Add("fi_3rdshare", -1);
                
                if (db.Equals("rep")) result = DbHelper.Execute("Acc_Rpt_MatchWinLoss2", p, DBType.REPORT, 600);//DatabaseType.ARCHIVE_1
                else if (db.Equals("main")) result = DbHelper.Execute("Acc_Rpt_MatchWinLoss2", p, DBType.MAIN, 600);//DatabaseType.MAIN


            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        /// <summary>
        /// @KhoaTran
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_WinLossByBettype(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate );
                p.Add("fi_3rdshare", 0);

                if (db.Equals("rep")) result = DbHelper.Execute("Acc_Rpt_WinLossByBettype", p, DBType.REPORT, 600);//DatabaseType.ARCHIVE_1
                else if (db.Equals("main")) result = DbHelper.Execute("Acc_Rpt_WinLossByBettype", p, DBType.MAIN, 600);//DatabaseType.MAINs

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        /// <summary>
        /// @KhoaTran
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_WinLossByBettype_14(DateTime wldate )
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);

                result = DbHelper.Execute("Acc_Rpt_WinLossByBettype14", p, DBType.REPORT, 600); //DatabaseType.ARCHIVE_1

            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        /// <summary>
        /// @Khoa.Tran
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_MatchWinLoss2_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);                
                p.Add("winlost_8rm", -1);
                p.Add("virtualwinlost_8rm", -1);
                p.Add("winlost_8rm_live", -1);
                p.Add("winlost_8rm_nonlive", -1);
                result = DbHelper.Execute("Acc_Rpt_MatchWinLoss214", p, DBType.REPORT, 600);//DatabaseType.ARCHIVE_1
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        /// <summary>
        /// Execute store procedure Rpt_AllCustomerWinLossBySiteFrom
        /// @PhuongDN
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="site"></param>
        /// <param name="betForm"></param>
        /// <returns></returns>
        public static DbResult RptAllCustomerWinLossBySiteFrom(DateTime fromDate, DateTime toDate, string site, string betForm)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("fromdate", FormatHandler.ResetStartDate(fromDate));
                p.Add("todate", FormatHandler.ResetEndDate(toDate));
                p.Add("site", site);
                p.Add("betfrom", betForm);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "AllCustomerWinLossBySiteFrom");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        /// <summary>
        /// @Khoa.Tran
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="site"></param>
        /// <param name="betForm"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_WinLostSummary(DateTime fromDate, DateTime toDate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("fromdate", (fromDate));
                p.Add("todate", (toDate));
                p.Add("_details", 0);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "WinLossSummaryReport");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        /// <summary>
        /// @Khoa.Tran   
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_DailyWLDirect_Sporttype(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", (wldate));                
                p.Add("linetotal", 0);
                p.Add("winlost_8rm", 0);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Sporttype");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Sporttype_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", (wldate));                
                p.Add("linetotal", 0);
                p.Add("winlost_8rm", 0);
                p.Add("winlost_barm", 0);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Sporttype_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }


        public static DbResult Acc_Rpt_DailyWLDirect_Sporttype_WH(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", (wldate));
                p.Add("linetotal", 0);
                p.Add("winlost_8rm", 0);
                p.Add("winlost_barm", 0);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Sporttype_WH");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        /// <summary>
        /// @Khoa.Tran
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_DailyWLDirect_MixParlay(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);

                var sp = BetListSection.Instance[BetListSection.StoreCollection.BetListStores]["Acc_Rpt_DailyWLDirect_MixParlay_MAIN"];

                if (ReportUtils.IsRepDB)
                {
                    sp = BetListSection.Instance[BetListSection.StoreCollection.BetListStores]["Acc_Rpt_DailyWLDirect_MixParlay_REPORT"];
                }

                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_MixParlay(DateTime wldate, int bettype)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                p.Add("bettype", bettype);

                /*if (ReportUtils.IsRepDB)
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Main]["Acc_Rpt_DailyWLDirect_MixParlay_REPORT"];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }
                else*/
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Main]["DailyWLDirect_MixParlay" + GetStoreMappingSubfix(wldate)];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_MixParlay_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);

                var sp = BetListSection.Instance[BetListSection.StoreCollection.BetListStores]["Acc_Rpt_DailyWLDirect_MixParlay14"];
                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_MixParlay_14(DateTime wldate, int bettype)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                p.Add("bettype", bettype);

                var sp = ReportSession.Instance[ReportSession.StoreCollection.Main]["Acc_Rpt_DailyWLDirect_MixParlay14"];
                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_MixParlay_WH(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);

                var sp = BetListSection.Instance[BetListSection.StoreCollection.BetListStores]["Acc_Rpt_DailyWLDirect_MixParlay_Punter"];

                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_MixParlay_WH(DateTime wldate, int bettype)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                p.Add("bettype", bettype);

                var sp = ReportSession.Instance[ReportSession.StoreCollection.Main]["Acc_Rpt_DailyWLDirect_MixParlay_Punter"];

                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        /// <summary>
        /// @Khoa.Tran
        /// Phuong.Nguyen updated
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="liveindicator"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_DailyWLDirect_OtherSport(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_othersport");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_OtherSport_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_othersport_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_OtherSport_WH(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_othersport_WH");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_OtherSport_Bettype(DateTime wldate, string sporttype)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                p.Add("sporttype", sporttype);
                p.Add("_details", 0);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_othersport_bettype");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_OtherSport_Bettype_14(DateTime wldate, string sporttype)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                p.Add("sporttype", sporttype);
                p.Add("_details", 0);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_othersport_bettype_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_OtherSport_Bettype_WH(DateTime wldate, string sporttype)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                p.Add("sporttype", sporttype);
                p.Add("_details", 0);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_othersport_bettype_WH");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Soccer_Bettype(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_soccer_bettype");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Soccer_Bettype_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);                
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_soccer_bettype_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Soccer_Bettype_WH(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_soccer_bettype_WH");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_HorseRacing(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);                
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_HorseRacing");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_HorseRacing_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_HorseRacing");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_HorseRacing_WH(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_HorseRacing_WH");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        /// <summary>
        /// @Khoa.Tran
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_DailyWLDirect_Soccer(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Soccer");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);//DatabaseType.MAIN
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Details(DateTime wldate, int liveindicator, int bettype, string sporttype, int IsHedge)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);                
                p.Add("sporttype", sporttype);
                p.Add("liveindicator", liveindicator);
                p.Add("bettype", bettype);
                p.Add("winlost_8rm", 0);
                p.Add("winlost_barm", 0);
                p.Add("ishedge", IsHedge);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Details_14(DateTime wldate, int liveindicator, int bettype, string sporttype, int IsHedge)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);                
                p.Add("sporttype", sporttype);
                p.Add("liveindicator", liveindicator);
                p.Add("bettype", bettype);
                p.Add("winlost_8rm", 0);
                p.Add("winlost_barm", 0);
                p.Add("ishedge", IsHedge);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Details_WH(DateTime wldate, int liveindicator, int bettype, string sporttype, int IsHedge)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("sporttype", sporttype);
                p.Add("liveindicator", liveindicator);
                p.Add("bettype", bettype);
                p.Add("winlost_8rm", 0);
                p.Add("winlost_barm", 0);
                p.Add("ishedge", IsHedge);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Detail_WH");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_HorseRacing_Details(DateTime wldate, int bettype)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);                
                p.Add("bettype", bettype);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_HorseRacing_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_HorseRacing_Details_14(DateTime wldate, int bettype)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);                
                p.Add("bettype", bettype);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_HorseRacing_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_HorseRacing_Details_WH(DateTime wldate, int bettype)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("bettype", bettype);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_HorseRacing_Detail_WH");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Finance_Details(DateTime wldate, int bettype)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);                
                p.Add("bettype", bettype);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Finance_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Finance_Details_14(DateTime wldate, int bettype)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);                
                p.Add("bettype", bettype);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Finance_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Finance_Details_WH(DateTime wldate, int bettype)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                p.Add("bettype", bettype);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Finance_Detail_WH");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        /// <summary>
        /// @Khoa.Tran
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DbResult Task_rptWinLost_UpdateTable(DateTime date)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", (date));
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "TaskWinLossUpdateTable");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);//DatabaseType.REPORT
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        /// <summary>
        /// @Khoa.Tran
        /// </summary>
        /// <param name="fdate"></param>
        /// <param name="tdate"></param>
        /// <param name="source"></param>
        /// <param name="sType"></param>
        /// <param name="bType"></param>
        /// <param name="live"></param>
        /// <returns></returns>
        public static DbResult Acc_Rpt_WinLostDetails(DateTime fdate, DateTime tdate, int source, int sType, int bType, int live)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("fromdate", fdate);
                p.Add("todate", tdate);
                p.Add("source", source);
                p.Add("sporttype", sType);
                p.Add("bettype", bType);
                p.Add("liveindicator", live);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLossDetail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);//DatabaseType.REPORT
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        /// <summary>
        /// Khoa Tran
        /// </summary>        
        public static DbResult WinlostHR_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HorseRacing_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHR_Details_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HorseRacing_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHR_Details_WH(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HorseRacing_Detail_WH");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHR_BetList_Details(int leagueid, string raceNo, int currency, DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HorseRacing_BetList_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHR_BetList_Details_14(int leagueid, string raceNo, int currency, DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HorseRacing_BetList_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHR_Group(DateTime fdate, DateTime tdate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("fromdate", fdate);
                p.Add("todate", tdate);                
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HorseRacing_Group");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHR_Group_Daily(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HorseRacing_Group_Daily");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFI_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FI_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFI_Details_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FI_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Finance(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);                
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Finance");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Finance_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Finance_14");
                result = DbHelper.Execute(spElement.Name , p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Finance_WH(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Finance_WH");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostCasino_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_CS_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostCasino_Details_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_CS_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostCasino_Details_WH(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("fromdate", wldate);
                p.Add("todate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_CS_Detail_WH");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);
                throw ex;

            }
            return result;
        }

        public static DbResult WinlostHR12BET_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HR12Bet_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
       
        public static DbResult WinlostHR12BET_Details_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HR12Bet_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHR12BET_BetList_Details(int leagueid, string raceNo, int currency, DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HR12Bet_Betlist_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHR12BET_BetList_Details_14(int leagueid, string raceNo, int currency, DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HR12Bet_Betlist_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Casino(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "DailyWLDirect_Casino_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Casino14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);                
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "DailyWLDirect_Casino_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Casino_WH(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", wldate);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "DailyWLDirect_Casino_Detail_WH");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult AutoDummyTransfer_Finance()
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("returnerrcode", 1);
                p.Add("errcode", 0);
                p.Add("errmsg", string.Empty);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Others, "AutoDummyTransfer_Finance");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHRMansion88_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HRM88_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHRMansion88_Details_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HRM88_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHRMansion_BetList_Details(int leagueid, string raceNo, int currency, DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HRM88_Betlist_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHRMansion_BetList_Details_14(int leagueid, string raceNo, int currency, DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HRM88_Betlist_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHR_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHR_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHR_Details_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHR_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHR_BetList_Details(int leagueid, string raceNo, int currency, DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHR_Betlist_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHR_BetList_Details_14(int leagueid, string raceNo, int currency, DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHR_Betlist_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHR12BET_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHR12Bet_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHR12BET_Details_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHR12Bet_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHR12BET_BetList_Details(int leagueid, string raceNo, int currency, DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHR12Bet_Betlist_Detail");
                result = DbHelper.Execute(spElement.Name  , p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHR12BET_BetList_Details_14(int leagueid, string raceNo, int currency, DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHR12Bet_Betlist_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHRMansion88_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHRMS88_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHRMansion88_Details_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHRMS88_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHRMansion_BetList_Details(int leagueid, string raceNo, int currency, DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHRMS88_Betlist_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHRMansion_BetList_Details_14(int leagueid, string raceNo, int currency, DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHRMS88_Betlist_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHRAB88_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HRAB88_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHRAB88_Details_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HRAB88_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHRAB88_BetList_Details(int leagueid, string raceNo, int currency, DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HRAB88_Betlist_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostHRAB88_BetList_Details_14(int leagueid, string raceNo, int currency, DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HRAB88_Betlist_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHRAB88_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHRAB88_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHRAB88_Details_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHRAB88_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHRAB88_BetList_Details(int leagueid, string raceNo, int currency, DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHRAB88_Betlist_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostFOddsHRAB88_BetList_Details_14(int leagueid, string raceNo, int currency, DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_FOddsHRAB88_Betlist_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostMiniGames_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_MiniGame_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinlostMiniGames_Details_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_MiniGame_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinLostBingo_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable {{"winlostdate", wldate}};
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_Bingo_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinLostBingo_Detail_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable {{"winlostdate", wldate}};
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_Bingo_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Bingo(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable {{"winlostdate", wldate}};
                
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Bingo");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Bingo_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();

                p.Add("winlostdate", wldate);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Bingo_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_Rpt_DailyWLDirect_Bingo_WH(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();

                p.Add("winlostdate", wldate);

                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.Main, "DailyWLDirect_Casino_Detail_WH");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinLostNumberGame_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable { { "winlostdate", wldate } };
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_Ng_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinLostNumberGame_Detail_14(DateTime wldate)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable { { "winlostdate", wldate } };
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_Ng_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult WinLostHRALog_Details(DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable { { "winlostdate", wldate } };
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HRAlog_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult WinLostHRALog_Details_14(DateTime wldate)
        {
            DbResult result = null; 
            try
            {
                var p = new Hashtable { { "winlostdate", wldate } };
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HRAlog_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult WinlostHRALog_BetList_Details(int leagueid, string raceNo, int currency, DateTime wldate, string db)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HRAlog_Betlist_Detail");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult WinlostHRALog_BetList_Details_14(int leagueid, string raceNo, int currency, DateTime wldate)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("leagueid", leagueid);
                p.Add("raceno", raceNo);
                p.Add("currency", currency);
                p.Add("winlostdate", wldate);
                var spElement = GetReportStoreProcedure(ReportSession.StoreCollection.WinLoss, "WinLoss_HRAlog_Betlist_Detail_14");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);                
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
    }
}
