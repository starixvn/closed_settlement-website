using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel.Activation;
using System.Text;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Transaction;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.ViewModel;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Business.Security;
using Sunplus.Accounting.Core.Web;
using Sunplus.Accounting.Core.Web.Controls;

//using Sunplus.Core.LoggingLib;

namespace Sunplus.Accounting.WcfServiceLibrary
{
    /// <summary>
    /// Summary description for DailyTransManager
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DailyTranService : IDailyTranService
    {
        public string GetDailyTransData(string date, string type, int status, string creator, string verifyBy, int pageIndex, int fromcache)
        {
            var dailyManager = CommonBootStrapper.Locator.GetInstance<IServiceDailyTran>();
            return dailyManager.GetDailyTransData(date, type, status, creator, verifyBy, pageIndex, fromcache);
          
        }

        public DailyTransactionDTO GetDailyAllTransData(string date, string type, string creator, int pageIndex, int pageSize, int fromcache)
        {
            var dailyList = new DailyTransactionDTO();
            var dailyManager = new DailyTransManager();
            dailyList =  dailyManager.GetDailyAllTransaction(date, type, creator, pageIndex, pageSize, fromcache);
            return dailyList;
        }

        public bool VerifyTransaction(string refNoList)
        {
            var dailyManager = new DailyTransManager();
            return dailyManager.VerifyTransaction(refNoList);
        }

        public bool UnVerifyTransaction(string refNo)
        {
            var dailyManager = new DailyTransManager();
            return dailyManager.UnVerifyTransaction(refNo);
        }

        public Dictionary<string, int> GetListTotalTransPerGroup(List<DailyTransaction> entity)
        {
            var userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
            return userService.GetListTotalTransPerGroup(entity);
        }



        /*public Dictionary<string, int> GetListTotalTransPerGroup(List<DailyTransaction> entity)
        {
            var userService = CommonBootStrapper.Locator.GetInstance<IUserService>();
            return userService.GetListTotalTransPerGroup(entity);
        }



        public void SaveFileExcelToDirectory(string date, string type, string creator, int totalRows)
        {
            var dailyManager = new DailyTransManager();
            dailyManager.SaveFiletoDirectory(date, type, creator, totalRows);
        }
        */
       
    }
}