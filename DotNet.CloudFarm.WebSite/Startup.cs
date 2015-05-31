using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DotNet.CloudFarm.WebSite.Startup))]
namespace DotNet.CloudFarm.WebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
