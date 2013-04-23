using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Sunplus.Accounting.Core.Extensions
{
    public class ResourceExtension
    {
        public enum Resources
        {
            Casino, ErrorMsg, Language, MiniTemplate, Viewlog, MoveCust, Racing, ViewlogV2, CategorizeStatus
        }
        public static string GetResourceValue(Resources resource, string key)
        {
            try
            {
                return HttpContext.GetGlobalResourceObject(resource.ToString(), key).ToString();
            }
            catch (Exception)
            {
                
                throw new Exception("Key: " + key + " does not exist in resource: " + resource.ToString());
            }
            
        }
    }
}
