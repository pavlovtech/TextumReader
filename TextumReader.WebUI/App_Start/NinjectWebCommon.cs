using System.Configuration;
using System.Data.Entity;
using Linguistics.Dictionary;
using TextumReader.DataLayer.Abstract;
using TextumReader.DataLayer.Concrete;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TextumReader.WebUI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(TextumReader.WebUI.App_Start.NinjectWebCommon), "Stop")]

namespace TextumReader.WebUI.App_Start
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
            var connectionString = ConfigurationManager.ConnectionStrings["TextumReader.TextumReaderDbContext"].ConnectionString;

            kernel.Bind<DbContext>().To<TextumReaderDbContext>().WithConstructorArgument("connectionString", connectionString);
            kernel.Bind<IGenericRepository>().To<TextumReaderRepository>();
            kernel.Bind<IWebDictionary>().To<GoogleTranslate>();
        }        
    }
}
