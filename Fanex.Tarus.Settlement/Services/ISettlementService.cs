using System.Collections.Generic;
using System.ServiceModel;
using Fanex.Tarus.Settlement.Entity;
using System.ServiceModel.Web;

namespace Fanex.Tarus.Settlement.Services
{
    [ServiceContract]
    public interface ISettlementService
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<League> GetLeagues(string fdate, string tdate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        List<League> GetMatches(string fdate, string tdate, string LeagueIds, int statusId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string GetMatchesHtml(string fdate, string tdate, string LeagueIds, int statusId);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string ReCalc(int MatchId, int IBCBetTypeID, string BankerCard, string PlayerCard, int AdminID, string AdminName, string ShowDate);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        string Refund(int MatchID, int AdminID, string AdminName, string ShowDate);
    }
}
