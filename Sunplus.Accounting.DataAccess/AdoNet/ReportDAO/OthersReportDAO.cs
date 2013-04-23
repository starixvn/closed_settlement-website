using System;
using System.Collections;
using System.Data;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;
using Sunplus.Accounting.DataAccess.IReportDAO;

namespace Sunplus.Accounting.DataAccess.AdoNet.ReportDAO
{
    public class OthersReportDAO: DataAccess, IOthersReportDAO
    {
        public DataTable SelectNumberOfBetsForCreditSites(DateTime fromDate, DateTime toDate)
        {
            var result = new DataTable();
            var param = new Hashtable();
            param.Add("datestart", fromDate);
            param.Add("dateend", toDate);
            var spElement = GetStoreProcedure("CreditSiteBetCount");
            var ds = DbHelper.Execute(spElement.Name, param, spElement.DbName, 600);
            if (ds != null && ds.ResultSet != null && ds.ResultSet.Tables.Count > 0)
                result = ds.ResultSet.Tables[0];
            return result;
        }

        protected StoreProcedureElement GetStoreProcedure(string site)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.Others, site);
        }


        public DbResult SelDuplicateTickets(string wldate)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("date", wldate);

            var spElement = GetStoreProcedure("SelDuplicateTickets");

            return DbHelper.Execute(spElement.Name, hashtable, spElement.DbName, 600);
        }
    }
}
