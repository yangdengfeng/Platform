using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Entity.ElasticSearch;
using Pkpm.Framework.Repsitory;
using PkpmGX.Models;
using Nest;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.ItemNameCore;
using Pkpm.Entity;
using Pkpm.Entity.Auth;
using Pkpm.Entity.DTO;
using ServiceStack;
using Pkpm.Framework.Logging;
using Pkpm.Core.AreaCore;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class WelcomeController : PkpmController
    {
        IESRepsitory<es_t_bp_item> tbpitemESRep;
        ICheckUnitService checkUnitService;
        IItemNameService itemNameService;
        IRepsitory<UserInItem> userItemrep;
        IRepsitory<UserInCustom> userCustomrep;
        IRepsitory<t_bp_custom> customRep;
        IRepsitory<t_bp_Equipment> equipmentRep;
        //IRepsitory<t_bp_area> areaRep;
        IAreaService areaService;
        string UnQualiKey = "UnQualified";
        string ModifyKey = "Modify";
        string ReportKey = "Report";



        string aggeKey = "Custom";

        public WelcomeController(IUserService userService,
             IESRepsitory<es_t_bp_item> tbpitemESRep,
             ICheckUnitService checkUnitService,
             IItemNameService itemNameService,
             IRepsitory<UserInItem> userItemrep,
             IRepsitory<UserInCustom> userCustomrep,
             IRepsitory<t_bp_custom> customRep,
             IRepsitory<t_bp_Equipment> equipmentRep,
              //IRepsitory<t_bp_area> areaRep
              IAreaService areaService
            ) : base(userService)
        {
            this.checkUnitService = checkUnitService;
            this.itemNameService = itemNameService;
            this.tbpitemESRep = tbpitemESRep;
            this.userCustomrep = userCustomrep;
            this.userItemrep = userItemrep;
            this.areaService = areaService;
            this.equipmentRep = equipmentRep;
            this.customRep = customRep;
        }

        // GET: Welcome
        public ActionResult Index()
        {

            var measnumEndDate = new PagingOptions<t_bp_custom>(0, 7, t => t.measnumEndDate);
            var measnumDate = customRep.GetByConditonPage<MeasnumDateModel>(t => t.measnumEndDate != null, t => new { t.ID, t.NAME, t.measnumEndDate }, measnumEndDate);//检测资质有效期日期

            var detectnumEndDate = new PagingOptions<t_bp_custom>(0, 7, t => t.detectnumEndDate);
            var detectnumDate = customRep.GetByConditonPage<DetectnumDateModel>(t => t.detectnumEndDate != null, t => new { t.ID, t.NAME, t.detectnumEndDate }, detectnumEndDate);//计量认证有效期日期

            var checkenddate = new PagingOptions<t_bp_Equipment>(0, 7, t => t.checkenddate);
            var checkdate = equipmentRep.GetByConditonPage<CheckdateModel>(t => t.checkenddate != null, t => new { t.id, t.EquName, t.checkenddate, t.customid }, checkenddate);//设备有效期

            var allUnitByArea = checkUnitService.GetAllCustomInArea();
            WelcomeViewMoel viewmodel = new WelcomeViewMoel()
            {
                EchartUnqualifyReportsCount = new List<EchartNameValue>(),
                EchartTotalReportsCount = new List<EchartNameValue>(),
                EcharModifyReportsCount = new List<EchartNameValue>(),
                MeasnumDate = new List<DueRemindModel>(),
                DetectnumDate = new List<DueRemindModel>(),
                Checkdate = new List<DueRemindModel>()

            };
            int i = 1;
            foreach (var item in measnumDate)
            {

                DueRemindModel dueRemind = new DueRemindModel();
                var dateTime = Convert.ToDateTime(DateTime.Now);
                var time = dateTime - Convert.ToDateTime(item.measnumEndDate);
                if (time.Days >= 0)
                {
                    dueRemind.Type = item.Name + "单位检测资质证书已经过期" + time.Days + "天";
                }
                else
                {
                    dueRemind.Type = item.Name + "单位检测资质证书有效期剩余" + time.Days * (-1) + "天";
                }
                dueRemind.Date = item.measnumEndDate.HasValue ? item.measnumEndDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                dueRemind.Num = i;
                i++;
                dueRemind.Id = item.Id;
                viewmodel.MeasnumDate.Add(dueRemind);
            }
            i = 1;
            foreach (var item in detectnumDate)
            {

                DueRemindModel dueRemind = new DueRemindModel();
                var dateTime = Convert.ToDateTime(DateTime.Now);
                var time = dateTime - Convert.ToDateTime(item.detectnumEndDate);
                if (time.Days >= 0)
                {
                    dueRemind.Type = item.Name + "单位计量认证证书已经过期" + time.Days + "天";
                }
                else
                {
                    dueRemind.Type = item.Name + "单位计量认证证书有效期剩余" + time.Days * (-1) + "天";
                }
                dueRemind.Num = i;
                i++;
                dueRemind.Date = item.detectnumEndDate.HasValue ? item.detectnumEndDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                dueRemind.Id = item.Id;
                viewmodel.DetectnumDate.Add(dueRemind);
            }

            i = 1;
            foreach (var item in checkdate)
            {

                DueRemindModel dueRemind = new DueRemindModel();
                var dateTime = Convert.ToDateTime(DateTime.Now);
                var time = dateTime - Convert.ToDateTime(item.checkenddate);
                var checkName = checkUnitService.GetCheckUnitById(item.customid);
                if (time.Days >= 0)
                {
                    dueRemind.Type = checkName + "单位仪器设备检定已经过期" + time.Days + "天";
                }
                else
                {
                    dueRemind.Type = checkName + "单位仪器设备检定有效期剩余" + time.Days * (-1) + "天";
                }
                dueRemind.Num = i;
                i++;
                dueRemind.Id = item.id;
                dueRemind.Date = item.checkenddate.HasValue ? item.checkenddate.Value.ToString("yyyy-MM-dd") : string.Empty;
                viewmodel.Checkdate.Add(dueRemind);
            }

            DateTime DtNow = DateTime.Now;
            string dtFormatStr = "yyyy-MM-dd'T'HH:mm:ss";
            string endDtStr = DtNow.AddDays(1).ToString(dtFormatStr);
            //string endDt = endDtStr
            DateTime DtStart = DtNow.AddDays(-7);
            string startDtStr = DtStart.ToString(dtFormatStr);

            #region 获得首页上部统计信息
            var instFilter = GetCurrentInstFilter();

            Func<Nest.QueryContainerDescriptor<es_t_bp_item>, QueryContainer> qcd = q =>
            {
                var qc = q.DateRange(qdr => qdr.Field(qdrf => qdrf.PRINTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)))
                    && q.DateRange(qddr => qddr.Field(qddrf => qddrf.PRINTDATE).LessThanOrEquals(DateMath.FromString(endDtStr)));

                if (instFilter.NeedFilter && instFilter.FilterInstIds.Count() > 0)
                {
                    qc = qc && +q.Terms(qtm => qtm.Field(qtmf => qtmf.CUSTOMID).Terms(instFilter.FilterInstIds));
                }

                return qc;
            };
            var response = tbpitemESRep.Search(s => s.Size(0).Query(qcd).Aggregations(af => af.Terms(UnQualiKey, adn => adn.Field(aaa => aaa.CONCLUSIONCODE))
                                                                        .Terms(ModifyKey, adm => adm.Field(aadm => aadm.HAVELOG))
                                                                        .Terms(ReportKey, ada => ada.Field(adaa => adaa.HAVREPORT))));

            if (response.IsValid)
            {
                viewmodel.TotalReports = (int)response.Total;
                var unQualiBuckets = response.Aggs.Terms(UnQualiKey).Buckets;
                foreach (var buctet in unQualiBuckets)
                {
                    if (buctet.Key == "N")
                    {
                        viewmodel.UnqualifyReports = (int)buctet.DocCount;
                    }
                }
                var modifyBuckets = response.Aggs.Terms(ModifyKey).Buckets;
                foreach (var buctet in modifyBuckets)
                {
                    if (buctet.Key == "1")
                    {
                        viewmodel.ModifyReports = (int)buctet.DocCount;
                    }
                }
                var reportBuckets = response.Aggs.Terms(ReportKey).Buckets;

                foreach (var buctet in reportBuckets)
                {
                    if (buctet.Key == "1")
                    {
                        viewmodel.PKRReports = (int)buctet.DocCount;
                    }
                }
            }
            if (viewmodel.TotalReports == 0)
            {
                viewmodel.UnqualifyPercentage = "0";
                viewmodel.ModifyPercentage = "0";
                viewmodel.PKRPercentage = "0";
            }
            else
            {
                viewmodel.UnqualifyPercentage = (viewmodel.UnqualifyReports * 100 / viewmodel.TotalReports).ToString();
                viewmodel.ModifyPercentage = (viewmodel.ModifyReports * 100 / viewmodel.TotalReports).ToString();
                viewmodel.PKRPercentage = (viewmodel.PKRReports * 100 / viewmodel.TotalReports).ToString();
            }

            #endregion

            #region 获得首页中间部分地图信息数据

            var countByArea = new Dictionary<string, int>();
            var unQualifyCount = new Dictionary<string, int>();
            var ModifyCount = new Dictionary<string, int>();

            var EchartTotalReportsCount = new Dictionary<string, int>();
            var EchartUnqualifyReportsCount = new Dictionary<string, int>();
            var EcharModifyReportsCount = new Dictionary<string, int>();

            var allArea = areaService.GetAllArea();

            var userItems = userItemrep.GetDictByCondition<string, string>(r => r.UserId == GetCurrentUserId() && r.UserItemType == UserItemType.UnQualifiedReport,
             r => new { r.Id, r.ItemTableName });
            var userInstIds = userCustomrep.GetDictByCondition<string, string>(r => r.UserId == GetCurrentUserId() && r.UserCustomType == UserCustomType.UnQualifiedCount,
                r => new { r.Id, r.CustomId });

            SysSearchModel model = new SysSearchModel()
            {
                StartDt = DateTime.Today.AddDays(-7),
                EndDt = DateTime.Today.AddDays(1)
            };
            var filterQuery = GetFilterQuery(checkUnitService, itemNameService, model, userInstIds, userItems);
            var mapResponse = tbpitemESRep.Search(s => s.Size(0).Query(filterQuery)
                         .Aggregations(af => af.Terms(aggeKey, item => item.Field(iif => iif.CUSTOMID).Size(1000)
                            .Aggregations(aaaf => aaaf.Terms("Conclution", aaa => aaa.Field(aac => aac.CONCLUSIONCODE)).Terms("Havelog", aab => aab.Field(aad => aad.HAVELOG))
                            ))));

            if (mapResponse.IsValid)
            {
                var bucks = mapResponse.Aggs.Terms(aggeKey).Buckets;
                foreach (var item in bucks)
                {
                    var unqualiBucks = item.Terms("Conclution").Buckets;
                    var modifyBucks = item.Terms("Havelog").Buckets;
                    var customId = item.Key;
                    var unitArea = string.Empty;

                    var areaBool = allUnitByArea.TryGetValue(item.Key, out unitArea);
                    if (areaBool)
                    {
                        GetDictResult(countByArea, item, unitArea);
                    }
                    else
                    {
                        unitArea = "其他";
                        GetDictResult(countByArea, item, unitArea);
                    }

                    foreach (var unqualifyItem in unqualiBucks)
                    {
                        if (unqualifyItem.Key == "N" || unqualifyItem.Key == "n")
                        {
                            GetDictResult(unQualifyCount, unqualifyItem, unitArea);
                        }
                    }

                    foreach (var modifyItem in modifyBucks)
                    {
                        if (modifyItem.Key == "1")
                        {
                            GetDictResult(ModifyCount, modifyItem, unitArea);
                        }
                    }
                }

                foreach (var item in countByArea)
                {
                    GetEchartValue(EchartTotalReportsCount, allArea, item);
                }

                foreach (var item in unQualifyCount)
                {
                    GetEchartValue(EchartUnqualifyReportsCount, allArea, item);
                }

                foreach (var item in ModifyCount)
                {
                    GetEchartValue(EcharModifyReportsCount, allArea, item);
                }

                foreach (var item in EchartTotalReportsCount)
                {
                    EchartNameValue echart = new EchartNameValue()
                    {
                        name = item.Key,
                        value = item.Value
                    };
                    viewmodel.EchartTotalReportsCount.Add(echart);
                }

                foreach (var item in EchartUnqualifyReportsCount)
                {
                    EchartNameValue echart = new EchartNameValue()
                    {
                        name = item.Key,
                        value = item.Value
                    };
                    viewmodel.EchartUnqualifyReportsCount.Add(echart);
                }

                foreach (var item in EcharModifyReportsCount)
                {
                    EchartNameValue echart = new EchartNameValue()
                    {
                        name = item.Key,
                        value = item.Value
                    };
                    viewmodel.EcharModifyReportsCount.Add(echart);
                }

                viewmodel.EchartTotalReportsCount = viewmodel.EchartTotalReportsCount.OrderBy(s => s.name).ToList();
                viewmodel.EchartUnqualifyReportsCount = viewmodel.EchartUnqualifyReportsCount.OrderBy(s => s.name).ToList();
                viewmodel.EcharModifyReportsCount = viewmodel.EcharModifyReportsCount.OrderBy(s => s.name).ToList();
            }

            #endregion

            #region 获得首页下部到期提醒数据
            #endregion

            return View(viewmodel);
        }

        private static void GetEchartValue(Dictionary<string, int> EchartReportsCount, List<t_bp_area> allArea, KeyValuePair<string, int> item)
        {
            if (item.Key.IsNullOrEmpty())
            {
                return;
            }
            List<t_bp_area> areas = new List<t_bp_area>();
            var areass = allArea.Where(t => t.AREANAME == item.Key.Trim());
            if (areass != null)
            {
                areas = areass.ToList();
            }
            if (areas != null && areas.Count > 0)
            {
                var area = areas.First();//获得地区信息
                if (area.PAREACODE == "45") //说明地区是市级，直接加进去
                {
                    GetDictResult(EchartReportsCount, item, area);
                }
                else//如果是县级区域或者是市辖区
                {
                    var pareCode = area.PAREACODE;
                    List<t_bp_area> pareAreas = new List<t_bp_area>();
                    var pareAreass = allArea.Where(t => t.AREACODE == pareCode);
                    if (pareAreass != null)
                    {
                        pareAreas = pareAreass.ToList();
                    }
                    if (pareAreas.Count > 0)
                    {
                        var pareArea = pareAreas.First();
                        GetDictResult(EchartReportsCount, item, pareArea);
                    }
                }
            }
            else
            {
                //忽略掉所有在上面地区为其他的选项，反正在页面显示不出来  by 振华 19.2.18
            }
        }

        private static void GetDictResult(Dictionary<string, int> Dict, KeyValuePair<string, int> item, t_bp_area area)
        {
            int areaCount = 0;
            if (Dict.TryGetValue(area.AREANAME, out areaCount))
            {
                Dict[area.AREANAME] = areaCount + item.Value;
            }
            else
            {
                Dict.Add(area.AREANAME, item.Value);
            }
        }

        private static void GetDictResult(Dictionary<string, int> dict, KeyedBucket<string> item, string unitArea)
        {
            int count = 0;
            if (dict.TryGetValue(unitArea, out count))
            {
                dict[unitArea] = count + (int)item.DocCount;
            }
            else
            {
                dict.Add(unitArea, (int)item.DocCount);
            }
        }
    }
}