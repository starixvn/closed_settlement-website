using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sunplus.Accounting.Core.Cache
{
    public class AppCache
    {

        public static Dictionary<string, string> AccessPermission
        {
            get
            {
                Dictionary<string, string> retValue = new Dictionary<string, string>();
                retValue = HttpContext.Current.Cache["AccessPermission"] == null ? retValue : (Dictionary<string, string>)HttpContext.Current.Cache["AccessPermission"];
                return retValue;
            }
            set
            {
                HttpContext.Current.Cache.Remove("AccessPermission");
                HttpContext.Current.Cache.Add("AccessPermission", value, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.Zero,
                                              System.Web.Caching.CacheItemPriority.Normal, null);
            }
        }
    }
}
