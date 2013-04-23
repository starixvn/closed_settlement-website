

namespace Fanex.Tarus.Web.Controllers.Filters
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using Fanex.Tarus.Business.Implement;
    using Fanex.Tarus.Business.Interface;

    using Sunplus.Accounting.Core.Common;

    public class AuthenticateHost: ActionFilterAttribute
    {
        private readonly IPermissionService _permissionService;
        public AuthenticateHost():this(null){}
        public AuthenticateHost(IPermissionService permissionService)
        {
            _permissionService = permissionService ?? new PermissionService();
        }
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var actionName = filterContext.ActionDescriptor.ActionName;
            var controlName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            var pathActionName = controlName + "/" + actionName;

            bool hasPermissionHost = _permissionService.GetHostPermission(pathActionName, AppSession.PageAccess);

            if (!hasPermissionHost)
            {
                UpdateActionDenied(filterContext);
                return;
            }
            base.OnActionExecuting(filterContext);
        }

        

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new NotImplementedException();
        }


        private static void UpdateActionDenied(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                controller = "Common",
                action = "UpdateActionDenied"
            }));
        }
    }
}