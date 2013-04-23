using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using SharedCache.WinServiceCommon.Provider.Cache;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Helper.IP;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess;

namespace Sunplus.Accounting.Business
{
    /// <summary>
    /// Utilities class for IP
    /// </summary>
    public class IpManagement
    {
        public static List<IPVNBean> IPVNList
        {
            get
            {
                List<IPVNBean> listIpvn;
                try
                {
                    listIpvn = IndexusDistributionCache.SharedCache.Get("ipvnlist") as List<IPVNBean> ?? IBCProvider.Utils.GetIPVN();
                }
                catch (Exception ex)
                {
                    listIpvn = IBCProvider.Utils.GetIPVN();
                    IPVNList = listIpvn;
                }
                return listIpvn;
            }
            set { IndexusDistributionCache.SharedCache.Add("ipvnlist", value, DateTime.Now.AddMinutes(WebSettings.CacheExpire)); }
        }

        public static bool IsVNIP(string IP)
        {
            try
            {
                double ipNumber = 0;
                ipNumber = IP2Number(IP);
                List<IPVNBean> listIPVN = IPVNList;
                int total = listIPVN.Count;
                for (int i = 0; i < total; i++)
                {
                    IPVNBean item = listIPVN[i];
                    if (item == null)
                        return false;
                    if (item.IPFrom <= ipNumber && ipNumber <= item.IPTo)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return false;
            }
        }

        public static bool IsVNIP(string IP, List<IPVNBean> _listIPVN)
        {
            try
            {
                double IPNumber = 0;
                IPNumber = IP2Number(IP);
                foreach (IPVNBean item in _listIPVN)
                {
                    if (item.IPFrom <= IPNumber && IPNumber <= item.IPTo)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return false;
            }
        }

        public static bool IsVNIP_Linq(string IP)
        {
            try
            {
                double ipNumber = 0;
                ipNumber = IP2Number(IP);
                List<IPVNBean> listIPVN = IPVNList;
                IEnumerable<IPVNBean> query = from IPVNBean bean in listIPVN
                                              where bean.IPFrom <= ipNumber && ipNumber <= bean.IPTo
                                              select bean;
                if (query.Count() > 0)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return false;
            }
        }

        //Check input string is a valid IP4 format
        public static bool IsValidIP(string addr)
        {
            string pattern =
                @"^([1-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])(\.([0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])){3}$";
            var check = new Regex(pattern);
            bool valid = false;
            if (addr == "")
            {
                valid = false;
            }
            else
            {
                valid = check.IsMatch(addr, 0);
            }
            return valid;
        }

        //Replace VN IP with other one or empty string
        public static string ReplaceVNIP(string IP)
        {
            if(IsVNIP(IP))
            {
                if (AppSession.ViewIPVN || WebSettings.IPVNReplace == "1")
                    return IP;
                else
                    return WebSettings.IPVNReplace;
            }
            else
            {
                return IP;
            }

        }
        public static string ReplaceCountryName(string IP)
        {
            var ipManager = new IpManager();
            string countryName = ipManager.GetCountryNameByIp(IP);
                if (IsVNIP(IP))
                {
                    if (AppSession.ViewIPVN || WebSettings.IPVNReplace == "1")
                        return countryName;
                    else
                        return "-";
                }
                else
                {
                    return countryName;
                }
               

        }

        public static string ReplaceVNIP(string IP, List<IPVNBean> _IPVNList)
        {
            if (AppSession.ViewIPVN)
            {
                return IP;
            }
            else
            {
                if (IsVNIP(IP, _IPVNList))
                    return WebSettings.IPVNReplace == "1" ? IP : WebSettings.IPVNReplace;
                else
                    return IP;
            }
        }

        //Convert IP to nummber
        public static double IP2Number(string IP)
        {
            try
            {
                double longIP = 0;
                if (IsValidIP(IP))
                {
                    string[] sIP = IP.Split('.');
                    longIP = double.Parse(sIP[0])*16777216 + double.Parse(sIP[1])*65536 + double.Parse(sIP[2])*256 +
                             double.Parse(sIP[3]);
                }
                return longIP;
            }
            catch (Exception ex)
            {
                Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);
                return 0.0;
            }
        }

        //Save IPVN table to Cache
        //Replace with your suitable cache
        //In this case we are using SharedCache: http://www.sharedcache.com/cms/

        public static string FormatIP(string ip)
        {
            return new AccountingIPFormat(ip).Format();
        }

        public static string FormatIPs(string content)
        {
            return new AccountingIPFormat("").FormatAll(content);
        }
    }

    
}