using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ElasticSearch
{
    [Serializable]
    public class es_t_pkpm_binaryReport
    {
        public string CUSTOMID { get; set; }
        public string REPORTNUM { get; set; }
        public string REPORTTYPE { get; set; }
        public int? ORDERID { get; set; }
        public int? STATUS { get; set; }
        public string REPORTPATH { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
        public string PKRCONTENTIDPATH { get; set; }
        public string SYSPRIMARYKEYS { get; set; }

    }
}
