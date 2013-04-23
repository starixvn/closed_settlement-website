using System;
using System.Collections;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Customer
{
    public class Select4Edit: DataAccess
    {
        private static Core.Configurations.StoreProcedureElement GetStoreProcedure(string aliasName)
        {
            var sps = CustomerSession.Instance[Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.CustomerSession.StoreCollection.CustomerControl];
            return sps != null ? sps[aliasName] : null;

        }

        /// <summary>
        /// Get super information for editting
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns> 
        public static DbResult GetSuperInfo(int custid)
        {
            DbResult ds = null; 
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);

                var spElement = new Select4Edit().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "CustomerSelCEditCustomer");

                ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2208
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public static DbResult GetSuperInfo_acc(int custid)
        {
            DbResult ds = null;
            try
            {
                var param = new Hashtable();
                param.Add("custid", custid);

                var spElement = new Select4Edit().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "CustomerSelCEditCustomer");

                ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Get master info
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custId"></param>
        /// <param name="sRecommend"></param>
        /// <returns></returns>
        public static DbResult GetMasterInfo(int custId, int sRecommend)
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("custid", custId);
                hash.Add("srecommend", sRecommend);

                var spElement = new Select4Edit().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "CustomerSelSEditCustomer");

                rs = DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// execute SP CustomerSelMEditCustomer to get data for editing an agent
        /// @PhuongDN
        /// Using DNAide's Lib
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="masterId"></param>
        /// <returns>dataresult</returns>
        public static DbResult SelAgentForEdit(int agentId, int masterId)
        {
            DbResult ds = null;
            try
            {
                var p = new Hashtable();
                p.Add("custid", agentId);
                p.Add("mrecommend", masterId);

                var spElement = new Select4Edit().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "CustomerSelMEditCustomer");
                

                ds = DbHelper.Execute(spElement.Name, p, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Nguyen Thi Minh Thu 
        /// 06/12/2006
        /// execute store procedure "CustomerSelEditCustomer"
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custid"></param>
        /// <returns></returns>
        public static DbResult CustomerSelEditCustomerRpt(object custid)
        {
            DbResult ds = null;
            var param = new Hashtable();
            try
            {
                param.Add("custid", custid);

                var spElement = new Select4Edit().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "SelEditCustomer");

                ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>       
        /// execute store procedure "Acc_CustomerSelMemberSearch_AutoSuspend" to get data for Suspend List.
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static DbResult GetSuspendCustomerList(Hashtable hash)
        {
            DbResult ds = null;
            try
            {
                var spElement = new Select4Edit().GetStoreProcedure(CustomerSession.StoreCollection.CustomerControl, "CustomerSelMemberSearch_AutoSuspend");
                ds = DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>       
        /// execute store procedure "Acc_CustomerSelMemberSearch_AutoSuspend_1" to get data for Suspend List.
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static DbResult GetSuspendCustomerList_1(Hashtable hash)
        {
            DbResult ds = null;
            try
            {
                var spElement = new Select4Edit().GetStoreProcedure(CustomerSession.StoreCollection.CustomerControl, "CustomerSelMemberSearch_AutoSuspend");
                ds = DbHelper.Execute(spElement.Name, hash, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>       
        /// CITS
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static DbResult acc_CustomerSelAgentInfo(Hashtable param)
        {
            DbResult ds = null;
            try
            {
                var spElement = GetStoreProcedure("CustomerSelAgentInfo");
                ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Using DNAide's library
        /// Tho.Nguyen updated: 12/1/2008
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DbResult CustomerSelCustomerInfo(Hashtable param)
        {
            DbResult ds = null;
            try
            {
                var spElement = GetStoreProcedure("CustomerSelCustomerInfo");

                ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }

        /// <summary>
        /// Using DNAide's Library
        /// Tho.Nguyen updated: 12/3/2008
        /// </summary>
        /// <param name="custId"></param>
        /// <returns></returns>
        public static DbResult TransferOptionSel(object custId)
        {
            DbResult ds = null;
            var param = new Hashtable();
            try
            {
                param.Add("custid", custId);
                param.Add("transferoption", "");
                var spElement = new Select4Edit().GetStoreProcedure(CustomerSession.StoreCollection.CustomerControl, "CustomerSelTransferOption");
                ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }


        public static DbResult VirtualRateSel(object custId)
        {
            DbResult ds = null;
            var param = new Hashtable();
            try
            {
                param.Add("custid", custId);
                var spElement = new Select4Edit().GetStoreProcedure(CustomerSession.StoreCollection.CustomerControl, "CustomerSelVirtualMultiplier");
                ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }


        public static DbResult EditSuperOnMemberSel(object custId, object agentId)
        {
            DbResult ds = null;
            var param = new Hashtable();
            try
            {
                param.Add("custid", custId);
                param.Add("agentid", agentId);

                var spElement = new Select4Edit().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "CustomerSelEditCustomer_SAg");

                ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }


        public static DbResult EditMasterOnMemberSel(object custId, object agentId)
        {
            DbResult ds = null;
            var param = new Hashtable();
            try
            {
                param.Add("custid", custId);
                param.Add("agentid", agentId);

                var spElement = new Select4Edit().GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.SportbookPtSession.StoreCollection.SportBooksPtControl, "CustomerSelEditCustomer_MAg");

                ds = DbHelper.Execute(spElement.Name, param, spElement.DbName);
                
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return ds;
        }
    }
}