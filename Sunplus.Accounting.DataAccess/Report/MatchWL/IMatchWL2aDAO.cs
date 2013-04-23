using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DNAide.Data;

namespace Sunplus.Accounting.DataAccess.Report.MatchWL
{
    public interface IMatchWL2aDAO
    {
        DbResult MatchWL(DateTime wldate);
        DbResult MatchWL_WH(DateTime wldate);
        DbResult MatchWL14(DateTime wldate);
        DbResult MatchWLNEW(DateTime wldate);

        DbResult WinLossByBettype(DateTime wldate);
        DbResult WinLossByBettype_WH(DateTime wldate);
        DbResult WinLossByBettype14(DateTime wldate);
        DbResult WinLossByBettypeNEW(DateTime wldate);
    }
}
