using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AzureBits.Core.Models
{
    [Serializable]
    public class UploadedImage
    {
        public UploadedImage()
        {
            // hard-coded to a single thumbnail at 200 x 300 for now
            Thumbnails = new List<Thumbnail> { new Thumbnail { Width = 200, Height = 300 } };
        }

        public string Name { get; set; }
        public string ContentType { get; set; }

        [JsonIgnore]
        public byte[] Data { get; set; }
        
        public string Url { get; set; }
        public List<Thumbnail> Thumbnails { get; set; }
        public bool DeleteOriginalAfterProcessing { get; set; }
    }
}