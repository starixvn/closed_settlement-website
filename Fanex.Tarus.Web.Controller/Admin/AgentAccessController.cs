using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanex.Tarus.Web.Controllers.Admin
{
    using Fanex.Tarus.Business.Implement;
    using Fanex.Tarus.Business.Interface;
    using Fanex.Tarus.Web.Controllers.Filters;

    using Resources;

    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Extensions;
    using Sunplus.Accounting.Entities;
    

    public class AgentAccessController : BaseController
    {
        
        //
        // GET: /Admin/
        private readonly IAdminService _adminService;

        public AgentAccessController()
            : this(null)
        {
            
        }

        public AgentAccessController(IAdminService adminService)
        {
            _adminService = adminService ?? new AdminService();
        }

        [AuthenticateRead]
        public ActionResult AgentAccessList()
        {

            return View("~/Views/Admin/GrantAccessToAgentSite.cshtml", _adminService.GetListAgentAcess());
        }

        [AuthenticateHost]
        public ActionResult AddUser(string userName, string remark)
        {
            var result = new Result();
            try
            {
                if (!string.IsNullOrWhiteSpace(userName)) userName = userName.Trim();
                result = _adminService.AddUser(userName, AppSession.AccId, remark);
                //return !string.IsNullOrWhiteSpace(result) ? Content(result) : Content(ErrorMsg.Success.ToLower());
            }
            catch (Exception exception)
            {

                LogExtensions.ExceptionHandler(exception);
                //return Content(ErrorMsg.SystemError);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ReloadData()
        {
            var entityList = _adminService.GetListAgentAcess();
            //Purpose: for filtering on webpage. Must be seperated to 2 array
            return Json(new
                {
                    vm1 = entityList,
                    vm2 = entityList
                }, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateHost]
        public ActionResult Update(int custId, bool isAllowed, string remark)
        {
            var result = new Result();
            try
            {
                 result = _adminService.Update(custId, isAllowed, remark);
                //return !string.IsNullOrWhiteSpace(result) ? Content(result) : Content("success");
            }
            catch (Exception exception)
            {

                LogExtensions.ExceptionHandler(exception);
                //return Content(ErrorMsg.SystemError);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AuthenticateHost]
        public ActionResult Delete(int custId)
        {
            var result = new Result();
            try
            {
                result = _adminService.Delete(custId);
                //return !string.IsNullOrWhiteSpace(result) ? Content(result) : Content("success");
            }
            catch (Exception exception)
            {

                LogExtensions.ExceptionHandler(exception);
                //return Content(ErrorMsg.SystemError);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
