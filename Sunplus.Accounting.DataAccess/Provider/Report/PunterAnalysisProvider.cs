using System;
using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.DataAccess.Provider.Report
{
    public abstract class PunterAnalysisProvider : DataAccess
    {
        public static PunterAnalysisProvider Instance()
        {
            return
                (PunterAnalysisProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.Report.PunterAnalysisProviderType,
                                               GetDAOClient(DAOClient.AdoNet), GetDataSource(DataSource.SqlServer))));
        }

        public virtual DbResult SelTopPunterSummary(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult SelTopPunterByCustId(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult SelTopPunterBySporttype(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult SelTopPunterByBettype(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult SelIPByCustomer(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult SelBetCountByCustomer(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult DirectCustomerReport(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult SelSports()
        {
            throw new NotImplementedException();
        }

        public virtual DbResult SelLeagues(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult SelTopPunterSummaryByLeague(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult SelTopPunterByLeagueCustId(Hashtable param)
        {
            throw new NotImplementedException();
        }
    }
}