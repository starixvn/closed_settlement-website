using System;
using System.Collections;
using DNAide.Core;
using DNAide.Data;

using Sunplus.Accounting.Core.Configurations;
using System.Data;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Configurations.StoreProcedureGroup;
using Sunplus.Accounting.DataAccess.IReportDAO;
using Sunplus.Accounting.DataAccess.Report;
using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class ReportMainDAO : DataAccessReport, IReportMainDAO
    {
        public DbResult SelectMixparlayDetail(string winlossDate, int betytpe)
        {
            DbResult result = null;
            try
            {
                Hashtable p = new Hashtable();

                p.Add("winlostdate", winlossDate);
                p.Add("bettype", betytpe);

                /*if (ReportUtils.IsRepDB)
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Main]["Acc_Rpt_DailyWLDirect_MixParlay_REPORT"];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }
                else*/
                {
                    var sp = ReportSession.Instance[ReportSession.StoreCollection.Main]["DailyWLDirect_MixParlayAdvance" + GetStoreMappingSubfix(CastAide.AsDateTime(winlossDate))];
                    result = DbHelper.Execute(sp.Name, p, sp.DbName, 300);
                }
            }
            catch (Exception ex)
            {
                
                    Sunplus.Accounting.Core.Extensions.LogExtensions.ExceptionHandler(ex);

            }
            return result;
        }
    }
}
