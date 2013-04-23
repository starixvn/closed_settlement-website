using System.ServiceModel;
using System.ServiceModel.Web;

// NOTE: If you change the interface name "IServiceCustomerInfo" here, you must also update the reference to "IServiceCustomerInfo" in Web.config.
namespace Sunplus.Accounting.Business
{
    [ServiceContract]
    public interface IServiceCustomerInfo
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GenDownlineList(int custId, int roleId, int pageIndex);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GenOnlineList(int custId);

        //[OperationContract]
        //[WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        //string GenMenuMinMaxPT(int custId, int roleId, string currency, string userName, int agentId, int masterId, int superId );
    }
}