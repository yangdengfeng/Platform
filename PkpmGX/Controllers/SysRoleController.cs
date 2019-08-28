using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Framework.Repsitory;
using Dhtmlx.Model.Grid;
using ServiceStack;
using Pkpm.Entity;
using Dhtmlx.Model.TreeView;
using PkpmGX.Models;
using ServiceStack.OrmLite;
using Pkpm.Entity.DTO;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class SysRoleController : PkpmController
    {
        IRepsitory<Role> rep;
        IRepsitory<Path> menuRep;
        IRoleService roleService;

        public SysRoleController(IRepsitory<Role> rep,
            IRepsitory<Path> menuRep,
            IRoleService roleService,
            IUserService userService) : base(userService)
        {
            this.rep = rep;
            this.menuRep = menuRep;
            this.roleService = roleService;
        }


        // GET: SysRole
        public ActionResult Index()
        {
            return View();
        }

        // GET: SysRole/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SysRole/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysRole/Create
        [HttpPost]
        public ActionResult Create(NewRoleViewModel newRoleModel)
        {
            ControllerResult result = ControllerResult.SuccResult;

            Role role = new Role()
            {
                Name = newRoleModel.RoleName,
                Description = newRoleModel.RoleDesc,
                Code = newRoleModel.RoleCode
            };

            string errorMsg = string.Empty;

            bool addResult = roleService.AddNewRole(role, out errorMsg);
            if (!addResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("进行新增角色操作，角色名称{0}，角色编码{1}".Fmt(newRoleModel.RoleName, newRoleModel.RoleCode));
            }
            return Content(result.ToJson());
        }

        // GET: SysRole/Edit/5
        public ActionResult Edit(int id)
        {
            var role = rep.GetById(id);
            string str = role.ToJson();
            return Content(str);
        }

        // POST: SysRole/Edit/5
        [HttpPost]
        public ActionResult Edit(EditRoleViewModel roleViewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;

            Role role = new Role()
            {
                Id = roleViewModel.RoleId,
                Name = roleViewModel.RoleName,
                Description = roleViewModel.RoleDesc,
                Code = roleViewModel.RoleCode
            };
            string errorMsg = string.Empty;

            bool editResult = roleService.EditRole(role, out errorMsg);
            if (!editResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("对id为{0}进行角色编辑操作，角色编号为{1}，角色名称为{2}".Fmt(roleViewModel.RoleId, roleViewModel.RoleCode, roleViewModel.RoleName));
            }
            return Content(result.ToJson());
        }

        public ActionResult Grid()
        {
            var roles = rep.GetByCondition(r => r.Id > 0);

            string str = GetGridXmlString(roles);

            return Content(str, "text/xml");
        }

        private string GetGridXmlString(List<Role> roles)
        {
            DhtmlxGrid grid = new DhtmlxGrid();

            int index = 1;
            foreach (var item in roles)
            {
                DhtmlxGridRow row = new DhtmlxGridRow(item.Id.ToString());
                row.AddCell(index.ToString());
                row.AddCell(item.Name);
                row.AddCell(item.Description);
                row.AddCell(item.Code);
                row.AddLinkJsCells(GetOpDicts(item.Id.ToString()));
                row.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                row.AddCell(new DhtmlxGridCell("删除", false).AddCellAttribute("title", "删除"));
                grid.AddGridRow(row);

                index++;
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return str;
        }

        private Dictionary<string, string> GetOpDicts(string id)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("[模块]", "roleModule({0})".Fmt(id));
            dict.Add("[按钮]", "roleAction({0})".Fmt(id));
            return dict;
        }

        public ActionResult MenuAction(int id)
        {
            List<Path> rolePaths = roleService.GetPathsByRole(id);
            List<Pkpm.Entity.Action> pathActons = roleService.GetActionsByPathIds(rolePaths.Select(rp => rp.Id).ToList());
            List<Pkpm.Entity.Action> roleActions = roleService.GetActionsByRole(id);

            DhtmlxTreeView treeView = new DhtmlxTreeView();
            foreach (var item in rolePaths.Where(m => !m.IsCategory))
            {
                DhtmlxTreeViewItem categoryItem = new DhtmlxTreeViewItem("Path{0}".Fmt(item.Id), item.Name, false, false, DhtmlxTreeViewCheckbox.hidden);
                bool hasAction = false;
                foreach (var subItem in pathActons.Where(ra => ra.PathId == item.Id))
                {
                    bool hasPath = roleActions.Exists(r => r.Id == subItem.Id);

                    DhtmlxTreeViewItem subTreeItem = new DhtmlxTreeViewItem(subItem.Id.ToString(), subItem.Name, false, hasPath, DhtmlxTreeViewCheckbox.NotSet);
                    categoryItem.AddTreeViewItem(subTreeItem);
                    hasAction = true;
                }
                if (hasAction)
                {
                    treeView.AddTreeViewItem(categoryItem);
                }
            }

            string str = treeView.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult Menu(int id)
        {
            List<Path> rolePaths = roleService.GetPathsByRole(id);

            var menus = menuRep.GetByCondition(m => m.Status == 1);
            DhtmlxTreeView treeView = new DhtmlxTreeView();
            foreach (var item in menus.Where(m => m.IsCategory))
            {
                DhtmlxTreeViewItem categoryItem = new DhtmlxTreeViewItem(item.Id.ToString(), item.Name, false, false, DhtmlxTreeViewCheckbox.hidden);
                foreach (var subItem in menus.Where(su => su.CategoryId == item.Id))
                {
                    bool hasPath = rolePaths.Exists(r => r.Id == subItem.Id);

                    DhtmlxTreeViewItem subTreeItem = new DhtmlxTreeViewItem(subItem.Id.ToString(), subItem.Name, false, hasPath, DhtmlxTreeViewCheckbox.NotSet);
                    categoryItem.AddTreeViewItem(subTreeItem);
                }
                treeView.AddTreeViewItem(categoryItem);
            }

            string str = treeView.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult Search(SearchRoleViewModel viewModel)
        {
            bool hasPredicate = false;
            var predicate = PredicateBuilder.True<Role>();

            if (!string.IsNullOrWhiteSpace(viewModel.RoleCode))
            {
                hasPredicate = true;
                predicate = predicate.And(r => r.Code.Contains(viewModel.RoleCode));
            }

            if (!string.IsNullOrWhiteSpace(viewModel.RoleName))
            {
                hasPredicate = true;
                predicate = predicate.And(r => r.Name.Contains(viewModel.RoleName));
            }

            var roles = hasPredicate ? rep.GetByCondition(predicate) : rep.GetByCondition(r => r.Id > 0);
            string str = GetGridXmlString(roles);
            return Content(str, "text/xml");
        }


        [HttpPost]
        public ActionResult AddAction(int id, string actionIds)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                // TODO: Add update logic here
                List<int> actionIdList = new List<int>();
                string errorMsg = string.Empty;
                bool addResult = true;
                if (actionIds.IsNullOrEmpty()) //增加对没有任何按钮权限的判断 18.9.21
                {
                    addResult = roleService.AddActionsIntoRole(actionIdList, id, out errorMsg);
                }
                else
                {
                    foreach (var item in actionIds.Split(','))
                    {
                        actionIdList.Add(int.Parse(item));
                    }
                    addResult = roleService.AddActionsIntoRole(actionIdList, id, out errorMsg);

                }
                if (!addResult)
                {
                    result.IsSucc = addResult;
                    result.ErroMsg = errorMsg;
                }
                else
                {
                    LogUserAction("对id为{0}进行新增角色按钮操作".Fmt(id));
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
        public ActionResult AddMenu(int id, string menuIds)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                // TODO: Add update logic here
                string errorMsg = string.Empty;
                List<int> pathIds = new List<int>();
                bool addResult = true;
                if (menuIds.IsNullOrEmpty())
                {
                    addResult = roleService.AddPathsIntoRole(pathIds, id, out errorMsg);
                }
                else
                {
                    foreach (var item in menuIds.Split(','))
                    {
                        pathIds.Add(int.Parse(item));
                    }
                    addResult = roleService.AddPathsIntoRole(pathIds, id, out errorMsg);
                }
                if (!addResult)
                {
                    result.IsSucc = addResult;
                    result.ErroMsg = errorMsg;
                }
                else
                {
                    LogUserAction("对id为{0}进行角色模块设置操作".Fmt(id));
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
        public ActionResult Delete(int id)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;

            bool delResult = roleService.DeleteRole(id, out errorMsg);
            if (!delResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("对id为{0}进行角色删除操作".Fmt(id));
            }
            return Content(result.ToJson());
        }
    }
}
