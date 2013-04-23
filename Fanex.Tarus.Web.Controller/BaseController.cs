using System;
using System.Web;
using System.Web.Mvc;

namespace Fanex.Tarus.Web.Controllers
{
    using System.Web.Security;

    using Business.Implement;
    using Business.Interface;
    using System.Web.Routing;

    using Sunplus.Accounting.Core.Common;
    using Sunplus.Accounting.Core.Configurations;

    public class BaseController : Controller
    {
        private readonly ICommonService _commonService;
        private readonly IPermissionService _permissionService;
        public BaseController() : this(null, null) { }
        public BaseController(ICommonService commonService, IPermissionService permissionService)
        {
            _commonService = commonService ?? new CommonService();
            _permissionService = permissionService ?? new PermissionService();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var downTime = _commonService.GetDowntimeInfo();
            //downTime.IsDowntime = true;

            if (downTime.IsDowntime)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Common",
                    action =
                "UnderMaintenance",
                    from = downTime.From,
                    to = downTime.To
                }));


            }

            else
            {
                if (!WebSettings.NotAuthenticateAction.Contains(filterContext.ActionDescriptor.ActionName.ToLower()))
                {

                    if (null == AppSession.PageAccess || null == AppSession.RoleAccess)
                    {
                        System.Web.HttpContext.Current.Session.Clear();
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName) { Expires = DateTime.Now.AddDays(-1) };
                        System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                        FormsAuthentication.SignOut();
                        filterContext.Result = ForceLogin();

                    }
                }
            }


            base.OnActionExecuting(filterContext);

        }

        private static void AccessDenied(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                controller = "Common",
                action = "AccessDenied"
            }));
        }

        protected ActionResult ForceLogin()
        {
            return new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        controller = "Account",
                        action = "SignIn"
                    }));
        }
    }
}
