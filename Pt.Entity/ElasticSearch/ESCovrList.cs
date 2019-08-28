using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ElasticSearch
{
    [Serializable]
    public class es_covrlist
    {
        public string UNITCODE { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public int? ID { get; set; }
        public string CODE { get; set; }
        public string PROJECTSERIALNO { get; set; }
        public string PROJECTNAME { get; set; }
        public string CUSTOMID { get; set; }
        public string CHECKITEMTYPE { get; set; }
        public string CHECKITEMNAME { get; set; }
        public DateTime? DECLAREDATE { get; set; }
        public DateTime? BEGINDATE { get; set; }
        public DateTime? ENDDATE { get; set; }
        public string TIMEPERIOD { get; set; }
        public int? STATUS { get; set; }
        public string TESTERS { get; set; }
        public string DECLARANT { get; set; }
        public string DECLARANTIDNO { get; set; }
        public DateTime? FINISHTIME { get; set; }
        public string FINISHER { get; set; }
        public string FINISHIDNO { get; set; }
        public string REMARK { get; set; }
        public string COORDINATES { get; set; }
        public string IMAGES { get; set; }
        public string VIDEOS { get; set; }
        public string WITNESS { get; set; }
        public string CHECKTYPECODE { get; set; }
        public string CHECKTYPENAME { get; set; }
        public string CHECKITEMCODE { get; set; }
        public string CHECKPARAMNAME { get; set; }
        public DateTime? UPLOADTIME { get; set; }
    }
}
