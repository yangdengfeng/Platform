using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using PkpmGX.Models;
using Pkpm.Entity;
using ServiceStack.OrmLite;
using ServiceStack;
using Pkpm.Framework.Repsitory;
using Dhtmlx.Model.Grid;
using Pkpm.Core.SysDictCore;
using Pkpm.Framework.Common;
using Pkpm.Core.STCustomCore;
using Pkpm.Entity.DTO;
using Pkpm.Core.STCheckEquipCore;
using Dhtmlx.Model.Toolbar;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class STCheckEquipController : PkpmController
    {
        ISTCheckEquipService sTCheckEquipService;
        ISysDictService sysDictServcie;
        ISTCustomService sTCustomService;
        IRepsitory<SupvisorJob> supvisorRep;
        IRepsitory<t_bp_Equipment_ST> rep;
        public STCheckEquipController(IUserService userService,
           IRepsitory<t_bp_Equipment_ST> rep,
            IRepsitory<SupvisorJob> supvisorRep,
        ISTCustomService sTCustomService,
           ISysDictService sysDictServcie,
           ISTCheckEquipService sTCheckEquipService) : base(userService)
        {
            this.supvisorRep = supvisorRep;
            this.sysDictServcie = sysDictServcie;
            this.sTCustomService = sTCustomService;
            this.rep = rep;
            this.sTCheckEquipService = sTCheckEquipService;
        }

        public ActionResult ToolBar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            var buttons = GetCurrentUserPathActions();
            if (HaveButtonFromAll(buttons, "Create"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("NewSTCheckEquip", "新增") { Img = "fa fa-plus", Imgdis = "fa fa-plus" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("1"));
            }
                        
            if (HaveButtonFromAll(buttons, "Delete"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("DeleteEquip", "[删除]") { Img = "fa fa-times", Imgdis = "fa fa-times" });
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


        // GET: STCheckEquip
        public ActionResult Index()
        {
            var model = new STCheckEquipViewModel();
            model.Status= sysDictServcie.GetDictsByKey("customStatus");
            return View(model);
        }

        // GET: STCheckEquip/Details/5
        public ActionResult Details(string id)
        {
            var model = GetSTEquipByid(id);
            return View(model);
        }

        private STCheckEquipModel GetSTEquipByid(string id)
        {
            var allSTUnit = GetCurrentInstsST();
            
            var equipment=rep.GetById(id);
            var model = new STCheckEquipModel()
            {
                id = equipment.Id.ToString(),
                CustomId = equipment.customid,
                EquipName = equipment.EquName,
                EquipSpec = equipment.equspec,
                EquipType = equipment.equtype,
                BuyTime = equipment.buytime.HasValue ? GetUIDtString(equipment.buytime.Value, "yyyy-MM-dd") : "",
                TimeStart = equipment.timestart.HasValue ? GetUIDtString(equipment.timestart.Value, "yyyy-MM-dd") : "",
                TimeEnd = equipment.timeend.HasValue ? GetUIDtString(equipment.timeend.Value, "yyyy-MM-dd") : "",
                CheckTime = equipment.checktime.HasValue ? GetUIDtString(equipment.checktime.Value, "yyyy-MM-dd") : "",
                CustomName= sTCustomService.GetSTUnitByIdFromAll(allSTUnit, equipment.customid),
               
            };
            model.Time = model.TimeStart + " - " + model.TimeEnd;

            return model;
        }

        // GET: STCheckEquip/Create
        public ActionResult Create()
        {
            var model = new STCheckEquipEditModel();
            model.allSTUnit = GetCurrentInstsST();
            return View(model);
           
        }

        // POST: STCheckEquip/Create
        [HttpPost]
        public ActionResult Create(STCheckEquipModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            result.IsSucc = true;
            try
            {
                DateTime? timeStar = null, timeEnd = null;
                CommonUtils.GetLayuiDateRange(model.Time, out timeStar, out timeEnd);
                var editModel = new STCheckEquipEditServiceModel()
                {
                    Customid = model.CustomName,
                    EquName = model.EquipName,
                    equspec = model.EquipSpec,
                    equtype = model.EquipType,
                    TimeStart = timeStar,
                    TimeEnd = timeEnd,
                };
                if (!model.BuyTime.IsNullOrEmpty())
                {
                    editModel.buyTime = DateTime.Parse(model.BuyTime);
                }
                if (!model.CheckTime.IsNullOrEmpty())
                {
                    editModel.checktime = DateTime.Parse(model.CheckTime);
                }
                string erroMsg = string.Empty;
                if (!sTCheckEquipService.CreateCheckEquip(editModel, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = false;
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

        // GET: STCheckEquip/Edit/5
        public ActionResult Edit(string id)
        {
            var model = new STCheckEquipEditModel();
            model.STCheckEquip= GetSTEquipByid(id);
            model.allSTUnit = GetCurrentInstsST();
            return View(model);
            
        }

        // POST: STCheckEquip/Edit/5
        [HttpPost]
        public ActionResult Edit(STCheckEquipModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            result.IsSucc = true;
            try
            {
                DateTime? timeStar = null, timeEnd = null;
                CommonUtils.GetLayuiDateRange(model.Time, out timeStar, out timeEnd);
                var editModel = new STCheckEquipEditServiceModel()
                {
                    id = model.id,
                    Customid = model.CustomName,
                    EquName = model.EquipName,
                    equspec = model.EquipSpec,
                    equtype = model.EquipType,
                    TimeStart = timeStar,
                    TimeEnd = timeEnd,
                    //checktime =   DateTime.Parse(model.CheckTime),
                };
                if (!model.BuyTime.IsNullOrEmpty()){
                    editModel.buyTime = DateTime.Parse(model.BuyTime);
                }
                if (!model.CheckTime.IsNullOrEmpty())
                {
                    editModel.checktime = DateTime.Parse(model.CheckTime);
                }
                string erroMsg = string.Empty;
                if(!sTCheckEquipService.EditCheckEquip(editModel,out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = false;
                }
            }
            catch(Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
                result.IsSucc = false;
            }

            return Content(result.ToJson());
        }

        // GET: STCheckEquip/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: STCheckEquip/Delete/5
        [HttpPost]
        public ActionResult Delete(string Id)
        {
            ControllerResult result = ControllerResult.SuccResult;
            result.IsSucc = true;
            try
            {
                string erroMsg = string.Empty;
                if (!sTCheckEquipService.DeleteEquip(Id, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = false;
                }
                else
                {
                    LogUserAction("对id为{0}的商砼设备信息进行了删除操作".Fmt(Id));
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

        public ActionResult Search(STCheckEquipModels model)
        {
            var data = GetSearchResult(model);
            DhtmlxGrid grid = new DhtmlxGrid();
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            grid.AddPaging(data.TotalCount, pos);
            var buttons = GetCurrentUserPathActions();
            var allSTUnit = GetCurrentInstsST();
            var compayTypes = sysDictServcie.GetDictsByKey("customStatus");
            for(int i = 0; i < data.Results.Count; i++)
            {
               
                var equip = data.Results[i];
                var start= equip.timestart.HasValue? GetUIDtString(equip.timestart.Value, "yyyy-MM-dd"):"";
                 var end = equip.timeend.HasValue? GetUIDtString(equip.timeend.Value, "yyyy-MM-dd"): "";
                DhtmlxGridRow row = new DhtmlxGridRow(equip.id.ToString());
                row.AddCell("");
                row.AddCell(pos + i + 1);
                row.AddCell(equip.EquName+ equip.Equtype + equip.Equspec);
                row.AddCell( sTCustomService.GetSTUnitByIdFromAll(allSTUnit,equip.customid));
                if (start != "" && end != "")
                {
                    row.AddCell(start + "至" + end);
                }
                else
                {
                    row.AddCell(string.Empty);
                }
               
                row.AddCell(SysDictUtility.GetKeyFromDic(compayTypes, equip.approvalstatus));
                Dictionary<string, string> dict = new Dictionary<string, string>();
                if (HaveButtonFromAll(buttons, "ApplyChange") && sTCheckEquipService.CanApplyChangeCustom(equip.approvalstatus))
                {
                    dict.Add("[申请修改]", "applyChange({0},\"{1}\")".Fmt(equip.id, equip.EquName));
                }

                if (HaveButtonFromAll(buttons, "ConfirmApplyChange") && (equip.approvalstatus == "5"))
                {
                    dict.Add("[审核申请修改]", "confirmApplyChange({0},\"{1}\")".Fmt(equip.id, equip.EquName));
                }
                row.AddLinkJsCells(dict);
                if (HaveButtonFromAll(buttons, "Edit")&&(equip.approvalstatus=="0"||equip.approvalstatus == "5"))// && checkUnitService.CanEditCustom(custom.APPROVALSTATUS))
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

        private SearchResult<STCheckUIModel> GetSearchResult(STCheckEquipModels model)
        {
            
            var predicate = PredicateBuilder.True<t_bp_Equipment_ST>();
            //过滤已经删除的机构
            predicate = predicate.And(tp => tp.data_status == null || tp.data_status != "-1");
            if (!model.EquName.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.EquName.Contains(model.EquName));
            }
            if (!model.EquType.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.equtype.Contains(model.EquType));
            }
            if (!model.Status.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.equtype==model.Status);
            }
            if (!model.CheckUnitName.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.customid==model.CheckUnitName);
            }

            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;

            PagingOptions<t_bp_Equipment_ST> pagingOption = new PagingOptions<t_bp_Equipment_ST>(pos, count, t => new { t.Id });
            var equips = rep.GetByConditonPage<STCheckUIModel>(predicate, r => new {
                r.Id,
                r.customid,
                r.EquName,
                r.equtype,
                r.equspec,
                r.timestart,
                r.timeend,
                r.approvalstatus
            }, pagingOption);

            return new SearchResult<STCheckUIModel>(pagingOption.TotalItems, equips);
        }

        //批量返回状态
        [HttpPost]
        public ActionResult SetInstState(string selectedId, string state, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            result.IsSucc = true;
            try
            {
                string erroMsg = string.Empty;
                if (!sTCheckEquipService.SetInstSendState(selectedId, state, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = false;
                }
                else
                {
                    LogUserAction("对商砼设备ID为{0}进行了状态返回操作".Fmt(selectedId));
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

        public ActionResult Applychange(string ID)
        {
            var model = new STCheckEquipApplyChangeViewModel();
            model.ID = ID;
            return View(model);

        }

        [HttpPost]
        public ActionResult ApplyChange(STCheckEquipApplyChange applyChangeModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                SupvisorJob job = new SupvisorJob()
                {
                    ApproveType = ApproveType.STApproveEquip,
                    CreateBy = GetCurrentUserId(),
                    CustomId = applyChangeModel.SubmitId,
                    CreateTime = DateTime.Now,
                    NeedApproveId = applyChangeModel.SubmitId.ToString(),
                    NeedApproveStatus = NeedApproveStatus.CreateForChangeApply,
                    SubmitName = applyChangeModel.SubmitName,
                    SubmitText = applyChangeModel.SubmitText
                };


                string erroMsg = string.Empty;
                if (!sTCheckEquipService.ApplyChangeForCustom(job, applyChangeModel.SubmitId, out erroMsg))
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

        public ActionResult GetSuperJob(string Id)
        {
            SupvisorJob Result = new SupvisorJob();
            var model = supvisorRep.GetByCondition(w => w.CustomId == Id && w.NeedApproveId == Id && w.ApproveType == ApproveType.STApproveEquip).OrderByDescending(o => o.CreateTime);
            return View(model.FirstOrDefault());

        }
        
        [HttpPost]
        public ActionResult ConfirmApplyChange(STCheckEquipApplyChange applyChangeModel)
        {
            var Status = applyChangeModel.Status == "yes" ? "0" : "7";
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                result.IsSucc = true;
                if (!sTCheckEquipService.UpdateCustomStatus(applyChangeModel.SubmitId, Status, "", out erroMsg))
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


        public ActionResult STEquipDetails(string customid)
        {
            ViewBag.customId = customid;
            return View();
        }
    }
}
