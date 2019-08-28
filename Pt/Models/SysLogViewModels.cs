using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class SysLogViewModels
    {
        public string LogUserName { get; set; }
        public string LogType { get; set; }
        public DateTime? LogStartDt { get; set; }
        public DateTime? LogEndDt { get; set; }

        public int? posStart { get; set; }
        public int? count { get; set; }
    }
}