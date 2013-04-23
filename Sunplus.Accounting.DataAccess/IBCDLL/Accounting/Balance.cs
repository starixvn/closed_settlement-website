using System;
using System.Collections;
using System.Data;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public sealed class Balance
    {
        private static Core.Configurations.StoreProcedureElement GetExchangeStoreProcedure(string aliasName)
        {
            var sps = ExchangeRateSession.Instance[ExchangeRateSession.StoreCollection.ExchangeRateControl];

            return sps != null ? sps[aliasName] : null;

        }
        /// <summary>
        /// private constructor
        /// </summary>
        private Balance()
        {
        }


        /// <summary>
        /// call store CustomerPwdUpd 
        /// @author: Vu Thi Hong Hanh
        /// </summary>
        /// <param name="custId">custId</param>
        /// <param name="userPwd">userPwd</param>
        /// <param name="userId">userId</param>
        /// <returns></returns>
        public static Boolean CustomerPwdUpd(int custId, string userPwd, int userId)
        {
            Boolean isUpdate = false;
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custId);
                hash.Add("userpwd", userPwd);
                hash.Add("userid", userId);

                DbHelper.ExecuteCmd("Acc_CustomerPwdUpd", hash, DBType.MAIN);
                isUpdate = true;
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return isUpdate;
        }

        /// <summary>
        /// Execute SP ExchangeSelAccount
        /// @PhuongDN
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>
        /// <returns></returns>
        public static DbResult ExchangeSelAccount()
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                var spElement = GetExchangeStoreProcedure("EchangeSelAcc");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 300);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// Execute Sp ExchangeDel
        /// @PhuongDN
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="exid"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string[] ExchangeDel(int uid, int exid, string lang)
        {
            string[] res = {"", ""};
            try
            {
                var hash = new Hashtable();
                hash.Add("userid", uid);
                hash.Add("exchangeid", exid);
                hash.Add("language", lang);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                DbResult ds = null;

                ds = DbHelper.ExecuteCmd("Acc_ExchangeDel", hash, DBType.MAIN, 300);
                if (ds != null)
                {
                    res[0] = ds.Outputs["errcode"].ToString();
                    res[1] = ds.Outputs["errmsg"].ToString();
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return res;
        }

        /// <summary>
        /// Execute SP Exchange Ins
        /// @PhuongDN
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>
        /// <param name="actualrate"></param>
        /// <param name="creator"></param>
        /// <param name="currency"></param>
        /// <param name="uid"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string[] ExchangeIns(double minbet, double actualrate, string creator, string currency, int uid,
                                           string lang)
        {
            DbResult rs = null;
            string[] res = {"", "", ""};
            try
            {
                var hash = new Hashtable();
                hash.Add("userid", uid);
                hash.Add("exchangeid", -1);
                hash.Add("actualrate", actualrate);
                hash.Add("creator", creator);
                hash.Add("currency", currency);
                hash.Add("minbet", minbet);
                hash.Add("language", lang);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");

                rs = DbHelper.ExecuteCmd("Acc_ExchangeIns", hash, DBType.MAIN, 300);
                if (rs != null)
                {
                    res[0] = rs.Outputs["exchangeid"].ToString();
                    res[1] = rs.Outputs["errcode"].ToString();
                    res[2] = rs.Outputs["errmsg"].ToString();
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return res;
        }

        /// <summary>
        /// Execute SP Acc_ExchangeSelByID
        /// @PhuongDN
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>
        /// <param name="exid"></param>
        /// <returns></returns>
        public static string[] ExchangeSelByID(int exid)
        {
            DbResult rs = null;
            string[] res = {"", "", ""};
            try
            {
                var hash = new Hashtable();
                hash.Add("exchangeid", exid);
                var spElement = GetExchangeStoreProcedure("EchangeSelById");
                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName, 300);
                if (rs != null)
                {
                    if (rs.ResultSet.Tables[0].Rows.Count > 0)
                    {
                        res[0] = rs.ResultSet.Tables[0].Rows[0]["currency"].ToString();
                        res[1] = rs.ResultSet.Tables[0].Rows[0]["actualrate"].ToString();
                        res[2] = rs.ResultSet.Tables[0].Rows[0]["minbet"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return res;
        }

        /// <summary>
        /// Execute SP Acc_ExchangeUpd
        /// @PhuongDN
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 2/12/2008
        /// </summary>
        /// <param name="exid"></param>
        /// <param name="rate"></param>
        /// <param name="minbet"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static string[] ExchangeUpd(int exid, double rate, double minbet, int uid, string ip)
        {
            string[] res = {"", ""};
            try
            {
                var hash = new Hashtable();
                hash.Add("userid", uid);
                hash.Add("exchangeid", exid);
                hash.Add("actualrate", rate);
                hash.Add("minbet", minbet);
                hash.Add("errcode", 0);
                hash.Add("errmsg", "");
                hash.Add("ip", ip);
                DbResult ds = null;

                ds = DbHelper.ExecuteCmd("Acc_ExchangeUpd", hash, DBType.MAIN, 300);
                if (ds != null)
                {
                    res[0] = ds.Outputs["errcode"].ToString();
                    res[1] = ds.Outputs["errmsg"].ToString();
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return res;
        }

        /// <summary>
        /// Get Company balance report
        /// </summary>
        /// <returns></returns>
        public static DbResult CompanyBalance()
        {
            DbResult ds = null;
            try
            {
                ds = DbHelper.Execute("Acc_Rpt_Balance_Company", DBType.REPORT); // swich to DB06 to reduce loading
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>
        /// <returns></returns>
        public static IDataReader ExChangeRateSel()
        {
            IDataReader dbr = null;
            try
            {
                dbr = DbHelper.ExecuteReader("Acc_ExchangeRateSel", DBType.DB03);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw;
            }
            return dbr;
        }

        /// <summary>
        /// Execute SP Acc_ExchangeRateSelInvalid
        /// @DungTong 20081029
        /// Using DNAide's library
        /// Tho.Nguyen updated 12/2/2008
        /// </summary>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        public static IDataReader InvalidExChangeRateSel(DateTime fromdate, DateTime todate)
        {
            IDataReader dbr = null;
            var ht = new Hashtable();
            ht.Add("fromdate", fromdate);
            ht.Add("todate", todate);
            try
            {
                dbr = DbHelper.ExecuteReader("Acc_ExchangeRateSelInvalid", ht, DBType.DB03);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dbr;
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>
        /// <param name="ExRatFromId"></param>
        /// <param name="ExRatToId"></param>
        /// <param name="minActual"></param>
        /// <param name="maxActual"></param>
        /// <param name="UserId"></param>
        /// <param name="Ip"></param>
        /// <returns></returns>
        public static DbResult ExChangeRateIns(int ExRatFromId, int ExRatToId, double minActual, double maxActual,
                                               int UserId, string Ip)
        {
            DbResult dbr = null;
            var htb = new Hashtable();
            try
            {
                htb.Add("fromexchangeid", ExRatFromId);
                htb.Add("toexchangeid", ExRatToId);
                htb.Add("minactualrate", minActual);
                htb.Add("maxactualrate", maxActual);
                htb.Add("userid", UserId);
                htb.Add("ip", Ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", "");
                htb.Add("language", "E");


                dbr = DbHelper.ExecuteCmd("Acc_ExchangeRateIns", htb, DBType.DB03);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw;
            }
            return dbr;
        }

        public static DbResult ExChangeRateUpd(int ExRatFromId, int ExRatToId, double minActual, double maxActual,
                                               int UserId, string Ip)
        {
            DbResult dbr = null;
            var htb = new Hashtable();
            try
            {
                htb.Add("fromexchangeid", ExRatFromId);
                htb.Add("toexchangeid", ExRatToId);
                htb.Add("minactualrate", minActual);
                htb.Add("maxactualrate", maxActual);
                htb.Add("userid", UserId);
                htb.Add("ip", Ip);
                htb.Add("errcode", 0);
                htb.Add("errmsg", "");
                htb.Add("language", "E");


                dbr = DbHelper.ExecuteCmd("Acc_ExchangeRateUpd", htb, DBType.DB03);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw;
            }
            return dbr;
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>
        /// <param name="ExRateFromId"></param>
        /// <param name="ExRateToId"></param>
        /// <returns></returns>
        public static IDataReader ExChangeRateSelHis(int ExRateFromId, int ExRateToId)
        {
            IDataReader dbr = null;
            try
            {
                var htb = new Hashtable();
                htb.Add("fromexchangeid", ExRateFromId);
                htb.Add("toexchangeid", ExRateToId);


                dbr = DbHelper.ExecuteReader("Acc_ExchangeRateSelHistory", htb, DBType.DB03);
            }
            catch
            {
                throw;
            }
            return dbr;
        }

        /// <summary>
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/2/2008
        /// </summary>
        /// <returns></returns>
        public static DbResult ExchangeSelCurrency()
        {
            DbResult dbr = null;
            try
            {
                var spElement = GetExchangeStoreProcedure("ExchangeSel");

                dbr = DbHelper.Execute(spElement.Name, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dbr;
        }
    }


    public sealed class CustomerBalance
    {
        private static Core.Configurations.StoreProcedureElement GetCustomerStoreProcedure(string aliasName)
        {
            var sps = CustomerSession.Instance[CustomerSession.StoreCollection.CustomerControl];

            return sps != null ? sps[aliasName] : null;

        }
        /// <summary>
        /// Select Customer Balance for agent
        /// SP: Update Given Credit Successfully
        /// @viet hoang
        /// </summary>
        /// <param name="_custid"></param>
        /// <returns></returns>
        public static DbResult CustomerSelectClearBalance(int _custid)
        {
            try
            {
                var param = new Hashtable();
                param.Add("custid", _custid);
                var spElement = GetCustomerStoreProcedure("CustomerSelClearBalance");
                return DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Update Customer Balance for agent
        /// SP: Acc_CustomerInsClearBalance
        /// @viet hoang
        /// </summary>
        /// <param name="_accId"></param>
        /// <param name="_custId"></param>
        /// <param name="_custIdList"></param>
        /// <returns></returns>
        public static void CustomerInsertClearBalance(int _accId, int _custId, string _custIdList, bool ischeckall,
                                                      ref string errmsg, string ip)
        {
            try
            {
                var param = new Hashtable();
                param.Add("fromcustid", _custId);
                param.Add("tocustids", _custIdList);
                param.Add("userid", _accId);
                param.Add("language", "E");
                param.Add("errcode", 0);
                param.Add("errmsg", string.Empty);
                if (!ischeckall)
                {
                    param.Add("cusstatus", "");
                }
                else
                {
                    param.Add("cusstatus", "All");
                }
                param.Add("ip", ip);

                var spElement = GetCustomerStoreProcedure("CustomerInsClearBalance");
                DbResult dr = DbHelper.ExecuteCmd(spElement.Name, param, spElement.DbName, 120);
                if ((int) dr.Outputs["errcode"] > 0)
                    errmsg = dr.Outputs["errmsg"].ToString();
            }
            catch
            {
                throw;
            }
        }
    }
}