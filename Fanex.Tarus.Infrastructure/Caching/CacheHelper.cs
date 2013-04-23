using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fanex.Tarus.Infrastructure.Caching
{
    using Fanex.Caching;

    public static class CacheHelper
    {
        private static readonly ICache CacheService = null;

        static CacheHelper()
        {
            CacheService = Fanex.Caching.CacheService.CreateInstance(CacheType.MemCache);
        }

        public static void Clear()
        {
            CacheService.Clear();
            
        }

        public static bool Contains(string key)
        {
            
            return CacheService.Contains(key);
        }

        public static int Count()
        {
            
            return CacheService.Count;
        }

        public static T Get<T>(string key)
        {
            return CacheService.Get<T>(key);
        }

        public static void Set<T>(string key, T value)
        {
            CacheService.Set(key, value);
        }

        public static  void Set<T>(string key, T value, DateTime absoluteExpire)
        {
            CacheService.Set(key, value, absoluteExpire);
        }

        public static  void Set<T>(string key, T value, TimeSpan slidingExpire)
        {
            CacheService.Set(key, value, slidingExpire);

            
        }

        public static void Remove(string key)
        {
            CacheService.Remove(key);
        }

        public static  bool TryGet<T>(string key, out T value)
        {
           return CacheService.TryGet(key, out value);
        }

    }
}
