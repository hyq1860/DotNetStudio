using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NugetMvc5Site.Startup))]
namespace NugetMvc5Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
