using System.Collections;
using DNAide.Data;
using Sunplus.Accounting.Core.Configurations;

namespace Sunplus.Accounting.DataAccess.Report.DailyTurnoverDAO
{
    public class DailyTurnoverDAO : DataAccessReport, IDailyTurnoverDAO
    {
        private StoreProcedureElement getSPMapping(string key)
        {
            return GetStoreProcedure(Sunplus.Accounting.Core.Configurations.StoreProcedureGroup.ReportSession.StoreCollection.DailyTurnover, key);
        }


        public DbResult SelDailyTurnover(string date, bool isActual, string key)
        {
            var element = getSPMapping(key);
            Hashtable hashtable = new Hashtable();
            hashtable.Add("fromdate", date);
            hashtable.Add("isactual", isActual);

            return DbHelper.Execute(element.Name, hashtable, element.DbName,1800);
        }


        public DbResult SelDailyChangeDetial(string date)
        {
            var element = getSPMapping("DailyTurnoverChangeDetail");
            Hashtable hashtable = new Hashtable();
            hashtable.Add("fromdate", date);

            return DbHelper.Execute(element.Name, hashtable, element.DbName, 1800);
            
        }
    }
}
