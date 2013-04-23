using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.DataAccess.Provider.Report;

/// <summary>
/// Summary description for PunterAnalysisProviderClient
/// </summary>

namespace Sunplus.Accounting.DataAccess.AdoNet.SqlServer.Report
{
    public class OpenToteSettlementProviderClient : OpenToteSettlementProvider
    {
        public override DbResult WinlostHR_OpenTote(Hashtable param)
        {
            string db = DBType.MAIN;
            if (param["db"].ToString() == "rep")
                db = DBType.WAREHOUSE;
            return DbHelper.Execute("Acc_Rpt_WinlostHR_OpenTote", param, db, 600);
        }

        public override DbResult WinlostHR_OpenTote14(Hashtable param)
        {
            return DbHelper.Execute("Acc_Rpt_WinlostHR_OpenTote14", param, DBType.WAREHOUSE, 600);
        }

        public override DbResult WinlostHR_OpenTote_Details(Hashtable param)
        {
            string db = DBType.MAIN;
            if (param["db"].ToString() == "rep")
                db = DBType.WAREHOUSE;
            return DbHelper.Execute("Acc_Rpt_WinlostHR_OpenTote_Details", param, db, 600);
        }

        public override DbResult WinlostHR_OpenTote_Details14(Hashtable param)
        {
            return DbHelper.Execute("Acc_Rpt_WinlostHR_OpenTote_Details14", param, DBType.WAREHOUSE, 600);
        }

        public override DbResult WinlostHR_OpenTote_BetList(Hashtable param)
        {
            string db = DBType.MAIN;
            if (param["db"].ToString() == "rep")
                db = DBType.WAREHOUSE;
            return DbHelper.Execute("Acc_Rpt_WinlostHR_OpenTote_BetList", param, db, 600);
        }

        public override DbResult WinlostHR_OpenTote_BetList14(Hashtable param)
        {
            return DbHelper.Execute("Acc_Rpt_WinlostHR_OpenTote_BetList14", param, DBType.WAREHOUSE, 600);
        }

        public override DbResult Horse_OpenToteTO3rd(Hashtable param)
        {
            return DbHelper.Execute("Acc_Rpt_Horse_OpenToteTO3rd", param, DBType.WAREHOUSE, 600);
        }

        //--------Test--------

        public override DbResult WinlostHR_OpenTote_Test(Hashtable param)
        {
            string db = DBType.MAIN;
            if (param["db"].ToString() == "rep")
                db = DBType.WAREHOUSE;
            return DbHelper.Execute("Acc_Rpt_WinlostHR_OpenTote_xTest", param, db, 600);
        }

        public override DbResult WinlostHR_OpenTote14_Test(Hashtable param)
        {
            return DbHelper.Execute("Acc_Rpt_WinlostHR_OpenTote14_xTest", param, DBType.WAREHOUSE, 600);
        }

        public override DbResult WinlostHR_OpenTote_Details_Test(Hashtable param)
        {
            string db = DBType.MAIN;
            if (param["db"].ToString() == "rep")
                db = DBType.WAREHOUSE;
            return DbHelper.Execute("Acc_Rpt_WinlostHR_OpenTote_Details_xTest", param, db, 600);
        }

        public override DbResult WinlostHR_OpenTote_Details14_Test(Hashtable param)
        {
            return DbHelper.Execute("Acc_Rpt_WinlostHR_OpenTote_Details14_xTest", param, DBType.WAREHOUSE, 600);
        }

        public override DbResult WinlostHR_OpenTote_BetList_Test(Hashtable param)
        {
            string db = DBType.MAIN;
            if (param["db"].ToString() == "rep")
                db = DBType.WAREHOUSE;
            return DbHelper.Execute("Acc_Rpt_WinlostHR_OpenTote_BetList_xTest", param, db, 600);
        }

        public override DbResult WinlostHR_OpenTote_BetList14_Test(Hashtable param)
        {
            return DbHelper.Execute("Acc_Rpt_WinlostHR_OpenTote_BetList14_xTest", param, DBType.WAREHOUSE, 600);
        }
    }
}