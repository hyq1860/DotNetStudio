using System.Web.Mvc;
using System.Web.Routing;

namespace DotNet.WebSite.MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "DotNet.WebSite.MVC.Controllers" }
                );

            routes.MapRoute(
                "Article",
                "{controller}/{action}/{id}.html",
                new { controller = "Content", action = "Article", id = UrlParameter.Optional },
                new {id = @"\d+"},
                new[] { "DotNet.WebSite.MVC.Controllers" }
                );

            routes.MapRoute(
                "Communicate",
                "{controller}/{action}/{pageIndex}",
                new { controller = "Communicate", action = "Index", pageIndex = UrlParameter.Optional },
                new { id = @"\d+" },
                new[] { "DotNet.WebSite.MVC.Controllers" }
                );

            routes.MapRoute(
                "BackstageCommunicate",
                "{controller}/{action}/{pageIndex}",
                new { controller = "Content", action = "CommunicateList", pageIndex = UrlParameter.Optional },
                new { id = @"\d+" },
                new[] { "DotNet.WebSite.MVC.Controllers.Backstage" }
                );
        }
    }
}