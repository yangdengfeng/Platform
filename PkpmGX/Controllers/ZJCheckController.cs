using QZWebService.ServiceModel;
using PkpmGX.Architecture;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using PkpmGX.Models;
using Pkpm.Core.ZJCheckService;
using Dhtmlx.Model.Grid;
using Newtonsoft.Json;
using ServiceStack;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class ZJCheckController : PkpmController
    {
        IZJCheckService zJCheckService;
        public ZJCheckController(IZJCheckService zJCheckService, IUserService userService) : base(userService)
        {
            this.zJCheckService = zJCheckService;
        }

        // GET: ZJCheck
        public ActionResult Index()
        {
            return View();
        }

        // GET: ZJCheck/Details/5
        public ActionResult Details(int id)
        {
            ZJCheckDetailsModel model = new ZJCheckDetailsModel()
            {
                JZPeople = new ZJCheckPeople(),
                CheckPeople = new List<ZJCheckPeople>(),
                programmePileList = new ZJCheckprogrammePileList()
            };
            GetZJCheckById requset = new GetZJCheckById()
            {
                id = id.ToString()
            };
            var response = zJCheckService.GetZJCheckById(requset);
            if (response.IsSucc)
            {
                model.CheckPeople = JsonConvert.DeserializeObject<List<ZJCheckPeople>>(response.data.testingpeople);
                model.programmePileList.projectname = response.data.projectname;
                model.programmePileList.checknum = response.data.checknum;
                model.programmePileList.testingpeople = response.data.testingpeople;
                model.programmePileList.testingequipment = response.data.testingequipment;
                model.programmePileList.plandate = (response.data.planstartdate.HasValue ? response.data.planstartdate.Value.ToString("yyyy-MM-dd") : string.Empty) + "-" + (response.data.planenddate.HasValue ? response.data.planenddate.Value.ToString("yyyy-MM-dd") : string.Empty);
                model.programmePileList.basictype = response.data.basictype;
                model.programmePileList.structuretype = response.data.structuretype;
                model.programmePileList.piletype = response.data.piletype;
                model.programmePileList.elevation = response.data.elevation;
                model.programmePileList.pilenum = response.data.pilenum;
                model.programmePileList.eigenvalues = response.data.eigenvalues;
                model.programmePileList.pilelenght = response.data.pilelenght;
                model.programmePileList.areadisplacement = response.data.areadisplacement;
                model.programmePileList.pilediameter = response.data.pilediameter;
                model.programmePileList.concretestrength = response.data.concretestrength;
                model.programmePileList.jzsynum = response.data.jzsynum;
                model.programmePileList.jzsynos = response.data.jzsynos;

                model.programmePileList.filepath = "http://175.6.228.209:8090/file"+ response.data.filepath;
                model.programmePileList.filename = response.data.filename;
                model.programmePileList.addtime = response.data.addtime;
                model.programmePileList.stuas = response.data.stuas;
                model.programmePileList.hqfilepath = "http://175.6.228.209:8090/file" + response.data.hqfilepath;
                model.programmePileList.hqfilename = response.data.hqfilename;
                model.programmePileList.customid = response.data.customid;
                model.programmePileList.witnesspeople = response.data.witnesspeople;
                model.programmePileList.projectname = response.data.projectname;
                model.programmePileList.projectregnum = response.data.projectregnum;
                model.programmePileList.areainfo = response.data.areainfo;
                model.programmePileList.areaname = response.data.areaname;
                model.programmePileList.constructionunit = response.data.constructionunit;
                model.programmePileList.supervisionunit = response.data.supervisionunit;
                model.programmePileList.designunit = response.data.designunit;
                model.programmePileList.constructionunits = response.data.constructionunits;
                model.programmePileList.personinchargename = response.data.personinchargename;
                model.programmePileList.personinchargetel = response.data.personinchargetel;
                model.programmePileList.projectaddress = response.data.projectaddress;
                model.programmePileList.customname = response.data.customname;
                model.programmePileList.reportcount = response.data.reportcount;


            }
            return View(model);
        }

        // GET: ZJCheck/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ZJCheck/Create
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

        // GET: ZJCheck/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ZJCheck/Edit/5
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

        // GET: ZJCheck/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ZJCheck/Delete/5
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

        public ActionResult Search(ZJCheckSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            GetZJCheckList searchdata = new GetZJCheckList()
            {
                CheckUnitName = model.CheckUnitName,
                CheckEquip = model.CheckEquip,
                CheckPeople = model.CheckPeople,
                Area = model.Area,
                Report = model.Report,
                ZX = model.ZX,
                posStart = model.posStart,
                count = model.count,
                ProjectName = model.ProjectName,
                StartDate = model.StartDate,
                EndDate =model.EndDate
            };
            var response = zJCheckService.GetZJCheck(searchdata);
            int totalCount = (int)response.totalCount;
            DhtmlxGrid grid = new DhtmlxGrid();
            grid.AddPaging(totalCount, pos);
            int index = pos;
            foreach (var item in response.datas)
            {
                var strpeople = string.Empty;
                if (!item.testingpeople.IsNullOrEmpty())
                {
                    var names = JsonConvert.DeserializeObject<List<LiftingEquipmentPeopleModel>>(item.testingpeople);
                    List<string> testingPeople = new List<string>();
                    foreach (var name in names)
                    {
                        testingPeople.Add(name.name);
                    }
                    strpeople = testingPeople.Join(",");
                }


                DhtmlxGridRow row = new DhtmlxGridRow(item.Id);
                row.AddCell(++index);
                row.AddCell(item.customname);
                row.AddCell(item.projectname);
                row.AddCell(item.areainfo);
                row.AddCell((int)item.pilenum);
                row.AddCell(strpeople);
                row.AddCell(string.Empty);
                row.AddCell(item.testingequipment);
                row.AddCell(item.checknum);
                row.AddCell(item.reportcount);
                row.AddCell(item.filepath.IsNullOrEmpty() ? "否" : "是");
                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }
    }
}
