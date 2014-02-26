using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TextumReader.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Home",
                "{controller}/{action}",
                new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{category}",
                new { controller = "Material", action = "Index", category = UrlParameter.Optional }
            );
            routes.MapRoute(
                "DictRoute",
                "{controller}/{action}",
                new { controller = "Dictionary", action = "Index"}
            );
        }
    }
}