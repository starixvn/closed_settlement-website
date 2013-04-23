using System;
using System.Collections.Generic;
using System.Linq;
using Norm;
using Sunplus.Data.DataAbstract.Log;

namespace Sunplus.Data.DataAbstract.Storage.NoSQL.MongoDB
{
    public class MongoDataStorage : DataAccess, INoSQL
    {
        
        private readonly Mongo _server;
        private readonly MongoDatabase _database;

        public MongoDataStorage()
        {
            try
            {
                if (_server != null) return;
                _server = new Mongo(NoSqlElement.DbName, NoSqlElement.Host, NoSqlElement.Port.ToString(),
                                    NoSqlElement.Option);
                _database = _server.Database;
            }
            catch(Exception exception)
            {
                Error.WriteLog(exception);
               
            }
        }

        public T Select<T>(object value)
        {
            try
            {                
                var queryCommand = new {_id = Q.Equals(value.ToString())};
                var result = _database.GetCollection<T>().Find(queryCommand).FirstOrDefault();
                _server.ConnectionProvider.Close(_database.CurrentConnection);
                return result;
            }
            catch (Exception ex)
            {
                Error.WriteLog(ex);
                return default(T);
            }
        }

        IEnumerable<T> IDataStorage.SelectAll<T>()
        {
            try
            {
                var result = _database.GetCollection<T>().Find();
                _server.ConnectionProvider.Close(_database.CurrentConnection);
                return result;
            }
            catch (Exception ex)
            {
                Error.WriteLog(ex);
                return null;
            }
        }

        public bool DropCollection<T>()
        {
            try
            {
                _database.DropCollection(typeof(T).Name);
                _server.ConnectionProvider.Close(_database.CurrentConnection);
                return true;
            }
            catch (Exception exception)
            {
                Error.WriteLog(exception);
                return false;
            }
        }

        public bool Update<T>(string key, T data)
        {
            try
            {
                _database.GetCollection<T>().Save(data);
                _server.ConnectionProvider.Close(_database.CurrentConnection);
                 return true;
               
            }
            catch (Exception exception)
            {
                Error.WriteLog(exception);
                return false;
            }
        }

        public object GetCollection<T>()
        {
            try
            {
                var result = _database.GetCollection<T>();
                _server.ConnectionProvider.Close(_database.CurrentConnection);
                return result;
            }
            catch (Exception ex)
            {
                Error.WriteLog(ex);
                return null;
            }
        }

        public bool Insert<T>(string key, T data)
        {
            try
            {
                _database.GetCollection<T>().Insert(data);
                _server.ConnectionProvider.Close(_database.CurrentConnection);
                return true;
            }
            catch (Exception ex)
            {
                Error.WriteLog(ex);
                return false;
            }
        }

        public bool Insert<T>(string key, T data, TimeSpan expire)
        {
            return true;
        }

        public bool InsertBatch<T>(string key, IEnumerable<T> data)
        {
            try
            {
                _database.GetCollection<T>().Insert(data);
                _server.ConnectionProvider.Close(_database.CurrentConnection);
                return true;
            }
            catch (Exception ex)
            {
                Error.WriteLog(ex);
                return false;
            }
        }

        public bool Delete<T>(string key, object value)
        {
            try
            {
                _database.GetCollection<T>().Delete(new {_id = Q.Equals(value)});
                _server.ConnectionProvider.Close(_database.CurrentConnection);
                return true;
            }
            catch (Exception exception)
            {
                Error.WriteLog(exception);
                return false;
            }
        }

        public bool DeleteAll<T>()
        {
            try
            {
                _database.DropCollection(typeof(T).Name);
                _server.ConnectionProvider.Close(_database.CurrentConnection);
                return true;
            }
            catch (Exception exception)
            {
                Error.WriteLog(exception);
                return false;
            }
        }

        public object Get(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }
    }
}
