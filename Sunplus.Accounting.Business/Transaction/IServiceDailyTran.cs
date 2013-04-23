using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities.ViewModel;

// NOTE: If you change the interface name "IServiceDailyTran" here, you must also update the reference to "IServiceDailyTran" in Web.config.
namespace Sunplus.Accounting.Business.Transaction
{
    [ServiceContract]
    public interface IServiceDailyTran
    {
        /*[OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetDailyTransData(int pageIndex, string date, string type, int status, string creator, string verifyBy);*/

        string GetDailyTransData(string date, string type, int status, string creator, string verifyBy, int pageIndex, int fromcache);

        DailyTransactionDTO GetDailyAllTransaction(string date, string type, string creator, int pageIndex, int pageSize, int fromcache);

        DailyTransactionDTO GetDailyTransData(string date, string type, int status, string creator, string verifyBy, int pageIndex, int pageSize, int fromcache);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        bool VerifyTransaction(string refNoList);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        bool UnVerifyTransaction(string refNo);


        void ExportToExcelforDailyAllTxn(string date, string type, string creator, int totalRows);

        void ExportToExcelDailyFinance(string date, int status, string type, string creator, string verifyBy);
        
    }
}