using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DotNet.CloudFarm.WebSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "WeiXin",
            url: "Weixin/{action}/{id}",
            defaults: new { controller = "WeiXin", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Default", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "Houtai",
                url: "Houtai/{action}/{id}",
                defaults: new { controller = "Houtai", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
