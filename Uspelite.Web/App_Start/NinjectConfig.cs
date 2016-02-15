[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Uspelite.Web.NinjectConfig), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Uspelite.Web.NinjectConfig), "Stop")]

namespace Uspelite.Web
{
    using System;
    using System.Web;
    using Data;
    using Data.Repositories;
    using Infrastructure;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using Ninject.Web.Common;
    using Services.Common;
    using Services.Common.Contracts;
    using Constants = Common.Constants;

    public static class NinjectConfig
    {
        public static Action<IKernel> RegisterDependencies = kernel =>
        {
            kernel.Bind(typeof(IRepository<>)).To(typeof(GenericRepository<>));
            kernel.Bind(typeof(IUspeliteDbContext)).To(typeof(UspeliteDbContext)).InRequestScope();
        };

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
                ObjectFactory.InitializeKernel(kernel);
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
            RegisterDependencies(kernel);

            kernel.Bind(x => x
                .From(Constants.ServicesDataAssembly)
                .SelectAllClasses()
                .BindDefaultInterface());

            kernel.Bind(x => x
                .From(Constants.ServicesWebAssembly)
                .SelectAllClasses()
                .BindDefaultInterface());

            kernel.Bind<IImageHelper>().To<ImageHelper>();

        }
    }
}
