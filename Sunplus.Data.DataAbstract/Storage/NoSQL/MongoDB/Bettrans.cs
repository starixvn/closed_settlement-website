using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;


namespace Sunplus.Accounting.DataAbstract.Storage.NoSQL.MongoDB
{
    public class Bettrans : DataAccess, IBettransStorage
    {
        private string _tableName;
        private MongoServer _server;
        private MongoDatabase _database;

        private void Initialise()
        {
            if (_server != null) return;
            _server = MongoServer.Create(base.ConnectionString);
            _database = _server.GetDatabase(base.DbName);
            _tableName = "bettrans";
        }

        public List<T> GetAllRows<T>()
        {
            this.Initialise();
            using (_server.RequestStart(_database))
            {
                var dataTable = _database.GetCollection<T>(this._tableName);
                return dataTable.FindAll().ToList();
            }
        }

        public List<T> GetRows<T>(string[] keys, object[] values)
        {
            this.Initialise();
            //------------------------------------------------------
            if (keys == null || values == null)
            {
                return null;
            }
            if (keys.Length != values.Length)
            {
                return null;
            }

            //------------------------------------------------------
            using (_server.RequestStart(_database))
            {
                var dataTable = _database.GetCollection<T>(this._tableName);
                //------------------------------------------------------
                //Build query list
                var queryCompletes = new QueryComplete[keys.Length];
                for (var i = 0; i < keys.Length; ++i)
                {
                    queryCompletes[i] = new QueryComplete(new BsonDocument(keys[i], BsonValue.Create(values[i])));
                }
                var cursor = dataTable.Find(Query.And(queryCompletes));
                return cursor.ToList<T>();
            }
        }

        public List<T> GetRows<T>(string key, object value)
        {
            this.Initialise();
            if (string.IsNullOrEmpty(key) || value == null)
            {
                return null;
            }

            //------------------------------------------------------
            using (_server.RequestStart(_database))
            {
                var dataTable = _database.GetCollection<T>(this._tableName);
                var queryCompletes = new QueryComplete[1];
                queryCompletes[0] = new QueryComplete(new BsonDocument(key, BsonValue.Create(value)));
                return dataTable.Find(Query.And(queryCompletes)).ToList();
            }
        }

        public bool Insert(object row)
        {
            this.Initialise();
            using (_server.RequestStart(_database))
            {
                var dataTable = _database.GetCollection<BsonDocument>(this._tableName);
                try
                {
                    dataTable.Insert(row.ToBsonDocument());
                    return true;
                }
                catch (Exception exception)
                {
                    return false;
                }
            }
        }

        public bool InsertBatch(IEnumerable<object> rows)
        {
            this.Initialise();
            using (_server.RequestStart(_database))
            {   
                var dataTable = _database.GetCollection<BsonDocument>(this._tableName);
                try
                {
                    var bsonDocuments = rows.Select(row => row.ToBsonDocument()).ToList();
                    dataTable.InsertBatch(bsonDocuments);
                    return true;
                }
                catch (Exception exception)
                {
                    return false;
                }
            }
        }

        public bool Update(string key, object value, object row)
        {
            this.Initialise();
            if (string.IsNullOrEmpty(key) || value == null || row == null)
            {
                return false;
            }
            //------------------------------------------------------
            try
            {
                using (_server.RequestStart(_database))
                {
                    var dataTable = _database.GetCollection<BsonDocument>(this._tableName);
                    var queryCompletes = new QueryComplete[1];
                    queryCompletes[0] = new QueryComplete(new BsonDocument(key, BsonValue.Create(value)));

                    var bsonDocument = row.ToBsonDocument();
                    var update = new global::MongoDB.Driver.Builders.UpdateBuilder();

                    var fieldsName = base.GetListPropertyName(row.GetType());
                    foreach (var fieldName in fieldsName.Where(fieldName => !fieldName.Equals("_id")))
                    {
                        update.Set(fieldName, BsonValue.Create(bsonDocument[fieldName]));
                    }
                    if (fieldsName.Length > 0)
                    {
                        var sortBy = SortBy.Descending(fieldsName[1]);
                        dataTable.FindAndModify(Query.And(queryCompletes), sortBy, update, true);
                    }
                    return true;
                }
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public bool Remove(string key, object value)
        {
            this.Initialise();
            using (_server.RequestStart(_database))
            {
                var dataTable = _database.GetCollection<BsonDocument>(this._tableName);
                try
                {
                    dataTable.Remove(Query.EQ(key, BsonValue.Create(value)));
                    return true;
                }
                catch (Exception exception)
                {
                    return false;
                }
            }
        }

        public bool Remove(string[] keys, object[] values)
        {
            this.Initialise();
            using (_server.RequestStart(_database))
            {
                var dataTable = _database.GetCollection<BsonDocument>(this._tableName);
                try
                {
                    //------------------------------------------------------
                    //Build query list
                    var queryCompletes = new QueryComplete[keys.Length];
                    for (var i = 0; i < keys.Length; ++i)
                    {
                        queryCompletes[i] = new QueryComplete(new BsonDocument(keys[i], BsonValue.Create(values[i])));
                    }
                    dataTable.Remove(Query.And(queryCompletes));
                    return true;
                }
                catch (Exception exception)
                {
                    return false;
                }
            }
        }

        public bool RemoveAll()
        {
            this.Initialise();
            using (_server.RequestStart(_database))
            {
                var dataTable = _database.GetCollection<BsonDocument>(this._tableName);
                try
                {
                    dataTable.RemoveAll();
                    return true;
                }
                catch (Exception exception)
                {
                    return false;
                }
            }
        }

        #region wrapped
        public object Get(string key)
        {
            return null;
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public bool Insert(string key, object value)
        {

            return false;
        }

        public bool Delete(string key)
        {

            return false;
        }
        #endregion
    }
}
