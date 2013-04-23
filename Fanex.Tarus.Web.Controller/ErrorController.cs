using System.Web.Mvc;

namespace Fanex.Tarus.Web.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Index()
        {
            return View("~/Views/Shared/NoInfo.cshtml");
        }

    }
}
