using System;
using System.Collections;
using System.Web;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public sealed class AccountCredit
    {
        private static Core.Configurations.StoreProcedureElement GetStoreProcedure(string aliasName)
        {
            var sps = CustomerSession.Instance[CustomerSession.StoreCollection.CustomerControl];
            return sps != null ? sps[aliasName] : null;

        }

        /// <summary>
        /// call store CustomerSelCredit
        /// @author: Vu Thi Hong Hanh
        /// </summary>
        /// <param name="custid">custid</param>
        /// <returns></returns>
        /// Phuong.Nguyen updated
        public static DbResult CustomerSelCredit(int custid)
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custid);
                var spElement = GetStoreProcedure("CustomerSelCredit");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 300);
            }
            catch
            {
                throw;
            }
            return rs;
        }

        /// <summary>
        /// CITS
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="refno"></param>
        /// <param name="custId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static DbResult Acc_CreditTermSelGroupHeadline()
        {
            try
            {
                var hash = new Hashtable();
                var spElement = GetStoreProcedure("CreditTermSelGroupHeadline");
                return DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// CITS
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <returns></returns>
        public static DbResult Acc_CreditTermSelByGroupID(int groupid)
        {
            try
            {
                var hash = new Hashtable();
                hash.Add("groupid", groupid);
                var spElement = GetStoreProcedure("CreditTermSelByGroupID");
                return DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// CITS
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public static DbResult Acc_CreditTermUpdAddToGroup(string username, int groupid, int userid)
        {
            try
            {
                var hash = new Hashtable();
                hash.Add("username", username);
                hash.Add("groupid", groupid);
                hash.Add("userid", userid);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("language", "E");
                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);
                return DbHelper.ExecuteCmd("Acc_CreditTermUpdAddToGroup", hash, DBType.MAIN);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// CITS
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="username"></param>
        /// <param name="groupid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static DbResult Acc_CreditTermUpdRemoveFromGroup(string CustIDList, int groupid, int userid)
        {
            try
            {
                var hash = new Hashtable();
                hash.Add("custidlist", CustIDList);
                hash.Add("groupid", groupid);
                hash.Add("userid", userid);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("language", "E");
                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);
                return DbHelper.ExecuteCmd("Acc_CreditTermUpdRemoveFromGroup", hash, DBType.MAIN);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// CITS
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="groupid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static DbResult Acc_CreditTermUpdSetGroupHeadline(int custid, int groupid, int userid)
        {
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custid);
                hash.Add("groupid", groupid);
                hash.Add("userid", userid);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("language", "E");
                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);
                return DbHelper.ExecuteCmd("Acc_CreditTermUpdSetGroupHeadline", hash, DBType.MAIN);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// CITS
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="groupid"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static DbResult Acc_CreditTermSelByCustID(int custid)
        {
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custid);
                var spElement = GetStoreProcedure("CreditTermSelByCustID");
                return DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// @Khoa.Tran
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public static DbResult Acc_CreditTermSelCheckerList()
        {
            try
            {
                return DbHelper.Execute("Acc_CreditTermSelCheckerList", DBType.DB03);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// CITS
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        public static DbResult Acc_CreditTermSelGroupHeadlineByGroupID(int groupid)
        {
            try
            {
                var hash = new Hashtable();
                hash.Add("groupid", groupid);
                var spElement = GetStoreProcedure("CreditTermSelGroupHeadlineByGroupID");
                return DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }

        /// <summary>
        /// CITS
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custid"></param>
        /// <param name="creditgrade"></param>
        /// <param name="preagreeamount"></param>
        /// <param name="installmentamount"></param>
        /// <param name="installmentdate"></param>
        /// <param name="accountremark"></param>
        /// <param name="groupID"></param>
        /// <param name="IsGroupHeadline"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static DbResult Acc_CreditTermUpdCustomer(int custid, string creditgrade, double preagreeamount,
                                                         double installmentamount, string installmentdate,
                                                         string accountremark, int groupID, int IsGroupHeadline,
                                                         int UserID, int checkerid)
        {
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custid);
                hash.Add("creditgrade", creditgrade);
                hash.Add("preagreeamount", preagreeamount);
                hash.Add("installmentamount", installmentamount);

                hash.Add("installmentdate", installmentdate);
                hash.Add("accountremark", accountremark);
                hash.Add("groupid", groupID);
                hash.Add("isgroupheadline", IsGroupHeadline);

                hash.Add("userid", UserID);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("language", "E");
                hash.Add("checker", checkerid);
                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);
                var spElement = GetStoreProcedure("CreditTermUpdCustomer");
                return DbHelper.ExecuteCmd(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
        }
    }
}