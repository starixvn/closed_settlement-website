

namespace Fanex.Tarus.Web.Controllers.Filters
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using Fanex.Tarus.Business.Implement;
    using Fanex.Tarus.Business.Interface;

    using Sunplus.Accounting.Core.Common;

    public class AuthenticateRead: FilterAttribute, IActionFilter
    {
        private readonly IPermissionService _permissionService;
        public AuthenticateRead():this(null){}
        public AuthenticateRead(IPermissionService permissionService)
        {
            _permissionService = permissionService ?? new PermissionService();
        }
        
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var actionName = filterContext.ActionDescriptor.ActionName;
            var controlName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            var pathActionName = controlName + "/" + actionName;

            bool hasPermissionRead = _permissionService.HasPermission(pathActionName, AppSession.PageAccess);

            if(!hasPermissionRead)
            {
                AccessDenied(filterContext);
                return;
            }
        }

        

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new NotImplementedException();
        }

        private static void AccessDenied(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            {
                controller = "Common",
                action = "AccessDenied"
            }));
        }
       
    }
}