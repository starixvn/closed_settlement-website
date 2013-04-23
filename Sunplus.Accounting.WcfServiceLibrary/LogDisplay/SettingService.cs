using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.LogDisplay;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.LogDisplay;

namespace Sunplus.Accounting.WcfServiceLibrary.LogDisplay
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SettingService : ISettingService
    {
        public Sunplus.Accounting.Entities.LogDisplay.ResultLog GetLogSetting(int custId, string custName, DateTime fromDate, DateTime toDate, int viewBy, int page, int pageSize)
        {
            var result = new ResultLog();
            var service = CommonBootStrapper.Locator.GetInstance<ISettingManager>();
            var logParam = new LogParam() { 
                CustId = custId, 
                CustName = custName,
                FromDate = fromDate, 
                ToDate= toDate,
                ViewBy = viewBy, 
                Page = page,
                PageSize = pageSize,
                Type = (int)ELogType.Setting
            };
            result = service.GetLogDisplay(logParam);
            //result = service.GetLogSetting(custId, custName, fromDate, toDate,  viewBy, page, pageSize);
            return result;
        }

        public Sunplus.Accounting.Entities.LogDisplay.ResultLog GetLogStatus(int custId, string custName, DateTime fromDate, DateTime toDate, int page, int pageSize)
        {
            var result = new ResultLog();
            var service = CommonBootStrapper.Locator.GetInstance<ISettingManager>();
            var logParam = new LogParam()
            {
                CustId = custId,
                CustName = custName,
                FromDate = fromDate,
                ToDate = toDate,
                Page = page,
                PageSize = pageSize,
                Type = (int)ELogType.Status
            };
            result = service.GetLogDisplay(logParam);
            // result = service.GetLogStatus(custId, custName, fromDate, toDate, page, pageSize);
            return result;
        }

        public Sunplus.Accounting.Entities.LogDisplay.ResultLog GetLogLogin(int custId, string custName, DateTime fromDate, DateTime toDate, int page, int pageSize)
        {
            var result = new ResultLog();
            var service = CommonBootStrapper.Locator.GetInstance<ISettingManager>();
            var logParam = new LogParam()
            {
                CustId = custId,
                CustName = custName,
                FromDate = fromDate,
                ToDate = toDate,
                Page = page,
                PageSize = pageSize,
                Type = (int)ELogType.Login
            };
            result = service.GetLogDisplay(logParam);
            //var result = service.GetLogLogin(custId, custName, fromDate, toDate, page, pageSize);
            return result;
        }

        public Sunplus.Accounting.Entities.LogDisplay.ResultLog GetLogCredit(int custId, string custName, DateTime fromDate, DateTime toDate, int page, int pageSize)
        {
            var result = new ResultLog();
            var service = CommonBootStrapper.Locator.GetInstance<ISettingManager>();
            var logParam = new LogParam()
            {
                CustId = custId,
                CustName = custName,
                FromDate = fromDate,
                ToDate = toDate,
                Page = page,
                PageSize = pageSize,
                Type = (int)ELogType.Credit
            };
            result = service.GetLogDisplay(logParam);
            //result = service.GetLogCredit(custId, custName, fromDate, toDate, page, pageSize);
            return result;
        }
    }
}
