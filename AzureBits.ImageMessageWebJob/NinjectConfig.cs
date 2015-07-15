using System.Configuration;
using AzureBits.Core.Services;
using AzureBits.Services;
using Ninject.Modules;

namespace AzureBits.ImageMessageWebJob
{
    public class NinjectConfig : NinjectModule
    {
        public override void Load()
        {
            Bind<IImageProcessor>().To<ImageProcessor>();

            Bind<IImageService>().To<ImageService>()
                .WithConstructorArgument("containerName", ConfigurationManager.AppSettings["ImagesContainer"])
                .WithConstructorArgument("imageRootPath", ConfigurationManager.AppSettings["ImageRootPath"])
                .WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings["BlobStorageConnectionString"].ConnectionString);

        }
    }
}
