using System;
using System.Collections;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public sealed class Transfer:DataAccess
    {
        private static StoreProcedureElement GetCustomerStoreProcedure(string aliasName)
        {
            var sps = CustomerSession.Instance[CustomerSession.StoreCollection.CustomerControl];
            return sps != null ? sps[aliasName] : null;

        }
        /// <summary>
        /// call SP BettransInsTransferAdmin 
        /// @author:Vu Hanh
        /// </summary>
        /// <param name="fromCustId">fromCustId</param>
        /// <param name="toUserName">toUserName</param>
        /// <param name="transDate">transDate</param>
        /// <param name="amount">amount</param>
        /// <param name="creator">creator</param>
        /// <param name="lang">lang</param>
        /// <returns></returns>
        public static Boolean BettransInsTransferAdmin(int fromCustId, string toUserName, DateTime transDate,
                                                       double amount, int creator, string lang)
        {
            Boolean isUpdated = false;
            try
            {
                if (lang == null)
                {
                    throw new ArgumentNullException(lang);
                }
                var hash = new Hashtable();
                hash.Add("fromcustid", fromCustId);
                hash.Add("tousername", toUserName);
                hash.Add("transdate", transDate);
                hash.Add("amount", amount);
                hash.Add("creator", creator);
                if (lang.Equals("zh-CN"))
                {
                    hash.Add("language", "CS");
                }
                else if (lang.Equals("ja-JP"))
                {
                    hash.Add("language", "J");
                }
                else
                {
                    hash.Add("language", "E");
                }

                DbHelper.ExecuteCmd("BettransInsTransfer_Admin", hash, DBType.MAIN);
                isUpdated = true;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return isUpdated;
        }

        /// <summary>
        /// call SP CustomerSelMultipleTransfer
        /// @author:Vu Hanh
        /// Phuong.Nguyen updated
        /// </summary>
        /// <param name="custid">custId</param>
        /// <param name="closed">closed</param>
        /// <returns></returns>
        public static DbResult CustomerSelMultipleTransfer(int custId, int closed)
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custId);
                hash.Add("closed", closed);
                var spElement = GetCustomerStoreProcedure("CustomerSelMultipleTransfer");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// Insert Bettrans Transfer
        /// store procedure: Acc_BettransInsTransfer
        /// @viet hoang
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="_accId"></param>
        /// <param name="_fromCustId"></param>
        /// <param name="_toUser"></param>
        /// <param name="_exRate"></param>
        /// <param name="_amount"></param>
        /// <param name="_remark"></param>
        /// <param name="_transDate"></param>
        /// <param name="_ip"></param>
        /// <returns></returns>
        public static DbResult BettransInsertTransfer(int _accId, int _fromCustId, string _toUser, double _exRate,
                                                      double _amount, string _remark, string _ip)
        {
            DbResult dr = null;
            try
            {
                var p = new Hashtable();
                p.Add("userid", _accId); // int
                p.Add("fromcustid", _fromCustId); // int
                p.Add("tousername", _toUser); // nvarchar
                p.Add("exchangerate", _exRate); // smallmoney
                p.Add("amount", _amount); // money
                p.Add("remark", _remark); // nvarchar
                // p.Add("transdate", _transDate);     // date
                p.Add("language", "E"); // varchar
                //p.Add("yesterdaydata", _isYesterday); // bit
                p.Add("errcode", 0); // int
                p.Add("errmsg", string.Empty); // varchar
                p.Add("ip", _ip); // varchar

                var spElement = GetCustomerStoreProcedure("BettransInertTransfer");
                dr = DbHelper.Execute(spElement.Name, p, spElement.DbName, 120);
                if ((int) dr.Outputs["errcode"] > 0) throw new AccException(dr.Outputs["errmsg"].ToString());
            }
            catch (AccException accException)
            {
                throw accException;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dr;
        }

        /// <summary>
        /// Confirm Insert Bettrans Transfer 
        /// store procedure: Acc_BettransInsTransfer_Confirm
        /// @viet hoang
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="_fromUserName"></param>
        /// <param name="_toUser"></param>
        /// <param name="_exRate"></param>
        /// <returns></returns>
        public static DbResult BettransInsertTransfer_Confirm(string _fromUserName, string _toUser, double _exRate)
        {
            DbResult dr = null;
            try
            {
                var p = new Hashtable();
                p.Add("fromusername", _fromUserName);
                p.Add("tousername", _toUser);
                p.Add("exchangerate", _exRate);
                p.Add("fromcurrency", string.Empty);
                p.Add("tocurrency", string.Empty);
                p.Add("errcode", 0);
                p.Add("errmsg", string.Empty);
                p.Add("language", "E");

                dr = DbHelper.Execute("Acc_BettransInsTransfer_Confirm", p, DBType.MAIN, 120);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dr;
        }

        /// <summary>
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        /// </summary>
        /// <param name="custID"></param>
        /// <param name="agentName"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static DbResult TransferAllMember(int custID, string agentName, string ip)
        {
            var p = new Hashtable();
            p.Add("agentname", agentName);
            p.Add("userid", custID);

            p.Add("errcode", 0);
            p.Add("errmsg", string.Empty);
            p.Add("ip", ip);

            var spElement = GetCustomerStoreProcedure("TransferAllMembers");
            return DbHelper.ExecuteCmd(spElement.Name, p, spElement.DbName, 600);
        }

        public static DbResult TransferAllDownlines(int custID, string agentName, string ip)
        {
            var p = new Hashtable();
            p.Add("agentname", agentName);
            p.Add("userid", custID);

            p.Add("errcode", 0);
            p.Add("errmsg", string.Empty);
            p.Add("ip", ip);
            var spElement = GetCustomerStoreProcedure("TransferAllDownlines");
            return DbHelper.ExecuteCmd(spElement.Name, p, spElement.DbName, 600);
        }
    }
}