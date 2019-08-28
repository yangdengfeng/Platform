using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using PkpmGX.Architecture;
using PkpmGX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Dhtmlx.Model.Grid;
using Newtonsoft.Json;
using Dhtmlx.Model.Toolbar;
using Pkpm.Entity.DTO;
using ServiceStack;
using Pkpm.Core.ApplyQualifySixCore;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.SysDictCore;
using Pkpm.Framework.Common;
using System.Xml.Linq;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pkpm.Framework.FileHandler;

namespace PkpmGX.Controllers
{
    public class ApplyQualifySixController : PkpmController
    {
        IRepsitory<t_D_UserTableSix> repSix;
        IRepsitory<t_D_UserTableOne> repOne;
        IRepsitory<t_bp_People> peopleRep;
        IFileHandler fileHander;
        IDbConnectionFactory dbFactory;
        IRepsitory<t_D_UserTableSixFile> sixFileRep;
        IApplyQualifyService applyQuailityService;
        ICheckUnitService checkUnitService;
        ISysDictService sysDictServcie;
        public ApplyQualifySixController(IRepsitory<t_D_UserTableSix> repSix,
             IRepsitory<t_bp_People> peopleRep,
               IRepsitory<t_D_UserTableOne> repOne,
                IRepsitory<t_D_UserTableSixFile> sixFileRep,
        IApplyQualifyService applyQuailityService,
        IFileHandler fileHander,
        ICheckUnitService checkUnitService, IDbConnectionFactory dbFactory,
        ISysDictService sysDictServcie,
        IUserService userService) : base(userService)
        {
            this.repOne = repOne;
            this.applyQuailityService = applyQuailityService;
            this.repSix = repSix;
            this.peopleRep = peopleRep;
            this.sixFileRep = sixFileRep;
            this.fileHander = fileHander;
            this.checkUnitService = checkUnitService;
            this.sysDictServcie = sysDictServcie;
            this.dbFactory = dbFactory;
        }

        // GET: ApplyQuakifySix
        public ActionResult Index(string id)
        {
            ViewBag.Id = id;
            var path = sixFileRep.GetByCondition(r => r.pid == int.Parse(id)).FirstOrDefault();
            var filepath=string.Empty;
            if (path != null)
            {
                filepath = path.filepath;
            }
            
            return View(filepath);
        }

