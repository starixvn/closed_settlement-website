using System;
using System.Linq;
using Sunplus.Data.DataAbstract.Configurations;

namespace Sunplus.Data.DataAbstract.Storage.NoSQL.MongoDB
{
    public class DataAccess
    {

        protected NoSQLElement NoSqlElement;
        public DataAccess()
        {
            NoSqlElement = StorageConfiguration.NoSQL();
        }

        protected string[] GetListPropertyName(Type type)
        {
            var propertyInfos = type.GetProperties();
            var listPropertyName = propertyInfos.Select(propertyInfo => propertyInfo.Name).ToList();
            return listPropertyName.ToArray();
        }
    }
}
