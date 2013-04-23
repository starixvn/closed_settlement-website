using System.ServiceModel;
using System.ServiceModel.Web;

/// <summary>
/// Summary description for IServiceCustomerList
/// </summary>
[ServiceContract]
public interface IServiceSuspend
{
    [OperationContract]
    [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
    string GenSuspendListContent(string searchText, string selectAlpha, int PageIndex);
}