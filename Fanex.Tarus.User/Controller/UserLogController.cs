using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fanex.Tarus.Web.Controllers;

namespace Fanex.Tarus.User.Controller
{
    public class UserLogController : BaseController
    {
        //
        // GET: /UserLog/

        public ActionResult Index()
        {
            return View("~/Views/User/UserControlLog/Index.cshtml");
        }

    }
}
