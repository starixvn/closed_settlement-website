using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fanex.Tarus.Web.Models.User.AccountManagement;
using Fanex.Tarus.User.Business.Services;
using Fanex.Tarus.User.Business.Interface;

namespace Fanex.Tarus.Web.Controllers.User
{
    public class UserInfoController : BaseController
    {
        private readonly IUserInfoService _userInfoService;

        public UserInfoController()
        {
            _userInfoService = new UserInfoService();
        }

        public ActionResult Index()
        {
            var viewData = new ViewData { Departments = _userInfoService.GetAllDept(), Accounts = _userInfoService.GetAllAccount() };
            return View("~/Views/User/AccountManagement/Index.cshtml", viewData);
        }

        public JsonResult GetAllAccount()
        {
            return Json(_userInfoService.GetAllAccount(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGroupPolicyList(int groupId)
        {
            return Json(_userInfoService.GetGroupPolicyList(groupId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateGroupPolicy(string groupPolicies, int groupId)
        {
            return Json(_userInfoService.UpdateGroupPolicy(groupPolicies, groupId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateDept(int id, string name, string description)
        {
            return Json(_userInfoService.UpdateDept(id, name, description), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepts()
        {
            return Json(_userInfoService.GetAllDept(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteDept(int id)
        {
            return Json(_userInfoService.DeleteDept(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddDept(string name, string description)
        {
            return Json(_userInfoService.AddDept(name, description), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddAccount(string userName, string firstName, string lastName, int groupId, string password)
        {
            return Json(_userInfoService.AddAccount(userName, firstName, lastName, groupId, password), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddExistedAccount(string custName, int groupId)
        {
            return Json(_userInfoService.AddExistedAccount(custName, groupId), JsonRequestBehavior.AllowGet);
        }
    }
}
