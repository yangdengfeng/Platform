using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class CheckStatisHNKYSearchModel
    {
        public string CUSTOMID { get; set; }
        public string Area { get; set; }
        public string ProjectName { get; set; }
        public string SHEJIDENGJI { get; set; }
        public int? qiangduwuxiao { get; set; }
        public string BAIFENBIStart { get; set; }
        public string LINQIStart { get; set; }
        public string YANGHUTIAOJIAN { get; set; }
        public string BAIFENBIEnd { get; set; }
        public string LINQIEnd { get; set; }
        public DateTime? StartDt { get; set; }
        public DateTime? EndDt { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }
}