using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ElasticSearch
{
    [Serializable]
    public class es_t_bp_question
    {
        public string CUSTOMID { get; set; }
        public string QUESTIONPRIMARYKEY { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public string ITEMTABLENAME { get; set; }
        public string SAMPLENUM { get; set; }
        public string RECORDMAN { get; set; }
        public DateTime? RECORDTIME { get; set; }
        public string RECORDINGPHASE { get; set; }
        public string AUDITINGMAN { get; set; }
        public DateTime? AUDITINGTIME { get; set; }
        public string ISAUDITING { get; set; }
        public string APPROVEMAN { get; set; }
        public DateTime? APPROVETIME { get; set; }
        public string ISAPPROVE { get; set; }
        public string NEEDPROCMAN { get; set; }
        public string ISPROCED { get; set; }
        public DateTime? NEEDPROCTIME { get; set; }
        public string QUESTIONTYPES { get; set; }
        public string CONTEXT { get; set; }
        public string AUDITINGCONTEXT { get; set; }
        public string APPROVECONTEXT { get; set; }
        public int? MODIFYRANGE { get; set; }
        public string ISMODIFIED { get; set; }
        public string ISREFUNDMONEY { get; set; }
        public int? ISCONSOLEAPPROVED { get; set; }
        public string PK { get; set; }
        public DateTime? UPLOADTIME { get; set; }
        public DateTime? UPDATETIME { get; set; }
    }
}
