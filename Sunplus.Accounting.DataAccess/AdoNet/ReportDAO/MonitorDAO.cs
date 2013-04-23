using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Accounting.Entities.Report.Monitor;
using Sunplus.Core.LoggingLib;
using DNAide.Core;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class MonitorDAO : DataAccess, IMonitorDAO
    {
        private enum GetKeySpMapping
        {
            GetCurrenies,
            Monitor_SportSel,
            Monitor_BettypeSel,
            Monitor_LeagueSel,
            Monitor_Report,
            Monitor_ReportArchive
        }

        #region getSPMapping
        private StoreProcedureElement getSPMapping(GetKeySpMapping mapping)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Others, mapping.ToString());
        }

        
        #endregion

        #region GetCurrency
        public DataTable GetCurrency()
        {
            
            StoreProcedureElement spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.HelperSession.StoreCollection.HelperControl, GetKeySpMapping.GetCurrenies.ToString());

            //var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.HelperSession.StoreCollection.HelperControl, "GetCurrenies");

            DataTable dataTable = new DataTable();
            try
            {
                string db = spElement.DbName;
                string sp = spElement.Name;

                DbResult dbResult = DbHelper.Execute(sp, db);
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
            return dataTable;
        }
        #endregion

        #region GetSports
        public DataTable GetSports()
        {
            DataTable dataTable = null;
            try
            {
                var spElement = getSPMapping(GetKeySpMapping.Monitor_SportSel);
                var db = spElement.DbName;
                var sp = spElement.Name;

                DbResult dbResult = DbHelper.Execute(sp, db);
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
            return dataTable;
        }
        #endregion

        #region GetBettypes
        public DataTable GetBettypes(List<Items> option, int sporttype)
        {
            DataTable dataTable = null;
            try
            {
                var spElement = getSPMapping(GetKeySpMapping.Monitor_BettypeSel);
                var db = spElement.DbName;
                var sp = spElement.Name;

                Hashtable hashtable = new Hashtable();

                foreach (Items items in option)
                {
                    if(items.Key.Length>0)
                        hashtable.Add(items.Key,CastAide.AsInt16(items.Value) );
                }
                hashtable.Add("sporttype", sporttype);
                

                DbResult dbResult = DbHelper.Execute(sp,hashtable, db);
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
            return dataTable;
        }
        #endregion

        #region GetLeague
        public DataTable GetLeague(List<Items> option, int sportId, string fromdate, string todate, int currency, int bettype, int live)
        {
            DataTable dataTable = null;
            try
            {
                var spElement = getSPMapping(GetKeySpMapping.Monitor_LeagueSel);
                var db = spElement.DbName;
                var sp = spElement.Name;

                Hashtable hashtable = new Hashtable();

                foreach (Items items in option)
                {

                    if (items.Key.Length > 0)
                        hashtable.Add(items.Key,
                                      CastAide.AsInt16(option[0].Value) == 1 ? 1 : CastAide.AsInt16(items.Value));
                }

                hashtable.Add("sporttype", sportId);
                hashtable.Add("fromdate", fromdate);
                hashtable.Add("todate", todate);
                hashtable.Add("currency", currency);
                hashtable.Add("bettype", bettype);
                if (live > -1)
                {
                    hashtable.Add("live", live);
                }
                DbResult dbResult = DbHelper.Execute(sp,hashtable, db,1000);
                if (dbResult != null && dbResult.ResultSet.Tables.Count > 0)
                    dataTable = dbResult.ResultSet.Tables[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
            return dataTable;
        }
        #endregion

        #region GetMAMonitorReport
        public DataTable GetMAMonitorReport(Param param)
        {
            DataTable dataTable = null;
            try
            {
                var spElement = getSPMapping(GetKeySpMapping.Monitor_ReportArchive);
                var db = "";
                var sp = "";
                DataTable dtWH = null;
                Hashtable hashtable = new Hashtable();


                string fdate = param.fromdate;
                string tdate = param.todate;

                hashtable.Add("fromdate", param.fromdate);
                hashtable.Add("todate", param.todate);
                hashtable.Add("currency", param.currency);

                foreach (Items items in param.product)
                {
                    if(items.Key.Length>0)
                        hashtable.Add(items.Key, items.Value);
                }

                bool showsb = CastAide.AsBoolean(hashtable["showsb"]);
                if(showsb)
                    hashtable.Add("sporttype", param.sporttype);

                hashtable.Add("bettype", param.bettype);
                if(param.live<2 && param.live>-1)
                    hashtable.Add("live", param.live);
                if (param.usernames != "")
                {
                    hashtable.Add("usernames", param.usernames);
                }
                if (param.leagues != "")
                {
                    hashtable.Add("leagues", param.leagues);
                }
                hashtable.Add("reportby", param.reportby);

                if(CastAide.AsDateTime(param.fromdate) >= ReportUtils.BalanceUp.BalanceUpDay )//archive
                {
                    spElement = getSPMapping(GetKeySpMapping.Monitor_Report);
                    db = spElement.DbName;
                    sp = spElement.Name;
                    dataTable = DbHelper.Execute(sp, hashtable, db, 1000).ResultSet.Tables[0];
                }
                else //wh
                {
                    spElement = getSPMapping(GetKeySpMapping.Monitor_ReportArchive);
                    db = spElement.DbName;
                    sp = spElement.Name;
                    dataTable = DbHelper.Execute(sp, hashtable, db,1000).ResultSet.Tables[0]; 
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return null;
            }
            return dataTable;
        }
        #endregion


       
    }
}
