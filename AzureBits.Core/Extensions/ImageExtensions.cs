using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AzureBits.Core.Extensions
{
    public static class ImageExtensions
    {
        public static byte[] ToJpegFormatByteArray(this Image value)
        {
            using (var memoryStream = new MemoryStream())
            {
                value.Save(memoryStream, ImageFormat.Jpeg);
                return memoryStream.ToArray();
            }
        }
    }
}
