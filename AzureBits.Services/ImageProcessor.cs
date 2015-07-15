using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using AzureBits.Core.Models;
using AzureBits.Core.Services;

namespace AzureBits.Services
{
    public class ImageProcessor : IImageProcessor
    {
        public async Task<Bitmap> CreateThumbnailFromOriginalAsync(Bitmap originalImage, Thumbnail thumbnail)
        {
            return await GetResizedImageAsync(originalImage, thumbnail.Width, thumbnail.Height);
        }

        private async Task<Bitmap> GetResizedImageAsync(Bitmap originalImage, int width, int height)
        {
            // Get the image's original width and height
            var originalWidth = originalImage.Width;
            var originalHeight = originalImage.Height;

            if (height > originalHeight)
            {
                height = originalHeight;
            }

            if (width > originalWidth)
            {
                width = originalWidth;
            }

            // To preserve the aspect ratio
            var ratioX = (float)width / (float)originalWidth;
            var ratioY = (float)height / (float)originalHeight;
            var ratio = Math.Min(ratioX, ratioY);

            // New width and height based on aspect ratio
            var newWidth = (int)(originalWidth * ratio);
            var newHeight = (int)(originalHeight * ratio);

            // Convert other formats (including CMYK) to RGB.
            var newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            // Draws the image in the specified size with quality mode set to HighQuality
            using (var graphics = Graphics.FromImage(newImage))
            using (var memoryStream = new MemoryStream())
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                newImage.Save(memoryStream, ImageFormat.Jpeg);
            }

            return newImage;
        }
    }
}
