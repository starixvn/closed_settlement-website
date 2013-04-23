using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunplus.Accounting.DataAbstract.Storage.NoSQL.Cassandra
{
    public class Bettrans : IBettransStorage
    {
        public object Get(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool Insert(string key, object value)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string key)
        {
            throw new NotImplementedException();
        }

        public List<T> GetRows<T>(string[] keys, object[] values)
        {
            throw new NotImplementedException();
        }

        public List<T> GetRows<T>(string key, object value)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAllRows<T>()
        {
            throw new NotImplementedException();
        }

        public bool Insert(object row)
        {
            throw new NotImplementedException();
        }

        public bool InsertBatch(IEnumerable<object> rows)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key, object value)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string[] keys, object[] values)
        {
            throw new NotImplementedException();
        }

        public bool RemoveAll()
        {
            throw new NotImplementedException();
        }

        public bool Update(string key, object value, object row)
        {
            throw new NotImplementedException();
        }
    }
}
