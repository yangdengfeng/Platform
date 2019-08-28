using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class TotalStatisViewModel
    {
        public string Day { get; set; }
        public string Week { get; set; }
        public string Month { get; set; }
        public string Quarter { get; set; }
        public string Year { get; set; }
    }

    public class TotalStatisSearchModel
    {
        public DateTime? StartDt { get; set; }
        public DateTime? EndDt { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
        public string Type { get; set; }
        public string DtType { get; set; }
        public string status { get; set; }
        public string CheckInstID { get; set; }
        public string SearchType { get; set; }
    }
}