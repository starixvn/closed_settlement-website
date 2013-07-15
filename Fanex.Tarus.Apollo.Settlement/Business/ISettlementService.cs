using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Fanex.Tarus.Apollo.Settlement.Entity;

namespace Fanex.Tarus.Apollo.Settlement.Business
{
    public interface ISettlementService
    {
        DataTable SelectMatchForBill(string matchId);

        ActionResult Set_ConfirmedAction(string matchIds, int action);

        ActionResult Set_ConfirmedAction_Single(string matchId, int action, int fth, int fta, int hth, int hta, string goalrecord, string betstatus);

        ActionResult Set_ConfirmedAction_Parlay(string matchIds);

        int ParseActionType(string type);
        
    }
}
