using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DNAide.Core;
using DNAide.Utils;
using DNAide.Web;

using Sunplus.Accounting.Business.CacheManager;
using Sunplus.Accounting.Core.Cache;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.IBCDLL.Accounting;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Business.UI
{
    /// <summary>
    /// Summary description for GenerateHtml
    /// </summary>
    public static class GenerateHtml
    {
        public static string GetContentInvalidExRate(DateTime fromdate, DateTime todate)
        {
            string ListMain = WebUtils.LoadTemplate("ExRate/InvalidExRate.html");
            string ListContent = TemplateHandler.CopyBlock(ListMain, "limit-content");
            string CopyContent = "";
            string bgcolor_row = "";
            string themePath = WebSettings.ThemePath;
            int counter = 0;
            var Content = new StringBuilder();


            try
            {
                Dictionary<int, string> exchange = IBCProvider.Utils.GetExchange();

                using (IDataReader dr = Balance.InvalidExChangeRateSel(fromdate, todate))
                {
                    while (dr.Read())
                    {
                        counter++;
                        bgcolor_row = (bgcolor_row == "") ? "evenrow" : "";
                        CopyContent = ListContent;

                        TemplateHandler.Assign(ref CopyContent, "bgcolor-row", bgcolor_row);
                        TemplateHandler.Assign(ref CopyContent, "No_Value", counter.ToString());
                        TemplateHandler.Assign(ref CopyContent, "TransactionDate_Value", dr["transdate"]);
                        TemplateHandler.Assign(ref CopyContent, "FromCustomer_Value", dr["fromcust"]);
                        TemplateHandler.Assign(ref CopyContent, "ToCustomer_Value", dr["tocust"]);
                        TemplateHandler.Assign(ref CopyContent, "FromCurrency_Value",
                                               CacheUtils.GetExchangeName(exchange, CastAide.AsInt32(dr["fromexchangeid"])));
                        TemplateHandler.Assign(ref CopyContent, "ToCurrency_Value",
                                               CacheUtils.GetExchangeName(exchange, CastAide.AsInt32(dr["toexchangeid"])));
                        TemplateHandler.Assign(ref CopyContent, "Min_Value", NumberFormat.DecFormat(dr["minexrate"], 4));
                        TemplateHandler.Assign(ref CopyContent, "Max_Value", NumberFormat.DecFormat(dr["maxexrate"], 4));
                        TemplateHandler.Assign(ref CopyContent, "InputRate_Value", dr["inputExRate"]);
                        TemplateHandler.Assign(ref CopyContent, "Amount_Value", NumberFormat.DecFormat(dr["Amount"], 2));
                        TemplateHandler.Assign(ref CopyContent, "Creator_Value", dr["creator"]);

                        Content.Append(CopyContent);
                    }
                }

                if (counter == 0)
                {
                    return AccCommon.GenNoInformation("11");
                }
                else
                {
                    return Content.ToString();
                }
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                throw;
            }
            return string.Empty;
        }

        public static string GenLinkPopup(string rootpath, string pageurl, string title, int width, int height, int left,
                                          int top, string linkText)
        {
            try
            {
                string htmlLinkPopup = WebUtils.LoadTemplate("LinkPopup.htm");
                TemplateHandler.Assign(ref htmlLinkPopup, "rootpath", rootpath);
                TemplateHandler.Assign(ref htmlLinkPopup, "pageurl", pageurl);
                TemplateHandler.Assign(ref htmlLinkPopup, "wdtitle", title);
                TemplateHandler.Assign(ref htmlLinkPopup, "wdwidth", width);
                TemplateHandler.Assign(ref htmlLinkPopup, "wdheight", height);
                TemplateHandler.Assign(ref htmlLinkPopup, "wdleft", left);
                TemplateHandler.Assign(ref htmlLinkPopup, "wdtop", top);
                TemplateHandler.Assign(ref htmlLinkPopup, "lblLink", linkText);
                return htmlLinkPopup;
            }
            catch (Exception ex)
            {
                
                {
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                }
                return string.Empty;
            }
        }
    }
}
