using System;
using System.Collections.Generic;
using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace Sunplus.Data.DataAbstract.Storage.Cache.Extentions
{
    public class Memcached : ICache
    {
        private static MemcachedClient Cache;

        public Memcached()
        {
            try
            {
                if (Cache == null)
                {
                    Cache = new MemcachedClient("memcached");
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Init()
        {
            if (Cache == null)
            {
                Cache = new MemcachedClient("memcached");
            }
        }

        

        public bool Set(string key, object value)
        {
            try
            {
                Cache.Store(StoreMode.Set, key, value);
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public bool Set(string key, object value, TimeSpan expire)
        {
            try
            {
                Cache.Store(StoreMode.Set, key, value, expire);
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }
        public object Get(string key)
        {
            try
            {
                return Cache.Get(key);
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public bool InsertBatch<T>(string key, IEnumerable<T> data)
        {
            throw new NotImplementedException();
        }

        public T Select<T>(object value)
        {
            throw new NotImplementedException();
        }

        IEnumerable<T> IDataStorage.SelectAll<T>()
        {
            throw new NotImplementedException();
        }

        public bool Insert<T>(string key, T data)
        {
            return this.Set(key, data);
        }

        public bool Insert<T>(string key, T data, TimeSpan expire)
        {
            return this.Set(key, data, expire);
        }

        public bool Insert<T>(string key, IEnumerable<T> data)
        {
            return this.Set(key, data);
        }

        public bool Delete<T>(string key, object data)
        {
            try
            {
                Cache.Remove(key);
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public bool DeleteAll<T>()
        {
            try
            {
                Cache.FlushAll();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DropCollection<T>()
        {
            throw new NotImplementedException();
        }

        public bool Update<T>(string key, T data)
        {
            throw new NotImplementedException();
        }

        public object GetCollection<T>()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            try
            {
                return Cache.Get<T>(key);
            }
            catch (Exception ex)
            {

            }
            return default(T);
        }

        public void FlushAll()
        {
            try
            {
                Cache.FlushAll();
            }
            catch (Exception ex)
            {

            }
        }

        public List<T> GetSubkey<T>(string[] keys, string subKey)
        {
            //Cache.PerformMultiGet<T>(keys, delegate(IMultiGetOperation im, KeyValuePair<sting, T>){
            //        return im.Result();
            //});
            return new List<T>();
        }

    }
}
