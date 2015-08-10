using AzureBits.Core.Models;
using AzureBits.Core.Services;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AzureBits.Web.App_Start.NinjectConfig), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(AzureBits.Web.App_Start.NinjectConfig), "Stop")]

namespace AzureBits.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using AzureBits.Services;
    using System.Configuration;


    public static class NinjectConfig
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
            kernel.Bind<IImageService>().To<ImageService>()
                .WithConstructorArgument("containerName", ConfigurationManager.AppSettings["ImagesContainer"])
                .WithConstructorArgument("imageRootPath", ConfigurationManager.AppSettings["ImageRootPath"])
                .WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString);

            kernel.Bind<IQueueService<UploadedImage>>().To<QueueService<UploadedImage>>()
                .WithConstructorArgument("queueName", ConfigurationManager.AppSettings["ImagesQueue"])
                .WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString);
        }
    }
}