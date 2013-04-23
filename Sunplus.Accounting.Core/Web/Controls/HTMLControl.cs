using System.Collections;

namespace Sunplus.Accounting.Core.Web.Controls
{
    /// <summary>
    /// Summary description for HTMLControl
    /// </summary>
    public class HTMLControl
    {
        public static string Select(Hashtable items, string id, string classname, string jsfunction)
        {
            string strselect = "";
            strselect = "<select id='" + id + "' name='" + id + "' class='" + classname + "' onchange='" + jsfunction + "'>";
            foreach (DictionaryEntry item in items)
            {
                strselect += "<option value='" + item.Value + "'>" + item.Key + "</option>";
            }
            strselect += "</select>";
            return strselect;
        }
    }
}