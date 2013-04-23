using System;
using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

/// <summary>
/// Summary description for FIWLProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.Report
{
    public class FIWLProvider : DataAccess
    {
        public static FIWLProvider Instance()
        {
            return
                (FIWLProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.Report.FIWLProviderType,
                                               GetDAOClient(DAOClient.AdoNet), GetDataSource(DataSource.SqlServer))));
        }

        public virtual DbResult FIWLDetails(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult FIWLDetails_14(Hashtable param)
        {
            throw new NotImplementedException();
        }
    }
}