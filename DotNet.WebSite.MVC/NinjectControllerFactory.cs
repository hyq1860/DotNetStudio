using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using DotNet.IoC;

namespace DotNet.WebSite.MVC
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        //  通过重写此方法实现用Ninject提供接口的实例
        protected override IController GetControllerInstance(RequestContext requestContext,
        Type controllerType)
        {
            return controllerType == null
                ? null
                : (IController)CommonBootStrapper.ServiceLocator.GetInstance(controllerType);
        }
    }
}
