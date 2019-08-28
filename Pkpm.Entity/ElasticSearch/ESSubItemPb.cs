using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ElasticSearch
{
    [Serializable]
    public class es_sub_item_pb
    {
        public string SYSPRIMARYKEY { get; set; }
        public string SUBITEMCOLUMN { get; set; }
        public string SUBITEMNAME { get; set; }
        public string PDJG { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
    }
}
