using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ElasticSearch
{
    [Serializable]
    public class es_t_bp_modify_log
    {
        public string CUSTOMID { get; set; }
        public string MODIFYPRIMARYKEY { get; set; }
        public string QUESTIONPRIMARYKEY { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public int? MODIFYTIMES { get; set; }
        public string SAMPLENUM { get; set; }
        public string REPORTNUM { get; set; }
        public string ENTRUSTNUM { get; set; }
        public string PROJECTNUM { get; set; }
        public string ITEMTABLENAME { get; set; }
        public string MODIFYMAN { get; set; }
        public string FIELDNAME { get; set; }
        public DateTime? MODIFYDATETIME { get; set; }
        public string BEFOREMODIFYVALUES { get; set; }
        public string AFTERMODIFYVALUES { get; set; }
        public string SENDFLAGS { get; set; }
        public string COMPUTERNAME { get; set; }
        public string MACADDRESS { get; set; }
        public int? ISUPLOADED { get; set; }
        public string PK { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
    }
}
