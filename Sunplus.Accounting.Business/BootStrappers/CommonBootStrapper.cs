
using Microsoft.Practices.ServiceLocation;

namespace Sunplus.Accounting.Business.BootStrappers
{
    /// <summary>
    /// Summary description for CommonBootStrapper
    /// </summary>
    public abstract class CommonBootStrapper
    {
        public static IServiceLocator Locator;

        public CommonBootStrapper()
        {
            Locator = CreateServiceLocator();
        }

        protected abstract IServiceLocator CreateServiceLocator();
    }
}