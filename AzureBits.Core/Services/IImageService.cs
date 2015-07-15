using System.Drawing;
using System.Threading.Tasks;
using System.Web;
using AzureBits.Core.Models;

namespace AzureBits.Core.Services
{
    public interface IImageService
    {
        Task<UploadedImage> CreateUploadedImage(HttpPostedFileBase file);
        Task AddImageToBlobStorageAsync(UploadedImage image);
        Task AddBitmapToBlobStorageAsync(Bitmap bitmap, string name, string contentType);
    }
}