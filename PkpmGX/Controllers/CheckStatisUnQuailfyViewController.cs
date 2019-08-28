using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.ItemNameCore;
using Pkpm.Core.UserCustomize;
using Pkpm.Framework.Repsitory;
using Pkpm.Entity.ElasticSearch;
using Pkpm.Entity;
using Nest;
using Pkpm.Entity.Auth;
using Pkpm.Entity.DTO;
using ServiceStack;
using Dhtmlx.Model.Grid;
using Dhtmlx.Model.Toolbar;
//using Pkpm.Framework.Logging;
using System.Text;
using Pkpm.Framework.Common;
using Pkpm.Core.ReportCore;
using Pkpm.Core.AreaCore;
using PkpmGX.Models;
using Pkpm.Framework.Logging;

namespace PkpmGX.Controllers
{
    public class CheckStatisUnQuailfyViewController : Controller
    {
        protected string printedPhrase;
        protected string testedPhrase;
        protected string collectedPhrase;
        protected string filterCustom;

        protected string checkedPhrase;
        protected string proofreadPhrase;
        protected string verifyedPhrase;
        protected string approvaledPhrase;
        protected string giveOutPhrase;
        protected string fileedPhrase;
        protected string canceledPhrase;
        ICheckUnitService checkUnitService;
        IItemNameService itemNameService;
        IUserCustomize userCustomizeService;
        IESRepsitory<es_t_bp_item> tbpitemESRep;
        //IRepsitory<WxInspectUnit> m_repUnit;
        IRepsitory<UserInItem> userItemrep;
        IRepsitory<UserInCustom> userCustomrep;
        IAreaService AreaService;
        private static ILogger logger = new NLogLoggerFactory().CreateLogger("Pkpm.Web.Controllers.WelcomeController");
        IReportService reportService;
        string aggeKey;
        public CheckStatisUnQuailfyViewController(
             ICheckUnitService checkUnitService,
             IItemNameService itemNameService,
             IUserCustomize userCustomizeService,
             IESRepsitory<es_t_bp_item> tbpitemESRep,
        //IRepsitory<WxInspectUnit> m_repUnit;
             IRepsitory<UserInItem> userItemrep,
             IAreaService AreaService,
        IReportService reportService,
             IRepsitory<UserInCustom> userCustomrep) //: base(userService)
        {
            this.checkUnitService = checkUnitService;
            this.itemNameService = itemNameService;
            this.tbpitemESRep = tbpitemESRep;
            //this.m_repUnit = repUnit;
            this.userItemrep = userItemrep;
            this.AreaService = AreaService;
            this.userCustomrep = userCustomrep;
            this.userCustomizeService = userCustomizeService;
            this.reportService = reportService;
            aggeKey = "ItemNum";
            printedPhrase = System.Configuration.ConfigurationManager.AppSettings["PrintedSamplePhrase"];//已打印
            testedPhrase = System.Configuration.ConfigurationManager.AppSettings["TestedSamplePhrase"];//已检测
            collectedPhrase = System.Configuration.ConfigurationManager.AppSettings["CollectedSamplePhrase"];//已收样
            checkedPhrase = System.Configuration.ConfigurationManager.AppSettings["checkedSamplePhrase"];//已复核
            proofreadPhrase = System.Configuration.ConfigurationManager.AppSettings["proofreadSamplePhrase"];//已校核
            verifyedPhrase = System.Configuration.ConfigurationManager.AppSettings["verifyedSamplePhrase"];//已审核
            approvaledPhrase = System.Configuration.ConfigurationManager.AppSettings["approvaledSamplePhrase"];//已批准
            giveOutPhrase = System.Configuration.ConfigurationManager.AppSettings["giveOutSamplePhrase"];//已发放
            fileedPhrase = System.Configuration.ConfigurationManager.AppSettings["fileedSamplePhrase"];//已归档
            canceledPhrase = System.Configuration.ConfigurationManager.AppSettings["canceledSamplePhrase"];//已作废
            filterCustom = System.Configuration.ConfigurationManager.AppSettings["searchCustomBlackList"];//已收样
        }

        // GET: CheckStatisUnQuailfy
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UnquailfyAnalysis(SysSearchModel model)
        {
            return View(model);
        }

        // GET: CheckStatisUnQuailfy/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CheckStatisUnQuailfy/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckStatisUnQuailfy/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CheckStatisUnQuailfy/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckStatisUnQuailfy/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CheckStatisUnQuailfy/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckStatisUnQuailfy/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Statis(SysSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            model.GroupType = "Item";
            var statisData = GetStatisData(model);

            StatisChart chart = new StatisChart();
            if (statisData.IsValid)
            {
                var bucks = statisData.Aggs.Terms(aggeKey).Buckets;
                var allItems = itemNameService.GetAllItemName();
                string itemValue = string.Empty;
                //string CustomId = InstUserCustomId();
                int index = 1;
                bool ProjectCount30 = false;
                foreach (var item in bucks)
                {
                    switch (aggeKey)
                    {
                        case "ItemNum":
                            if (index++ > 30)
                            {
                                ProjectCount30 = true;
                                break;
                            }
                            var itemKeys = item.Key.Split('|');
                            var jxlb = itemKeys[0];
                            var itemCode = itemKeys[1];
                            itemValue = itemNameService.GetItemCNNameFromAll(allItems, jxlb, itemCode);
                            break;
                        case "Custom":
                            if (index++ > 30)
                            {
                                ProjectCount30 = true;
                                break;
                            }
                            itemValue = checkUnitService.GetCheckUnitById(item.Key);
                            break;
                        case "Project"://按工程分组只显示30个
                            if (index++ > 30)
                            {
                                ProjectCount30 = true;
                                break;
                            }
                            itemValue = item.Key;
                            break;
                    }
                    if (ProjectCount30)
                    {
                        break;
                    }
                    chart.StatisChartItems.Add(new StatisChartItem()
                    {
                        StatisKey = item.Key,
                        StatisName = itemValue,
                        DocCount = item.DocCount.HasValue ? item.DocCount.Value : 0
                    });
                }
            }

            return Content(chart.ToJson());

        }

