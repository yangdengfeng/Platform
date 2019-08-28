using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Framework.Repsitory;
using Pkpm.Entity;
using Pkpm.Core.PathCore;
using Dhtmlx.Model.Menu;
using PkpmGX.Models;
using ServiceStack.OrmLite;
using Dhtmlx.Model.Grid;
using ServiceStack;
using Dhtmlx.Model.TreeView;
using Pkpm.Entity.DTO;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class SysMenuController : PkpmController
    {
        IRepsitory<Path> rep;
        IRepsitory<Pkpm.Entity.Action> actionRep;
        IPathService pathServcie;

        public SysMenuController(IRepsitory<Path> rep,
            IRepsitory<Pkpm.Entity.Action> actionRep,
            IPathService pathServcie,
            IUserService userService) : base(userService)
        {
            this.rep = rep;
            this.actionRep = actionRep;
            this.pathServcie = pathServcie;
        }


        // GET: SysMenu
        public ActionResult Index()
        {
            return View();
        }

        // GET: SysMenu/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SysMenu/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Menu()
        {
            DhtmlxMenu menu = new DhtmlxMenu();
            DhtmlxMenuContainerItem newItem = new DhtmlxMenuContainerItem("new", "新建");
            newItem.AddMenuItem(new DhtmlxMenuContainerItem("newMenCategory", "新建模块类别"));
            newItem.AddMenuItem(new DhtmlxMenuContainerItem("newMenu", "新建模块"));
            menu.AddItem(newItem);
            menu.AddItem(new DhtmlxMenuSeparatorItem("s1"));
            DhtmlxMenuContainerItem editItem = new DhtmlxMenuContainerItem("edit", "编辑");
            editItem.AddMenuItem(new DhtmlxMenuContainerItem("EditMenuCategory", "编辑模块类别"));
            editItem.AddMenuItem(new DhtmlxMenuContainerItem("DeleteMenuCategory", "删除模块类别"));
            menu.AddItem(editItem);

            string str = menu.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult Grid(int id)
        {
            var paths = rep.GetByCondition(r => r.CategoryId == id);
            var actions = actionRep.GetByCondition(ar => paths.Select(p => p.Id).ToList().Contains(ar.PathId));
            string str = GetGridXmlString(paths, actions);

            return Content(str, "text/xml");
        }

        public ActionResult Search(SearchMenuViewModel viewModel)
        {
            bool hasPredicate = false;
            var predicate = PredicateBuilder.True<Path>();

            if (!string.IsNullOrWhiteSpace(viewModel.MenuName))
            {
                hasPredicate = true;
                predicate = predicate.And(p => p.Name.Contains(viewModel.MenuName));
            }

            var paths = hasPredicate ? rep.GetByCondition(predicate) : rep.GetByCondition(r => r.Id > 0);
            var actions = actionRep.GetByCondition(ar => paths.Select(p => p.Id).ToList().Contains(ar.PathId));
            string str = GetGridXmlString(paths, actions);
            return Content(str, "text/xml");
        }

        private static string GetGridXmlString(List<Path> paths, List<Pkpm.Entity.Action> actions)
        {
            DhtmlxGrid grid = new DhtmlxGrid();

            foreach (var item in paths.OrderBy(p => p.OrderNo))
            {
                DhtmlxGridRow row = new DhtmlxGridRow(item.Id.ToString());
                row.AddCell(item.Name);
                row.AddCell(item.Url);
                row.AddCell(item.OrderNo.ToString());
                row.AddCell(item.Status.ToString());
                row.AddLinkJsCell("按钮", "actionInPath({0})".Fmt(item.Id.ToString()));
                row.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                row.AddCell(new DhtmlxGridCell("删除", false).AddCellAttribute("title", "删除"));

                var pathActions = actions.Where(a => a.PathId == item.Id).ToList();

                foreach (var pathAction in pathActions)
                {
                    DhtmlxGridRow actionRow = new DhtmlxGridRow(string.Format("Action,{0}", pathAction.Id));
                    actionRow.AddCell(pathAction.Name);
                    actionRow.AddCell(pathAction.Url);
                    actionRow.AddCell(string.Empty);
                    actionRow.AddCell(pathAction.Status.ToString());
                    actionRow.AddCell(string.Empty);
                    actionRow.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                    actionRow.AddCell(new DhtmlxGridCell("删除", false).AddCellAttribute("title", "删除"));

                    row.AddRow(actionRow);
                }

                grid.AddGridRow(row);
            }

            return grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
        }

        public ActionResult Category()
        {
            List<Path> paths = rep.GetByCondition(r => r.IsCategory);

            DhtmlxTreeView treeView = new DhtmlxTreeView();
            DhtmlxTreeViewItem item = new DhtmlxTreeViewItem("Main", "模块类别", true, false, DhtmlxTreeViewCheckbox.NotSet);
            foreach (var menuItem in paths.OrderBy(p => p.OrderNo))
            {
                item.AddTreeViewItem(menuItem.Id.ToString(), menuItem.Name);
            }
            treeView.AddTreeViewItem(item);

            string str = treeView.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult Action(int id)
        {
            var action = actionRep.GetById(id);
            return Content(action.ToJson());
        }

        [HttpPost]
        public ActionResult EditAction(EditAcionViewModel editActionViewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;

            Pkpm.Entity.Action action = new Pkpm.Entity.Action()
            {
                Name = editActionViewModel.ActionName,
                PathId = editActionViewModel.PathId,
                Status = editActionViewModel.ActionStatus,
                Url = string.IsNullOrWhiteSpace(editActionViewModel.ActionuUrl) ? string.Empty : editActionViewModel.ActionuUrl.Trim(),
                Id = editActionViewModel.ActionId
            };
            string errorMsg = string.Empty;

            bool editResult = pathServcie.EditPathAction(action, out errorMsg);
            if (!editResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("对ID为{0}的模块按钮进行了编辑操作,按钮名称为{1}，按钮地址为{2}，是否启用为{3}".Fmt(editActionViewModel.ActionId,
                    editActionViewModel.ActionName,
                    editActionViewModel.PathId,
                    editActionViewModel.ActionStatus));
            }
            return Content(result.ToJson());
        }

        [HttpPost]
        public ActionResult NewAction(NewActonViewModel newActionViewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            Pkpm.Entity.Action action = new Pkpm.Entity.Action()
            {
                Name = newActionViewModel.ActionName,
                PathId = newActionViewModel.PathId,
                Status = newActionViewModel.ActionStatus,
                Url = string.IsNullOrWhiteSpace(newActionViewModel.ActionuUrl) ? string.Empty : newActionViewModel.ActionuUrl.Trim()
            };
            string errorMsg = string.Empty;

            bool editResult = pathServcie.AddPathAction(action, out errorMsg);
            if (!editResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("对模块按钮进行了新增操作,按钮名称为{0}，按钮地址为{1}，是否启用为{2}".Fmt(newActionViewModel.ActionName,
                    newActionViewModel.PathId,
                    newActionViewModel.ActionStatus));
            }
            return Content(result.ToJson());
        }

        // POST: SysMenu/Create
        [HttpPost]
        public ActionResult Create(NewMenuViewModel newMenuViewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            Path path = new Path()
            {
                Name = newMenuViewModel.MenuName,
                OrderNo = newMenuViewModel.MenuOrderNo,
                IsCategory = newMenuViewModel.IsCategory == 1,
                CategoryId = newMenuViewModel.CategoryId,
                Status = newMenuViewModel.MenuStatus,
                Icon = newMenuViewModel.MenuIcon,
                Url = string.IsNullOrWhiteSpace(newMenuViewModel.MenuUrl) ? string.Empty : newMenuViewModel.MenuUrl.Trim()
            };
            string errorMsg = string.Empty;

            bool editResult = pathServcie.AddPath(path, out errorMsg);
            if (!editResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("对模块进行了新增操作,模块名称为{0}，模块编号为{1}，模块状态为{2}".Fmt(newMenuViewModel.MenuName,
                    newMenuViewModel.MenuOrderNo,
                    newMenuViewModel.MenuStatus));
            }
            return Content(result.ToJson());
        }

        // GET: SysMenu/Edit/5
        public ActionResult Edit(int id)
        {
            var path = rep.GetById(id);
            string str = path.ToJson();
            return Content(str);
        }

        // POST: SysMenu/Edit/5
        [HttpPost]
        public ActionResult Edit(EditMenuViewModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            Path path = new Path()
            {
                Id = model.PathId,
                CategoryId = model.CategoryId,
                IsCategory = model.IsCategory == 1,
                Status = model.MenuStatus,
                Name = model.MenuName,
                OrderNo = model.MenuOrderNo,
                Icon = string.IsNullOrWhiteSpace(model.MenuIcon) ? string.Empty : model.MenuIcon,
                Url = string.IsNullOrWhiteSpace(model.MenuUrl) ? string.Empty : model.MenuUrl
            };
            string errorMsg = string.Empty;

            bool editResult = pathServcie.EditPath(path, out errorMsg);
            if (!editResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("对id为{0}模块进行了修改操作,父节点id为{1}，模块编号为{2}，模块状态为{3},模块名称为{4}".Fmt(model.PathId, model.CategoryId, model.MenuOrderNo, model.MenuStatus, model.MenuName));
            }
            return Content(result.ToJson());
        }

        // GET: SysMenu/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult ActionDelete(int id, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;
            bool editResult = pathServcie.DeletePathAction(id, out errorMsg);
            if (!editResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("对id为{0}模块按钮进行了删除操作".Fmt(id));
            }
            return Content(result.ToJson());
        }

        // POST: SysMenu/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;
            bool editResult = pathServcie.DeltePath(id, out errorMsg);
            if (!editResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("对id为{0}模块进行了删除操作".Fmt(id));
            }
            return Content(result.ToJson());
        }
    }
}
