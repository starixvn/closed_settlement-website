using System;
using System.Collections.Generic;
using System.Linq;
using DNAide.Core;
using SharedCache.WinServiceCommon.Provider.Cache;
using Sunplus.Accounting.DataAccess;

namespace Sunplus.Accounting.Business.CacheManager
{
    /// <summary>
    /// Summary description for AppCache
    /// </summary>
    public class CacheUtils
    {
    
       
        public static Dictionary<int, string> Exchange
        {
            get
            {
                Dictionary<int, string> exchange;
                try
                {
                    exchange = IndexusDistributionCache.SharedCache.Get("exchange") as Dictionary<int, string> ??
                               IBCProvider.Utils.GetExchange();
                }
                catch (Exception ex)
                {
                    exchange = IBCProvider.Utils.GetExchange();
                    Exchange = exchange;
                }


                return exchange;
            }
            set { IndexusDistributionCache.SharedCache.Add("exchange", value, DateTime.Now.AddHours(5)); }
        }


        public static string GetAllSiteNames
        {
            get
            {
                try
                {
                    return IndexusDistributionCache.SharedCache.Get("AccGetAllSiteNames") as string;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            set { IndexusDistributionCache.SharedCache.Add("AccGetAllSiteNames", value, DateTime.Now.AddHours(5)); }
        }

        public static string GetExchangeName(int id)
        {
            Dictionary<int, string> exchange = Exchange;
            string value = (from k in exchange
                            where k.Key == id
                            select k.Value).FirstOrDefault();
            return CastAide.AsString(value);
        }
        public static string GetExchangeName(Dictionary<int, string> exchange, int id)
        {
            string value = (from k in exchange
                            where k.Key == id
                            select k.Value).FirstOrDefault();
            return CastAide.AsString(value);
        }
    }
}
