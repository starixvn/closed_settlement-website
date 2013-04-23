using System.Collections.Generic;
using System.ServiceModel.Activation;
using Sunplus.Accounting.Business.BootStrappers;
using Sunplus.Accounting.Business.Settle;
using Sunplus.Accounting.Entities.Settle;
using Sunplus.Accounting.Entities;
using Sunplus.Accounting.Core.Common;

namespace Sunplus.Accounting.WcfServiceLibrary.Settle
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SettlementService:ISettlementService
    {
        #region ISettlementService Members

        public SettleLogResult SelSettleLogByMachCode(string matchCode, string fDate, string tDate)
        {
            var settle = CommonBootStrapper.Locator.GetInstance<IViewLogManager>();
            var result = settle.SelSettleLogByMachCode(matchCode, fDate, tDate);
            return result;
        }

        #endregion


        public Result UdpMatch_Pending(string matchid, bool ispending)
        {
            var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
            return settle.UdpMatch_Pending(matchid, ispending);
        }

        public string ReCalcNumberGame(int matchid, int firstBallvalue, int secondBallValue, int thirdBallValue)
        {
            var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
            var ip = Sunplus.Accounting.Core.Helper.Utils.GetClientIP();
            return settle.ReCalcNumberGame(matchid, firstBallvalue, secondBallValue, thirdBallValue, ip);
        }

        public List<NumberGameBalls> GetNumberGameBalls(int matchid)
        {
            var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
            return settle.GetNumberGameBalls(matchid);
        }

        public void UpdBallNoByMatchID(int matchid, string ballNo)
        {
            var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
            var ip = Sunplus.Accounting.Core.Helper.Utils.GetClientIP();
            settle.UpdBallNoByMatchID(matchid, ballNo, "01", AppSession.AccId, AppSession.AccUserName, ip);
        }

        public string RefundRunningBetDaqTickets(int Matchid)
        {
            var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
            var ip = Sunplus.Accounting.Core.Helper.Utils.GetClientIP();
            return settle.RefundRunningBetDaqTickets(Matchid, AppSession.AccId, ip);
        }


        public string SetMatchIsUndecided(int matchId, bool isUndecided, int userId)
        {
            var settle = CommonBootStrapper.Locator.GetInstance<ISettlement>();
            settle.SetMatchIsUndecided(matchId, isUndecided, userId);
            return string.Empty;
        }
    }
}
