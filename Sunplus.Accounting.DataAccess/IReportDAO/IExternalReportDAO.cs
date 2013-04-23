using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNAide.Data;
using Sunplus.Accounting.Entities.Report.External;

namespace Sunplus.Accounting.DataAccess.IReportDAO
{
    public interface IExternalReportDAO
    {
        DbResult SelWinLossDailyByCom(string winlostdate);
        DbResult SelWinLossDailyBySuper(string winlostdate, int custid);
        DbResult SelWinLossDailyByMaster(string winlostdate, int custid);
        DbResult SelWinLossDailyByAgent(string winlostdate, int custid);

        DbResult SelWinLossDetailByCom(string param);
        DbResult SelWinLossDetailBySuper(int custid, string param, int pageIndex, int pageSize);
        DbResult SelWinLossDetailByMaster(int custid, string param, int pageIndex, int pageSize);
        DbResult SelWinLossDetailByAgent(int custid, string param, int pageIndex, int pageSize);
    }
}
