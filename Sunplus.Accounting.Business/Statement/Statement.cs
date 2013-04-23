using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;
using IBCUtils;

using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Core.CachingLib;
using Sunplus.Core.LoggingLib;
using Microsoft.Security.Application;

//using Sunplus.Core.LoggingLib;
//using Sunplus.Core.CachingLib;

namespace Sunplus.Accounting.Business.Statement
{
    /// <summary>
    /// Summary description for Statement
    /// </summary>
    public class StatementHTML
    {
        #region All page

        private const string TEMPLATE_UNVERIFIED_BUTTON =
            "<input type=\"button\" class=\"unverify\"  value=\"Unverify\" title=\"Unverify\"  onclick=\"return UnVerifyTrans(this);\"  id=\"{0}\"/>";

        private const string TEMPLATE_VERIFIED_BUTTON =
            "<input type=\"button\" class=\"verified\" value=\"Verify\" title=\"Verify\"  onclick=\"return VerifyTrans(this);\"  id=\"{0}\"/>";

        /// <summary>
        /// 
        /// </summary>
        /// <returns>html string</returns>
        public static string GetAuditStatement()
        {
            if (string.IsNullOrEmpty(AppSession.CustUName)
                || AppSession.CustID == 0)
            {
                return AlertMessage.GetMsg(IbcError.SESSION_EXPIRED_MSG, AlertMessage.MsgType.Error);
            }
            else if (AppSession.CustRoleId == 0)
            {
                return AlertMessage.GetMsg(IbcError.REPORT_NOT_AVAILABLE_MSG, AlertMessage.MsgType.Error);
            }

            string tpl = WebUtils.LoadTemplate("Statement/AuditContent.htm");
            string excelTpl = tpl;
            var htmlReport = new StringBuilder();
            var excelRows = new StringBuilder();
            try
            {
                string rowTpl = TemplateHandler.CopyBlock(tpl, "ReportContent");

                DbResult ds = null;
                decimal total2 = 0;
                int index = 0;
                //ds = AccountStatement.Acc_AuditStatement(CastAide.AsInt32
                //    (HttpContext.Current.Session["CustId"]));

                var userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
                int custId = userService.GetCustomerIdByUsername(
                    WebUtils.GetQueryStringValue("custname", string.Empty));

                //ds = AccountStatement.Acc_AuditStatement(AppSession.CustID);

                if (custId > 0)
                {
                    ds = AccountStatement.Acc_AuditStatement(custId);

                    if (ds.ResultSet.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.ResultSet.Tables[0].Rows)
                        {
                            index++;
                            string rowHtml = rowTpl;
                            //string accDate = FormatHandler.FormatDateOptionalTime(row["accdate"]);
                            string strAccDate = string.Empty;
                            DateTime accDate = CastAide.AsDateTime(row["accdate"]);
                            string remark = CastAide.AsString(row["transdesc"]);
                            string type = CastAide.AsString(row["transtype"]);
                            decimal total = CastAide.AsDecimal(row["totalamt"]);
                            total2 += total;

                            strAccDate = accDate.DayOfWeek + " <!--nobr--><br><!--/nobr-->" +
                                         FormatHandler.FormatDateOptionalTime(accDate);
                            TemplateHandler.Assign(ref rowHtml, "{Date}", strAccDate);

                            TemplateHandler.Assign(ref rowHtml, "{Total2}", FormatHandler.FormatNumber20(total2));

                            if (index%2 != 0)
                                TemplateHandler.Assign(ref rowHtml, "{odd_even}", "evenrow");
                            else
                                TemplateHandler.Assign(ref rowHtml, "odd_even", string.Empty);

                            TemplateHandler.Assign(ref rowHtml, "{Remark}", remark);
                            TemplateHandler.Assign(ref rowHtml, "{Type}", type);
                            TemplateHandler.Assign(ref rowHtml, "{Total}", FormatHandler.FormatNumber20(total));

                            htmlReport.Append(rowHtml);
                            TemplateHandler.RemoveBlock(ref rowHtml, "nobr");
                            excelRows.Append(rowHtml);
                        }

                        TemplateHandler.ReplaceBlock(ref tpl, "ReportContent", htmlReport.ToString());
                        //Excel
                        TemplateHandler.ReplaceBlock(ref excelTpl, "ReportContent", excelRows.ToString());
                    }
                    else
                    {
                        TemplateHandler.ReplaceBlock(ref tpl, "ReportContent", AccCommon.GenNoInformation("5"));
                        //Excel
                        TemplateHandler.ReplaceBlock(ref excelTpl, "ReportContent", AccCommon.GenNoInformation("5"));
                    }
                }
                else
                {
                    TemplateHandler.ReplaceBlock(ref tpl, "ReportContent", AccCommon.GenNoInformation("5"));
                    //Excel
                    TemplateHandler.ReplaceBlock(ref excelTpl, "ReportContent", AccCommon.GenNoInformation("5"));
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                TemplateHandler.ReplaceBlock(ref tpl, "ReportContent", AccCommon.GenNoInformation("5"));
                //Excel
                TemplateHandler.ReplaceBlock(ref excelTpl, "ReportContent", AccCommon.GenNoInformation("5"));
            }

            ReportCache.REPORT_STATEMENT_AUDIT = excelTpl;
            return tpl;
        }

