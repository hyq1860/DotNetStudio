using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Web;
using DotNet.IoC;
using DotNet.WebSite.Infrastructure.Ioc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace DotNet.CloudFarm.WebSite
{
    public partial class Startup
    {
        // 有关配置身份验证的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {

            //app.CreatePerOwinContext(CreateKernel);
            //app.UseNinjectMiddleware(CreateKernel);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            CookieAuthenticationProvider provider = new CookieAuthenticationProvider();

            var originalHandler = provider.OnApplyRedirect;
            provider.OnApplyRedirect = context =>
            {
                //insert your logic here to generate the redirection URI
                string NewURI = "....";
                //Overwrite the redirection uri
                context.RedirectUri = NewURI;
                originalHandler.Invoke(context);
            };

            // 使应用程序可以使用 Cookie 来存储已登录用户的信息
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                CookieHttpOnly = true,

                Provider = new CookieAuthenticationProvider
                {
                    
                    OnApplyRedirect = context =>
                    {
                        
                        File.WriteAllText("C:\\1.txt",DateTime.Now.ToString());
                        context.Response.Redirect(context.RedirectUri);
                    }
                }
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // 取消注释以下行可允许使用第三方登录提供程序登录
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication();
        }

        private static void ApplyRedirect(CookieApplyRedirectContext context)
        {
            Uri absoluteUri;
            if (Uri.TryCreate(context.RedirectUri, UriKind.Absolute, out absoluteUri))
            {
                var path = PathString.FromUriComponent(absoluteUri);
                if (path == context.OwinContext.Request.PathBase + context.Options.LoginPath)
                    context.RedirectUri = "http://accounts.domain.com/login" +
                        new QueryString(
                            context.Options.ReturnUrlParameter,
                            context.Request.Uri.AbsoluteUri);
            }

            context.Response.Redirect(context.RedirectUri);
        }
    }
}