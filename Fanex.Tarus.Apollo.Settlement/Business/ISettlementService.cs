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
        ActionResult Set_ConfirmedAction_Parlay(string matchIds, int action);

        ActionResult Set_ConfirmedAction_Single(string matchId, int action, int fth, int fta, int hth, int hta, string goalrecord, string betstatus);
        
    }
}
