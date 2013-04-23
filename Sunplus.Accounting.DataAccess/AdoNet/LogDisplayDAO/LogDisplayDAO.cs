using System;
using System.Linq;
using System.Text;
using Sunplus.Accounting.DataAccess.ILogDisplay;
using DNAide.Data;
using System.Collections;
using Sunplus.Accounting.Core.Configurations;
using System.Data;
using DNAide.Core;

namespace Sunplus.Accounting.DataAccess.AdoNet.LogDisplayDAO
{
    public class LogDisplayDAO : DataAccess, ILogDisplayDAO
    {
        public System.Data.DataTable GetLogDisplay(Hashtable param, string logType)
        {
            DbResult dbResult = null;
            string spMaping = string.Empty;
            switch (logType) 
            {
                case "setting":
                case "status":
                    spMaping = "GetLogSetting";
                    break;
                case "login":
                    spMaping = "GetLogLogin";
                    break;
                case "credit":
                    spMaping = "GetLogCredit";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(spMaping))
            {
               
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.LogDisplaySection.StoreCollection.Stores, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
                if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                    return dbResult.ResultSet.Tables[0];
            }
            return null;
        }

        public int GetLogDisplayTotal(Hashtable param, string logType)
        {
            DbResult dbResult = null; int total = 0;
            string spMaping = string.Empty;
            switch (logType)
            {
                case "setting":
                case "status":
                    spMaping = "GetLogSettingTotal";
                    break;
                case "login":
                    spMaping = "GetLogLoginTotal";
                    break;
                case "credit":
                    spMaping = "GetLogCreditTotal";
                    break;
                default:
                    break;
            }
            if (!string.IsNullOrEmpty(spMaping))
            {
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.LogDisplaySection.StoreCollection.Stores, spMaping);
                dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
                if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                {
                    DataRow[] rows = dbResult.ResultSet.Tables[0].Select();
                    if (rows.Count() > 0) { 
                        total = CastAide.AsInt32(rows[0].ItemArray[0].ToString()); 
                    }
                }
            }
            return total;
        }
    }
}