        /// <summary>
        /// Anders: if from date and todate datatype is tring
        /// how can we perform compare, convert to datetime?
        /// </summary>
        /// <param name="fdate"></param>
        /// <param name="tdate"></param>
        /// <returns></returns>
        public static string GetCompanyStatement(string fdate, string tdate)
        {
            string contentTemplate = WebUtils.LoadTemplate("Statement/CompanyStatementContent.htm");
            string excelContent = contentTemplate;

            var htmlReport = new StringBuilder();
            var excelRows = new StringBuilder();
            DateTime fdt, tdt;
            try
            {
                if (!Validator.ValidDateMMDDYYYYNotNull(fdate, out fdt).IsSuccess())
                    //if (HttpContext.Current.Session["fdate"] != null)
                    //    fdt = (DateTime)HttpContext.Current.Session["fdate"];
                    //else
                    //    fdt = DateTime.Now.AddDays(-1);
                {
                    fdt = DateTime.Now;
                }


                if (!Validator.ValidDateMMDDYYYYNotNull(tdate, out tdt).IsSuccess())
                    //if (HttpContext.Current.Session["tdate"] != null)
                    //    tdt = (DateTime)HttpContext.Current.Session["tdate"];
                    //else
                    //    tdt = DateTime.Now;
                {
                    tdt = DateTime.Now;
                }

                //HttpContext.Current.Session["fdate"] = fdt;
                //HttpContext.Current.Session["tdate"] = tdt;


                //if (HttpContext.Current.Session["fdate"] == null && HttpContext.Current.Session["tdate"] == null)
                //    return AlertMessage.GetMsg(IbcError.DATE_INVALID_FORMAT_MSG, AlertMessage.MsgType.Error);

                string itemTemplate = TemplateHandler.CopyBlock(contentTemplate, "ReportContent");

                DbResult ds = null;
                //what does it means
                decimal sumAmount = 0;
                int index = 0;
                ds = AccountStatement.Acc_CompanyStatement(fdt, tdt);
                if (ds.ResultSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.ResultSet.Tables[0].Rows)
                    {
                        index++;
                        string rowHtml = itemTemplate;
                        //string accDate = FormatHandler.FormatDateOptionalTime(row["accdate"]);
                        DateTime accDate = CastAide.AsDateTime(row["accdate"]);
                        string strAccDate = string.Empty;
                        string type = CastAide.AsString(row["transtype"]);
                        decimal totalamt = CastAide.AsDecimal(row["totalamt"]);
                        sumAmount += totalamt;


                        //just provide hyperlink for non-bet transaction
                        if (string.Compare(type.ToLower(), "bet", true) == 0)
                        {
                            //TemplateHandler.RemoveBlock(ref rowHtml,"link");
                            TemplateHandler.Assign(ref rowHtml, "link", string.Empty);
                            strAccDate = accDate.DayOfWeek + " <!--nobr--><br><!--/nobr-->" +
                                         FormatHandler.FormatDateOptionalTime(accDate);
                            TemplateHandler.Assign(ref rowHtml, "{Date}", strAccDate);
                        }
                        else
                        {
                            //TemplateHandler.Assign(ref rowHtml, "{link}", "class='canchange' onclick = 'ShowByDayAndType(\"" + type + "\",\"" + FormatHandler.FormatDateOptionalTime(accDate) + "\")'");
                            TemplateHandler.Assign(ref rowHtml, "{link}",
                                                   "class='canchange' onclick = 'viewDetail(\"" + type + "\",\"" +
                                                   FormatHandler.FormatDateOptionalTime(accDate) + "\")'");

                            strAccDate = accDate.DayOfWeek + " <!--nobr--><br><!--/nobr-->" +
                                         FormatHandler.FormatDateOptionalTime(accDate);
                            TemplateHandler.Assign(ref rowHtml, "{Date}", strAccDate);
                        }

                        //FormatNumber20 what does it mean
                        TemplateHandler.Assign(ref rowHtml, "{Total2}", FormatHandler.FormatNumber20(sumAmount));

                        //if (index % 2 != 0)
                        //    TemplateHandler.Assign(ref rowHtml, "odd_even", "evenrow");
                        //else
                        //    TemplateHandler.Assign(ref rowHtml, "odd_even", string.Empty);

                        TemplateHandler.Assign(ref rowHtml, "odd_even", (index%2 != 0) ? "evenrow" : string.Empty);

                        TemplateHandler.Assign(ref rowHtml, "{Type}", type);
                        TemplateHandler.Assign(ref rowHtml, "{Total}", FormatHandler.FormatNumber20(totalamt));


                        htmlReport.Append(rowHtml);
                        TemplateHandler.RemoveBlock(ref rowHtml, "nobr");
                        excelRows.Append(rowHtml);
                    }
                    TemplateHandler.ReplaceBlock(ref contentTemplate, "ReportContent", htmlReport.ToString());
                    //Excel
                    TemplateHandler.ReplaceBlock(ref excelContent, "ReportContent", excelRows.ToString());
                }
                else
                {
                    TemplateHandler.ReplaceBlock(ref contentTemplate, "ReportContent", AccCommon.GenNoInformation("5"));
                    //Excel
                    TemplateHandler.ReplaceBlock(ref excelContent, "ReportContent", AccCommon.GenNoInformation("5"));
                }
            }
            catch (Exception ex)
            {
                //Logs.Write(ex.Message);
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }

