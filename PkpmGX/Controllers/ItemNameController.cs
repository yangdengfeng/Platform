using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Dhtmlx.Model.Toolbar;
using Dhtmlx.Model.Menu;
using Pkpm.Framework.Repsitory;
using Pkpm.Entity;
using Dhtmlx.Model.Grid;
using System.Xml.Linq;
using Pkpm.Core.ItemNameCore;
using Dhtmlx.Model.TreeView;
using ServiceStack;
using Pkpm.Entity.DTO;

namespace PkpmGX.Controllers
{
    public class ItemNameController : PkpmController
    {
        IRepsitory<totalitems> rep;
        IRepsitory<CheckItemType> typerep;
        IItemNameService itemNameService;

        public ItemNameController(IUserService userService,
            IRepsitory<totalitems> rep,
            IItemNameService itemNameService,
            IRepsitory<CheckItemType> typerep) : base(userService)
        {
            this.rep = rep;
            this.typerep = typerep;
            this.itemNameService = itemNameService;
        }

        // GET: ItemName
        public ActionResult Index()
        {
            return View();
        }

        // GET: ItemName/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ItemName/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemName/Create
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

        // GET: ItemName/Edit/5
        public ActionResult Edit(int id)
        {
            var parm = rep.GetById(id);
            return Content(parm.ToJson());
        }

        // POST: ItemName/Edit/5
        [HttpPost]
        public ActionResult Edit(totalitems model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;
            var editResult = itemNameService.EditTotalItem(model, out errorMsg);
            if (!editResult)
            {
                result.IsSucc = false;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("对id为{0}进行了修改检测项目参数操作，检测项目参数名称{1}".Fmt(model.Id, model.ItemName));
            }
            return Content(result.ToJson());
        }

        // GET: ItemName/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ItemName/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;
            var DelResult = itemNameService.DelTotalItemById(id, out errorMsg);
            if (!DelResult)
            {
                result.IsSucc = false;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("对id为{0}进行了删除检测项目参数操作".Fmt(id));
            }
            return Content(result.ToJson());
        }

        public ActionResult Toolbar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("NewItemParm", "新建检测项目参数") { Img = "fa fa-clone", Imgdis = "fa fa-clone" });
            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult Menu()
        {
            DhtmlxMenu menu = new DhtmlxMenu();
            DhtmlxMenuContainerItem newType = new DhtmlxMenuContainerItem("new", "新建");
            newType.AddMenuItem(new DhtmlxMenuContainerItem("newType", "新建类别"));
            //newItem.AddMenuItem(new DhtmlxMenuContainerItem("newItem", "新建项目"));
            menu.AddItem(newType);
            //menu.AddItem(new DhtmlxMenuSeparatorItem("s1"));
            string str = menu.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult ParmDetails(string id)
        {
            var parms = rep.GetByCondition(t => t.typecode == id);
            DhtmlxGrid grid = new DhtmlxGrid();
            for (int i = 0; i < parms.Count; i++)
            {
                var parm = parms[i];
                DhtmlxGridRow row = new DhtmlxGridRow(parm.Id.ToString());
                row.AddCell((i + 1).ToString());
                row.AddCell(parm.typecode);
                row.AddCell(parm.itemcode);
                row.AddCell(parm.ItemName);
                row.AddCell(parm.itemtype);

                row.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                row.AddCell(new DhtmlxGridCell("删除", false).AddCellAttribute("title", "删除"));
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult NameCate()
        {
            XElement rootElem = BuildItemCodeTree(itemNameService);

            var itemTypes = itemNameService.GetAllTypeCodes();
            var itemCodes = itemNameService.GetAllItemCodes();

            DhtmlxTreeView treeView = new DhtmlxTreeView();

            bool isOpen = true;
            foreach (var itemType in itemTypes)
            {
                DhtmlxTreeViewItem typeItem = new DhtmlxTreeViewItem(itemType.CheckItemCode, "{0}({1})".Fmt(itemType.CheckItemName, itemType.CheckItemCode), isOpen,false, DhtmlxTreeViewCheckbox.NotSet);
                //isOpen = false;

                foreach (var codeItem in itemCodes.Where(f => f.itemcode != null && f.itemcode.Contains(itemType.CheckItemCode)))
                {
                    if (codeItem != null)
                    { typeItem.AddTreeViewItem(codeItem.itemcode, "{0}".Fmt(codeItem.itemcode)); }
                }

                treeView.AddTreeViewItem(typeItem);
            }
            string str = treeView.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");

        }

        [HttpPost]
        public ActionResult NewCheckType(CheckItemType model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;
            bool newResult = itemNameService.NewCheckItemType(model, out errorMsg);
            if (!newResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("进行了新建检测类别操作，检测类别编号为{0}，检测类别名称{1}".Fmt(model.CheckItemCode, model.CheckItemName));
            }
            return Content(result.ToJson());

        }

        [HttpPost]
        public ActionResult NewCheckItem(totalitems model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;
            bool newResult = itemNameService.NewTotalItem(model, out errorMsg);
            if (!newResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("进行了新建检测项目操作，检测类别编号为{0}，检测项目编号{1}，检测项目名称{2}".Fmt(model.typecode,
                    model.itemcode,
                    model.ItemName));
            }
            return Content(result.ToJson());
        }




    }
}
