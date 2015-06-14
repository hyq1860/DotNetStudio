using DotNet.CloudFarm.Domain.Contract.Order;
using DotNet.CloudFarm.Domain.Contract.Product;
using DotNet.CloudFarm.Domain.Contract.User;
using DotNet.CloudFarm.Domain.DTO.Order;
using DotNet.CloudFarm.Domain.DTO.Product;
using DotNet.CloudFarm.Domain.DTO.User;
using DotNet.CloudFarm.Domain.Impl.Order;
using DotNet.CloudFarm.Domain.Impl.Product;
using DotNet.CloudFarm.Domain.Impl.User;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(DotNet.CloudFarm.WebSite.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(DotNet.CloudFarm.WebSite.App_Start.NinjectWebCommon), "Stop")]

namespace DotNet.CloudFarm.WebSite.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //业务接口注入点

            //用户服务
            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IUserDataAccess>().To<UserDataAccess>();

            //商品服务
            kernel.Bind<IProductService>().To<ProductService>();
            kernel.Bind<IProductDataAccess>().To<ProductDataAccess>();

            //订单服务
            kernel.Bind<IOrderService>().To<OrderService>();
            kernel.Bind<IOrderDataAccess>().To<OrderDataAccess>();

        }        
    }
}
