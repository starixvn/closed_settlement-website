/// <summary>
/// Summary description for NumberGame
/// </summary>

using Sunplus.Accounting.Entities;
using System.Collections.Generic;

namespace Sunplus.Accounting.Business.Settlement
{

    public class NumberGame
    {
        public NumberGame()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public ErrorDetail Refund(int matchId, int userId)
        {
            return DataAccess.IBCDLL.Accounting.Settle.RefundNumberGame(matchId, userId);
        }

        public ErrorDetail RefundFt(int matchId, int userId)
        {
            return DataAccess.IBCDLL.Accounting.Settle.RefundNumberGameFt(matchId, userId);
        }

        public ErrorDetail PrivateMessage(int matchId, string action, int userId, Dictionary<string, string> messageSend)
        {
            return DataAccess.IBCDLL.Accounting.Settle.PrivateMessageRefundNumberGame(matchId, action, userId, messageSend);
        }
    }
}
