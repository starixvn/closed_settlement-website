using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Text;
using System.Web;
using DNAide.Core;
using DNAide.Utils;
using DNAide.Web;

using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Constant;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Core.Common
{
    /// <summary>
    /// Summary description for CalendarControl
    /// </summary>
    public static class AccCommon
    {
        public const int pageSize = 50;

        private static string vers = string.Empty;


        public static string FuncMgr
        {
            get { return CastAide.AsString(ConfigurationManager.AppSettings["funcMgr"]); }
        }

        public static bool IsLocalTest
        {
            get { return (CastAide.AsString(ConfigurationManager.AppSettings["LocalTest"]) == "1"); }
        }


        /// <summary>
        /// return version of js and css file
        /// </summary>
        /// <returns></returns>
        public static string Version()
        {
            if (string.IsNullOrEmpty(vers)) vers = ConfigurationManager.AppSettings["version"] ?? string.Empty;
            return vers;
        }

        public static bool IsDummySuper(int custid)
        {
            return (custid < 100);
        }
   

        public static string GenReportFooter(string url, int pageNum, int cpage, int colsNo)
        {
            try
            {
                string pageIndexTpl = WebUtils.LoadTemplate("PageIndex.htm");
                string footerHtml = WebUtils.LoadTemplate("ReportFooterPaging.htm");
                TemplateHandler.Assign(ref footerHtml, "{CurrentPage}", cpage.ToString());
                TemplateHandler.Assign(ref footerHtml, "{Colspan}", colsNo.ToString());
                string pageList = string.Empty;

                for (int i = 0; i < pageNum; ++i)
                {
                    var pageIndex = (string) pageIndexTpl.Clone();

                    TemplateHandler.Assign(ref pageIndex, "{Url}", url);
                    if (cpage == (i + 1))
                    {
                        TemplateHandler.Assign(ref pageIndex, "{Page}", string.Empty);
                        TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", "" + (i + 1));
                    }
                    else
                    {
                        TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
                        TemplateHandler.Assign(ref pageIndex, "{Page}", "" + (i + 1));
                    }
                    if (i%20 == 0 && i > 0)
                        pageList += "<br />" + pageIndex;
                    else
                        pageList += pageIndex;
                }
                if (pageList.Length > 0)
                    TemplateHandler.Assign(ref footerHtml, "{PageList}", pageList.Remove(pageList.Length - 1));
                else
                    TemplateHandler.Assign(ref footerHtml, "{PageList}", string.Empty);
                return footerHtml;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }

        public static string GenReportFooter_1(string url, int pageNum, int cpage, int colsNo)
        {
            try
            {
                string pageIndexTpl = WebUtils.LoadTemplate("PageIndex1.htm");
                string footerHtml = WebUtils.LoadTemplate("ReportFooterPaging1.htm");
                string GotoPageHtml = WebUtils.LoadTemplate("GotoPage.htm");
                TemplateHandler.Assign(ref footerHtml, "{CurrentPage}", cpage.ToString());
                TemplateHandler.Assign(ref footerHtml, "{Colspan}", colsNo.ToString());
                string pageList = string.Empty;

                int startpage, endpage;
                startpage = cpage - 3;
                if (startpage < 1)
                    startpage = 0;

                endpage = startpage + 5;
                if (endpage > pageNum)
                    endpage = pageNum;

                if ((endpage - startpage) < 5 && (cpage < pageNum) && (pageNum - 5 > 0))
                {
                    startpage = pageNum - 5;
                    endpage = pageNum;
                }

                if ((endpage + 1) == pageNum)
                    endpage = pageNum;

                if (cpage >= 4)
                {
                    var pageIndex = (string) pageIndexTpl.Clone();
                    TemplateHandler.Assign(ref pageIndex, "{Url}", url);
                    TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
                    TemplateHandler.Assign(ref pageIndex, "{PageText}", "1");
                    TemplateHandler.Assign(ref pageIndex, "{Page}", "1");
                    if ((cpage - 2) == 2)
                        pageList += pageIndex;
                    else
                        pageList += pageIndex + " ... ";
                }

                if ((endpage == cpage) && (endpage - 5) > 0)
                    startpage = endpage - 5;

                for (int i = startpage; i < endpage; ++i)
                {
                    var pageIndex = (string) pageIndexTpl.Clone();

                    TemplateHandler.Assign(ref pageIndex, "{Url}", url);
                    if (cpage == (i + 1))
                    {
                        pageIndex = "<strong>" + cpage + "</strong>";
                    }
                    else
                    {
                        TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
                        TemplateHandler.Assign(ref pageIndex, "{PageText}", "" + (i + 1));
                        TemplateHandler.Assign(ref pageIndex, "{Page}", "" + (i + 1));
                    }
                    pageList += pageIndex;
                }

                if ((endpage + 1) < pageNum)
                {
                    var pageIndex = (string) pageIndexTpl.Clone();
                    TemplateHandler.Assign(ref pageIndex, "{Url}", url);
                    TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
                    TemplateHandler.Assign(ref pageIndex, "{PageText}", " " + pageNum);
                    TemplateHandler.Assign(ref pageIndex, "{Page}", " " + pageNum);
                    pageList += " ... " + pageIndex;
                }

                if (pageList.Length > 0)
                {
                    TemplateHandler.Assign(ref footerHtml, "{PageList}", pageList.Remove(pageList.Length - 1));
                    TemplateHandler.Assign(ref footerHtml, "{GotoPage}", GotoPageHtml);
                    TemplateHandler.Assign(ref footerHtml, "{Url}", url);
                }
                else
                {
                    TemplateHandler.Assign(ref footerHtml, "{PageList}", "<strong>1</strong>");
                    TemplateHandler.Assign(ref footerHtml, "{Url}", "");
                }
                return footerHtml;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }

        public static string Acc_GenPager(int pageNum, int cpage, string jsonclick)
        {
            try
            {
                string pagerHtml = WebUtils.LoadTemplate("AccPager.htm");
                string pageItem = TemplateHandler.CopyBlock(pagerHtml, "PageItem");
                TemplateHandler.RemoveBlock(ref pagerHtml, "PageItem");
                string pageIndex = "";
                TemplateHandler.Assign(ref pagerHtml, "{CurrentPage}", cpage.ToString());

                string pageList = string.Empty;
                if (jsonclick != "")
                {
                    jsonclick = jsonclick + "('{PageText}');";
                }

                int startpage, endpage;
                startpage = cpage - 3;
                if (startpage < 1)
                    startpage = 0;

                endpage = startpage + 5;
                if (endpage > pageNum)
                    endpage = pageNum;

                if ((endpage - startpage) < 5 && (cpage < pageNum) && (pageNum - 5 > 0))
                {
                    startpage = pageNum - 5;
                    endpage = pageNum;
                }

                if ((endpage + 1) == pageNum)
                    endpage = pageNum;

                if (cpage >= 4)
                {
                    pageIndex = (string) pageItem.Clone();
                    TemplateHandler.Assign(ref pageIndex, "{jsonclick}", jsonclick);
                    TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
                    TemplateHandler.Assign(ref pageIndex, "{PageText}", "1");
                    if ((cpage - 2) == 2)
                        pageList += pageIndex;
                    else
                        pageList += pageIndex + " ... ";
                }

                if ((endpage == cpage) && (endpage - 5) > 0)
                    startpage = endpage - 5;

                for (int i = startpage; i < endpage; ++i)
                {
                    pageIndex = (string) pageItem.Clone();

                    TemplateHandler.Assign(ref pageIndex, "{jsonclick}", jsonclick);
                    if (cpage == (i + 1))
                    {
                        pageIndex = "<strong>" + cpage + "</strong>";
                    }
                    else
                    {
                        TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
                        TemplateHandler.Assign(ref pageIndex, "{PageText}", "" + (i + 1));
                    }
                    pageList += pageIndex;
                }

                if ((endpage + 1) < pageNum)
                {
                    pageIndex = (string) pageItem.Clone();
                    TemplateHandler.Assign(ref pageIndex, "{jsonclick}", jsonclick);
                    TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
                    TemplateHandler.Assign(ref pageIndex, "{PageText}", "" + pageNum);
                    pageList += " ... " + pageIndex;
                }

                if (pageList.Length > 0)
                {
                    TemplateHandler.Assign(ref pagerHtml, "{PageList}", pageList.Remove(pageList.Length - 1));
                }
                else
                {
                    TemplateHandler.Assign(ref pagerHtml, "{PageList}", "<strong>1</strong>");
                }
                return pagerHtml;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }

        /// <summary>
        /// Caculate page index
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="page">start at 1</param>
        /// <param name="rowNo"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="pageNum"></param>
        public static void CaculatePage(int pageSize, int page, int rowNo, out int startIndex, out int endIndex,
                                        out int pageNum)
        {
            try
            {
                startIndex = 0;
                endIndex = 0;
                pageNum = rowNo/pageSize;
                int rowLastPage = rowNo%pageSize;
                if (rowLastPage > 0)
                    pageNum++;
                if (page < 1 || page > pageNum)
                {
                    page = 1;
                }
                startIndex = (page - 1)*pageSize;
                if (pageNum != 0)
                    endIndex = startIndex + ((rowLastPage > 0 && page == pageNum) ? rowLastPage : pageSize);
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }

        public static void Redirect2SSL()
        {
            try
            {
                if (WebSettings.IsSslLogin)
                {
                    if (!HttpContext.Current.Request.Url.AbsoluteUri.Contains("https"))
                        HttpContext.Current.Response.Redirect(
                            HttpContext.Current.Request.Url.AbsoluteUri.Replace("http", "https"));
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }

        public static void LeaveSSL()
        {
            try
            {
                if (HttpContext.Current.Request.Url.AbsoluteUri.Contains("https"))
                    HttpContext.Current.Response.Redirect(
                        HttpContext.Current.Request.Url.AbsoluteUri.Replace("https", "http"));
            }
            catch (Exception ex)
            {
                //
                //{
                //    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                //}
                //throw ex;
            }
        }

        public static string ReplaceData(string str)
        {
            return str.Replace("\"\"", "&quot;").Replace("'", "&#39;").Replace(Environment.NewLine, "");
        }

        public static string GetRoleName(int roleId)
        {
            switch (roleId)
            {
                case 1:
                    return "Member";
                case 2:
                    return "Agent";
                case 3:
                    return "Master";
                case 4:
                    return "Super";
                default:
                    return "UnKnown";
            }
        }

        public static string GetListAlPhabet(string currSearch)
        {
            var html = new StringBuilder();
            string[] arrAlpha = {
                                    "All", "0-9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                    "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
                                };
            foreach (string str in arrAlpha)
            {
                if (string.Compare(str, currSearch, true) == 0)
                {
                    html.Append("<option selected='true' value='" + str + "'>" + str + "</option>");
                }
                else
                {
                    html.Append("<option value='" + str + "'>" + str + "</option>");
                }
            }
            return html.ToString();
        }

        public static string GetListAlPhabet2(string currSearch)
        {
            var html = new StringBuilder();
            string[] arrAlpha = {
                                    "All", "0-9", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                    "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
                                };
            var index = 0;
            foreach (var str in arrAlpha)
            {
                if (index == 0)
                    html.Append(str);
                else html.Append("," + str);
                ++index;
            }
            return html.ToString();
        }

        public static string GenListStatus(string current)
        {
            string html = string.Empty;
            html += "<option value=\"\">All</option>";
            switch (current)
            {
                case "0":
                    html += "<option selected=\"true\" value=\"0\">Active</option>";
                    html += "<option value=\"1\">Closed</option>";
                    break;
                case "1":
                    html += "<option value=\"0\">Active</option>";
                    html += "<option selected=\"true\" value=\"1\">Closed</option>";
                    break;
                default:
                    html += "<option value=\"0\">Active</option>";
                    html += "<option value=\"1\">Closed</option>";
                    break;
            }
            return html;
        }

        public static string GetListUserType(Globals.UserType userType)
        {
            try
            {
                string html = string.Empty;
                html += "<option value=\"\">All</option>";
                switch (userType)
                {
                    case Globals.UserType.Betting:
                        html += "<option selected=\"true\" value=\"" + (int) Globals.UserType.Betting + "\">" +
                                HttpContext.GetGlobalResourceObject(Constants.Resources.LanguageResources, Constants.Resources.Language.UserBetting)  + "</option>";
                        html += "<option value=\"" + (int) Globals.UserType.Internal + "\">" + HttpContext.GetGlobalResourceObject(Constants.Resources.LanguageResources, Constants.Resources.Language.UserInternal)  +
                                "</option>";
                        break;
                    case Globals.UserType.Internal:
                        html += "<option value=\"" + (int) Globals.UserType.Betting + "\">" + HttpContext.GetGlobalResourceObject(Constants.Resources.LanguageResources, Constants.Resources.Language.UserBetting)  +
                                "</option>";
                        html += "<option selected=\"true\" value=\"" + (int) Globals.UserType.Internal + "\">" +
                                HttpContext.GetGlobalResourceObject(Constants.Resources.LanguageResources, Constants.Resources.Language.UserInternal)  + "</option>";
                        break;
                    default:
                        html += "<option value=\"" + (int) Globals.UserType.Betting + "\">" + HttpContext.GetGlobalResourceObject(Constants.Resources.LanguageResources, Constants.Resources.Language.UserBetting)  +
                                "</option>";
                        html += "<option value=\"" + (int) Globals.UserType.Internal + "\">" + HttpContext.GetGlobalResourceObject(Constants.Resources.LanguageResources, Constants.Resources.Language.UserInternal)  +
                                "</option>";
                        break;
                }
                return html;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw ex;
            }
        }


        public static string GetSportType(int sportID)
        {
            switch (sportID)
            {
                case 1:
                    return "SOCCER";
                case 2:
                    return "BASKETBALL";
                case 3:
                    return "FOOTBALL";
                case 4:
                    return "HOCKEY";
                case 5:
                    return "TENNIS";
                case 6:
                    return "VOLLEYBALL";
                case 7:
                    return "BILLIARD";
                case 8:
                    return "BASEBALL";
                case 9:
                    return "BADMINTON";
                case 10:
                    return "GOLF";
                case 11:
                    return "MOTORSPORTS";
                case 12:
                    return "SWIMMING";
                case 13:
                    return "SYNCHRONIZED SWIMMING";
                case 14:
                    return "WATER POLO";
                case 15:
                    return "DIVING";
                case 16:
                    return "BOXING";
                case 17:
                    return "ARCHERY";
                case 18:
                    return "TABLE TENNIS";
                case 19:
                    return "WEIGHTLIFTING";
                case 20:
                    return "CANOEING";
                case 21:
                    return "GYMNASTICS";
                case 22:
                    return "ATHLETICS";
                case 23:
                    return "EQUESTRIAN";
                case 24:
                    return "HANDBALL";
                case 25:
                    return "DARTS";
                case 99:
                    return "OTHER SPORTS";
                default:
                    return string.Empty;
            }
        }

        public static string GenNoInformation(string colspan)
        {
            string noDataTpl = WebUtils.LoadTemplate("NoInformation.htm");
            TemplateHandler.Assign(ref noDataTpl, "{bgcolor-nodata}", "whitebg");
            TemplateHandler.Assign(ref noDataTpl, "{tdno}", colspan);
            TemplateHandler.Assign(ref noDataTpl, "{nodata}",
                                   HttpContext.GetGlobalResourceObject("Language", "thereisnodata").ToString());
            return noDataTpl;
        }


        public static string GetBetTypeName(int bettype)
        {
            string typename = "";
            switch (bettype)
            {
                case 1:
                    typename = "Handicap";
                    break;
                case 2:
                    typename = "Odd/Even";
                    break;
                case 12:
                    typename = "1st Odds/Even";
                    break;
                case 3:
                    typename = "Over/Under";
                    break;
                case 4:
                    typename = "Correct Score";
                    break;
                case 5:
                    typename = "1X2";
                    break;
                case 6:
                    typename = "Total Goal";
                    break;
                case 7:
                    typename = "1st Handicap";
                    break;
                case 8:
                    typename = "1st Over/Under";
                    break;
                case 14:
                    typename = " First Goal/Last Goal";
                    break;
                case 15:
                    typename = "1st 1X2";
                    break;
                case 16:
                    typename = "HT/FT";
                    break;
                case 20:
                    typename = "Money Line";
                    break;
                default:
                    typename = "N/A";
                    break;
            }
            return typename;
        }

        /* add LiveType and SportType
         By Faisall*/
        public static string GetLiveName(int livetype)
        {
            string liveName = "";
            switch (livetype)
            { 
                case 0:
                    liveName = "Non Live";
                    break;
                case 1:
                    liveName = "Live";
                    break;
                case 2:
                    liveName = "Mix Live and Non Live";
                    break;
                default:
                    break;

            }
            return liveName;
        }

        public  static string GetSportName(int sportType)
        {
            string sportName = "";
            switch (sportType)
            {
                case 1:
                    sportName = "Soccer Sport";
                    break;
                case 2:
                    sportName = "Other Sports";
                    break;
                case 3:
                    sportName = "Multi Sports";
                    break;
                default:
                    break;

            }
            return sportName;
        }

        public static string DecFormat(object dec, byte decDigits)
        {
            decimal value = CastAide.AsDecimal(dec, 0);
            if (value == 0)
                return "";
            if (value > 0)
            {
                return NumberFormat.DecFormat(value, decDigits);
            }
            else
            {
                value = -value;
                return "<font style='color:#f11'>(" + NumberFormat.DecFormat(value, decDigits) + ")</font>";
            }
        }

        public static string PercentFormat(decimal value, byte decDigits)
        {
            if (value == 0)
                return "";
            value = 100*value;
            if (value > 0)
            {
                return NumberFormat.DecFormat(value, decDigits) + "%";
            }
            else
            {
                value = -value;
                return "<font style='color:#f11'>(" + NumberFormat.DecFormat(value, decDigits) + "%)</font>";
            }
        }

        public static string PercentFormat(object dividend, object divisor, byte decDigits)
        {
            decimal dv = CastAide.AsDecimal(dividend, 0);
            decimal ds = CastAide.AsDecimal(divisor, 0);
            if (ds == 0)
                return "";
            decimal value = 100*dv/ds;
            if (value == 0)
                return "";
            if (value > 0)
            {
                return NumberFormat.DecFormat(value, decDigits) + "%";
            }
            else
            {
                value = -value;
                return "<font style='color:#f11'>(" + NumberFormat.DecFormat(value, decDigits) + "%)</font>";
            }
        }


        //Phuong.Nguyen <2008-12-30>
        public static string GetPlaceFormat(int placeNo)
        {
            switch (placeNo)
            {
                case 1:
                    return HttpContext.GetGlobalResourceObject(Sunplus.Accounting.Core.Common.Constants.Resources.LanguageResources, "No1").ToString();
                case 2:
                    return HttpContext.GetGlobalResourceObject(Sunplus.Accounting.Core.Common.Constants.Resources.LanguageResources, "No2").ToString();
                case 3:
                    return  HttpContext.GetGlobalResourceObject(Sunplus.Accounting.Core.Common.Constants.Resources.LanguageResources, "No3").ToString();;
                default:
                    return placeNo + HttpContext.GetGlobalResourceObject(Sunplus.Accounting.Core.Common.Constants.Resources.LanguageResources, "NoOthers").ToString();
            }
        }
        public static string GenPager_Betlist(int pageNum, int cpage, int colsNo)
        {
            string footerHtml = WebUtils.LoadTemplate("BetlistFooterPaging.htm");
            string pageList = PagerManager.GenPagingFooter_Betlist(pageNum, cpage);
            TemplateHandler.Assign(ref footerHtml, "{Colspan}", colsNo.ToString());
            TemplateHandler.Assign(ref footerHtml, "{Pager}", pageList);
            return footerHtml;
        }
        public static void GetDatetimeOption(string dateOption, ref DateTime fromdate, ref DateTime todate)
        {
            //Get FromDate, ToDate
            switch (dateOption)
            {
                case "0": //default
                case "1": //today
                    fromdate = todate = DateTime.Now;
                    break;
                case "2": //Yesterday
                    fromdate = DateTime.Now.AddDays(-1);
                    todate = DateTime.Now.AddDays(-1);
                    break;
                case "3": //current week
                    fromdate = DateTime.Now.AddDays(-1*Convert.ToInt32(DateTime.Now.DayOfWeek));
                    todate = DateTime.Now;
                    break;
                case "4": //last week
                    fromdate = DateTime.Now.AddDays(-1*Convert.ToInt32(DateTime.Now.DayOfWeek) - 7);
                    todate = fromdate.AddDays(6);
                    break;
                case "5": //Current Month
                    fromdate = DateTime.Now.AddDays(-1*(DateTime.Now.Day - 1));
                    todate = DateTime.Now;
                    break;
                case "6": //Last Month
                    fromdate = DateTime.Now.AddDays(-1*(DateTime.Now.Day - 1)).AddMonths(-1);
                    todate = DateTime.Now.AddDays(-1*(DateTime.Now.Day));
                    break;
                case "7": //Since Last Month
                    fromdate = DateTime.Now.AddDays(-1*(DateTime.Now.Day - 1)).AddMonths(-1);
                    todate = DateTime.Now;
                    break;
                    /*
            case "8": //Specific Date
                fromdate = CastAide.AsDateTime(fdate, DateTime.Now);
                todate = CastAide.AsDateTime(tdate, DateTime.Now);
                break;
             */
                default:
                    fromdate = todate = DateTime.Now;
                    break;
            }
        }

        public static DataTable GetDataFromExcel(string ExcelPath)
        {
            var dtb = new DataTable();
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ExcelPath + ";Extended Properties=Excel 8.0;";
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

        public static bool IsValidExtension(string fileName, ref string ext)
        {
            string[] arr = fileName.Split('\\');
            string[] arr1 = arr[arr.Length - 1].Split('.');
            ext = arr1[arr1.Length - 1].ToLower();
            //return ext.Equals("xls") == true || ext.Equals("xlsx") == true || ext.Equals("ods") == true;
            return ext.Equals("xls");
        }
    }

    public static class Status
    {
        public static readonly string RUNNING = "running";
        public static readonly string WAITING = "waiting";
        public static readonly string VOID = "VOID";
        public static readonly string REJECT = "REJECT";
        public static readonly string WIN = "WON";
        public static readonly string LOSE = "LOSE";
        public static readonly string Completed = "completed";
        public static readonly string Official = "Official";
        public static readonly string Refund = "Refund";
        public static readonly string Refund_FT = "Refund FT";
    }

    public static class Level
    {
        public static readonly string SUPER = "Super";
        public static readonly string MASTER = "Master";
        public static readonly string AGENT = "Agent";
        public static readonly string MEMBER = "Member";
    }

    /// <summary>
    /// using for Exception Handling - Enterprise Library
    /// </summary>
    public static class ExceptionPolicyName
    {
        public const string DataAccess = "DataAccess";
        public const string Business = "Business";
        public const string UI = "UI";
    }

    public static class LoggingCategory
    {
        public const string DefaultCategory = "General";
        public const string Accounting = "Accounting";
    }
}