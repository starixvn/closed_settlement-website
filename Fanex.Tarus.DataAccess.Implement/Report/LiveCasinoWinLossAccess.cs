
namespace Fanex.Tarus.DataAccess.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using DNAide.Core;
    using Fanex.Data;
    using Fanex.Tarus.DataAccess.Interface.Report;
    using Fanex.Tarus.Entities.Report;

    public class LiveCasinoWinLossAccess : ILiveCasinoWinLossAccess
    {
        protected DateTime MovebetDate { get; set; }
        public LiveCasinoWinLossAccess()
        {
            var balanceDate = MoveBetDate();
            MovebetDate = balanceDate["MoveBetsDay"];

        }

        protected Dictionary<string, DateTime> MoveBetDate()
        {
            var arrdate = new Dictionary<string, DateTime>();
            using (ObjectDb objDB = new ObjectDb("SelBalanceUpDay"))
            {
                var ds = objDB.ExecuteDataSet();
                DataRow rs = ds.Tables[0].Rows[0];
                arrdate.Add("MoveBetsDay", CastAide.AsDateTime(rs["movebetsday"]));

            }
            return arrdate;
        }

        public DataSet GetLiveCasinoWinLoss(LiveCasinoWinLossParams Params)
        {
            using (ObjectDb objDB = new ObjectDb("LiveCasinoWinLoss_DW", 1200))
            {
                var result = new LiveCasinoWinLoss();
                result.TotalItemByReportType = new List<LiveCasinoWinLossDTO>();
                result.GrandTotalCurrency = new List<LiveCasinoWinLossDTO>();

                var list = new List<LiveCasinoWinLossDTO>();

                objDB.AddInParameter("fromdate", DbType.String, Params.FromDate);
                objDB.AddInParameter("todate", DbType.String, Params.ToDate);
                objDB.AddInParameter("username", DbType.String, Params.UserName);
                objDB.AddInParameter("excludeTest", DbType.Boolean, Params.ExcludeTest);
                objDB.AddInParameter("SiteID", DbType.Int32, Params.SiteID);
                objDB.AddInParameter("GameTypeID", DbType.Int32, Params.GameType);
                objDB.AddInParameter("GameTableID", DbType.Int32, Params.GameTable);
                objDB.AddInParameter("currency", DbType.Int32, Params.Currency);
                objDB.AddInParameter("ReportBy", DbType.Int32, Params.ReportBy);
                objDB.AddInParameter("MoveBetDay", DbType.String, MovebetDate);

                var ds = objDB.ExecuteDataSet();
                return ds;                
            }
        }

        public DataTable GetCompanyAllowPlayLiveCasino()
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("GameStatistics_LicenseeSel"))
            {
                var ds = objDB.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                    datatable = ds.Tables[0];
            }
            return datatable;
        }

        public DataTable GetCurrency()
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("GameStatistics_SelCurrency"))
            {
                var ds = objDB.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                    datatable = ds.Tables[0];
            }
            return datatable;
        }

        public DataTable GetGameTableByGameType(int gameType)
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("GameStatistics_GameTableSel"))
            {
                objDB.AddInParameter("GameTypeID", DbType.Int32, gameType);
                var ds = objDB.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                    datatable = ds.Tables[0];
            }
            return datatable;
        }

        public DataTable GetGameType()
        {
            var datatable = new DataTable();
            using (ObjectDb objDB = new ObjectDb("GameStatistics_GameTypeSel"))
            {
                var ds = objDB.ExecuteDataSet();
                if (ds != null && ds.Tables.Count > 0)
                    datatable = ds.Tables[0];
            }
            return datatable;
        }
    }
}
