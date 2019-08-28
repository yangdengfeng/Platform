using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using ServiceStack;
using QZWebService.ServiceModel;
using PkpmGX.Models;
using Newtonsoft.Json;
using Pkpm.Framework.PkpmConfigService;
using Dhtmlx.Model.Grid;

namespace PkpmGX.Controllers
{
    /// <summary>
    /// 起重设备检测列表
    /// </summary>
    [Authorize]
    public class LiftingEquipmentController : PkpmController
    {
        IPkpmConfigService pkpmConfigService;
        static string GetSceneDataUrl;
        public LiftingEquipmentController(IPkpmConfigService pkpmConfigService, IUserService userService) : base(userService)
        {
            this.pkpmConfigService = pkpmConfigService;

            GetSceneDataUrl = pkpmConfigService.GetSceneDataUrl;

        }

        // GET: LiftingEquipment
        public ActionResult Index()
        {
            return View();
        }

        // GET: LiftingEquipment/Details/5
        [AllowAnonymous]
        public ActionResult Details(string qrinfo)
        {
            LiftingEquipmentViewModels model = new LiftingEquipmentViewModels()
            {
                programme = new view_programmeLiftList(),
                CheckPeoples = new List<LiftingEquipmentPeopleModel>(),
                WitnessPeoples = new List<LiftingEquipmentPeopleModel>()
            };
            if (!qrinfo.IsNullOrEmpty())
            {

                var client = new JsonServiceClient(GetSceneDataUrl);

                GetProgramme request = new GetProgramme();
                request.qrinfo = qrinfo;
                var responses = client.Get(request);
                if (responses.IsSucc)
                {
                    model.programme = responses.programe;
                }
                if (!responses.programe.testingpeople.IsNullOrEmpty())
                {

                    var Peoples = JsonConvert.DeserializeObject<List<LiftingEquipmentPeopleModel>>(responses.programe.testingpeople);
                    model.CheckPeoples.AddRange(Peoples);

                }
                if (!responses.programe.witnesspeople.IsNullOrEmpty())
                {
                    var Peoples = JsonConvert.DeserializeObject<List<LiftingEquipmentPeopleModel>>(responses.programe.witnesspeople);
                    model.WitnessPeoples.AddRange(Peoples);

                }
            }
            return View(model);
        }

        // GET: LiftingEquipment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LiftingEquipment/Create
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

        // GET: LiftingEquipment/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LiftingEquipment/Edit/5
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

        // GET: LiftingEquipment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LiftingEquipment/Delete/5
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

        public ActionResult Search(LiftingEquipmentSearchModel model)
        {
            DhtmlxGrid grid = new DhtmlxGrid();


            var client = new JsonServiceClient(GetSceneDataUrl);

            GetListProgramme request = new GetListProgramme()
            {
                customId = model.customId,
                projectname = model.projectname,
                areainfo = model.areainfo,
                testpeople = model.testpeople,
                posStart = model.posStart,
                count = model.count,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            if (model.IsReport.HasValue)
            {
                request.IsReport = model.IsReport.Value;
            }
            if (model.IsPhoto.HasValue)
            {
                request.IsPhoto = model.IsPhoto.Value;
            }

            var responses = client.Get(request);

            int pos = model.posStart.HasValue ? model.posStart.Value : 0;

            if (responses.IsSucc)
            {
                grid.AddPaging(responses.totalCount, pos);
                int index = 1 + pos;
                foreach (var item in responses.programes)
                {
                    List<string> peopleName = new List<string>();
                    if(!item.testingpeople.IsNullOrEmpty())
                    {
                        var Peoples = JsonConvert.DeserializeObject<List<LiftingEquipmentPeopleModel>>(item.testingpeople);
                        foreach (var people in Peoples)
                        {
                            peopleName.Add(people.name);
                        }
                    }
                    DhtmlxGridRow row = new DhtmlxGridRow(item.id);
                    row.AddCell(index++);
                    row.AddCell(item.customname);
                    row.AddCell(item.projectname);
                    row.AddCell(item.areainfo);
                    row.AddCell(item.checktype);
                    row.AddCell(peopleName.Join(","));
                    row.AddCell(GetUIDtString(item.addtime, "yyyy-MM-dd"));
                    row.AddCell(item.checknum);
                    if (item.reportcount > 0)
                    {
                        row.AddLinkJsCell(item.reportcount, "detailsReport(\"{0}\",\"{1}\")".Fmt(item.checknum,item.projectnum));
                    }
                    else
                    {
                        row.AddCell(item.reportcount);
                    }
                    row.AddCell(item.photoid.HasValue ? "有拍照" : "无拍照");
                    row.AddCell(item.longitude.HasValue && item.latitude.HasValue ?"有":"无");
                    //row.AddCell(new DhtmlxGridCell("查看",false).AddCellAttribute("title","查看"));
                    if(item.qrinfo.IsNullOrEmpty())
                    {
                        row.AddCell(string.Empty);
                    }
                    else
                    {
                        row.AddLinkJsCell("查看", "DetailsQrInfo(\"{0}\")".Fmt(item.qrinfo));
                    }
                    grid.AddGridRow(row);
                }
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult GetProgrammeReports(string checknum,string projectnum)
        {
            DhtmlxGrid grid = new DhtmlxGrid();

            GetProgrammeReports request = new GetProgrammeReports()
            {
                checknum = checknum,
                projectnum = projectnum
            };

            var client = new JsonServiceClient(GetSceneDataUrl);

            var response = client.Get(request);
            var index = 1;
            if(response.IsSucc)
            {
                foreach (var item in response.reports)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(item.Id);
                    row.AddCell(index++);
                    row.AddCell(item.checknum);
                    row.AddCell(item.reportnum);
                    row.AddLinkJsCell("查看","Details(\"{0}\",\"{1}\")".Fmt(item.customid,item.reportnum));
                    grid.AddGridRow(row);
                }
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult GetSysPrimaryKey(string customId, string reportNum)
        {
            LiftingEquipmentGetSysPrimaryKeyModel model = new LiftingEquipmentGetSysPrimaryKeyModel();


            var client = new JsonServiceClient(GetSceneDataUrl);

            var response = client.Get(new GetSysprimaryByReportNum()
            {
                customId = customId,
                reportNum = reportNum
            });

            if(response.IsSucc)
            {
                model.sysPrimaryKey = response.SysPrimaryKey;
            }

            return Content(model.ToJson());
        }
    }
}