        private ISearchResponse<es_t_bp_item> GetStatisData(SysSearchModel model)
        {
            if (model.GroupType != "Custom") //不以机构分组则只查不合格，以机构分组则查询所有报告，再聚合一次不合格数量
            {
                model.modelType = SysSearchModelModelType.UnQualified;
            }
       
            var filterQuery = GetFilterQuery(checkUnitService, itemNameService, model);
            ISearchResponse<es_t_bp_item> response = null;
            if (!string.IsNullOrEmpty(model.GroupType))
            {
                switch (model.GroupType)
                {
                    case "Item":
                        aggeKey = "ItemNum";
                        //response = tbpitemESRep.Search(s => s.Size(0).Query(filterQuery)
                        //.Aggregations(af => af.Terms(aggeKey, item => item
                        //.Field(iif => iif.ITEMNAME).Size(1000))));
                        response = tbpitemESRep.Search(s => s.Size(0).Query(filterQuery)
                                             .Aggregations(af => af.Terms(aggeKey, item => item
                                                .Script("doc['REPORTJXLB'].value+'|'+doc['ITEMNAME'].value").Size(1000))));
                        break;
                    case "Custom":
                        aggeKey = "Custom";
                        response = tbpitemESRep.Search(s => s.Size(0).Query(filterQuery)
                                     .Aggregations(af => af.Terms(aggeKey, item => item
                                        .Field(iif => iif.CUSTOMID).Size(1000)
                                        .Aggregations(aaaf => aaaf.Terms("Conclution", aaa => aaa.Field(aac => aac.CONCLUSIONCODE).Aggregations(tt => tt.Terms("ProductFactory", ttt => ttt.Field(tttt => tttt.PRODUCEFACTORY))))
                                        ))));
                        break;
                    case "Project":
                        aggeKey = "Project";
                        response = tbpitemESRep.Search(s => s.Size(0).Query(filterQuery)
                                     .Aggregations(af => af.Terms(aggeKey, item => item
                                        .Field(iif => iif.PROJECTNAME[0].Suffix("PROJECTNAMERAW")).Size(1000))));
                        break;
                    default:
                        aggeKey = "ItemNum";
                        response = tbpitemESRep.Search(s => s.Size(0).Query(filterQuery)
                                     .Aggregations(af => af.Terms(aggeKey, item => item
                                        .Field(iif => iif.ITEMNAME).Size(1000))));
                        break;
                }
            }


            return response;
        }

