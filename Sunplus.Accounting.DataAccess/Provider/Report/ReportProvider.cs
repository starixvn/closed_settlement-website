 /// <summary>
/// Summary description for ReportProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.Report
{
    public class ReportProvider
    {
        public static ReportProvider Instance
        {
            get { return new ReportProvider(); }
        }

        /// <summary>
        /// Default provider client; DAO Client: AdoNet; DataSource: SqlServer
        /// </summary>
        /// <returns></returns>
        public LeagueWLProvider LeagueWL()
        {
            return LeagueWLProvider.Instance();
        }

        /// <summary>
        /// League Win/Loss Provider
        /// </summary>
        /// <param name="client">DAO Client: AdoNet, Linq, WCF...</param>
        /// <param name="source">Data Source: SqlServer, Access, Oracle, DB2</param>
        /// <returns></returns>
        public LeagueWLProvider LeagueWL(DataAccess.DAOClient client, DataAccess.DataSource source)
        {
            return LeagueWLProvider.Instance(client, source);
        }


        public MatchWLProvider MatchWL(DataAccess.DAOClient client, DataAccess.DataSource source)
        {
            return MatchWLProvider.Instance(client, source);
        }

        public PunterAnalysisProvider PunterAnalysis()
        {
            return PunterAnalysisProvider.Instance();
        }

        public OpenToteSettlementProvider OpenToteSettlement()
        {
            return OpenToteSettlementProvider.Instance();
        }


        public HRGroupDailyProvider HRGroupDaily()
        {
            return HRGroupDailyProvider.Instance();
        }

        public NonePTProvider NonePT()
        {
            return NonePTProvider.Instance();
        }

        public DirectBalanceProvider DirectBalance()
        {
            return DirectBalanceProvider.Instance();
        }

        public DownLineBalanceProvider DownLineBalance()
        {
            return DownLineBalanceProvider.Instance();
        }
    }
}