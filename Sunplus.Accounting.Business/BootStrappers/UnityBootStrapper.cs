using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Sunplus.Accounting.Business.Admin;

using Sunplus.Accounting.Business.Settle;
using Sunplus.Accounting.DataAccess;
using Sunplus.Accounting.DataAccess.AdoNet.BetAggregatorDAO;
using Sunplus.Accounting.DataAccess.AdoNet.ReportDAO;
using Sunplus.Accounting.DataAccess.IBetAggregatorDAO;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.DataAccess.AdoNet.CreditTermDAO;
using Sunplus.Accounting.DataAccess.AdoNet.PositionTaking.Racing;
using Sunplus.Accounting.DataAccess.ICreditTermDAO;

using Sunplus.Accounting.DataAccess.PositionTaking.Racing;
using Sunplus.Accounting.DataAccess.AdoNet;
using Sunplus.Accounting.DataAccess.AdoNet.Admin;
using Sunplus.Accounting.DataAccess.IAdmin;
using Sunplus.Accounting.DataAccess.Report.MatchAnalysis;
using Sunplus.Accounting.DataAccess.Report.MatchWL;
using Sunplus.Accounting.DataAccess.Settlement;
using Sunplus.Accounting.DataAccess.SpecialDelete;
using Sunplus.Accounting.DataAccess.SpecialDelete.TicketsListing;
using Sunplus.Core.LoggingLib;
using Sunplus.Accounting.DataAccess.PositionTaking;
using Sunplus.Accounting.DataAccess.AdoNet.PositionTaking;
using Sunplus.Accounting.DataAccess.ILogDisplay;
using Sunplus.Accounting.DataAccess.AdoNet.LogDisplayDAO;
using Sunplus.Accounting.Business.LogDisplay;
using Sunplus.Accounting.Business.Transaction;
using Sunplus.Accounting.DataAccess.Report.DailyTurnoverDAO;
using Sunplus.Accounting.DataAccess.Report.ResetWinning;

namespace Sunplus.Accounting.Business.BootStrappers
{
    /// <summary>
    /// Summary description for UnityBootStrapper
    /// </summary>
    public class UnityBootStrapper : CommonBootStrapper
    {
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
            container
                .RegisterType<IUserDAO, UserDAO>()
                .RegisterType<IUserService, UserService>()
                .RegisterType<IMenuManager, MenuManager>()
                .RegisterType<Sunplus.Core.LoggingLib.ILogger, EntLibLogger>(new ContainerControlledLifetimeManager())
                .RegisterType<IExchangeRateDAO, ExchangeRateDAO>()
                .RegisterType<ISpecialDeleteDAO, SpecialDeleteDAO>()
                
                .RegisterType<IOthersDAO, OthersDAO>()
                .RegisterType<IMatchWL2aDAO, MatchWL2aDAO>()
               
                .RegisterType<IHelperDAO, HelperDAO>()
                .RegisterType<IHelper, Helper>()
                .RegisterType<ICustomerDAO, CustomerDAO>()
                .RegisterType<ICreditTermGroupDAO, CreditTermGroupDAO>()
                .RegisterType<IExternalSiteDAO, ExternalSiteDAO>()
                .RegisterType<IExternalSite, ExternalSite>()
                .RegisterType<IRacingDAO, RacingDAO>()
              
                .RegisterType<IMeetingCommDAO, MeetingCommDAO>()
                .RegisterType<IReportRacingDAO, ReportRacingDAO>()
                .RegisterType<IReportMainDAO, ReportMainDAO>()
                .RegisterType<IViewLogManager, ViewLogManager>()
                .RegisterType<ILogDisplayDAO, LogDisplayDAO>()
                .RegisterType<ISettingManager, SettingManager>()
                .RegisterType<IServiceDailyTran, DailyTransManager>()
                .RegisterType<ITicketsListingDAO, TicketsListingDAO>()
                ;

            RegisterReport(container);
            RegisterBetList(container);
            RegisterBA(container);
            RegisterPoker(container);
            RegisterAgencyGroup(container);
            RegisterSettlement(container);
        }

        private static void RegisterType<T1, T2>()
        {
            throw new System.NotImplementedException();
        }

        private static void RegisterReport(IUnityContainer container)
        {
            container
                .RegisterType<ILicenseDAO, LicenseDAO>()
                .RegisterType<Sunplus.Accounting.DataAccess.IReportDAO.INumberGameReportDAO, Sunplus.Accounting.DataAccess.AdoNet.ReportDAO.NumberGameReportDAO>()
                .RegisterType<IOthersReportDAO, OthersReportDAO>()
                .RegisterType<IMonitorDAO, MonitorDAO>()
                
                .RegisterType<IReportWinLossDAO, ReportWinLossDAO>()
                
                .RegisterType<IExternalReportDAO, ExternalReportDAO>()
                .RegisterType<IMatchAnalysisDAO, MatchAnalysisDAO>()
                .RegisterType<IResetWinningDAO, ResetWinningDAO>()
                ;
        }

      

        private static void RegisterBetList(IUnityContainer container)
        {
                //.RegisterType<IBetListManager, BetListManager>()
            //.RegisterType<Sunplus.Accounting.DataAbstract.DAO.IBetListDAO, Sunplus.Accounting.DataAbstract.DAO.BetListDAO>();
        }

        private static void RegisterBA(IUnityContainer container)
        {
            container.RegisterType<ILicenseeConfigDAO, LicenseeConfigDAO>();
            //.RegisterType<Sunplus.Data.DataAbstract.DAO.IBetListDAO, Sunplus.Data.DataAbstract.DAO.BetListDAO>();
        }

        private static void RegisterAgencyGroup(IUnityContainer container)
        {
        }

        private static void RegisterPoker(IUnityContainer container)
        {
            container
                .RegisterType<IPokerReportDAO, PokerReportDAO>()
                .RegisterType<IPokerDAO, PokerDAO>()
                ;
        }

        private static void RegisterSettlement(IUnityContainer container)
        {
            container
                .RegisterType<ISettlement, Sunplus.Accounting.Business.Settle.Settlement>()
                .RegisterType<ISettlementDAO, SettlementDAO>()

                ;

        }

    }
}
