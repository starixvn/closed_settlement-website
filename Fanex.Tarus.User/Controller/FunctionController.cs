using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fanex.Tarus.Web.Controllers;
using Fanex.Tarus.User.Entities;
using Fanex.Tarus.User.Business.Interface;
using Fanex.Tarus.User.Business.Services;

namespace Fanex.Tarus.User.Controller
{
    public class FunctionController : BaseController
    {
        private readonly IFunctionService _functionService;

        public FunctionController()
        {
            _functionService = new FunctionService();
        }

        public ActionResult Index()
        {
            var viewData = new FuncManagementViewData
            {
                ParentFunctionList = _functionService.GetFunctionParentList("ACCOUNTING"),
                FunctionList = _functionService.GetAccountingFunctionList()
            };
            return View("~/Views/User/FunctionManagement/Index.cshtml", viewData);
        }

        public JsonResult ReloadViewModel()
        {
            var viewData = new FuncManagementViewData
            {
                ParentFunctionList = _functionService.GetFunctionParentList("ACCOUNTING"),
                FunctionList = _functionService.GetAccountingFunctionList()
            };
            return Json(viewData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParentFunction(string systemName)
        {
            return Json(_functionService.GetFunctionParentList(systemName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditFunction(int funcId, string name, string desc, string fileNames, string parent, string systemName)
        {
            return Json(_functionService.EditFunction(funcId, name, desc, fileNames, parent, systemName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddFunction(string name, string desc, string fileNames, string parent, string systemName)
        {
            return Json(_functionService.AddFunction(name, desc, fileNames, parent, systemName), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFunction(int funcId)
        {
            return Json(_functionService.DeleteFunction(funcId), JsonRequestBehavior.AllowGet);
        }
    }
}
