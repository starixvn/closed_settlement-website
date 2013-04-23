using System;
using Sunplus.Accounting.Core.Common;

/// <summary>
/// Summary description for Report2aProvider
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.Report
{
    public abstract class WLProvider : DataAccess
    {
        /// <summary>
        /// DAO Client: AdoNet;
        /// Data Source: SqlServer
        /// </summary>
        /// <returns></returns>
        public static WLProvider Instance()
        {
            return
                (WLProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.Report.WLProviderType, GetDAOClient(DAOClient.AdoNet),
                                               GetDataSource(DataSource.SqlServer))));
        }

        /// <summary>
        /// Manual DAO Client, Data Source
        /// </summary>
        /// <param name="client"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static WLProvider Instance(DAOClient client, DataSource source)
        {
            return
                (WLProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.Report.WLProviderType, GetDAOClient(client),
                                               GetDataSource(source))));
        }


        public virtual ReportBean MatchWL(DateTime wldate, string db)
        {
            throw new NotImplementedException();
        }

        public virtual ReportBean MatchWL14(DateTime wldate)
        {
            throw new NotImplementedException();
        }
    }
}