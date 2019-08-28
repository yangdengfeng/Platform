using Dhtmlx.Model.Grid;
using Dhtmlx.Model.Toolbar;
using Newtonsoft.Json;
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
using Pkpm.Entity.DTO;
using ServiceStack;
using ServiceStack.OrmLite;
using Pkpm.Core.SysDictCore;
using Pkpm.Framework.Common;
using System.Xml.Linq;
using ServiceStack.Data;

namespace PkpmGX.Controllers
{
    public class ApplyQualifyFiveController : PkpmController
    {
        IRepsitory<t_D_UserTableFive> repFive;
        IRepsitory<t_bp_People> peopleRep;
        ISysDictService sysDictServcie;
        IDbConnectionFactory dbFactory;
        public ApplyQualifyFiveController(IUserService userService,
             IRepsitory<t_bp_People> peopleRep,
              ISysDictService sysDictServcie,
               IDbConnectionFactory dbFactory,
        IRepsitory<t_D_UserTableFive> repFive) : base(userService)
        {
            this.peopleRep = peopleRep;
            this.repFive = repFive;
            this.dbFactory = dbFactory;
            this.sysDictServcie = sysDictServcie;
        }

        // GET: ApplyQualifyFive
        public ActionResult Index()
        {
            return View();
        }

        // GET: ApplyQualifyFive/Details/5
        public ActionResult Details(string pid, string customId)
        {
            ApplyQualifyFiveDetailsModel model = new ApplyQualifyFiveDetailsModel()
            {
                pid = pid,
                customId = customId,
                checkWork = sysDictServcie.GetDictsByKey("CheckWork")

            };

            return View(model);
        }

        // GET: ApplyQualifyFive/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplyQualifyFive/Create
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

        // GET: ApplyQualifyFive/Edit/5
        public ActionResult Edit(string pid, string customId)
        {
            ApplyQualifyFiveDetailsModel model = new ApplyQualifyFiveDetailsModel()
            {
                pid = pid,
                customId = customId,
                checkWork = sysDictServcie.GetDictsByKey("CheckWork")

            };

            return View(model);
        }

        // POST: ApplyQualifyFive/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ApplyQualifyFiveEditSaveModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            List<string> strs = new List<string>();
            var root = XElement.Parse(model.gridStr);
            foreach (XElement rowElem in root.Elements("row"))
            {
                DetailQualifyFiveSearchViewModel str = new DetailQualifyFiveSearchViewModel();
                var id = (int?)rowElem.Attribute("id");
                var text = (string)rowElem;
                var index = 0;
                foreach (XElement cellElem in rowElem.Elements("cell"))
                {
                    switch (index)
                    {
                        case 1: str.jclb = (string)cellElem; break;
                        case 2: str.jclr = (string)cellElem; break;
                        case 3: str.name = (string)cellElem; break;
                        case 4: str.zgzsh = (string)cellElem; break;
                        case 5: str.sgzsh = (string)cellElem; break;
                        case 6: str.zcdwmc = (string)cellElem; break;
                        case 7: str.zc = (string)cellElem; break;
                        case 8: str.zy = (string)cellElem; break;
                        case 9: str.xl = (string)cellElem; break;
                        case 10: str.jcnx = (string)cellElem; break;
                        case 11: str.detectnumenddate = (string)cellElem; break;
                        case 12: str.bz = (string)cellElem; break;
                        case 13: str.zgzshpath = (string)cellElem; break;
                        case 14: str.sgzshpath = (string)cellElem; break;
                        case 15: str.zcpath = (string)cellElem; break;
                        case 16: str.xlpath = (string)cellElem; break;
                    }
                    index++;
                }
                strs.Add(str.ToJson());
            }
            using (var db = dbFactory.Open())
            {
                using (var dbTran = db.OpenTransaction())
                {
                    try
                    {
                        db.Delete<t_D_UserTableFive>(t => t.unitcode == model.CustomId && t.pid == model.PId);
                        foreach (var item in strs)
                        {
                            t_D_UserTableFive five = new t_D_UserTableFive()
                            {
                                gzjl = item,
                                pid = model.PId,
                                unitcode = model.CustomId,
                                staitc = 0
                            };
                            db.Insert(five);
                        }

                        //更新t_D_UserTableOne上的相应标识
                        db.UpdateOnly(new t_D_UserTableOne
                        {
                            fivepath_zl = "1"
                        }, r => new
                        {
                            r.fivepath_zl
                        }, r => r.id == Convert.ToInt32(model.PId));

                        dbTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTran.Rollback();
                        result.ErroMsg = ex.Message;
                        result = ControllerResult.FailResult;
                    }
                }
            }

            return Content(result.ToJson());
        }

        // GET: ApplyQualifyFive/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ApplyQualifyFive/Delete/5
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

        public ActionResult ToolBar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();

