using Nest;
using Pkpm.Entity.ElasticSearch;
using Pkpm.Framework.Repsitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.CovrliistService
{
    public class CovrlistService : ICovrlistService
    {
        IESRepsitory<es_covrlist> m_covrlistESRep;

        public CovrlistService(IESRepsitory<es_covrlist> covrlistESRep)
        {
            this.m_covrlistESRep = covrlistESRep;
        }

        public List<string> GetScheduleCodes(string sysPrimaryKey)
        {
            List<string> curlistCodes = new List<string>();
            if (string.IsNullOrWhiteSpace(sysPrimaryKey))
            {
                return curlistCodes;
            }
            //sysprimarykey查询covrlist，通过code关联查询WxSchedule，显示现场检测详情、图片、视频
            Func<QueryContainerDescriptor<es_covrlist>, QueryContainer> filterQuery = q => q.Term(t => t.Field(f => f.SYSPRIMARYKEY).Value(sysPrimaryKey));
            var covrlist = m_covrlistESRep.Search(s => s.Source(sf => sf.Includes(sfi => sfi.Fields(
                         f => f.SYSPRIMARYKEY,
                         f => f.UNITCODE,
                         f => f.CODE
                         ))).Query(filterQuery).Index("cq-t-bp-subitem"));

            foreach (var item in covrlist.Documents)
            {
                curlistCodes.Add(item.CODE);
            }
            return curlistCodes;
        }

        public List<string> GetScheduleCodesByPrimaryKeys(List<string> sysPrimaryKey)
        {
            List<string> curlistCodes = new List<string>();
            if (sysPrimaryKey != null && sysPrimaryKey.Count <= 0)
            {
                return curlistCodes;
            }
            //sysprimarykey查询covrlist，通过code关联查询WxSchedule，显示现场检测详情、图片、视频
            Func<QueryContainerDescriptor<es_covrlist>, QueryContainer> filterQuery = q => q.Terms(t => t.Field(f => f.SYSPRIMARYKEY).Terms(sysPrimaryKey));
            var covrlist = m_covrlistESRep.Search(s => s.Source(sf => sf.Includes(sfi => sfi.Fields(
                         f => f.SYSPRIMARYKEY,
                         f => f.UNITCODE,
                         f => f.CODE
                         ))).Query(filterQuery).Index("cq-t-bp-subitem").Size(1000));

            foreach (var item in covrlist.Documents)
            {
                curlistCodes.Add(item.CODE);
            }
            return curlistCodes;
        }

        public ISearchResponse<es_covrlist> GetCovrList(string Code)
        {
            Func<QueryContainerDescriptor<es_covrlist>, QueryContainer> filterQuery = q => q.Term(t => t.Field(f => f.CODE).Value(Code));
            var covrlist = m_covrlistESRep.Search(s => s.Source(sf => sf.Includes(sfi => sfi.Fields(
                         f => f.SYSPRIMARYKEY,
                         f => f.CODE,
                         f => f.UPLOADTIME,
                         f => f.PROJECTNAME
                         ))).Query(filterQuery).Index("cq-t-bp-subitem"));
            return covrlist;
        }

        public List<string> GetCovrListByCodes(List<string> Codes)
        {
            List<string> SysPrimaryKey = new List<string>();
            Func<QueryContainerDescriptor<es_covrlist>, QueryContainer> filterQuery = q => q.Terms(t => t.Field(f => f.CODE).Terms(Codes));
            var covrlist = m_covrlistESRep.Search(s => s.Source(sf => sf.Includes(sfi => sfi.Fields(
                         f => f.SYSPRIMARYKEY,
                         f => f.CODE,
                         f => f.UPLOADTIME,
                         f => f.PROJECTNAME
                         ))).Query(filterQuery).Index("cq-t-bp-subitem").Size(1000));
            if (covrlist.Documents.Count > 0)
            {
                foreach (var item in covrlist.Documents)
                {
                    SysPrimaryKey.Add(item.SYSPRIMARYKEY);
                }
            }
            return SysPrimaryKey;
        }
    }
}
