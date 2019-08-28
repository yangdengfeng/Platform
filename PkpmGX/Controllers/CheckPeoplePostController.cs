using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class CheckPeoplePostController : PkpmController
    {
        public CheckPeoplePostController(IUserService userService) : base(userService)
        {
        }

        // GET: CheckPeoplePost
        public ActionResult Index()
        {
            return View();
        }

        // GET: CheckPeoplePost/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CheckPeoplePost/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckPeoplePost/Create
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

        // GET: CheckPeoplePost/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckPeoplePost/Edit/5
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

        // GET: CheckPeoplePost/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckPeoplePost/Delete/5
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
    }
}
