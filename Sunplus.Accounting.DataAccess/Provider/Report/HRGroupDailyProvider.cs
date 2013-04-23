using System;
using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.DataAccess.Provider.Report
{
    public abstract class HRGroupDailyProvider : DataAccess
    {
        public static HRGroupDailyProvider Instance()
        {
            return
                (HRGroupDailyProvider)
                Activator.CreateInstance(
                    Type.GetType(string.Format(Globals.IBCSetting.Report.HRGroupDailyProviderType,
                                               GetDAOClient(DAOClient.AdoNet), GetDataSource(DataSource.SqlServer))));
        }

        /// <summary>
        /// OpenTote - new report
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual DbResult WinlostHR_Group_Daily(Hashtable param)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opten Tote - new report
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual DbResult WinlostHR_Group(Hashtable param)
        {
            throw new NotImplementedException();
        }
    }
}