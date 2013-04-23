using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DNAide.Core;
using ExcelUtil.Spreadsheet;
using Sunplus.Accounting.Entities.Report;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Web;


//using IBC.Core.Web;
    /// <summary>
    /// Summary description for WinLossReporPageBase
    /// </summary> 
    public class WinLossReporPageBase : RequiredAuthorizationPage
    {
        public const string TemplateLinkColumn =
            "<td class='link' ><a href='javascript:ViewDownLineReport({0})'>{1}</a></td>";

        public const string TemplateColumn = "<td>{0}</td>";

        /// <summary>
        /// <![CDATA[<td {1}>{0}</td>]]>        
        /// </summary>
        public const string TemplateStyleColumn = "<td {1}>{0}</td>";

        public const string CurrencyFormat = "#,##0.00";

        protected override int CustId
        {
            get { return CastAide.AsInt32(Request.QueryString["uplineid"]); }
        }

        /// <summary>
        /// Just allow export report if and only if button export is enabled
        /// </summary>
        public bool IsExportEnabled
        {
            get { return (ReportButton != null && ReportButton.Enabled); }
            set
            {
                if (ReportButton != null)
                {
                    ReportButton.Enabled = value;
                }
            }
        }

        public virtual ImageButton ReportButton
        {
            get
            {
                var masterPage = (Master as ReportMasterPageBase);
                return masterPage != null ? masterPage.ExportButton : null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebUtils.SetNoCache();

            if (!Page.IsPostBack)
            {
                if (Request.QueryString["option"] != null)
                {
                    if (IsPageValid())
                    {
                        PreRenderReportEvent(sender, e);
                        RenderReport(ReportControl, GetReportData(), NoInfoPanel);
                        PostRenderReportEvent(sender, e);
                    }
                    else
                    {
                        NoInfoPanel.Visible = true;
                        IsExportEnabled = false;
                    }
                }
                else
                {
                    IsExportEnabled = false;
                }
            }
        }


        /// <summary>
        /// Export file
        /// </summary>
        public void ExportExcelFile()
        {
            if (IsExportEnabled)
            {
                ExcelFile excelFile = CreateExcelFile();
                if (excelFile != null && !string.IsNullOrEmpty(ExportFileName))
                {
                    Response.ContentType = "Application/x-msexcel";
                    Response.AddHeader("Content-Disposition",
                                       string.Format("attachment; filename={0}.xls", ExportFileName));
                    // excelFile.Protected = false;

                    excelFile.SaveXls(Response.OutputStream);
                }
            }
        }

        public virtual ExcelFile CreateExcelFile()
        {
            throw new NotImplementedException();
        }


        public virtual void PreRenderReportEvent(object sender, EventArgs e)
        {
            //todo
        }

        public virtual void PostRenderReportEvent(object sender, EventArgs e)
        {
            //todo
        }

        /// <summary>
        /// Render html string winloss comm and total
        /// <![CDATA[<td>value</td>]]>
        /// </summary>
        /// <param name="winLossComm"></param>
        /// <returns></returns>
        protected string RenderWinLossCommTotal(WinLossComm winLossComm)
        {
            var html = new StringBuilder();
            html.AppendFormat(TemplateColumn, winLossComm.WinLoss.ToString(CurrencyFormat));
            html.AppendFormat(TemplateColumn, winLossComm.Comm.ToString(CurrencyFormat));
            html.AppendFormat(TemplateColumn, winLossComm.Total.ToString(CurrencyFormat));

            return html.ToString();
        }

        protected virtual string RenderWinLossCommTotalByReport(IWinLoss winloss)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// render username hyperlink, turnover, grosscomm
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected virtual string RenderBaseInfo(IWinLoss item)
        {
            var html = new StringBuilder();
            //username link uplineid
            html.AppendFormat(RenderColumnUserHyperlink(item));
            //turnover
            html.AppendFormat(TemplateColumn, item.Turnover.ToString(CurrencyFormat));
            //gross comm
            html.AppendFormat(TemplateColumn, item.GrossComm.ToString(CurrencyFormat));

            return html.ToString();
        }

        protected virtual string RenderColumnUserHyperlink(IWinLoss item)
        {
            var html = new StringBuilder();
            html.AppendFormat(TemplateLinkColumn, item.CustId, item.Username);
            return html.ToString();
        }

        /// <summary>
        /// render a row of report winloss detail
        /// </summary>
        /// <param name="dataItem"></param>
        /// <returns>string</returns>
        public virtual string RenderRowItem(object dataItem)
        {
            IWinLoss winloss = (dataItem is MasterWinLoss)
                                   ? (dataItem as MasterWinLoss)
                                   : ((dataItem is AgentWinLoss)
                                          ? (dataItem as AgentWinLoss)
                                          : (dataItem as MemberWinLoss));

            var html = new StringBuilder();

            //render baseinfo
            html.Append(RenderBaseInfo(winloss));
            //virtual render win/loss comm total
            html.Append(RenderWinLossCommTotalByReport(winloss));
            //render company total
            if (winloss != null) html.AppendFormat(TemplateColumn, winloss.UplineTotal.ToString(CurrencyFormat));
            //if (winloss.UplineTotal > 0)
            //{

            //}
            //else 
            //{
            //    html.AppendFormat(TemplateStyleColumn, winloss.UplineTotal.ToString(CurrencyFormat), "style='color:#FF0000;'");
            //}
            return html.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        protected string RenderFooterBaseInfo(IWinLoss entity)
        {
            var html = new StringBuilder();
            //base info
            html.AppendFormat(TemplateStyleColumn, "Total", "style='padding-right:5px;'");
            if (entity != null)
            {
                html.AppendFormat(TemplateColumn, entity.Turnover.ToString(CurrencyFormat));
                html.AppendFormat(TemplateColumn, entity.GrossComm.ToString(CurrencyFormat));
            }

            return html.ToString();
        }

        protected virtual string RenderFooterByReport(IWinLoss entity)
        {
            throw new NotImplementedException();
        }

        protected string RenderFooter(object dataItem)
        {
            var html = new StringBuilder();
            if (dataItem != null)
            {
                IWinLoss winloss = (dataItem is MasterWinLoss)
                                       ? (dataItem as MasterWinLoss)
                                       : ((dataItem is AgentWinLoss)
                                              ? (dataItem as AgentWinLoss)
                                              : (dataItem as MemberWinLoss));
                html.Append(RenderFooterBaseInfo(winloss));
                html.Append(RenderWinLossCommTotalByReport(winloss));
                if (winloss != null) html.AppendFormat(TemplateColumn, winloss.UplineTotal.ToString(CurrencyFormat));
            }
            return html.ToString();
        }


        public virtual IList GetReportData()
        {
            throw new NotImplementedException();
        }

        public void RenderReport(Repeater rptControl, IList entityList, HtmlGenericControl noInfoPanel)
        {
            if (entityList.Count > 0)
            {
                rptControl.DataSource = entityList;
                rptControl.DataBind();

                noInfoPanel.Visible = false;

                //caculate last item for rendering footer
                //lastItem = winlossService.CreateTotalLastItem(entityList);
                IsExportEnabled = true;
            }
            else
            {
                noInfoPanel.Visible = true;
                IsExportEnabled = false;
            }
        }

        /// <summary>
        /// Check valid param request
        /// call before any request get report data
        /// </summary>
        /// <returns></returns>
        public virtual bool IsPageValid()
        {
            bool isPageValid = false;
            isPageValid = (ToDate.CompareTo(FromDate) >= 0);
            return isPageValid;
        }

        #region Properties

        protected IWinLoss lastItem;

        public virtual string CurrentUrl
        {
            get { throw new NotImplementedException(); }
        }

        public virtual string DownlineUrl
        {
            get { throw new NotImplementedException(); }
        }

        public virtual Repeater ReportControl
        {
            get { throw new NotImplementedException(); }
        }

        public virtual string ExportFileName
        {
            get { throw new NotImplementedException(); }
        }

        public virtual HtmlGenericControl NoInfoPanel
        {
            get { throw new NotImplementedException(); }
        }

        public virtual string ReportTitle { get; set; }

        public int UplineId
        {
            get
            {
                if (Request.QueryString["uplineid"] != null)
                {
                    return Convert.ToInt32(Request.QueryString["uplineid"]);
                }

                return -1;
            }
        }

        public DateTime FromDate
        {
            get
            {
                DateTime now = DateTime.Now;
                if (Request.QueryString["fdate"] != null)
                {
                    DateTime.TryParse(Request.QueryString["fdate"], out now);
                }

                return now;
            }
        }

        public DateTime ToDate
        {
            get
            {
                DateTime now = DateTime.Now;
                if (Request.QueryString["tdate"] != null)
                {
                    DateTime.TryParse(Request.QueryString["tdate"], out now);
                }

                return now;
            }
        }

        public string Option
        {
            get
            {
                string option = "11111";
                if (Request.QueryString["option"] != null)
                {
                    var regx = new Regex("[0-1]{5}", RegexOptions.Compiled);
                    if (regx.IsMatch(Request.QueryString["option"]))
                    {
                        option = Request.QueryString["option"];
                    }
                }

                return option;
            }
        }

        #endregion
    }
