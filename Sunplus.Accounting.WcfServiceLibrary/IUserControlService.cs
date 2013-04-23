using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities;

/// <summary>
/// Summary description for IUserControlService
/// </summary>

namespace Sunplus.Accounting.WcfServiceLibrary
{
    [ServiceContract]
    public interface IUserControlService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<FunctionBO> GetFunctionList();

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<UserControlBO> GetGroupPolicyList(int groupId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<UserControlBO> GetUserPolicyList(int userId);
    }
}