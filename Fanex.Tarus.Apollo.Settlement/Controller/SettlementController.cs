using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Tarus.Web.Controllers;
using System.Web.Mvc;
using Fanex.Tarus.Apollo.Settlement.Business;

namespace Fanex.Tarus.Apollo.Settlement.Controller
{
    public class SettlementController : BaseController
    {
        ISettlementService _service;
        public SettlementController()
        {
            _service = new SettlementService();
        }

        [HttpPost]
        public JsonResult Set_ConfirmedAction_Parlay(string matchIds, int action)
        {
            var result = _service.Set_ConfirmedAction_Parlay(matchIds, action);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SetConfirmedAction_Single(string matchId, int action, int homeHT, int awayHT, int homeFT, int awayFT, string goalrecord, string betstatus)
        {
            var result = _service.Set_ConfirmedAction_Single(matchId, action, homeFT, awayFT, homeHT, awayHT, goalrecord, betstatus);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
