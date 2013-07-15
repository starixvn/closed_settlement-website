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
        public JsonResult SetConfirmedAction(string matchIds, int action)
        {
            var result = _service.Set_ConfirmedAction(matchIds, action);
            return Json(result, JsonRequestBehavior.AllowGet); 
        }
    }
}
