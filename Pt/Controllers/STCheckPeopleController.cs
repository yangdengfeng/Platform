using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using PkpmGX.Models;
using Pkpm.Core.SysDictCore;
using Dhtmlx.Model.Toolbar;
using Pkpm.Entity.DTO;
using ServiceStack;
using Pkpm.Core.STCheckPeopleCore;
using Dhtmlx.Model.Grid;
using Pkpm.Entity;
using ServiceStack.OrmLite;
using Pkpm.Framework.Repsitory;
using Pkpm.Core.STCustomCore;
using Pkpm.Framework.Common;
using System.Text.RegularExpressions;
using Pkpm.Framework.FileHandler;
using System.IO;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class STCheckPeopleController : PkpmController
    {
        ISysDictService sysDictServcie;
        ISTCheckPeopleService sTCheckPeopleService;
        IRepsitory<t_bp_People_ST> rep;
        IRepsitory<t_bp_custom_ST> repCustom;
        IRepsitory<SupvisorJob> supvisorRep;
        ISTCustomService sTCustomService;
        IRepsitory<t_bp_PeoChange_ST> peoChangeRep;
        IRepsitory<t_bp_PeoEducation_ST> eduRep;
        IRepsitory<t_bp_PeoAwards_ST> peoAwardRep;
        IRepsitory<t_bp_PeoPunish_ST> punishRep;
        IFileHandler fileHander;
        public STCheckPeopleController(IUserService userService,
             IRepsitory<t_bp_PeoChange_ST> peoChangeRep,
             IRepsitory<t_bp_PeoEducation_ST> eduRep,
             IRepsitory<t_bp_PeoAwards_ST> peoAwardRep,
             IFileHandler fileHander,
             IRepsitory<t_bp_custom_ST> repCustom,
        IRepsitory<t_bp_PeoPunish_ST> punishRep,
             ISysDictService sysDictServcie,
             IRepsitory<t_bp_People_ST> rep,
             ISTCustomService sTCustomService,
             IRepsitory<SupvisorJob> supvisorRep,
        ISTCheckPeopleService sTCheckPeopleService) : base(userService)
        {
            this.peoAwardRep = peoAwardRep;
            this.peoChangeRep = peoChangeRep;
            this.repCustom = repCustom;
            this.fileHander = fileHander;
            this.eduRep = eduRep;
            this.punishRep = punishRep;
            this.supvisorRep = supvisorRep;
            this.rep = rep;
            this.sysDictServcie = sysDictServcie;
            this.sTCheckPeopleService = sTCheckPeopleService;
            this.sTCustomService = sTCustomService;
        }


        public ActionResult ToolBar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            var buttons = GetCurrentUserPathActions();
            if (HaveButtonFromAll(buttons, "Create"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("NewSTCheckPeople", "新增") { Img = "fa fa-plus", Imgdis = "fa fa-plus" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("1"));
            }

            if (HaveButtonFromAll(buttons, "Delete"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("DeletePeople", "[删除]") { Img = "fa fa-times", Imgdis = "fa fa-times" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("3"));
            }

            if (HaveButtonFromAll(buttons, "Submit"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("SetState", "[递交]") { Img = "fa fa-paper-plane", Imgdis = "fa fa-paper-plane" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("4"));
            }
            if (HaveButtonFromAll(buttons, "ReturnStatus"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("ReturnState", "[状态返回]") { Img = "fa fa-undo", Imgdis = "fa fa-undo" });
            }

            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }
        // GET: STCheckPeople
        public ActionResult Index()
        {
            var model = new STCheckPeopleViewModel();
            model.Status = sysDictServcie.GetDictsByKey("customStatus");
            return View(model);
        }

        // GET: STCheckPeople/Details/5
        public ActionResult Details(int id)
        {
            var model = new STCheckPeopleDetails();
            model.People = rep.GetById(id);
            var allSTUnit = GetCurrentInstsST();
            model.People.isjs = sysDictServcie.GetDictsByKey("noYes", model.People.isjs);
            model.People.ismanager = sysDictServcie.GetDictsByKey("noYes", model.People.ismanager);
            model.People.issy = sysDictServcie.GetDictsByKey("noYes", model.People.issy);
            model.People.ishaspostnum = sysDictServcie.GetDictsByKey("noYes", model.People.ishaspostnum);
            model.People.Title = sysDictServcie.GetDictsByKey("STPersonnelTitles", model.People.Title);
            model.UnitName = sTCustomService.GetSTUnitByIdFromAll(allSTUnit, model.People.Customid);

            model.STCheckPeoplePunish = CheckPeoplePunish(model.People.Id);
            model.STCheckPeopleAwards = CheckPeopleAwards(model.People.Id);
            model.STCheckPeopleChange = CheckPeopleChange(model.People.Id);
            model.STCheckPeopleEducation = CheckPeopleEducation(model.People.Id);
            model.STCheckPeopleFile = CheckPeopleFile(
               model.People.sbnumpath,
               model.People.selfnumPath,
               model.People.educationpath,
               model.People.PostPath,
               model.People.PhotoPath);

            return View(model);
        }

        public List<STCheckPeoplePunishModel> CheckPeoplePunish(int id)
        {
            var CheckPeoplePunishs = new List<STCheckPeoplePunishModel>();
            var punish = punishRep.GetByCondition(r => r.PeopleId == id);
            for (int i = 0; i < punish.Count; i++)
            {
                var pu = punish[i];
                STCheckPeoplePunishModel CheckPeoplePunish = new STCheckPeoplePunishModel();
                CheckPeoplePunish.Number = (i + 1).ToString();
                CheckPeoplePunish.PunDate = pu.PunDate.HasValue ? pu.PunDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                CheckPeoplePunish.PunName = pu.PunName;
                CheckPeoplePunish.PunUnit = pu.PunUnit;
                CheckPeoplePunish.PunContent = pu.PunContent;
                CheckPeoplePunish.Id = pu.Id.ToString();
                CheckPeoplePunish.PeopleId = pu.PeopleId.ToString();
                CheckPeoplePunishs.Add(CheckPeoplePunish);
            }
            return CheckPeoplePunishs;
        }

        public List<STCheckPeopleAwardsModel> CheckPeopleAwards(int id)
        {
            List<STCheckPeopleAwardsModel> CheckPeopleAwards = new List<STCheckPeopleAwardsModel>();
            var peoAward = peoAwardRep.GetByCondition(r => r.PeopleId == id);
            for (int i = 0; i < peoAward.Count; i++)
            {
                var peoAwa = peoAward[i];
                STCheckPeopleAwardsModel model = new STCheckPeopleAwardsModel();
                model.Number = (i + 1).ToString();
                model.AwaDate = peoAwa.AwaDate.HasValue ? peoAwa.AwaDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                model.AwaContent = peoAwa.AwaContent;
                model.AwaUnit = peoAwa.AwaUnit;
                model.Id = peoAwa.Id.ToString();
                model.PeopleId = peoAwa.PeopleId.ToString();
                CheckPeopleAwards.Add(model);
            }
            return CheckPeopleAwards;
        }

        public List<STCheckPeopleChangeModel> CheckPeopleChange(int id)
        {
            List<STCheckPeopleChangeModel> CheckPeopleChanges = new List<STCheckPeopleChangeModel>();
            var peoChange = peoChangeRep.GetByCondition(r => r.PeopleId == id);
            for (int i = 0; i < peoChange.Count; i++)
            {
                var change = peoChange[i];
                STCheckPeopleChangeModel CheckPeopleChange = new STCheckPeopleChangeModel();
                CheckPeopleChange.Number = i + 1;
                CheckPeopleChange.ChaDate = change.ChaDate.HasValue ? change.ChaDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                CheckPeopleChange.ChaContent = change.ChaContent;
                CheckPeopleChange.Id = change.Id.ToString();
                CheckPeopleChange.PeopleId = change.PeopleId.ToString();
                CheckPeopleChanges.Add(CheckPeopleChange);
            }
            return CheckPeopleChanges;
        }

        public List<STCheckPeopleEducationModel> CheckPeopleEducation(int id)
        {
            List<STCheckPeopleEducationModel> CheckPeopleEducations = new List<STCheckPeopleEducationModel>();
            var education = eduRep.GetByCondition(r => r.PeopleId == id);
            for (int i = 0; i < education.Count; i++)
            {
                var edu = education[i];
                STCheckPeopleEducationModel CheckPeopleEducation = new STCheckPeopleEducationModel();
                CheckPeopleEducation.Number = i + 1;
                CheckPeopleEducation.TrainDate = edu.TrainDate.HasValue ? edu.TrainDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                CheckPeopleEducation.TrainContent = edu.TrainContent;
                CheckPeopleEducation.TrainUnit = edu.TrainUnit;
                CheckPeopleEducation.TestDate = edu.TestDate.HasValue ? edu.TestDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                CheckPeopleEducation.Id = edu.Id.ToString();
                CheckPeopleEducation.PeopleId = edu.PeopleId.ToString();
                CheckPeopleEducations.Add(CheckPeopleEducation);
            }
            return CheckPeopleEducations;
        }

        public List<STCheckPeopleGridFileModel> CheckPeopleFile( string sbnumpath, string selfnumPath, string educationpath, string postPath, string photoPath)
        {
            string[,] FileRangeAr = new string[5, 2];
            FileRangeAr[0, 0] = "身份证";
            FileRangeAr[1, 0] = "证件照";
            FileRangeAr[2, 0] = "最高学历证件";
            FileRangeAr[3, 0] = "岗位证书";
            FileRangeAr[4, 0] = "社保";
            FileRangeAr[0, 1] = selfnumPath;
            FileRangeAr[1, 1] = photoPath;
            FileRangeAr[2, 1] = educationpath;
            FileRangeAr[3, 1] = postPath;
            FileRangeAr[4, 1] = sbnumpath;


            List<STCheckPeopleGridFileModel> CheckPeopleFiles = new List<STCheckPeopleGridFileModel>();
            for (int i = 0; i < 5; i++)
            {
                STCheckPeopleGridFileModel CheckPeopleFile = new STCheckPeopleGridFileModel();
                CheckPeopleFile.FileName = FileRangeAr[i, 0];
                CheckPeopleFile.Number = i;
                CheckPeopleFile.FileState = string.IsNullOrEmpty(FileRangeAr[i, 1]) ? "未上传" : "已上传";
                CheckPeopleFile.Path = FileRangeAr[i, 1];
                switch (i)
                {
                    case 0:
                        CheckPeopleFile.Type = "selfnumPath";
                        break;
                    case 1:
                        CheckPeopleFile.Type = "PhotoPath";
                        break;
                    case 2:
                        CheckPeopleFile.Type = "educationpath";
                        break;
                    case 3:
                        CheckPeopleFile.Type = "PostPath";
                        break;
                    case 4:
                        CheckPeopleFile.Type = "sbnumpath";
                        break;
                }
                CheckPeopleFiles.Add(CheckPeopleFile);
            }
            return CheckPeopleFiles;
        }

     
        // GET: STCheckPeople/Create
        public ActionResult Create()
        {
            var model = new STCheckPeopleEditViewModel();
            model.allSTUnit = GetCurrentInstsST();
            model.STPersonnelTitles = sysDictServcie.GetDictsByKey("STPersonnelTitles");
            model.NoYes = sysDictServcie.GetDictsByKey("noYes");
            model.Sex = sysDictServcie.GetDictsByKey("Sex");
            model.STCheckPeopleFile = CheckPeopleFile( null, null, null, null, null);
            return View(model);
        }

        // POST: STCheckPeople/Create
        [HttpPost]
        public ActionResult Create(STCheckPeopleSaveViewModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            result.IsSucc = true;
            STCheckPeopleSaveModel createModel = new STCheckPeopleSaveModel();
            try
            {
                if (repCustom.GetByCondition(r => r.Id == model.Customid).Count == 0)
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = "新增失败，该机构系统中不存在！";
                    return Content(result.ToJson());
                }
               
                DateTime? postnumstartdate = null;
                DateTime? postnumenddate = null;
                CommonUtils.GetLayuiDateRange(model.Postnumdate, out postnumstartdate, out postnumenddate);
                model.postnumstartdate = postnumstartdate;
                model.postnumenddate = postnumenddate;
                createModel.People = new t_bp_People_ST()
                {
                    
                    Approvalstatus = "0",
                    Name = model.Name,
                    Customid = model.Customid,
                    Sex = model.Sex,
                    PostDate=model.PostDate,
                    Birthday = model.Birthday,
                    SelfNum = model.SelfNum,
                    PostNum = model.PostNum,
                    ishaspostnum = model.ishaspostnum,
                    postnumstartdate = model.postnumstartdate,
                    postnumenddate = model.postnumenddate,
                    School = model.School,
                    Education = model.Education,
                    Title = model.Title,
                    Professional = model.Professional,
                    Tel = model.Tel,
                    zw = model.zw,
                    Email = model.Email,
                    SBNum = model.SBNum,
                    isjs = model.isjs,
                    ismanager = model.ismanager,
                    issy = model.issy,
                    PostType = model.PostType,
                    postDelayReg = model.postDelayReg,
                    selfnumPath = model.selfnumPath,
                    educationpath = model.educationpath,
                    PhotoPath = model.PhotoPath,
                    PostPath = model.PostPath,
                    sbnumpath = model.sbnumpath
                };
                createModel.PeoAward = new List<t_bp_PeoAwards_ST>();
                createModel.PeoChange = new List<t_bp_PeoChange_ST>();
                createModel.PeoPunish = new List<t_bp_PeoPunish_ST>();
                createModel.PeoEducation = new List<t_bp_PeoEducation_ST>();

                if (model.peoAward != null)
                {
                    foreach (var item in model.peoAward)
                    {
                        createModel.PeoAward.Add(item);
                    }
                }
                if (model.peoPunish != null)
                {
                    foreach (var item in model.peoPunish)
                    {
                        createModel.PeoPunish.Add(item);
                    }
                }
                if (model.peoEducation != null)
                {
                    foreach (var item in model.peoEducation)
                    {
                        createModel.PeoEducation.Add(item);
                    }
                }
                if (model.peoChange != null)
                {
                    foreach (var item in model.peoChange)
                    {
                        createModel.PeoChange.Add(item);
                    }
                }
                string erroMsg = string.Empty;
                if (!sTCheckPeopleService.CreatePeople(createModel, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = false;
                }
                else
                {
                    LogUserAction("进行了商砼人员信息新增操作，姓名为{0}，身份证号为{1}".Fmt(model.Name,model.SelfNum));
                }

            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
                result.IsSucc = false;
            }
            return Content(result.ToJson());
        }

        // GET: STCheckPeople/Edit/5
        public ActionResult Edit(int id)
        {
            var model = new STCheckPeopleEditViewModel();
            model.People = rep.GetById(id);
            model.allSTUnit = GetCurrentInstsST();
           // model.UnitName = sTCustomService.GetSTUnitByIdFromAll(allSTUnit, model.People.Customid);
            model.NoYes = sysDictServcie.GetDictsByKey("noYes");
            model.Sex = sysDictServcie.GetDictsByKey("Sex");
            model.Birthday = model.People.Birthday.HasValue ? model.People.Birthday.Value.ToString("yyyy-MM-dd") : string.Empty;
            model.STPersonnelTitles = sysDictServcie.GetDictsByKey("STPersonnelTitles");
            var postnumstartdate = model.People.postnumstartdate.HasValue ? model.People.postnumstartdate.Value.ToString("yyyy-MM-dd") : string.Empty;
            var postnumenddate = model.People.postnumenddate.HasValue ? model.People.postnumenddate.Value.ToString("yyyy-MM-dd") : string.Empty;
            model.Postnumdate = postnumstartdate + " - " + postnumenddate;
            model.PostDate = model.People.PostDate.HasValue ? model.People.PostDate.Value.ToString("yyyy-MM-dd") : string.Empty;
            model.STCheckPeoplePunish = CheckPeoplePunish(model.People.Id);
            model.STCheckPeopleAwards = CheckPeopleAwards(model.People.Id);
            model.STCheckPeopleChange = CheckPeopleChange(model.People.Id);
            model.STCheckPeopleEducation = CheckPeopleEducation(model.People.Id);
            model.STCheckPeopleFile = CheckPeopleFile(
               model.People.sbnumpath,
               model.People.selfnumPath,
               model.People.educationpath,
               model.People.PostPath,
               model.People.PhotoPath);
            return View(model);
        }


        // POST: STCheckPeople/Edit/5
        [HttpPost]
        public ActionResult Edit(STCheckPeopleSaveViewModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            result.IsSucc = true;
            STCheckPeopleSaveModel editModel = new STCheckPeopleSaveModel();
            try
            {
                DateTime? postnumstartdate = null;
                DateTime? postnumenddate = null;
                CommonUtils.GetLayuiDateRange(model.Postnumdate, out postnumstartdate, out postnumenddate);
                model.postnumstartdate = postnumstartdate;
                model.postnumenddate = postnumenddate;
                editModel.People = new t_bp_People_ST()
                {
                    PostDate=model.PostDate,
                    Id = model.Id,
                    Name = model.Name,
                    Customid = model.Customid,
                    Sex = model.Sex,
                    Birthday = model.Birthday,
                    SelfNum = model.SelfNum,
                    PostNum = model.PostNum,
                    ishaspostnum = model.ishaspostnum,
                    postnumstartdate = model.postnumstartdate,
                    postnumenddate = model.postnumenddate,
                    School = model.School,
                    Education = model.Education,
                    Title = model.Title,
                    Professional = model.Professional,
                    Tel = model.Tel,
                    zw = model.zw,
                    Email = model.Email,
                    SBNum = model.SBNum,
                    isjs = model.isjs,
                    ismanager = model.ismanager,
                    issy = model.issy,
                    PostType = model.PostType,
                    postDelayReg = model.postDelayReg,
                    selfnumPath = model.selfnumPath,
                    educationpath = model.educationpath,
                    PhotoPath = model.PhotoPath,
                    PostPath = model.PostPath,
                    sbnumpath = model.sbnumpath
                };
                editModel.PeoAward = new List<t_bp_PeoAwards_ST>();
                editModel.PeoChange = new List<t_bp_PeoChange_ST>();
                editModel.PeoPunish = new List<t_bp_PeoPunish_ST>();
                editModel.PeoEducation = new List<t_bp_PeoEducation_ST>();
                if (model.peoAward != null)
                {
                    foreach (var item in model.peoAward)
                    {
                        item.PeopleId = model.Id;
                        editModel.PeoAward.Add(item);
                    }
                }
                if (model.peoPunish != null)
                {
                    foreach (var item in model.peoPunish)
                    {
                        item.PeopleId = model.Id;
                        editModel.PeoPunish.Add(item);
                    }
                }
                if (model.peoEducation != null)
                {
                    foreach (var item in model.peoEducation)
                    {
                        item.PeopleId = model.Id;
                        editModel.PeoEducation.Add(item);
                    }
                }
                if (model.peoChange != null)
                {
                    foreach (var item in model.peoChange)
                    {
                        item.PeopleId = model.Id;
                        editModel.PeoChange.Add(item);
                    }
                }
                string erroMsg = string.Empty;
                if (!sTCheckPeopleService.EditPeople(editModel,out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = false;
                }
                else
                {
                    LogUserAction("进行了商砼人员信息修改操作，id为{0}".Fmt(model.Id));
                }


            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
                result.IsSucc = false;
            }
            return Content(result.ToJson());
        }

        // GET: STCheckPeople/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: STCheckPeople/Delete/5
        [HttpPost]
        public ActionResult Delete(string id)
        {
            ControllerResult result = ControllerResult.SuccResult;
            result.IsSucc = true;
            try
            {
                string erroMsg = string.Empty;
                if (!sTCheckPeopleService.DeletePeople(id, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = false;
                }
                else
                {
                    LogUserAction("对id为{0}的商砼人员信息进行了删除操作".Fmt(id));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
                result.IsSucc = false;
            }

            return Content(result.ToJson());
        }

        [HttpPost]
        public ActionResult SetInstState(string selectedId, string state, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            result.IsSucc = true;
            try
            {
                string erroMsg = string.Empty;
                if (!sTCheckPeopleService.SetInstSendState(selectedId, state, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = false;
                }
                else
                {
                    LogUserAction("对商砼人员ID为{0}进行了状态返回操作".Fmt(selectedId));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
                result.IsSucc = false;
            }

            return Content(result.ToJson());
        }

        public ActionResult Search(STCheckPeopleSearchModel model)
        {
            var data = GetSearchResult(model);
            DhtmlxGrid grid = new DhtmlxGrid();
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            grid.AddPaging(data.TotalCount, pos);
            var buttons = GetCurrentUserPathActions();
            var allSTUnit = GetCurrentInstsST();
            var compayTypes = sysDictServcie.GetDictsByKey("customStatus");
            for (int i = 0; i < data.Results.Count; i++)
            {
                var people = data.Results[i];
                DhtmlxGridRow row = new DhtmlxGridRow(people.Id.ToString());
                row.AddCell("");
                row.AddCell(pos + i + 1);
                row.AddCell(people.Name);
                row.AddCell(sTCustomService.GetSTUnitByIdFromAll(allSTUnit, people.Customid));
                row.AddCell(people.SelfNum);
                row.AddCell(people.PostNum);
                row.AddCell(people.Tel);
                row.AddCell(SysDictUtility.GetKeyFromDic(compayTypes, people.Approvalstatus));
                Dictionary<string, string> dict = new Dictionary<string, string>();
                if (HaveButtonFromAll(buttons, "ApplyChange") && sTCheckPeopleService.CanApplyChangeCustom(people.Approvalstatus))
                {
                    dict.Add("[申请修改]", "applyChange({0},\"{1}\")".Fmt(people.Id, people.Name));
                }

                if (HaveButtonFromAll(buttons, "ConfirmApplyChange") && (people.Approvalstatus == "5"))
                {
                    dict.Add("[审核申请修改]", "confirmApplyChange({0},\"{1}\")".Fmt(people.Id, people.Name));
                }
                row.AddLinkJsCells(dict);
                if (HaveButtonFromAll(buttons, "Edit") && (people.Approvalstatus == "0" || people.Approvalstatus == "6"))// && checkUnitService.CanEditCustom(custom.APPROVALSTATUS))
                {
                    row.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                }
                else
                {
                    row.AddCell(string.Empty);
                }
                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");

        }

        public SearchResult<STCheckPeopleUIModel> GetSearchResult(STCheckPeopleSearchModel model)
        {
            var predicate = PredicateBuilder.True<t_bp_People_ST>();
            //过滤已经删除的机构
            predicate = predicate.And(tp => tp.data_status == null || tp.data_status != "-1");
            if (!model.Name.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.Name.Contains(model.Name));
            }
            if (!model.CheckUnitName.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.Customid == model.CheckUnitName);
            }
            if (!model.Status.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.Approvalstatus == model.Status);
            }
            if (model.AgeStart.HasValue)
            {
                var ageStarDateTime = DateTime.Now.AddYears(0 - model.AgeStart.Value);
                predicate = predicate.And(t => t.Birthday.HasValue && t.Birthday.Value <= ageStarDateTime);
            }

            if (model.AgeEnd.HasValue)
            {
                var ageEndDateTime = DateTime.Now.AddYears(0 - model.AgeEnd.Value);
                predicate = predicate.And(t => t.Birthday.HasValue && t.Birthday.Value >= ageEndDateTime);
            }
            //if (!model.AgeStart.IsNullOrEmpty())//   string.IsNullOrEmpty(model.AgeStart.ToString())) //  !model.AgeStart.IsNullOrEmpty())
            //{
            //    predicate = predicate.And(t => t.Birthday >=DateTime.Parse(model.AgeStart));
            //}
            //if (!model.AgeEnd.IsNullOrEmpty())// string.IsNullOrEmpty(model.AgeEnd.ToString())) //  !model.AgeStart.IsNullOrEmpty())
            //{
            //    predicate = predicate.And(t => t.Birthday <=DateTime.Parse(model.AgeEnd));
            //}
            if (!model.PositionCategory.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.PostType.Contains(model.PositionCategory));// model.PositionCategory.Contains(t.PostType));
            }
            if (!model.PostCertNum.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.PostNum.Contains(model.PostCertNum));
            }
            if (!model.TechTitle.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.postname.Contains(model.TechTitle));
            }
            if (!model.IDnum.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.SelfNum.Contains(model.IDnum));
            }
            #region  设备查看人员新增此部分
            if (model.IsAdmin)
            {
                predicate = predicate.And(testc => testc.ismanager == "1");
            }
            if(model.IsCheck)
            {
                predicate = predicate.And(t => t.issy == "1");
            }
            if(model.IsTitle)
            {
                predicate = predicate.And(t => t.isjs == "1");
            }
            #endregion
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;

            PagingOptions<t_bp_People_ST> pagingOption = new PagingOptions<t_bp_People_ST>(pos, count, t => new { t.Id });
            var people = rep.GetByConditonPage<STCheckPeopleUIModel>(predicate, r => new
            {
                r.Id,
                r.Name,
                r.Customid,
                r.SelfNum,
                r.PostNum,
                r.Tel,
                r.Approvalstatus
            }, pagingOption);
            return new SearchResult<STCheckPeopleUIModel>(pagingOption.TotalItems, people);
        }


        public ActionResult Applychange(string ID)
        {
            var model = new STCheckPeopleApplyChangeViewModel();
            model.ID = ID;
            return View(model);

        }

        [HttpPost]
        public ActionResult ApplyChange(STCheckPeopleApplyChange applyChangeModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                SupvisorJob job = new SupvisorJob()
                {
                    ApproveType = ApproveType.STApprovePeople,
                    CreateBy = GetCurrentUserId(),
                    CustomId = applyChangeModel.SubmitId,
                    CreateTime = DateTime.Now,
                    NeedApproveId = applyChangeModel.SubmitId.ToString(),
                    NeedApproveStatus = NeedApproveStatus.CreateForChangeApply,
                    SubmitName = applyChangeModel.SubmitName,
                    SubmitText = applyChangeModel.SubmitText
                };


                string erroMsg = string.Empty;
                if (!sTCheckPeopleService.ApplyChangeForCustom(job, applyChangeModel.SubmitId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("进行了申请修改操作，申请人为{0}，申请原因为{1}".Fmt(applyChangeModel.SubmitName, applyChangeModel.SubmitText));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        public ActionResult ConfirmApplyChange(string Id)
        {
            SupvisorJob Result = new SupvisorJob();
            var model = supvisorRep.GetByCondition(w => w.CustomId == Id && w.NeedApproveId == Id && w.ApproveType == ApproveType.STApprovePeople).OrderByDescending(o => o.CreateTime);
            return View(model.FirstOrDefault());

        }

        [HttpPost]
        public ActionResult ConfirmApplyChange(STCheckEquipApplyChange applyChangeModel)
        {
            var Status = applyChangeModel.Status == "yes" ? "6" : "7";
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                result.IsSucc = true;
                if (!sTCheckPeopleService.UpdateCustomStatus(applyChangeModel.SubmitId, Status, "", out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = false;
                }
                else
                {
                    LogUserAction("进行了审核申请修改操作");
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
                result.IsSucc = true;
            }

            return Content(result.ToJson());
        }


        public ActionResult GetFile(string path)
        {
            var model = new STCheckPeopleFileModel()
            {
                File = new Dictionary<string, int>()
            };
            var number = 1;
            if (!path.IsNullOrEmpty())
            {
                var paths = path.Split('|');
                foreach (var item in paths)
                {
                    if (item.IsNullOrEmpty())
                    {
                        continue;
                    }
                    model.File.Add(item, number++);

                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public FileResult Image(ImageViewDownload model)
        {
            var paths = model.itemValue.Split('|');
            string fileName=string.Empty;
            string mimeType=string.Empty;
            foreach (var item in paths)
            {
                if (!item.IsNullOrEmpty()&&item!="")
                {
                     fileName = Regex.Replace(item, Regex.Escape("/userfiles"), "", RegexOptions.IgnoreCase);//model.itemValue;
                     mimeType = MimeMapping.GetMimeMapping(fileName);
                   
                }
            }
            Stream stream = fileHander.LoadFile("userfiles", fileName);
            return new FileStreamResult(stream, mimeType);
        }

        public ActionResult ChangeAddCheckItem()
        {
            return View();
        }


        public ActionResult EducationAddCheckItem()
        {
            return View();
        }

        public ActionResult AwardsAddCheckItem()
        {
            return View();
        }

        public ActionResult PunishAddCheckItem()
        {
            return View();
        }
        public ActionResult EditFile(string filePath, string id, string name)
        {
            STCheckPeopleEditAttachFileModel model = new STCheckPeopleEditAttachFileModel()
            {
                Id=string.Empty,
                paths = new Dictionary<int, string>()
            };
            if (!filePath.IsNullOrEmpty())
            {
                model.path = filePath;
                int i = 1;
                var paths = filePath.Split('|').ToList();
                foreach (var item in paths)
                {
                    if (!item.IsNullOrEmpty())
                    {
                        model.paths.Add(i++, item);
                    }
                }
            }
          
            model.Id = id;
            return View(model);
        }

        [HttpPost]
        public ActionResult attachUpload(string customName)
        {
            var allSTUnit = GetCurrentInstsST();
            var customids = allSTUnit.Where(r => r.Value == customName).Select(r => r.Key).ToList();
            string customid = string.Empty;
            if (customids != null && customids.Count > 0)
            {
                customid = customids.First();
            }

            string realname = Request.Files["file"].FileName;

            var extensionName = System.IO.Path.GetExtension(realname);

            string filename = "/{0}/{1}{2}".Fmt(customid, Guid.NewGuid().ToString().Replace("-", ""), extensionName);

            fileHander.UploadFile(Request.Files["file"].InputStream, "userfiles", filename);

            DhtmlxUploaderResult uploader = new DhtmlxUploaderResult() { state = true, name = filename };
            string str = uploader.ToJson();
            return Content(str);
        }


        public ActionResult STPeopleDetails(STPeopleDetailsViewModel model )
        {
            return View(model);
        }
    }
}
