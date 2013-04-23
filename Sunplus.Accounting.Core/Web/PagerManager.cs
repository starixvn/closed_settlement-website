using System;
using DNAide.Web;
using Sunplus.Accounting.Core.Common;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.Core.Web
{
    /// <summary>
    /// Summary description for PagerManager
    /// </summary>
    public class PagerManager
    {
        public PagerManager()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        //----Paging
        public static string GenPagingFooter(string url, int pageNum, int cpage)
        {
            try
            {
                string pageIndexTpl = WebUtils.LoadTemplate("PageIndex.htm");
                string footerHtml = WebUtils.LoadTemplate("Paging.html");
                string GotoPageHtml = WebUtils.LoadTemplate("GotoPage.htm");
                TemplateHandler.Assign(ref GotoPageHtml, "Url", url);
                //TemplateHandler.Assign(ref footerHtml, "{CurrentPage}", cpage.ToString());
                TemplateHandler.Assign(ref footerHtml, "{CurrentPage}", "");
                TemplateHandler.Assign(ref footerHtml, "totalpages", pageNum.ToString());
                string currentUrl = url + "?page=" + cpage;
                TemplateHandler.Assign(ref footerHtml, "currenturl", currentUrl); // current url
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
                    TemplateHandler.Assign(ref pageIndex, "{PageText}", pageNum);
                    TemplateHandler.Assign(ref pageIndex, "{Page}", pageNum);
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
                return string.Empty;
            }
        }
        public static string GenPagingFooter_Betlist(int pageNum, int cpage)
        {
            try
            {
                string pageIndexTpl = WebUtils.LoadTemplate("BetList/PageIndex.htm");
                string footerHtml = WebUtils.LoadTemplate("Paging.html");
                string GotoPageHtml = WebUtils.LoadTemplate("BetList/GotoPage.htm");

                TemplateHandler.Assign(ref footerHtml, "{CurrentPage}", "");
                TemplateHandler.Assign(ref footerHtml, "totalpages", pageNum.ToString());

                TemplateHandler.Assign(ref footerHtml, "currenturl", ""); // current url
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
                    TemplateHandler.Assign(ref pageIndex, "{CurrentPage}", string.Empty);
                    TemplateHandler.Assign(ref pageIndex, "{PageText}", pageNum);
                    TemplateHandler.Assign(ref pageIndex, "{Page}", pageNum);
                    pageList += " ... " + pageIndex;
                }

                if (pageList.Length > 0)
                {
                    TemplateHandler.Assign(ref footerHtml, "{PageList}", pageList.Remove(pageList.Length - 1));
                    TemplateHandler.Assign(ref footerHtml, "{GotoPage}", GotoPageHtml);
                }
                else
                {
                    TemplateHandler.Assign(ref footerHtml, "{PageList}", "<strong>1</strong>");
                }
                return footerHtml;
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
