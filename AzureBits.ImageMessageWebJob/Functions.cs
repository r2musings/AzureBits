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
        private static IKernel kernel = new StandardKernel();

        static Functions()
        {
            kernel.Load(Assembly.GetExecutingAssembly());
        }

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        // 
        public async static void ProcessQueueMessageAsync([QueueTrigger("images")] UploadedImage uploadedImage, 
            [Blob("images/{Name}", FileAccess.Read)] Stream originalImageStream, 
            TextWriter log)
        {
             var _imageProcessor = kernel.Get<IImageProcessor>();

            if (originalImageStream != null && originalImageStream.Length > 0)
            {
                var originalImage = new Bitmap(originalImageStream);
                foreach (var thumbnail in uploadedImage.Thumbnails)
                {
                    var newImage = await _imageProcessor.CreateThumbnailFromOriginalAsync(originalImage, thumbnail);
                }
            }
            
            Console.WriteLine(uploadedImage.Url);
        }
    }
}