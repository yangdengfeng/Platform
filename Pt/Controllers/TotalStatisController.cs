using PkpmGX.Architecture;
using PkpmGX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Dhtmlx.Model.Toolbar;
using Nest;
using Pkpm.Entity.ElasticSearch;
using ServiceStack;
using Pkpm.Framework.Repsitory;
using Dhtmlx.Model.Grid;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.ItemNameCore;

namespace PkpmGX.Controllers
{
    public class TotalStatisController : PkpmController
    {
        IESRepsitory<es_t_bp_item> tbpitemESRep;
        IItemNameService itemNameService;
        ICheckUnitService checkUnitService;
        string UnQualiKey = "UnQualified";
        string ModifyKey = "Modify";
        string AcsKey = "Acs";
        public TotalStatisController(IESRepsitory<es_t_bp_item> tbpitemESRep,
             ICheckUnitService checkUnitService,
              IItemNameService itemNameService,
        IUserService userService) : base(userService)
        {
            this.tbpitemESRep = tbpitemESRep;
            this.checkUnitService = checkUnitService;
            this.itemNameService = itemNameService;
        }

        // GET: TotalStatis
        public ActionResult Index()
        {
            TotalStatisViewModel model = new TotalStatisViewModel();
            model.Day = DateTime.Now.ToString("yyyy-MM-dd");
            var week = Convert.ToInt32(DateTime.Now.DayOfWeek) < 1 ? 7 : Convert.ToInt32(DateTime.Now.DayOfWeek)-1;
            model.Week = DateTime.Now.AddDays(-week).ToString("yyyy-MM-dd");
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            model.Month = Convert.ToDateTime(year + "-" + month + "-" + "1").ToString("yyyy-MM-dd");
            model.Year = Convert.ToDateTime(year + "-" + "1" + "-" + "1").ToString("yyyy-MM-dd");
            var a=(month-1)/3;
            if (a == 0)
            {
                model.Quarter=Convert.ToDateTime(year+"-"+1+"-"+1).ToString("yyyy-MM-dd");
            }else if (a==1)
            {
                model.Quarter = Convert.ToDateTime(year + "-" + 4 + "-" + 1).ToString("yyyy-MM-dd");
            }
            else if (a == 2)
            {
                model.Quarter = Convert.ToDateTime(year + "-" + 7 + "-" + 1).ToString("yyyy-MM-dd");
            }
            else if (a == 3)
            {
                model.Quarter = Convert.ToDateTime(year + "-" + 10 + "-" + 1).ToString("yyyy-MM-dd");
            }
            return View(model);
        }

        // GET: TotalStatis/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TotalStatis/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult GridSearch(TotalStatisSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            var response = GetGridResult(model);
            DhtmlxGrid grid = new DhtmlxGrid();
            if (response.IsValid)
            {
                int totalCount = (int)response.Total;
                grid.AddPaging(totalCount, pos);
                int index = pos;
                var allInst = checkUnitService.GetAllCheckUnit();
                var allItems = itemNameService.GetAllItemName();
                foreach (var item in response.Documents)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(item.SYSPRIMARYKEY);
                    row.AddCell(index + 1);
                    row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allInst, item.CUSTOMID));
                    row.AddCell(item.PROJECTNAME);
                  
                    if (itemNameService.IsCReport(item.REPORTJXLB, item.ITEMNAME))
                    {
                       row.AddCell(item.ITEMCHNAME.IsNullOrEmpty() ?itemNameService.GetItemCNNameFromAll(allItems,item.REPORTJXLB, item.ITEMNAME) : item.ITEMCHNAME);
                    }
                    else
                    {
                        row.AddCell(item.ITEMCHNAME.IsNullOrEmpty() ?itemNameService.GetItemCNNameFromAll(allItems,item.REPORTJXLB, item.ITEMNAME) : item.ITEMCHNAME);
                    }
                    row.AddCell(GetUIDtString(item.PRINTDATE));
                   
