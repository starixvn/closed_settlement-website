using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Sunplus.Accounting.Entities.Report.NumberGame;

namespace Sunplus.Accounting.DataAccess.IReportDAO
{
    public interface INumberGameReportDAO
    {
        MonthlyWinloss MonthlyWinloss(int month, int year);

    }
}
