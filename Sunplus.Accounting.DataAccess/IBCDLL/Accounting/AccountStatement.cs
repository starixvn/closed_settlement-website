using System;
using System.Collections;
using System.Data;
using DNAide.Data;
using Fanex.Data;
using IBCUtils;

using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public sealed class AccountStatement
    {
        private AccountStatement()
        {
        }

        /// <summary>
        /// Nguyen Thi Minh Thu code 02/08/2006
        /// call store procedure Rpt_AllStatement
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>
        /// <param name="fromdate">from date</param>
        /// <param name="todate">to date</param>
        /// <returns>dataresult</returns>
        public static DbResult Acc_AllStatement(DateTime fromdate, DateTime todate, int custid, string username,
                                                int allStatementNumber)
        {
            DbResult result = null;
            var p = new Hashtable();
            p.Add("custid", custid);
            p.Add("username", username);
            p.Add("fromdate", FormatHandler.ResetStartDate(fromdate));
            p.Add("todate", FormatHandler.ResetEndDate(todate));
            try
            {
                if (allStatementNumber == 1)
                {
                    var spElement = new BetListStoreProcedure().GetBetListStoreProcedure(BetListSection.StoreCollection.BetListStores, "AllStatement");
                    result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
                }
                else if (allStatementNumber == 2)
                {
                    p.Add("showuser", 0);

                    result = DbHelper.Execute("Acc_AllStatement_FULL2", p, DBType.WAREHOUSE, 300);
                }
                else if (allStatementNumber == 3)
                {
                    result = DbHelper.Execute("Acc_Statement", p, DBType.WAREHOUSE, 300);
                }
                else if (allStatementNumber == 4)
                {
                    result = DbHelper.Execute("Acc_AllStatement4Agent", p, DBType.REPORT, 300);
                }
                else if (allStatementNumber == 5)
                {
                    result = DbHelper.Execute("Acc_AllStatementCompany", p, DBType.REPORT, 300);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        //Henry added: 2009-10-21
        public static DbResult Acc_MemberStatement(DateTime fromdate, DateTime todate, int custid, DateTime BalanceUpDay,
                                                   bool IncludeAccBal)
        {
            DbResult result = null;
            var p = new Hashtable();
            p.Add("custid", custid);
            p.Add("fromdate", fromdate);
            p.Add("todate", todate);
            p.Add("balanceupday", BalanceUpDay);
            p.Add("includeaccbal", IncludeAccBal);

            try
            {
                var spElement = new BetListStoreProcedure().GetBetListStoreProcedure(Core.Configurations.StoreProcedureGroup.BetListSection.StoreCollection.BetListStores, "AllStatement_Full");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 120);

                //SqlDataReader reader = DbHelper.ExecuteReader("Acc_AllStatement_Full", p, DBType.REPORT, 120) as SqlDataReader;
                //DataSet ds = DbHelper.ConvertDataReaderToDataSet(reader);
                //result = new DbResult(null, ds);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        //Henry added: 2009-10-24
        public static DbResult Acc_DetailStatement(DateTime fromdate, DateTime todate, int custid)
        {
            DbResult result = null;
            var p = new Hashtable();
            p.Add("custid", custid);
            p.Add("fromdate", fromdate);
            p.Add("todate", todate);
            try
            {
                result = DbHelper.Execute("Acc_Statement", p, DBType.WAREHOUSE, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        //Henry added: 2009-10-24
        public static DbResult Acc_AuditStatement(int custid)
        {
            DbResult result = null;
            var p = new Hashtable();
            p.Add("custid", custid);
            try
            {
                result = DbHelper.Execute("Acc_AllStatement4Agent", p, DBType.REPORT, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public static DbResult Acc_CompanyStatement(DateTime fromdate, DateTime todate)
        {
            DbResult result = null;
            var p = new Hashtable();
            p.Add("fromdate", fromdate);
            p.Add("todate", todate);
            try
            {
                result = DbHelper.Execute("Acc_AllStatementCompany", p, DBType.REPORT, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>
        /// <param name="fdate"></param>
        /// <param name="transtype"></param>
        /// <returns></returns>
        public static DbResult Acc_AllStatementCompanyDetail(DateTime fdate, string transtype)
        {
            DbResult result = null;
            var p = new Hashtable();
            p.Add("fromdate", fdate);
            p.Add("transtype", transtype);
            try
            {
                result = DbHelper.Execute("Acc_AllStatementCompanyDetail", p, DBType.REPORT, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// Delete transaction(statement)
        /// </summary>
        /// Phuong.Nguyen updated
        /// <param name="refno"></param>
        /// <param name="custid"></param>
        /// <param name="userid"></param>
        public static IbcError Acc_BettransDelTransaction(long refno, int custid, int userid)
        {
            IbcError error = IbcError.SuccessResult;
            DbResult ds = null;
            var p = new Hashtable();
            p.Add("refno", refno);
            p.Add("custid", custid);
            p.Add("userid", userid);
            p.Add("errcode", 0);
            p.Add("errmsg", "");
            try
            {
                var spElement =
                    new TransactionStoreProcedure().GetTransactionStoreProcedure(
                        TransactionGroupSection.StoreCollection.Bettrans, "BettranDelTransaction");

                ds = DbHelper.ExecuteCmd(spElement.Name, p, spElement.DbName);
                if (ds != null)
                {
                    if (Convert.ToInt32(ds.Outputs["errcode"]) != 0)
                    {
                        error.ErrorCode = Convert.ToInt32(ds.Outputs["errcode"]);
                        error.ErrorMsg = Convert.ToString(ds.Outputs["errmsg"]);
                    }
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return error;
        }

        /// <summary>
        /// Nguyen Thi Minh Thu code 08/12/2006
        /// call store procedure Rpt_AgentStatement 
        /// this function support for AgentStatement.aspx
        /// </summary>
        /// Phuong.Nguyen updated
        /// <param name="custid">custid</param>
        /// <returns>dataresult</returns>
        public static DbResult RptAgentStatement(int custid)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();
                p.Add("custid", custid);
                var spElement = new BetListStoreProcedure().GetBetListStoreProcedure(BetListSection.StoreCollection.BetListStores, "AgentStatement");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        /// <summary>
        /// Nguyen Thi Minh Thu code 02/08/2006
        /// call store procedure Rpt_BetListByDayAll
        /// </summary>
        /// Phuong.Nguyen updated <2008-12-03>
        /// <param name="fromdate">fromdate </param>
        /// <param name="custid"> custid</param>
        /// <returns> dataresult</returns>
        public static DataSet RptBetListByDayAll(DateTime fromdate, int custid)
        {
            var result = new DataSet();
            try
            {
                var p = new Hashtable();
                p.Add("custid", custid);
                p.Add("fromdate", fromdate);

                using (ObjectDb db = new ObjectDb("BetListByDayAll"))
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

        private class BetListStoreProcedure : DataAccess {
            public Sunplus.Accounting.Core.Configurations.StoreProcedureElement GetBetListStoreProcedure(BetListSection.StoreCollection storeCollection, string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }
        }
        private class TransactionStoreProcedure : DataAccess
        {
            public Sunplus.Accounting.Core.Configurations.StoreProcedureElement GetTransactionStoreProcedure(TransactionGroupSection.StoreCollection storeCollection, string aliasName)
            {
                return GetStoreProcedure(storeCollection, aliasName);
            }
        }
    }
}