using Pkpm.Entity;
using Pkpm.Framework.Cache;
using Pkpm.Framework.Repsitory;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.AreaCore
{
    public class AreaService : IAreaService
    {
        IRepsitory<t_bp_area> rep;
        ICache<Dictionary<string, string>> cacheArea;
        ICache<List<t_bp_area>> cacheAllArea;

        ICache<List<AreaModel>> cacheTreeArea;
        public AreaService(IRepsitory<t_bp_area> rep, ICache<Dictionary<string, string>> cacheArea, ICache<List<AreaModel>> cacheTreeArea, ICache<List<t_bp_area>> cacheAllArea)
        {
            this.rep = rep;
            this.cacheArea = cacheArea;
            this.cacheTreeArea = cacheTreeArea;
            this.cacheAllArea = cacheAllArea;
        }
        public Dictionary<string, string> GetAllDictArea()
        {
            var cacheKey = PkPmCacheKeys.AllAreaFromArea;
            var areas = cacheArea.Get(cacheKey);
            if (areas == null)
            {
                areas = rep.GetDictByCondition<string, string>(t => true, t => new { t.AREACODE, t.AREANAME });
                cacheArea.Remove(cacheKey);
                cacheArea.Add(cacheKey, areas);
            }
            return areas;
        }

        public List<t_bp_area> GetAllArea()
        {
            var cacheKey = PkPmCacheKeys.AllListArea;
            var areas = cacheAllArea.Get(cacheKey);
            if (areas == null)
            {
                areas = rep.GetByCondition(t => t.PAREACODE != null);
                cacheAllArea.Remove(cacheKey);
                cacheAllArea.Put(cacheKey, areas);
            }
            return areas;
        }

        public string GetAreasByArea(string Area)
        {
            var result = new List<string>();
            var resultStr = string.Empty;
            if (!Area.IsNullOrEmpty())
            {
                var tbpAreas = rep.GetByCondition(t => t.AREANAME == Area);
                if (tbpAreas.Count > 0)
                {
                    var tbpArea = tbpAreas.First();
                    if (tbpArea.PAREACODE != "45")
                    {
                        resultStr = Area;
                    }
                    else
                    {
                        var childAreas = rep.GetByCondition(t => t.PAREACODE == tbpArea.AREACODE);
                        if (childAreas.Count > 0)
                        {
                            foreach (var item in childAreas)
                            {
                                result.Add(item.AREANAME);
                            }
                            result.Add(Area);//把市区的地址加进去，不然会有的查不到
                        }
                        resultStr = result.Join(",");
                    }
                }

            }
            return resultStr;
        }

        public List<AreaModel> GetTreeArea()
        {
            var cacheKey = PkPmCacheKeys.AllAreaByTree;
            var areas = cacheTreeArea.Get(cacheKey);
            if (areas == null)
            {
                var allArea = rep.GetByCondition(t => true);
                var rootNodes = allArea.Where(t => t.PAREACODE == "45");
                int i = 1;
                foreach (var item in rootNodes)
                {
                    string rootName = string.Empty;
                    if (item.AREANAME.Contains('市'))
                    {
                        rootName = item.AREANAME.Split('市')[0];
                        rootName += "地区";
                    }
                    //构建地区节点
                    AreaModel model = new AreaModel()
                    {
                        AreaCode = i.ToString(),
                        Name = rootName,
                        ParentCode = -1,
                        Childs = new List<AreaModel>()
                    };
                    AreaModel chileModel = new AreaModel()
                    {
                        AreaCode = item.AREACODE,
                        Name = item.AREANAME,
                        ParentCode = i,
                        Childs = new List<AreaModel>()
                    };
                    var lastNodes = allArea.Where(t => t.PAREACODE == item.AREACODE);
                    foreach (var lastNode in lastNodes)
                    {
                        AreaModel lastNodeModel = new AreaModel()
                        {
                            AreaCode = lastNode.AREACODE,
                            Name = lastNode.AREANAME,
                            ParentCode = int.Parse(lastNode.AREACODE),
                        };
                        chileModel.Childs.Add(lastNodeModel);
                    }
                    model.Childs.Add(chileModel);
                    i++;
                    areas.Add(model);
                }
            }
            cacheTreeArea.Remove(cacheKey);
            cacheTreeArea.Add(cacheKey, areas);
            return areas;
        }

    }

    public class AreaModel
    {
        public string AreaCode { get; set; }
        public int ParentCode { get; set; }
        public string Name { get; set; }
        public List<AreaModel> Childs { get; set; }
    }

}
