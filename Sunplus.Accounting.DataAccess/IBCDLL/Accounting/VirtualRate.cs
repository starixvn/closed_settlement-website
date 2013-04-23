using System;
using System.Collections;
using System.Data;
using System.Data.OleDb;
using System.Web;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.IBCDLL.Accounting
{
    /// <summary>
    /// @Author: Simon20090820
    /// </summary>
    public sealed class VirtualRate
    {
        private static Core.Configurations.StoreProcedureElement GetStoreProcedure(string aliasName)
        {
            var sps = CustomerSession.Instance[CustomerSession.StoreCollection.CustomerControl];
            return sps != null ? sps[aliasName] : null;

        }
        public static DataTable GetDataFromExcel(string ExcelPath)
        {
            var dtb = new DataTable();
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelPath +
                             ";Extended Properties=Excel 8.0;";
            var excelCon = new OleDbConnection(strConn);
            try
            {
                excelCon.Open();
                var myCommand = new OleDbCommand("SELECT * FROM [Sheet1$]", excelCon);
                var objAdap = new OleDbDataAdapter();
                objAdap.SelectCommand = myCommand;
                objAdap.Fill(dtb);
                excelCon.Close();
                excelCon.Dispose();
                return dtb;
            }
            catch (Exception ex)
            {
                excelCon.Close();
                excelCon.Dispose();
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                throw ex;
            }
            return null;
        }

        public static DbResult InsDataTmpExcel(string arrUsernames, string arrVirtualRates)
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("userid", AppSession.AccId);
                hash.Add("arrusernames", arrUsernames);
                hash.Add("arrvirtualrates", arrVirtualRates);
                rs = DbHelper.Execute("Acc_VirtualRateMulInsTmpExcel", hash, DBType.WAREHOUSE);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        public static DbResult CheckData()
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("userid", AppSession.AccId);
                rs = DbHelper.Execute("Acc_VirtualRateMulSelCheck", hash, DBType.WAREHOUSE);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }

            return rs;
        }

        public static DbResult DelDataTmpExcel()
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("userid", AppSession.AccId);
                rs = DbHelper.Execute("Acc_VirtualRateMulDelTmpExcel", hash, DBType.WAREHOUSE);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        public static DbResult UpdateVirtualRateIntoCustinfo(string filename, int filesize, int index)
        {
            DbResult rs = null;
            try
            {
                var hash = new Hashtable();
                hash.Add("userid", AppSession.AccId);
                hash.Add("ip", HttpContext.Current.Request.UserHostAddress);
                hash.Add("filename", filename);
                hash.Add("filesize", filesize);
                hash.Add("index", index);
                rs = DbHelper.Execute("Acc_VirtualRateMulUpd", hash, DBType.MAIN);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }

        public static DbResult ViewVRHistory(Hashtable hash)
        {
            DbResult rs = null;
            try
            {
                var spElement = GetStoreProcedure("CustomerSelVirtualRateHistory");
                return DbHelper.Execute(spElement.Name, hash, spElement.DbName);
                
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return rs;
        }
    }
}