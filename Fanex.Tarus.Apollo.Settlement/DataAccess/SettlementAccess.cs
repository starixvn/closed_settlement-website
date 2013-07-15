using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Sunplus.Accounting.Core.Extensions;
using Fanex.Data;
using System.Collections;
using Fanex.Tarus.Apollo.Settlement.Entity;

namespace Fanex.Tarus.Apollo.Settlement.DataAccess
{
    public class SettlementAccess : ISettlementAccess
    {

        public ActionResult Set_ConfirmedAction(string matchId, int action, int fth, int fta, int hth, int hta, string goalrecord, string betstatus)
        {
            ActionResult result = new ActionResult();
            try
            {
                bool isHTCal = action == 2;//Haft Time
                using (ObjectDb objDb = new ObjectDb("Apollo_Set_ConfirmedAction", 120))
                {
                    Hashtable p = new Hashtable();
                    p.Add("matchid", matchId);
                    p.Add("action", action);

                    if (!isHTCal)
                    {
                        p.Add("fth", fth);
                        p.Add("fta", fta);
                    }

                    p.Add("hth", hth);
                    p.Add("hta", hta);
                    p.Add("goalrecord", goalrecord);
                    p.Add("betstatus", betstatus);

                    objDb.AssignParamValues(p);
                    var rs = objDb.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                result.ErrCode = 100;
                result.ErrMsg = ex.Message;
            }
            return result;
        }




        public ActionResult Set_ConfirmedAction_Parlay(string matchId)
        {
            ActionResult result = new ActionResult();
            try
            {
                using (ObjectDb objDb = new ObjectDb("Apollo_Set_ConfirmedActionParlay", 120))
                {
                    Hashtable p = new Hashtable();
                    p.Add("matchids", matchId);

                    objDb.AssignParamValues(p);
                    var rs = objDb.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                result.ErrCode = 100;
                result.ErrMsg = ex.Message;
            }
            return result;
        }
    }
}
