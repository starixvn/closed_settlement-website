using System;
using Sunplus.Accounting.Entities.LogDisplay;

namespace Sunplus.Accounting.Business.LogDisplay
{
    public interface ISettingManager
    {
        Sunplus.Accounting.Entities.LogDisplay.ResultLog GetLogDisplay(LogParam param);

        Sunplus.Accounting.Entities.LogDisplay.ResultSp GetLogSetting(int custId, string custName, DateTime fromDate, DateTime toDate, int viewBy, int page, int pageSize);

        Sunplus.Accounting.Entities.LogDisplay.ResultSp GetLogStatus(int custId, string custName, DateTime fromDate, DateTime toDate, int page, int pageSize);

        Sunplus.Accounting.Entities.LogDisplay.ResultLogin GetLogLogin(int custId, string custName, DateTime fromDate, DateTime toDate, int page, int pageSize);

        Sunplus.Accounting.Entities.LogDisplay.ResultCredit GetLogCredit(int custId, string custName, DateTime fromDate, DateTime toDate, int page, int pageSize);
    }
}
