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
    public class TrainUserController : PkpmController
    {
        public TrainUserController(IUserService userService) : base(userService)
        {
        }

        // GET: TrainUser
        public ActionResult Index()
        {
            return View();
        }

        // GET: TrainUser/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TrainUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainUser/Create
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

        // GET: TrainUser/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TrainUser/Edit/5
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

        // GET: TrainUser/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TrainUser/Delete/5
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
