using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using Pkpm.Core.SysDictCore;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Framework.PkpmConfigService;
using PkpmGX.Models;
using Newtonsoft.Json;
using Dhtmlx.Model.Grid;
using ServiceStack.OrmLite;
using ServiceStack;
using Dhtmlx.Model.Toolbar;
using System.Xml.Linq;
using Pkpm.Entity.DTO;
using ServiceStack.Data;

namespace PkpmGX.Controllers
{
    public class ApplyQualifySevenController : PkpmController
    {
        IRepsitory<t_D_UserTableSeven> repSeven;
        IDbConnectionFactory dbFactory;
        IRepsitory<t_bp_Equipment> equipRep;
        ISysDictService sysDictServcie;
        ICheckUnitService checkUnitService;
        IPkpmConfigService pkpmConfigService;
        public ApplyQualifySevenController(IRepsitory<t_D_UserTableSeven> repSeven,
             IRepsitory<t_bp_Equipment> equipRep,
             ISysDictService sysDictServcie,
             IDbConnectionFactory dbFactory,
             ICheckUnitService checkUnitService,
             IPkpmConfigService pkpmConfigService,
             IUserService userService) : base(userService)
        {
            this.repSeven = repSeven;
            this.equipRep = equipRep;
            this.dbFactory = dbFactory;
            this.sysDictServcie = sysDictServcie;
            this.checkUnitService = checkUnitService;
            this.pkpmConfigService = pkpmConfigService;
        }

        // GET: ApplyQualifySeven
        public ActionResult Index()
        {
            return View();
        }

        // GET: ApplyQualifySeven/Details/5
        public ActionResult Details(string pid, string customId)
        {
            ApplyQualifySevenDetailsModel model = new ApplyQualifySevenDetailsModel()
            {
                pid = pid,
                customId = customId
            };
            return View(model);
        }

        // GET: ApplyQualifySeven/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplyQualifySeven/Create
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

        // GET: ApplyQualifySeven/Edit/5
        public ActionResult Edit(string pid, string customId)
        {
            //customId = "1901301";
            ApplyQualifySevenDetailsModel model = new ApplyQualifySevenDetailsModel()
            {
                pid = pid,
                customId = customId
            };
            return View(model);
        }

        // POST: ApplyQualifySeven/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(ApplyQualifySevenEditSaveModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            List<ApplyQualifySevenEquipModel> Equips = new List<ApplyQualifySevenEquipModel>();
            var root = XElement.Parse(model.gridStr);
            List<int?> Ids = new List<int?>();

            foreach (XElement rowElem in root.Elements("row"))
            {

                var id = (int?)rowElem.Attribute("id");
                if(Ids.Contains(id))//如果重复导入同一个设备，直接跳过重复设备
                {
                    continue;
                }
                Ids.Add(id);
                var text = (string)rowElem;

                var index = 0;
                ApplyQualifySevenEquipModel equip = new ApplyQualifySevenEquipModel();

                foreach (XElement cellElem in rowElem.Elements("cell"))
                {
                    switch (index)
                    {
                        case 1:
                            var checkItem = (string)cellElem;
                            if (checkItem.IsNullOrEmpty())
                            {
                                result = ControllerResult.FailResult;
                                result.ErroMsg = "请输入检测项目之后再次提交";
                                return Content(result.ToJson());
                            }
                            equip.jcxm = (string)cellElem;
                            break;
                        case 2:
                            equip.zyyqsb = (string)cellElem;
                            break;
                        case 3:
                            equip.clfw = (string)cellElem;

                            break;
                        case 4:
                            equip.zqddj = (string)cellElem;

                            break;
                        case 5:
                            equip.jdxzjg = (string)cellElem;
                            break;
                        case 6:
                            var dates = (string)cellElem;
                            if (!dates.IsNullOrEmpty())
                            {
                                var endDates = dates.Split('~');
                                if (endDates != null && endDates.Count() > 0)
                                {
                                    if (endDates.Count() == 1) //兼容已经保存的数据，split之后只有一项
                                    {
                                        equip.yxrq = DateTime.Parse(endDates[0]);
                                    }
                                    else
                                    {
                                        var endDate = DateTime.Parse(endDates[1]);
                                        equip.yxrq = endDate;
                                    }
                                }
                            }
                            break;
                        case 7:
                            equip.zjxxm = (string)cellElem;
                            break;
                        case 8:
                            equip.zjxgfmcjbh = (string)cellElem;
                            break;
                        case 9:
                            equip.bz = (string)cellElem;
                            break;
                    }
                    index++;
                }
                Equips.Add(equip);
            }



