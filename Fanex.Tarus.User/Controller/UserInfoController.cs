using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fanex.Tarus.User.Business.Services;
using Fanex.Tarus.User.Business.Interface;
using Fanex.Tarus.Entities;
using Fanex.Tarus.User.Entities;
using Fanex.Tarus.Web.Controllers;

namespace Fanex.Tarus.User.Controller
{
    public class UserInfoController : BaseController
    {
        private readonly IUserInfoService _userInfoService;
        private readonly IUserLogService _userLogService;

        public UserInfoController()
        {
            _userInfoService = new UserInfoService();
            _userLogService = new UserLogService();
        }

        public ActionResult Index()
        {
            var viewData = new AccManagementViewData { Departments = _userInfoService.GetAllDept(), Accounts = _userInfoService.GetAllAccount() };
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

        public JsonResult UpdateGroupPolicy(string groupPolicies, string oldGroupPolicies, int groupId)
        {
            return Json(_userInfoService.UpdateGroupPolicy(groupPolicies, oldGroupPolicies, groupId), JsonRequestBehavior.AllowGet);
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

        public JsonResult UpdateAccount(List<PostParam> postParams)
        {
            return Json(_userInfoService.UpdateAccount(postParams), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangeAccountStatus(int accId, bool closed)
        {
            return Json(_userInfoService.ChangeAccountStatus(accId, closed), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserPolicyList(int userId)
        {
            return Json(_userInfoService.GetUserPolicyList(userId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateAdminPolicy(string memberPolicies, string oldMemberPolicies, int custId)
        {
            return Json(_userInfoService.UpdateAdminPolicy(memberPolicies, oldMemberPolicies, custId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserControlLogSelect(DateTime fromDate, DateTime toDate, int type, string userName, int pageIndex, int pageSize)
        {
            return Json(_userLogService.UserControlLogSelect(fromDate, toDate, type, userName, pageIndex, pageSize), JsonRequestBehavior.AllowGet);
        }
    }
}
