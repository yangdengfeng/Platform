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
using Pkpm.Entity.DTO;
using Pkpm.Core.SoftwareVersService;
using ServiceStack;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class SysSoftwareVersController : PkpmController
    {
        IRepsitory<t_sys_Version> rep;
        ISoftwareVersService softwareVersService;
        public SysSoftwareVersController(IUserService userService, 
            IRepsitory<t_sys_Version> rep,
            ISoftwareVersService softwareVersService) : base(userService)
        {
            this.softwareVersService = softwareVersService;
            this.rep = rep;
        }

        // GET: SysSoftwareVers
        public ActionResult Index()
        {
            return View();
        }

        // GET: SysSoftwareVers/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SysSoftwareVers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysSoftwareVers/Create
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

        // GET: SysSoftwareVers/Edit/5
        public ActionResult Edit(int id)
        {
            var model = rep.GetById(id);
            return View(model);
        }

        // POST: SysSoftwareVers/Edit/5
        [HttpPost]
        public ActionResult Edit(SysSoftwareVersEditModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!softwareVersService.EditSoftwareVers(model.id, model.UserCode, model.UserName, model.FileVersionDate,model.FileVersion,model.EndDate, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = true;
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

        // GET: SysSoftwareVers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SysSoftwareVers/Delete/5
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

        public ActionResult Search(SysSoftwareVersSearchModel model)
        {
            var data = GetSearchResult(model);
            DhtmlxGrid grid = new DhtmlxGrid();
            var buttons = GetCurrentUserPathActions();
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            grid.AddPaging(data.TotalCount, pos);
            for (int i = 0; i < data.Results.Count; i++)
            {
                var softwareVers = data.Results[i];
                DhtmlxGridRow row = new DhtmlxGridRow(softwareVers.id.ToString());
                row.AddCell(pos + i + 1);
                row.AddCell(softwareVers.usercode);
                row.AddCell(softwareVers.name);
                row.AddCell(softwareVers.FileVersion);
                row.AddCell(softwareVers.FileVersionDate);
                row.AddCell(softwareVers.EndDate);

                ///暂时未找到数据来源
                row.AddCell(string.Empty);
                row.AddCell(string.Empty);
                row.AddCell(string.Empty);
              
                if (HaveButtonFromAll(buttons, "Edit"))
                {
                    row.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                }
                else
                {
                    row.AddCell(string.Empty);
                }
                //if (HaveButtonFromAll(buttons, "Delete"))
                //{
                //    row.AddCell(new DhtmlxGridCell("删除", false).AddCellAttribute("title", "删除"));
                //}
                //else
                //{
                //    row.AddCell(string.Empty);
                //}
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");

        }

        private SearchResult<t_sys_Version> GetSearchResult(SysSoftwareVersSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            var predicate = PredicateBuilder.True<t_sys_Version>();
            if (!string.IsNullOrEmpty(model.Name))
            {
                predicate = predicate.And(t => t.name.Contains(model.Name));//.Contains(t.name));
            }
            string sortProperty = "FileVersionDate";
            PagingOptions<t_sys_Version> pagingOption = new PagingOptions<t_sys_Version>(pos, count, sortProperty, true);

            var information = rep.GetByConditionSort<t_sys_Version>(predicate, r => new { r.id, r.usercode, r.name, r.FileVersion,r.FileVersionDate,r.EndDate }, pagingOption);
            return new SearchResult<t_sys_Version>(pagingOption.TotalItems, information);
        }
    }
}