        public ActionResult StatisGrid(SysSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            model.GroupType = "Item";
            var response = GetStatisData(model);

            DhtmlxGrid grid = new DhtmlxGrid();
            if (response.IsValid)
            {

                int index = pos;
                var bucks = response.Aggs.Terms(aggeKey).Buckets;
                var allItems = itemNameService.GetAllItemName();
                int totalCount = bucks.Count;
                grid.AddPaging(totalCount, pos);
                string itemValue = string.Empty;
                foreach (var item in bucks)
                {
                    string itemKey = item.Key;
                    switch (aggeKey)
                    {
                        case "ItemNum":
                            var itemKeys = item.Key.Split('|');
                            var jxlb = itemKeys[0];
                            var itemCode = itemKeys[1];
                            itemValue = itemNameService.GetItemCNNameFromAll(allItems, jxlb, itemCode);
                            break;
                        case "Custom":
                            itemValue = checkUnitService.GetCheckUnitById(item.Key);
                            break;
                        case "Project":
                            itemValue = item.Key;
                            //item.Key= HttpUtility.UrlEncode(item.Key);
                            break;

                    }
                    DhtmlxGridRow row = new DhtmlxGridRow(item.Key);
                    row.AddCell((index + 1).ToString());
                    row.AddCell(itemValue);
                    row.AddCell((item.DocCount.HasValue ? item.DocCount.Value : 0).ToString());
                    row.AddLinkJsCell("材料动态分析表查看", "showUnquailfyAnalysis(\"{0}\",\"{1}\")".Fmt(itemKey, itemValue));
                    grid.AddGridRow(row);

                    index++;
                }
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        /// <summary>
        /// 根据机构分组统计之后，塞到对应的地区中
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Search(SysSearchModel model)
        {
            model.GroupType = "Custom";
            var response = GetStatisData(model);
            DhtmlxGrid grid = new DhtmlxGrid();
            /*
             * 1.查出所有报告数量以及分组统计不合格报告数量
             * 2.countByArea 存储该地区报告总数
             * 3.需要一个存储该地区所有不合格报告数量
             * 4.存储改地区所有有不合格报告数量的厂家    厂家总数
             * 
             */
            if (response.IsValid)
            {
                var bucks = response.Aggs.Terms("Custom").Buckets;
                Dictionary<string, int> countByArea, unQualifyCount, countUnitByArea, unQualifyUnitCountByArea;
                GetSearchResult(bucks, out countByArea, out unQualifyCount, out countUnitByArea, out unQualifyUnitCountByArea);
                var allAreas = AreaService.GetAllArea();
                /*
                 *1.从所有的地区中选择名字相对应的地区，判断是否为市级节点，是直接加入，不是的话需要判断本节点所属的市级节点是否存在，如果存在直接加上，不存在则增加一条数值为0的记录 
                 */
                var index = 1;
                var rootAreas = new List<AreaResultModel>();
                var childAreas = new List<AreaResultModel>();
                foreach (var item in countByArea)
                {
                    if (item.Key.IsNullOrEmpty())
                    {
                        continue;
                    }


                    var unQualifyCountRow = 0;
                    var unitCountByArea = 0;
                    var unqualifyUnitCount = 0;
                    var isUnqualify = unQualifyCount.TryGetValue(item.Key, out unQualifyCountRow); //isUnqualify 标识是否有不合格报告
                    var isUnitCount = countUnitByArea.TryGetValue(item.Key, out unitCountByArea);
                    var isUnqualifyUnitCount = unQualifyUnitCountByArea.TryGetValue(item.Key, out unqualifyUnitCount);
                    List<t_bp_area> areas = new List<t_bp_area>();

                    var areass = allAreas.Where(t => t.AREANAME == item.Key);


                    if (areass != null)
                    {
                        areas = areass.ToList();
                    }

                    if (areas != null && areas.Count > 0)
                    {
                        var area = areas.First();
                        logger.Debug("areasfirst");
                        AreaResultModel areaResult = new AreaResultModel()
                        {
                            AreaCode = area.AREACODE,
                            ParentCode = area.PAREACODE,
                            Name = area.AREANAME,
                            TotalCount = (int)item.Value,
                            UnqualifuCount = unQualifyCountRow,
                            FactoryCount = unitCountByArea,
                            UnqualifyFactoryCount = unqualifyUnitCount
                        };

                        if (area.PAREACODE == "45")//市
                        {
                            GetRootAreaResult(rootAreas, area, areaResult, areaResult);
                        }
                        else //市辖区，县等
                        {
                            childAreas.Add(areaResult);
                            var parentCode = area.PAREACODE;
                            if (parentCode != null)
                            {
                                List<t_bp_area> parentAreas = new List<t_bp_area>();
                                var parentAreass = allAreas.Where(t => t.AREACODE == parentCode);
                                if (parentAreass != null)
                                {
                                    parentAreas = parentAreass.ToList();
                                }
                                if (parentAreas != null && parentAreas.Count > 0)
                                {
                                    var parentArea = parentAreas.First(); //获得所属市地区信息
                                    AreaResultModel parentareaResult = new AreaResultModel()
                                    {
                                        AreaCode = parentArea.AREACODE,
                                        Name = parentArea.AREANAME,
                                        TotalCount = 0,
                                        UnqualifuCount = 0,
                                        FactoryCount = 0,
                                        UnqualifyFactoryCount = 0
                                    };
                                    GetRootAreaResult(rootAreas, parentArea, areaResult, parentareaResult);
                                }
                            }

                        }
                    }
                }
                foreach (var item in rootAreas)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(item.AreaCode);
                    row.AddCell(index++);
                    row.AddCell(item.Name);
                    row.AddCell(item.TotalCount);
                    row.AddCell(item.UnqualifuCount);
                    row.AddCell(item.TotalCount == 0 ? "0" : Math.Round(((item.UnqualifuCount * 1.00 / item.TotalCount) * 100), 2).ToString() + "%");
                    row.AddCell(item.FactoryCount);
                    row.AddCell(item.UnqualifyFactoryCount);
                    row.AddCell(item.FactoryCount == 0 ? "0" : Math.Round(((item.UnqualifyFactoryCount * 1.00 / item.FactoryCount) * 100), 2).ToString() + "%");
                    row.AddCell(string.Empty);//同比
                    row.AddCell(string.Empty);//环比

                    if (item.TotalCount > 0)
                    {
                        row.AddLinkJsCell("详情查看", "showItemKeyGrid(\"{0}\")".Fmt(item.Name));
                    }

                    List<AreaResultModel> thisChildAreas = new List<AreaResultModel>();

                    var thisChildAreass = childAreas.Where(t => t.ParentCode == item.AreaCode);
                    if (thisChildAreass != null)
                    {
                        thisChildAreas = thisChildAreass.ToList();
                    }
                    if (thisChildAreas != null && thisChildAreas.Count > 0)
                    {
                        var childIndex = 1;
                        foreach (var childItem in thisChildAreas)
                        {
                            DhtmlxGridRow childRow = new DhtmlxGridRow(childItem.AreaCode);
                            childRow.AddCell(childIndex++);
                            childRow.AddCell(childItem.Name);
                            childRow.AddCell(childItem.TotalCount);
                            childRow.AddCell(childItem.UnqualifuCount);
                            childRow.AddCell(childItem.TotalCount == 0 ? "0" : Math.Round(((childItem.UnqualifuCount * 1.00 / childItem.TotalCount) * 100), 2).ToString() + "%");
                            childRow.AddCell(childItem.FactoryCount);
                            childRow.AddCell(childItem.UnqualifyFactoryCount);
                            childRow.AddCell(childItem.FactoryCount == 0 ? "0" : Math.Round(((childItem.UnqualifyFactoryCount * 1.00 / childItem.FactoryCount) * 100), 2).ToString() + "%");
                            childRow.AddCell(string.Empty);//同比
                            childRow.AddCell(string.Empty);//环比
                            if (childItem.UnqualifuCount > 0)//有不合格报告才显示详情查看
                            {
                                //TODO 由于改了实现方式，将市辖区，县的信息塞到了市中，在不合格详情查看时需要增加如果是市，则需要查本市所有的不合格数据
                                childRow.AddLinkJsCell("详情查看", "showItemKeyGrid(\"{0}\")".Fmt(childItem.Name));
                            }
                            row.AddRow(childRow);
                        }
                    }
                    grid.AddGridRow(row);

                }
                //foreach (var item in countByArea)
                //{
                //    var unQualifyCountRow = 0;
                //    var unitCountByArea = 0;
                //    var unqualifyUnitCount = 0;
                //    var isUnqualify = unQualifyCount.TryGetValue(item.Key, out unQualifyCountRow); //isUnqualify 标识是否有不合格报告
                //    var isUnitCount = countUnitByArea.TryGetValue(item.Key, out unitCountByArea);
                //    var isUnqualifyUnitCount = unQualifyUnitCountByArea.TryGetValue(item.Key, out unqualifyUnitCount);

                //    DhtmlxGridRow row = new DhtmlxGridRow(item.Key);
                //    row.AddCell(index++);
                //    row.AddCell(item.Key);
                //    row.AddCell(item.Value);
                //    row.AddCell(isUnqualify ? unQualifyCountRow : 0);
                //    row.AddCell(item.Value == 0 ? "0" : Math.Round(((unQualifyCountRow * 1.00 / item.Value) * 100), 2).ToString() + "%");
                //    row.AddCell(unitCountByArea.ToString());
                //    row.AddCell(unqualifyUnitCount.ToString());
                //    row.AddCell(unitCountByArea == 0 ? "0" : Math.Round(((unqualifyUnitCount * 1.00 / unitCountByArea) * 100), 2).ToString() + "%");
                //    row.AddCell(string.Empty);//同比
                //    row.AddCell(string.Empty);//环比
                //    if (isUnqualify)//有不合格报告才显示详情查看
                //    {
                //        row.AddLinkJsCell("详情查看", "showItemKeyGrid(\"{0}\")".Fmt(item.Key));
                //    }
                //    grid.AddGridRow(row);
                //}
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        /// <summary>
        /// 获得根节点地区信息
        /// </summary>
        /// <param name="rootAreas">根节点存储</param>
        /// <param name="area">当前判断的市级区域</param>
        /// <param name="areaResult">当前区域的数据</param>
        /// <param name="parentAreaResult">当前市级数据</param>
        private static void GetRootAreaResult(List<AreaResultModel> rootAreas, t_bp_area area, AreaResultModel areaResult, AreaResultModel parentAreaResult)
        {
            var existAreas = rootAreas.Where(t => t.AreaCode == area.AREACODE).ToList();
            if (existAreas.Count > 0)//认为已经有本区域的数据存在，直接在上面加就好了
            {
                var areaIndex = rootAreas.IndexOf(existAreas.First());
                rootAreas[areaIndex].TotalCount += areaResult.TotalCount;
                rootAreas[areaIndex].UnqualifuCount += areaResult.UnqualifuCount;
                rootAreas[areaIndex].UnqualifyFactoryCount += areaResult.UnqualifyFactoryCount;
                rootAreas[areaIndex].FactoryCount += areaResult.FactoryCount;
            }
            else
            {
                rootAreas.Add(parentAreaResult);
            }
        }

        private void GetSearchResult(IReadOnlyCollection<KeyedBucket<string>> bucks, out Dictionary<string, int> countByArea, out Dictionary<string, int> unQualifyCount, out Dictionary<string, int> countUnitByArea, out Dictionary<string, int> unQualifyUnitCountByArea)
        {
            var allChekUnitInArea = checkUnitService.GetAllCustomInArea();//获取所有机构ID，机构所属地区
            countByArea = new Dictionary<string, int>();
            unQualifyCount = new Dictionary<string, int>();
            countUnitByArea = new Dictionary<string, int>();     //地区厂家总数
            unQualifyUnitCountByArea = new Dictionary<string, int>();//地区不合格报告厂家数量
            foreach (var item in bucks)
            {
                var conclutionBucks = item.Terms("Conclution");
                string unitArea = string.Empty;

                if (allChekUnitInArea.TryGetValue(item.Key, out unitArea))
                {

                    GetDictResult(countByArea, unitArea, (int)item.DocCount);
                    //int count = 0;
                    //if (countByArea.TryGetValue(unitArea, out count))//能够获取到则countByArea有这个地区的数据，取出来与现在的相加即可，不能则countByArea新加一项
                    //{
                    //    countByArea[unitArea] = count + (int)item.DocCount;
                    //}
                    //else
                    //{
                    //    countByArea.Add(unitArea, (int)item.DocCount);
                    //}
                }
                else  //数据库中没有此机构,暂时把机构的数据归为其他地区
                {

                    int count = 0;
                    unitArea = "其他";
                    GetDictResult(countByArea, unitArea, (int)item.DocCount);
                    //if (countByArea.TryGetValue(unitArea, out count))
                    //{
                    //    countByArea[unitArea] = count + (int)item.DocCount;
                    //}
                    //else
                    //{
                    //    countByArea.Add(unitArea, (int)item.DocCount);
                    //}
                }
                //logger.Debug(item.Key + "," + item.DocCount);
                //logger.Debug(unitArea);

                foreach (var conclutionBuck in conclutionBucks.Buckets)
                {
                    if (conclutionBuck.Key == "N" || conclutionBuck.Key == "n")
                    {
                        var conclutionValue = (int)conclutionBuck.DocCount;//获得不合格报告数量
                        //int unQualifyAreaCount = 0;

                        GetDictResult(unQualifyCount, unitArea, conclutionValue);
                        //if (unQualifyCount.TryGetValue(unitArea, out unQualifyAreaCount))   //将不合格报告数量存入地区数组
                        //{
                        //    unQualifyCount[unitArea] = unQualifyAreaCount + conclutionValue;
                        //}
                        //else
                        //{
                        //    unQualifyCount.Add(unitArea, conclutionValue);
                        //}


                        //int areaUnqualifyUnitCount = 0;
                        var ProductFactorycount = 0;
                        var ProductFactoryBucks = conclutionBuck.Terms("ProductFactory");
                        foreach (var ProductFactoryBuck in ProductFactoryBucks.Buckets)
                        {
                            ProductFactorycount++;
                        }

                        GetDictResult(unQualifyUnitCountByArea, unitArea, ProductFactorycount);
                        //if (unQualifyUnitCountByArea.TryGetValue(unitArea, out areaUnqualifyUnitCount))
                        //{
                        //    unQualifyUnitCountByArea[unitArea] = areaUnqualifyUnitCount + ProductFactorycount;
                        //}
                        //else
                        //{
                        //    unQualifyUnitCountByArea.Add(unitArea, ProductFactorycount);
                        //}

                        GetDictResult(countUnitByArea, unitArea, ProductFactorycount);
                    }

                    else
                    {
                        var ProductFactorycount = 0;
                        var ProductFactoryBucks = conclutionBuck.Terms("ProductFactory");
                        foreach (var ProductFactoryBuck in ProductFactoryBucks.Buckets)
                        {
                            ProductFactorycount++;
                        }

                        GetDictResult(countUnitByArea, unitArea, ProductFactorycount);

                    }
                }


                //int areaUnitCount = 0;
                //if (countUnitByArea.TryGetValue(unitArea, out areaUnitCount))  //如果已经存在本区域的数据，则在本区域数据上加1，不存在则加入本字典
                //{
                //    countUnitByArea[unitArea] = areaUnitCount++;
                //}
                //else
                //{
                //    countUnitByArea.Add(unitArea, 1);
                //}
            }
        }

        /// <summary>
        /// 对数据字典的操作
        /// </summary>
        /// <param name="Dict">需要操作的数据字典</param>
        /// <param name="unitArea">所在地区</param>
        /// <param name="count">新增的数量</param>
        private static void GetDictResult(Dictionary<string, int> Dict, string unitArea, int count)
        {
            int areaUnitCount = 0;
            if (Dict.TryGetValue(unitArea, out areaUnitCount))  //如果已经存在本区域的数据，则在本区域数据上加上统计出来的机构数量，不存在则加入本字典
            {
                Dict[unitArea] = areaUnitCount + count;
            }
            else
            {
                Dict.Add(unitArea, count);
            }
        }


        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="searchModel">查询参数</param>
        /// <param name="fileFormat">文件格式,2003/2007</param>
        /// <returns>用于下载的Excel文件内容</returns>
        public ActionResult Export(SysSearchModel searchModel, int? fileFormat)
        {
            searchModel.GroupType = "Custom";
            var response = GetStatisData(searchModel);
            bool xlsx = (fileFormat ?? 2007) == 2007;
            ExcelExporter ee = new ExcelExporter("材料动态分析表", xlsx);
            ee.SetColumnTitles("序号, 地区, 报告总数, 不合格数量, 不合格数量占比, 生产厂家总数, 不合格生产商家数量, 不合格生产厂家占比 ");
            if (response.IsValid)
            {
                var bucks = response.Aggs.Terms("Custom").Buckets;
                //countByArea 地区报告总数   unQualifyCount 地区报告不合格总数  countUnitByArea 地区厂家总数   unQualifyUnitCountByArea 地区厂家不合格总数
                Dictionary<string, int> countByArea, unQualifyCount, countUnitByArea, unQualifyUnitCountByArea;
                GetSearchResult(bucks, out countByArea, out unQualifyCount, out countUnitByArea, out unQualifyUnitCountByArea);
                var index = 1;
                foreach (var item in countByArea)
                {
                    var unQualifyCountRow = 0;
                    var unitCountByArea = 0;
                    var unqualifyUnitCount = 0;
                    var isUnqualify = unQualifyCount.TryGetValue(item.Key, out unQualifyCountRow); //isUnqualify 标识是否有不合格报告
                    var isUnitCount = countUnitByArea.TryGetValue(item.Key, out unitCountByArea);
                    var isUnqualifyUnitCount = unQualifyUnitCountByArea.TryGetValue(item.Key, out unqualifyUnitCount);

                    ExcelRow row = ee.AddRow();
                    row.AddCell(index++);
                    row.AddCell(item.Key);
                    row.AddCell(item.Value);
                    row.AddCell(isUnqualify ? unQualifyCountRow : 0);
                    row.AddCell(item.Value == 0 ? "0" : Math.Round(((unQualifyCountRow * 1.00 / item.Value) * 100), 2).ToString() + "%");
                    row.AddCell(unitCountByArea.ToString());
                    row.AddCell(unqualifyUnitCount.ToString());
                    row.AddCell(unitCountByArea == 0 ? "0" : Math.Round(((unqualifyUnitCount * 1.00 / unitCountByArea) * 100), 2).ToString() + "%");
                }
            }
            // 改动4：返回字节流
            return File(ee.GetAsBytes(), ee.MIME, ee.FileName);

        }

        private ISearchResponse<es_t_bp_item> GetStatisDetailData(SysSearchModel model)
        {
            model.modelType = SysSearchModelModelType.UnQualified;
         
            var filterQuery = GetFilterQuery(checkUnitService, itemNameService, model);

            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;

            var field = GetDefaultFields();
            ISearchResponse<es_t_bp_item> response = tbpitemESRep.Search(s => s.Source(t => t.Includes(field)).Sort(cs => cs.Descending(sd => sd.PRINTDATE)).From(pos).Size(count).
                       Query(filterQuery));

            return response;
        }
        public Func<FieldsDescriptor<es_t_bp_item>, IPromise<Fields>> GetDefaultFields()
        {
            return t => t.Fields(
                      f => f.SYSPRIMARYKEY,
                              f => f.CUSTOMID,
                              f => f.PROJECTNAME,
                              f => f.STRUCTPART,
                              f => f.ITEMNAME,
                              f => f.ITEMCHNAME,
                              f => f.ACSTIME,
                              f => f.ENTRUSTDATE,
                              f => f.CHECKDATE,
                              f => f.PRINTDATE,
                              f => f.SAMPLENUM,
                              f => f.REPORTNUM,
                              f => f.HAVREPORT,
                              f => f.ISCREPORT
                              //f => f.SUBITEMCODE
                             );
        }

        public ActionResult SearchGrid(SysSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            DhtmlxGrid grid = new DhtmlxGrid();
            var response = GetStatisDetailData(model);
            if (response.IsValid)
            {
                int totalCount = (int)response.Total;

                grid.AddPaging(totalCount, pos);

                int index = pos;
                var allInst = checkUnitService.GetAllCheckUnit();
                var allItems = itemNameService.GetAllItemName();
                var pkrReports = reportService.GetPkrReportNums(response.Documents);
                foreach (var item in response.Documents)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(item.SYSPRIMARYKEY);
                    row.AddCell((index + 1).ToString());
                    row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allInst, item.CUSTOMID));
                    row.AddCell(item.PROJECTNAME);
                    if (item.ITEMCHNAME.IsNullOrEmpty())
                    {
                        row.AddCell(itemNameService.GetItemCNNameFromAll(allItems, item.REPORTJXLB, item.ITEMNAME));
                    }
                    else
                    {
                        row.AddCell(item.ITEMCHNAME);
                    }

                    row.AddCell(GetUIDtString(item.PRINTDATE));
                    BuildReportNumRow(reportService, pkrReports, item, row);
                    row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                    index++;
                    grid.AddGridRow(row);
                }
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }
        protected string GetUIDtString(DateTime? dt, string format = "yyyy-MM-dd HH:mm")
        {
            if (dt.HasValue)
            {
                return dt.Value.ToString(format);
            }
            else
            {
                return string.Empty;
            }

        }
        protected void BuildReportNumRow(IReportService reportService, Dictionary<string, int> pkrReports, es_t_bp_item item, DhtmlxGridRow row)
        {
            if (pkrReports != null
                && pkrReports.Count > 0)
            {

                var pkrReportNum = reportService.GetCryptPkrReportNumFromDict(pkrReports, item);

                if (pkrReportNum.IsNullOrEmpty())
                {
                    row.AddLinkJsCell(item.REPORTNUM, string.Empty);
                }
                else
                {
                    row.AddLinkJsCell(item.REPORTNUM, "getPKRReportView(\"{0}\")".Fmt(pkrReportNum));
                }

            }
            else
            {
                row.AddLinkJsCell(item.REPORTNUM, string.Empty);
            }
        }

        /// <summary>
        /// 画过去一年的厂家不合格报告折线图
        /// </summary>
        /// <returns></returns>
        public ActionResult DrawBrokenLineImage(SysSearchModel model)
        {
            HomeEchartData<EchartNameValue> data = new HomeEchartData<EchartNameValue>();
            data.records = new List<EchartNameValue>();

      
            model.IsMonthAgg = true;
            var filterQuery = GetFilterQuery(checkUnitService, itemNameService, model);
            return Content(string.Empty);
        }



        public ActionResult ToolBar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("PrintStats", "打印统计表") { Img = "fa fa-files-o", Imgdis = "fa fa-files-o" });
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("ExportStats", "导出统计表") { Img = "fa fa-files-o", Imgdis = "fa fa-files-o" });

            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        /// <summary>
        /// 获取过滤条件
        /// </summary>
        /// <param name="checkUnitService">检测机构</param>
        /// <param name="itemNameService">检测项目</param>
        /// <param name="model"></param>
        /// <param name="userCustoms">用户自定义检测机构</param>
        /// <param name="userItems">用户自定义检测项目</param>
        /// <returns></returns>
        protected Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> GetFilterQuery(ICheckUnitService checkUnitService,
            IItemNameService itemNameService,
            SysSearchModel model)
        {
            #region 过滤条件

            Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> filterQuery = q =>
            {
                string dtFormatStr = "yyyy-MM-dd'T'HH:mm:ss";


                QueryContainer initQuery = q.Exists(qe => qe.Field(qef => qef.SYSPRIMARYKEY));

                #region 对时间的处理
                //model.SearchType 只有从综合统计过来才有值，兼容综合统计部分
                if (model.SearchType.IsNullOrEmpty())
                {
                    if (model.StartDt.HasValue || model.EndDt.HasValue)
                    {
                        string startDtStr = model.StartDt.HasValue ? model.StartDt.Value.ToString(dtFormatStr) : string.Empty;
                        string endDtStr = model.EndDt.HasValue ? model.EndDt.Value.AddDays(1).ToString(dtFormatStr) : string.Empty;
                        if (model.DtType == "EntrustDt")
                        {
                            if (!startDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.ENTRUSTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                            }
                            if (!endDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.ENTRUSTDATE).LessThan(DateMath.FromString(endDtStr)));
                            }
                        }
                        else if (model.DtType == "CheckDt")
                        {
                            if (!startDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.CHECKDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                            }
                            if (!endDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.CHECKDATE).LessThan(DateMath.FromString(endDtStr)));
                            }
                        }
                        else if (model.DtType == "ReportDt")
                        {
                            if (!startDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.PRINTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                            }
                            if (!endDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.PRINTDATE).LessThan(DateMath.FromString(endDtStr)));
                            }
                        }
                        else if (model.DtType == "APPROVEDATE")
                        {
                            if (!startDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.APPROVEDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                            }
                            if (!endDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.APPROVEDATE).LessThan(DateMath.FromString(endDtStr)));
                            }
                        }
                        else if (model.DtType == "UploadDt")
                        {
                            if (!startDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.UPLOADTIME).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                            }
                            if (!endDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.UPLOADTIME).LessThan(DateMath.FromString(endDtStr)));
                            }
                        }
                        else if (model.DtType == "ENTRUSTDATE")
                        {
                            if (!startDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.ENTRUSTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                            }
                            if (!endDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.ENTRUSTDATE).LessThan(DateMath.FromString(endDtStr)));
                            }
                        }
                        else  //如果dtType为空值或者其他值，则按照报告时间进行查询
                        {
                            if (!startDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.PRINTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                            }
                            if (!endDtStr.IsNullOrEmpty())
                            {
                                initQuery = initQuery && +q.DateRange(d => d.Field(f => f.PRINTDATE).LessThan(DateMath.FromString(endDtStr)));
                            }
                        }
                    }
                }
                else
                {
                    
                    string startDtStr = string.Empty;
                    string endDtStr = string.Empty;
                    var date = DateTime.Today;
                    switch (model.SearchType)
                    {
                        case "Search":
                            if (model.StartDt.HasValue || model.EndDt.HasValue)
                            {
                                startDtStr = model.StartDt.HasValue ? model.StartDt.Value.ToString(dtFormatStr) : string.Empty;
                                endDtStr = model.EndDt.HasValue ? model.EndDt.Value.AddDays(1).ToString(dtFormatStr) : string.Empty;
                            }
                            break;
                        case "day":
                            startDtStr = DateTime.Today.ToString(dtFormatStr);
                            endDtStr = DateTime.Today.AddDays(1).ToString(dtFormatStr);
                            break;
                        case "week":
                            DateTime startWeek = date.AddDays(1 - Convert.ToInt32(date.DayOfWeek.ToString("d"))); //本周周一
                            DateTime endWeek = startWeek.AddDays(7);
                            startDtStr = startWeek.ToString(dtFormatStr);
                            endDtStr = endWeek.ToString(dtFormatStr);
                            break;
                        case "month":
                            DateTime startMonth = date.AddDays(1 - date.Day);  //本月月初  
                            DateTime endMonth = startMonth.AddMonths(1);  //本月月末  
                            // DateTime startMonth = new DateTime(2016, 09, 01);  //本月月初  
                            //DateTime endMonth = startMonth.AddMonths(1);  //本月月末  
                            startDtStr = startMonth.ToString(dtFormatStr);
                            endDtStr = endMonth.ToString(dtFormatStr);
                            break;
                        case "quarter":
                            DateTime startQuarter = date.AddMonths(0 - (date.Month - 1) % 3).AddDays(1 - date.Day);  //本季度初  
                            DateTime endQuarter = startQuarter.AddMonths(3);
                            startDtStr = startQuarter.ToString(dtFormatStr);
                            endDtStr = endQuarter.ToString(dtFormatStr);
                            break;
                        case "year":
                            DateTime startYear = new DateTime(date.Year, 1, 1);  //本年年初  
                            DateTime endYear = new DateTime(date.Year, 12, 31).AddDays(1);  //本年年末  
                            startDtStr = startYear.ToString(dtFormatStr);
                            endDtStr = endYear.ToString(dtFormatStr);
                            break;
                    }
                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.ENTRUSTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                    }
                    if (!endDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.ENTRUSTDATE).LessThanOrEquals(DateMath.FromString(endDtStr)));
                    }
                }
                #endregion

                if (!model.ReportStatus.IsNullOrEmpty())
                {
                    if (model.ReportStatus == "0")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(printedPhrase));
                    }
                    else if (model.ReportStatus == "1")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(collectedPhrase));//收样
                    }
                    else if (model.ReportStatus == "2")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(testedPhrase));//检测
                    }
                }

                #region 对综合查询数据状态的处理
                if (!model.DataState.IsNullOrEmpty())
                {
                    if (model.DataState == "0")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(collectedPhrase));//收样
                    }
                    else if (model.DataState == "1")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(checkedPhrase));//复核
                    }
                    else if (model.DataState == "2")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(testedPhrase));//检测
                    }
                    else if (model.DataState == "3")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(proofreadPhrase));//校核
                    }
                    else if (model.DataState == "4")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(verifyedPhrase));//审核
                    }
                    else if (model.DataState == "5")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(approvaledPhrase));//批准
                    }
                    else if (model.DataState == "6")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(printedPhrase));//打印
                    }
                    else if (model.DataState == "7")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(fileedPhrase));//归档
                    }
                    else if (model.DataState == "8")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(giveOutPhrase));//发放
                    }
                    else if (model.DataState == "9")
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(canceledPhrase));//作废
                    }

                }
                #endregion

                if (model.GetStartDt.HasValue)
                {
                    var getStartDtStr = model.GetStartDt.Value.ToString(dtFormatStr);
                    initQuery = initQuery && q.DateRange(d => d.Field(f => f.ACSTIME).GreaterThanOrEquals(DateMath.FromString(getStartDtStr)));
                }

                if (model.GetEndDt.HasValue)
                {
                    var gteEndDtStr = model.GetEndDt.Value.ToString(dtFormatStr);
                    initQuery = initQuery && q.DateRange(d => d.Field(f => f.ACSTIME).LessThan(DateMath.FromString(gteEndDtStr)));
                }



                if (model.ReportStartDt.HasValue)
                {
                    var reportStartDt = model.ReportStartDt.Value.ToString(dtFormatStr);
                    initQuery = initQuery && q.DateRange(d => d.Field(f => f.PRINTDATE).GreaterThanOrEquals(DateMath.FromString(reportStartDt)));
                }
                if (model.ReportEndDt.HasValue)
                {
                    var reportEndDt = model.ReportEndDt.Value.ToString(dtFormatStr);
                    initQuery = initQuery && q.DateRange(d => d.Field(f => f.PRINTDATE).LessThan(DateMath.FromString(reportEndDt)));
                }

                if (!string.IsNullOrWhiteSpace(model.Area))
                {
                    var areas = model.Area.Split(',').ToList();

                    Dictionary<string, string> CheckUnits = checkUnitService.GetUnitByArea(areas);
                    if (CheckUnits.Count > 0)
                    {
                        initQuery = initQuery && +q.Terms(t => t.Field(f => f.CUSTOMID).Terms(CheckUnits.Keys.ToList()));
                    }
                }

                if (!model.CustomId.IsNullOrEmpty())
                {
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.CUSTOMID).Value(model.CustomId));
                }

                if (!string.IsNullOrWhiteSpace(model.CheckInstID))
                {
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.CUSTOMID).Value(model.CheckInstID));
                }

                if (!string.IsNullOrWhiteSpace(model.ProjectName))
                {
                    //initQuery = initQuery && q.QueryString(m => m.DefaultField(f => f.PROJECTNAME[0].Suffix("PROJECTNAMERAW")).Query("{0}{1}{0}".Fmt("*", model.ProjectName)));
                    initQuery = initQuery && q.Wildcard(w => w.Field(wf => wf.PROJECTNAME).Value("{0}{1}{0}".Fmt("*", model.ProjectName)));
                }

                if (!model.TestCategories.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Term(qt => qt.Field(qtf => qtf.REPORTJXLB).Value(model.TestCategories));
                }

                if (!string.IsNullOrWhiteSpace(model.EntrustUnit))
                {
                    initQuery = initQuery && q.Match(m => m.Field(f => f.ENTRUSTUNIT).Query(model.EntrustUnit));
                }

                if (!model.JDType.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Terms(m => m.Field(f => f.EXPLAIN).Terms(model.JDType));
                }

                if (!string.IsNullOrWhiteSpace(model.Num))
                {
                    //编号查询，支持模糊
                    initQuery = initQuery && (q.Term(qm => qm.Field(qmf => qmf.SAMPLENUM).Value(model.Num))
                                          || q.QueryString(qrm => qrm.DefaultField(qrmf => qrmf.SAMPLENUM).Query("{0}{1}{0}".Fmt("*", model.Num)))
                                          || q.Term(qe => qe.Field(qef => qef.REPORTNUM).Value(model.Num))
                                          || q.QueryString(qrm => qrm.DefaultField(qrmf => qrmf.REPORTNUM).Query("{0}{1}{0}".Fmt("*", model.Num)))
                                          || q.Term(qr => qr.Field(qrf => qrf.ENTRUSTNUM).Value(model.Num))
                                          || q.QueryString(qrm => qrm.DefaultField(qrmf => qrmf.ENTRUSTNUM).Query("{0}{1}{0}".Fmt("*", model.Num))));


                }

                if (!string.IsNullOrWhiteSpace(model.ReportNum))
                {
                    //initQuery = initQuery && q.QueryString(m => m.DefaultField(f => f.REPORTNUM).Query("{0}{1}{0}".Fmt("*", model.ReportNum)));
                    initQuery = initQuery && q.Wildcard(w => w.Field(f => f.REPORTNUM).Value("{0}{1}{0}".Fmt("*", model.ReportNum)));
                }

                if (model.IsCType.HasValue && model.IsCType.Value == 1)
                {
                    initQuery = initQuery && +q.Range(qtm => qtm.Field(qtmf => qtmf.ISCREPORT).GreaterThanOrEquals(1));
                }

                if (model.IsChanged.HasValue && model.IsChanged.Value != -1)
                {
                    if (model.IsChanged.Value == 1)
                    {
                        initQuery = initQuery && +q.Range(r => r.Field(f => f.HAVELOG).GreaterThanOrEquals(1));
                    }
                    else if (model.IsChanged.Value == 0)
                    {
                        initQuery = initQuery && (!q.Exists(e => e.Field(f => f.HAVELOG)) || q.Term(t => t.Field(f => f.HAVELOG).Value(0)));
                    }
                }

                if (model.HasArc.HasValue && model.HasArc.Value != -1)
                {
                    if (model.HasArc.Value == 1)
                    {
                        initQuery = initQuery && +q.Range(r => r.Field(f => f.HAVEACS).GreaterThanOrEquals(1)) && q.Exists(r => r.Field(f => f.ACSTIME));
                    }
                    else if (model.HasArc.Value == 0)
                    {
                        initQuery = initQuery && (!q.Exists(e => e.Field(f => f.HAVEACS)) || q.Term(t => t.Field(f => f.HAVEACS).Value(0)));
                    }
                }

                if (!string.IsNullOrWhiteSpace(model.CheckStatus) && model.CheckStatus != "A")
                {
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.CONCLUSIONCODE).Value(model.CheckStatus));
                }
                if (!string.IsNullOrEmpty(model.SampleNum))
                {
                    initQuery = initQuery && q.QueryString(qrm => qrm.DefaultField(qrmf => qrmf.SAMPLENUM).Query("{0}{1}{0}".Fmt("*", model.SampleNum)));
                    //+ q.Term(t => t.Field(f => f.SAMPLENUM).Value(model.SampleNum));
                }

                if (model.IsReport.HasValue)
                {
                    if (model.IsReport.Value == 1)
                    {
                        initQuery = initQuery && q.Term(qt => qt.Field(qtf => qtf.HAVREPORT).Value(1));
                    }
                    else if (model.IsReport.Value == 0)
                    {
                        initQuery = initQuery && (!q.Exists(t => t.Field(tf => tf.HAVREPORT)) || q.Term(tt => tt.Field(ttt => ttt.HAVREPORT).Value(0)));
                    }
                }

                //委托编号
                if (!model.EntrustNum.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.QueryString(qrm => qrm.DefaultField(qrmf => qrmf.ENTRUSTNUM).Query("{0}{1}{0}".Fmt("*", model.EntrustNum)));

                }

                ////样品编号
                //if (!model.SAMPLENUM.IsNullOrEmpty())
                //{
                //    initQuery = initQuery && q.QueryString(qrm => qrm.DefaultField(qrmf => qrmf.SAMPLENUM).Query("{0}{1}{0}".Fmt("*", model.SAMPLENUM)));
                //}

                if (!model.ReportNumPrefix.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Term(m => m.Field(f => f.REPORMNUMWITHOUTSEQ).Value(model.ReportNumPrefix));
                }



                #region 兼容不同控制器
                //不是综合查询就只显示已打印,统计以及不需要登录的综合查询也查询所有的数据
                if (model.modelType != SysSearchModelModelType.TotalSearch && model.modelType != SysSearchModelModelType.CheckStatis && model.modelType != SysSearchModelModelType.TotalSearchView)
                {
                    if ("1" == System.Configuration.ConfigurationManager.AppSettings["ShowPrinted"])
                    {
                        initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(printedPhrase));
                    }
                }

                if (!model.SiderbarType.IsNullOrEmpty())
                {
                    if (model.Group == "1")
                    {
                        initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.PROJECTNAME).Value(model.SiderbarType));

                    }
                    else
                    {
                        //initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.ITEMNAME).Value(model.SiderbarType));
                        if (model.SiderbarType.Contains('|'))
                        {
                            var keys = model.SiderbarType.Split('|');
                            var typeCode = keys[0];
                            var itemCode = keys[1];
                            initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.ITEMNAME).Value(itemCode)) && q.Term(tt => tt.Field(ttt => ttt.REPORTJXLB).Value(typeCode));
                        }
                    }
                }


                if (model.modelType == SysSearchModelModelType.NoAcsSearch)
                {
                    List<string> needAcsItems = itemNameService.GetAcsItemNames();
                    initQuery = initQuery && +q.Terms(qts => qts.Field(qtsf => qtsf.ITEMNAME).Terms(needAcsItems))
                                         && (q.Range(qt => qt.Field(qtf => qtf.HAVEACS).LessThanOrEquals(0)) //字段小于等于0
                                              || !q.Exists(qe => qe.Field(qef => qef.HAVEACS))); //无此字段  
                }

                if (model.modelType == SysSearchModelModelType.UnQualified)
                {
                    //不合格报告的条件
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.CONCLUSIONCODE).Value("N"));
                }

                if (!model.itemkey.IsNullOrEmpty())
                {
                    //不合格报告中（材料动态分析中）点击查看某一项的动态分析表中使用
                    
                    if (model.itemkey.Length == 9)
                    {
                        var keys = model.itemkey.Split('|');
                        var typeCode = keys[0];
                        var itemCode = keys[1];
                        initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.REPORTJXLB).Value(typeCode));
                        initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.ITEMNAME).Value(itemCode));
                    }
                    else
                    {
                        initQuery = initQuery && +q.Term(t => t.Field(f => f.ITEMNAME).Value(model.itemkey.Trim()));
                    }
                }

                if (model.modelType == SysSearchModelModelType.AcsTimeStatisc)
                {
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.HAVEACS).Value(1)) && +q.Exists(e => e.Field(f => f.ACSTIME)) && +q.Exists(e => e.Field(f => f.CHECKDATE));

                    initQuery = initQuery && (q.Script(qs => qs.Inline("doc['CHECKDATE'].date.year > doc['ACSTIME'].date.year "))
                                           || q.Script(qs => qs.Inline("doc['CHECKDATE'].date.year ==  doc['ACSTIME'].date.year && doc['CHECKDATE'].date.monthOfYear > doc['ACSTIME'].date.monthOfYear  "))
                                           || q.Script(qs => qs.Inline("doc['CHECKDATE'].date.year == doc['ACSTIME'].date.year && doc['CHECKDATE'].date.monthOfYear == doc['ACSTIME'].date.monthOfYear && doc['CHECKDATE'].date.dayOfYear > doc['ACSTIME'].date.dayOfYear")));

                }

                if (model.modelType == SysSearchModelModelType.ReportdataAnalysis)
                {
                    string startDtStr = model.StartDt.HasValue ? model.StartDt.Value.ToString(dtFormatStr) : "";
                    string endDtStr = model.EndDt.HasValue ? model.EndDt.Value.AddDays(1).ToString(dtFormatStr) : "";
                    if (!(startDtStr.IsNullOrEmpty() && endDtStr.IsNullOrEmpty()))
                    {
                        initQuery = q.DateRange(d => d.Field(f => f.CHECKDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)).LessThanOrEquals(DateMath.FromString(endDtStr)));
                    }
                }

                if (model.modelType == SysSearchModelModelType.NoDataUploadAllDetails)
                {
                    initQuery = initQuery && (!q.Term(qtf => qtf.Field(t => t.HAVREPORT).Value(1)));
                }

                //if (model.modelType == SysSearchModelModelType.ReportCategory)
                //{
                //    initQuery = initQuery && q.Term(qt => qt.Field(qtf => qtf.SUBITEMCODE).Value(model.SubItemCodeRaw));
                //    initQuery = initQuery && q.Term(qt => qt.Field(qtf => qtf.PARMCODE).Value(model.ParamCodeRaw));
                //}

                //兼容某些需要查询被修改报告
                if (model.modelType == SysSearchModelModelType.ModifyReport)
                {
                    initQuery = initQuery && +(q.Term(qtf => qtf.Field(t => t.HAVELOG).Value(1)));
                }

                #endregion


                if (model.IsMonthAgg)
                {
                    DateTime DtNow = DateTime.Now;
                    string endDtStr = DtNow.ToString(dtFormatStr);
                    DateTime DtStart = DtNow.AddMonths(-12);
                    string startDtStr = new DateTime(DtStart.Year, DtStart.Month, 1).ToString(dtFormatStr);
                    initQuery = initQuery && +q.DateRange(qdr => qdr.Field(qdrf => qdrf.ENTRUSTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)))
                    && q.DateRange(qddr => qddr.Field(qddrf => qddrf.ENTRUSTDATE).LessThanOrEquals(DateMath.FromString(endDtStr)));
                }

                #region 兼容不合格报告
                if (!model.ItemName.IsNullOrEmpty())
                {
                    if (model.ItemName.Length == 9)
                    {
                        var keys = model.ItemName.Split('|');
                        var typeCode = keys[0];
                        var itemCode = keys[1];
                        initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.REPORTJXLB).Value(typeCode));
                        initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.ITEMNAME).Value(itemCode));
                    }
                    else
                    {
                        initQuery = initQuery && +q.Term(t => t.Field(f => f.ITEMNAME).Value(model.ItemName));
                    }
        
                }

                if (!string.IsNullOrWhiteSpace(model.ProjectNameRaw))
                {
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.PROJECTNAME[0].Suffix("PROJECTNAMERAW")).Value(model.ProjectNameRaw));
                }
                #endregion
                if (!model.CheckItem.IsNullOrEmpty())
                {
                    if (model.CheckItem.Length == 8)
                    {
                        var typeCode = model.CheckItem.Substring(0, 4);
                        var itemCode = model.CheckItem.Substring(4, 4);
                        initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.REPORTJXLB).Value(typeCode));
                        initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.ITEMNAME).Value(itemCode));
                    }
                    else
                    {
                        initQuery = initQuery && +q.Term(t => t.Field(f => f.ITEMNAME).Value(model.CheckItem));
                    }
                }
                //if (!model.TestCategories.IsNullOrEmpty())
                //{
                //    var data = itemNameService.GetItemNameByCategory(model.TestCategories);
                //    if (data.Count > 0)
                //    {
                //        initQuery = initQuery && q.Terms(qt => qt.Field(qtf => qtf.ITEMNAME).Terms(data.Keys.ToList()));
                //    }
                //}


                return initQuery;
            };
            #endregion


            return filterQuery;
        }
    }
}
