using System;
using System.Collections;
using DNAide.Core;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using System.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.DataAccess.Report;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class ReportRacingDAO : DataAccessReport, IReportRacingDAO
    {
        private enum SpAlias
        {
            WinlostRacingTote,
            WinlostRacingToteDetail,
            WinlostRacingToteDumpFile,
            WinlostRacingBetlist,

            WinlostRacingTote_New,
            WinlostRacingToteDetail_New,
            WinlostRacingBetlist_New,
            
            DailyWLDirectRacing,
            DailyWLDirectRacingBySport,
            DailyWLDirectRacingBySportDetails,
            WinlostRacing_Group,
            WinlostRacing_Group_Daily,

            DailyWLDirect_Sporttype,
            DailyWLDirect_MixParlay,
            DailyWLDirect_NumberGame,

            WinlostRacingLicensee_SiteSel,
            WinlostRacingLicensee_Details,
            WinlostRacingLicensee_BetList_Details

            
        }

        public DataTable WinlostRacingTote(Enumerations.ReportType reportType, string winlossDate)
        {
            DbResult dbResult = null;

            Hashtable param = new Hashtable();
            param.Add("winlostdate", CastAide.AsDateTime(winlossDate));
            param.Add("reporttype", ((int)reportType));
            string strMapping = SpAlias.WinlostRacingTote.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(winlossDate));
          
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);
            
            dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult.ResultSet.Tables[0];
            else
                return null;
        }

        public DataTable WinlostRacingToteNew(Enumerations.ReportType reportType, string winlossDate)
        {
            DbResult dbResult = null;

            Hashtable param = new Hashtable();
            param.Add("winlostdate", CastAide.AsDateTime(winlossDate));
            param.Add("reporttype", ((int)reportType));
            string strMapping = SpAlias.WinlostRacingTote_New.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(winlossDate));

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);

            dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult.ResultSet.Tables[0];
            else
                return null;
        }


        public DataTable WinlostRacingTote_Details(string winlossDate, string poolCurrency, string partnerId)
        {
            DbResult dbResult = null;

            Hashtable param = new Hashtable();
            param.Add("winlostdate", CastAide.AsDateTime(winlossDate));
            param.Add("poolcurrency", poolCurrency);
            param.Add("partnerid", partnerId);

            string strMapping = SpAlias.WinlostRacingToteDetail.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(winlossDate));

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);

            dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult.ResultSet.Tables[0];
            else
                return null;
        }

        public DataTable WinlostRacingTote_DetailsNew(string winlossDate, string poolCurrency, string partnerId)
        {
            DbResult dbResult = null;

            Hashtable param = new Hashtable();
            param.Add("winlostdate", CastAide.AsDateTime(winlossDate));
            param.Add("poolcurrency", poolCurrency);
            param.Add("partnerid", partnerId);

            string strMapping = SpAlias.WinlostRacingToteDetail_New.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(winlossDate));

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);

            dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult.ResultSet.Tables[0];
            else
                return null;
        }


        public DataTable WinlostRacingToteDumpFiles(string winlossdate, int reportType)
        {
            DbResult dbResult = null;

            string strMapping = SpAlias.WinlostRacingToteDumpFile.ToString();

            //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Racing").Stores[strMapping];
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);
            var param = new Hashtable { { "winlostdate", winlossdate }, { "reporttype", reportType } };
            dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult.ResultSet.Tables[0];
            else
                return null;
        }


        public DataTable WinlostRacingToteBetlist(string winlossdate, string leagueid, string raceNo, string currency)
        {
            DbResult dbResult = null;

            string strMapping = SpAlias.WinlostRacingBetlist.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(winlossdate)); 

            //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Racing").Stores[strMapping];
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);
            Hashtable param = new Hashtable();
            param.Add("winlostdate", winlossdate);
            param.Add("leagueid", leagueid);
            param.Add("raceno", raceNo);
            param.Add("currency", currency); 
            dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult.ResultSet.Tables[0];
            else
                return null;
        }


        public DataTable WinlostRacingToteBetlistNew(string winlossdate, string leagueid, string raceNo, string currency)
        {
            DbResult dbResult = null;

            string strMapping = SpAlias.WinlostRacingBetlist_New.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(winlossdate));

            //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Racing").Stores[strMapping];
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);
            Hashtable param = new Hashtable();
            param.Add("winlostdate", winlossdate);
            param.Add("leagueid", leagueid);
            param.Add("raceno", raceNo);
            param.Add("currency", currency);
            dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult.ResultSet.Tables[0];
            else
                return null;
        }

        public DataTable DailyWLDirectRacing(string winlossdate)
        {
            DbResult dbResult = null;

            string strMapping = SpAlias.DailyWLDirectRacing.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(winlossdate)); 

            //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Racing").Stores[strMapping];
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);
            Hashtable param = new Hashtable();
            param.Add("winlostdate", winlossdate);
            
            dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult.ResultSet.Tables[0];
            else
                return null;
        }


        public DataTable DailyWLDirectRacingBySport(string sporttype, string winlossdate)
        {
            DbResult dbResult = null;

            string strMapping = SpAlias.DailyWLDirectRacingBySport.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(winlossdate));

            //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Racing").Stores[strMapping];
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);
            Hashtable param = new Hashtable();
            param.Add("winlostdate", winlossdate);
            param.Add("sporttype", sporttype);

            dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult.ResultSet.Tables[0];
            else
                return null;
        }


        public DataTable DailyWLDirectRacingBySportDetails(string winlossdate, string sporttype, string bettype)
        {
            DbResult dbResult = null;

            string strMapping = SpAlias.DailyWLDirectRacingBySportDetails.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(winlossdate));

            //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Racing").Stores[strMapping];
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);
            Hashtable param = new Hashtable();
            param.Add("winlostdate", winlossdate);
            param.Add("sporttype", sporttype);
            param.Add("bettype", bettype); 

            dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult.ResultSet.Tables[0];
            else
                return null;
        }


        public DataTable WinlostRacing_Group(string fromdate, string todate)
        {
            DbResult rsGroup = null, rsGroupDaily = null;
            DataTable dtGroup = new DataTable();
            Hashtable param = new Hashtable();
            string strMappingDaily = SpAlias.WinlostRacing_Group_Daily.ToString();
            string strMapping = SpAlias.WinlostRacing_Group.ToString();

            //var spElementDaily = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Racing").Stores[strMappingDaily];
            //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Racing").Stores[strMapping]; 
            var spElementDaily = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMappingDaily);
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);
            
            if(CastAide.AsDateTime(fromdate) == CastAide.AsDateTime(todate) && CastAide.AsDateTime(todate) == DateTime.Today)
            {
                param = new Hashtable();
                param.Add("winlostdate", fromdate);
                dtGroup = DbHelper.Execute(spElementDaily.Name, param, spElementDaily.DbName).ResultSet.Tables[0]; 
            }
            else
            {
                param = new Hashtable();
                param.Add("fromdate", fromdate);
                param.Add("todate", todate);
                rsGroup = DbHelper.Execute(spElement.Name, param, spElement.DbName); 
                if (CastAide.AsDateTime(todate) >= DateTime.Today)
                {
                    param = new Hashtable();
                    param.Add("winlostdate", todate);
                    rsGroupDaily = DbHelper.Execute(spElementDaily.Name, param, spElementDaily.DbName);
                }

                #region Combine group daily into group
                dtGroup = rsGroup.ResultSet.Tables[0];
                if (rsGroupDaily != null)
                {
                    DataTable dtGroupDaily = rsGroupDaily.ResultSet.Tables[0];
                    foreach (DataRow drGroupDaily in dtGroupDaily.Rows)
                    {
                        DataRow drGroup = dtGroup.NewRow();
                        for (int i = 0; i < drGroupDaily.ItemArray.Length; i++)
                        {
                            drGroup[i] = drGroupDaily[i];
                        }
                        dtGroup.Rows.Add(drGroup);
                    }
                }
                #endregion
            }
            return dtGroup;
        }

        public DbResult DailyWLDirect_Sporttype(string wldate)
        {
            DbResult dbResult = null;

            string strMapping = SpAlias.DailyWLDirect_Sporttype.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(wldate));

            //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Main").Stores[strMapping];
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Main, strMapping);
            Hashtable param = new Hashtable();

            param.Add("winlostdate", (wldate));
            param.Add("linetotal", 0);
            //param.Add("winlost_8rm", 0);
            //param.Add("winlost_barm", 0);
            //param.Add("fi_3rdshare", 0);

            dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult;
            else
                return null;
        }

        #region WinlostRacingLicensee_SiteSel
        public DataTable WinlostRacingLicensee_SiteSel()
        {
            DataTable dataTable = null;
            string strMapping = SpAlias.WinlostRacingLicensee_SiteSel.ToString() ;

            //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Racing").Stores[strMapping];
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);

            Hashtable hashtable = new Hashtable();
            hashtable.Add("productprefix", "RB");

            DbResult dbResult = DbHelper.Execute(spElement.Name, hashtable, spElement.DbName);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult.ResultSet.Tables[0];
            else
                return null;
            return dataTable;
        }
        #endregion

        #region WinlostRacingLicensee_Details
        public DataTable WinlostRacingLicensee_Details(string winlossdate, string site)
        {
            DataTable dataTable = null;
            string strMapping = SpAlias.WinlostRacingLicensee_Details.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(winlossdate));

            //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Racing").Stores[strMapping] ;
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);
            
            Hashtable hashtable = new Hashtable();
            hashtable.Add("winlostdate", winlossdate); 
            hashtable.Add("site", site);

            DbResult dbResult = DbHelper.Execute(spElement.Name, hashtable, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult.ResultSet.Tables[0];
            else
                return null;
            return dataTable;
            
        }
        #endregion

        #region WinlostRacingLicensee_BetList_Details
        public DataTable WinlostRacingLicensee_BetList_Details(string leagueid, string raceNo, string currency, DateTime winlostdate, string site)
        {
             DataTable dataTable = null;
            string strMapping = SpAlias.WinlostRacingLicensee_BetList_Details.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(winlostdate));

            //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Racing").Stores[strMapping] ;
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);
            
            Hashtable hashtable = new Hashtable();
            hashtable.Add("winlostdate", winlostdate);  
            hashtable.Add("site", site);
            hashtable.Add("leagueid", leagueid);
            hashtable.Add("raceno", raceNo);
            hashtable.Add("currency", currency);
            
            DbResult dbResult = DbHelper.Execute(spElement.Name, hashtable, spElement.DbName );
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult.ResultSet.Tables[0];
            else
                return null;
            return dataTable;
        }
        #endregion

        #region SelWinlostHR_Details
        public DataTable SelWinlostHR_Details(string wldate)
        {
            DbResult dbResult = null;

            string strMapping = "WinlostHR_Details" + GetStoreMappingSubfix(CastAide.AsDateTime(wldate));

            //var spElement = GetStoreProcedure(StoreProcedureSection.StoreCollection.Report, "Racing").Stores[strMapping];
            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Racing, strMapping);
            Hashtable param = new Hashtable();

            param.Add("winlostdate", (wldate));

            dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult.ResultSet.Tables[0];
            else
                return null;
        }
        #endregion

        #region IReportRacingDAO Members


        public DbResult DailyWLDirect_MixParlay(string winlossdate, int bettype)
        {
            string strMapping = SpAlias.DailyWLDirect_MixParlay.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(winlossdate));

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Main, strMapping);
            Hashtable param = new Hashtable();

            param.Add("winlostdate", winlossdate);
            param.Add("bettype", bettype);

            DbResult dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult;
            else
                return null;
        }

        #endregion

        #region IReportRacingDAO Members


        public DbResult DailyWLDirect_NumberGame(string winlossdate)
        {
            string strMapping = SpAlias.DailyWLDirect_NumberGame.ToString() + GetStoreMappingSubfix(CastAide.AsDateTime(winlossdate));

            var spElement = GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Main, strMapping);
            Hashtable param = new Hashtable();

            param.Add("winlostdate", winlossdate);
            

            DbResult dbResult = DbHelper.Execute(spElement.Name, param, spElement.DbName, 1000);
            if (dbResult != null && dbResult.ResultSet != null && dbResult.ResultSet.Tables.Count > 0)
                return dbResult;
            else
                return null;
        }

        #endregion
    }
  
}
