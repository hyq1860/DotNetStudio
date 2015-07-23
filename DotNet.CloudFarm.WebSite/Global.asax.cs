using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DotNet.IoC;
using DotNet.WebSite.Infrastructure.Ioc;

namespace DotNet.CloudFarm.WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // IOC的注入
            //var boottrapper = new NinjectBoottrapper();
            //BootStrapperManager.Initialize(boottrapper);

            //GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(boottrapper.Kernel);
            //DependencyResolver.SetResolver(CommonBootStrapper.GetInstance<IDependencyResolver>());

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();

        }

        protected void Application_Error()
        {
            Exception ex = Server.GetLastError().GetBaseException(); 
            var logger =log4net.LogManager.GetLogger("ApplicationError");
            logger.Error(ex);
        }
    }
}
