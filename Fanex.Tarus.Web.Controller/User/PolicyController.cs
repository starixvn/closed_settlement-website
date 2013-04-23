using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fanex.Tarus.Web.Controllers.User
{
    using Fanex.Tarus.User.Business.Interface;
    using Fanex.Tarus.User.Business.Services;

    public class PolicyController : Controller
    {
        //
        // GET: /Policy/

        private IPolicyService _policyService;


        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService ?? new PolicyService();
        }


        public PolicyController():this(null)
        {
            
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
