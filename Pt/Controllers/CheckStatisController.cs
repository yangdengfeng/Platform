using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Entity;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.ItemNameCore;
using Nest;
using Pkpm.Entity.ElasticSearch;
using Pkpm.Framework.Repsitory;
using Dhtmlx.Model.Sidebar;
using Dhtmlx.Model.Grid;
using ServiceStack;
using Pkpm.Core.ReportCore;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class CheckStatisController : PkpmController
    {
        ICheckUnitService checkUnitService;
        IItemNameService itemNameService;
        IReportService reportService;
        IESRepsitory<es_t_bp_item> tbpESRep;
        string aggKey = "statis";
        public CheckStatisController(ICheckUnitService checkUnitService, IItemNameService itemNameService, IReportService reportService, IESRepsitory<es_t_bp_item> tbpESRep, IUserService userService) : base(userService)
        {
            this.checkUnitService = checkUnitService;
            this.itemNameService = itemNameService;
            this.reportService = reportService;
            this.tbpESRep = tbpESRep;
        }

        // GET: CheckStatis
        public ActionResult Index()
        {
            var date = DateTime.Now;
            var day = GetUIDtString(DateTime.Now, "yyyy-MM-dd");
            var week = GetUIDtString(date.AddDays(1 - Convert.ToInt32(date.DayOfWeek.ToString("d"))), "yyyy-MM-dd"); //本周周一
            var month = GetUIDtString(date.AddDays(1 - date.Day), "yyyy-MM-dd");
            var year = GetUIDtString(new DateTime(date.Year, 1, 1), "yyyy-MM-dd");
            ViewBag.Day = day;
            ViewBag.Week = week;
            ViewBag.Month = month;
            ViewBag.Year = year;
            return View();
        }

        // GET: CheckStatis/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CheckStatis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckStatis/Create
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

        // GET: CheckStatis/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckStatis/Edit/5
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

        // GET: CheckStatis/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckStatis/Delete/5
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
        private ISearchResponse<es_t_bp_item> GetSearchResponse(SysSearchModel model)
        {
            var filter = GetFilterQuery(checkUnitService, itemNameService, model, new Dictionary<string, string>(), new Dictionary<string, string>());
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            return tbpESRep.Search(t => t.Query(filter).From(pos).Size(count)
                            .Source(tt => tt.Includes(tst => tst.Fields(
                                  ttt => ttt.SYSPRIMARYKEY,
                                  ttt => ttt.ENTRUSTUNIT,
                                  ttt => ttt.CONSTRACTUNIT,
                                  ttt => ttt.CUSTOMID,
                                  ttt => ttt.PROJECTNAME,
                                  ttt => ttt.STRUCTPART,
                                  ttt => ttt.REPORTJXLB,
                                  ttt => ttt.ITEMNAME,
                                  ttt => ttt.CHECKTYPE,
                                  ttt => ttt.ENTRUSTDATE,
                                  ttt => ttt.CHECKDATE,
                                  ttt => ttt.PRINTDATE,
                                  ttt => ttt.REPORTNUM,
                                  ttt => ttt.CHECKCONCLUSION
                                  ))));
        }

        private ISearchResponse<es_t_bp_item> GetGridSearchResponse(SysSearchModel model)
        {
            var filter = GetFilterQuery(checkUnitService, itemNameService, model, new Dictionary<string, string>(), new Dictionary<string, string>());
            if (model.Group == "1")
            {
                return tbpESRep.Search(t => t.Size(0).Query(filter).Aggregations(tt => tt.Terms(aggKey, tst => tst.Field(tstt => tstt.PROJECTNAME).Size(1000))));
            }
            else
            {
                return tbpESRep.Search(t => t.Size(0).Query(filter).Aggregations(tt => tt.Terms(aggKey, tst => tst.Script("doc['REPORTJXLB'].value+'|'+doc['ITEMNAME'].value"))));//tst => tst.Field(tstt => tstt.ITEMNAME).Size(1000))));
            }
        }

        public ActionResult SiderBar(SysSearchModel model)
        {
            model.modelType = SysSearchModelModelType.CheckStatis;

            DhtmlxSidebar siderbar = new DhtmlxSidebar();

            var response = GetGridSearchResponse(model);

            if (response.IsValid)
            {
                var buckets = response.Aggs.Terms(aggKey).Buckets;

                var allItem = itemNameService.GetAllItemName();

                foreach (var bucket in buckets)
                {
                    if (model.Group == "1")
                    {
                        var key = bucket.Key;
                        var bubble = bucket.DocCount.ToString();
                        siderbar.AddSidebarItem(new DhtmlxSidebarItem(key, key, bubble));
                    }
                    else
                    {
                        var key = bucket.Key;
                        var ItemName = string.Empty;

                        if(key.Contains('|'))
                        {
                            var keys = key.Split('|');
                            var typeName = keys[0];
                            var itemCode = keys[1];
                            ItemName = itemNameService.GetItemCNNameFromAll(allItem, typeName, itemCode);
                        }

                        if (ItemName.IsNullOrEmpty())
                        {
                            continue;
                        }
                        var bubble = bucket.DocCount.ToString();
                        siderbar.AddSidebarItem(new DhtmlxSidebarItem(key, ItemName, bubble));
                    }
                }
            }
            string str = siderbar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }


        public ActionResult Search(SysSearchModel model)
        {
            model.modelType = SysSearchModelModelType.CheckStatis;
            var response = GetSearchResponse(model);

            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int index = pos + 1;

            DhtmlxGrid grid = new DhtmlxGrid();

            var allInsts = checkUnitService.GetAllCheckUnit();
            var allItems = itemNameService.GetAllItemName();

            if (response.IsValid)
            {
                int totalCount = (int)response.Total;
                grid.AddPaging(totalCount, pos);

                var pkrReports = reportService.GetPkrReportNums(response.Documents);

                foreach (var item in response.Documents)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(item.SYSPRIMARYKEY);
                    row.AddCell(index++);
                    row.AddCell(item.ENTRUSTUNIT);
                    row.AddCell(item.CONSTRACTUNIT);
                    row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allInsts, item.CUSTOMID));
                    row.AddCell(item.PROJECTNAME);
                    row.AddCell(item.STRUCTPART);
                    row.AddCell(itemNameService.GetItemCNNameFromAll(allItems, item.REPORTJXLB, item.ITEMNAME));
                    row.AddCell(item.CHECKTYPE);
                    row.AddCell(GetUIDtString(item.ENTRUSTDATE, "yyyy-MM-dd"));
                    row.AddCell(GetUIDtString(item.CHECKDATE, "yyyy-MM-dd"));
                    row.AddCell(GetUIDtString(item.PRINTDATE, "yyyy-MM-dd"));
                    BuildReportNumRow(reportService, pkrReports, item, row);
                    row.AddCell(item.CHECKCONCLUSION);
                    row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));

                    grid.AddGridRow(row);
                }
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

       
    }
}
