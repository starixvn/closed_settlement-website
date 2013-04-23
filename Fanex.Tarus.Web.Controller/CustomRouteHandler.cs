using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;
using System.Web.UI;

namespace Fanex.Tarus.Web.Controllers
{
    public class CustomRouteHandler: IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            throw new NotImplementedException();

        }
    }
}