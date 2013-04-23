using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNAide.Data;

namespace Sunplus.Accounting.DataAccess.Report.DailyTurnoverDAO
{
    public interface IDailyTurnoverDAO
    {
        DbResult SelDailyTurnover(string date, bool isActual, string key);

        DbResult SelDailyChangeDetial(string date);
    }
}
