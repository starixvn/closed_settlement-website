using System;
using System.Collections;
using System.Collections.Generic;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.DataAccess.Entity.SqlServer.Report;

/// <summary>
/// Summary description for MatchWLProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.Report
{
    using System.Data;

    public abstract class MatchWLProvider : DataAccess
    {
        public static MatchWLProvider Instance(DAOClient client, DataSource source)
        {
            return
                (MatchWLProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.Report.MatchWLProviderType, GetDAOClient(client),
                                               GetDataSource(source))));
        }

        public virtual List<MatchWLRow> MatchWL(DateTime wldate, ref Hashtable Ouputs, string db)
        {
            throw new NotImplementedException();
        }

        public virtual List<MatchWLRow> MatchWL14(DateTime wldate, ref Hashtable Outputs)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult MatchWL(DateTime wldate, string db)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult MatchWL14(DateTime wldate)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult MatchWL_WH(DateTime wldate)
        {
            throw new NotImplementedException();
        }

        public virtual DataSet MatchWLSingleBet(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult WinLossByBettype(DateTime wldate, string db)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult WinLossByBettype14(DateTime wldate)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult WinLossByBettype_WH(DateTime wldate)
        {
            throw new NotImplementedException();
        }
    }
}