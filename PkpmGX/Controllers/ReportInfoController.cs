using Dhtmlx.Model.Grid;
using PkpmGX.Architecture;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Framework.PkpmConfigService;
using ServiceStack;
using Pkpm.Core.CheckUnitCore;
using Newtonsoft.Json;
using PkpmGX.Models;
using QZWebService.ServiceModel;
using Pkpm.Framework.Repsitory;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class ReportInfoController : PkpmController
    {
        IPkpmConfigService PkpmConfigService;
        ICheckUnitService checkUnitService;
        //IRepsitory<view_programmeSecneList> programmeSecneListRep;
        string GetSceneDataUrl;
        public ReportInfoController(IUserService userService,
            IPkpmConfigService PkpmConfigService,
            ICheckUnitService checkUnitService
            //IRepsitory<view_programmeSecneList> programmeSecneListRep
            ) : base(userService)
        {
            this.GetSceneDataUrl = PkpmConfigService.GetSceneDataUrl;
            this.checkUnitService = checkUnitService;
            //this.programmeSecneListRep = programmeSecneListRep;
        }

        // GET: ReportInfo
        public ActionResult Index()
        {
            return View();
        }

        // GET: ReportInfo/Details/5
        public ActionResult Details(string id)
        {
            GetProjectInfos model = new GetProjectInfos()
            {
                id = int.Parse(id)
            };
            var client = new JsonServiceClient(GetSceneDataUrl);
            var data = client.Get(model);


            ProjectInfoDetailModels Model = new ProjectInfoDetailModels()
            {
                Id = data.project.Id,
                projectnum = data.project.projectnum,
                projectname = data.project.projectname,
                projectaddress = data.project.projectaddress,
                customid = data.project.customid,
                customname = data.project.customname,
                designunit = data.project.designunit,
                checknum = data.project.checknum,
                supervisionunit = data.project.supervisionunit,
                projectregnum = data.project.projectregnum,
                constructionunit = data.project.constructionunit,
                constructionunits = data.project.constructionunits,
                areainfo = data.project.areainfo,
                areaname = data.project.areaname,
                personinchargename = data.project.personinchargename,
                personinchargetel = data.project.personinchargetel,
                JZPeople = new List<witnesspeopleModels>()
            };
           
            if (!data.project.witnesspeople.IsNullOrEmpty())
            {
                Model.JZPeople = JsonConvert.DeserializeObject<List<witnesspeopleModels>>(data.project.witnesspeople);
            }

            return View(Model);
        }

        // GET: ReportInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportInfo/Create
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

        // GET: ReportInfo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReportInfo/Edit/5
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

        // GET: ReportInfo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReportInfo/Delete/5
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

        public ActionResult Search(GetXCProjectInfos model)
        {

            var client = new JsonServiceClient(GetSceneDataUrl);
            var data = client.Get(model);
            DhtmlxGrid grid = new DhtmlxGrid();
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int index = pos;
            grid.AddPaging(data.totalCount, pos);
            foreach (var item in data.datas)
            {
                //循环内处理单位名称、见证人员处理
                DhtmlxGridRow row = new DhtmlxGridRow(item.Id.ToString());
                row.AddCell((++index).ToString());
                row.AddCell(item.customname);
                row.AddCell(item.projectname);
                if (!item.witnesspeople.IsNullOrEmpty())
                {
                    var witnesspeople = JsonConvert.DeserializeObject<List<witnesspeopleModels>>(item.witnesspeople);
                    List<string> names = new List<string>();
                    foreach (var name in witnesspeople)
                    {
                        names.Add(name.name);
                    }
                    row.AddCell(names.Join(","));
                }
                else
                {
                    row.AddCell(string.Empty);
                }

                row.AddCell(item.areainfo);
                row.AddCell(new DhtmlxGridCell("[查看]", false).AddCellAttribute("title", "查看"));
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult ProgrammeSecneListSearch(GetprogrammeSecneLists model)
        {

            var client = new JsonServiceClient(GetSceneDataUrl);
            var data = client.Get(model);
            DhtmlxGrid grid = new DhtmlxGrid();
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int index = pos;
            grid.AddPaging(data.totalCount, pos);
            foreach (var item in data.datas)
            {
                DhtmlxGridRow row = new DhtmlxGridRow(item.Id.ToString());
                row.AddCell((++index).ToString());
                row.AddCell(item.customname);
                row.AddCell(item.projectname);
                row.AddCell(item.areainfo);
                row.AddCell(item.checktype);
                if (!item.testingpeople.IsNullOrEmpty())
                {
                    var witnesspeople = JsonConvert.DeserializeObject<List<witnesspeopleModels>>(item.testingpeople);
                    List<string> names = new List<string>();
                    foreach (var name in witnesspeople)
                    {
                        names.Add(name.name);
                    }
                    row.AddCell(names.Join("、"));
                }
                else
                {
                    row.AddCell(string.Empty);
                }
                row.AddCell(item.structpart);
                row.AddCell(item.checknum);
                if (item.reportcount > 0)
                {
                    row.AddLinkJsCell(item.reportcount, "DetailsReport(\"{0}\",\"{1}\")".Fmt(item.checknum, item.projectnum));
                }
                else
                {
                    row.AddCell(item.reportcount);
                }
                if (item.photoid.HasValue && item.photoid != 0)
                {
                    row.AddCell("已拍照");
                }
                else
                {
                    row.AddCell("无拍照");
                }
                if(item.latitude.HasValue && item.longitude.HasValue)
                {
                    row.AddCell("有");
                }
                else
                {
                    row.AddCell("无");
                }
                if(!item.filename.IsNullOrEmpty())
                {
                    row.AddCell("是");
                }
                else
                {
                    row.AddCell("否");
                } 
                row.AddCell(new DhtmlxGridCell("[查看]", false).AddCellAttribute("title", "查看"));
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult SearchProgrammeSecneList(GetprogrammeSecneLists model)
        {
            return View();
        }


        public ActionResult DetailsProgrammeSecneList(string id)
        {
            GetPSLs model = new GetPSLs()
            {
                id = int.Parse(id)
            };
            var client = new JsonServiceClient(GetSceneDataUrl);
            var data = client.Get(model);
            programmeSecneListDetailModels Model = new programmeSecneListDetailModels()
            {
                projectnum = data.project.projectnum,
                projectname = data.project.projectname,
                projectaddress = data.project.projectaddress,
                customid = data.project.customid,
                customname = data.project.customname,
                designunit = data.project.designunit,
                checknum = data.project.checknum,
                supervisionunit = data.project.supervisionunit,
                projectregnum = data.project.projectregnum,
                constructionunit = data.project.constructionunit,
                constructionunits = data.project.constructionunits,
                areainfo = data.project.areainfo,
                personinchargename = data.project.personinchargename,
                personinchargetel = data.project.personinchargetel,
                JZPeople = new List<witnesspeopleModels>(),
                JCPeople = new List<witnesspeopleModels>(),
                filename = data.project.filename,
                filepath = data.project.filepath,
                hqfilename = data.project.hqfilename,
                hqfilepath = data.project.hqfilepath,
                checktype =data.project.checktype,
                structpart = data.project.structpart,
                planstartdate = data.project.planstartdate.Value.ToString("yyyy-MM-dd"),
                planenddate = data.project.planenddate.Value.ToString("yyyy-MM-dd"),
                latitude = data.project.latitude,
                longitude = data.project.longitude,
                photopath = data.project.photopath,
                peoplepath = data.project.peoplepath
            };
            string day = Model.planstartdate + "至" + Model.planenddate;
            Model.planstartdate = day;
            if (!data.project.witnesspeople.IsNullOrEmpty())
            {
                Model.JZPeople = JsonConvert.DeserializeObject<List<witnesspeopleModels>>(data.project.witnesspeople);
            }
            if (!data.project.witnesspeople.IsNullOrEmpty())
            {
                Model.JCPeople = JsonConvert.DeserializeObject<List<witnesspeopleModels>>(data.project.testingpeople);
            }
            return View(Model);
        }

        public ActionResult DetailsReport(string checknum, string projectnum)
        {
            GetReports model = new GetReports()
            {
                checknum = checknum,
                projectnum = projectnum,
            };
            var client = new JsonServiceClient(GetSceneDataUrl);
            var data = client.Get(model);
            DhtmlxGrid grid = new DhtmlxGrid();
            int index = 1;
            foreach (var item in data.Reports)
            {
                DhtmlxGridRow row = new DhtmlxGridRow(item.Id.ToString());
                row.AddCell(index++);
                row.AddCell(item.checknum);
                row.AddCell(item.reportnum);
                row.AddLinkJsCell("查看", "Details(\"{0}\",\"{1}\")".Fmt(item.customid,item.reportnum));
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult Display(string reportNum,string customId)
        {
            tab_xc_reportModel model = new tab_xc_reportModel();

            var client = new JsonServiceClient(GetSceneDataUrl);

            var response = client.Get(new XCGetSysprimaryByReportNum()
            {
                customId = customId,
                reportNum = reportNum
            });

            if(response.IsSucc)
            {
                model.sysprimarykey = response.SysPrimaryKey;
            }

            return Content(model.ToJson());
        }


    }
}
