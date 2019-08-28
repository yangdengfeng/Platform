using Nest;
using Pkpm.Entity.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.CovrliistService
{
    public interface ICovrlistService
    {
        /// <summary>
        /// 通过primarykey取现场检测任务编号
        /// </summary>
        /// <param name="sysPrimaryKey"></param>
        /// <returns>List<string> Codes</string></returns>
        List<string> GetScheduleCodes(string sysPrimaryKey);
        /// <summary>
        /// 通过现场检测任务编号获取COVRList（包含PrimaryKey，PROJECTNAME，UPLOADTIME字段）
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        ISearchResponse<es_covrlist> GetCovrList(string Code);
        List<string> GetScheduleCodesByPrimaryKeys(List<string> sysPrimaryKey);
        List<string> GetCovrListByCodes(List<string> Codes);
    }
}
