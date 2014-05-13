using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Sunplus.Accounting.Core.Extensions;
using Fanex.Tarus.Apollo.Settlement.DataAccess;
using Fanex.Tarus.Apollo.Settlement.Entity;
using System.Web;

namespace Fanex.Tarus.Apollo.Settlement.Business
{
    public class SettlementService : ISettlementService
    {
        ISettlementAccess _access;
        public SettlementService()
        {
            _access = new SettlementAccess();
        }




        public ActionResult Set_ConfirmedAction_Parlay(string matchIds, int action)
        {
            ActionResult result = new ActionResult();
            try
            {
                if (action == 5)//settle for parlay
                {
                    result = _access.Set_ConfirmedAction_Parlay(matchIds);
                    if (result.ErrCode == 0)
                    {
                        result.ErrMsg = "Set_ConfirmedActionParlay for matchid " + matchIds + " to apollo DB successfully.";
                    }
                }
                else//refund for parlay
                {
                    result = _access.Set_ConfirmedAction(matchIds, action, 0, 0, 0, 0, string.Empty, string.Empty);
                    if (result.ErrCode == 0)
                    {
                        result.ErrMsg = "Set_ConfirmedAction for matchid " + matchIds + " to apollo DB successfully.";
                    }
                }

            }
            catch (Exception ex)
            {
                result = new ActionResult()
                {
                    ErrCode = 100,
                    ErrMsg = ex.Message
                };

                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }



        public ActionResult Set_ConfirmedAction_Single(string matchId, int action, int fth, int fta, int hth, int hta, string goalrecord, string betstatus)
        {
            ActionResult result = new ActionResult();
            try
            {
                if (action == 13)//refund All
                {
                    betstatus = "Refund;";
                }

                result = _access.Set_ConfirmedAction(matchId, action, fth, fta, hth, hta, goalrecord, betstatus);
                if (result.ErrCode == 0)
                {
                    result.ErrMsg = "Set_ConfirmedAction for matchid " + matchId + " to apollo DB successfully.";
                }
            }
            catch (Exception ex)
            {
                result = new ActionResult()
                {
                    ErrCode = 100,
                    ErrMsg = ex.Message
                };

                LogExtensions.ExceptionHandler(ex);
            }
            return result;
        }

    }
}
