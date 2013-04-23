using System;
using System.Collections;
using System.Data;
using DNAide.Core;
using DNAide.Data;
using Fanex.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for Betlist
/// </summary>
namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public class Betlist:DataAccess
    {
        public static StoreProcedureElement GetBetListStoreProcedure(string alias)
        {
            var sps = BetListSection.Instance[BetListSection.StoreCollection.BetListStores];
            return sps != null ? sps[alias] : null;
        }
        public static StoreProcedureElement GetReportStoreProcedure(ReportSession.StoreCollection storeCollection, string alias)
        {
            var sps = ReportSession.Instance[storeCollection];
            return sps != null ? sps[alias] : null;
        }
        
        public static DbResult M88_BetList(int custid, DateTime fromdate, DateTime todate, DateTime movebetsday, bool showsb, bool showhr)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", fromdate);
                p.Add("todate", todate);
                p.Add("movebetsday", movebetsday);
                p.Add("showsb", showsb);
                p.Add("showhr", showhr);
                var spElement = GetBetListStoreProcedure("M88_BetList");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult AB88_BetList(int custid, DateTime fromdate, DateTime todate, DateTime movebetsday, bool showsb, bool showhr)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", fromdate);
                p.Add("todate", todate);
                p.Add("movebetsday", movebetsday);
                p.Add("showsb", showsb);
                p.Add("showhr", showhr);
                var spElement = GetBetListStoreProcedure("Winloss_M88_BetList");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult BetList_12Bet(int custid, DateTime fromdate, DateTime todate, DateTime movebetsday, bool showsb, bool showhr)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", fromdate);
                p.Add("todate", todate);
                p.Add("movebetsday", movebetsday);
                p.Add("showsb", showsb);
                p.Add("showhr", showhr);
                var spElement = GetBetListStoreProcedure("Winloss_12Bet_BetList");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult ALog_BetList(int custid, DateTime fromdate, DateTime todate, DateTime movebetsday, bool showsb, bool showhr)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", fromdate);
                p.Add("todate", todate);
                p.Add("movebetsday", movebetsday);
                p.Add("showsb", showsb);
                p.Add("showhr", showhr);
                var spElement = GetBetListStoreProcedure("Winloss_ALog_BetList");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult M88_BetList_History(int custid, DateTime fromdate, DateTime todate, bool showsb, bool showhr)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", fromdate);
                p.Add("todate", todate);
                p.Add("showsb", showsb);
                p.Add("showhr", showhr);
                var spElement = GetBetListStoreProcedure("Winloss_M88_BetList_History");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult AB88_BetList_History(int custid, DateTime fromdate, DateTime todate, bool showsb, bool showhr)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", fromdate);
                p.Add("todate", todate);
                p.Add("showsb", showsb);
                p.Add("showhr", showhr);
                var spElement = GetBetListStoreProcedure("Winloss_AB88_BetList_History");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult BetList_History_12BET(int custid, DateTime fromdate, DateTime todate, bool showsb, bool showhr)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", fromdate);
                p.Add("todate", todate);
                p.Add("showsb", showsb);
                p.Add("showhr", showhr);
                var spElement = GetBetListStoreProcedure("Winloss_12Bet_BetList_History");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult ALog_BetList_History(int custid, DateTime fromdate, DateTime todate, bool showsb, bool showhr)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", fromdate);
                p.Add("todate", todate);
                p.Add("showsb", showsb);
                p.Add("showhr", showhr);
                var spElement = GetBetListStoreProcedure("Winloss_ALog_BetList_History");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult BetList_SelMixParlay(string refno)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("refno", refno);

                var sp = BetListSection.Instance[BetListSection.StoreCollection.BetListStores]["GetMixparlay"];

                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DataSet BetList_SelDetailCombinationMP(int custid, string refno, double transid, string betid, DateTime movebetday, DateTime balanceupday, ref double outStading)
        {
            var result = new DataSet();
            try
            {
                var p = new Hashtable();
                p.Add("refno", refno);
                p.Add("transid", transid);
                p.Add("betid", betid);
                p.Add("custid", custid);
                p.Add("movebetday", movebetday);
                p.Add("balanceupday", balanceupday);
                p.Add("outstanding", 0);
                
                using (ObjectDb db = new ObjectDb("GetCombinationMixparlay"))
                {
                    db.AssignParamValues(p);
                    result = db.ExecuteDataSet();
                    outStading = CastAide.AsDouble(db.GetParameterValue("outstanding"));
                }
                
            }
            catch (Exception ex)
            {
                Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult BetList_SelDetailCombinationMP(string refno)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("refno", refno);

                var sp = BetListSection.Instance[BetListSection.StoreCollection.BetListStores]["GetCombinationMixparlay"];

                result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult BetList_SelMatchResultParlay(string refno)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("refno", refno);
                var spElement = GetBetListStoreProcedure("Age_BetList_SelMatchResultParlay");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch
            {
                throw;
            }
            return result;
        }
        public static DbResult BetList_SBResults(int matchID)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("matchid", matchID);
                var spElement = GetBetListStoreProcedure("BetList_SelMatchResult");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Acc_BetList_SelBettype(DateTime fromdate, DateTime todate, int custid)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("fromdate", (fromdate));
                p.Add("todate", (todate));
                p.Add("custid", custid);
                p.Add("movebetsday", ReportUtils.BalanceUp.MoveBetsDay);
                p.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);
                var spElement = GetBetListStoreProcedure("BetList_SelBettype");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DataSet BetsSelBetList(DateTime fromdate, DateTime todate, int custid, int bettype)
        {
            var result = new DataSet();
            try
            {
                Hashtable p = new Hashtable();
                p.Add("fromdate", ((fromdate >= ReportUtils.BalanceUp.BalanceUpDay) ? fromdate : ReportUtils.BalanceUp.BalanceUpDay));
                p.Add("todate", (todate));
                p.Add("custid", custid);
                p.Add("bettype", bettype);
                p.Add("movebetsday", ReportUtils.BalanceUp.MoveBetsDay);
                using (ObjectDb db = new ObjectDb("BetsSelBetList", 180))
                {
                    db.AssignParamValues(p);
                    result = db.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
            return result;
        }
        public static DataSet BetsSelBetList_Old(DateTime fromdate, DateTime todate, int custid, int bettype)
        {
            var result = new DataSet();
            try
            {
                Hashtable p = new Hashtable();
                DateTime _tmpdate = ReportUtils.BalanceUp.BalanceUpDay.AddDays(-1);
                p.Add("fromdate", (fromdate));
                p.Add("todate", ((todate < _tmpdate) ? todate : _tmpdate));
                p.Add("custid", custid);
                p.Add("bettype", bettype);


                using (ObjectDb db = new ObjectDb("BetsSelBetList_Old", 180))
                {
                    db.AssignParamValues(p);
                    result = db.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DataSet BetsSelBetList_ByTransID(int custid, string transID)
        {
            var result = new DataSet();
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custid);
                p.Add("transid", transID);
                using (ObjectDb db = new ObjectDb("ByTransID", 180))
                {
                    db.AssignParamValues(p);
                    result = db.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;

            }
            return result;
        }
        public static DataSet BetListByDayAll(int custid, DateTime fromdate, int transTypeId)
        {
            var result = new DataSet();
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", IBCUtils.FormatHandler.ResetStartDate(fromdate));
                p.Add("transtype", transTypeId);

                using (ObjectDb db = new ObjectDb("BetListByDayAll", 180))
                {
                    db.AssignParamValues(p);
                    result = db.ExecuteDataSet();
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        public static DbResult BetList_SelMatchResultFinance(long transID)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("transid", transID);
                p.Add("balanceupday", ReportUtils.BalanceUp.BalanceUpDay);
                p.Add("movebetsday", ReportUtils.BalanceUp.MoveBetsDay);
                var spElement = GetBetListStoreProcedure("Age_BetList_SelMatchResultFinance");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 180);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        
        #region spondemo
        public static DbResult SpondemoBetList(int custId, DateTime fromDate, DateTime toDate, DateTime moveBetsDay, bool showSB, bool showHR)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custId);
                p.Add("fromdate", fromDate);
                p.Add("todate", toDate);
                p.Add("movebetsday", moveBetsDay);
                p.Add("showsb", showSB);
                p.Add("showhr", showHR);
                var spElement = GetBetListStoreProcedure("Winloss_Spondemo_BetList");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult SpondemoBetListHistory(int custId, DateTime fromDate, DateTime toDate, bool showSB, bool showHR)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custId);
                p.Add("fromdate", fromDate);
                p.Add("todate", toDate);
                p.Add("showsb", showSB);
                p.Add("showhr", showHR);
                var spElement = GetBetListStoreProcedure("Winloss_Spondemo_BetList_History");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        #endregion

        #region Dela
        public static DbResult DelaBetList(int custId, DateTime fromDate, DateTime toDate, DateTime moveBetsDay, bool showSB, bool showHR)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custId);
                p.Add("fromdate", fromDate);
                p.Add("todate", toDate);
                p.Add("movebetsday", moveBetsDay);
                p.Add("showsb", showSB);
                p.Add("showhr", showHR);
                var spElement = GetBetListStoreProcedure("Winloss_Dela_BetList");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult DelaBetListHistory(int custId, DateTime fromDate, DateTime toDate, bool showSB, bool showHR)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custId);
                p.Add("fromdate", fromDate);
                p.Add("todate", toDate);
                p.Add("showsb", showSB);
                p.Add("showhr", showHR);
                var spElement = GetBetListStoreProcedure("Winloss_Dela_BetList_History");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        #endregion

        #region TCL
        public static DbResult TLC_BetList(int custid, DateTime fromdate, DateTime todate, DateTime movebetsday, bool showsb, bool showhr)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", fromdate);
                p.Add("todate", todate);
                p.Add("movebetsday", movebetsday);
                p.Add("showsb", showsb);
                p.Add("showhr", showhr);
                var spElement = GetBetListStoreProcedure("Winloss_TLC_BetList");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult TLC_BetList_History(int custid, DateTime fromdate, DateTime todate, bool showsb, bool showhr)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", fromdate);
                p.Add("todate", todate);
                p.Add("showsb", showsb);
                p.Add("showhr", showhr);
                var spElement = GetBetListStoreProcedure("Winloss_TLC_BetList_History");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        #endregion

        #region Fun88
        public static DbResult Fun88_BetList(int custid, DateTime fromdate, DateTime todate, DateTime movebetsday, bool showsb, bool showhr)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", fromdate);
                p.Add("todate", todate);
                p.Add("movebetsday", movebetsday);
                p.Add("showsb", showsb);
                p.Add("showhr", showhr);
                var spElement = GetBetListStoreProcedure("Winloss_AB88_BetList");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }

        public static DbResult Fun88_BetList_History(int custid, DateTime fromdate, DateTime todate, bool showsb, bool showhr)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", fromdate);
                p.Add("todate", todate);
                p.Add("showsb", showsb);
                p.Add("showhr", showhr);
                var spElement = GetBetListStoreProcedure("Winloss_AB88_BetList_History");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
        #endregion
        /// <summary>
        /// Get Bingo Game Result
        /// </summary>
        /// <returns></returns>
        public static DataTable GetNumberGameResult(int matchID)
        {
            var result = new DataTable();
            try
            {
                var p = new Hashtable {{"matchid", matchID}};
                var spElement = GetBetListStoreProcedure("SelMatchResultNumberGame");
                var dbResult = DbHelper.Execute(spElement.Name, p, spElement.DbName, 180);
                if (dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                    result = dbResult.ResultSet.Tables[0];
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
    }
}
