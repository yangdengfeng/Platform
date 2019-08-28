using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class CheckStatisSJKYViewModels
    {
    }

    public class CheckStatisSJKYSearchModel
    {
        public string CustomId { get; set; }
        public DateTime? ReportStartDate { get; set; }
        public DateTime? ReportEndDate { get; set; }
        public double? qiangduhigh { get; set; }
        public string yanghutiaojian { get; set; }
        public string qiangdudengji { get; set; }
        public string projectName { get; set; }
        public double? qiangdulow { get; set; }
        public string Area { get; set; }
        public int? qiangduwuxiao { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }
}