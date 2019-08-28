using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Dhtmlx.Model.Toolbar;
using Nest;
using Dhtmlx.Model.Grid;
using Pkpm.Framework.Repsitory;
using Pkpm.Entity.ElasticSearch;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.ItemNameCore;
using ServiceStack;
using Pkpm.Framework.Common;
using Microsoft.AspNet.Identity;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class CheckStatisReportController : PkpmController
    {
        IESRepsitory<es_t_bp_item> tbpitemESRep;
        ICheckUnitService checkUnitService;
        IItemNameService itemNameService;
        string aggeKey;
        string tBuckKey;

        public CheckStatisReportController(IUserService userService, 
            IESRepsitory<es_t_bp_item> tbpitemESRep, 
            ICheckUnitService checkUnitService,
            IItemNameService itemNameService) : base(userService)
        {
            this.tbpitemESRep = tbpitemESRep;
            this.checkUnitService = checkUnitService;
            this.itemNameService = itemNameService;
            aggeKey = "CustomId";
            tBuckKey = "ReportUpload";
        }

        // GET: CheckStatisReport
        public ActionResult Index()
        {
            return View();
        }

        // GET: CheckStatisReport/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CheckStatisReport/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckStatisReport/Create
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

        // GET: CheckStatisReport/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckStatisReport/Edit/5
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

        // GET: CheckStatisReport/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckStatisReport/Delete/5
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

        public ActionResult Toolbar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Export", "导出") { Img = "fa fa-clone", Imgdis = "fa fa-clone" });
            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult Search()
        {
            var reportUploadResult = GetReportUploadStatis();

            DhtmlxGrid grid = new DhtmlxGrid();
            if (reportUploadResult.IsValid)
            {
                var bucks = reportUploadResult.Aggs.Terms(aggeKey).Buckets;
                var allInsts = GetCurrentEliminateIsUerInsts();

                int index = 1;
                Dictionary<string, DateTime> dicExist = new Dictionary<string, DateTime>();
                foreach (var item in bucks)
                {
                    var tbuck = item.TopHits(tBuckKey);
                    if (tbuck != null && tbuck.Total > 0)
                    {
                        var lastItem = tbuck.Documents<es_t_bp_item>().First();
                        if (lastItem != null && lastItem.UPLOADTIME.HasValue)
                        {
                            dicExist[item.Key] = lastItem.UPLOADTIME.Value;
                        }
                    }
                }

                foreach (var custom in allInsts)
                {
                    //if (!checkUnitService.IsCanUploadUnit(custom.Key))
                    //{
                    //    continue;
                    //}

                    var currentDt = DateTime.Now;
                    if (dicExist.ContainsKey(custom.Key))
                    {
                        DhtmlxGridRow row = new DhtmlxGridRow(Guid.NewGuid().ToString());
                        row.AddCell(index.ToString());
                        row.AddCell(custom.Value);

                        var lastDt = dicExist[custom.Key];
                        TimeSpan dtSpan = currentDt - lastDt;
                        row.AddCell(lastDt.ToString("yyyy-MM-dd HH:mm"));
                        row.AddCell("{0}天{1}小时{2}分".Fmt(dtSpan.Days, dtSpan.Hours, dtSpan.Minutes));


                        grid.AddGridRow(row);
                        index++;
                    }
                    else
                    {
                        DhtmlxGridRow row = new DhtmlxGridRow(Guid.NewGuid().ToString());
                        row.AddCell(index.ToString());
                        row.AddCell(custom.Value);

                        var lastDt = DateTime.Now.AddYears(-1);
                        var dtSpan = currentDt - lastDt;
                        row.AddCell(lastDt.ToString("yyyy-MM-dd HH:mm"));
                        row.AddCell("{0}天{1}小时{2}分".Fmt(dtSpan.Days, dtSpan.Hours, dtSpan.Minutes));


                        grid.AddGridRow(row);
                        index++;
                    }
                }
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }
        public ActionResult Export(int? fileFormat)
        {
            var reportUploadResult = GetReportUploadStatis();
            bool xlsx = (fileFormat ?? 2007) == 2007;
            ExcelExporter ee = new ExcelExporter("报告上传统计", xlsx);
            ee.SetColumnTitles("序号, 检测机构,最新上传时间,未上传间隔 ");
            if (reportUploadResult.IsValid)
            {
                var bucks = reportUploadResult.Aggs.Terms(aggeKey).Buckets;
                int userId = HttpContext.User.Identity.GetUserId<Int32>();
                var allInsts = GetCurrentEliminateIsUerInsts();
                int index = 1;
                Dictionary<string, DateTime> dicExist = new Dictionary<string, DateTime>();
                foreach (var item in bucks)
                {
                    var tbuck = item.TopHits(tBuckKey);
                    if (tbuck != null && tbuck.Total > 0)
                    {
                        var lastItem = tbuck.Documents<es_t_bp_item>().First();
                        if (lastItem != null && lastItem.UPLOADTIME.HasValue)
                        {
                            dicExist[item.Key] = lastItem.UPLOADTIME.Value;
                        }
                    }
                }

                foreach (var custom in allInsts)
                {
                    //if (!checkUnitService.IsCanUploadUnit(custom.Key))
                    //{
                    //    continue;
                    //}

                    var currentDt = DateTime.Now;
                    if (dicExist.ContainsKey(custom.Key))
                    {
                        ExcelRow row = ee.AddRow();
                        row.AddCell(index.ToString());
                        row.AddCell(custom.Value);

                        var lastDt = dicExist[custom.Key];
                        var dtSpan = currentDt - lastDt;
                        row.AddCell(lastDt.ToString("yyyy-MM-dd HH:mm"));
                        row.AddCell("{0}天{1}小时{2}分".Fmt(dtSpan.Days, dtSpan.Hours, dtSpan.Minutes));
                        index++;
                    }
                    else
                    {
                        ExcelRow row = ee.AddRow();
                        row.AddCell(index.ToString());
                        row.AddCell(custom.Value);

                        var lastDt = DateTime.Now.AddYears(-1);
                        var dtSpan = currentDt - lastDt;
                        row.AddCell(lastDt.ToString("yyyy-MM-dd HH:mm"));
                        row.AddCell("{0}天{1}小时{2}分".Fmt(dtSpan.Days, dtSpan.Hours, dtSpan.Minutes));
                        index++;
                    }
                }
            }
            // 改动4：返回字节流
            return File(ee.GetAsBytes(), ee.MIME, ee.FileName);
        }

        private ISearchResponse<es_t_bp_item> GetReportUploadStatis()
        {
            var filterQuery = GetFilterQuery(checkUnitService, itemNameService, new Pkpm.Entity.SysSearchModel(), null, null);

            var response = tbpitemESRep.Search(s => s.Size(0).Query(filterQuery)
                             .Aggregations(af => af.Terms(aggeKey, item => item
                                        .Field(iif => iif.CUSTOMID).Size(200)
                                            .Aggregations(aaf => aaf.TopHits(tBuckKey, tdt => tdt.Size(1).Source(ccs => ccs.Includes(ccsi => ccsi.Field(ccsif => ccsif.UPLOADTIME)))
                                                    .Sort(cs => cs.Field(csf => csf.UPLOADTIME).Descending()))))));
            return response;
        }
    }
}
