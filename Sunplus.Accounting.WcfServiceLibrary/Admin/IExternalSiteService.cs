using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Admin;
using Sunplus.Accounting.Entities.Customer;

namespace Sunplus.Accounting.WcfServiceLibrary.Admin
{
     [ServiceContract]
    public interface IExternalSiteService
    {
         [OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         List<string> GetGroupName();

         [OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         List<CustDetail> GetInfoCusts(string groupname, string username);

         [OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         ExternalSiteGridBO PagerCust(string groupName, int pageIndex, int pageSize, bool fromcache, string username);

         [OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         Result DeleteCustomer(int custId);

         [OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         Result AddCustomer(string username, string groupName);

         /*[OperationContract]
         [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
         ExternalSiteGridBO SearchCust(string username, string groupname, int pageSize, int pageIndex);*/
    }
}