using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using DotNet.IoC;
using DotNet.WebSite.MVC;
using Microsoft.Practices.ServiceLocation;
using Ninject;

namespace DotNet.WebSite.Infrastructure.Ioc
{
    public class NinjectBoottrapper : CommonBootStrapper
    {
        public override IServiceLocator CreateServiceLocator()
        {
            return new NinjectServiceLocator(Kernel);
        }

        public IKernel Kernel
        {
            get
            {
                var settings = new NinjectSettings { LoadExtensions = false, InjectAttribute = typeof(NinjectServiceAttribute) };
                return new StandardKernel(settings,new RegisterServiceModule());
            }
        }
    }

    internal class RegisterServiceModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            // bind for mvc
            this.Bind<IDependencyResolver>().To<NinjectDependencyResolver>();

            this.Bind<IControllerFactory>().To<NinjectControllerFactory>();

            //业务接口注入点
            //this.Bind<IUserService>().To<UserService>();
        }
    }
}
