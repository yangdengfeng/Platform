using Pkpm.Entity.ST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class STReportDetailViewModel
    {
        public es_t_bp_item MainItem { get; set; }
        public string ItemCNName { get; set; }
        public string CheckItemNormal { get; set; }

        public string CutomName { get; set; }
        public List<TotalSearchAcsTimeModel> acsTimeModel { get; set; }
        public List<TotalSearchCommonReportModel> commonReprtModel { get; set; }
        public List<TotalSearchModifyModel> modifyModel { get; set; }
        /// <summary>
        /// 溯源，原材料信息
        /// </summary>
        public List<TotalSearchRawMaterialModel> RawMaterialModel { get; set; }
    }

    public class TotalSearchRawMaterialModel
    {
        public string index { get; set; }
        public string Name { get; set; }
        public string PRODUCEFACTORY { get; set; }
        public string ENTRYDATE { get; set; }
        public string ENTRYAMOUNT { get; set; }
        public string JYPC { get; set; }
    }
    public class STSysSearchModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? IsChanged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DtType { get; set; }
        public string EntrustUnit { get; set; }
        public string CheckInstID { get; set; }
        public string CheckStatus { get; set; }
        public DateTime? StartDt { get; set; }
        public DateTime? EndDt { get; set; }
        public string CheckItem { get; set; }
        public string CheckItemCodes { get; set; }
        public int? HasArc { get; set; }
        public string Area { get; set; }
        public string SampleNum { get; set; }

        /// <summary>
        /// 报告编号
        /// </summary>
        public string ReportNum { get; set; }

        /// <summary>
        /// 报告查询开始时间
        /// </summary>
        public DateTime? ReportStartDt { get; set; }

        /// <summary>
        /// 报告查询结束时间
        /// </summary>
        public DateTime? ReportEndDt { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        public string DataState { get; set; }

        /// <summary>
        /// 委托编号
        /// </summary>
        public string EntrustNum { get; set; }

        public string ReportStatus { get; set; }
        public int? IsCType { get; set; }
      
        public string ItemName { get; set; }
      
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

}