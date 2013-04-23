using System;

namespace Sunplus.Accounting.Core.Cache
{
    public interface ICacheFactory
    {

        object Get(string key);
        void Set(string key, object value);

        void Set(string key, object value, TimeSpan expire);

        T Get<T>(string key);
        void Set<T>(string key, T value);
        void Set<T>(string key, T value, TimeSpan expire);

        bool Delete(string key);
    }
}
