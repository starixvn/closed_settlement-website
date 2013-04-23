using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNAide.Core;
using DNAide.Data;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.AdoNet.ReportDAO;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.Report.MatchWL
{
    public class MatchWL2aDAO: DataAccessReport, IMatchWL2aDAO
    {
        public DbResult MatchWL(DateTime wldate)
        {
            DbResult dbResult = null;
            try
            {
                var param = new Hashtable();
                param.Add("winlostdate", wldate);
                param.Add("winlost_8rm", 0);
                string strMapping = "MatchWinLoss2";

                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Main, strMapping);

                dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dbResult;

            
        }

        public DbResult MatchWL_WH(DateTime wldate)
        {
            DbResult dbResult = null;
            try
            {
                var param = new Hashtable();
                param.Add("fromdate", wldate);
                param.Add("todate", wldate);
                param.Add("winlost_8rm", 0);

                string strMapping = "MatchWinLoss2_WH";

                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Main, strMapping);
                dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dbResult;
        }

        public DbResult MatchWL14(DateTime wldate)
        {
            DbResult dbResult = null;
            try
            {
                var param = new Hashtable();
                param.Add("winlostdate", wldate);
                param.Add("winlost_8rm", 0);

                string strMapping = "MatchWinLoss2_14";

                //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Main").Stores[strMapping];
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Main, strMapping);
                dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dbResult;
        }

        public DbResult WinLossByBettype(DateTime wldate)
        {
            
            DbResult dbResult = null;
            try
            {
                var p = new Hashtable();
                p.Add("winlostdate", wldate);

                string strMapping = "WinlossByBettype";

                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Main, strMapping);
                dbResult = DbHelper.Execute(spElement.Name, p, spElement.DbName, 1000);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dbResult;
        }

        public DbResult WinLossByBettype_WH(DateTime wldate)
        {
            DbResult dbResult = null;
            try
            {
                var p = new Hashtable();

                p.Add("fromdate", wldate);
                p.Add("todate", wldate);

                string strMapping = "WinlossByBettype_WH";

                //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Main").Stores[strMapping];
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Main, strMapping);
                dbResult = DbHelper.Execute(spElement.Name, p, spElement.DbName, 1000);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dbResult;
        }

        public DbResult WinLossByBettype14(DateTime wldate)
        {
            DbResult dbResult = null;
            try
            {
                var p = new Hashtable();
                p.Add("winlostdate", wldate);

                string strMapping = "WinlossByBettype_14";

                //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Main").Stores[strMapping];
                var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Main, strMapping);
                dbResult = DbHelper.Execute(spElement.Name, p, spElement.DbName, 1000);
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
            }
            return dbResult;
        }

        #region IMatchWL2aDAO Members


        public DbResult MatchWLNEW(DateTime wldate)
        {
            string subfix = GetStoreMappingSubfix(wldate);
            string strMapping = "MatchWinLoss2" + subfix;


            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Main, strMapping);
            var param = new Hashtable();
            if (subfix.Contains("_WH"))
            {
                param.Add("fromdate", wldate);
                param.Add("todate", wldate);
                param.Add("winlost_8rm", 0);
            }
            else
            {
                param.Add("winlostdate", wldate);
                param.Add("winlost_8rm", 0);    
            }

            param.Add("winlost_barm",0);

            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
        }

        public DbResult WinLossByBettypeNEW(DateTime wldate)
        {
            string subfix = GetStoreMappingSubfix(wldate);
            string strMapping = "WinlossByBettype" + subfix;
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Main, strMapping);
            var param = new Hashtable();
            if (subfix.Contains("_WH"))
            {
                param.Add("fromdate", wldate);
                param.Add("todate", wldate);
            }
            else
            {
                param.Add("winlostdate", wldate);    
            }
            
            return DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
        }

        #endregion
    }
}
