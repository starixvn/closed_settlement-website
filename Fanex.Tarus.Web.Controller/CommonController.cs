namespace Fanex.Tarus.Web.Controllers
{
    using System.Web.Mvc;

    using Fanex.Tarus.Business.Implement;
    using Fanex.Tarus.Business.Interface;
    using Fanex.Tarus.Web.Models;

    using Sunplus.Accounting.Core.Constant;
    using Sunplus.Accounting.Entities;

    public class CommonController:Controller
    {
        private readonly ICommonService _commonService;

        public CommonController():this(null){}
        public CommonController(ICommonService commonService)
        {
            _commonService = commonService ?? new CommonService();
        }


        public ActionResult AccessDenied()
        {
            return View();
        }

        public JsonResult UpdateActionDenied()
        {
            Result result = new Result
                                {
                                    ErrorCode = ErrorCode.ERR_UPDATE_DENIED,
                                    ErrorMessage = Resources.ErrorMsg.errUpdateDenied
                                };
           
           // var jsonResult = Json(result);
           // jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            //return Json(jsonResult, JsonRequestBehavior.AllowGet);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult UnderMaintenance(string from, string to)
        {
            return PartialView(new UnderMaintenanceModel {From = from, To = to});
        }

        public JsonResult UnhandleException()
        {
            return null;
        }
    }
}