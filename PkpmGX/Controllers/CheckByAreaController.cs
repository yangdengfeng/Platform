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
    public class CheckByAreaController : PkpmController
    {
        public CheckByAreaController(IUserService userService) : base(userService)
        {
        }

        // GET: CheckByArea
        public ActionResult Index()
        {
            return View();
        }

        // GET: CheckByArea/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CheckByArea/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckByArea/Create
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

        // GET: CheckByArea/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckByArea/Edit/5
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

        // GET: CheckByArea/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckByArea/Delete/5
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
