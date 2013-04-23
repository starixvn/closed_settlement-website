using System;
using SharedCache.WinServiceCommon.Provider.Cache;

namespace Sunplus.Accounting.Core.Cache.Extension
{
    public static class CacheExtension
    {
        public static bool Cached = DNAide.Core.CastAide.AsBoolean(System.Configuration.ConfigurationManager.AppSettings["cached"], false);

        public static void Set(string key, object value)
        {
            if (!Cached)
            {
                return;
            }
            try
            {
                IndexusDistributionCache.SharedCache.Add(key, value);
            }
            catch (Exception ex)
            {
                //TODO
            }
        }
        public static void Set(string key, object value, DateTime expire)
        {
            if (!Cached)
            {
                return;
            }
            try
            {
                IndexusDistributionCache.SharedCache.Add(key, value, expire);
            }
            catch (Exception ex)
            {
                //TODO
            }
        }
       
        public static T Get<T>(string key)
        {
            if (!Cached)
            {
                return default(T);
            }
            try
            {
                return IndexusDistributionCache.SharedCache.Get<T>(key);
            }
            catch (Exception ex)
            {
                //TODO
            }
            return default(T);
        }

        public static object Get(string key)
        {
            if (!Cached)
            {
                return null;
            }
            try
            {
                return IndexusDistributionCache.SharedCache.Get(key);
            }
            catch (Exception ex)
            {
                //TODO
            }
            return default(object);
        }

        public static void Delete(string key)
        {
            if (!Cached)
            {
                return;
            }
            try
            {
                IndexusDistributionCache.SharedCache.Remove(key);
            }
            catch (Exception ex)
            {
                //TODO
            }
        }

        public static void RegexDelete(string regularExpression)
        {
            if (!Cached)
            {
                return;
            }
            try
            {
                IndexusDistributionCache.SharedCache.RegexRemove(regularExpression);
            }
            catch (Exception ex)
            {
                //TODO
            }
        }
    }
}
