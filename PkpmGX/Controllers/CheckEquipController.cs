using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Framework.FileHandler;
using Pkpm.Core.SysDictCore;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using Pkpm.Entity.Models;
using Dhtmlx.Model.Toolbar;
using ServiceStack;
using Pkpm.Entity.DTO;
using ServiceStack.OrmLite;
using Dhtmlx.Model.Grid;
using Pkpm.Framework.Common;
using PkpmGX.Models;
using System.Text.RegularExpressions;
using System.IO;
using Dhtmlx.Model.TreeView;
using System.Reflection;
using System.ComponentModel;
using Dhtmlx.Model.Form;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class CheckEquipController : PkpmController
    {
        IFileHandler fileHander;
        ISysDictService sysDictServcie;
        ICheckUnitService checkUnitService;
        IRepsitory<t_bp_Equipment> rep;
        IRepsitory<t_bp_Equipment_tmp> repTmp;
        IRepsitory<t_bp_custom> customRep;

        IRepsitory<t_bp_equItemSubItemList> equItemSubItemListRep;
        IRepsitory<t_bp_EquipmentTypeList> equipTypeList;

        public CheckEquipController(ISysDictService sysDictServcie,
            ICheckUnitService checkUnitService,
            IRepsitory<t_bp_equItemSubItemList> equItemSubItemListRep,
            IRepsitory<t_bp_Equipment> rep,
            IRepsitory<t_bp_Equipment_tmp> repTmp,
             IRepsitory<t_bp_custom> customRep,
             IRepsitory<t_bp_EquipmentTypeList> equipTypeList,
            IFileHandler fileHander,
            IUserService userService) : base(userService)
        {
            this.sysDictServcie = sysDictServcie;
            this.checkUnitService = checkUnitService;
            this.equItemSubItemListRep = equItemSubItemListRep;
            this.rep = rep;
            this.repTmp = repTmp;
            this.fileHander = fileHander;
            this.equipTypeList = equipTypeList;
            this.customRep = customRep;
        }

        // GET: CheckEquip
        public ActionResult Index()
        {
            CheckEquipViewModels viewModel = new CheckEquipViewModels();
            viewModel.CompayTypes = sysDictServcie.GetDictsByKey("CompanyType");
            viewModel.CheckInsts = new Dictionary<string, string>();// checkUnitService.GetAllCheckUnit();
            //viewModel.apparatusType = sysDictServcie.GetDictsByKey("apparatusType");
            viewModel.personnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");

            return View(viewModel);
        }

        public ActionResult ToolBar()
        {
            var buttons = GetCurrentUserPathActions();
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            if (HaveButtonFromAll(buttons, "Create"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Create", "新增设备") { Img = "fa fa-clone", Imgdis = "fa fa-clone" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("1"));
            }
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Export", "导出") { Img = "fa fa-clone", Imgdis = "fa fa-clone" });
            toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("2"));
            if (HaveButtonFromAll(buttons, "Delete"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("deleteById", "[删除]") { Img = "fa fa-times", Imgdis = "fa fa-times" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("3"));
            }
            if (HaveButtonFromAll(buttons, "Submit"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Send", "[递交]") { Img = "fa fa-paper-plane", Imgdis = "fa fa-paper-plane" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("4"));
            }
            if (HaveButtonFromAll(buttons, "ReturnState"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("ReturnState", "[状态返回]") { Img = "fa fa-undo", Imgdis = "fa fa-undo" });
            }

            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        // GET: CheckEquip/Details/5
        public ActionResult Details(int id)
        {
            CheckEquipDetailsModel model = new CheckEquipDetailsModel()
            {
                Equip = new t_bp_Equipment(),
                CheckItems = new List<t_bp_equItemSubItemList>(),
                IsRepaircerfNumPath = string.Empty,
                IsCheckcerfNumPath = string.Empty
            };
            var equipment = rep.GetById(id);
            equipment.customid = checkUnitService.GetCheckUnitById(equipment.customid);
            equipment.equclass = sysDictServcie.GetDictsByKey("apparatusType", equipment.equclass);
            equipment.approvalstatus = sysDictServcie.GetDictsByKey("personnelStatus", equipment.approvalstatus);
            model.Equip = equipment;
            model.CheckItems = equItemSubItemListRep.GetByCondition(r => r.equId == id);
            model.IsCheckcerfNumPath = equipment.checkcerfnumpath.IsNullOrEmpty() ? "未上传" : (equipment.repaircerfnumpath.IsNullOrEmpty() ? "未上传" : "已上传");
            //model.IsRepaircerfNumPath =;
            return View(model);
        }

        // GET: CheckEquip/Create
        public ActionResult Create()
        {
            CheckEquipViewModels viewModel = new CheckEquipViewModels();
            //viewModel.CheckInsts = checkUnitService.GetAllCheckUnit();
            viewModel.YesNo = sysDictServcie.GetDictsByKey("yesNo");
            viewModel.apparatusType = sysDictServcie.GetDictsByKey("apparatusType");
            viewModel.EquClass = sysDictServcie.GetDictsByKey("apparatusType"); //仪器类别 
            viewModel.personnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");
            viewModel.CheckInsts = GetCurrentInsts();

            return View(viewModel);
        }

        // POST: CheckEquip/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CheckEquipSaveViewModel viewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;

            #region  对检定时间和校准时间的处理
            if (!viewModel.checkdate.IsNullOrEmpty())
            {
                int index = 0;
                var date = viewModel.checkdate.Split(' ');
                foreach (var item in date)
                {
                    switch (index)
                    {
                        case 0:
                            viewModel.checkstartdate = DateTime.Parse(item);
                            break;
                        case 2:
                            viewModel.checkenddate = DateTime.Parse(item);
                            break;
                    }
                    index++;
                }
            }

            if (!viewModel.repairedate.IsNullOrEmpty())
            {
                int index = 0;
                var date = viewModel.repairedate.Split(' ');
                foreach (var item in date)
                {
                    switch (index)
                    {
                        case 0:
                            viewModel.repairstartdate = DateTime.Parse(item);
                            break;
                        case 2:
                            viewModel.repairenddate = DateTime.Parse(item);
                            break;
                    }
                    index++;
                }
            }
            #endregion

            CheckEquipSaveModel createModel = new CheckEquipSaveModel();
            try
            {
                createModel.OpUserId = GetCurrentUserId();

                createModel.equipment = new t_bp_Equipment()
                {
                    customid = viewModel.customid,
                    equclass = viewModel.equclass,
                    EquName = viewModel.EquName,
                    equspec = viewModel.equspec,
                    degree = viewModel.degree,
                    equnum = viewModel.equnum,
                    equtype = viewModel.equtype,
                    testrange = viewModel.testrange,
                    uncertainty = viewModel.uncertainty,
                    checkunit = viewModel.checkunit,
                    checkcerfnum = viewModel.checkcerfnum,
                    checkstartdate = viewModel.checkstartdate,
                    checkenddate = viewModel.checkenddate,
                    repairunit = viewModel.repairunit,
                    repaircerfnum = viewModel.repaircerfnum,
                    repairstartdate = viewModel.repairstartdate,
                    repairenddate = viewModel.repairenddate,
                    selfcheckitem = viewModel.selfcheckitem,
                    selfcheckstandardname = viewModel.selfcheckstandardname,
                    selfchecknum = viewModel.selfchecknum,
                    selfrepairitem = viewModel.selfrepairitem,
                    selfrepairstandardname = viewModel.selfrepairstandardname,
                    selfrepairnum = viewModel.selfrepairnum,
                    explain = viewModel.explain,
                    isautoacs = viewModel.isautoacs,
                    autoacsprovider = viewModel.autoacsprovider,
                    approveadvice = viewModel.approveadvice,
                    approvalstatus = "0", //未递交
                    checkcerfnumpath = viewModel.checkcerfnumpath,
                    repaircerfnumpath = viewModel.repaircerfnumpath,
                    equclassId = viewModel.equclassId
                };

                createModel.Projects = new List<t_bp_equItemSubItemList>();
                if (!viewModel.ProjectGrid.IsNullOrEmpty())
                {
                    //var rows = XElement.Parse(viewModel.ProjectGrid).Elements("row");
                    var ProjectGrid = viewModel.ProjectGrid.Replace("删除", "|");
                    var rows = ProjectGrid.Split('|');
                    foreach (var rowItem in rows)
                    {
                        if (rowItem.IsNullOrEmpty() || rowItem == ",")
                        {
                            continue;
                        }
                        var cellItems = rowItem.Split(',');
                        var Project = new t_bp_equItemSubItemList();
                        int cellIndex = 0;
                        foreach (var cellItem in cellItems)
                        {
                            if (cellIndex == 0)
                            {
                                Project.itemChName = cellItem;
                            }
                            else if (cellIndex == 1)
                            {
                                Project.subItemList = cellItem;
                            }
                            cellIndex++;
                        }

                        createModel.Projects.Add(Project);
                    }
                }

                string erroMsg = string.Empty;
                if (!checkUnitService.CreatEquipment(createModel, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("进行了新增设备的操作");
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        private SearchResult<t_bp_Equipment> GetSearchResult(CheckEquipSearchModel searchModel)
        {

            var predicate = PredicateBuilder.True<t_bp_Equipment>();
            var stop = customRep.GetByConditon<string>(t => t.IsUse == 2, t => new { t.ID });
            foreach (var item in stop)
            {
                predicate = predicate.And(tp => tp.customid != item);
            }
           
            #region 动态查询
            if (!string.IsNullOrWhiteSpace(searchModel.CheckInst))
            {

                predicate = predicate.And(t => t.customid == searchModel.CheckInst);
            }

            var instFilter = GetCurrentInstFilter();
            if (instFilter.NeedFilter && instFilter.FilterInstIds.Count() > 0)
            {

                predicate = predicate.And(t => instFilter.FilterInstIds.Contains(t.customid));
            }

            if (!string.IsNullOrWhiteSpace(searchModel.CheckUnit))
            {

                predicate = predicate.And(t => t.checkunit != null && t.checkunit.Contains(searchModel.CheckUnit));
            }

            if (!string.IsNullOrWhiteSpace(searchModel.RepairUnit))
            {

                predicate = predicate.And(t => t.repairunit != null && t.repairunit.Contains(searchModel.RepairUnit));
            }

            if (!string.IsNullOrWhiteSpace(searchModel.EquNum))
            {

                predicate = predicate.And(t => t.equnum != null && t.equnum.Contains(searchModel.EquNum));
            }

            if (!string.IsNullOrWhiteSpace(searchModel.EquType))
            {

                predicate = predicate.And(t => t.equtype != null && t.equtype.Contains(searchModel.EquType));
            }


            //TODO:单位性质
            if (!string.IsNullOrWhiteSpace(searchModel.CustomType))
            {
                //hasPredicate = true;
                ///predicate = predicate.And(t =>  != null && t.equtype.Contains(searchModel.CustomType));
            }


            if (!string.IsNullOrWhiteSpace(searchModel.EquName))
            {

                predicate = predicate.And(t => t.EquName != null && t.EquName.Contains(searchModel.EquName));
            }

            if (!string.IsNullOrWhiteSpace(searchModel.EquClass) && searchModel.EquClass != "-1")
            {

                predicate = predicate.And(t => t.equclass != null && t.equclass.Contains(searchModel.EquClass));
            }

            if (searchModel.CheckStartDt.HasValue)
            {

                predicate = predicate.And(t => t.checkstartdate.HasValue && t.checkstartdate.Value <= searchModel.CheckStartDt.Value);
            }

            if (searchModel.CheckEndDt.HasValue)
            {

                predicate = predicate.And(t => t.checkenddate.HasValue && t.checkenddate.Value >= searchModel.CheckEndDt.Value);
            }

            if (searchModel.RepairStartDt.HasValue)
            {

                predicate = predicate.And(t => t.repairstartdate.HasValue && t.repairstartdate.Value <= searchModel.RepairStartDt.Value);
            }

            if (searchModel.RepairEndDt.HasValue)
            {

                predicate = predicate.And(t => t.repairenddate.HasValue && t.repairenddate.Value >= searchModel.RepairEndDt.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Status) && searchModel.Status != "-1")
            {

                predicate = predicate.And(t => t.approvalstatus == searchModel.Status);
            }

            //对企业类型查询条件进行处理  by振华
            if (!searchModel.CompanyType.IsNullOrEmpty())
            {
                List<string> customIds = customRep.GetByConditon<string>(t => t.companytype == searchModel.CompanyType, t => t.ID);
                predicate = predicate.And(t => customIds.Contains(t.customid));
            }

            #endregion



            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            int count = searchModel.count.HasValue ? searchModel.count.Value : 30;

            PagingOptions<t_bp_Equipment> pagingOption = new PagingOptions<t_bp_Equipment>(pos, count, t => new { t.id });

            var equips = rep.GetByConditonPage(predicate, pagingOption);

            return new SearchResult<t_bp_Equipment>(pagingOption.TotalItems, equips);
        }

        public ActionResult Search(CheckEquipSearchModel searchModel)
        {
            var searchResult = GetSearchResult(searchModel);
            var instDicts = checkUnitService.GetAllCheckUnit();
            var personnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");
            DhtmlxGrid grid = new DhtmlxGrid();
            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            grid.AddPaging(searchResult.TotalCount, pos);

            var allInsts = checkUnitService.GetAllCheckUnit();
            var buttons = GetCurrentUserPathActions();

            for (int i = 0; i < searchResult.Results.Count; i++)
            {
                var unitName = string.Empty;
                var equip = searchResult.Results[i];
                if (equip.checkunit.IsNullOrEmpty())
                {
                    unitName = equip.repairunit; //不管校准机构是否为空
                }

                DhtmlxGridRow row = new DhtmlxGridRow(equip.id.ToString());
                row.AddCell("");
                row.AddCell((pos + i + 1).ToString());
                row.AddCell(equip.equnum);
                row.AddCell(string.Format("{0}/{1}/{2}", equip.EquName, equip.equtype, equip.equspec));
                row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allInsts, equip.customid));
                //row.AddCell(equip.checkitem);
                row.AddCell(equip.checkunit);
                row.AddCell(string.Format("{0}</br>{1}", equip.checkstartdate.HasValue ? GetUIDtString(equip.checkstartdate.Value) : (equip.repairstartdate.HasValue ? GetUIDtString(equip.repairstartdate.Value) : ""),
                                                equip.checkenddate.HasValue ? GetUIDtString(equip.checkenddate.Value) : (equip.repairenddate.HasValue ? GetUIDtString(equip.repairenddate.Value) : "")), true);
                //row.AddCell(equip.repairunit);
                //row.AddCell(string.Format("{0}</br>{1}", equip.repairstartdate.HasValue ? GetUIDtString(equip.repairstartdate.Value) : "", equip.repairenddate.HasValue ? GetUIDtString(equip.repairenddate.Value) : ""), true);
                if (equip.checkstartdate.HasValue && equip.checkenddate.HasValue)
                {
                    if (equip.checkenddate >= DateTime.Now)
                    {
                        TimeSpan time = equip.checkenddate.Value - DateTime.Now;
                        row.AddCell(time.TotalDays.ToString("f0") + "天");
                    }
                    else
                    {
                        TimeSpan time = DateTime.Now - equip.checkenddate.Value;
                        row.AddCell("已经超期" + time.Days.ToString() + "天");
                    }
                }
                else if (equip.repairstartdate.HasValue && equip.repairenddate.HasValue)
                {
                    if (equip.repairenddate >= DateTime.Now)
                    {
                        TimeSpan time = equip.repairenddate.Value - DateTime.Now;
                        row.AddCell(time.TotalDays.ToString("f0") + "天");
                    }
                    else
                    {
                        TimeSpan time = DateTime.Now - equip.repairenddate.Value;
                        row.AddCell("已经超期" + time.TotalDays.ToString("f0") + "天");
                    }
                }
                else
                {
                    row.AddCell(string.Empty);
                }
                row.AddCell(SysDictUtility.GetKeyFromDic(personnelStatus, equip.approvalstatus));

                //Dictionary<string, string> dict = new Dictionary<string, string>();
                ////已递交	1
                //if (HaveButtonFromAll(buttons, "ApplyChange") && checkUnitService.CanApplyChangeEquip(equip.approvalstatus))
                //{
                //    dict.Add("[申请修改]", "applyChange({0},\"{1}\",\"{2}\")".Fmt(equip.id, equip.EquName, equip.customid));
                //}
                //row.AddLinkJsCells(dict);

                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                if (HaveButtonFromAll(buttons, "Edit") && equip.approvalstatus == "0")
                {
                    row.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                }
                else
                {
                    row.AddCell(string.Empty);
                }

                //对设备进行修改后，进行审核操作
                if (HaveButtonFromAll(buttons, "Audit") && equip.approvalstatus == "1")
                {
                    row.AddLinkJsCell("审核", "AuditEquipment(\"{0}\")".Fmt(equip.id));
                }
                else
                {
                    row.AddCell(string.Empty);
                }

                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        
        public ActionResult EditAttachFile(string filePath, string name)
        {
            CheckEquipEditAttachFileModel model = new CheckEquipEditAttachFileModel();
            model.FilePath = filePath;
            if (!filePath.IsNullOrEmpty())
            {
                var index = filePath.LastIndexOf('|');
                if (index == filePath.Length - 1)
                {
                    filePath = filePath.Substring(0, filePath.Length - 1);
                }

                model.Paths = filePath.Split('|');
            }
            model.Name = name;
            return View(model);
        }

        public ActionResult CreateAttachFile(string filePath, string name)
        {
            CheckEquipEditAttachFileModel model = new CheckEquipEditAttachFileModel();
            model.Paths = filePath.Split('|');
            model.Name = name;
            model.FilePath = filePath;
            return View(model);
        }

        public ActionResult AddCheckItemAndParm()
        {
            return View();
        }

        // GET: CheckEquip/Edit/5
        public ActionResult Edit(int id)
        {

            CheckEquipViewModels viewModel = new CheckEquipViewModels();
            viewModel.Equip = rep.GetById(id);
            //viewModel.Equip.customid = checkUnitService.GetCheckUnitById(viewModel.Equip.customid);
            viewModel.EquClass = sysDictServcie.GetDictsByKey("apparatusType"); //仪器类别 
            //viewModel.Equip.equclass = sysDictServcie.GetDictsByKey("apparatusType", viewModel.Equip.equclass); 
            viewModel.Equip.approvalstatus = sysDictServcie.GetDictsByKey("personnelStatus", viewModel.Equip.approvalstatus);
            viewModel.YesNo = sysDictServcie.GetDictsByKey("yesNo");
            viewModel.CheckInsts = checkUnitService.GetAllCheckUnit();
            viewModel.apparatusType = sysDictServcie.GetDictsByKey("apparatusType");
            viewModel.personnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");

            viewModel.CheckItems = equItemSubItemListRep.GetByCondition(r => r.equId == id);
            viewModel.IsCheckcerfNumPath = viewModel.Equip.checkcerfnumpath.IsNullOrEmpty() ? "未上传" : "已上传";
            viewModel.IsRepaircerfNumPath = viewModel.Equip.repaircerfnumpath.IsNullOrEmpty() ? "未上传" : "已上传";
            if (!viewModel.Equip.checkstartdate.HasValue && !viewModel.Equip.checkenddate.HasValue)
            {
                //上述两个字段都为空，下面两个都不为空的时候替换掉
                if (viewModel.Equip.repairstartdate.HasValue && viewModel.Equip.repairenddate.HasValue)
                {
                    viewModel.Equip.checkstartdate = viewModel.Equip.repairstartdate;
                    viewModel.Equip.checkenddate = viewModel.Equip.repairenddate;
                }
            }
            return View(viewModel);
        }

        // POST: CheckEquip/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CheckEquipSaveViewModel viewModel)
        {
            //检定和校准合到一起了  时间信息和路径信息往check 开头的数据中存  checkstartdate  checkenddate  checknumpath
            ControllerResult result = ControllerResult.SuccResult;

            #region  对检定时间和校准时间的处理
            if (!viewModel.checkdate.IsNullOrEmpty() && viewModel.checkdate != " - ")
            {
                int index = 0;
                var date = viewModel.checkdate.Split(' ');
                foreach (var item in date)
                {
                    switch (index)
                    {
                        case 0:
                            viewModel.checkstartdate = DateTime.Parse(item);
                            break;
                        case 2:
                            viewModel.checkenddate = DateTime.Parse(item);
                            break;
                    }
                    index++;
                }
            }
            #endregion

            CheckEquipTmpSaveModel editModel = new CheckEquipTmpSaveModel();
            try
            {
                editModel.OpUserId = GetCurrentUserId();
                editModel.equipment = new t_bp_Equipment_tmp()
                {
                    id = viewModel.id,
                    customid = viewModel.customid,
                    equclass = viewModel.equclass,
                    EquName = viewModel.EquName,
                    equspec = viewModel.equspec,
                    degree = viewModel.degree,
                    equnum = viewModel.equnum,
                    equtype = viewModel.equtype,
                    testrange = viewModel.testrange,
                    uncertainty = viewModel.uncertainty,
                    checkunit = viewModel.checkunit,
                    checkcerfnum = viewModel.checkcerfnum,
                    checkstartdate = viewModel.checkstartdate,
                    checkenddate = viewModel.checkenddate,
                    repairunit = viewModel.repairunit,
                    repaircerfnum = viewModel.repaircerfnum,
                    repairstartdate = viewModel.repairstartdate,
                    repairenddate = viewModel.repairenddate,
                    selfcheckitem = viewModel.selfcheckitem,
                    selfcheckstandardname = viewModel.selfcheckstandardname,
                    selfchecknum = viewModel.selfchecknum,
                    selfrepairitem = viewModel.selfrepairitem,
                    selfrepairstandardname = viewModel.selfrepairstandardname,
                    selfrepairnum = viewModel.selfrepairnum,
                    explain = viewModel.explain,
                    isautoacs = viewModel.isautoacs,
                    autoacsprovider = viewModel.autoacsprovider,
                    approveadvice = viewModel.approveadvice,
                    approvalstatus = string.IsNullOrEmpty(viewModel.approvalstatus) ? "" : sysDictServcie.GetDictsByName("personnelStatus", viewModel.approvalstatus),
                    checkcerfnumpath = viewModel.checkcerfnumpath,
                    repaircerfnumpath = viewModel.repaircerfnumpath,
                    equclassId = viewModel.equclassId
                };

                editModel.Projects = new List<t_bp_equItemSubItemList>();
                if (!viewModel.ProjectGrid.IsNullOrEmpty())
                {
                    //var rows = XElement.Parse(viewModel.ProjectGrid).Elements("row");
                    var ProjectGrid = viewModel.ProjectGrid.Replace("删除", "|");
                    var rows = ProjectGrid.Split('|');
                    foreach (var rowItem in rows)
                    {
                        if (rowItem.IsNullOrEmpty() || rowItem == ",")
                        {
                            continue;
                        }
                        var cellItems = rowItem.Split(',');
                        var Project = new t_bp_equItemSubItemList();
                        int cellIndex = 0;
                        foreach (var cellItem in cellItems)
                        {
                            if (cellIndex == 0)
                            {
                                Project.itemChName = cellItem;
                            }
                            else if (cellIndex == 1)
                            {
                                Project.subItemList = cellItem;
                            }
                            cellIndex++;
                        }
                        Project.equId = viewModel.id;
                        editModel.Projects.Add(Project);
                    }

                    //string[] sArray = viewModel.ProjectGrid.Split('\n');
                    //for (int i = 0; i < sArray.Length; i++)
                    //{
                    //    var Project = new t_bp_equItemSubItemList();
                    //    string[] siArray = sArray[i].Split(',');
                    //    Project.itemChName = siArray[1];
                    //    Project.subItemList = siArray[2];
                    //    Project.id = string.IsNullOrEmpty(siArray[3]) ? -1 : Convert.ToInt32(siArray[3]);
                    //    Project.equId = string.IsNullOrEmpty(siArray[4]) ? viewModel.id : Convert.ToInt32(siArray[4]);
                    //    editModel.Projects.Add(Project);
                    //}
                }

                string erroMsg = string.Empty;
                if (!checkUnitService.EditEquipment(editModel, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对id为{0}的设备信息进行了修改操作".Fmt(viewModel.id));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }


        /// <summary>
        /// 比较两实体的差异
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ColumnsDiffModel> CompareEntityDiff(string id)
        {
            List<ColumnsDiffModel> cdmList = new List<ColumnsDiffModel>();

            try
            {
                CheckEquipCompareModel people = GetCheckEquipCompareModel(rep.GetById(id), null);
                CheckEquipCompareModel peopleTmp = GetCheckEquipCompareModel(null, repTmp.GetById(id));
                if (peopleTmp == null) return cdmList;

                ColumnsDiffModel cdm = null;
                string get_old = string.Empty;
                string get_new = string.Empty;
                PropertyInfo[] mPi = typeof(CheckEquipCompareModel).GetProperties();
                foreach (PropertyInfo pi in mPi)
                {
                    string displayName = pi.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                    if (IsNumAndEnCh(displayName)) continue;

                    if (pi.Name.ToUpper().EndsWith("DATE"))
                    {
                        get_old = pi.GetValue(people, null) == null ? "" : Convert.ToDateTime(pi.GetValue(people, null)).ToString("yyyy-MM-dd");
                        get_new = pi.GetValue(peopleTmp, null) == null ? "" : Convert.ToDateTime(pi.GetValue(peopleTmp, null)).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        get_old = pi.GetValue(people, null) == null ? "" : pi.GetValue(people, null).ToString().Trim();
                        get_new = pi.GetValue(peopleTmp, null) == null ? "" : pi.GetValue(peopleTmp, null).ToString().Trim();
                    }

                    if (get_old != get_new)
                    {
                        cdm = new ColumnsDiffModel
                        {
                            Column = pi.Name, //字段
                            ColumnName = displayName, //字段自定义名
                            OldValue = get_old, //字段旧值
                            NewValue = get_new //字段新值
                        };

                        cdmList.Add(cdm);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return cdmList;
        }

        /// <summary>
        /// 审核，获取修改值进行对比展示
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult AuditEquipment(string Id)
        {
            string str = string.Empty;
            DhtmlxForm dForm = new DhtmlxForm();
            var itemcols = CompareEntityDiff(Id);
            if (itemcols.Count > 0)
            {
                var checkUnits = checkUnitService.GetAllCheckUnit();
                int index = 1;
                dForm.AddDhtmlxFormItem(new DhtmlxFormLabel("序号").AddStringItem("offsetLeft", "50"));
                foreach (var col in itemcols)
                {
                    dForm.AddDhtmlxFormItem(new DhtmlxFormInput(col.Column + "xuhao", "", (index++).ToString()).AddStringItem("offsetLeft", "50").AddStringItem("inputWidth", "50").AddStringItem("readonly", "true"));
                }

                dForm.AddDhtmlxFormItem(new DhtmlxFormNewcolumn());
                dForm.AddDhtmlxFormItem(new DhtmlxFormLabel("名称").AddStringItem("offsetLeft", "50"));
                foreach (var col in itemcols)
                {
                    dForm.AddDhtmlxFormItem(new DhtmlxFormInput(col.Column + "name", "", col.ColumnName).AddStringItem("offsetLeft", "50").AddStringItem("readonly", "true"));
                }
                dForm.AddDhtmlxFormItem(new DhtmlxFormButton("N", "审核不通过").AddStringItem("offsetLeft", "100"));

                dForm.AddDhtmlxFormItem(new DhtmlxFormNewcolumn());
                dForm.AddDhtmlxFormItem(new DhtmlxFormLabel("原始值").AddStringItem("offsetLeft", "50"));
                foreach (var col in itemcols)
                {
                    string value = string.Empty;
                    if (col.Column == "customid" && !string.IsNullOrEmpty(col.OldValue))
                    {
                        value = checkUnits[col.OldValue];
                    }
                    else
                    {
                        value = col.OldValue;
                    }

                    dForm.AddDhtmlxFormItem(new DhtmlxFormInput(col.Column + "|old", "", value).AddStringItem("offsetLeft", "50").AddStringItem("readonly", "true"));
                }
                dForm.AddDhtmlxFormItem(new DhtmlxFormButton("Y", "审核通过").AddStringItem("offsetLeft", "115"));

                dForm.AddDhtmlxFormItem(new DhtmlxFormNewcolumn());
                dForm.AddDhtmlxFormItem(new DhtmlxFormLabel("修改值").AddStringItem("offsetLeft", "50"));
                foreach (var col in itemcols)
                {
                    string value = string.Empty;
                    if (col.Column == "customid" && !string.IsNullOrEmpty(col.NewValue))
                    {
                        value = checkUnits[col.NewValue];
                    }
                    else
                    {
                        value = col.NewValue;
                    }

                    dForm.AddDhtmlxFormItem(new DhtmlxFormInput(col.Column + "|new", "", value).AddStringItem("offsetLeft", "50").AddStringItem("readonly", "true"));
                }
            }
            else
            {
                dForm.AddDhtmlxFormItem(new DhtmlxFormLabel("该设备信息无修改项，无需审核！").AddStringItem("offsetLeft", "300"));
            }

            str = dForm.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        /// <summary>
        /// 审核操作
        /// </summary>
        /// <param name="FormCol"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AuditEquipment(FormCollection FormCol)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;
            string opr = FormCol["Operate"].ToString();
            Dictionary<string, List<SysDict>> dic = new Dictionary<string, List<SysDict>>();
            var saveResult = checkUnitService.AuditEquipment(FormCol, dic, out errorMsg);
            if (!saveResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("进行了设备信息审核操作：{0}，设备ID为{1}".Fmt(opr, FormCol["Id"].ToString()));
            }

            return Content(result.ToJson());
        }

        /// <summary>
        /// 检查人员信息是否能被修改
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CheckEdit(string Id)
        {
            var model = rep.GetById(Id);
            return Content(model.ToJson());
        }

        /// <summary>
        /// 统一人员信息比较实体
        /// </summary>
        /// <param name="people"></param>
        /// <param name="peopleTmp"></param>
        /// <returns></returns>
        public CheckEquipCompareModel GetCheckEquipCompareModel(t_bp_Equipment equip, t_bp_Equipment_tmp equipTmp)
        {
            CheckEquipCompareModel model = null;
            if (equip != null)
            {
                return new CheckEquipCompareModel()
                {
                    id = equip.id,
                    customid = equip.customid,
                    equclass = equip.equclass,
                    EquName = equip.EquName,
                    equspec = equip.equspec,
                    degree = equip.degree,
                    equnum = equip.equnum,
                    equtype = equip.equtype,
                    testrange = equip.testrange,
                    uncertainty = equip.uncertainty,
                    checkunit = equip.checkunit,
                    checkcerfnum = equip.checkcerfnum,
                    checkstartdate = equip.checkstartdate,
                    checkenddate = equip.checkenddate,
                    repairunit = equip.repairunit,
                    repaircerfnum = equip.repaircerfnum,
                    repairstartdate = equip.repairstartdate,
                    repairenddate = equip.repairenddate,
                    selfcheckitem = equip.selfcheckitem,
                    selfcheckstandardname = equip.selfcheckstandardname,
                    selfchecknum = equip.selfchecknum,
                    selfrepairitem = equip.selfrepairitem,
                    selfrepairstandardname = equip.selfrepairstandardname,
                    selfrepairnum = equip.selfrepairnum,
                    explain = equip.explain,
                    isautoacs = equip.isautoacs,
                    autoacsprovider = equip.autoacsprovider,
                    approveadvice = equip.approveadvice,
                    checkcerfnumpath = equip.checkcerfnumpath,
                    repaircerfnumpath = equip.repaircerfnumpath,
                    equclassId = equip.equclassId
                };
            }

            if (equipTmp != null)
            {
                return new CheckEquipCompareModel()
                {
                    id = equipTmp.id,
                    customid = equipTmp.customid,
                    equclass = equipTmp.equclass,
                    EquName = equipTmp.EquName,
                    equspec = equipTmp.equspec,
                    degree = equipTmp.degree,
                    equnum = equipTmp.equnum,
                    equtype = equipTmp.equtype,
                    testrange = equipTmp.testrange,
                    uncertainty = equipTmp.uncertainty,
                    checkunit = equipTmp.checkunit,
                    checkcerfnum = equipTmp.checkcerfnum,
                    checkstartdate = equipTmp.checkstartdate,
                    checkenddate = equipTmp.checkenddate,
                    repairunit = equipTmp.repairunit,
                    repaircerfnum = equipTmp.repaircerfnum,
                    repairstartdate = equipTmp.repairstartdate,
                    repairenddate = equipTmp.repairenddate,
                    selfcheckitem = equipTmp.selfcheckitem,
                    selfcheckstandardname = equipTmp.selfcheckstandardname,
                    selfchecknum = equipTmp.selfchecknum,
                    selfrepairitem = equipTmp.selfrepairitem,
                    selfrepairstandardname = equipTmp.selfrepairstandardname,
                    selfrepairnum = equipTmp.selfrepairnum,
                    explain = equipTmp.explain,
                    isautoacs = equipTmp.isautoacs,
                    autoacsprovider = equipTmp.autoacsprovider,
                    approveadvice = equipTmp.approveadvice,
                    checkcerfnumpath = equipTmp.checkcerfnumpath,
                    repaircerfnumpath = equipTmp.repaircerfnumpath,
                    equclassId = equipTmp.equclassId
                };
            }

            return model;
        }

        /// <summary>
        /// 判断输入的字符串是否只包含数字和英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumAndEnCh(string input)
        {
            string pattern = @"^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        }


        // GET: CheckEquip/Delete/5
        public ActionResult Delete(string selectedId)
        {
            return Content("");
        }

        [HttpPost]
        public ActionResult Send(string selectedId)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkUnitService.SendStatueForEquips(selectedId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对id为{0}的设备信息进行了递交操作".Fmt(selectedId));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        [HttpPost]
        public ActionResult ReturnState(string selectedId)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkUnitService.ReturnStateFroEquips(selectedId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对id为{0}的设备信息进行了状态返回操作".Fmt(selectedId));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        [HttpPost]
        public ActionResult ApplyChange(CheckEuipApplyChange applyChangeModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                SupvisorJob job = new SupvisorJob()
                {
                    ApproveType = ApproveType.ApproveEquip,
                    CreateBy = GetCurrentUserId(),
                    CustomId = applyChangeModel.SubmitCustomId,
                    CreateTime = DateTime.Now,
                    NeedApproveId = applyChangeModel.SubmitId.ToString(),
                    NeedApproveStatus = NeedApproveStatus.CreateForChangeApply,
                    SubmitName = applyChangeModel.SubmitName,
                    SubmitText = applyChangeModel.SubmitText
                };


                string erroMsg = string.Empty;
                if (!checkUnitService.ApplyChangeForEquip(job, applyChangeModel.SubmitId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对设备信息进行了申请修改操作，申请人为{0}，申请原因{1}".Fmt(applyChangeModel.SubmitName,
                        applyChangeModel.SubmitText));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        // POST: CheckEquip/Delete/5
        [HttpPost]
        public ActionResult Delete(string selectedId, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkUnitService.DeleteEquipments(selectedId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对id为{0}的设备信息进行了删除操作".Fmt(selectedId));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        public ActionResult ProjectGrid(int id)
        {
            var equList = equItemSubItemListRep.GetByCondition(r => r.equId == id);
            DhtmlxGrid grid = new DhtmlxGrid();

            for (int i = 0; i < equList.Count; i++)
            {
                var equ = equList[i];
                DhtmlxGridRow row = new DhtmlxGridRow(equ.id.ToString());
                row.AddCell((i + 1).ToString());
                row.AddCell(equ.itemChName);
                row.AddCell(equ.subItemList);
                row.AddCell(equ.id.ToString());
                row.AddCell(equ.equId.ToString());
                row.AddLinkJsCell("删除", "myGrid_delete({0})".Fmt(equ.id));
                grid.AddGridRow(row);
                //  grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }


        public ActionResult FileGrid(int id)
        {

            string[,] FileRangeAr = new string[6, 2];
            FileRangeAr[0, 0] = "设备检定证书";
            FileRangeAr[1, 0] = "设备校准证书";
            if (id > 0)
            {
                var equip = rep.GetById(id);
                FileRangeAr[0, 1] = equip.checkcerfnumpath;
                FileRangeAr[1, 1] = equip.repaircerfnumpath;
            }
            else
            {
                FileRangeAr[0, 1] = "";
                FileRangeAr[1, 1] = "";
            }
            DhtmlxGrid grid = new DhtmlxGrid();

            for (int i = 0; i < 2; i++)
            {
                DhtmlxGridRow row = new DhtmlxGridRow(i.ToString());
                row.AddCell(FileRangeAr[i, 0]);
                row.AddCell(string.IsNullOrEmpty(FileRangeAr[i, 1]) ? "未上传" : "已上传");
                row.AddLinkJsCell("查看", "fileGrid_look(\"{0}\")".Fmt(i));
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }


        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="searchModel">查询参数</param>
        /// <param name="fileFormat">文件格式,2003/2007</param>
        /// <returns>用于下载的Excel文件内容</returns>
        public ActionResult Export(CheckEquipSearchModel searchModel, int? fileFormat)
        {
            // 改动1：（向上看）复制Search为新动作Export，添加参数fileFormat

            var searchResult = GetSearchResult(searchModel);

            var instDicts = checkUnitService.GetAllCheckUnit();
            var personnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");

            // 改动2：创建导出类实例（而非 DhtmlxGrid），设置列标题
            bool xlsx = (fileFormat ?? 2007) == 2007;
            ExcelExporter ee = new ExcelExporter("检测设备管理", xlsx);
            ee.SetColumnTitles("序号, 设备编号, 仪器设备名称/型号/规格, 所属检测机构, 检测项目名称, 检定机构, 检定有效日期, 校准机构, 校准有效日期, 检定/校准有效期剩余时间, 状态");
            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            var allInsts = checkUnitService.GetAllCheckUnit();
            for (int i = 0; i < searchResult.Results.Count; i++)
            {
                var equip = searchResult.Results[i];

                // 改动3：添加 ExcelRow 对象（而非 dhtmlxGridRow）
                ExcelRow row = ee.AddRow();

                row.AddCell((pos + i + 1).ToString());
                row.AddCell(equip.equnum);
                row.AddCell(string.Format("{0}/{1}/{2}", equip.EquName, equip.equtype, equip.equspec));
                row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allInsts, equip.customid));
                row.AddCell(equip.checkitem);
                row.AddCell(equip.checkunit);
                row.AddCell(string.Format("{0}~{1}", equip.checkstartdate.HasValue ? GetUIDtString(equip.checkstartdate.Value) : "", equip.checkenddate.HasValue ? GetUIDtString(equip.checkenddate.Value) : ""));
                row.AddCell(equip.repairunit);
                row.AddCell(string.Format("{0}~{1}", equip.repairstartdate.HasValue ? GetUIDtString(equip.repairstartdate.Value) : "", equip.repairenddate.HasValue ? GetUIDtString(equip.repairenddate.Value) : ""));
                if (equip.checkstartdate.HasValue && equip.checkenddate.HasValue)
                {
                    if (equip.checkenddate >= DateTime.Now)
                    {
                        TimeSpan time = equip.checkenddate.Value - DateTime.Now;
                        row.AddCell(time.Days.ToString() + "天");
                    }
                    else
                    {
                        TimeSpan time = DateTime.Now - equip.checkenddate.Value;
                        row.AddCell("已经超期" + time.TotalDays.ToString("f0") + "天");
                    }
                }
                else if (equip.repairstartdate.HasValue && equip.repairenddate.HasValue)
                {
                    if (equip.repairenddate >= DateTime.Now)
                    {
                        TimeSpan time = equip.repairenddate.Value - DateTime.Now;
                        row.AddCell(time.TotalDays.ToString("f0") + "天");
                    }
                    else
                    {
                        TimeSpan time = DateTime.Now - equip.repairenddate.Value;
                        row.AddCell("已经超期" + time.TotalDays.ToString("f0") + "天");
                    }
                }
                else
                {
                    row.AddCell(string.Empty);
                }
                row.AddCell(SysDictUtility.GetKeyFromDic(personnelStatus, equip.approvalstatus));
            }

            // 改动4：返回字节流
            return File(ee.GetAsBytes(), ee.MIME, ee.FileName);
        }

        public ActionResult getEquipsByCustomID(CheckEquipSearchModel searchModel)
        {
            var searchResult = GetSearchResult(searchModel);
            var instDicts = checkUnitService.GetAllCheckUnit();
            var personnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");

            DhtmlxGrid grid = new DhtmlxGrid();
            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            grid.AddPaging(searchResult.TotalCount, pos);
            var allInsts = checkUnitService.GetAllCheckUnit();
            for (int i = 0; i < searchResult.Results.Count; i++)
            {
                var equip = searchResult.Results[i];
                DhtmlxGridRow row = new DhtmlxGridRow(equip.id.ToString());
                row.AddCell((pos + i + 1).ToString());
                row.AddCell(string.Format("{0}/{1}/{2}", equip.EquName, equip.equtype, equip.equspec));
                row.AddCell(equip.checkunit);
                row.AddCell(equip.testrange);
                row.AddCell(equip.uncertainty);
                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        [HttpPost]
        public ActionResult attachUpload(string customName)
        {
            string customid = checkUnitService.GetCheckUnitByName(customName);

            string realname = Request.Files["file"].FileName;

            var extensionName = System.IO.Path.GetExtension(realname);

            string filename = "/{0}/{1}{2}".Fmt(customid, Guid.NewGuid().ToString().Replace("-", ""), extensionName);

            fileHander.UploadFile(Request.Files["file"].InputStream, "userfiles", filename);

            DhtmlxUploaderResult uploader = new DhtmlxUploaderResult() { state = true, name = filename };
            string str = uploader.ToJson();
            return Content(str);
        }

        public FileResult Image(ImageViewDownload model)
        {
            //导过来的历史数据，路径都带有"/userfiles"，文件系统里面的路径没有"/userfiles"，所以要去掉
            string fileName = Regex.Replace(model.itemValue, Regex.Escape("/userfiles"), "", RegexOptions.IgnoreCase);//model.itemValue;
            string mimeType = MimeMapping.GetMimeMapping(fileName);
            Stream stream = fileHander.LoadFile("userfiles", fileName);
            return new FileStreamResult(stream, mimeType);
        }


        [HttpPost]
        public ActionResult UpdatePaths(ImageViewUpload model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errMsg = string.Empty;
            bool success = this.checkUnitService.UpdateAttachPathsIntoEquip(model.id, model.itemName, model.itemValue, out errMsg);
            if (!success)
            {
                LogUserAction("对设备id为{0}的{1}字段进行了存储地址的更新操作,操作失败".Fmt(model.itemId, model.itemName));
                result = ControllerResult.FailResult;
                result.ErroMsg = errMsg;
            }
            else
            {
                LogUserAction("对设备id为{0}的{1}字段进行了存储地址的更新操作".Fmt(model.itemId, model.itemName));
            }
            return Content(result.ToJson());
        }

        [HttpPost]
        public ActionResult DeleteImage(ImageViewUpload model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string fileName = Regex.Replace(model.itemValue, Regex.Escape("/userfiles"), "", RegexOptions.IgnoreCase);
            try
            {
                fileHander.DeleteFile("userfiles", fileName);
                if (model.id > 0)
                {
                    string errMsg = string.Empty;
                    var equip = rep.GetById(model.id);
                    string pathvalues = string.Empty;
                    switch (model.itemName)
                    {
                        case "checkcerfnumpath":
                            pathvalues = equip.checkcerfnumpath;
                            break;
                        case "repaircerfnumpath":
                            pathvalues = equip.repaircerfnumpath;
                            break;

                    }
                    var queryStr = from str in pathvalues.Split('|')
                                   where str != model.itemValue
                                   select str;
                    //model.itemName存的是字段名
                    bool success = this.checkUnitService.UpdateAttachPathsIntoEquip(model.id, model.itemName, queryStr.Join("|"), out errMsg);
                    if (success)
                    {
                        LogUserAction("对设备id为{0}的{1}字段进行了存储地址的更新操作".Fmt(model.itemId, model.itemName));
                    }
                    else
                    {
                        LogUserAction("对设备id为{0}的{1}字段进行了存储地址的更新,操作失败".Fmt(model.itemId, model.itemName));
                    }
                }

            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = "删除失败:" + ex.Message;
            }

            return Content(result.ToJson());
        }

        public ActionResult BuildDTressData(int equipId, bool IsEdit = true)
        {
            DTreeResponse response = new DTreeResponse()
            {
                code = 200,
                msg = "",
                status = new Status { code = 200, message = "sucessd" },
                data = new List<DTree>()
            };
            var equClassId = rep.GetByConditon<string>(t => t.id == equipId, t => t.equclassId).FirstOrDefault();
            var allEquipmentTypeList = equipTypeList.GetByCondition(t => true);
            var rootNodes = allEquipmentTypeList.DistinctBy(t => t.typcode).ToList();

            foreach (var rootNode in rootNodes)
            {
                DTree dtree = new DTree()
                {
                    parentId = "-1",
                    id = rootNode.typcode,
                    title = rootNode.typename,
                    children = new List<DTree>()
                };
                var secNodes = allEquipmentTypeList.Where(t => t.itemcode.Contains(rootNode.typcode)).DistinctBy(t => t.itemcode).ToList();
                foreach (var secNode in secNodes)
                {
                    DTree secDTtee = new DTree()
                    {
                        parentId = rootNode.typcode,
                        id = secNode.itemcode,
                        title = secNode.itemname,
                        children = new List<DTree>(),
                        checkArr = new List<CheckArr>(),
                    };

                    var lastNodes = allEquipmentTypeList.Where(t => t.parmcode.Contains(secNode.itemcode)).ToList();
                    foreach (var lastNode in lastNodes)
                    {
                        DTree lastTree = new DTree()
                        {
                            parentId = secNode.itemcode,
                            id = lastNode.parmcode,
                            title = lastNode.parmname,
                            last = true,
                            checkArr = new List<CheckArr>()
                        };
                        CheckArr checkarr = new CheckArr()
                        {
                            type = "0",
                            @checked = "0"
                        };
                        if (!equClassId.IsNullOrEmpty())
                        {
                            if (equClassId.Contains(lastNode.parmcode))
                            {
                                checkarr.@checked = "1";
                            }
                        }
                        lastTree.checkArr.Add(checkarr);

                        if (IsEdit)
                        {
                            secDTtee.children.Add(lastTree);
                        }
                        else if (checkarr.@checked == "1")
                        {
                            secDTtee.children.Add(lastTree);
                        }

                    }
                    if (secDTtee.children.Count > 0)
                    {
                        dtree.children.Add(secDTtee);
                    }
                }
                if (dtree.children.Count > 0)
                {
                    response.data.Add(dtree);
                }
            }
            return Content(response.ToJson());
        }
    }
}
