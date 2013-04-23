using System;
using System.Collections;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.Provider.Report;
using Sunplus.Core.LoggingLib;

/// <summary>
/// Summary description for PunterAnalysisProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.Report
{
    public class DownLineBalanceProviderClient : DownLineBalanceProvider
    {
        protected Core.Configurations.StoreProcedureElement GetStoreProcedure(string aliasName)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Balance, aliasName);
        }
        public override DbResult RptMABalance(string db, int roleid, int custid, bool isYesterday, string username,
                                              string firstletter, string currency)
        {
            DbResult result = null;
            try
            {
                var p = new Hashtable();

                if (!string.IsNullOrEmpty(username) && !username.Equals("All"))
                    p.Add("keyword", username);

                if (!string.IsNullOrEmpty(firstletter) && !firstletter.Equals("All"))
                    p.Add("firstchar", firstletter);

                if (!string.IsNullOrEmpty(currency) && !currency.Equals("0"))
                    p.Add("currency", currency);

                //if (string.IsNullOrEmpty(db)) db = "rep";
                //string dbType = (string.Compare(db, "rep", true) == 0) ? DBType.REPORT : DBType.MAIN;

                var spElement = new Core.Configurations.StoreProcedureElement();

                if (roleid == 3) //master
                {
                    p.Add("superid", custid);
                    spElement = GetStoreProcedure(isYesterday ? "masterbalanceyesterday" : "masterbalance");
                }
                else if (roleid == 2) //Agent
                {
                    p.Add("masterid", custid);
                    spElement = GetStoreProcedure(isYesterday ? "agentbalanceyesterday" : "agentbalance");
                    
                }
                else if (roleid == 1) //member
                {
                    p.Add("agentid", custid);
                    spElement = GetStoreProcedure(isYesterday ? "memberbalanceyesterday" : "memberbalance");
                }

                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 600);
            }
            catch (Exception ex)
            {
                // error assigning the datareader value to a property
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }

            return result;
        }

        public override DbResult RptSABalance(string db, bool isYesterday, Hashtable p)
        {
            DbResult result = null;
            try
            {
                var spElement = GetStoreProcedure(isYesterday ? "superbalanceyesterday" : "superbalance");
                result = DbHelper.Execute(spElement.Name, p, spElement.DbName, 600);                
            }
            catch (Exception ex)
            {
                {
                    Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }

            return result;
        }

        public override DbResult BalanceCompHistory()
        {
            try
            {
                return DbHelper.Execute("Acc_Rpt_Balance_Company_History", DBType.LOG);
            }
            catch (InvalidOperationException invalidOEx)
            {
                throw invalidOEx;
            }
        }

        public override DbResult BalanceCompHistory(DateTime BalanceUpDay)
        {
            try
            {
                var param = new Hashtable();
                param.Add("balanceupday", BalanceUpDay);
                return DbHelper.Execute("Acc_Rpt_Balance_Company_History", param, DBType.LOG);
            }
            catch (InvalidOperationException invalidOEx)
            {
                throw invalidOEx;
            }
        }
    }
}