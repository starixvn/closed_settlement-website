using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Provider.Report;

/// <summary>
/// Summary description for PunterAnalysisProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.Report
{
    public class DirectBalanceProviderClient : DirectBalanceProvider
    {
        public override DbResult Balance_Direct_Master(string db)
        {
            string dbtype = DBType.REPORT;
            if (db == "main")
                dbtype = DBType.MAIN;
            return DbHelper.Execute("Acc_Rpt_Balance_Direct_Master", dbtype);
        }

        public override DbResult Balance_Direct_Agent(string db)
        {
            string dbtype = DBType.REPORT;
            if (db == "main")
                dbtype = DBType.MAIN;
            return DbHelper.Execute("Acc_Rpt_Balance_Direct_Agent", dbtype);
        }

        public override DbResult Balance_Direct_Member(string db)
        {
            string dbtype = DBType.REPORT;
            if (db == "main")
                dbtype = DBType.MAIN;
            return DbHelper.Execute("Acc_Rpt_Balance_Direct_Member", dbtype);
        }

        public override DbResult Balance_Direct_MasterDownline(string db, int masterid)
        {
            var param = new Hashtable();
            param.Add("masterid", masterid);
            string dbtype = DBType.REPORT;
            if (db == "main")
                dbtype = DBType.MAIN;
            return DbHelper.Execute("Acc_Rpt_Balance_Agent", param, dbtype, 600);
        }

        public override DbResult Balance_Direct_AgentDownline(string db, int agentid)
        {
            var param = new Hashtable();
            param.Add("agentid", agentid);
            string dbtype = DBType.REPORT;
            if (db == "main")
                dbtype = DBType.MAIN;
            return DbHelper.Execute("Acc_Rpt_Balance_Member", param, dbtype, 600);
        }
    }
}