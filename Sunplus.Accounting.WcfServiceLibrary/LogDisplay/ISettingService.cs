using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.LogDisplay;

namespace Sunplus.Accounting.WcfServiceLibrary.LogDisplay
{
    [ServiceContract]
    public interface ISettingService
    {
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Sunplus.Accounting.Entities.LogDisplay.ResultLog GetLogSetting(int custId, string custName, DateTime fromDate, DateTime toDate, int viewBy, int page, int pageSize);

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Sunplus.Accounting.Entities.LogDisplay.ResultLog GetLogStatus(int custId, string custName, DateTime fromDate, DateTime toDate, int page, int pageSize);

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Sunplus.Accounting.Entities.LogDisplay.ResultLog GetLogLogin(int custId, string custName, DateTime fromDate, DateTime toDate, int page, int pageSize);


        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Sunplus.Accounting.Entities.LogDisplay.ResultLog GetLogCredit(int custId, string custName, DateTime fromDate, DateTime toDate, int page, int pageSize);
    }
}