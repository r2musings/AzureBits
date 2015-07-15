﻿
using System;

namespace AzureBits.Core.Models
{
    [Serializable]
    public class Thumbnail
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
    }
}