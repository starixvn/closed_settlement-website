using System;
using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.DataAccess.Provider.Report
{
    public abstract class NonePTProvider : DataAccess
    {
        public static NonePTProvider Instance()
        {
            return
                (NonePTProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.Report.NonePTProvider, GetDAOClient(DAOClient.AdoNet),
                                               GetDataSource(DataSource.SqlServer))));
        }

        public virtual DbResult NonePTMemSummary(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult NonePTLastestBet(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult GetBettypeList()
        {
            throw new NotImplementedException();
        }

        public virtual DbResult GetCurrencyList()
        {
            throw new NotImplementedException();
        }
    }
}