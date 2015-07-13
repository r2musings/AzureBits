using System;
using System.Collections.Generic;

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

        // no need to serialize the file bytes of the image to the queue (will be larger than 65k max)
        [NonSerialized]
        private byte[] _data;
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public string Url { get; set; }
        public List<Thumbnail> Thumbnails { get; set; }
    }
}