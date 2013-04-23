using System;
using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.DataAccess.Provider.Report
{
    public abstract class NonePTMemSummaryProvider : DataAccess 
    {
        public static NonePTMemSummaryProvider Instance()
        {
            return (NonePTMemSummaryProvider)Activator.CreateInstance(Type.GetType(string.Format(Globals.IBCSetting.Report.NonePTMemSummaryProvider, GetDAOClient(DAOClient.AdoNet), GetDataSource(DataSource.SqlServer))));
        }

        public virtual DbResult NonePTMemSummary(Hashtable param)
        {
            throw new NotImplementedException();
        }

    }
}