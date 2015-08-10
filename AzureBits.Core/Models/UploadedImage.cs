using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzureBits.Core.Models
{
public class UploadedImage
{
    public UploadedImage()
    {
        Thumbnails = new List<Thumbnail>();
    }

    public string Name { get; set; }
    public string ContentType { get; set; }

    [JsonIgnore]
    public byte[] Data { get; set; }
        
    public string Url { get; set; }
    public List<Thumbnail> Thumbnails { get; set; }
}
}