using System;
using System.Collections;
using System.Data;
using DNAide.Data;

namespace Sunplus.Accounting.DataAccess.IBCDLL
{
    /// <summary>
    /// Creater: Vinh
    /// Date: 03/19/08
    /// Using DNAide's Library
    /// Tho.Nguyen updated: 12/3/2008
    /// </summary>
    public class LogDisplay
    {
        public static DataSet GetLogData(string strUserName, string strOwner, DateTime fromdate, DateTime todate)
        {
            var hsParam = new Hashtable();
            if (!string.IsNullOrEmpty(strUserName))
                hsParam.Add("username", strUserName);
            hsParam.Add("fromdate", fromdate);
            hsParam.Add("todate", todate);
            return DbHelper.Execute("Acc_UserLogRunSPSel", hsParam, DBType.LOG, 600).ResultSet;
        }

        public static DataSet GetCreditLogData(string strUserName, string strOwner, DateTime fromdate, DateTime todate)
        {
            var hsParam = new Hashtable();
            if (!string.IsNullOrEmpty(strUserName))
                hsParam.Add("username", strUserName);
            hsParam.Add("fromdate", fromdate);
            hsParam.Add("todate", todate);
            return DbHelper.Execute("Acc_UserLogCreditSel", hsParam, DBType.LOG, 600).ResultSet;
        }
    }
}