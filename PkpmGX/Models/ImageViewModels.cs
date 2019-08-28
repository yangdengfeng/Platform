using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class ImageViewDownload
    {
        public string action { get; set; }
        public string itemId { get; set; }
        public string itemValue { get; set; }
        public string itemName { get; set; }
    }

    public class ImageViewUpload
    {
        public string action { get; set; }
        public string itemId { get; set; }
        public string itemValue { get; set; }
        public string itemName { get; set; }
        public int id { get; set; }
    }

    public class ImageViewUploadResult
    {
        public bool state { get; set; }
        public string itemId { get; set; }
        public string itemValue { get; set; }
    }
}