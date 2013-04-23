using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using CasinoLibrary.Model.BaseBO;
using CasinoLibrary.Model.Common;
using DNAide.Data;
using JSoft.Utility;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    public static class CasinoSynchronise
    {
        
        private static CultureInfo cult = CultureInfo.CurrentCulture;

        /// <summary>
        /// Get Super List synchronised.
        /// AnhTuan@20090319
        /// </summary>
        /// <param name="..."></param>
        public static DbResult Acc_Rpt_CasinoSynchronise(string username, int searchby)
        {
            DbResult rs = null;
            try
            {
                var p = new Hashtable();
                p.Add("searchby", searchby);
                p.Add("keyword", username);
                var spElement = SpConfig.GetCasinoStoreProcedure("CasinoSynchronise");
                rs = DbHelper.Execute(spElement.Name, p, spElement.DbName, 180);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        /// <summary>
        /// Get synchronised list from Casino Admin DB.
        /// AnhTuan@20090319
        /// </summary>
        /// <param name="..."></param>
        public static DataTable AP_Casino_Synchronise(string username, int RoleID)
        {
            DataTable dtb = null;
            try
            {
                Dictionary<string, string> objPara = null;
                objPara = new Dictionary<string, string>();
                objPara.Add("Keyword", username);
                objPara.Add("RoleID", RoleID.ToString());

                var nInParameters = new SqlParameter[2];
                nInParameters[0] = objPara.ContainsKey("Keyword")
                                       ? new SqlParameter("@Keyword", StringUtil.CStr(objPara["Keyword"]))
                                       : new SqlParameter("@Keyword", SqlDbType.NVarChar);
                nInParameters[0].Direction = ParameterDirection.Input;
                nInParameters[1] = objPara.ContainsKey("RoleID")
                                       ? new SqlParameter("@RoleID", StringUtil.CInt(objPara["RoleID"]))
                                       : new SqlParameter("@RoleID", SqlDbType.TinyInt);
                nInParameters[1].Direction = ParameterDirection.Input;

                PersistenceMgr objMgr = null;
                var bs = new CustGameTypeBaseBO();
                objMgr = bs.GetDBManager(DBType.CS_BackupDB);
                dtb = objMgr.ExecSPbyParamsWithDataTable("AP_Casino_Synchronise", nInParameters);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dtb;
        }

        /// <summary>
        /// Update SyncCasino = 1.	
        /// Simon@20090704
        /// </summary>
        public static DbResult Acc_CustomerSyncUpd(int accid, int custid)
        {
            DbResult rs = null;
            try
            {
                var p = new Hashtable();
                p.Add("userid", accid);
                p.Add("custid", custid);
                rs = DbHelper.Execute("Acc_CustomerSyncUpd", p, DBType.MAIN, 120);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        private class SpConfig: DataAccess
        {
            public static StoreProcedureElement GetCasinoStoreProcedure(string alias)
            {
                var sps = CasinoSession.Instance[CasinoSession.StoreCollection.CasinoControl];
                return sps != null ? sps[alias] : null;
            }
        }
    }
}