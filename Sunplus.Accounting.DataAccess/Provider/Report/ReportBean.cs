using System.Collections;
using System.Collections.Generic;
using System.Data;
using DNAide.Data;

/// <summary>
/// Summary description for Report
/// </summary>

namespace Sunplus.Accounting.DataAccess.Provider.Report
{
    public class ReportBean
    {
        public Hashtable Outputs = new Hashtable();
        public List<DataRow[]> lsRows = new List<DataRow[]>();

        public ReportBean(DbResult rs)
        {
            DataTableCollection dts = rs.ResultSet.Tables;

            foreach (DataTable dt in dts)
            {
                lsRows.Add(dt.Select());
            }
            Outputs = rs.Outputs;
        }
    }
}