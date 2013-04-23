namespace Fanex.Tarus.Web.Controllers.Filters
{
    using System.Web.Mvc;

    using Sunplus.Accounting.Core.Extensions;

    public class LogExceptionFilter: FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            //throw new NotImplementedException();
            //LogExtensions.WriteLog(filterContext.Exception.StackTrace);
            LogExtensions.ExceptionHandler(filterContext.Exception);
            //return;

          
            //write log here
        }

        private static void UnhandleException(ExceptionContext filterContext)
        {
           
        }
    }
}