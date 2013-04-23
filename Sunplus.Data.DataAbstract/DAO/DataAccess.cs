using Norm;
using Sunplus.Data.DataAbstract.BootStrappers;
using Sunplus.Data.DataAbstract.Configurations;
using Sunplus.Data.DataAbstract.Storage;

namespace Sunplus.Data.DataAbstract.DAO
{
    public class DataAccess
    {
        protected IDataStorage DataStorage = null;
        protected NoSQLElement NoSqlElement = null; 
        protected readonly Mongo Provider;

        protected readonly MongoDatabase Database;

        public DataAccess()
        {
            NoSqlElement = StorageConfiguration.NoSQL();
            DataStorage = CommonBootStrapper.Locator.GetInstance<IDataStorage>();
            //Provider = new Mongo(NoSqlElement.DbName, NoSqlElement.Host, NoSqlElement.Port.ToString(), NoSqlElement.Option);
            //Database = Provider.Database;
        }
    }
}
