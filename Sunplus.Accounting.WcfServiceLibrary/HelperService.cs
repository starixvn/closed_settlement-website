using System;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using System.Web;
using Sunplus.Accounting.Business;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.ViewModel;
using Sunplus.Accounting.Core.Helper;
using Sunplus.Accounting.Core.Extensions;
using Sunplus.Accounting.Core.Common;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "HelperService" in code, svc and config file together.

namespace Sunplus.Accounting.WcfServiceLibrary
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class HelperService : IHelperService
    {
        #region IHelperService Members

        public void DoWork()
        {
        }

        public string[] GetStandardCurrencyList()
        {
            var helper = new Sunplus.Accounting.Business.Helper();
            var result = helper.GetStandardCurrencyList();
            return result;
        }

        public List<SystemBO> GetSystemList()
        {
            return Utils.GetSystemList() as List<SystemBO>;
        }

        public List<SiteDTO> GetSiteList()
        {
            var objSystem = SessionExtension.GetSession(HttpContext.Current.Session, "CurrentSystem");
            var listSite = new List<SiteDTO>();
            if (objSystem != null)
            {
                var helper = CommonBootStrapper.Locator.GetInstance<IHelper>();
                
                listSite = helper.GetSites((Enumerations.AccountingSystem) objSystem);
            }

            return listSite;
            
        }

        #endregion
    }
}