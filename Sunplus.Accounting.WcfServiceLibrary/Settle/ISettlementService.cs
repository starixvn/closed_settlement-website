using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Entities.Settle;

namespace Sunplus.Accounting.WcfServiceLibrary.Settle
{
    [ServiceContract]
    public interface ISettlementService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        SettleLogResult SelSettleLogByMachCode(string matchCode, string fDate, string tDate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Result UdpMatch_Pending(string matchid, bool ispending);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string ReCalcNumberGame(int matchid, int firstBallvalue, int secondBallValue, int thirdBallValue);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<NumberGameBalls> GetNumberGameBalls(int matchid);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        void UpdBallNoByMatchID(int matchid, string ballNo);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string RefundRunningBetDaqTickets(int Matchid);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string SetMatchIsUndecided(int matchId, bool isUndecided, int userId);
    }
}
