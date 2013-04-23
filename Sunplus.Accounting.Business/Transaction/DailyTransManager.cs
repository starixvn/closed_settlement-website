using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel.Activation;
using System.Text;
using DNAide.Utils;
using DNAide.Web;

using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.ViewModel;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Core.LoggingLib;
using FormatHandler = IBCUtils.FormatHandler;
using DNAide.Data;
using System.Data;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.Core.Extensions;
using DNAide.Core;
using ExcelUtil.Spreadsheet;
using System.Linq;
using System.Drawing;
using System.Web;
using Sunplus.Accounting.Core.Cache.Extension;
using System.IO;

//using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Business.Transaction
{
    /// <summary>
    /// Summary description for DailyTransManager
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DailyTransManager : IServiceDailyTran
    {
        private const int PAGE_SIZE = 100;

        private const string TEMPLATE_UNVERIFIED_BUTTON =
            "<input type=\"button\" class=\"unverify\"  value=\"Unverify\" title=\"Unverify\"  onclick=\"return UnVerifyTrans(this);\"  id=\"{0}\"/>";

        private const string TEMPLATE_VERIFIED_BUTTON =
            "<input type=\"button\" class=\"verified\" value=\"Verify\" title=\"Verify\"  onclick=\"return VerifyTrans(this);\"  id=\"{0}\"/>";
        private string _keyDailyAllTxn = AppSession.Username + "_SelDailyAllTxn";
        private string _keyDailyFinanceTxn = AppSession.Username + "_SelDailyFinanceTxn";

        #region IServiceDailyTran Members

        /// <summary>
        /// return finace daily transaction to Json string
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="date"></param>
        /// <param name="Type"></param>
        /// <param name="Creator"></param>
        /// <returns></returns>
        #region get daily finance transaction
        
        #endregion

        #region get Daily All Transaction
        public string GetDailyAllTransaction(int pageIndex, string date, string type, string creator)
        {
            var tpl = string.Empty;
            int TotalPages = 0;
            try
            {
                tpl = WebUtils.LoadTemplate("Transactions/DailyAllTransaction.htm");
                DateTime transDate = DateTime.Parse(date, CultureInfo.CurrentCulture);
                transDate = IBCUtils.FormatHandler.ResetStartDate(transDate);
                GetDailyAllTransactionContent(transDate, ref tpl, type, creator, pageIndex, 100,  ref TotalPages);
            }
            catch (DNAide.Utils.AccException ex)
            {
                tpl = AlertMessage.GetMsg(ex.Message.ToString(), AlertMessage.MsgType.Error);
                Logs.Write(ex.Message);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }

            Hashtable data = new Hashtable();
            data.Add("Content", tpl);
            data.Add("Totalpages", TotalPages);
            return JsonUtil.ToJson(data);
        }

        public DailyTransactionDTO GetDailyAllTransaction(string date, string type, string creator, int pageIndex, int pageSize, int fromcache)
        {
            var dailyAllTxnPaging = new DailyTransactionDTO();
            var maxLenList = 65000;
            var totalRow = 0;
            var indexList = pageIndex * pageSize / maxLenList;
            var keyCache = string.Format(_keyDailyAllTxn + "_{0}", indexList);
            var dtoCaching = CacheExtension.Get<DailyTransactionDTO>(keyCache);

            if (dtoCaching == null || fromcache == 0)
            {
                dtoCaching = GetDailyAllTxnfromDB(date, type, creator, pageIndex, pageSize);
            }

            if (dtoCaching.items != null)
            {
                dailyAllTxnPaging = GetDailyAllTxnDTO(dtoCaching, pageIndex, pageSize, maxLenList, totalRow);
            }
            return dailyAllTxnPaging;

        }

        public DailyTransactionDTO GetDailyAllTxnDTO(DailyTransactionDTO data, int pageIndex, int pageSize, int maxLenList, int totalRowList)
        {
       
            var indexList = pageIndex * pageSize / maxLenList;
            var counterList = data.items.Count;
            var offset = ((pageIndex - 1) * pageSize) - (indexList * maxLenList);
            var counter = Math.Min(pageSize, (counterList - offset));

            data.items = data.items.Select(item => item).Skip(offset).Take(counter).ToList();

            return data;
        }

        public DailyTransactionDTO GetDailyAllTxnfromDB(string date, string type, string creator, int pageIndex, int pageSize)
        {
            var maxLenList = 65000;
            var totalRow = 0;
            var indexList = pageIndex * pageSize / maxLenList;
            var keyCache = _keyDailyAllTxn + "_{0}";
            DailyTransactionDTO cacheReturn = new DailyTransactionDTO();

            var userDAO = CommonBootStrapper.Locator.GetInstance<IUserDAO>();
            DbResult dbResult = userDAO.GetDailyAllTransaction(CastAide.AsDateTime(date), type, creator);

            if (dbResult != null && dbResult.ResultSet.Tables[0].Rows.Count > 0)
            {
                var tableRow = dbResult.ResultSet.Tables[0];
                var rowData = tableRow.Rows.Cast<DataRow>();
                totalRow = rowData.Count();
                var numList = totalRow % maxLenList > 0 ? (totalRow / maxLenList) + 1 : totalRow / maxLenList;

                Dictionary<int, List<DataRow>> listDataRow = new Dictionary<int, List<DataRow>>();
                for (int index = 0; index < numList; index++)
                {
                    var offset = index * maxLenList;
                    var takeList = Math.Min(maxLenList, (totalRow - offset));
                    var rowDTO = rowData.Select(dr => dr).Skip(offset).Take(takeList).ToList();

                    var dailyCaching = new DailyTransactionDTO();
                    dailyCaching.items = new List<DailyTransaction>();
                    foreach (var dr in rowDTO)
                    {
                        var entity = new DailyTransaction();
                        try
                        {
                            entity.Username = (string)dr["username"];
                            entity.transDate = CastAide.AsString(dr["transdate"]);
                            entity.Amount = Convert.ToDouble(dr["amount"]);
                            entity.Description = dr.IsNull("transdesc") ? "" : dr["transdesc"].ToString();
                            entity.Type = (string)dr["typenamee"];
                            entity.Creator = (string)dr["creator"];
                            dailyCaching.items.Add(entity);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    // save cache
                    dailyCaching.totalRows = totalRow;
                    CacheExtension.Set(string.Format(keyCache, index), dailyCaching);
                    if (indexList == index)
                    {
                        cacheReturn = dailyCaching;
                    }
                }
            }
            return cacheReturn;
        }
        
        #endregion
        /// <summary>
        /// verify multiple transaction
        /// </summary>
        /// <param name="refNoList"></param>
        /// <returns></returns>
        public bool VerifyTransaction(string refNoList)
        {
            bool isSuccessfull = false;

            var user = new User();
            user.Id = AppSession.AccId;
            user.Username = AppSession.AccUserName;
            user.LastLoginIP = WebUtils.CurrentIp;

            #region check permision before update to DB

            bool hasUpdateTransPermission = Permissions.GetHostPermission
                (Enumerations.Functions.FinancialManagement.DailyFinanceTxn);
            if (hasUpdateTransPermission == false)
                return false;

            #endregion

            var userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
            isSuccessfull = userService.VerfiyDailyTransactions(user, refNoList);

            return isSuccessfull;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refNo"></param>
        /// <returns></returns>
        public bool UnVerifyTransaction(string refNo)
        {
            bool isSuccessfull = false;

            var user = new User();
            user.Id = AppSession.AccId;
            user.Username = AppSession.AccUserName;
            user.LastLoginIP = WebUtils.CurrentIp;

            #region check permision before update to DB

            bool hasUpdateTransPermission = Permissions.GetHostPermission
                (Enumerations.Functions.FinancialManagement.DailyFinanceTxn);
            if (hasUpdateTransPermission == false)
                return false;

            #endregion

            var userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
            isSuccessfull = userService.UnverifyTransaction(user, refNo);


            return isSuccessfull;
        }

        #endregion



        /// <summary>
        /// Build string render from daily finance transaction data
        /// </summary>
        /// <param name="transDate"></param>
        /// <param name="content"></param>
        /// <param name="status"></param>
        /// <param name="Type">0: Unverified, 1: Verfiied, 2: All</param>
        /// <param name="Creator"></param>
        /// <param name="PageIndex"></param>
        /// <param name="TotalPages"></param>
        /// <param name="cult"></param>
        #region get finance transaction content
        private string GetFinanceTransactionContent(string transDate, int status
                                                    , string type, string creator, int PageIndex, int TotalPages,
                                                    string verifyBy, List<DailyTransaction> entityList)
        {
            string content = string.Empty;
            string itemTemplate = string.Empty;

            //get daily finance transaction data
            var userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
            /*List<DailyTransaction> entityList = userService
                .GetDailyTransaction(transDate, PageIndex, PAGE_SIZE, status,
                                     type, creator, ref TotalPages, verifyBy);*/

            var strBuilder = new StringBuilder();

            //get persmission of logined user
            bool hasUpdateTransPermission = Permissions.GetHostPermission
                (Enumerations.Functions.FinancialManagement.DailyFinanceTxn);

            string[] arrDate = transDate.ToString().Split('/');
            string strDate = arrDate[0] + "/" + arrDate[1] + "/" + arrDate[2].Substring(0, 4);


            //load template by permission
            if (hasUpdateTransPermission)
            {
                content = WebUtils.LoadTemplate("Transactions/DailyTransaction.htm");
                itemTemplate = WebUtils.LoadTemplate("Transactions/DailyTxnRow.htm");
            }
            else
            {
                content = WebUtils.LoadTemplate("Transactions/DailyTransactionReadOnly.htm");
                itemTemplate = WebUtils.LoadTemplate("Transactions/DailyTxnRowReadOnly.htm");
            }

            if (entityList.Count > 0)
            {
                #region Define params

                string itemContent = string.Empty;
                string rowStyle = string.Empty;

                //continous page index
                int rowIndex = (PageIndex - 1) * PAGE_SIZE;

                //render row by row data
                string prevRefNo = entityList[0].RefNo;
                //get dictionary list, 
                Dictionary<string, int> list = userService.GetListTotalTransPerGroup(entityList);

                string actionTemplate = string.Empty;

                #endregion

                int rowIndexOnCurrentPage = 0;
                int totalVeirfyAllow = 0;
                foreach (DailyTransaction entity in entityList)
                {
                    rowIndex++;
                    rowIndexOnCurrentPage++;

                    itemContent = itemTemplate;

                    #region Assign to template data

                    TemplateHandler.Assign(ref itemContent, "{date}",
                                           entity.transDate);

                    //change row style for transaction group
                    //use ref no to seperate row style
                    if (prevRefNo != entity.RefNo || rowIndexOnCurrentPage == 1)
                    {
                        prevRefNo = entity.RefNo;
                        rowStyle = (!rowStyle.Contains("evenrow")
                                        ? " class=\"evenrow ui-widget-content jqgrow ui-row-ltr\""
                                        : "class=\"ui-widget-content jqgrow ui-row-ltr\"");
                        TemplateHandler.Assign(ref itemContent, "{rowspan}", list[entity.RefNo]);
                    }
                    else
                    {
                        if (rowIndex > 1)
                        {
                            //do not remove first row
                            TemplateHandler.RemoveBlock(ref itemContent, "CheckBoxBlock");
                            TemplateHandler.RemoveBlock(ref itemContent, "UnVerifiedButtonBlock");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref itemContent, "{rowspan}", list[entity.RefNo]);
                        }

                        //TemplateHandler.Assign(ref itemContent, "{rowspan}", list[entity.RefNo]);
                    }

                    //FormatNumber2 -> what does it do? what format
                    TemplateHandler.Assign(ref itemContent, "{amount}",
                                           FormatHandler.FormatNumber2(entity.Amount));
                    TemplateHandler.Assign(ref itemContent, "{no}", rowIndex);
                    TemplateHandler.Assign(ref itemContent, "{name}", entity.RefNo);
                    TemplateHandler.Assign(ref itemContent, "{rowStyle}", rowStyle);
                    TemplateHandler.Assign(ref itemContent, "{username}", entity.Username);
                    TemplateHandler.Assign(ref itemContent, "{description}", entity.Description);
                    TemplateHandler.Assign(ref itemContent, "{type}", entity.Type);
                    TemplateHandler.Assign(ref itemContent, "{description}", entity.Type);
                    TemplateHandler.Assign(ref itemContent, "{id}", string.Format("id='{0}'", entity.RefNo));

                    //Mergeable columns
                    TemplateHandler.Assign(ref itemContent, "{creator}", entity.Creator);
                    TemplateHandler.Assign(ref itemContent, "{verifiedby}", entity.CheckerName);
                    TemplateHandler.Assign(ref itemContent, "{verifiedtime}",
                                           ((CastAide.AsDateTime(entity.CheckedTime) != DateTime.MinValue)
                                                ? entity.CheckedTime
                                                : string.Empty));


                    if (entity.CheckerId > 0)
                    {
                        /*
                     * the transaction had already VERIFIED
                     * disable verify checkbox, and set checked
                     * show Unverify button.
                     * But hide all action button if user doest not have permission
                     */
                        TemplateHandler.Assign(ref itemContent, "{checked}", "checked=checked");
                        TemplateHandler.Assign(ref itemContent, "{disabled}", "disabled=disabled");

                        actionTemplate = (hasUpdateTransPermission)
                                             ? string.Format(TEMPLATE_UNVERIFIED_BUTTON, entity.RefNo)
                                             : string.Empty;
                    }
                    else
                    {
                        /*
                     *  the transaction has NOT VERIFIED yet
                     *  enable checkbox for select
                     *  show verify button
                     */

                        TemplateHandler.Assign(ref itemContent, "{checked}", string.Empty);
                        TemplateHandler.Assign(ref itemContent, "{disabled}", string.Empty);
                        actionTemplate = (hasUpdateTransPermission)
                                             ? string.Format(TEMPLATE_VERIFIED_BUTTON, entity.RefNo)
                                             : string.Empty;

                        totalVeirfyAllow++;
                    }

                    TemplateHandler.Assign(ref itemContent, "{action}", actionTemplate);

                    #endregion

                    strBuilder.Append(itemContent);
                }
                TemplateHandler.Assign(ref content, "{disbaleAll}", totalVeirfyAllow > 0 ? "" : "disabled='disabled'");
                TemplateHandler.Assign(ref content, "{content}", strBuilder.ToString());
                TemplateHandler.Assign(ref content, "{totalpage}", TotalPages.ToString());

                /*
             *  Just show paging only if total page > 1             
             */

                TemplateHandler.Assign(ref content, "{footerpager}",
                                       (TotalPages > 1)
                                           ? Pager.InitPager(TotalPages, PageIndex, string.Empty)
                                           : string.Empty);
                TemplateHandler.Assign(ref content, "{fColspan}", "11");
            }
            else
            {
                /*
             *  Generate no information section
             */
                TemplateHandler.Assign(ref content, "{content}",
                                       AccCommon.GenNoInformation(((hasUpdateTransPermission) ? "10" : "9")));
                TemplateHandler.RemoveBlock(ref content, "noinfo");
                TemplateHandler.RemoveBlock(ref content, "havingdata");
                TemplateHandler.Assign(ref content, "{footerpager}", string.Empty);
                TemplateHandler.Assign(ref content, "{fColspan}", "10");
            }

            TemplateHandler.RemoveComment(ref content);
            TemplateHandler.ShrinkHtml(ref content);

            return content;
        }
        #endregion

        #region get daily all transaction
        private void GetDailyAllTransactionContent(DateTime transDate, ref string tplContent, string type, string creator, int pageIndex, int pageSize, ref int totalPages)
        {
            var cult = CultureInfo.CurrentCulture;
            var getDailyAllTxn = new List<DailyTransaction>();
            var str = new StringBuilder();
            try
            {
                var userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
                List<DailyTransaction> entityList = userService
                    .GetDailyAllTransaction(transDate, type, creator, pageIndex, pageSize, ref totalPages);

                var sRow = WebUtils.LoadTemplate("Transactions/DailyAllTxnRow.htm");
                var newRow = string.Empty;
                var odd_even = string.Empty;
                AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyDailyAllTxn, entityList);
                getDailyAllTxn = (List<DailyTransaction>)AppSession.GetSession(System.Web.HttpContext.Current.Session,
                                                           _keyDailyAllTxn);
                if (entityList.Count > 0)
                {                    
                    foreach (DailyTransaction entity in entityList)
                    {
                        newRow = sRow;
                        TemplateHandler.Assign(ref newRow, "{date}", string.Format(cult, "{0:M/d/yyyy hh:mm:ss tt}", entity.transDate));

                        odd_even = (!odd_even.Contains("evenrow")
                                        ? " class=\"evenrow ui-widget-content jqgrow ui-row-ltr\""
                                        : "class=\"ui-widget-content jqgrow ui-row-ltr\"");

                        //odd_even = (odd_even == "class=\"evenrow\"") ? "" : "class=\"evenrow\"";
                        TemplateHandler.Assign(ref newRow, "{odd_even}", odd_even);

                        TemplateHandler.Assign(ref newRow, "{username}", entity.Username);
                        TemplateHandler.Assign(ref newRow, "{description}", entity.Description);
                        TemplateHandler.Assign(ref newRow, "{type}", entity.Type);
                        TemplateHandler.Assign(ref newRow, "{amount}", FormatHandler.FormatNumber2(entity.Amount));
                        TemplateHandler.Assign(ref newRow, "{creator}", entity.Creator);
                        TemplateHandler.Assign(ref newRow, "{no}", entity.RowIndex);
                        str.Append(newRow);
                    }
                    TemplateHandler.Assign(ref tplContent, "{content}", str.ToString());
                    TemplateHandler.Assign(ref tplContent, "{totalpage}", totalPages.ToString());
                    TemplateHandler.Assign(ref tplContent, "{footerpager}", Pager.InitPager(totalPages, pageIndex, ""));
                }
                else
                {
                    TemplateHandler.Assign(ref tplContent, "{content}", AccCommon.GenNoInformation("7"));
                    TemplateHandler.RemoveBlock(ref tplContent, "havingdata");
                    TemplateHandler.Assign(ref tplContent, "{footerpager}", "");
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
        }
        #endregion

        

        #region IServiceDailyTran Members

        public void ExportToExcelforDailyAllTxn(string date, string type, string creator, int totalRows)
        {
            try
            {
                #region excel format
                const int startCol = 0;
                const int startRow = 2;
                var nextRow = 0;
                var excelFile = new ExcelFile();
                var maxRowsInSheet = 65000;
                //var dailyAll = new DailyTransactionDTOList();
                #endregion
                #region add new sheet
                var totalSheets = totalRows % maxRowsInSheet > 0 ? (totalRows / maxRowsInSheet) + 1 : (totalRows / maxRowsInSheet);
                //var totalSheets = 5;
                int index = 0;
                for (int i = 0; i < totalSheets; i++)
                {
                    var keyCache = _keyDailyAllTxn + "_" + i;
                    DailyTransactionDTO dailyTxn = CacheExtension.Get<DailyTransactionDTO>(keyCache);
                    if (dailyTxn == null)
                    {
                        int counter = 100;
                        var pageIndex = i * maxRowsInSheet * counter;
                        dailyTxn = GetDailyAllTxnfromDB(date, type, creator, pageIndex, counter);
                    }
                    if (dailyTxn == null)
                    {
                        continue;
                    }
                    var ws = excelFile.Worksheets.Add("DailyTransacion" + (i + 1));
                    ws.Columns[0].Width = 10 * 256;
                    ws.Columns[1].Width = 20 * 256;
                    ws.Columns[2].Width = 10 * 256;
                    ws.Columns[3].Width = 30 * 256;
                    ws.Columns[4].Width = 10 * 256;
                    ws.Columns[5].Width = 10 * 256;
                    ws.Columns[6].Width = 15 * 256;
                    ws.Cells.GetSubrangeAbsolute(0, 0, 0, 4).Merged = true;
                    ws.Cells.GetSubrangeAbsolute(0, 0, 0, 4).Style.Font.Weight = ExcelFont.BoldWeight;
                    ws.Cells.GetSubrangeAbsolute(0, 0, 0, 4).Value = "Daily Transaction - " + string.Format("{0:MM/dd/yyyy}", date);

                    var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
                    ws.Cells[1, 0].Style = bgHeaderNormalFormat;
                    ws.Cells[1, 0].Value = "No";
                    ws.Cells[1, 1].Style = bgHeaderNormalFormat;
                    ws.Cells[1, 1].Value = "Date";
                    ws.Cells[1, 2].Style = bgHeaderNormalFormat;
                    ws.Cells[1, 2].Value = "User Name";
                    ws.Cells[1, 3].Style = bgHeaderNormalFormat;
                    ws.Cells[1, 3].Value = "Description";
                    ws.Cells[1, 4].Style = bgHeaderNormalFormat;
                    ws.Cells[1, 4].Value = "Type";
                    ws.Cells[1, 5].Style = bgHeaderNormalFormat;
                    ws.Cells[1, 5].Value = "Amount";
                    ws.Cells[1, 6].Style = bgHeaderNormalFormat;
                    ws.Cells[1, 6].Value = "Creator";

                    int r = 0, g = 0, b = 0;
                    //var totalRowinSheet = (totalSheets - 1 == i) ? totalRows - (maxRowsInSheet * i) : maxRowsInSheet;
                    for (int j = 0; j < dailyTxn.items.Count; j++)
                    {

                        if (j % 2 == 0)
                        {
                            r = 217;
                            g = 232;
                            b = 235;
                        }
                        else
                        {
                            r = 248;
                            g = 248;
                            b = 248;
                        }

                        var transDetail = dailyTxn.items[j];
                        ws.Cells[j + startRow, 0].Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                        ws.Cells[j + startRow, 0].Value = index + 1;
                        ws.Cells[j + startRow, 1].Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                        ws.Cells[j + startRow, 1].Value = transDetail.transDate;
                        ws.Cells[j + startRow, 2].Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                        ws.Cells[j + startRow, 2].Value = transDetail.Username;
                        ws.Cells[j + startRow, 3].Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                        ws.Cells[j + startRow, 3].Value = transDetail.Description;
                        ws.Cells[j + startRow, 4].Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                        ws.Cells[j + startRow, 4].Value = transDetail.Type;
                        ws.Cells[j + startRow, 5].Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                        ws.Cells[j + startRow, 5].Value = transDetail.Amount;
                        ws.Cells[j + startRow, 6].Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                        ws.Cells[j + startRow, 6].Value = transDetail.Creator;
                        index++;
                    }
                }

                #endregion

                var response = HttpContext.Current.Response;
                //response.Write("<script>alert('test');</script>");
                response.Clear();
                response.ContentType = "Application/x-msexcel";
                response.AddHeader("Content-Disposition", "attachment; filename=DailyTransaction-" + date + ".xls");
                excelFile.SaveXls(response.OutputStream);
            }
            catch (Exception ex)
            { }
        }

        #endregion


        public DailyTransactionDTO GetDailyTransData(string date, string type, int status, string creator, string verifyBy, int pageIndex, int pageSize, int fromcache)
        {
            var dailyFinance = new DailyTransactionDTO();
            var dailyFinancePaging = new DailyTransactionDTO();
            try
            {
               // var getdataSession = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyDailyFinanceTxn);
                var getdataSession = CacheExtension.Get<DailyTransactionDTO>(_keyDailyFinanceTxn);
                if (getdataSession == null || fromcache == 0)
                {
                    dailyFinance = GetDailyTransDatafromDB(date, type, status, creator, verifyBy);
                }
                else
                {
                    dailyFinance = (DailyTransactionDTO)getdataSession;
                }
                // convert object to html string
                dailyFinancePaging.items = dailyFinance.items.Select(c => c).Skip(PAGE_SIZE * (pageIndex - 1)).Take(PAGE_SIZE).ToList();

                dailyFinancePaging.totalRows = dailyFinance.totalRows;
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            return dailyFinancePaging;
        }

        public string GetFinanceTransactionContent(List<DailyTransaction> entityList, int TotalPages, int PageIndex)
        {
            string content = string.Empty;
            string itemTemplate = string.Empty;
            var strBuilder = new StringBuilder();
            bool hasUpdateTransPermission = Permissions.GetHostPermission(Enumerations.Functions.FinancialManagement.DailyFinanceTxn);

            if (hasUpdateTransPermission)
            {
                content = WebUtils.LoadTemplate("Transactions/DailyTransaction.htm");
                itemTemplate = WebUtils.LoadTemplate("Transactions/DailyTxnRow.htm");
            }
            else
            {
                content = WebUtils.LoadTemplate("Transactions/DailyTransactionReadOnly.htm");
                itemTemplate = WebUtils.LoadTemplate("Transactions/DailyTxnRowReadOnly.htm");
            }

            if (entityList.Count > 0)
            {
                #region Define params

                string itemContent = string.Empty;
                string rowStyle = string.Empty;

                //continous page index
                int rowIndex = (PageIndex - 1) * PAGE_SIZE;

                //render row by row data
                string prevRefNo = entityList[0].RefNo;
                //get dictionary list, 
                var userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
                Dictionary<string, int> list = userService.GetListTotalTransPerGroup(entityList);

                string actionTemplate = string.Empty;

                #endregion

                int rowIndexOnCurrentPage = 0;
                int totalVeirfyAllow = 0;
                foreach (DailyTransaction entity in entityList)
                {
                    rowIndex++;
                    rowIndexOnCurrentPage++;

                    itemContent = itemTemplate;

                    #region Assign to template data

                    TemplateHandler.Assign(ref itemContent, "{date}",
                                           entity.transDate);
                    
                    //change row style for transaction group
                    //use ref no to seperate row style
                    if (prevRefNo != entity.RefNo || rowIndexOnCurrentPage == 1)
                    {
                        prevRefNo = entity.RefNo;
                        rowStyle = (!rowStyle.Contains("evenrow")
                                        ? " class=\"evenrow ui-widget-content jqgrow ui-row-ltr\""
                                        : "class=\"ui-widget-content jqgrow ui-row-ltr\"");
                        TemplateHandler.Assign(ref itemContent, "{rowspan}", list[entity.RefNo]);
                    }
                    else
                    {
                        if (rowIndex > 1)
                        {
                            //do not remove first row
                            TemplateHandler.RemoveBlock(ref itemContent, "CheckBoxBlock");
                            TemplateHandler.RemoveBlock(ref itemContent, "UnVerifiedButtonBlock");
                        }
                        else
                        {
                            TemplateHandler.Assign(ref itemContent, "{rowspan}", list[entity.RefNo]);
                        }

                        //TemplateHandler.Assign(ref itemContent, "{rowspan}", list[entity.RefNo]);
                    }

                    //FormatNumber2 -> what does it do? what format
                    TemplateHandler.Assign(ref itemContent, "{amount}",
                                           FormatHandler.FormatNumber2(entity.Amount));
                    TemplateHandler.Assign(ref itemContent, "{no}", rowIndex);
                    TemplateHandler.Assign(ref itemContent, "{name}", entity.RefNo);
                    TemplateHandler.Assign(ref itemContent, "{rowStyle}", rowStyle);
                    TemplateHandler.Assign(ref itemContent, "{username}", entity.Username);
                    TemplateHandler.Assign(ref itemContent, "{description}", entity.Description);
                    TemplateHandler.Assign(ref itemContent, "{type}", entity.Type);
                    TemplateHandler.Assign(ref itemContent, "{description}", entity.Type);
                    TemplateHandler.Assign(ref itemContent, "{id}", string.Format("id='{0}'", entity.RefNo));

                    //Mergeable columns
                    TemplateHandler.Assign(ref itemContent, "{creator}", entity.Creator);
                    TemplateHandler.Assign(ref itemContent, "{verifiedby}", entity.CheckerName);
                    TemplateHandler.Assign(ref itemContent, "{verifiedtime}",
                                           ((CastAide.AsDateTime(entity.CheckedTime) != DateTime.MinValue)
                                                ? entity.transDate
                                                : string.Empty));


                    if (entity.CheckerId > 0)
                    {
                        /*
                     * the transaction had already VERIFIED
                     * disable verify checkbox, and set checked
                     * show Unverify button.
                     * But hide all action button if user doest not have permission
                     */
                        TemplateHandler.Assign(ref itemContent, "{checked}", "checked=checked");
                        TemplateHandler.Assign(ref itemContent, "{disabled}", "disabled=disabled");

                        actionTemplate = (hasUpdateTransPermission)
                                             ? string.Format(TEMPLATE_UNVERIFIED_BUTTON, entity.RefNo)
                                             : string.Empty;
                    }
                    else
                    {
                        /*
                     *  the transaction has NOT VERIFIED yet
                     *  enable checkbox for select
                     *  show verify button
                     */

                        TemplateHandler.Assign(ref itemContent, "{checked}", string.Empty);
                        TemplateHandler.Assign(ref itemContent, "{disabled}", string.Empty);
                        actionTemplate = (hasUpdateTransPermission)
                                             ? string.Format(TEMPLATE_VERIFIED_BUTTON, entity.RefNo)
                                             : string.Empty;

                        totalVeirfyAllow++;
                    }

                    TemplateHandler.Assign(ref itemContent, "{action}", actionTemplate);

                    #endregion

                    strBuilder.Append(itemContent);
                }
                TemplateHandler.Assign(ref content, "{disbaleAll}", totalVeirfyAllow > 0 ? "" : "disabled='disabled'");
                TemplateHandler.Assign(ref content, "{content}", strBuilder.ToString());
                TemplateHandler.Assign(ref content, "{totalpage}", TotalPages.ToString());

                /*
             *  Just show paging only if total page > 1             
             */

                TemplateHandler.Assign(ref content, "{footerpager}",
                                       (TotalPages > 1)
                                           ? Pager.InitPager(TotalPages, PageIndex, string.Empty)
                                           : string.Empty);
                TemplateHandler.Assign(ref content, "{fColspan}", "11");
            }
            else
            {
                /*
             *  Generate no information section
             */
                TemplateHandler.Assign(ref content, "{content}",
                                       AccCommon.GenNoInformation(((hasUpdateTransPermission) ? "10" : "9")));
                TemplateHandler.RemoveBlock(ref content, "noinfo");
                TemplateHandler.RemoveBlock(ref content, "havingdata");
                TemplateHandler.Assign(ref content, "{footerpager}", string.Empty);
                TemplateHandler.Assign(ref content, "{fColspan}", "10");
            }

            TemplateHandler.RemoveComment(ref content);
            TemplateHandler.ShrinkHtml(ref content);

            return content;
            
        }

        public DailyTransactionDTO GetDailyTransDatafromDB(string date, string type, int status, string creator, string verifyBy)
        {
            var dailyFinance = new DailyTransactionDTO();
            var _userDAO = CommonBootStrapper.Locator.GetInstance<IUserDAO>();
            dailyFinance = _userDAO.GetDailyTransaction(CastAide.AsDateTime(date), status, type, creator, verifyBy);
            //AppSession.AddSession(System.Web.HttpContext.Current.Session, _keyDailyFinanceTxn, dailyFinance);
            CacheExtension.Set(_keyDailyFinanceTxn, dailyFinance);
            var dailyTransactionDTO = CacheExtension.Get<DailyTransactionDTO>(_keyDailyFinanceTxn);
            return dailyFinance;
        }


      

        public string GetDailyTransData(string date, string type, int status, string creator, string verifyBy, int pageIndex, int fromcache)
        {
            string uiContent = string.Empty;
            int TotalPages = 0;
            var dailyFinance = new DailyTransactionDTO();
            var dailyFinancePaging = new DailyTransactionDTO();
            try
            {
                //var getdataSession = AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyDailyFinanceTxn);
                var getdataSession = CacheExtension.Get<DailyTransactionDTO>(_keyDailyFinanceTxn);
                if (getdataSession == null || fromcache == 0)
                {
                    dailyFinance = GetDailyTransDatafromDB(date, type, status, creator, verifyBy);
                }
                else
                {
                    dailyFinance = (DailyTransactionDTO)getdataSession;
                }
                // convert object to html string
                dailyFinancePaging.items = dailyFinance.items.Select(c => c).Skip(PAGE_SIZE * (pageIndex - 1)).Take(PAGE_SIZE).ToList();
                TotalPages = dailyFinance.totalRows / PAGE_SIZE;
                if ((dailyFinance.totalRows % PAGE_SIZE) != 0)
                    TotalPages++;
                uiContent = GetFinanceTransactionContent(date, status, type, creator, pageIndex, TotalPages, verifyBy, dailyFinancePaging.items);                                 
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
            }
            var data = new Hashtable();
            data.Add("Content", uiContent);
            data.Add("Totalpages", TotalPages);
            return JsonUtil.ToJson(data);
         
        }

        public void ExportToExcelDailyFinance(string date, int status, string type, string creator, string verifyBy)
        {
            var excelFile = new ExcelFile();
            var startRow = 2;
            var MaxRowInSheet = 65000;
            var objgetDailyAllTxn = (DailyTransactionDTO)CacheExtension.Get<DailyTransactionDTO>(_keyDailyFinanceTxn);
            var objdailyAllTxn = objgetDailyAllTxn != null
                                        ? (DailyTransactionDTO)objgetDailyAllTxn
                                        : GetDailyTransDatafromDB(date, type, status, creator, verifyBy);
            var totalRows = objdailyAllTxn.totalRows;
            var totalSheets = totalRows % MaxRowInSheet > 0 ? (totalRows / MaxRowInSheet) + 1 : (totalRows / MaxRowInSheet);
            string prevRefNo = objdailyAllTxn.items[0].RefNo;
            var index = 0;
            for (int i = 0; i < totalSheets; i++)
            {
                #region assign data
                var ws = excelFile.Worksheets.Add("DailyFinanceTransacion" + (i + 1));
                ws.Columns[0].Width = 5 * 256;
                ws.Columns[1].Width = 20 * 256;
                ws.Columns[2].Width = 10 * 256;
                ws.Columns[3].Width = 30 * 256;
                ws.Columns[4].Width = 10 * 256;
                ws.Columns[5].Width = 10 * 256;
                ws.Columns[6].Width = 15 * 256;
                ws.Columns[7].Width = 10 * 256;
                ws.Columns[8].Width = 20 * 256;
                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 4).Merged = true;
                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 4).Style.Font.Weight = ExcelFont.BoldWeight;
                ws.Cells.GetSubrangeAbsolute(0, 0, 0, 4).Value = "Daily Finance Transactions - " + string.Format("{0:MM/dd/yyyy}", date);
                var bgHeaderNormalFormat = new Core.Helper.ExcelGemboxUtils().GetStyleTitle(Color.FromArgb(205, 222, 255));
                ws.Cells[1, 0].Style = bgHeaderNormalFormat;
                ws.Cells[1, 0].Value = "No";
                ws.Cells[1, 1].Style = bgHeaderNormalFormat;
                ws.Cells[1, 1].Value = "Date";
                ws.Cells[1, 2].Style = bgHeaderNormalFormat;
                ws.Cells[1, 2].Value = "User Name";
                ws.Cells[1, 3].Style = bgHeaderNormalFormat;
                ws.Cells[1, 3].Value = "Description";
                ws.Cells[1, 4].Style = bgHeaderNormalFormat;
                ws.Cells[1, 4].Value = "Type";
                ws.Cells[1, 5].Style = bgHeaderNormalFormat;
                ws.Cells[1, 5].Value = "Amount";
                ws.Cells[1, 6].Style = bgHeaderNormalFormat;
                ws.Cells[1, 6].Value = "Creator";
                ws.Cells[1, 7].Style = bgHeaderNormalFormat;
                ws.Cells[1, 7].Value = "Verify by";
                ws.Cells[1, 8].Style = bgHeaderNormalFormat;
                ws.Cells[1, 8].Value = "Checked Time";
              
                //var objdailyAllTxn = (DailyTransactionDTO)AppSession.GetSession(System.Web.HttpContext.Current.Session, _keyDailyFinanceTxn);

                //objdailyAllTxn = objdailyAllTxn ?? GetDailyTransDatafromDB(date, type, status, creator, verifyBy);
                int r = 0, g = 0, b = 0;
                
                var userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
                Dictionary<string, int> list = userService.GetListTotalTransPerGroup(objdailyAllTxn.items);
                int rowIndexOnCurrentPage = 0;
                var bgRow1 = new Core.Helper.ExcelGemboxUtils().GetStyleText(217, 232, 235, HorizontalAlignmentStyle.Center);
                var bgRow2 = new Core.Helper.ExcelGemboxUtils().GetStyleText(248, 248, 248, HorizontalAlignmentStyle.Center);
                var totalRowinSheet = (totalSheets - 1 == i) ? totalRows - (MaxRowInSheet * i) : MaxRowInSheet;
                for (int j = 0; j < totalRowinSheet; j++)
                {

                    rowIndexOnCurrentPage++;
                    if (rowIndexOnCurrentPage == 1)
                    {
                        r = 217;
                        g = 232;
                        b = 235;
                    }
                    else
                    {
                        if (prevRefNo != objdailyAllTxn.items[index].RefNo)
                        {
                            if (r == 217)
                            {
                                r = 248;
                                g = 248;
                                b = 248;
                            }
                            else
                            {
                                r = 217;
                                g = 232;
                                b = 235;
                            }
                        }
                    }
                   
                    ws.Cells[j + startRow, 0].Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                    ws.Cells[j + startRow, 0].Value = rowIndexOnCurrentPage;
                    ws.Cells[j + startRow, 1].Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                    ws.Cells[j + startRow, 1].Value = objdailyAllTxn.items[index].transDate;
                    ws.Cells[j + startRow, 2].Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                    ws.Cells[j + startRow, 2].Value = objdailyAllTxn.items[index].Username;
                    ws.Cells[j + startRow, 3].Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                    ws.Cells[j + startRow, 3].Value = objdailyAllTxn.items[index].Description;
                    ws.Cells[j + startRow, 4].Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                    ws.Cells[j + startRow, 4].Value = objdailyAllTxn.items[index].Type;
                    ws.Cells[j + startRow, 5].Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                    ws.Cells[j + startRow, 5].Value = objdailyAllTxn.items[index].Amount;
                    if (prevRefNo != objdailyAllTxn.items[index].RefNo || rowIndexOnCurrentPage == 1)
                    {
                        int key = list[objdailyAllTxn.items[index].RefNo];
                        ws.Cells.GetSubrangeAbsolute(j + startRow, 6, j + startRow + key - 1, 6).Merged = true;
                        ws.Cells.GetSubrangeAbsolute(j + startRow, 6, j + startRow + key - 1, 6).Value = objdailyAllTxn.items[index].Creator;
                        ws.Cells.GetSubrangeAbsolute(j + startRow, 6, j + startRow + key - 1, 6).Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                        ws.Cells.GetSubrangeAbsolute(j + startRow, 7, j + startRow + key - 1, 7).Merged = true;
                        ws.Cells.GetSubrangeAbsolute(j + startRow, 7, j + startRow + key - 1, 7).Value = objdailyAllTxn.items[index].CheckerName;
                        ws.Cells.GetSubrangeAbsolute(j + startRow, 6, j + startRow + key - 1, 7).Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                        ws.Cells.GetSubrangeAbsolute(j + startRow, 8, j + startRow + key - 1, 8).Merged = true;
                        ws.Cells.GetSubrangeAbsolute(j + startRow, 8, j + startRow + key - 1, 8).Value = (CastAide.AsDateTime(objdailyAllTxn.items[index].CheckedTime) != DateTime.MinValue)
                                                    ? objdailyAllTxn.items[index].CheckedTime
                                                    : string.Empty;
                        ws.Cells.GetSubrangeAbsolute(j + startRow, 6, j + startRow + key - 1, 8).Style = new Core.Helper.ExcelGemboxUtils().GetStyleText(r, g, b, HorizontalAlignmentStyle.Center);
                    }
                    
                    prevRefNo = objdailyAllTxn.items[index].RefNo;
                    index++;
                }
                #endregion
                
            }
            var response = HttpContext.Current.Response;
            response.Clear();
            response.ContentType = "Application/x-msexcel";
            response.AddHeader("Content-Disposition", "attachment; filename=DailyFinanceTransaction-" + date + ".xls");
            excelFile.SaveXls(response.OutputStream);
        }

       
    }
}
