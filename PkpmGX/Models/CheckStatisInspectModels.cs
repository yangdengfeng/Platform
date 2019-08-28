using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class CheckStatisInspectSearchModel
    {
        public string projectName { get; set; }
        public string ItemCNName { get; set; }
        public string ReportNum { get; set; }
        public string SAMPLENUM { get; set; }
        public string ENTRUSTNUM { get; set; }
        public string Area { get; set; }
        public string Acs { get; set; }
        public string Change { get; set; }
        public string Type { get; set; }
        public string DtType { get; set; }
        public string CheckStatus { get; set; }
        public DateTime? StartDt { get; set; }
        public DateTime? EndDt { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }
}