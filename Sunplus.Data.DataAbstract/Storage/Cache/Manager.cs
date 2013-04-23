using System;
namespace Sunplus.Data.DataAbstract.Storage.Cache
{
    public class Manager : Abstract
    {
        public Manager()
        {
            base.InitCaching();
        }

        public override object Get(string key)
        {
            return Caching.Get(key);
        }

        public override T Get<T>(string key)
        {
            return Caching.Get<T>(key);
        }

        public override bool Insert<T>(string key, T value)
        {
            return Caching.Insert(key, value);
        }

        

        public override bool Delete<T>(string key, object data)
        {
            return Caching.Delete<T>(key, data);
        }

        public override void FlushAll()
        {
            Caching.FlushAll();
        }

        public override bool Insert<T>(string key, T value, TimeSpan expire)
        {
            return Caching.Insert(key, value, expire);
        }

        ~Manager() { }
    }
}
