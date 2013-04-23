using DNAide.Web;

namespace Sunplus.Accounting.Core.Web.Controls
{
    /// <summary>
    /// Summary description for Pager
    /// </summary>
    public class Pager
    {
        public static string InitPager(int pageNum, int cpage, string jsonclick)
        {
            try
            {
                string pagerHtml = WebUtils.LoadTemplate("AccPager.htm");
                string pageItem = TemplateHandler.CopyBlock(pagerHtml, "PageItem");
                TemplateHandler.RemoveBlock(ref pagerHtml, "PageItem");
                string pageIndex = "";

                string pageList = string.Empty;
                if (jsonclick != "")
                {
                    jsonclick = jsonclick + "({PageText});";
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
                        TemplateHandler.Assign(ref pageIndex, "{PageText}", "" + (i + 1));
                    }
                    pageList += pageIndex;
                }

                if ((endpage + 1) < pageNum)
                {
                    pageIndex = (string) pageItem.Clone();
                    TemplateHandler.Assign(ref pageIndex, "{jsonclick}", jsonclick);
                    TemplateHandler.Assign(ref pageIndex, "{PageText}", "" + pageNum);
                    pageList += " ... " + pageIndex;
                }

                if (pageList.Length > 0)
                {
                    TemplateHandler.Assign(ref pagerHtml, "{PageList}", pageList);
                }
                else
                {
                    TemplateHandler.Assign(ref pagerHtml, "{PageList}", "<strong>1</strong>");
                }
                return pagerHtml;
            }
            catch
            {
                throw;
            }
        }
    }
}