            //var buttons = GetCurrentUserPathActions();
            //if (HaveButtonFromAll(buttons, "Import"))
            //{
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Import", "导入") { Img = "fa fa-plus", Imgdis = "fa fa-plus" });
            toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("1"));

            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Save", "保存") { Img = "fa fa-plus", Imgdis = "fa fa-plus" });
            toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("2"));
            //}

            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult ImportToolBar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();

            //var buttons = GetCurrentUserPathActions();
            //if (HaveButtonFromAll(buttons, "Import"))
            //{
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("ImportEquips", "导入") { Img = "fa fa-plus", Imgdis = "fa fa-plus" });
            toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("1"));
            //}

            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }


        public ActionResult SearchImport(ApplyQualifyFiveSearchModel model)
        {
            DhtmlxGrid grid = new DhtmlxGrid();
            var precadite = PredicateBuilder.True<t_bp_People>();
            if (!model.CustomId.IsNullOrEmpty())
            {
                precadite = precadite.And(tt => tt.Customid == model.CustomId.Trim());
            }
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            var paging = new PagingOptions<t_bp_People>(pos, count, t => t.id);
            var datas = peopleRep.GetByConditonPage(precadite, paging);
            var totalCount = (int)paging.TotalItems;
            var PersonnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");
            grid.AddPaging(totalCount, pos);
            var index = 1;
            foreach (var item in datas)
            {
                DhtmlxGridRow row = new DhtmlxGridRow(item.id);
                row.AddCell(string.Empty);
                row.AddCell(index++);
                row.AddCell(item.Name);
                row.AddCell(string.Empty);
                row.AddCell(string.Empty);
                row.AddCell(string.Empty);
                row.AddCell(SysDictUtility.GetKeyFromDic(PersonnelTitles, item.Title));
                row.AddCell(item.Professional);
                row.AddCell(item.Education);
                row.AddCell(string.Empty);
                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                row.AddCell(string.Empty);
                row.AddCell(string.Empty);
                row.AddCell(item.titlepath);
                row.AddCell(item.educationpath);

                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }


        public ActionResult DetailsSearch(DetailQualifyFiveSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            PagingOptions<t_D_UserTableFive> pagingOption = new PagingOptions<t_D_UserTableFive>(pos, count, t => new { t.id });
            DhtmlxGrid grid = new DhtmlxGrid();
            var userTableFive = repFive.GetByConditonPage(r => r.pid == model.pid, pagingOption);
            grid.AddPaging(pagingOption.TotalItems, pos);
            int index = pos + 1;
            foreach (var item in userTableFive)
            {
                var zczshpath = string.Empty;
                var sgzshpath = string.Empty;
                var zcpath = string.Empty;
                var xlpath = string.Empty;

                var data = JsonConvert.DeserializeObject<DetailQualifyFiveSearchViewModel>(item.gzjl);

                DhtmlxGridRow row = new DhtmlxGridRow(item.id);
                row.AddCell(index);
                row.AddCell(data.jclb);
                row.AddCell(data.jclr);
                row.AddCell(data.name);
                row.AddCell(data.zgzsh);
                row.AddCell(data.sgzsh);
                row.AddCell(data.zcdwmc);
                row.AddCell(data.zc);
                row.AddCell(data.zy);
                row.AddCell(data.xl);
                row.AddCell(data.jcnx);
                row.AddCell(data.detectnumstartdate + "至" + data.detectnumenddate);
                row.AddCell(data.bz);
                row.AddLinkJsCell("注册证书附件查看", "uploadFile(\"{0}\")".Fmt(data.zgzshpath + "+" + "注册证书附件"));
                row.AddLinkJsCell("上岗证书附件查看", "uploadFile(\"{0}\")".Fmt(data.sgzshpath + "+" + "上岗证书附件"));
                row.AddLinkJsCell("职称附件查看", "uploadFile(\"{0}\")".Fmt(data.zcpath + "+" + "职称附件"));
                row.AddLinkJsCell("学历附件查看", "uploadFile(\"{0}\")".Fmt(data.xlpath + "+" + "学历附件"));
                index++;
                grid.AddGridRow(row);

            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult Search(DetailQualifyFiveSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            PagingOptions<t_D_UserTableFive> pagingOption = new PagingOptions<t_D_UserTableFive>(pos, count, t => new { t.id });
            DhtmlxGrid grid = new DhtmlxGrid();
            var userTableFive = repFive.GetByConditonPage(r => r.pid == model.pid, pagingOption);
            grid.AddPaging(pagingOption.TotalItems, pos);
            int index = pos + 1;
            foreach (var item in userTableFive)
            {
                var zczshpath = string.Empty;
                var sgzshpath = string.Empty;
                var zcpath = string.Empty;
                var xlpath = string.Empty;

                var data = JsonConvert.DeserializeObject<DetailQualifyFiveSearchViewModel>(item.gzjl);

                DhtmlxGridRow row = new DhtmlxGridRow(item.id);
                row.AddCell(index);
                row.AddCell(data.jclb);
                row.AddCell(data.jclr);
                row.AddCell(data.name);
                row.AddCell(data.zgzsh);
                row.AddCell(data.sgzsh);
                row.AddCell(data.zcdwmc);
                row.AddCell(data.zc);
                row.AddCell(data.zy);
                row.AddCell(data.xl);
                row.AddCell(data.jcnx);
                row.AddCell(data.detectnumstartdate + "至" + data.detectnumenddate);
                row.AddCell(data.bz);
                row.AddLinkJsCell("注册证书附件查看", "uploadFile(\"{0}\")".Fmt(data.zgzshpath + "+" + "注册证书附件"));
                row.AddLinkJsCell("上岗证书附件查看", "uploadFile(\"{0}\")".Fmt(data.sgzshpath + "+" + "上岗证书附件"));
                row.AddLinkJsCell("职称附件查看", "uploadFile(\"{0}\")".Fmt(data.zcpath + "+" + "职称附件"));
                row.AddLinkJsCell("学历附件查看", "uploadFile(\"{0}\")".Fmt(data.xlpath + "+" + "学历附件"));
                row.AddCell(new DhtmlxGridCell("删除", false).AddCellAttribute("title", "删除"));

                index++;
                grid.AddGridRow(row);

            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

    }
}
