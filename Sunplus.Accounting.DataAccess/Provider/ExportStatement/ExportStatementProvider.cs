using System;
using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

/// <summary>
/// Summary description for ExportStatementProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.ExportStatement
{
    public abstract class ExportStatementProvider : DataAccess
    {
        public static ExportStatementProvider Instance()
        {
            return
                (ExportStatementProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.ExportStatement.ExportStatementProviderType,
                                               GetDAOClient(DAOClient.AdoNet), GetDataSource(DataSource.SqlServer))));
        }

        public virtual DbResult AllStatement_SelCheckByUser(string userlist)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult AllStatement_UsersList(Hashtable param)
        {
            throw new NotImplementedException();
        }
    }
}