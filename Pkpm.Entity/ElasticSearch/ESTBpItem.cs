using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.ElasticSearch
{
    [Serializable]
    public class es_t_bp_item
    {
        public string CUSTOMID { get; set; } 

        public string SYSPRIMARYKEY { get; set; } 
        
        public string PROJECTNUM { get; set; }

        public string ITEMNAME { get; set; } 

        public string SENDSAMPLEMAN { get; set; }

        public DateTime? REPORTCONSENTDATE { get; set; }

        public string SAMPLENUM { get; set; }

        /// <summary>
        /// 委托编号
        /// </summary>
        public string ENTRUSTNUM { get; set; }

        public string REPORTNUM { get; set; }

        /// <summary>
        ///  MA19 之类
        /// </summary>
        public string REPORTJXLB { get; set; }

        /// <summary>
        /// 报告流水号
        /// </summary>
        public int? REPSEQNO { get; set; }

        /// <summary>
        /// 除流水号之外的报告编号
        /// </summary>
        public string REPORMNUMWITHOUTSEQ { get; set; }

        public string SAMPLENAME { get; set; }

        public string STANDARDNAME { get; set; }

        /// <summary>
        /// 检测类别
        /// </summary>
        public string CHECKTYPE { get; set; } 
      
        public string STRUCTPART { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        public string SAMPLEDISPOSEPHASE { get; set; }

        /// <summary>
        /// 没有分词的状态位
        /// </summary>
        public string SAMPLEDISPOSEPHASEORIGIN { get; set; }

       

        public string PRODUCEFACTORY { get; set; }

        public DateTime? ENTRUSTDATE { get; set; }

        public DateTime? CHECKDATE { get; set; }

        public DateTime? AUDITINGDATE { get; set; }

        public DateTime? APPROVEDATE { get; set; }

        public DateTime? PRINTDATE { get; set; }

        public string CHECKCONCLUSION { get; set; }

        public string CONCLUSIONCODE { get; set; }

        public string ACCEPTSAMPLEMAN { get; set; }

        public string FIRSTCHECKMAN { get; set; }

        public string SECONDCHECKMAN { get; set; }

        public string VERIFYMAN { get; set; }

        public string AUDITINGMAN { get; set; }

        public string APPROVEMAN { get; set; }

        public string PRINTMAN { get; set; }

        public string EXTENDMAN { get; set; }

        public string EXPLAIN { get; set; }

        public int? REPORTCONSENTDAYS { get; set; } 

        public string SAMPLECHARGETYPE { get; set; }

        public float? NEEDCHARGEMONEY { get; set; }

        public string COLLATEMAN { get; set; }

        public DateTime? COLLATEDATE { get; set; }

        public DateTime? VERIFYDATE { get; set; }

        public DateTime? EXTENDDATE { get; set; }

        public string BEFORESTATUS { get; set; }

        public string AFTERSTATUS { get; set; }

        public string TEMPERATURE { get; set; }

        public string HUMIDITY { get; set; }

        public string PDSTANDARDNAME { get; set; }

        public DateTime? FACTCHECKDATE { get; set; } 

        public string SENDSAMPLEMANTEL { get; set; }

        public string DATATYPE { get; set; }

        public string REPORTFILE { get; set; }

        public string INVALIDATE { get; set; }

        public string INVALIDATETEXT { get; set; }

        public DateTime? ADDTIME { get; set; }

        public string REPORTSTYLESTYPES { get; set; }

        public string SENDTOWEB { get; set; } 

        public string CONSTRACTUNIT { get; set; }

        public string ENTRUSTUNIT { get; set; }

        public string CERTCHECK { get; set; }

        public string PROJECTNAME { get; set; }

        public int? HAVEACS { get; set; }

        public int? HAVELOG { get; set; }

        public int? HAVREPORT { get; set; }

        public int? ISCREPORT { get; set; }

        public DateTime? UPDATETIME { get; set; }

        public string WORKSTATION { get; set; }

        public float? POINTCOUNT { get; set; }

        public string ITEMCHNAME { get; set; }

        public string SUBITEMLIST { get; set; }

        public string UNITCODE { get; set; }

        public string QRCODEBAR { get; set; }

        public string SUPERCODE { get; set; }

        public string SUPERUNIT { get; set; }

        public DateTime? ACSTIME { get; set; }

        public string TAKESAMPLEMAN { get; set; }

        public string CSIZE { get; set; } 

        public string INSTRUMENTNUM { get; set; }

        public string INSTRUMENTNAME { get; set; }

        public string CUSTOMCODE { get; set; }

        public string CODEBAR { get; set; }

        public string COLUMN1 { get; set; }

        public string COLUMN2 { get; set; }

        public string CONTRACTNUM { get; set; }

        public string TASKNUM { get; set; }

        public string RECORDNUM { get; set; }

        public string REPORTPATH { get; set; }

        public int? REPORTTYPES { get; set; }

        public int? JDSTATIC { get; set; } 
       
        public DateTime? UPLOADTIME { get; set; } 
         
    }
}
