using System;
using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

/// <summary>
/// Summary description for ExChangeRateProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.ExchangeRate
{
    public abstract class ExChangeRateProvider : DataAccess
    {
        public static ExChangeRateProvider Instance()
        {
            return
                (ExChangeRateProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.ExChangeRate.ExChangeRateProviderType,
                                               GetDAOClient(DAOClient.AdoNet), GetDataSource(DataSource.SqlServer))));
        }

        public virtual DbResult ExchangeSelAccount()
        {
            throw new NotImplementedException();
        }

        public virtual DbResult ExchangeIns(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult ExchangeSelByID(int exid)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult ExchangeUpd(Hashtable param)
        {
            throw new NotImplementedException();
        }
    }
}