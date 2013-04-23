using System;
using System.Collections;
using System.Data;
using System.ServiceModel.Activation;
using System.Text;
using DNAide.Core;
using DNAide.Data;
using DNAide.Web;

using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Web.Controls;
using Sunplus.Accounting.DataAccess.IBCDLL.Customer;
using IBCUtils;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Business
{
    /// <summary>
    /// Summary description for ServiceCustomerList
    /// </summary>
    /// 
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ServiceSuspend : IServiceSuspend
    {

        public ServiceSuspend()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #region IServiceSuspend Members
        public string GenSuspendListContent(string searchText, string selectAlpha, int PageIndex)
        {
            int TotalPages = 0;
            StringBuilder html = new StringBuilder();
            string strHeader = "", strContent = "";
            try
            {
                strHeader = WebUtils.LoadTemplate("Customer/SuspendCustListHeader.htm");
                strContent = WebUtils.LoadTemplate("Customer/SuspendCustListRow.htm");

                searchText = (!string.IsNullOrEmpty(searchText) && searchText.Equals("All")) ? "" : searchText;

                selectAlpha = (!string.IsNullOrEmpty(selectAlpha) && selectAlpha.Equals("All")) ? "" : selectAlpha;

                var hash = new Hashtable();

                if (!string.IsNullOrEmpty(searchText))
                {
                    string keyword = "%" + searchText.Replace("'", "") + "%";
                    hash.Add("keyword", keyword);
                }
                else
                    searchText = "All";
                if (!string.IsNullOrEmpty(selectAlpha))
                {
                    string firstchar = selectAlpha;
                    if (string.Compare(firstchar, "0-9", true) == 0)
                    {
                        firstchar = "[" + firstchar + "]";
                    }
                    firstchar = firstchar + "%";
                    hash.Add("firstchar", firstchar);
                }

                DbResult ds = null;
          
            


                hash.Add("pageindex", PageIndex);
                hash.Add("totalpages", TotalPages);
                hash.Add("pagesize", AccCommon.pageSize);
                ds = Select4Edit.GetSuspendCustomerList_1(hash);
                TotalPages = CastAide.AsInt32(ds.Outputs["totalpages"]);
                if (PageIndex < 1 || PageIndex > TotalPages)
                {
                    PageIndex = 1;
                }
                if (ds != null)
                {
                    html.Append(strHeader);

                    if (ds.ResultSet.Tables[0].Rows.Count > 0)
                    {
                        DataRowCollection rows = ds.ResultSet.Tables[0].Rows;

                        string odd_even = "";
                        int rowCnt = (PageIndex - 1)*AccCommon.pageSize + 1;
                        if (PageIndex == 1) rowCnt = 1;

                        foreach (DataRow row in rows)
                        {
                            string str = strContent;
                            odd_even = (odd_even == "") ? "evenrow" : "";
                            TemplateHandler.Assign(ref str, "{odd_even}", odd_even);
                            TemplateHandler.Assign(ref str, "{no}", rowCnt.ToString());
                            TemplateHandler.Assign(ref str, "{username}", row["username"].ToString());
                            TemplateHandler.Assign(ref str, "{custid}", row["custid"].ToString());
                            if (FormatHandler.GetBool(row["autosuspend"]))
                            {
                                TemplateHandler.Assign(ref str, "{str-checked}", "checked");
                            }
                            else
                            {
                                TemplateHandler.Assign(ref str, "{str-checked}", "");
                            }
                            rowCnt++;
                            html.Append(str);
                        }

                        html.Append("<tr><td class='3' colspan='3'>");
                        html.Append(Pager.InitPager(TotalPages, PageIndex, ""));
                        html.Append("</td></tr>");
                        html.Append("</table>");
                    }
                    else
                    {
                        html.Append(WebUtils.LoadHtml("NoInformation.htm", new[] {"{tdno}", "{nodata}"},
                                                      new[] {"3", ResourceExtension.GetResourceValue(ResourceExtension.Resources.Language, "thereisnodata")}));
                        html.Append("</table>");
                    }
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
            }
            return html.ToString();
        }
        #endregion
    }
}
