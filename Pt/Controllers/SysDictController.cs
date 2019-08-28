using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Entity.DTO;
using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using Pkpm.Core.SysDictCore;
using PkpmGX.Models;
using ServiceStack;
using Dhtmlx.Model.Grid;
using Dhtmlx.Model.TreeView;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class SysDictController : PkpmController
    {

        IRepsitory<SysDict> rep;
        ISysDictService dictService;

        public SysDictController(IRepsitory<SysDict> rep,
            ISysDictService dictService,
            IUserService userService) : base(userService)
        {
            this.rep = rep;
            this.dictService = dictService;
        }

        // GET: SysDict
        public ActionResult Index()
        {
            return View();
        }

        // GET: SysDict/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SysDict/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysDict/Create字典类别
        [HttpPost]
        public ActionResult Create(NewDictViewModel newDictViewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;

            SysDict sysDict = new SysDict
            {
                Name = newDictViewModel.DictName,
                KeyValue = newDictViewModel.KeyValue,
                OrderNo = newDictViewModel.DictOrderNo,
                Status = newDictViewModel.DictStatus,
                CategoryId = newDictViewModel.CategoryId
            };
            string errorMsg = string.Empty;
            var editResult = dictService.AddSysDict(sysDict, out errorMsg);
            if (!editResult)
            {
                result.IsSucc = false;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("进行了字典类别新增操作，字典名称为{0}，编码为{1}，序号为{2}，状态为{3}，父节点id为{4}".Fmt(newDictViewModel.DictName,
                    newDictViewModel.KeyValue,
                    newDictViewModel.DictOrderNo,
                    newDictViewModel.DictStatus,
                    newDictViewModel.CategoryId));
            }
            return Content(result.ToJson());
        }

        // GET: SysDict/Edit/5
        public ActionResult Edit(int id)
        {
            var sysDict = rep.GetById(id);
            string str = sysDict.ToJson();
            return Content(str);
        }

        // POST: SysDict/Edit/5
        [HttpPost]
        public ActionResult Edit(EditDictViewModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;

            SysDict sysDict = new SysDict
            {
                Id = model.SysDictId,
                Name = model.DictName,
                KeyValue = model.KeyValue == null ? string.Empty : model.KeyValue,
                OrderNo = model.DictOrderNo,
                Status = model.DictStatus,
                CategoryId = model.CategoryId
            };
            string errorMsg = string.Empty;
            var editResult = dictService.EditSysDict(sysDict, out errorMsg);
            if (!editResult)
            {
                result.IsSucc = false;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("对id为{0}进行了字典类别修改操作，字典名称为{1}，编码为{2}，序号为{3}，状态为{4}，父节点id为{5}".Fmt(model.SysDictId,
                    model.DictName,
                    model.KeyValue,
                    model.DictOrderNo,
                    model.DictStatus,
                    model.CategoryId));
            }
            return Content(result.ToJson());
        }

        // GET: SysDict/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SysDict/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;

            string errorMsg = string.Empty;
            var editResult = dictService.DeleteSysDict(id, out errorMsg);
            if (!editResult)
            {
                result.IsSucc = false;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("对id为{0}进行了字典类别删除操作".Fmt(id));
            }
            return Content(result.ToJson());
        }

        public ActionResult Grid()
        {
            var sysDicts = rep.GetByCondition(r => r.Id > 0);

            string str = GetGridXmlString(sysDicts);

            return Content(str, "text/xml");
        }

        private static string GetGridXmlString(List<SysDict> sysDicts)
        {
            DhtmlxGrid grid = new DhtmlxGrid();

            foreach (var item in sysDicts.Where(p => p.CategoryId < 0))
            {
                DhtmlxGridRow row = new DhtmlxGridRow(item.Id);
                row.AddCell(item.Name);
                row.AddCell(item.KeyValue);
                row.AddCell("");
                row.AddCell(item.Status);
                row.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                row.AddCell(new DhtmlxGridCell("删除", false).AddCellAttribute("title", "删除"));

                foreach (var subItem in sysDicts.Where(p => p.CategoryId == item.Id))
                {
                    DhtmlxGridRow subRow = new DhtmlxGridRow(subItem.Id.ToString());
                    subRow.AddCell(subItem.Name);
                    subRow.AddCell(subItem.KeyValue);
                    subRow.AddCell(subItem.OrderNo);
                    subRow.AddCell(subItem.Status);
                    subRow.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                    subRow.AddCell(new DhtmlxGridCell("删除", false).AddCellAttribute("title", "删除"));

                    row.AddRow(subRow);
                }

                grid.AddGridRow(row);
            }

            return grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
        }

        public ActionResult Category()
        {
            List<SysDict> sysDicts = rep.GetByCondition(r => r.CategoryId < 0);

            DhtmlxTreeView treeView = new DhtmlxTreeView();
            DhtmlxTreeViewItem item = new DhtmlxTreeViewItem("Dict", "字典类别", true, false, DhtmlxTreeViewCheckbox.NotSet);
            foreach (var dictItem in sysDicts)
            {
                item.AddTreeViewItem(dictItem.Id, dictItem.Name);
            }
            treeView.AddTreeViewItem(item);

            string str = treeView.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        // GET: SysDict/Search
        public ActionResult Search(SearchDictViewModel viewModel)
        {
            List<SysDict> sysDicts = new List<SysDict>();
            if (!string.IsNullOrWhiteSpace(viewModel.DictName))
            {
                //获取字典类别
                sysDicts = rep.GetByCondition(r => r.CategoryId == -1 && r.Name.Contains(viewModel.DictName));
                if (sysDicts.Count > 0)
                {
                    var subDicts = rep.GetByCondition(p => sysDicts.Select(s => s.Id).ToList().Contains(p.CategoryId));
                    sysDicts.AddRange(subDicts);
                }

            }
            else
            {
                sysDicts = rep.GetByCondition(r => r.Id > 0);
            }

            string str = GetGridXmlString(sysDicts);
            return Content(str, "text/xml");
        }
    }
}

