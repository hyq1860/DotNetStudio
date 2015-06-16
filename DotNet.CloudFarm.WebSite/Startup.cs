using System.Reflection;
using System.Web.Http;
using DotNet.CloudFarm.WebSite.App_Start;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;

[assembly: OwinStartupAttribute(typeof(DotNet.CloudFarm.WebSite.Startup))]
namespace DotNet.CloudFarm.WebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            //var webApiConfiguration = new HttpConfiguration();
            //webApiConfiguration.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional, controller = "values" });
            //app.UseNinjectMiddleware(NinjectWebCommon.CreateKernel).UseNinjectWebApi(webApiConfiguration);
        }
    }
}
