using System;
using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.DataAccess.Provider.Report
{
    public abstract class OpenToteSettlementProvider : DataAccess
    {
        public static OpenToteSettlementProvider Instance()
        {
            return
                (OpenToteSettlementProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.Report.OpenToteSettlementProviderType,
                                               GetDAOClient(DAOClient.AdoNet), GetDataSource(DataSource.SqlServer))));
        }

        public virtual DbResult WinlostHR_OpenTote(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult WinlostHR_OpenTote14(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult WinlostHR_OpenTote_Details(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult WinlostHR_OpenTote_Details14(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult WinlostHR_OpenTote_BetList(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult WinlostHR_OpenTote_BetList14(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult Horse_OpenToteTO3rd(Hashtable param)
        {
            throw new NotImplementedException();
        }

        //------ Test-----
        public virtual DbResult WinlostHR_OpenTote_Test(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult WinlostHR_OpenTote14_Test(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult WinlostHR_OpenTote_Details_Test(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult WinlostHR_OpenTote_Details14_Test(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult WinlostHR_OpenTote_BetList_Test(Hashtable param)
        {
            throw new NotImplementedException();
        }

        public virtual DbResult WinlostHR_OpenTote_BetList14_Test(Hashtable param)
        {
            throw new NotImplementedException();
        }
    }
}