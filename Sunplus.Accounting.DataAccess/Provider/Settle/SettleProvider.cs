using System;
using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

/// <summary>
/// Summary description for SettleProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.Settle
{
    public abstract class SettleProvider : DataAccess
    {
        public static SettleProvider Instance(DAOClient client)
        {
            return
                (SettleProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.Settle.SettleProviderType, GetDAOClient(client),
                                               GetDataSource(DataSource.SqlServer))));
        }

        public static SettleProvider Instance()
        {
            return
                (SettleProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.Settle.SettleProviderType,
                                               GetDAOClient(DAOClient.AdoNet), GetDataSource(DataSource.SqlServer))));
        }

        public virtual DbResult SelMatchSettleLog(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult SelMatchInfo(Hashtable param)
        {
            throw new NotImplementedException();
        }
    }
}