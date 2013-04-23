using System.Collections.Generic;
using System;

namespace Sunplus.Data.DataAbstract.Storage
{
    public interface IDataStorage
    {
        object Get(string key);

        T Get<T>(string key);

        bool InsertBatch<T>(string key, IEnumerable<T> data);

        T Select<T>(object value);

        IEnumerable<T> SelectAll<T>();

        bool Insert<T>(string key, T data);

        bool Insert<T>(string key, T data, TimeSpan expire);

        bool Delete<T>(string key, object data);

        bool DeleteAll<T>();

        bool DropCollection<T>();

        bool Update<T>(string key, T data);

        object GetCollection<T>();


    }
}
