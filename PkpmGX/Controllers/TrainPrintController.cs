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
    public class TrainPrintController : PkpmController
    {
        public TrainPrintController(IUserService userService) : base(userService)
        {
        }

        // GET: TrainPrint
        public ActionResult Index()
        {
            return View();
        }

        // GET: TrainPrint/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TrainPrint/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainPrint/Create
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

        // GET: TrainPrint/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TrainPrint/Edit/5
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

        // GET: TrainPrint/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TrainPrint/Delete/5
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
