using System.Web.Mvc;
using System.Web.Routing;

namespace WingStudio
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Sorry",
               url: "Sorry",
               defaults: new { controller = "Home", action = "BrowserVersionLow" }
           );

            routes.MapRoute(
             name: "User",
             url: "User/{action}/{id}",
             defaults: new { controller = "User", action = "Index", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "Admin",
              url: "Admin/{action}/{id}",
              defaults: new { controller = "Admin", action = "Dashboard", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "SuperAdmin",
              url: "SuperAdmin/{action}/{id}",
              defaults: new { controller = "SuperAdmin", action = "ManageUser", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "Home",
              url: "{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
