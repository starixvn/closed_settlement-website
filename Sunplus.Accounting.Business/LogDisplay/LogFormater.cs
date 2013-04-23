using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sunplus.Accounting.Entities.LogDisplay.SettingPT;
using Sunplus.Accounting.Core.Configurations;
using System.Xml.Linq;
using DNAide.Core;
using System.Configuration;
using Sunplus.Accounting.Core.Extensions;

namespace Sunplus.Accounting.Business.LogDisplay
{
    public class LogFormater
    {
        public static TrackingElement FormatElement(XAttribute attr)
        {
            return FormatElement(attr, null);
        }

        public static TrackingElement FormatElement(XAttribute attr, ActionElement action)
        {
            var elm = new TrackingElement();
            if (attr != null)
            {
                var elmVal  = FormatValue(attr);
                var param   = elmVal.P;
                elm.P       = param;
                elm.Val     = elmVal.Val;
                if (action != null && !string.IsNullOrEmpty(action.Prefix))
                    param = MacthParam(param, action);
                elm.N = GetResourceName(param);   
            }
            return elm;
        }

        public static TrackingValue FormatValue(XAttribute attr)
        {
            var elm = new TrackingValue();
            if (attr != null)
            {
                elm.P = attr.Name.ToString();
                var logFormater = LogSection.Instance.Formater.Cast<ParamElement>().Where(format => format.Name == elm.P.ToLower()).SingleOrDefault<ParamElement>();
                elm.Val = attr.Value;
                if (logFormater != null && CastAide.AsInt32(logFormater.Value, 0) > 0)
                {
                    var formatId = CastAide.AsInt32(logFormater.Value);
                    elm.Val = GetValue(formatId, elm.Val);
                }
            }
            return elm;
        }

        public static string GetValue(int formatId, string val)
        {
            var strVal = string.Empty;
            switch (formatId)
            {
                case 1:
                    strVal = FormatTransferOption(val);
                    break;
                case 2:
                    strVal = ReplaceIP(val);
                    break;
                default:
                    break;
            }
            return strVal;
        }

        public static string FormatTransferOption(string str)
        {
            if (str.IndexOf("1") != -1)
            {
                var days = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
                var strChange = ""; var ind = 0;
                foreach (char c in str.ToCharArray())
                {
                    if (c.Equals('1'))
                    {
                        strChange += "[" + days[ind] + "] ";
                    }
                    ind++;
                    if (ind == str.ToCharArray().Length - 1) { break; }
                }
                str = strChange;
            }
            else if (str.Equals("00000000"))
            {
                str = "[Daily]";
            }
            else
            {
                str = "";
            }
            return str;
        }

        public static string GetResourceName(string key)
        {
            var str = string.Empty;
            try
            {
                return ResourceExtension.GetResourceValue(ResourceExtension.Resources.Viewlog, key).ToString();
            }
            catch (Exception ex)
            {
                // missing resource
                str = key;
                // get resoi=urce V2
                try
                {
                    return ResourceExtension.GetResourceValue(ResourceExtension.Resources.ViewlogV2, key).ToString();
                }
                catch { /*ignore*/ }
            }
            return str;
        }

        public static string ReplaceIP(object ip)
        {
            var ipVNList = IpManagement.IPVNList;
            string ipReplaced = IpManagement.ReplaceVNIP(CastAide.AsString(ip), ipVNList);
            if (string.IsNullOrEmpty(ipReplaced))
                return "";
            else if (ipReplaced.Trim() == WebSettings.IPVNReplace.Trim())
                return WebSettings.IPVNReplace;
            else
                return string.Format("<a href='{0}' target=_blank>{1}</a>", string.Format(ConfigurationManager.AppSettings["iplocation"].Replace("{ip}", CastAide.AsString(ip))), CastAide.AsString(ip));
        }

        public static string MacthParam(string param, ActionElement action)
        {
            Regex regx = new Regex(action.Regx, RegexOptions.IgnoreCase);
            bool matched = regx.IsMatch(param);
            if (matched)
                param = param + action.Prefix;
            return param;
        }
    }
}
