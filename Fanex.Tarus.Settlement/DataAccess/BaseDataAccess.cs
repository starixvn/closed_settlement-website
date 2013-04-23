using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Fanex.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;

namespace Fanex.Tarus.Settlement.DataAccess
{
    public class BaseDataAccess
    {
        public DataSet GetLeagues(string fdate, string tdate)
        {
            using (ObjectDb objDB = new ObjectDb("Acc_Set_LeagueSel"))
            {
                objDB.AddInParameter("fromdate", DbType.DateTime, fdate);
                objDB.AddInParameter("todate", DbType.DateTime, tdate);
                objDB.AddInParameter("sporttype", DbType.Int32, 162);
                var ds = objDB.ExecuteDataSet();
                return ds;
            }
        }

        public virtual DataSet GetMatch(int MatchId)
        {
            throw new NotImplementedException();
        }
    }
}
