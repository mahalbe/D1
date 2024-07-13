using System.Web.Mvc;
using System.Web.Routing;

namespace ISas.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("Content/{*relpath}");
            //routes.RouteExistingFiles = true;
            //routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                //url: "{action}/{id}",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            //defaults: new { controller = "Testing", action = "TestAPI", id = UrlParameter.Optional }
            );
        }
    }
}