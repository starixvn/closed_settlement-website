using System;
using System.Data;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

/// <summary>
/// Summary description for ReportProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.Customer
{
    public abstract class CustomerProvider : DataAccess
    {
        /// <summary>
        /// DAO Client: AdoNet;
        /// Data Source: SqlServer
        /// </summary>
        /// <returns></returns>
        public static CustomerProvider Instance()
        {
            return
                (CustomerProvider)
                CreateInstance(Globals.IBCSetting.Customer.ProviderType, DAOClient.AdoNet, DataSource.SqlServer);
        }

        /// <summary>
        /// Manual DAO Client, Data Source
        /// </summary>
        /// <param name="client"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static CustomerProvider Instance(DAOClient client, DataSource source)
        {
            return (CustomerProvider) CreateInstance(Globals.IBCSetting.Customer.ProviderType, client, source);
        }

        public virtual DbResult GetDownLineList(int custid, int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }

        public virtual DataRowCollection GetOnLineList(int custid)
        {
            throw new NotImplementedException();
        }
    }
}