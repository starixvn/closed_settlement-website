using Sunplus.Data.DataAbstract.BootStrappers;

namespace Sunplus.Data.DataAbstract.Storage.NoSQL
{
    public class Abstract
    {
        private static Configurations.StorageSection.NoSQLType _NoSQLType;

        public Abstract() : base()
        {
            _NoSQLType = Configurations.StorageConfiguration.NoSQLType();
        }

        public static IDataStorage Bettrans()
        {
            var bettrans = CommonBootStrapper.Locator.GetInstance<IDataStorage>();
            return bettrans;
        }

        

        ~Abstract() { }
    }

   
}
