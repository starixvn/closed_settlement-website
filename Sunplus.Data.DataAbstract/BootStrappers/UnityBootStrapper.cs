using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Sunplus.Data.DataAbstract.DAO;
using Sunplus.Data.DataAbstract.Storage;
using Sunplus.Data.DataAbstract.Storage.Cache;

namespace Sunplus.Data.DataAbstract.BootStrappers
{
    /// <summary>
    /// Summary description for UnityBootStrapper
    /// </summary>
    public class UnityBootStrapper : CommonBootStrapper
    {
        private static Configurations.StorageSection.NoSQLType _NoSQLType;
        public UnityBootStrapper()
        {
            _NoSQLType = Configurations.StorageConfiguration.NoSQLType();
        }
        protected override IServiceLocator CreateServiceLocator()
        {
            IUnityContainer container = new UnityContainer();
            Register(container);
            return new UnityServiceLocator(container);
        }

        /// <summary>
        /// Register all type to resolve 
        /// </summary>
        /// <param name="container"></param>
        private static void Register(IUnityContainer container)
        {
            if (_NoSQLType == Configurations.StorageSection.NoSQLType.Membase)
            {
                container
                    .RegisterType<IDataStorage, CacheDataStorage>()
                    //.RegisterType<IBetListDAO, BetListDAO>()
                    //.RegisterType<IIpVnDAO, IpVnDAO>()
                ;
            }
            
        }

        private static void RegisterType<T1, T2>()
        {
            throw new System.NotImplementedException();
        }       
    }
}
