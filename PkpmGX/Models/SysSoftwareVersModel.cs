using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class SysSoftwareVersModel
    {
    }

    public class SysSoftwareVersSearchModel
    {
        public string Name { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class SysSoftwareVersEditModel
    {
        public int id { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string FileVersion { get; set; }
        public string FileVersionDate { get; set; }
        public string EndDate { get; set; }
    }
}