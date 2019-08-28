using Pkpm.Core.UserRoleCore;
using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using PkpmGX.Architecture;
using PkpmGX.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class AdminController : PkpmController
    {

        IRepsitory<Path> pathRep;

        public AdminController(IRepsitory<Path> pathRep, IUserService userService) : base(userService)
        {
            this.pathRep = pathRep; 
        }

        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.UserName = userService.GetUserDisplayName(GetCurrentUserId());
            ViewBag.UserId = GetCurrentUserId();
            ViewBag.UserRole = userService.GetUserRole(ViewBag.UserId);

            return View();
        }

        public ActionResult TopSider()
        {
            ViewBag.UserName = userService.GetUserDisplayName(GetCurrentUserId());
            ViewBag.UserId = GetCurrentUserId();
            ViewBag.UserRole = userService.GetUserRole(ViewBag.UserId);
            return PartialView();
        }

        public ActionResult SiderBar()
        {
            SiderBarModel model = new SiderBarModel();
            model.SiderBarGroups = new List<SideBarGroup>();

            var paths = GetCurentUserPaths();

            var categoryMenus = paths.Where(p => p.IsCategory && p.Status == 1).OrderBy(p => p.OrderNo).ToList();

            foreach (var categoryMenu in categoryMenus)
            {
                SideBarGroup group = new SideBarGroup()
                {
                    Name = categoryMenu.Name,
                    Icon = categoryMenu.Icon.IsNullOrEmpty() || categoryMenu.Icon.EndsWith(".png") ? "&#xe626;" : categoryMenu.Icon
                };
                group.SiderBarItems = new List<SiderBarItem>();

                foreach (var menu in paths.Where(p => !p.IsCategory && p.CategoryId == categoryMenu.Id && p.Status == 1).OrderBy(p => p.OrderNo))
                {
                    SiderBarItem barItem = new SiderBarItem()
                    {
                        Name = menu.Name,
                        Url = menu.Url
                    };

                    group.SiderBarItems.Add(barItem);
                }

                model.SiderBarGroups.Add(group);
            }

            //string str= model.ToJson();
            //return Content(str);

            return PartialView(model);
        }
    }
}