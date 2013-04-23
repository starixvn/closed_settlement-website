using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.ViewModel;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IHelperService" in both code and config file together.

namespace Sunplus.Accounting.WcfServiceLibrary
{
    [ServiceContract]
    public interface IHelperService
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        string[] GetStandardCurrencyList();

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<SystemBO> GetSystemList();

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<SiteDTO> GetSiteList();
    }
    }