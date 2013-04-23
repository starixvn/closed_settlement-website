using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Sunplus.Data.DataAbstract.Storage.Cache
{
    [DataContract]
    public class CacheDataStorage : IDataStorage
    {

        private Storage.Cache.Manager _cache = new Manager();
       

        #region public method

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
            bool inserted = false;
            inserted = _cache.Insert(key, data);
            return inserted;
        }

        public bool Insert<T>(string key, IEnumerable<T> data)
        {
            bool inserted = false;

            inserted = _cache.Insert(key, data);


            return inserted;
        }
        
        #endregion public method

        public object Get(string key)
        {
            return _cache.Get(key);
        }

        public T Get<T>(string key)
        {
            return _cache.Get<T>(key);
        }


        public bool Delete<T>(string key, object value)
        {
            var deleted = _cache.Delete<T>(value.ToString(), null);
            return deleted;
        }

        public bool DeleteAll<T>()
        {
            throw new NotImplementedException();
        }

        public bool DropCollection<T>()
        {
            throw new NotImplementedException();
        }

        public bool Update<T>(string key, T data)
        {
            bool updated = false;
            if (data != null)
            {
                _cache.Insert<T>(key, data);
            }
            return updated;
        }

        public object GetCollection<T>()
        {
            throw new NotImplementedException();
        }

        public void DropCollection(string collectionName)
        {
            throw new NotImplementedException();
        }

        #region IDataStorage Members


        public bool Insert<T>(string key, T data, TimeSpan expire)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
