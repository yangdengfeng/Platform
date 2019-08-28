using Dhtmlx.Model.Grid;
using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Framework.Cache;
using Pkpm.Entity;
using Pkpm.Entity.ElasticSearch;
using Pkpm.Entity.DTO;
using ServiceStack;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class SysCacheController : PkpmController
    {
        ICache<InstShortInfos> cacheInst;
        ICache<ItemShortInfos> cacheItem;
        ICache<List<Path>> cachePath;
        ICache<List<Pkpm.Entity.Action>> cacheActions;
        ICache<Role> cacheRole;
        ICache<List<UserInArea>> cacheUserInArea;
        ICache<List<SysDict>> cacheDcit;
        ICache<List<UIArea>> cacheAreas;
        //ICache<List<t_hr_JsonColumn>> cacheJsonColumn;
        ICache<ESTHrItem> esHrItemCache;
        ICache<UserInspect> cacheUserInspect;
        public SysCacheController(IUserService userService,
            ICache<InstShortInfos> cacheInst,
            ICache<ItemShortInfos> cacheItem,
            ICache<List<Path>> cachePath,
            ICache<List<Pkpm.Entity.Action>> cacheActions,
            ICache<Role> cacheRole,
            ICache<List<UserInArea>> cacheUserInArea,
            ICache<List<SysDict>> cacheDcit,
            ICache<List<UIArea>> cacheAreas,
            //ICache<List<t_hr_JsonColumn>> cacheJsonColumn;
            ICache<ESTHrItem> esHrItemCache,
            ICache<UserInspect> cacheUserInspect) : base(userService)
        {
            this.cacheInst = cacheInst;
            this.cacheItem = cacheItem;
            this.cachePath = cachePath;
            this.cacheRole = cacheRole;
            this.cacheUserInArea = cacheUserInArea;
            this.cacheAreas = cacheAreas;
            this.cacheDcit = cacheDcit;
            this.cacheUserInspect = cacheUserInspect;
            this.esHrItemCache = esHrItemCache;
            this.cacheActions = cacheActions;
        }

        // GET: SysCache
        public ActionResult Index()
        {
            return View();
        }

        // GET: SysCache/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SysCache/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysCache/Create
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

        // GET: SysCache/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SysCache/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: SysCache/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;
            if (id == 1)
            {
                cacheInst.Clear();
            }
            else if (id == 2)
            {
                cacheItem.Clear();
            }
            else if (id == 3)
            {
                cachePath.Clear();
            }
            else if (id == 4)
            {
                cacheActions.Clear();
            }
            else if (id == 5)
            {
                cacheRole.Clear();
            }
            else if (id == 6)
            {
                cacheUserInArea.Clear();
            }
            else if (id == 7)
            {
                cacheDcit.Clear();
            }
            else if (id == 8)
            {
                cacheAreas.Clear();
            }
            //else if (id == 9)
            //{
            //    cacheJsonColumn.Clear();
            //}
            else if (id == 9)
            {
                cacheUserInspect.Clear();
            }
            else if (id == 10)
            {
                esHrItemCache.Clear();
            }


            return Content(result.ToJson());
        }

        public ActionResult Search()
        {
            DhtmlxGrid grid = new DhtmlxGrid();
            Dictionary<int, string> dicts = new Dictionary<int, string>();
            dicts.Add(1, "机构名称缓存");
            dicts.Add(2, "检测项目名称缓存");
            dicts.Add(3, "模块缓存");
            dicts.Add(4, "按钮缓存");
            dicts.Add(5, "角色缓存");
            dicts.Add(6, "用户地区缓存");
            dicts.Add(7, "字典缓存");
            dicts.Add(8, "地区缓存");
            //dicts.Add(9, "修改列缓存");
            dicts.Add(9, "监督编号缓存");
            dicts.Add(10, "二维码缓存");

            foreach (var item in dicts)
            {
                DhtmlxGridRow row = new DhtmlxGridRow(item.Key.ToString());

                row.AddCell(item.Key.ToString());
                row.AddCell(item.Value);
                row.AddLinkJsCell("清除缓存", " ClearCache({0})".Fmt(item.Key));

                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }
    }
}
