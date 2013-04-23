using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities.ViewModel;
using System.Collections.Generic;

// NOTE: If you change the interface name "IServiceDailyTran" here, you must also update the reference to "IServiceDailyTran" in Web.config.
namespace Sunplus.Accounting.WcfServiceLibrary
{
    [ServiceContract]
    public interface IDailyTranService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetDailyTransData(string date, string type, int status, string creator, string verifyBy, int pageIndex, int fromcache );


        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        DailyTransactionDTO GetDailyAllTransData(string date, string type, string creator, int pageIndex, int pageSize, int fromcache);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        bool VerifyTransaction(string refNoList);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        bool UnVerifyTransaction(string refNo);

        /*[OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Dictionary<string, int> GetListTotalTransPerGroup(List<DailyTransaction> entity);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void SaveFileExcelToDirectory(string date, string type, string creator, int totalRows);*/
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Dictionary<string, int> GetListTotalTransPerGroup(List<DailyTransaction> entity);

    }
}