using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fanex.Tarus.Business.Interface;
using Fanex.Tarus.Business.Implement;
using Sunplus.Accounting.Core.Common;
using Sunplus.Accounting.Entities;

namespace Fanex.Tarus.Web.Controllers.Admin
{
    public class QuickLinkController : Controller
    {
        //
        // GET: /QuickLink/

        private static IAdminService _adminService;

        public QuickLinkController(IAdminService adminService)
        {
            _adminService = adminService ?? new AdminService();
        }

        public QuickLinkController():this(null)
        {
            
        }


        public ActionResult Index()
        {
            return View("~/Views/Admin/QuickLink.cshtml");
        }

        [HttpPost]
        public JsonResult GetSearchMenu(string keyWord, bool isSuggestion)
        {
            List<MenuBO> tempMenu = AppSession.UserMenu != null ? AppSession.UserMenu : new List<MenuBO>();
            List<MenuBO> tempReportMenu = AppSession.ReportMenu != null ? AppSession.ReportMenu : new List<MenuBO>();
            return Json(_adminService.GetSearchMenu(keyWord,isSuggestion,tempMenu,tempReportMenu));
        }
    }
}
