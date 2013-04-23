using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Fanex.Data;
using Sunplus.Accounting.Entities.Report.MatchAnalysis;

namespace Sunplus.Accounting.DataAccess.Report.MatchAnalysis
{
    public class MatchAnalysisDAO:IMatchAnalysisDAO
    {
        public System.Data.DataTable SelMatchAnalysisByType(int matchId, int statusId, ReportType reportType)
        {
            string spName = "Acc_Rpt_MatchAnalysisByCurrency";

            switch (reportType)
            {
                case ReportType.Currency:
                    spName = "Acc_Rpt_MatchAnalysisByCurrency";
                    break;
                case ReportType.Bettype:
                    spName = "Acc_Rpt_MatchAnalysisByBettype";
                    break;
                case ReportType.Super:
                    spName = "Acc_Rpt_MatchAnalysisBySuper";
                    break;
                case ReportType.Member:
                    spName = "Acc_Rpt_MatchAnalysisByMember";
                    break;
                case ReportType.Country:
                    spName = "Acc_Rpt_MatchAnalysisByCountry";
                    break;
            }

            using (ObjectDb db = new ObjectDb(spName))
            {
                DataTable dt = new DataTable();
                db.AddInParameter("matchid", DbType.Int32, matchId);
                db.AddInParameter("statusid", DbType.Int32, statusId);
                var result = db.ExecuteDataSet();
                if (result.Tables.Count == 0)
                    return dt;
                return  result.Tables[0];
            }
        }




       
    }
}
