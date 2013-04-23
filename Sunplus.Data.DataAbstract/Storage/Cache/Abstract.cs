using System;
using System.Collections.Generic;
using Sunplus.Data.DataAbstract.Configurations;

namespace Sunplus.Data.DataAbstract.Storage.Cache
{
    public abstract class Abstract : Storage.IDataStorage
    {
        protected bool IsCacing;
        protected ICache Caching;

        public void InitCaching()
        {
            IsCacing = StorageConfiguration.Caching();
            if (IsCacing && Caching == null)
            {
                Caching = new Extentions.Memcached();
            }
        }

        public abstract object Get(string key);

        public abstract T Get<T>(string key);
        public bool InsertBatch<T>(string key, IEnumerable<T> data)
        {
            throw new NotImplementedException();
        }


        public abstract void FlushAll();

        public T Select<T>(object value)
        {
            throw new NotImplementedException();
        }

        IEnumerable<T> IDataStorage.SelectAll<T>()
        {
            throw new NotImplementedException();
        }

        public virtual bool Insert<T>(string key, T data)
        {
            throw new NotImplementedException();
        }

        public virtual bool Insert<T>(string key, T data, TimeSpan expire)
        {
            throw new NotImplementedException();
        }

        public virtual bool Insert<T>(string key, IEnumerable<T> data)
        {
            throw new NotImplementedException();
        }

        public virtual bool Delete<T>(string key, object data)
        {
            throw new NotImplementedException();
        }

        public virtual bool DeleteAll<T>()
        {
            throw new NotImplementedException();
        }

        public bool DropCollection<T>()
        {
            throw new NotImplementedException();
        }

        public virtual bool Update<T>(string key, T data)
        {
            throw new NotImplementedException();
        }

        public object GetCollection<T>()
        {
            throw new NotImplementedException();
        }

        public void DropCollection(string collectionName)
        {
            throw new NotImplementedException();
        }
    }
}
