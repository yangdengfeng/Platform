using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.Auth
{


    public enum UserItemType
    {
        /// <summary>
        /// 不合格报告
        /// </summary>
        UnQualifiedReport,

        /// <summary>
        /// 无曲线报告
        /// </summary>
        NoAcsReport,

        /// <summary>
        /// 曲线异常报告
        /// </summary>
        AcsExcepReport,
        /// <summary>
        /// 报告数量分析
        /// </summary>
        ReportDataAnalysis,
    }
}
