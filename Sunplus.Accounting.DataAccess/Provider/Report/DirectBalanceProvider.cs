using System;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.DataAccess.Provider.Report
{
    public abstract class DirectBalanceProvider : DataAccess
    {
        public static DirectBalanceProvider Instance()
        {
            return
                (DirectBalanceProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.Report.DirectBalanceProvider,
                                               GetDAOClient(DAOClient.AdoNet), GetDataSource(DataSource.SqlServer))));
        }


        public virtual DbResult Balance_Direct_Master(string db)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult Balance_Direct_Agent(string db)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult Balance_Direct_Member(string db)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult Balance_Direct_MasterDownline(string db, int masterid)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult Balance_Direct_AgentDownline(string db, int agentid)
        {
            throw new NotImplementedException();
        }
    }
}