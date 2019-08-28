using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ElasticSearch
{
    [Serializable]
    public class es_t_bp_wordreport
    {
        public string CUSTOMID { get; set; }
        public string ITEMTABLENAME { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public string REPORTTYPES { get; set; }
        public string WORDREPORTPATH { get; set; }
        public DateTime? PRINTDATE { get; set; }
        public string SIGNIMAGEPATH { get; set; }
        public int? MODIFIED { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }

      
    }

    [Serializable]
    public class es_extReportMange
    {
        public string CUSTOMID { get; set; }
        public string IDENTKEY { get; set; }
        public string ITEMTABLENAME { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public string REPORTNUM { get; set; }
        public string FILETYPE { get; set; }
        public string REPORTNAME { get; set; }
        public string WORDREPORTPATH { get; set; }

        public DateTime? PRINTDATE { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }

       
    }

}
