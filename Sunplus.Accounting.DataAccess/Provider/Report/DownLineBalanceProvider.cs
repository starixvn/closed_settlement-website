using System;
using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.DataAccess.Provider.Report
{
    public abstract class DownLineBalanceProvider : DataAccess
    {
        public static DownLineBalanceProvider Instance()
        {
            return
                (DownLineBalanceProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.Report.DownLineBalanceProvider,
                                               GetDAOClient(DAOClient.AdoNet), GetDataSource(DataSource.SqlServer))));
        }

        public virtual DbResult RptMABalance(string db, int roleid, int custid, bool isYesterday, string username,
                                             string firstletter, string currency)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult RptSABalance(string db, bool isYesterday, Hashtable p)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult BalanceCompHistory()
        {
            throw new NotImplementedException();
        }

        public virtual DbResult BalanceCompHistory(DateTime BalanceUpDay)
        {
            throw new NotImplementedException();
        }
    }
}