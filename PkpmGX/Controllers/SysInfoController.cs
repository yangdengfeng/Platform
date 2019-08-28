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
using Pkpm.Framework.Repsitory;
using Dhtmlx.Model.Grid;
using Dhtmlx.Model.Toolbar;
using Pkpm.Entity.DTO;
using Pkpm.Core.SysInfoCore;
using ServiceStack;
using Pkpm.Core.SysDictCore;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class SysInfoController : PkpmController
    {
        IRepsitory<t_bp_PkpmJCRU> rep;
        ISysInfoService sysInfoService;
        ISysDictService sysDictService;
        public SysInfoController(IUserService userService, 
            IRepsitory<t_bp_PkpmJCRU> rep,
            ISysInfoService sysInfoService,
             ISysDictService sysDictService) : base(userService)
        {
            this.rep = rep;
            this.sysInfoService = sysInfoService;
            this.sysDictService = sysDictService;
        }

        // GET: SysInfo
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult ToolBar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            var buttons = GetCurrentUserPathActions();
            if (HaveButtonFromAll(buttons, "Create"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Create", "新增") { Img = "fa fa-plus", Imgdis = "fa fa-plus" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("1"));
            }
            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        // GET: SysInfo/Details/5
        public ActionResult Details(int id)
        {
           
            return View();
        }

        // GET: SysInfo/Create
        public ActionResult Create()
        {
            var model = sysDictService.GetDictsByKey("infoType");
            return View(model);
        }

        // POST: SysInfo/Create
        [HttpPost]
        public ActionResult Create(SysInfoCreateModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            var addTiem = GetUIDtString(DateTime.Now);
            try
            {
                string erroMsg = string.Empty;
                if (!sysInfoService.CreateInfo( model.Name, model.Content, model.Type,addTiem,out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = true;
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

        // GET: SysInfo/Edit/5
        public ActionResult Edit(int id)
        {

            var model = new SysInfoEditViewModel();
            model.PkpmJCRU = rep.GetById(id);
            model.Type = sysDictService.GetDictsByKey("infoType");
            return View(model);
        }

        // POST: SysInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(SysInfoEditModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!sysInfoService.EditInfo(int.Parse(model.Id), model.informationName, model.Content,model.Type, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = true;
                }
            }catch(Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
                result.IsSucc = false;
            }

            return Content(result.ToJson());
        }

        // GET: SysInfo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SysInfo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!sysInfoService.DeleteInfo(id, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对信息ID为{0}进行了删除操作".Fmt(id));
                }

            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        public ActionResult Search(SysInfoSearchModel model)
        {
            var data = GetSearchResult(model);
            DhtmlxGrid grid = new DhtmlxGrid();
            var buttons = GetCurrentUserPathActions();
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            grid.AddPaging(data.TotalCount, pos);
            for(int i = 0; i < data.Results.Count; i++)
            {
                var information = data.Results[i];
                DhtmlxGridRow row = new DhtmlxGridRow(information.Id.ToString());
                row.AddCell(pos + i + 1);
                row.AddCell(information.Name);
                row.AddCell(information.Content);
                row.AddCell(GetUIDtString(information.Time));
                if (HaveButtonFromAll(buttons, "Edit"))
                {
                    row.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                }
                else
                {
                    row.AddCell(string.Empty);
                }
                if (HaveButtonFromAll(buttons, "Delete"))
                {
                    row.AddCell(new DhtmlxGridCell("删除", false).AddCellAttribute("title", "删除"));
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

        private SearchResult<SysInfoSearchUIModel> GetSearchResult(SysInfoSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            var predicate = PredicateBuilder.True<t_bp_PkpmJCRU>();
            if (!string.IsNullOrEmpty(model.InformationName))
            {
                predicate = predicate.And(t => t.name.Contains(model.InformationName));//.Contains(t.name));
            }
            string sortProperty = "addtime";
            PagingOptions<t_bp_PkpmJCRU> pagingOption = new PagingOptions<t_bp_PkpmJCRU>(pos, count, sortProperty, true);

            var information = rep.GetByConditionSort<SysInfoSearchUIModel>(predicate, r => new { r.ID, r.name, r.content, r.addtime }, pagingOption);
            return new SearchResult<SysInfoSearchUIModel>(pagingOption.TotalItems, information);
        }
    }
}