            using (var db = dbFactory.Open())
            {
                using (var dbTran = db.OpenTransaction())
                {
                    try
                    {
                        db.Delete<t_D_UserTableSeven>(t => t.unitcode == model.CustomId && t.pid == model.PId);//编辑会把已有的所有数据都加载，然后重新提交数据，因此需要把已有的数据全部删除。
                        var index = 0;
                        foreach (var item in Equips)
                        {
                            t_D_UserTableSeven seven = new t_D_UserTableSeven()
                            {
                                gzjl = item.ToJson(),
                                pid = model.PId,
                                unitcode = model.CustomId,
                                EquipId = Ids[index],
                                staitc = 0
                            };
                            db.Insert(seven);
                            index++;
                        }

                        //更新t_D_UserTableOne上的相应标识
                        db.UpdateOnly(new t_D_UserTableOne
                        {
                            Sevenpath_zl = "1"
                        }, r => new
                        {
                            r.Sevenpath_zl
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

        // GET: ApplyQualifySeven/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ApplyQualifySeven/Delete/5
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

        public ActionResult Search(ApplyQualifySevenSearchModel model)
        {
            DhtmlxGrid grid = new DhtmlxGrid();
            var precadite = PredicateBuilder.True<t_D_UserTableSeven>();
            if (!model.PId.IsNullOrEmpty())
            {
                precadite = precadite.And(tt => tt.pid == model.PId.Trim());
            }
            if (!model.CustomId.IsNullOrEmpty())
            {
                precadite = precadite.And(tt => tt.unitcode == model.CustomId.Trim());
            }

            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            var pagine = new PagingOptions<t_D_UserTableSeven>(pos, count, tt => tt.id);

            var datas = repSeven.GetByConditonPage(precadite, pagine);
            var totalCount = (int)pagine.TotalItems;
            grid.AddPaging(totalCount, pos);
            int index = 1;

            var currentUserRole = GetCurrentUserRole();

            List<int?> equipIds = new List<int?>();
            foreach (var item in datas)
            {
                equipIds.Add(item.EquipId);
            }

            Dictionary<int, string> fileUrls = new Dictionary<int, string>();
            using (var db = dbFactory.Open())
            {
                fileUrls = db.Dictionary<int, string>(db.From<t_bp_Equipment>().Where(t => equipIds.Contains(t.id)).Select(t => new { t.id, t.repaircerfnumpath }));
            }

            foreach (var item in datas)
            {
                var equip = JsonConvert.DeserializeObject<ApplyQualifySevenEquipModel>(item.gzjl);
                if (equip != null)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(item.id);
                    var fileUrl = string.Empty;
                    if (item.EquipId.HasValue)
                    {
                        fileUrls.TryGetValue(item.EquipId.Value, out fileUrl);
                    }

                    row.AddCell(index++);
                    row.AddCell(equip.jcxm);
                    row.AddCell(equip.zyyqsb);
                    row.AddCell(equip.clfw);
                    row.AddCell(equip.zqddj);
                    row.AddCell(equip.jdxzjg);
                    row.AddCell(GetUIDtString(equip.yxrq));
                    row.AddCell(equip.zjxxm);
                    row.AddCell(equip.zjxgfmcjbh);
                    row.AddCell(equip.bz);
                    row.AddLinkJsCell("检定/校准文件查看", "uploadFile(\"{0}\")".Fmt(fileUrl));
                    grid.AddGridRow(row);
                }
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult SearchEdit(ApplyQualifySevenSearchModel model)
        {
            DhtmlxGrid grid = new DhtmlxGrid();
            var precadite = PredicateBuilder.True<t_D_UserTableSeven>();
            if (!model.PId.IsNullOrEmpty())
            {
                precadite = precadite.And(tt => tt.pid == model.PId.Trim());
            }
            if (!model.CustomId.IsNullOrEmpty())
            {
                precadite = precadite.And(tt => tt.unitcode == model.CustomId.Trim());
            }

            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            var pagine = new PagingOptions<t_D_UserTableSeven>(pos, count, tt => tt.id);

            var datas = repSeven.GetByConditonPage(precadite, pagine);
            var totalCount = (int)pagine.TotalItems;
            grid.AddPaging(totalCount, pos);
            int index = 1;

            List<int?> equipIds = new List<int?>();
            foreach (var item in datas)
            {
                equipIds.Add(item.EquipId);
            }

            Dictionary<int, string> fileUrls = new Dictionary<int, string>();
            using (var db = dbFactory.Open())
            {
                fileUrls = db.Dictionary<int, string>(db.From<t_bp_Equipment>().Where(t => equipIds.Contains(t.id)).Select(t => new { t.id, t.repaircerfnumpath }));
            }
            var currentUserRole = GetCurrentUserRole();

            foreach (var item in datas)
            {
                var equip = JsonConvert.DeserializeObject<ApplyQualifySevenEquipModel>(item.gzjl);
                if (equip != null)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(item.EquipId.HasValue ? item.EquipId.Value : item.id);
                    var fileUrl = string.Empty;
                    if (item.EquipId.HasValue)
                    {
                        fileUrls.TryGetValue(item.EquipId.Value, out fileUrl);
                    }
                    row.AddCell(index++);
                    row.AddCell(equip.jcxm);
                    row.AddCell(equip.zyyqsb);
                    row.AddCell(equip.clfw);
                    row.AddCell(equip.zqddj);
                    row.AddCell(equip.jdxzjg);
                    row.AddCell(GetUIDtString(equip.yxrq));
                    row.AddCell(equip.zjxxm);
                    row.AddCell(equip.zjxgfmcjbh);
                    row.AddCell(equip.bz);
                    row.AddLinkJsCell("检定/校准文件查看", "uploadFile(\"{0}\")".Fmt(fileUrl));
                    
                    row.AddCell(new DhtmlxGridCell("删除", false).AddCellAttribute("title", "删除"));
                   
                    grid.AddGridRow(row);
                }
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");

        }

        public ActionResult SearchImport(ApplyQualifySevenSearchModel model)
        {
            DhtmlxGrid grid = new DhtmlxGrid();
            var precadite = PredicateBuilder.True<t_bp_Equipment>();
            precadite = precadite.And(r => r.approvalstatus == "3");
            if (!model.CustomId.IsNullOrEmpty())
            {
                precadite = precadite.And(tt => tt.customid == model.CustomId.Trim());
            }
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;

            var paging = new PagingOptions<t_bp_Equipment>(pos, count, t => t.id);

            var datas = equipRep.GetByConditonPage(precadite, paging);

            var totalCount = (int)paging.TotalItems;

            grid.AddPaging(totalCount, pos);

            var index = 1;
            var allcustom = checkUnitService.GetAllCheckUnit();


            foreach (var item in datas)
            {
                string dateStr = string.Empty;
                if (item.checkenddate.HasValue && item.checkstartdate.HasValue)
                {
                    dateStr = GetUIDtString(item.checkstartdate, "yyyy-MM-dd") + "~" + GetUIDtString(item.checkenddate, "yyyy-MM-dd");
                }
                if (item.repairstartdate.HasValue && item.repairenddate.HasValue)
                {
                    dateStr = GetUIDtString(item.repairstartdate, "yyyy-MM-dd") + "~" + GetUIDtString(item.repairenddate, "yyyy-MM-dd");
                }

                DhtmlxGridRow row = new DhtmlxGridRow(item.id);
                row.AddCell(string.Empty);
                row.AddCell(index++);
                row.AddCell(item.equnum);
                row.AddCell(item.EquName + "/" + item.equtype + "/" + item.equspec);
                row.AddCell(item.testrange);
                row.AddCell(item.degree + "/" + item.uncertainty);
                row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allcustom, item.customid));
                row.AddCell(item.checkunit.IsNullOrEmpty() ? item.repairunit : item.checkunit);
                row.AddCell(dateStr);
                if (item.repairenddate.HasValue || item.checkenddate.HasValue)
                {
                    var datee = item.repairenddate.HasValue ? item.repairenddate : item.checkenddate;
                    var dateDiff = datee.Value - DateTime.Now;
                    var totalDay = (int)dateDiff.TotalDays;
                    if (totalDay < 0)
                    {
                        row.AddCell(new DhtmlxGridCell("已经超期{0}天".Fmt(0 - totalDay), false).AddCellAttribute("style", "color:red"));
                    }
                    else
                    {
                        row.AddCell("{0}天".Fmt(totalDay));
                    }
                }
                else
                {
                    row.AddCell(string.Empty);
                }
                row.AddCell(item.checkcerfnumpath.IsNullOrEmpty()?item.repaircerfnumpath:item.checkcerfnumpath);

                row.AddCell(string.Empty);



                grid.AddGridRow(row);

            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }
    }
}
