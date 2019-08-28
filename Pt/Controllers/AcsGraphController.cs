using Nest;
using Pkpm.Core;
using Pkpm.Core.ItemNameCore;
using Pkpm.Core.ReportCore;
using Pkpm.Core.UserRoleCore;
using Pkpm.Entity.DTO;
using Pkpm.Entity.ElasticSearch;
using Pkpm.Framework.FileHandler;
using Pkpm.Framework.Repsitory;
using PkpmGX.Architecture;
using PkpmGX.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PkpmGX.Controllers
{
    public class AcsGraphController : PkpmController
    {

        IFileHandler fileHander;
        IESRepsitory<es_t_bp_acs> acsESRep;
        IAcsChartService acsChart;
       IItemNameService itemNameService;
        IReportService reportService;

        public AcsGraphController(IFileHandler fileHander,
            IESRepsitory<es_t_bp_acs> acsESRep,
            IItemNameService itemNameService,
             IAcsChartService acsChart,
            IReportService reportService,
            IUserService userService) : base(userService)
        {
            this.fileHander = fileHander;
            this.acsESRep = acsESRep;
            this.acsChart = acsChart;
            this.reportService = reportService;
            this.itemNameService = itemNameService;
        }

        // GET: AcsGraph/Index?id=
        public ActionResult Index(string id)
        {
            AcsGraphViewModel viewModel = new AcsGraphViewModel();
            var getResponse = acsESRep.Get(new DocumentPath<es_t_bp_acs>(id).Index("gx-t-bp-acs"));
            if (getResponse.IsValid)
            {

                var acsItem = getResponse.Source;


                if (string.IsNullOrWhiteSpace(acsItem.ACSDATAPATH))
                {
                    viewModel.DhtmxChartJsonStr = new AcsChart().ToJson();
                }
                else
                {
                    viewModel.FilePath = HttpUtility.UrlEncode(acsItem.ACSDATAPATH);

                    //图片类型的acs
                    if (itemNameService.IsAcsPicItemType(acsItem.DATATYPES))
                    {
                        viewModel.IsImage = true;
                    }
                    else
                    {

                        byte[] pkrData = reportService.GetStoreFile(acsItem.ACSDATAPATH);

                        MemoryStream ms = new MemoryStream(pkrData);
                        var schart = acsChart.BuildAcsChart(acsItem, ms);
                        if (schart.ChartItems.Count > 0)
                        {
                            schart.yMaxValue = schart.ChartItems.Max(ci => ci.yValue).ToString("f2");
                            schart.xMaxValue = schart.ChartItems.Max(ci => ci.xTime).ToString("f2");
                        }

                        schart.acsTime = acsItem.ACSTIME.HasValue ? acsItem.ACSTIME.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
                        schart.yAxis = itemNameService.GetAcsAxisLabel(acsItem.ITEMTABLENAME);
                        viewModel.DhtmxChartJsonStr = schart.ToJson();

                    }
                }
            }
            return View(viewModel);
        }

        // GET: AcsGraph/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AcsGraph/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AcsGraph/Create
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

        // GET: AcsGraph/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AcsGraph/Edit/5
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

        // GET: AcsGraph/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AcsGraph/Delete/5
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

        public ActionResult Image(string id)
        {
            MemoryStream stream = null;

            byte[] pkrData = reportService.GetStoreFile(HttpUtility.UrlDecode(id));
            stream = new MemoryStream(pkrData);
            string fileName = "graph.jpg";
            string contentType = MimeMapping.GetMimeMapping(fileName);
            return File(stream.ToArray(), contentType);
        }
    }
}
