/// <summary>
/// Summary description for GenerateMenu
/// </summary>

using DNAide.Web;
using Sunplus.Accounting.Core.Web;

namespace Sunplus.Accounting.Business.Settle
{
    public class GenerateMenu
    {
        #region Menu

        public string Menu(string mode, ref bool isCalulate)
        {
            string s = WebUtils.LoadTemplate("Settle/SelectType.htm");

            isCalulate = true;
            if (string.Compare(mode, "overonly", true) == 0) // over only
            {
                isCalulate = false;
                TemplateHandler.Assign(ref s, "{mode}", "OVER");
                TemplateHandler.Assign(ref s, "{calculate}", "<a href='Index.aspx' target='_self'>CALCULATE</a>");
                TemplateHandler.Assign(ref s, "{htoveronly}",
                                       "<a href='Index.aspx?m=htoveronly' target='_self'>HT OVER ONLY</a>");
                TemplateHandler.Assign(ref s, "{overonly}", "OVER ONLY");
            }
            else if (string.Compare(mode, "htoveronly", true) == 0) // HT over only
            {
                isCalulate = false;
                TemplateHandler.Assign(ref s, "{mode}", "HTOVER");
                TemplateHandler.Assign(ref s, "{calculate}", "<a href='Index.aspx' target='_self'>CALCULATE</a>");
                TemplateHandler.Assign(ref s, "{htoveronly}", "HT OVER ONLY");
                TemplateHandler.Assign(ref s, "{overonly}",
                                       "<a href='Index.aspx?m=overonly' target='_self'>OVER ONLY</a>");
            }
            else // calculate
            {
                TemplateHandler.Assign(ref s, "{mode}", "CALC");
                TemplateHandler.Assign(ref s, "{calculate}", "CALCULATE");
                TemplateHandler.Assign(ref s, "{htoveronly}",
                                       "<a href='Index.aspx?m=htoveronly' target='_self'>HT OVER ONLY</a>");
                TemplateHandler.Assign(ref s, "{overonly}",
                                       "<a href='Index.aspx?m=overonly' target='_self'>OVER ONLY</a>");
            }

            return s;
        }

        #endregion
    }
}