using Sunplus.Accounting.DataAccess.Provider.ExchangeRate;
using Sunplus.Accounting.DataAccess.Provider.ExportStatement;
using Sunplus.Accounting.DataAccess.Provider.Report;
using Sunplus.Accounting.DataAccess.Provider.Settle;


namespace Sunplus.Accounting.DataAccess
{
    public static class IBCProvider
    {
        public static CasinoProvider Casino
        {
            get { return CasinoProvider.Instance; }
        }

        public static IBCCustomerProvider IBCCustomer
        {
            get { return IBCCustomerProvider.Instance; }
        }

        public static PositionTakingProvider PositionTaking
        {
            get { return PositionTakingProvider.Instance; }
        }

        public static PositionTakingProvider PositionTakingHR
        {
            get { return PositionTakingProvider.InstanceHR; }
        }

        public static UtilsProvider Utils
        {
            get { return UtilsProvider.Instance; }
        }

        public static SettleProvider Settle
        {
            get { return SettleProvider.Instance(); }
        }

        public static FIWLProvider FIWLDetails
        {
            get { return FIWLProvider.Instance(); }
        }

        public static ExChangeRateProvider ExChangeRate
        {
            get { return ExChangeRateProvider.Instance(); }
        }

        public static ExportStatementProvider ExportStatement
        {
            get { return ExportStatementProvider.Instance(); }
        }

        public static ReportProvider ReportProvider
        {
            get { return ReportProvider.Instance; }
        }
    }
}