using PkpmGX.Architecture;
using QZWebService.ServiceModel;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Framework.PkpmConfigService;
using Dhtmlx.Model.Grid;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class ZJPileUpdateInfoController : PkpmController
    {
        IPkpmConfigService pkpmconfigService;
        string GetSceneDataUrl;
        public ZJPileUpdateInfoController(IPkpmConfigService pkpmconfigService,
            IUserService userService) : base(userService)
        {
            this.pkpmconfigService = pkpmconfigService;

            GetSceneDataUrl = pkpmconfigService.GetSceneDataUrl;
        }

        // GET: ZJPileUpdateInfo
        public ActionResult Index()
        {
            return View();
        }

        // GET: ZJPileUpdateInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ZJPileUpdateInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ZJPileUpdateInfo/Create
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

        // GET: ZJPileUpdateInfo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ZJPileUpdateInfo/Edit/5
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

        // GET: ZJPileUpdateInfo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ZJPileUpdateInfo/Delete/5
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


        public ActionResult Search(PileUpdataInfo model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;

            var client = new JsonServiceClient(GetSceneDataUrl);
            var response = client.Get(model);
            DhtmlxGrid grid = new DhtmlxGrid();

            if (response.IsSucc)
            {
                int totalCount = (int)response.totalCount;
                grid.AddPaging(totalCount, pos);
                int index = pos;
                foreach (var item in response.datas)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(item.id);
                    row.AddCell(++index);
                    row.AddCell(item.customname);
                    row.AddCell(item.basicinfoid);
                    row.AddCell(item.utype);
                    row.AddCell(item.ordervalue);
                    row.AddCell(item.newvalue);
                    row.AddCell(item.addtime.HasValue ? item.addtime.Value.ToString("yyyy-MM-dd'T'HH:mm:ss") : string.Empty);
                    grid.AddGridRow(row);
                }
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }
    }
}
