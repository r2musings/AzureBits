using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using AzureBits.Core.Models;
using AzureBits.Core.Services;
using Microsoft.Azure.WebJobs;
using Ninject;

namespace AzureBits.ImageMessageWebJob
{
    public class Functions
    {
        private static readonly IKernel _kernel = new StandardKernel();
        
        static Functions()
        {
            _kernel.Load(Assembly.GetExecutingAssembly());
        }

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        // 
        public async static void ProcessQueueMessageAsync([QueueTrigger("images")] UploadedImage uploadedImage, 
            [Blob("images/{Name}", FileAccess.Read)] Stream originalImageStream, 
            TextWriter log)
        {
            IImageProcessor _imageProcessor =_kernel.Get<IImageProcessor>();
            IImageService _imageService = _kernel.Get<IImageService>();
            
            if (originalImageStream != null && originalImageStream.Length > 0)
            {
                var originalImage = new Bitmap(originalImageStream);
                for (var i=0; i < uploadedImage.Thumbnails.Count; i++)
                {
                    var thumbnail = uploadedImage.Thumbnails[i];
                    var bitmap = await _imageProcessor.CreateThumbnailFromOriginalAsync(originalImage, thumbnail);
                    var name = GetThumbnailName(uploadedImage.Name, i);
                    await _imageService.AddBitmapToBlobStorageAsync(bitmap, name, uploadedImage.ContentType);
                }
            }
            
            Console.WriteLine(uploadedImage.Url);
        }

        private static string GetThumbnailName(string originalImageName, int index)
        {
            var lastPeriodIndex = originalImageName.LastIndexOf(".", StringComparison.CurrentCultureIgnoreCase);
            var prefix = originalImageName.Substring(0, lastPeriodIndex);
            var suffix = originalImageName.Substring(lastPeriodIndex + 1);
            return string.Format("{0}_tn{1:D3}.{2}", prefix, index+1, suffix);

        }


    }
}