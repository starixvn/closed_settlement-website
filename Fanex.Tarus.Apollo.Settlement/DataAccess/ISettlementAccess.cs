using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.Tarus.Apollo.Settlement.Entity;

namespace Fanex.Tarus.Apollo.Settlement.DataAccess
{
    public interface ISettlementAccess
    {
        DataTable SelectMatchForBill(string matchId);

        ActionResult Set_ConfirmedAction(string matchId, int action, int fth, int fta, int hth, int hta, string goalrecord, string betstatus);

        ActionResult Set_ConfirmedAction_Parlay(string matchId);
    }
}
