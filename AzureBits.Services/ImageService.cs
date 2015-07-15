using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using AzureBits.Core.Extensions;
using AzureBits.Core.Models;
using AzureBits.Core.Services;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureBits.Services
{
    public class ImageService : IImageService
    {
        private readonly string _imageRootPath;
        private readonly string _containerName;
        private readonly string _connectionString;

        public ImageService(string containerName, string imageRootPath, string connectionString)
        {
            _imageRootPath = imageRootPath;
            _containerName = containerName;
            _connectionString = connectionString;
        }

        public async Task<UploadedImage> CreateUploadedImage(HttpPostedFileBase file)
        {
            if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
            {
                byte[] fileBytes = new byte[file.ContentLength];
                await file.InputStream.ReadAsync(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                return new UploadedImage
                {
                    ContentType = file.ContentType,
                    Data = fileBytes,
                    Name = file.FileName,
                    Url = string.Format("{0}/{1}", _imageRootPath, file.FileName)
                };
            }
            return null;
        }

        public async Task AddImageToBlobStorageAsync(UploadedImage image)
        {
            //  get the container reference
            var container = GetImagesBlobContainer();

            // using the container reference, get a block blob reference and set its type
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(image.Name);
            blockBlob.Properties.ContentType = image.ContentType;

            // finally, upload the image into blob storage using the block blob reference
            var fileBytes = image.Data;
            await blockBlob.UploadFromByteArrayAsync(fileBytes, 0, fileBytes.Length);
        }

        public async Task AddBitmapToBlobStorageAsync(Bitmap bitmap, string name, string contentType)
        {
            //  get the container reference
            var container = GetImagesBlobContainer();

            // using the container reference, get a block blob reference and set its type
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
            blockBlob.Properties.ContentType = contentType;

            // finally, upload the image into blob storage using the block blob reference
            var fileBytes = bitmap.ToJpegFormatByteArray();
            await blockBlob.UploadFromByteArrayAsync(fileBytes, 0, fileBytes.Length);
        }

        private CloudBlobContainer GetImagesBlobContainer()
        {
            // use the connection string to get the storage account
            var storageAccount = CloudStorageAccount.Parse(_connectionString);

            // using the storage account, create the blob client
            var blobClient = storageAccount.CreateCloudBlobClient();

            // finally, using the blob client, get a reference to our container
            var container = blobClient.GetContainerReference(_containerName);

            // if we had not created the container in the portal, this would automatically create it for us at run time
            container.CreateIfNotExists();

            // by default, blobs are private and would require your access key to download.
            //   You can allow public access to the blobs by making the container public.   
            container.SetPermissions(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            return container;
        }

        
    }
}