        public ActionResult SixToolBar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            var currentUserRole = GetCurrentUserRole();
            if (currentUserRole.Code == "JCZXYH")
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Create", "导入") { Img = "fa fa-plus", Imgdis = "fa fa-plus" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("1"));


                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Save", "保存") { Img = "fa fa-plus", Imgdis = "fa fa-plus" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("2"));


                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Updata", "社保人员文档") { Img = "fa fa-plus", Imgdis = "fa fa-plus" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("3"));
            }
            else
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Check", "社保人员文档") { Img = "fa fa-plus", Imgdis = "fa fa-plus" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("4"));
            }
            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }


        // GET: ApplyQuakifySix/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ApplyQuakifySix/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplyQuakifySix/Create
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

        // GET: ApplyQuakifySix/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ApplyQuakifySix/Edit/5
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

        // GET: ApplyQuakifySix/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ApplyQuakifySix/Delete/5
        [HttpPost]
        public ActionResult Delete(string id)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string errorMsg = string.Empty;
                if (!applyQuailityService.Delete(id, out errorMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = errorMsg;
                }

            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }
            return Content(result.ToJson());
        }

        public ActionResult CreateQualifySix(string id)
        {
            ViewBag.id = id;
            return View();
        }

        //public ActionResult CreateSeave(string selectedId, string pid, FormCollection collection)
        //{
        //    ControllerResult result = ControllerResult.SuccResult;
        //    var selectids = selectedId.Split(',').ToList();
        //    var peoples = peopleRep.GetByCondition(r => selectids.Contains(r.id.ToString()));
        //    List<string> strs = new List<string>();
        //    var unitcode = repOne.GetById(pid).unitCode;
        //    foreach (var item in peoples)
        //    {
        //        ApplyQualifSixData model = new ApplyQualifSixData()
        //        {
        //            xm = item.Name,
        //            xb = item.Sex,
        //            nl = item.age.HasValue ? item.age.Value.ToString() : string.Empty,
        //            zw = item.zw,
        //            xl = item.Education,
        //            zy = string.Empty,
        //            zc = item.Title,
        //            sfzhm = item.SelfNum,
        //            sfzhmpath = item.selfnumPath,
        //            sgzsh = string.Empty,
        //            sgzshpath = string.Empty,
        //            csjflb = string.Empty,
        //            csjfnx = string.Empty,
        //            xlpath = item.educationpath,
        //            zcpath = item.titlepath,
        //            shbxzh = item.SBNum
        //        };
        //        strs.Add(model.ToJson());

        //        try
        //        {
        //            string errorMsg = string.Empty;
        //if (!applyQuailityService.ImportPeople(strs, unitcode, pid, out errorMsg))
        //            {
        //                result = ControllerResult.FailResult;
        //                result.ErroMsg = errorMsg;
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            result = ControllerResult.FailResult;
        //            result.ErroMsg = ex.Message;
        //        }

        //    }
        //    return Content(result.ToJson());
        //}




        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Save(ApplyQualifySixSaveModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            List<ApplyQualifSixData> peoples = new List<ApplyQualifSixData>();

            var root = XElement.Parse(model.gridStr);
            //  var updataPath = XElement.Parse(model.updatapath);
            List<int?> Ids = new List<int?>();//存储所有的人员编号，方便在后续的对数据库的操作中对人员编号赋值

            var customId = repOne.GetById(model.PId).unitCode;

            foreach (var rowElem in root.Elements("row"))
            {
                var id = (int?)rowElem.Attribute("id");
                if (Ids.Contains(id))//如果已经有这个人员信息，直接下一个
                {
                    continue;
                }

                Ids.Add(id);

                var text = (string)rowElem;
                var index = 0;
                ApplyQualifSixData people = new ApplyQualifSixData();
                foreach (var cellElem in rowElem.Elements("cell"))
                {
                    switch (index)
                    {
                        case 0:
                            people.xm = (string)cellElem;
                            break;
                        case 1:
                            people.xb = (string)cellElem;
                            break;
                        case 2:
                            people.nl = (string)cellElem;
                            break;
                        case 3:
                            people.zw = (string)cellElem;
                            break;
                        case 4:
                            people.xl = (string)cellElem;
                            break;
                        case 5:
                            people.zy = (string)cellElem;
                            break;
                        case 6:
                            people.zc = (string)cellElem;
                            break;
                        case 7:
                            people.sfzhm = (string)cellElem;
                            break;
                        case 8:
                            people.sgzsh = (string)cellElem;
                            break;
                        case 9:
                            people.csjflb = (string)cellElem;
                            break;
                        case 10:
                            people.csjfnx = (string)cellElem;
                            break;
                        case 11:
                            people.shbxzh = (string)cellElem;
                            break;
                        case 12:
                            people.xlpath = (string)cellElem;
                            break;
                        case 13:
                            people.zcpath = (string)cellElem;
                            break;
                        case 14:
                            people.sfzhmpath = (string)cellElem;
                            break;
                        case 15:
                            people.sgzshpath = (string)cellElem;
                            break;
                    }
                    index++;
                }
                peoples.Add(people);
            }


            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        db.Delete<t_D_UserTableSix>(t => t.pid == model.PId && t.unitcode == customId);
                        var index = 0;
                        foreach (var item in peoples)
                        {
                            t_D_UserTableSix oneSxi = new t_D_UserTableSix()
                            {
                                PeopleId = Ids[index],
                                gzjl = item.ToJson(),
                                unitcode = customId,
                                pid = model.PId,
                                staitc = 0
                            };
                            db.Insert(oneSxi);
                            index++;
                        }
                        dbTrans.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        result.ErroMsg = ex.Message;
                        result = ControllerResult.FailResult;
                    }
                }
            }

            return Content(result.ToJson());
        }

        public ActionResult Search(QualifysixSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            PagingOptions<t_D_UserTableSix> pagingOption = new PagingOptions<t_D_UserTableSix>(pos, count, t => new { t.id });
            DhtmlxGrid grid = new DhtmlxGrid();
            var userTableSix = repSix.GetByCondition(r => r.pid == model.pid);
            grid.AddPaging(pagingOption.TotalItems, pos);
            int index = pos + 1;
            var currentUserRole = GetCurrentUserRole();

            List<int?> Ids = new List<int?>();
            foreach (var item in userTableSix)
            {
                if (item.PeopleId.HasValue)
                {
                    Ids.Add(item.PeopleId.Value);
                }
            }

            var fileUrls = peopleRep.GetByConditon<PeopleSearchModel>(t => Ids.Contains(t.id), t => new { t.id, t.educationpath, t.selfnumPath, t.titlepath, t.PostPath });


            foreach (var item in userTableSix)
            {
                var data = JsonConvert.DeserializeObject<ApplyQualifSixData>(item.gzjl);
                var rowId = item.PeopleId.HasValue ? item.PeopleId.Value : item.id;
                DhtmlxGridRow row = new DhtmlxGridRow(rowId);
                //row.AddCell(index);
                row.AddCell(data.xm);
                row.AddCell(data.xb);
                row.AddCell(data.nl);
                row.AddCell(data.zw);
                row.AddCell(data.xl);
                row.AddCell(data.zy);
                row.AddCell(data.zc);
                row.AddCell(data.sfzhm);
                row.AddCell(data.sgzsh);
                row.AddCell(data.csjflb);
                row.AddCell(data.csjfnx);
                row.AddCell(data.shbxzh);
                row.AddCell(data.xlpath);
                row.AddCell(data.zcpath);
                row.AddCell(data.sfzhmpath);
                row.AddCell(data.sgzshpath);
                if (Ids.Contains(item.PeopleId))
                {
                    var fileUrl = fileUrls.Where(t => t.id == item.PeopleId);
                    if (fileUrl != null && fileUrl.Count() > 0)
                    {
                        var filePath = fileUrl.First();
                        Dictionary<string, string> dict = new Dictionary<string, string>();
                        if (!filePath.educationpath.IsNullOrEmpty())
                        {
                            dict.Add("学历证书", "uploadFile(\"{0}\")".Fmt(filePath.educationpath));
                        }
                        if (!filePath.titlepath.IsNullOrEmpty())
                        {
                            dict.Add("职称证书", "uploadFile(\"{0}\")".Fmt(filePath.titlepath));
                        }
                        if (!filePath.PostPath.IsNullOrEmpty())
                        {
                            dict.Add("上岗证书", "uploadFile(\"{0}\")".Fmt(filePath.PostPath));
                        }
                        if (!filePath.selfnumPath.IsNullOrEmpty())
                        {
                            dict.Add("身份证扫描", "uploadFile(\"{0}\")".Fmt(filePath.selfnumPath));
                        }
                        row.AddLinkJsCells(dict);
                    }
                    else
                    {
                        row.AddCell(string.Empty);
                    }
                }
                else
                {
                    row.AddCell(string.Empty);
                }
                row.AddCell(string.Empty);
                row.AddCell(string.Empty);
                if (currentUserRole.Code == "JCZXYH")
                {
                    row.AddCell(new DhtmlxGridCell("删除", false).AddCellAttribute("title", "删除"));
                }
                else
                {
                    row.AddCell(string.Empty);
                }
                index++;
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult CreateQualifySixSearch(DetailQualifyFiveSearchModel model)
        {
            var instDicts = checkUnitService.GetAllCheckUnit();
            var workStatus = sysDictServcie.GetDictsByKey("workStatus");
            var personnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");
            var personnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");
            var unitcode = repOne.GetById(model.pid);
            var peoples = peopleRep.GetByCondition(r => r.Customid == unitcode.unitCode && r.Approvalstatus == "3");
            //var peoples = peopleRep.GetByCondition(r => r.Customid == "1900021");
            DhtmlxGrid grid = new DhtmlxGrid();
            string customname = string.Empty;
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int index = pos;
            foreach (var item in peoples)
            {
                customname = checkUnitService.GetCheckUnitByIdFromAll(instDicts, item.Customid);
                DhtmlxGridRow row = new DhtmlxGridRow(item.id.ToString());
                //勾选,序号,姓名,性别,年龄,学历,专业,身份证号,从事检测工作类别,从事检测工作年限,社会保险证号,岗位证书编号,职务,职称,在职状况,状态
                row.AddCell("");
                row.AddCell((++index).ToString());
                row.AddCell(item.Name);
                row.AddCell(item.Sex);
                row.AddCell(item.age);
                row.AddCell(item.Education);
                row.AddCell(item.Professional);
                //row.AddCell(customname);
                row.AddCell(item.SelfNum);
                row.AddCell(string.Empty);
                row.AddCell(item.gznx);
                row.AddCell(item.SBNum);
                row.AddCell(item.PostNum);
                row.AddCell(item.zw);
                row.AddCell(SysDictUtility.GetKeyFromDic(personnelTitles, item.Title));
                row.AddCell(SysDictUtility.GetKeyFromDic(workStatus, item.iscb));
                row.AddCell(SysDictUtility.GetKeyFromDic(personnelStatus, item.Approvalstatus));
                row.AddCell(item.educationpath);
                row.AddCell(item.titlepath);
                row.AddCell(item.selfnumPath);
                row.AddCell(item.PostPath);
                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }


        [HttpPost]
        public async Task<ActionResult> DeleteImage(ImageViewUpload model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            //string fileName = Regex.Replace(model.itemValue, Regex.Escape("/userfiles"), "", RegexOptions.IgnoreCase);


            return Content(result.ToJson());
        }

        [HttpPost]
        public async Task<ActionResult> attachUpload(string name)
        {
            //string customid = checkUnitService.GetCheckUnitByName(customName);

            string realname = Request.Files["file"].FileName;

            var extensionName = System.IO.Path.GetExtension(realname);

            string filename = "/{0}/{1}{2}".Fmt(name, Guid.NewGuid().ToString().Replace("-", ""), extensionName);

            fileHander.UploadFile(Request.Files["file"].InputStream, "userfiles", filename);

            DhtmlxUploaderResult uploader = new DhtmlxUploaderResult() { state = true, name = filename };
            string str = uploader.ToJson();
            return Content(str);
        }



    }
}
