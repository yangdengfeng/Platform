using Dhtmlx.Model.Grid;
using Pkpm.Core.UserRoleCore;
using Pkpm.Framework.PkpmConfigService;
using PkpmGX.Architecture;
using QZWebService.ServiceModel;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PkpmGX.Controllers
{
    public class ZJLocaleTestGPSController : PkpmController
    {
        IPkpmConfigService pkpmconfigService;
        string GetSceneDataUrl;
        public ZJLocaleTestGPSController(IPkpmConfigService pkpmconfigService,IUserService userService) : base(userService)
        {
            this.pkpmconfigService = pkpmconfigService;

            GetSceneDataUrl = pkpmconfigService.GetSceneDataUrl;
        }

        // GET: ZJLocaleTestGPS
        public ActionResult Index()
        {
            var client = new JsonServiceClient(GetSceneDataUrl);
            GetZjCheckGPSByArea model = new GetZjCheckGPSByArea();
            
            var response = client.Get(model);
            return View(response.gpsPileInfo);
        }

        // GET: ZJLocaleTestGPS/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ZJLocaleTestGPS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ZJLocaleTestGPS/Create
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


        public ActionResult Search(GetZjCheckGPSByArea model)
        {
            DhtmlxGrid grid = new DhtmlxGrid();
            if (model.Area.IsNullOrEmpty())
            {
                model.Area = "南宁市";
            }
            var client = new JsonServiceClient(GetSceneDataUrl);
            var response = client.Get(model);
            int index = 1;
            if (response.IsSucc)
            {
                foreach (var item in response.gpsPileInfo)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(index+","+item.GpsLongitude+","+item.GpsLatitude);
                    row.AddCell(index);
                    row.AddCell(item.projectname);
                    row.AddCell(item.SerialNo);
                    row.AddCell(item.PileNo);
                    row.AddCell(item.areaname);
                    grid.AddGridRow(row);
                    index++;
                }
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }
        // GET: ZJLocaleTestGPS/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ZJLocaleTestGPS/Edit/5
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

        // GET: ZJLocaleTestGPS/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ZJLocaleTestGPS/Delete/5
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
