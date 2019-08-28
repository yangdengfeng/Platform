using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using PkpmGX.Models;
using Nest;
using Pkpm.Entity.ElasticSearch;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.ItemNameCore;
using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using Dhtmlx.Model.Grid;
using Pkpm.Framework.Common;
using Pkpm.Core.SysDictCore;
using ServiceStack;
using Pkpm.Core.ReportCore;
using System.Xml.Linq;
using Dhtmlx.Model.Toolbar;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class CheckStatisInspectController : PkpmController
    {
        ICheckUnitService checkUnitService;
        IItemNameService itemNameService;
        IESRepsitory<es_t_bp_item> tbpitemESRep;
        ISysDictService sysDictService;
        IReportService reportService;
        public CheckStatisInspectController(IUserService userService,
            ICheckUnitService checkUnitService,
            IItemNameService itemNameService,
             IReportService reportService,
        IESRepsitory<es_t_bp_item> tbpitemESRep,
             ISysDictService sysDictService) : base(userService)
        {
            this.itemNameService = itemNameService;
            this.checkUnitService = checkUnitService;
            this.tbpitemESRep = tbpitemESRep;
            this.sysDictService = sysDictService;
            this.reportService = reportService;
        }

        // GET: CheckStatisInspect
        public ActionResult Index()
        {
            return View();
        }

        // GET: CheckStatisInspect/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CheckStatisInspect/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckStatisInspect/Create
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

        // GET: CheckStatisInspect/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckStatisInspect/Edit/5
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

        // GET: CheckStatisInspect/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckStatisInspect/Delete/5
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

        public ActionResult Search(SysSearchModel model)
        {
            ISearchResponse<es_t_bp_item> response = GetSearchResult(model);
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int totalCount = (int)response.Total;
            DhtmlxGrid grid = new DhtmlxGrid();
            grid.AddPaging(totalCount, pos);
            int index = pos;
            var allItems = itemNameService.GetAllItemName();
            var allInsts = checkUnitService.GetAllCheckUnit();
            var reportConclusions = sysDictService.GetDictsByKey("ReportConclusionCode");
            foreach (var item in response.Documents)
            {
                DhtmlxGridRow row = new DhtmlxGridRow(item.SYSPRIMARYKEY);
               
                row.AddCell(index + 1);
                if (item.CONCLUSIONCODE == "Y")
                {
                    row.AddCell(item.PROJECTNAME);
                }
                else
                {
                    row.AddCell(new DhtmlxGridCell("{0}".Fmt(item.PROJECTNAME), false).AddCellAttribute("style", "color:red"));
                    //row.AddCell(item.PROJECTNAME);
                }
              
                row.AddCell(SysDictUtility.GetKeyFromDic(reportConclusions, item.CONCLUSIONCODE, "/"));
                row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allInsts, item.CUSTOMID));
                if (item.ITEMCHNAME.IsNullOrEmpty())
                {
                    row.AddCell(itemNameService.GetItemCNNameFromAll(allItems, item.REPORTJXLB, item.ITEMNAME));
                }
                else
                {
                    row.AddCell(item.ITEMCHNAME);
                }
                row.AddLinkJsCell(item.REPORTNUM, "detailsReport(\"{0}\")".Fmt(item.SYSPRIMARYKEY));
                row.AddCell(item.SAMPLENUM);
                row.AddCell(item.ENTRUSTDATE.HasValue ? item.ENTRUSTDATE.Value.ToString("yyyy-MM-dd") : "/");
                row.AddCell(item.CHECKDATE.HasValue ? item.CHECKDATE.Value.ToString("yyyy-MM-dd") : "/");
                row.AddCell(reportService.GetReportDataStatus(item.SAMPLEDISPOSEPHASEORIGIN));
                index++;
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }


        private ISearchResponse<es_t_bp_item> GetSearchResult(SysSearchModel model)
        {
            var filterQuery = GetFilterQuery(checkUnitService, itemNameService, model, new Dictionary<string, string>(), new Dictionary<string, string>());
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            return tbpitemESRep.Search(s => s.Source(sf => sf.Includes(sfi => sfi.Fields(
                   f => f.SYSPRIMARYKEY,
                   f => f.APPROVEDATE,
                   f => f.SAMPLENAME,
                   f => f.PROJECTNAME,
                   f => f.STRUCTPART,
                   f => f.CONCLUSIONCODE,
                    f => f.ITEMNAME,
                    f => f.ITEMCHNAME,
                    f => f.REPORTNUM,
                    f=>f.CUSTOMID,
                    f => f.ENTRUSTDATE,
                    f => f.CHECKDATE,
                    f=>f.SAMPLENUM,
                    f=>f.SAMPLEDISPOSEPHASEORIGIN,
                    f => f.PRINTDATE,
                    f => f.ADDTIME,
                     f => f.SAMPLEDISPOSEPHASE,
                     f => f.ACSTIME,
                     f => f.CODEBAR,
                     f => f.QRCODEBAR
                   ))).From(pos).Size(count).Query(filterQuery));
        }

        public ActionResult ToolBar()
        {
            var buttons = GetCurrentUserPathActions();
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
           
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Export", "导出") { Img = "fa fa-clone", Imgdis = "fa fa-clone" });
            toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("2"));
            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }



        public ActionResult CheckItemCombo()
        {
            XElement element = BuildCheckItemCombo();
            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        protected XElement BuildCheckItemCombo()
        {
            var Categorys = itemNameService.GetAllItemName();

            XElement element = new XElement("complete",
                 new XElement("template",
                    new XElement("input", new XCData("#name#")),
                    new XElement("header", true),
                    new XElement("columns",
                        new XElement("column",
                            new XAttribute("width", 320),
                            new XAttribute("header", "检测项目"),
                            new XAttribute("option", "#name#")))),
                 new XElement("option",
                           new XAttribute("value", string.Empty),
                           new XElement("text",
                                new XElement("name", "全部"))),
                 from kv in Categorys
                 select new XElement("option",
                            new XAttribute("value", kv.Key),
                            new XElement("text",
                                new XElement("name", kv.Value.IsNullOrEmpty() ? "无检测项目" : kv.Value))));

            return element;
        }

        public ActionResult Export(SysSearchModel model,int? fileFormat)
        {
            ISearchResponse<es_t_bp_item> response = GetSearchResult(model);
            var allItems = itemNameService.GetAllItemName();
            var allInsts = checkUnitService.GetAllCheckUnit();
            var reportConclusions = sysDictService.GetDictsByKey("ReportConclusionCode");
            bool xlsx = (fileFormat ?? 2007) == 2007;
            ExcelExporter ee = new ExcelExporter("监督抽检", xlsx);
            ee.SetColumnTitles("序号, 工程名称, 合格, 机构名称, 检测项目, 报告编号, 样品编号, 委托日期, 检测日期, 数据状态");
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int index = pos;
            foreach (var item in response.Documents)
            {
                ExcelRow row = ee.AddRow();

                row.AddCell(index + 1);
                row.AddCell(item.PROJECTNAME);
                row.AddCell(SysDictUtility.GetKeyFromDic(reportConclusions, item.CONCLUSIONCODE, "/"));
                row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allInsts, item.CUSTOMID));
                if (item.ITEMCHNAME.IsNullOrEmpty())
                {
                    row.AddCell(itemNameService.GetItemCNNameFromAll(allItems, item.REPORTJXLB, item.ITEMNAME));
                }
                else
                {
                    row.AddCell(item.ITEMCHNAME);
                }
                row.AddCell(item.REPORTNUM);
                row.AddCell(item.SAMPLENUM);
                row.AddCell(item.ENTRUSTDATE.HasValue ? item.ENTRUSTDATE.Value.ToString("yyyy-MM-dd") : "/");
                row.AddCell(item.CHECKDATE.HasValue ? item.CHECKDATE.Value.ToString("yyyy-MM-dd") : "/");
                row.AddCell(reportService.GetReportDataStatus(item.SAMPLEDISPOSEPHASEORIGIN));
                index++;
               
            }
            return File(ee.GetAsBytes(), ee.MIME, ee.FileName);

        }
    }
}
