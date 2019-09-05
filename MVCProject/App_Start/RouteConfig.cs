using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
               name: "Admin",
               url: "admin/{id}",
               defaults: new { controller = "Admins", action = "Admin_Login", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "Student",
                url: "student/{id}",
                defaults: new { controller = "Students", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Faculty",
               url: "faculty/{id}",
               defaults: new { controller = "Faculties", action = "Login", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
