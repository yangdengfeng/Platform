using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.DTO
{
    public class UIModels
    {
    }
    public class HomeEchartData<T> where T : class
    {
        public int page { get; set; }
        public int totalcount { get; set; }
        public int pageCount { get; set; }
        public int pageSize { get; set; }
        public List<T> records { get; set; }
    }

    public class EchartNameValue
    {
        public string name { get; set; }
        public int value { get; set; }
    }

    public class ReportInfo
    {
        /// <summary>
        /// 报告编号
        /// </summary>
        public string ReportNum { get; set; }
        /// <summary>
        /// 工程名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 工程部位
        /// </summary>
        public string StructPart { get; set; }
        /// <summary>
        /// 检测项目
        /// </summary>
        public string CheckItem { get; set; }
        /// <summary>
        /// 检测参数
        /// </summary>
        public string CheckParam { get; set; }
        /// <summary>
        /// 检测结论
        /// </summary>
        public string Conclusion { get; set; }
        /// <summary>
        /// 报告日期
        /// </summary>
        public string ReportDate { get; set; }
        /// <summary>
        /// 防伪取样标识
        /// </summary>
        public string AntiFakeLabel { get; set; }
        /// <summary>
        /// 项目代码
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// 检测机构注册号码
        /// </summary>
        public string UnitCode { get; set; }
    }

    public class TBpModifyLogModel
    {
        public string PK { get; set; }
        public string FIELDNAME { get; set; }
        public string BEFOREMODIFYVALUES { get; set; }
        public string AFTERMODIFYVALUES { get; set; }
        public string MODIFYDATETIME { get; set; }
    }

}
