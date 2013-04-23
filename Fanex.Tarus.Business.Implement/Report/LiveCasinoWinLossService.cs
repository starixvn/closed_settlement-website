
namespace Fanex.Tarus.Business.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using DNAide.Core;
    using DNAide.Utils;
    using Fanex.Tarus.Business.Interface.Report;
    using Fanex.Tarus.DataAccess.Implement.Report;
    using Fanex.Tarus.DataAccess.Interface.Report;
    using Fanex.Tarus.Entities.Report;
    using Sunplus.Accounting.Core.Configurations;
    using Sunplus.Accounting.Core.Extensions;
    using ExcelUtil.Spreadsheet;
    using System.Drawing;

    public class LiveCasinoWinLossService : ILiveCasinoWinLossService
    {
        protected readonly ILiveCasinoWinLossAccess LiveCasinoWinLossAccess;
        private const string LiveCasinoWinLossSessionKey = "LiveCasinoWinLoss";
        private const string LiveCasnoWinLossMapedSessionKey = "LiveCasinoWinLossMapped";

        public LiveCasinoWinLossService(): this(null)
        {
        }

        public LiveCasinoWinLossService(ILiveCasinoWinLossAccess reportDataAccess)
        {
            LiveCasinoWinLossAccess = reportDataAccess ?? new LiveCasinoWinLossAccess();
        }

        void SortList<T>(List<T> dataSource, Sorting sorting)
        {
            PropertyInfo propInfo = typeof(T).GetProperty(sorting.SortCol);
            if (propInfo == null)
                return;
            bool isAsc = false;
            if (sorting.SortType.ToLower().Equals("asc"))
                isAsc = true;
            else
                isAsc = false;
            if (!sorting.IsDecimal)
            {
                if (sorting.IsDate)
                {
                    Comparison<T> compare = delegate(T a, T b)
                    {
                        object valueA = isAsc ? CastAide.AsDateTime(propInfo.GetValue(a, null)) : CastAide.AsDateTime(propInfo.GetValue(b, null));
                        object valueB = isAsc ? CastAide.AsDateTime(propInfo.GetValue(b, null)) : CastAide.AsDateTime(propInfo.GetValue(a, null));

                        return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
                    };
                    dataSource.Sort(compare);
                }
                else
                {
                    Comparison<T> compare = delegate(T a, T b)
                    {
                        object valueA = isAsc ? (propInfo.GetValue(a, null)) : (propInfo.GetValue(b, null));
                        object valueB = isAsc ? (propInfo.GetValue(b, null)) : (propInfo.GetValue(a, null));

                        return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
                    };
                    dataSource.Sort(compare);
                }
            }
            else
            {
                Comparison<T> compare = delegate(T a, T b)
                {
                    object valueA = isAsc ? CastAide.AsDecimal(propInfo.GetValue(a, null)) : CastAide.AsDecimal(propInfo.GetValue(b, null));
                    object valueB = isAsc ? CastAide.AsDecimal(propInfo.GetValue(b, null)) : CastAide.AsDecimal(propInfo.GetValue(a, null));

                    return valueA is IComparable ? ((IComparable)valueA).CompareTo(valueB) : 0;
                };
                dataSource.Sort(compare);
            }
        }
        
        public LiveCasinoWinLoss GetLiveCasinoWinLossData(LiveCasinoWinLossParams Params)
        {
            try
            {
                var LiveCasinoWinLoss = new LiveCasinoWinLoss();
                LiveCasinoWinLoss data;
                var items = new LiveCasinoWinLoss();
                #region get data
                data = MappingLivecasinoAgencyWinLossData(Params);
                List<LiveCasinoWinLossDTO> itemFilter = null;
                itemFilter = (from q in data.TotalItemByReportType select q).ToList();
                #endregion

                //Get Data Paging

               
                LiveCasinoWinLoss.CountItems = data.TotalItemByReportType.Count;
                LiveCasinoWinLoss.TotalCurrency = new List<LiveCasinoWinLossDTO>();

                #region add items to session
                items.TotalItemByReportType = itemFilter.Select(c => c).ToList();
                items.GrandTotalCurrency = data.GrandTotalCurrency;
                SessionExtension.AddSession(HttpContext.Current.Session, LiveCasinoWinLossSessionKey, items);
                var currencyList = (from q in items.TotalItemByReportType select q.CurrencyName).Distinct().ToList();
                #endregion

                if (LiveCasinoWinLoss.CountItems > 0)
                {
                    switch (Params.ReportBy)
                    {
                        case (int)FilterGameCondition.Daily:
                        case (int)FilterGameCondition.Monthly:
                        case (int)FilterGameCondition.GameType:
                        case (int)FilterGameCondition.GameTable:
                        case (int)FilterGameCondition.Agency:
                            LiveCasinoWinLoss.TotalItemByReportType = GetLiveCasinoWinLossGroup(data.TotalItemByReportType);
                            break;
                        case (int)FilterGameCondition.Username:
                            LiveCasinoWinLoss.TotalItemByReportType = data.TotalItemByReportType;
                            break;
                       
                    }
                }

                #region Total

                LiveCasinoWinLoss.TotalCurrency = data.TotalCurrency;
                LiveCasinoWinLoss.GrandTotalCurrency = data.GrandTotalCurrency;
                LiveCasinoWinLoss.RoleID = data.RoleID;
                LiveCasinoWinLoss.CountItems = LiveCasinoWinLoss.TotalItemByReportType.Count;
                #endregion
                return LiveCasinoWinLoss;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                if (ex.Message.ToLower().Contains("timeout")) {
                    return new LiveCasinoWinLoss()
                    {
                        CountItems = -1
                    };
                }
                
            }
            return new LiveCasinoWinLoss();
        }

        public LiveCasinoWinLoss GetLiveCasinoWinLoss(LiveCasinoWinLossParams Params)
        {
            try
            {
                LiveCasinoWinLoss getData = null;
                if (Params.FromCache)
                {
                    var getDataFromcache = SessionExtension.GetSession(HttpContext.Current.Session, LiveCasnoWinLossMapedSessionKey);
                    if (getDataFromcache != null)
                    {
                        getData = (LiveCasinoWinLoss)getDataFromcache;
                    }
                    else
                    {
                        getData = GetLiveCasinoWinLossData(Params);
                        SessionExtension.AddSession(HttpContext.Current.Session, LiveCasnoWinLossMapedSessionKey, getData);
                    }
                }
                else
                {
                    getData = GetLiveCasinoWinLossData(Params);
                    SessionExtension.AddSession(HttpContext.Current.Session, LiveCasnoWinLossMapedSessionKey, getData);
                }


                var LiveCasinoWinLoss = new LiveCasinoWinLoss();
                if (getData != null && getData.CountItems > 0)
                {
                    List<LiveCasinoWinLossDTO> items = getData.TotalItemByReportType;

                    if (Params.Sort.SortType.Length > 0)
                    {

                        if (Params.ReportBy == (int)FilterGameCondition.Username ||
                            Params.Sort.SortCol.ToLower().Equals("reporttype"))
                        {
                            SortList(items, Params.Sort);
                        }
                        else
                        {
                            #region sort all items - after add to group
                            var getDataNotMapped = (LiveCasinoWinLoss)SessionExtension.GetSession(HttpContext.Current.Session,
                                                                               LiveCasinoWinLossSessionKey);
                            if (getDataNotMapped != null)
                            {
                                List<LiveCasinoWinLossDTO> sortingItems = getDataNotMapped.TotalItemByReportType;
                                SortList(sortingItems, Params.Sort);
                                items = GetLiveCasinoWinLossGroup(sortingItems);
                            }
                            #endregion
                        }
                    }

                    var pagingItems = items.Select(c => c).Skip(Params.PageSize * (Params.PageIndex - 1)).Take(
                            Params.PageSize).ToList();
                    LiveCasinoWinLoss.TotalItemByReportType = pagingItems;
                    LiveCasinoWinLoss.GrandTotalCurrency = getData.GrandTotalCurrency;
                    LiveCasinoWinLoss.TotalCurrency = getData.TotalCurrency;
                    LiveCasinoWinLoss.CountItems = getData.CountItems;
                    LiveCasinoWinLoss.RoleID = getData.RoleID;
                }

                return LiveCasinoWinLoss;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return new LiveCasinoWinLoss();
        }
        
        #region private Methods
        private List<LiveCasinoWinLossDTO> GetLiveCasinoWinLossGroup(List<LiveCasinoWinLossDTO> list)
        {
            var a = new List<LiveCasinoWinLossDTO>();
            var items = new List<LiveCasinoWinLossDTO>();
            List<string> listDaily = list.Select(s => s.ReportType).Distinct().ToList();
            foreach (string daily in listDaily)
            {
                var item = new LiveCasinoWinLossDTO();
                item.ReportType = daily;
                item.Items = new List<LiveCasinoWinLossDTO>();
                item.Items.AddRange(list.Where(s => s.ReportType == daily).ToList());
                items.Add(item);
            }
            return items;
        }

        private string GetBettypeName(int ReportBy, DataRow Row)
        {
            var result = string.Empty;
            switch (ReportBy)
            {
                case (int)FilterGameCondition.Daily:
                    result = CastAide.AsDateTime(Row["Date"]).ToString("MM/dd/yyyy");
                    break;
                case (int)FilterGameCondition.Monthly:
                    result = CastAide.AsDateTime(Row["Month"]).ToString("yyyy/MM");
                    break;
                case (int)FilterGameCondition.Username:
                    result = CastAide.AsString(Row["username"]);
                    break;
                case (int)FilterGameCondition.GameType:
                    result = RemotingRef.RefData.GetBetTypeName(CastAide.AsString(Row["GameType"]), "E");
                    break;
                case (int)FilterGameCondition.GameTable:
                    result = RemotingRef.RefData.GetLeagueName(CastAide.AsString(Row["GameTable"]), "E");
                    break;
                case (int)FilterGameCondition.Agency:
                    result = CastAide.AsString(Row["username"]);
                    break;
                default:
                    result = CastAide.AsString(Row["username"]);
                    break;
            }
            return result;
        }

        private LiveCasinoWinLoss MappingLivecasinoAgencyWinLossData(LiveCasinoWinLossParams Params)
        {
            var result = new LiveCasinoWinLoss();
            result.TotalItemByReportType = new List<LiveCasinoWinLossDTO>();
            result.GrandTotalCurrency = new List<LiveCasinoWinLossDTO>();

            var list = new List<LiveCasinoWinLossDTO>();
            var agencyWinLoss = LiveCasinoWinLossAccess.GetLiveCasinoWinLoss(Params);
            if (agencyWinLoss.Tables.Count > 0)
            {
                // Get RoleID
                result.RoleID = CastAide.AsInt32(agencyWinLoss.Tables[2].Rows[0]["ShowDetail"]);

                #region items
                foreach (DataRow row in agencyWinLoss.Tables[0].Rows)
                {
                    var item = new LiveCasinoWinLossDTO
                    {
                        ReportType = GetBettypeName(Params.ReportBy, row),

                        CurrencyName = CastAide.AsString(row["Currency"]),

                        RollingTO = NumberFormat.DecFormat(row["RollingTO"], 2),
                        CompanyTO = NumberFormat.DecFormat(row["CompanyTO"], 2),
                        GrossComm = NumberFormat.DecFormat(row["GrossComm"], 2),
                        TotalBets = NumberFormat.DecFormat(row["Bets"], 0),

                        MemberComm = NumberFormat.DecFormat(row["PlayerComm"], 2),
                        MemberWinLoss = NumberFormat.DecFormat(row["Winloss"], 2),
                        MemberTotal = NumberFormat.DecFormat(CastAide.AsDouble(row["Winloss"]) + CastAide.AsDouble(row["PlayerComm"]), 2),

                        AgentComm = NumberFormat.DecFormat(row["Comm"], 2),
                        AgentWinLoss = NumberFormat.DecFormat(row["AWinloss"], 2),
                        AgentToltal = NumberFormat.DecFormat(CastAide.AsDouble(row["AWinloss"]) + CastAide.AsDouble(row["Comm"]), 2),

                        MasterComm = NumberFormat.DecFormat(row["AComm"], 2),
                        MasterWinLoss = NumberFormat.DecFormat(row["MWinloss"], 2),
                        MasterTotal = NumberFormat.DecFormat(CastAide.AsDouble(row["MWinloss"]) + CastAide.AsDouble(row["AComm"]), 2),

                        SuperComm = NumberFormat.DecFormat(row["SComm"], 2),
                        SuperWinLoss = NumberFormat.DecFormat(row["SWinloss"], 2),
                        SuperTotal = NumberFormat.DecFormat(CastAide.AsDouble(row["SWinloss"]) + CastAide.AsDouble(row["SComm"]), 2),

                        Company = NumberFormat.DecFormat(row["CompanyWinLoss"], 2)

                    };

                    list.Add(item);
                }
                result.TotalItemByReportType = list;
                result.CountItems = result.TotalItemByReportType.Count;

                #endregion

                #region total and grand total
                var dtGrandTotal = agencyWinLoss.Tables[1];
                var totalRows = dtGrandTotal.Rows.Count;
                if (totalRows > 0)
                {
                    #region total - each currency

                    result.TotalCurrency = new List<LiveCasinoWinLossDTO>();

                    for (int i = 0; i < totalRows - 1; i++)
                    {
                        result.TotalCurrency.Add(new LiveCasinoWinLossDTO()
                        {
                            ReportType = string.Format("Total {0}", dtGrandTotal.Rows[i]["Currency"]),
                            CurrencyName = CastAide.AsString(dtGrandTotal.Rows[i]["Currency"]),

                            RollingTO = NumberFormat.DecFormat(dtGrandTotal.Rows[i]["RollingTO"], 2),
                            CompanyTO = NumberFormat.DecFormat(dtGrandTotal.Rows[i]["CompanyTO"], 2),
                            GrossComm = NumberFormat.DecFormat(dtGrandTotal.Rows[i]["GrossComm"], 2),
                            TotalBets = NumberFormat.DecFormat(dtGrandTotal.Rows[i]["Bets"], 0),

                            MemberComm = NumberFormat.DecFormat(dtGrandTotal.Rows[i]["PlayerComm"], 2),
                            MemberWinLoss = NumberFormat.DecFormat(dtGrandTotal.Rows[i]["Winloss"], 2),
                            MemberTotal = NumberFormat.DecFormat(CastAide.AsDouble(dtGrandTotal.Rows[i]["PlayerComm"]) + CastAide.AsDouble(dtGrandTotal.Rows[i]["Winloss"]), 2),

                            AgentComm = NumberFormat.DecFormat(dtGrandTotal.Rows[i]["Comm"], 2),
                            AgentWinLoss = NumberFormat.DecFormat(dtGrandTotal.Rows[i]["AWinloss"], 2),
                            AgentToltal = NumberFormat.DecFormat(CastAide.AsDouble(dtGrandTotal.Rows[i]["Comm"]) + CastAide.AsDouble(dtGrandTotal.Rows[i]["AWinloss"]), 2),

                            MasterComm = NumberFormat.DecFormat(dtGrandTotal.Rows[i]["AComm"], 2),
                            MasterWinLoss = NumberFormat.DecFormat(dtGrandTotal.Rows[i]["MWinloss"], 2),
                            MasterTotal = NumberFormat.DecFormat(CastAide.AsDouble(dtGrandTotal.Rows[i]["MWinloss"]) + CastAide.AsDouble(dtGrandTotal.Rows[i]["AComm"]), 2),

                            SuperComm = NumberFormat.DecFormat(dtGrandTotal.Rows[i]["SComm"], 2),
                            SuperWinLoss = NumberFormat.DecFormat(dtGrandTotal.Rows[i]["SWinloss"], 2),
                            SuperTotal = NumberFormat.DecFormat(CastAide.AsDouble(dtGrandTotal.Rows[i]["SWinloss"]) + CastAide.AsDouble(dtGrandTotal.Rows[i]["SComm"]), 2),

                            Company = NumberFormat.DecFormat(dtGrandTotal.Rows[i]["CompanyWinLoss"], 2)
                        });
                    }

                    #endregion

                    #region grand total

                    #region base RM

                    result.GrandTotalCurrency.Add(new LiveCasinoWinLossDTO()
                    {
                        ReportType = "Grand Total RM",
                     
                        CurrencyName = "RM",
                        TotalBets =
                            NumberFormat.DecFormat(
                                dtGrandTotal.Rows[totalRows - 1]["Bets"], 0),
                        RollingTO = NumberFormat.DecFormat(dtGrandTotal.Rows[totalRows - 1]["RollingTO"], 2),
                        CompanyTO = NumberFormat.DecFormat(dtGrandTotal.Rows[totalRows - 1]["CompanyTO"], 2),
                        GrossComm = NumberFormat.DecFormat(dtGrandTotal.Rows[totalRows - 1]["GrossComm"], 2),

                        MemberComm = NumberFormat.DecFormat(dtGrandTotal.Rows[totalRows - 1]["PlayerComm"], 2),
                        MemberWinLoss = NumberFormat.DecFormat(dtGrandTotal.Rows[totalRows - 1]["Winloss"], 2),
                        MemberTotal = NumberFormat.DecFormat(CastAide.AsDouble(dtGrandTotal.Rows[totalRows - 1]["Winloss"]) + CastAide.AsDouble(dtGrandTotal.Rows[totalRows - 1]["PlayerComm"]), 2),


                        AgentComm = NumberFormat.DecFormat(dtGrandTotal.Rows[totalRows - 1]["Comm"], 2),
                        AgentWinLoss = NumberFormat.DecFormat(dtGrandTotal.Rows[totalRows - 1]["AWinloss"], 2),
                        AgentToltal = NumberFormat.DecFormat(CastAide.AsDouble(dtGrandTotal.Rows[totalRows - 1]["AWinloss"]) + CastAide.AsDouble(dtGrandTotal.Rows[totalRows - 1]["Comm"]), 2),

                        MasterComm = NumberFormat.DecFormat(dtGrandTotal.Rows[totalRows - 1]["AComm"], 2),
                        MasterWinLoss = NumberFormat.DecFormat(dtGrandTotal.Rows[totalRows - 1]["MWinloss"], 2),
                        MasterTotal = NumberFormat.DecFormat(CastAide.AsDouble(dtGrandTotal.Rows[totalRows - 1]["MWinloss"]) + CastAide.AsDouble(dtGrandTotal.Rows[totalRows - 1]["AComm"]), 2),

                        SuperComm = NumberFormat.DecFormat(dtGrandTotal.Rows[totalRows - 1]["SComm"], 2),
                        SuperWinLoss = NumberFormat.DecFormat(dtGrandTotal.Rows[totalRows - 1]["SWinloss"], 2),
                        SuperTotal = NumberFormat.DecFormat(CastAide.AsDouble(dtGrandTotal.Rows[totalRows - 1]["SWinloss"]) + CastAide.AsDouble(dtGrandTotal.Rows[totalRows - 1]["SComm"]), 2),

                        Company =
                            NumberFormat.DecFormat(
                                dtGrandTotal.Rows[totalRows - 1]["CompanyWinLoss"], 2)
                    });

                    #endregion

                    #endregion
                }

                #endregion
            }
            return result;
        }

        #endregion

        #region game statistics = filter

        private List<Category> GetAllCompanyAllowPlayLiveCasino()
        {
            var result = new List<Category>();
            try
            {
                result.Add(new Category()
                {
                    Id = -1,
                    Name = "IBC",
                    Selected = true
                });
                var getData = LiveCasinoWinLossAccess.GetCompanyAllowPlayLiveCasino();
                if (getData != null && getData.Rows.Count > 0)
                {
                    foreach (DataRow row in getData.Rows)
                    {
                        result.Add(new Category()
                        {
                            Id = CastAide.AsInt32(row["LicenseeID"]),
                            Name = CastAide.AsString(row["Licenseename"]),
                            Selected = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        private List<Category> GetCurrency()
        {
            var result = new List<Category>();
            try
            {
                result.Add(new Category()
                {
                    Id = -1,
                    Name = "Base RM",
                    Selected = true
                });
                var getData = LiveCasinoWinLossAccess.GetCurrency();
                if (getData != null && getData.Rows.Count > 0)
                {
                    foreach (DataRow row in getData.Rows)
                    {
                        result.Add(new Category()
                        {
                            Id = CastAide.AsInt32(row["ExchangeID"]),
                            Name = CastAide.AsString(row["Currency"]),
                            Selected = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        private List<Category> GetGameType()
        {
            var result = new List<Category>();
            try
            {
                var getData = LiveCasinoWinLossAccess.GetGameType();
                if (getData != null && getData.Rows.Count > 0)
                {
                    foreach (DataRow row in getData.Rows)
                    {
                        result.Add(new Category()
                        {
                            Id = CastAide.AsInt32(row["typeID"]),
                            Name = CastAide.AsString(row["typenamee"]),
                            Selected = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public List<Category> GetGameTableByGameType(int gameType)
        {
            var result = new List<Category>();
            try
            {
                var getData = LiveCasinoWinLossAccess.GetGameTableByGameType(gameType);
                if (getData != null && getData.Rows.Count > 0)
                {
                    foreach (DataRow row in getData.Rows)
                    {
                        result.Add(new Category()
                        {
                            Id = CastAide.AsInt32(row["GameTableID"]),
                            Name = CastAide.AsString(row["GameTable"]),
                            Selected = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

        public GameSearch GetGameFilterInfo()
        {
            var result = new GameSearch();

            var config = LiveCasinoSection.Instance;
            #region filter by game info
            #region assign company data
            result.Company = GetAllCompanyAllowPlayLiveCasino();
            #endregion

            #region assign game type data
            result.GameType = GetGameType();
            #endregion

            #region assign currency data

            result.Currency = GetCurrency();
            #endregion

            #region assign game table data
            #endregion
            #endregion

            #region filter by report type
            var reportType = config.Report[0].ReportType;
            var reportFilter = from r in reportType.Cast<FilterElement>().Where(i => i.Active) select r;
            if (reportFilter != null && reportFilter.Count() > 0)
            {
                result.ReportType = new List<Category>();
                foreach (FilterElement filter in reportFilter)
                {
                    result.ReportType.Add(new Category()
                    {
                        Id = filter.Id,
                        Name = filter.Name,
                        Selected = filter.Selected
                    });
                }
            }

            #endregion
            return result;
        }
        #endregion

        #region Export - Live Casino Agency WinLoss Report
        public void ExportLiveCasinoAgencyWinLossReport(LiveCasinoWinLossParams parameters)
        {
            parameters.PageIndex = 0;
            parameters.PageSize = 0;
            LiveCasinoWinLoss reportData = new LiveCasinoWinLoss();

            if (parameters.FromCache)
            {
                var getDataFromcache = SessionExtension.GetSession(HttpContext.Current.Session, LiveCasnoWinLossMapedSessionKey);
                if (getDataFromcache != null)
                    reportData = (LiveCasinoWinLoss)getDataFromcache;
                else
                    reportData = GetLiveCasinoWinLossData(parameters);                   
            }
            else            
                reportData = GetLiveCasinoWinLossData(parameters);

            var startCol = 0;
            var startRow = 3;
            var reportName = "LiveCasinoAgencyWinLoss";
            var excelFile = new ExcelFile();

            var maxItemsInSheet = 3000;
            if (reportData.TotalItemByReportType != null && reportData.TotalItemByReportType.Count > 0)
            {
                if (parameters.ReportBy == (int)FilterGameCondition.Username)
                    ExportLiveCasinoAgencyWinLossReportAssignBodyNormal(startRow, startCol, maxItemsInSheet, ref excelFile, reportData, parameters);
                else if (parameters.ReportBy == (int)FilterGameCondition.Agency)
                    ExportLiveCasinoAgencyWinLossReportAssignBodyByAgency(startRow, startCol, maxItemsInSheet, ref excelFile, reportData, parameters);
                else
                    ExportLiveCasinoAgencyWinLossReportAssignBodyByGroup(startRow, startCol, maxItemsInSheet, ref excelFile, reportData, parameters);
            }
            else
            {
                var errorMsg = reportData.CountItems == -1 ?
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.ErrorMsg, "TimeOutError") :
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblThereisnodata");

                var wsReport =
                    excelFile.Worksheets.Add(reportName);
                wsReport.Cells[startRow - 1, startCol].Value = errorMsg;
            }

            #region response file
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}.xls", reportName));
            excelFile.SaveXls(response.OutputStream);

            #endregion 
            
        }

        private void ExportLiveCasinoAgencyWinLossReportAssignHeader(int startRow, int startCol, ref ExcelWorksheet wsReport, int reportType, string filterUsername, string fromDate, string toDate) 
        {
            var bgHeaderFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            #region second row - title of columns
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Value = GetLiveCasinoAgencyWLTypeName(reportType, filterUsername);

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Currency");

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Totalbets");

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CompanyTO");

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "grosscomm");

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "numberBet");
            #region Member
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Value = "Member";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "WinLoss");
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Total");
            #endregion
            #region Agent
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Value = "Agent";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "WinLoss");
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Total");
            #endregion
            #region Master
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Value = "Master";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "WinLoss");
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Total");
            #endregion
            #region Super
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Value = "Super";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "WinLoss");
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Total");
            #endregion
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Company");
            #endregion

            #region report title
            var title = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "LiveCasinoGameAgencyWinLossReportTitle");
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Value = string.Format("{0} From {1:MM/dd/yyyy} To {2:MM/dd/yyyy}", title, CastAide.AsDateTime(fromDate),  CastAide.AsDateTime(toDate));
            #endregion
        }

        private void ExportLiveCasinoAgencyWinLossReportAssignHeaderByAgency(int startRow, int startCol, ref ExcelWorksheet wsReport, int reportType, int roleId, string fromDate, string toDate)
        {
            var bgHeaderFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
            var nextCol = 0;

            #region second row - title of columns
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol, startRow - 1, startCol).Value = GetLiveCasinoAgencyWLTypeName(reportType, string.Empty);

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Currency");

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Totalbets");

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "CompanyTO");

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "grosscomm");

            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "numberBet");
            #region Member
            if (roleId == 1)
            {
                wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Style = bgHeaderFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Value = "Member";

                wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
                wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "WinLoss");
                wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
                wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
                wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
                wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Total");
            }
            #endregion
            #region Agent
            if (roleId <= 2)
            {
                wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Style = bgHeaderFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Value = "Agent";

                wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
                wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "WinLoss");
                wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
                wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
                wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
                wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Total");
            }
            #endregion
            #region Master
            if (roleId <= 3)
            {
                wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Merged = true;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Style = bgHeaderFormat;
                wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Value = "Master";

                wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
                wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "WinLoss");
                wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
                wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
                wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
                wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                    ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Total");
            }
            #endregion
            #region Super
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol + 1), startRow - 2, startCol + nextCol + 3).Value = "Super";

            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "WinLoss");
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "lblComm");
            wsReport.Cells[startRow - 1, startCol + (nextCol += 1)].Style = bgHeaderFormat;
            wsReport.Cells[startRow - 1, startCol + nextCol].Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Total");
            #endregion
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol += 1), startRow - 1, startCol + nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 2, startCol + (nextCol), startRow - 1, startCol + nextCol).Value =
                ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "Company");
            
            #endregion

            #region report title
            var title = ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "LiveCasinoGameAgencyWinLossReportTitle");
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Merged = true;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Style = bgHeaderFormat;
            wsReport.Cells.GetSubrangeAbsolute(startRow - 3, startCol, startRow - 3, nextCol).Value = string.Format("{0} From {1:MM/dd/yyyy} To {2:MM/dd/yyyy}", title, CastAide.AsDateTime(fromDate),  CastAide.AsDateTime(toDate));
            #endregion
        }

        private void ExportLiveCasinoAgencyWinLossReportAssignBodyNormal(int startRow, int startCol, int maxItemsInSheet, ref ExcelFile excelFile, LiveCasinoWinLoss report, LiveCasinoWinLossParams parameters)
        {
            var totalSheet = (report.CountItems % maxItemsInSheet > 0 ? 1 : 0) + report.CountItems / maxItemsInSheet;
            var numberDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false);
            var numberIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(253, 252, 252), false, false);
            var numberFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false, true);
            var nextRow = 0;
            var itemIndex = 0;

            for (int i = 0; i < totalSheet; i++)
            {
                var wsReport = excelFile.Worksheets.Add(string.Format("LiveCasinoAgencyWinLoss_{0}", i + 1));
                #region header
                ExportLiveCasinoAgencyWinLossReportAssignHeader(startRow, startCol, ref wsReport, (int)FilterGameCondition.Username, parameters.UserName, parameters.FromDate, parameters.ToDate);
                #endregion

                #region content

                var totalItemsInCurrentSheet = i < totalSheet - 1 ? maxItemsInSheet : (report.CountItems - (i * maxItemsInSheet));
                for (int j = 0; j < totalItemsInCurrentSheet; j++)
                {
                    var item = report.TotalItemByReportType[itemIndex];
                    var nextCol = 0;

                    wsReport.Cells[startRow + nextRow, startCol].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol].Value = item.ReportType;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = item.CurrencyName;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.RollingTO);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.CompanyTO);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.GrossComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberIntFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.TotalBets);
                    #region Member
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberTotal);
                    #endregion
                    #region Agent
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentToltal);
                    #endregion
                    #region Master
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterTotal);
                    #endregion
                    #region Super
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperTotal);
                    #endregion

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.Company);

                    nextRow++;
                    itemIndex++;
                }
                #endregion
                
                #region currency total & grand total
                if (i == totalSheet - 1 && report.TotalCurrency != null)
                {
                    var numberTotalDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(247, 236, 201), true, false);
                    var numberTotalFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(247, 236, 201), true, false, true);
                    var numberTotalIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(247, 236, 201), true, false, true);

                    var numberGandTotalDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false);
                    var numberGandTotalFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false, true);
                    var numberGandTotalIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false);

                    #region total
                    foreach (var item in report.TotalCurrency)
                    {
                        var nextCol = 0;

                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style = numberTotalDefaultFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value = item.ReportType;

                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 2)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.RollingTO);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.CompanyTO);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.GrossComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalIntFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.TotalBets);
                        #region Member
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberTotal);
                        #endregion
                        #region Agent
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentToltal);
                        #endregion
                        #region Master
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterTotal);
                        #endregion
                        #region Super
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperTotal);
                        #endregion
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.Company);

                        nextRow++;
                    }
                    #endregion

                    #region grand total

                    foreach (var grandTotalItem in report.GrandTotalCurrency)
                    {
                        var nextColGrandTotal = 0;

                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style = numberGandTotalDefaultFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value = grandTotalItem.ReportType;

                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 2)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.RollingTO);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.CompanyTO);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.GrossComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalIntFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.TotalBets);
                        #region Member
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MemberWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MemberComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MemberTotal);
                        #endregion
                        #region Agent
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.AgentWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.AgentComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.AgentToltal);
                        #endregion
                        #region Master
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MasterWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MasterComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MasterTotal);
                        #endregion
                        #region Super
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.SuperWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.SuperComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.SuperTotal);
                        #endregion
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.Company);
                    }

                    #endregion
                }
                #endregion
                nextRow = 0;
            }
        }

        private void ExportLiveCasinoAgencyWinLossReportAssignBodyByGroup(int startRow, int startCol, int maxItemsInSheet, ref ExcelFile excelFile, LiveCasinoWinLoss report, LiveCasinoWinLossParams pParams)
        {
            var numberDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false);
            var numberFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false, true);
            var numberIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(253, 252, 252), false, false);
            var nextRow = 0;
            var currentSheet = 1;            
            #region content

            ExcelWorksheet wsReport = null;
            foreach (var group in report.TotalItemByReportType)
            {
                #region create header for each sheet
                if (group.Items.Count + nextRow + startRow > maxItemsInSheet | currentSheet == 1)
                {
                    nextRow = 0;
                    wsReport = excelFile.Worksheets.Add(string.Format("LiveCasinoAgencyWinLoss_{0}", currentSheet));
                    #region header
                    ExportLiveCasinoAgencyWinLossReportAssignHeader(startRow, startCol, ref wsReport, pParams.ReportBy, string.Empty, pParams.FromDate, pParams.ToDate);
                    #endregion

                    currentSheet++;
                }
                #endregion

                #region detail

                var isFirstRow = true;
                foreach (var item in group.Items)
                {
                    var nextCol = 0;
                    if (isFirstRow)
                    {
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol,
                                                           startRow + nextRow + (group.Items.Count - 1), startCol).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol,
                                                           startRow + nextRow + (group.Items.Count - 1), startCol).Style
                            = numberDefaultFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol,
                                                           startRow + nextRow + (group.Items.Count - 1), startCol).Value
                            = item.ReportType;
                    }

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = item.CurrencyName;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.RollingTO);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.CompanyTO);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.GrossComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberIntFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.TotalBets);
                    #region Member
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberTotal);
                    #endregion
                    #region Agent
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentToltal);
                    #endregion
                    #region Master
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterTotal);
                    #endregion
                    #region Super
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperTotal);
                    #endregion

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.Company);

                    nextRow++;
                    isFirstRow = false;
                }
                #endregion
            }

            #endregion

            #region currency total & grand total
            if (report.TotalCurrency != null)
            {
                var numberTotalDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(247, 236, 201), true, false);
                var numberTotalFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(247, 236, 201), true, false, true);
                var numberTotalIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(247, 236, 201), true, false);

                var numberGandTotalDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false);
                var numberGandTotalFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false, true);
                var numberGandTotalIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(162, 212, 247), true, false);

                #region total
                foreach (var item in report.TotalCurrency)
                {
                    var nextCol = 0;

                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style = numberTotalDefaultFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value = item.ReportType;

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 2)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.RollingTO);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.CompanyTO);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.GrossComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalIntFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.TotalBets);
                    #region Member
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberTotal);
                    #endregion
                    #region Agent
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentToltal);
                    #endregion
                    #region Master
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterTotal);
                    #endregion
                    #region Super
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperTotal);
                    #endregion
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.Company);

                    nextRow++;
                }
                #endregion

                #region grand total

                foreach (var grandTotalItem in report.GrandTotalCurrency)
                {
                    var nextColGrandTotal = 0;

                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style = numberGandTotalDefaultFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value = grandTotalItem.ReportType;

                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 2)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.RollingTO);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.CompanyTO);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.GrossComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalIntFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.TotalBets);
                    #region Member
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MemberWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MemberComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MemberTotal);
                    #endregion
                    #region Agent
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.AgentWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.AgentComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.AgentToltal);
                    #endregion
                    #region Master
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MasterWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MasterComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MasterTotal);
                    #endregion
                    #region Super
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.SuperWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.SuperComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.SuperTotal);
                    #endregion
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.Company);
                }

                #endregion
            }
            #endregion

        }

        private void ExportLiveCasinoAgencyWinLossReportAssignBodyByAgency(int startRow, int startCol, int maxItemsInSheet, ref ExcelFile excelFile, LiveCasinoWinLoss report, LiveCasinoWinLossParams pParams)
        {
            var numberDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false);
            var numberFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(253, 252, 252), false, false, true);
            var numberIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(253, 252, 252), false, false);
            var nextRow = 0;
            var currentSheet = 1;
            #region content

            ExcelWorksheet wsReport = null;
            foreach (var group in report.TotalItemByReportType)
            {
                #region create header for each sheet
                if (group.Items.Count + nextRow + startRow > maxItemsInSheet | currentSheet == 1)
                {
                    nextRow = 0;
                    wsReport = excelFile.Worksheets.Add(string.Format("LiveCasinoAgencyWinLoss_{0}", currentSheet));
                    #region header
                    ExportLiveCasinoAgencyWinLossReportAssignHeaderByAgency(startRow, startCol, ref wsReport, pParams.ReportBy, report.RoleID, pParams.FromDate, pParams.ToDate);
                    #endregion

                    currentSheet++;
                }
                #endregion

                #region detail

                var isFirstRow = true;
                foreach (var item in group.Items)
                {
                    var nextCol = 0;
                    if (isFirstRow)
                    {
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol,
                                                           startRow + nextRow + (group.Items.Count - 1), startCol).Merged = true;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol,
                                                           startRow + nextRow + (group.Items.Count - 1), startCol).Style
                            = numberDefaultFormat;
                        wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol,
                                                           startRow + nextRow + (group.Items.Count - 1), startCol).Value
                            = item.ReportType;
                    }

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = item.CurrencyName;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.RollingTO);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.CompanyTO);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.GrossComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberIntFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.TotalBets);
                    #region Member
                    if (report.RoleID == 1)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberTotal);
                    }
                    #endregion
                    #region Agent
                    if (report.RoleID <= 2)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentToltal);
                    }
                    #endregion
                    #region Master
                    if (report.RoleID <= 3)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterTotal);
                    }
                    #endregion
                    #region Super
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperTotal);
                    #endregion

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.Company);

                    nextRow++;
                    isFirstRow = false;
                }
                #endregion
            }

            #endregion

            #region currency total & grand total
            if (report.TotalCurrency != null)
            {
                var numberTotalDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(247, 236, 201), true, false);
                var numberTotalFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(247, 236, 201), true, false, true);
                var numberTotalIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(247, 236, 201), true, false);

                var numberGandTotalDefaultFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false);
                var numberGandTotalFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber(".00", Color.FromArgb(162, 212, 247), true, false, true);
                var numberGandTotalIntFormat = new Sunplus.Accounting.Core.Helper.ExcelGemboxUtils().GetStyleIntNumber("", Color.FromArgb(162, 212, 247), true, false);

                #region total
                foreach (var item in report.TotalCurrency)
                {
                    var nextCol = 0;

                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style = numberTotalDefaultFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value = item.ReportType;

                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 2)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.RollingTO);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.CompanyTO);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.GrossComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalIntFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.TotalBets);
                    #region Member
                    if (report.RoleID == 1)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MemberTotal);
                    }
                    #endregion
                    #region Agent
                    if (report.RoleID <= 2)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.AgentToltal);
                    }
                    #endregion
                    #region Master
                    if (report.RoleID <= 3)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.MasterTotal);
                    }
                    #endregion
                    #region Super
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.SuperTotal);
                    #endregion
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol += 1)].Style = numberTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextCol)].Value = CastAide.AsDecimal(item.Company);

                    nextRow++;
                }
                #endregion

                #region grand total

                foreach (var grandTotalItem in report.GrandTotalCurrency)
                {
                    var nextColGrandTotal = 0;

                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Merged = true;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Style = numberGandTotalDefaultFormat;
                    wsReport.Cells.GetSubrangeAbsolute(startRow + nextRow, startCol, startRow + nextRow, startCol + 1).Value = grandTotalItem.ReportType;

                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 2)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.RollingTO);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.CompanyTO);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.GrossComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalIntFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.TotalBets);
                    #region Member
                    if (report.RoleID == 1)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MemberWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MemberComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MemberTotal);
                    }
                    #endregion
                    #region Agent
                    if (report.RoleID <= 2)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.AgentWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.AgentComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.AgentToltal);
                    }
                    #endregion
                    #region Master
                    if (report.RoleID <= 3)
                    {
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MasterWinLoss);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MasterComm);
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                        wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.MasterTotal);
                    }
                    #endregion
                    #region Super
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.SuperWinLoss);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.SuperComm);
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalDefaultFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.SuperTotal);
                    #endregion
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal += 1)].Style = numberGandTotalFormat;
                    wsReport.Cells[startRow + nextRow, startCol + (nextColGrandTotal)].Value = CastAide.AsDecimal(grandTotalItem.Company);
                }

                #endregion
            }
            #endregion

        }

        private string GetLiveCasinoAgencyWLTypeName(int type, string filterUsername)
        {
            var result = "Daily";
            switch (type)
            {
                case (int)(FilterGameCondition.Username):
                    if (string.IsNullOrEmpty(filterUsername))
                        result = "Username";
                    else
                        result = filterUsername;
                    break;
                case (int)(FilterGameCondition.Agency):
                    result = "Username";
                    break;
                case (int)(FilterGameCondition.Monthly):
                    result = "Month";
                    break;
                case (int)(FilterGameCondition.GameType):
                    result = "Game";
                    break;
                case (int)(FilterGameCondition.GameTable):
                    result = "Table";
                    break;
                case (int)(FilterGameCondition.Daily):
                    result = "Date";
                    break;
            }
            return result;
        }
        #endregion
    
    }
}