                    row.AddCell(item.REPORTNUM);
                    row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                    index++;
                    grid.AddGridRow(row);
                }
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }


     

        public ActionResult Search(TotalStatisSearchModel model)
        {
            var response = GetResult(model);
            DhtmlxGrid grid = new DhtmlxGrid();
            if (response.IsValid)
            {
                var buckets = response.Aggs.Terms("CUSTOMID").Buckets;
                var allInsts = checkUnitService.GetAllCheckUnit();
                int i = 1;
                foreach (var bucket in buckets)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(bucket.Key);
                    string unqualiCount = string.Empty;
                    string modifyCount = string.Empty;
                    string AcsCount = string.Empty;
                    var unqualiBucket = bucket.Terms(UnQualiKey);
                    var modifyBucket = bucket.Terms<int>(ModifyKey);
                    var AcsBucket = bucket.Terms<int>(AcsKey);
                    var customId = bucket.Key;
                    var totalCount = bucket.DocCount;

                    foreach (var item in unqualiBucket.Buckets)
                    {
                        if (item.Key.ToString() == "N")
                        {
                            unqualiCount = item.DocCount.ToString();
                        }
                    }
                    foreach (var item in modifyBucket.Buckets)
                    {
                        if (item.Key.ToString() == "1")
                        {
                            modifyCount = item.DocCount.ToString();
                        }
                    }
                    foreach (var item in AcsBucket.Buckets)
                    {
                        if (item.Key.ToString() == "1")
                        {
                            AcsCount = item.DocCount.ToString();
                        }
                    }

                    row.AddCell(i.ToString());
                    var customName = checkUnitService.GetCheckUnitByIdFromAll(allInsts, customId);
                    row.AddCell(customName);
                    row.AddCell(totalCount.ToString());
                    if (unqualiCount.IsNullOrEmpty())
                    {
                        row.AddCell("0");
                    }
                    else
                    {
                        row.AddLinkJsCell(unqualiCount, "unqualiKeyGrid(\"{0}\",\"{1}\")".Fmt(customId, customName));
                    }
                    if (modifyCount.IsNullOrEmpty())
                    {
                        row.AddCell("0");
                    }
                    else
                    {
                        row.AddLinkJsCell(modifyCount, "modifyKeyGrid(\"{0}\",\"{1}\")".Fmt(customId, customName));
                    }
                    if (AcsCount.IsNullOrEmpty())
                    {
                        row.AddCell("0");
                    }
                    else
                    {
                        row.AddLinkJsCell(AcsCount, "AcsKeyGrid(\"{0}\",\"{1}\")".Fmt(customId, customName));
                    }
                    grid.AddGridRow(row);
                    i++;
                }
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ISearchResponse<es_t_bp_item> GetResult(TotalStatisSearchModel model)
        {
            var filterQuery = GetFilterQuery(model);
            return tbpitemESRep.Search(s => s.Size(0).Query(filterQuery).Aggregations(af => af.Terms("CUSTOMID", item => item.Field(iif => iif.CUSTOMID)
                                                             .Aggregations(aaf => aaf.Terms(UnQualiKey, adn => adn.Field(aaa => aaa.CONCLUSIONCODE))
                                                                     .Terms(ModifyKey, adm => adm.Field(aadm => aadm.HAVELOG))
                                                                     .Terms(AcsKey,afnm=>afnm.Field(aaaa=>aaaa.HAVEACS))).Size(1000))));
        }

        private ISearchResponse<es_t_bp_item> GetGridResult(TotalStatisSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            var filterQuery = GetFilterQuery(model);
            return tbpitemESRep.Search(s => s.Query(filterQuery).Size(count).From(pos));
        }

        public Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> GetFilterQuery(TotalStatisSearchModel model)
        {
            Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> filterQuery = q =>
            {
                string dtFormatStr = "yyyy-MM-dd'T'HH:mm:ss";
                string startDtStr = model.StartDt.HasValue ? model.StartDt.Value.ToString(dtFormatStr) : string.Empty;
                string endDtStr = model.EndDt.HasValue ? model.EndDt.Value.AddDays(1).ToString(dtFormatStr) : string.Empty;
                QueryContainer initQuery = q.Exists(qe => qe.Field(qef => qef.SYSPRIMARYKEY));
                if(model.DtType== "ENTRUSTDATE")
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
                else if(model.DtType== "CHECKDATE")
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
                if (!model.CheckInstID.IsNullOrEmpty())
                {
                    initQuery = initQuery && +q.Term(s => s.Field(ss => ss.CUSTOMID).Value(model.CheckInstID));
                }
                if (!model.SearchType.IsNullOrEmpty())
                {
                    if(model.SearchType== "modify")
                    {
                        //HAVELOG == 1;
                        initQuery = initQuery && +q.Term(s => s.Field(ss => ss.HAVELOG).Value("1"));
                    }
                    if(model.SearchType== "unquali")
                    {
                        initQuery = initQuery && +q.Term(s => s.Field(ss => ss.CONCLUSIONCODE).Value("N"));
                    }
                    if (model.SearchType == "acs")
                    {
                        initQuery = initQuery && +q.Term(s => s.Field(ss => ss.HAVEACS).Value("1"));
                    }
                }
                return initQuery;
            };
            return filterQuery;

        }

        public ActionResult Toolbar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Export", "导出") { Img = "fa fa-clone", Imgdis = "fa fa-clone" });
            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }


        // POST: TotalStatis/Create
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

        // GET: TotalStatis/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TotalStatis/Edit/5
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

        // GET: TotalStatis/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TotalStatis/Delete/5
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
    }
}
