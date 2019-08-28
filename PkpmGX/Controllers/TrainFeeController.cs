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
    public class TrainFeeController : PkpmController
    {
        public TrainFeeController(IUserService userService) : base(userService)
        {
        }

        // GET: TrainFee
        public ActionResult Index()
        {
            return View();
        }

        // GET: TrainFee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TrainFee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TrainFee/Create
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

        // GET: TrainFee/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TrainFee/Edit/5
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

        // GET: TrainFee/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TrainFee/Delete/5
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
