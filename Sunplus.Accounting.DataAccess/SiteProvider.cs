using Sunplus.Accounting.DataAccess.Provider.Betlist;
using Sunplus.Accounting.DataAccess.Provider.Customer;
using Sunplus.Accounting.DataAccess.Provider.Log;
using Sunplus.Accounting.DataAccess.Provider.Report;

/// <summary>
/// Summary description for SiteProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess
{
    public class SiteProvider
    {
        public static ReportProvider Report
        {
            get { return ReportProvider.Instance; }
        }

        public static BetlistProvider Betlist
        {
            get { return BetlistProvider.Instance; }
        }

       

        #region Customer Provider

        public static CustomerProvider Customer()
        {
            return CustomerProvider.Instance();
        }

        public static CustomerProvider Customer(DataAccess.DAOClient client, DataAccess.DataSource source)
        {
            return CustomerProvider.Instance(client, source);
        }

        #endregion

        #region Log Provider

        public static LogProvider Log()
        {
            return LogProvider.Instance();
        }

        public static LogProvider Log(DataAccess.DAOClient client, DataAccess.DataSource source)
        {
            return LogProvider.Instance(client, source);
        }

        #endregion
    }
}