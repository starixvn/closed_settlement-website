using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using Sunplus.Accounting.Business.Admin;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Admin;
using Sunplus.Accounting.Entities.Customer;

namespace Sunplus.Accounting.WcfServiceLibrary.Admin
{
     [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ExternalSiteService : IExternalSiteService
    {
        public List<string> GetGroupName()
        {
           
            var listGroupName =  new List<string>();
            var external = CommonBootStrapper.Locator.GetInstance<IExternalSite>();
            listGroupName = external.GetGroupName();
            return listGroupName;
        }

         public List<CustDetail> GetInfoCusts(string groupname, string username)
         {
             var listCust = new List<CustDetail>();
             var external = CommonBootStrapper.Locator.GetInstance<IExternalSite>();
             listCust = external.GetInfoCusts(groupname, username);
             return listCust;
         }

         public ExternalSiteGridBO PagerCust(string groupName, int pageIndex, int pageSize, bool fromcache, string username)
         {
             var externalsitegrid = new ExternalSiteGridBO();
             var external = CommonBootStrapper.Locator.GetInstance<IExternalSite>();
             externalsitegrid = external.PagerCustomers(groupName, pageIndex, pageSize, fromcache, username);
             return externalsitegrid;
         }


         public Result DeleteCustomer(int custId)
         {
             var external = CommonBootStrapper.Locator.GetInstance<IExternalSite>();
             Result result = external.DeleteCustGroupName(custId);
             return result;
         }

         public Result AddCustomer(string username, string groupName)
         {
             var external = CommonBootStrapper.Locator.GetInstance<IExternalSite>();
             Result result = external.AddCustGroupName(username, groupName);
             return result;
         }



       /*  public ExternalSiteGridBO SearchCust(string username, string groupName, int pageSize, int pageIndex)
         {
             var external = CommonBootStrapper.Locator.GetInstance<IExternalSite>();
             var listCustomer = external.SearchCustomer(username, groupName, pageSize, pageIndex);
             return listCustomer;
         }*/
    }
}