                TemplateHandler.ReplaceBlock(ref contentTemplate, "ReportContent", AccCommon.GenNoInformation("5"));
                //Excel
                TemplateHandler.ReplaceBlock(ref excelContent, "ReportContent", AccCommon.GenNoInformation("5"));
            }
            ReportCache.REPORT_STATEMENT_COMPANY = excelContent;
            return contentTemplate;
        }

        public static string GetDetailStatement(string fdate, string tdate)
        {
            if (string.IsNullOrEmpty(AppSession.CustUName)
                || AppSession.CustID == 0)
            {
                return AlertMessage.GetMsg(IbcError.SESSION_EXPIRED_MSG, AlertMessage.MsgType.Error);
            }
            else if (AppSession.CustRoleId == 0)
            {
                return AlertMessage.GetMsg(IbcError.REPORT_NOT_AVAILABLE_MSG, AlertMessage.MsgType.Error);
            }

            string tpl = WebUtils.LoadTemplate("Statement/DetailContent.htm");
            var htmlReport = new StringBuilder();
            DateTime fdt, tdt;
            bool candelete = false;
            try
            {
                if (!Validator.ValidDateMMDDYYYYNotNull(fdate, out fdt).IsSuccess())
                {
                    //if (HttpContext.Current.Session["fdate"] != null)
                    //    fdt = (DateTime)HttpContext.Current.Session["fdate"];
                    //else
                    //fdt = DateTime.Now.AddDays(-1);
                    fdt = DateTime.Now;
                }
                if (!Validator.ValidDateMMDDYYYYNotNull(tdate, out tdt).IsSuccess())
                {
                    //if (HttpContext.Current.Session["tdate"] != null)
                    //    tdt = (DateTime)HttpContext.Current.Session["tdate"];
                    //else
                    tdt = DateTime.Now;
                }

                //HttpContext.Current.Session["fdate"] = fdt;
                //HttpContext.Current.Session["tdate"] = tdt;


                //if (HttpContext.Current.Session["fdate"] == null && HttpContext.Current.Session["tdate"] == null)
                //    return AlertMessage.GetMsg(IbcError.DATE_INVALID_FORMAT_MSG, AlertMessage.MsgType.Error);


                string rowTpl = TemplateHandler.CopyBlock(tpl, "ReportContent");

                DbResult ds = null;
                int index = 0;
                decimal total = 0;

                var userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
                int custId = userService.GetCustomerIdByUsername(
                    WebUtils.GetQueryStringValue("custname", string.Empty));

                //ds = AccountStatement.Acc_DetailStatement(fdt, tdt,
                //    AppSession.CustID);

                if (custId > 0)
                {
                    ds = AccountStatement.Acc_DetailStatement(fdt, tdt,
                                                              custId);


                    if (ds.ResultSet.Tables[0].Rows.Count > 0)
                    {
                        candelete = Permissions.GetHostPermission(Enumerations.Functions.Transfer.DeleteTrans);
                        foreach (DataRow row in ds.ResultSet.Tables[0].Rows)
                        {
                            index++;
                            string rowHtml = rowTpl;
                            string transDate = FormatHandler.FormatDateOptionalTime(row["transdate"]);
                            string remark = CastAide.AsString(row["transdesc"]);
                            string creator = CastAide.AsString(row["creator"]);
                            decimal creditAmt = CastAide.AsDecimal(row["creditamt"]);
                            decimal debitAmt = CastAide.AsDecimal(row["debitamt"]);

                            if (index%2 != 0)
                                TemplateHandler.Assign(ref rowHtml, "odd_even", "evenrow");
                            else
                                TemplateHandler.Assign(ref rowHtml, "odd_even", string.Empty);

                            TemplateHandler.Assign(ref rowHtml, "{Remark}", remark);
                            TemplateHandler.Assign(ref rowHtml, "{Date}", transDate);
                            TemplateHandler.Assign(ref rowHtml, "{Creator}", creator);
                            TemplateHandler.Assign(ref rowHtml, "{Refno}", CastAide.AsString(row["refno"]));
                            TemplateHandler.Assign(ref rowHtml, "{Debit}", FormatHandler.FormatNumber20(debitAmt));
                            TemplateHandler.Assign(ref rowHtml, "{Credit}", FormatHandler.FormatNumber20(creditAmt));
                            TemplateHandler.Assign(ref rowHtml, "{Total}", FormatHandler.FormatNumber20(row["runningtotal"]));

                            total += creditAmt - debitAmt;
                            if (!CastAide.AsBoolean(row["allowdelete"], true) || !candelete)
                                TemplateHandler.RemoveBlock(ref rowHtml, "DelLink");

                            htmlReport.Append(rowHtml);
                        }
                        TemplateHandler.ReplaceBlock(ref tpl, "ReportContent", htmlReport.ToString());

                        //total
                        TemplateHandler.Assign(ref tpl, "{total}", FormatHandler.FormatNumber40(total));
                    }
                    else
                    {
                        TemplateHandler.ReplaceBlock(ref tpl, "ReportContent", AccCommon.GenNoInformation("8"));
                        TemplateHandler.RemoveBlock(ref tpl, "ReportFooter");
                    }
                }
                else
                {
                    TemplateHandler.ReplaceBlock(ref tpl, "ReportContent", AccCommon.GenNoInformation("8"));
                    TemplateHandler.RemoveBlock(ref tpl, "ReportFooter");
                }
            }
            catch (Exception ex)
            {
                //Logs.Write(ex.Message);
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }

                TemplateHandler.ReplaceBlock(ref tpl, "ReportContent", AccCommon.GenNoInformation("8"));
                TemplateHandler.RemoveBlock(ref tpl, "ReportFooter");
            }
            ReportCache.REPORT_STATEMENT_DETAIL = tpl;
            return tpl;
        }

        //public static DateTime? FromDate
        //{
        //    get
        //    {
        //        DateTime? fromDate = null;
        //        if (HttpContext.Current.Session["fdate"] != null)
        //        {
        //            DateTime now = DateTime.Now;
        //            DateTime.TryParse(HttpContext.Current.Session["fdate"].ToString(), out now);
        //            fromDate = now;
        //        }

        //        return fromDate;
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["fdate"] = value;
        //    }
        //}


        //public static DateTime? ToDate
        //{
        //    get
        //    {
        //        DateTime? toDate = null;
        //        if (HttpContext.Current.Session["tdate"] != null)
        //        {
        //            DateTime now = DateTime.Now;
        //            DateTime.TryParse(HttpContext.Current.Session["tdate"].ToString(), out now);
        //            toDate = now;
        //        }

        //        return toDate;
        //    }
        //    set
        //    {
        //        HttpContext.Current.Session["tdate"] = value;
        //    }
        //}

        public static string GetMemberStatement(string fromDate, string toDate)
        {
            if (string.IsNullOrEmpty(AppSession.CustUName)
                || AppSession.CustID == 0)
            {
                return AlertMessage.GetMsg(IbcError.SESSION_EXPIRED_MSG, AlertMessage.MsgType.Error);
            }
            else if (AppSession.CustRoleId == 0)
            {
                return AlertMessage.GetMsg(IbcError.REPORT_NOT_AVAILABLE_MSG, AlertMessage.MsgType.Error);
            }


            //get persmission of logined user
            bool hasUpdateTransPermission = Permissions.GetHostPermission
                (Enumerations.Functions.FinancialManagement.DailyFinanceTxn);

            string contentTemplate = (hasUpdateTransPermission)
                                         ? WebUtils.LoadTemplate("Statement/MemberSatement.htm")
                                         : WebUtils.LoadTemplate("Statement/MemberSatementReadOnly.htm");

            string excelTpl = contentTemplate;
            var htmlReport = new StringBuilder();
            //StringBuilder excelRows = new StringBuilder();
            DateTime fdt, tdt, balanceUpDay;

            try
            {
                balanceUpDay = ReportUtils.BalanceUp.BalanceUpDay;
                if (!Validator.ValidDateMMDDYYYYNotNull(fromDate, out fdt).IsSuccess())
                {
                    fdt = DateTime.Now.AddDays(-1);
                }

                if (!Validator.ValidDateMMDDYYYYNotNull(toDate, out tdt).IsSuccess())
                {
                    tdt = DateTime.Now;
                }


                string itemTemplate = TemplateHandler.CopyBlock(contentTemplate, "ReportContent");
                //DbResult ds = null;
                double total2 = 0;
                int idx = 0;
                double OpenBalance = 0, IsBalance = 0, linetotal = 0;
                int gotopen = 0;
                bool includeAccBal = false;

                if (WebSettings.IncludeAccuBalInStatement
                    && (fdt != balanceUpDay))
                {
                    includeAccBal = true;
                }

                var customer = new User();
                customer.Username = WebUtils.GetQueryStringValue("custname", string.Empty);
                var userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
                List<Entities.Statement> entityList = userService.GetCustomerStatement(
                    customer, fdt, tdt, balanceUpDay, includeAccBal);


                if (entityList.Count > 0)
                {
                    int totalUnVerify = 0;

                    #region foreach (Statement entity in entityList)                

                    foreach (Entities.Statement entity in entityList)
                    {
                        idx++;
                        string rowHtml = itemTemplate;
                        //string accDate = FormatHandler.FormatDateOptionalTime(row["accdate"]);
                        DateTime accDate = entity.CreatedDate; //CastAide.AsDateTime(row["accdate"]);
                        string strAccDate = string.Empty;
                        //what is it .Replace("<", "&lt;"); what do you mean?
                        //string remark =  entity.TransDescription.Replace("<", "&lt;");// CastAide.AsString(row["transdesc"]).Replace("<", "&lt;");
                        string remark = AntiXss.HtmlEncode(entity.TransDescription);
                        string type = entity.TransType; // CastAide.AsString(row["transtype"]);
                        
                        string creator = entity.Creator.Username; // CastAide.AsString(row["creator"]);
                        double total = entity.TotalAmount; //CastAide.AsDecimal(row["totalamt"]);
                        total2 += total;

                        #region Processing for balance

                        bool isAccBal = string.Compare(
                            remark.ToLowerInvariant(),
                            "accumulated balance", true) == 0;


                        if (string.Compare(type.ToLowerInvariant(), "bet", true) == 0
                            && accDate >= balanceUpDay)
                        {
                            TemplateHandler.Assign(ref rowHtml, "link",
                                                   "class='canchange' onclick='viewDetail(\""
                                                   + FormatHandler.FormatDateOptionalTime(accDate) + "\"," + entity.TransTypeId + ")' ");
                            strAccDate = accDate.DayOfWeek
                                         + " <!--nobr--><br><!--/nobr-->"
                                         + FormatHandler.FormatDateOptionalTime(accDate);
                            TemplateHandler.Assign(ref rowHtml, "{Date}", strAccDate);
                        }
                        else
                        {
                            TemplateHandler.Assign(ref rowHtml, "link", string.Empty);

                            //Do not show date at ACCUMULATED BALANCE row
                            if (isAccBal)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{Date}", "--");
                            }
                            else
                            {
                                strAccDate = accDate.DayOfWeek
                                             + " <!--nobr--><br><!--/nobr-->"
                                             + FormatHandler.FormatDateOptionalTime(accDate);

                                TemplateHandler.Assign(ref rowHtml, "{Date}", strAccDate);
                            }
                        }

                        //Total 2
                        //what is it
                        int index = remark.ToLower().IndexOf("opening balance");
                        if (index >= 0 && type.ToLower().Equals("bal"))
                        {
                            if (gotopen == 0)
                            {
                                gotopen = 1;
                                OpenBalance = total;
                                if(idx == 1)
                                    linetotal = OpenBalance;
                            }

                            IsBalance = 1;
                        }
                        else
                        {
                            linetotal += total;
                            IsBalance = 0;
                        }


                        if (IsBalance == 1)
                        {
                            if (Math.Abs(linetotal - total) > 1)
                            {
                                string t =
                                    "NOT EQUAL <!--nobr--><br><!--/nobr--><b>RESET Total <!--nobr--><br><!--/nobr-->Diff:" +
                                    (linetotal - total);
                                TemplateHandler.Assign(ref rowHtml, "{Total2}", t);
                                TemplateHandler.Assign(ref rowHtml, "{OkClass}", "class='f3pink'");
                                linetotal = total;
                            }
                            else
                            {
                                TemplateHandler.Assign(ref rowHtml, "{Total2}", "OK");
                                TemplateHandler.Assign(ref rowHtml, "{OkClass}", "class='f3green'");
                            }
                        }
                        else
                        {
                            TemplateHandler.Assign(ref rowHtml, "{Total2}",
                                                   FormatHandler.FormatNumber20(linetotal));
                            if (linetotal < 0)
                            {
                                TemplateHandler.Assign(ref rowHtml, "{OkClass}", " class='text_red' ");
                            }
                            else
                            {
                                TemplateHandler.Assign(ref rowHtml, "{Total2}", string.Empty);
                            }
                        }


                        if (isAccBal)
                        {
                            TemplateHandler.Assign(ref rowHtml, "odd_even", "statement-accu-row");
                        }

                        #endregion

                        TemplateHandler.Assign(ref rowHtml, "odd_even",
                                               (idx%2 != 0) ? "evenrow" : string.Empty);
                        TemplateHandler.Assign(ref rowHtml, "{Remark}",
                                               type.ToUpper() + " <!--nobr--><br><!--/nobr-->-" + remark);
                        TemplateHandler.Assign(ref rowHtml, "{Total}",
                                               FormatHandler.FormatNumber20(total));

                        //show red for negative value
                        TemplateHandler.Assign(ref rowHtml, "{cssNegative}",
                                               (total < 0) ? " class='text_red' " : string.Empty);

                        TemplateHandler.Assign(ref rowHtml, "{Creator}",
                                               (string.IsNullOrEmpty(creator) ? "--" : creator));

                        //verify section
                        string templateAction = string.Empty;

                        #region Processing verify status

                        //if this transaction is verified already
                        //disable checkbox verify, show button unverify
                        if (entity.IsVerified)
                        {
                            TemplateHandler.Assign(ref rowHtml, "{verifiedby}", entity.Checker.Username);

                            //add action unverify for verified transaction
                            if (entity.IsAllowedVerified)
                            {
                                templateAction = string.Format(TEMPLATE_UNVERIFIED_BUTTON,
                                                               entity.RefNo);

                                TemplateHandler.Assign(ref rowHtml, "{checked}", "checked=checked");
                                TemplateHandler.Assign(ref rowHtml, "{disabled}", "disabled=disabled");
                            }
                            else
                            {
                                TemplateHandler.RemoveBlock(ref rowHtml, "verifycheckbox");
                                TemplateHandler.Assign(ref rowHtml, "{checked}", string.Empty);
                                TemplateHandler.Assign(ref rowHtml, "{disabled}", string.Empty);
                            }
                        }
                        else
                        {
                            TemplateHandler.Assign(ref rowHtml, "{verifiedby}", string.Empty);

                            //add action verify for unverified transaction
                            if (entity.IsAllowedVerified)
                            {
                                templateAction = string.Format(TEMPLATE_VERIFIED_BUTTON,
                                                               entity.RefNo);
                                TemplateHandler.Assign(ref rowHtml, "{checked}", string.Empty);
                                TemplateHandler.Assign(ref rowHtml, "{disabled}", string.Empty);

                                totalUnVerify++;
                            }
                            else
                            {
                                TemplateHandler.RemoveBlock(ref rowHtml, "verifycheckbox");
                                TemplateHandler.Assign(ref rowHtml, "{checked}", "checked=checked");
                                TemplateHandler.Assign(ref rowHtml, "{disabled}", "disabled=disabled");
                            }
                        }

                        #endregion


                        var checkTime = string.Empty;
                        if(entity.CheckedTime.HasValue)
                        {
                            checkTime = Convert.ToDateTime(entity.CheckedTime).ToString();
                        }

                        TemplateHandler.Assign(ref rowHtml, "{checkedtime}", checkTime);

                        TemplateHandler.Assign(ref rowHtml, "{action}", templateAction);
                        TemplateHandler.Assign(ref rowHtml, "{id}", entity.RefNo);

                        htmlReport.Append(rowHtml);
                        TemplateHandler.RemoveBlock(ref rowHtml, "nobr");

                        //excelRows.Append(rowHtml);
                    }

                    #endregion

                    TemplateHandler.ReplaceBlock(ref contentTemplate, "ReportContent", htmlReport.ToString());
                    //if there is no transaction having status allow verify
                    //then remove button verify multiple, correspoing checkbox column
                    //show the same format with readonly
                    int firstIndex = entityList.FindIndex(statement => statement.IsAllowedVerified);
                    if (firstIndex < 0)
                    {
                        TemplateHandler.RemoveBlock(ref contentTemplate, "verifyheader");
                        //TemplateHandler.RemoveBlock(ref contentTemplate, "verifycheckbox");
                    }

                    //for excel

                    if (totalUnVerify == 0)
                        TemplateHandler.Assign(ref contentTemplate, "{AllVerifyDisabled}", "disabled='disabled'");
                    else
                        TemplateHandler.Assign(ref contentTemplate, "{AllVerifyDisabled}", string.Empty);
                }
                else
                {
                    RenderNoInoSection(ref contentTemplate, ref excelTpl);
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }

                RenderNoInoSection(ref contentTemplate, ref excelTpl);
            }


            //AppCache.REPORT_STATEMENT = excelTpl;
            excelTpl = contentTemplate.Clone().ToString();
            TemplateHandler.RemoveBlock(ref excelTpl, "verifyheader");
            //TemplateHandler.RemoveBlock(ref excelTpl, "verifycheckbox");
            TemplateHandler.RemoveBlock(ref excelTpl, "action");
            TemplateHandler.RemoveComment(ref excelTpl);


            string key = "CustomerStatement" + AppSession.AccUserName;
            AppSession.AddSession(HttpContext.Current.Session, key, excelTpl);
            //TemplateHandler.RemoveComment(ref contentTemplate, );
            
            //AppCache.REPORT_STATEMENT = excelTpl;

            //TemplateHandler.RemoveComment(ref contentTemplate);
            return contentTemplate;
        }

        private static void RenderNoInoSection(ref string contentTemplate, ref string excelTpl)
        {
            //Generate no informaction section
            TemplateHandler.RemoveBlock(ref contentTemplate, "verifyheader");
            TemplateHandler.ReplaceBlock(ref contentTemplate, "ReportContent", AccCommon.GenNoInformation("8"));

            //for excel
            TemplateHandler.ReplaceBlock(ref excelTpl, "ReportContent", AccCommon.GenNoInformation("8"));
            TemplateHandler.RemoveBlock(ref excelTpl, "verifyheader");
        }

        #endregion Methods
    }
}