using System;
using System.Data;

/// <summary>
/// Summary description for LogProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.Log
{
    public abstract class LogProvider : DataAccess
    {
        private static string _ProviderType = "DAL.{0}.{1}.Log.LogClient";

        public static LogProvider Instance()
        {
            return (LogProvider) CreateInstance(_ProviderType, DAOClient.AdoNet, DataSource.SqlServer);
        }

        public static LogProvider Instance(DAOClient client, DataSource source)
        {
            return (LogProvider) CreateInstance(_ProviderType, client, source);
        }

        public virtual DataRowCollection GeneralLog(int custId)
        {
            throw new NotImplementedException();
        }

        public virtual DataRowCollection CreditLog(int custId)
        {
            throw new NotImplementedException();
        }
    }
}