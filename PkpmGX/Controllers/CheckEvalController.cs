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
    public class CheckEvalController : PkpmController
    {
        public CheckEvalController(IUserService userService) : base(userService)
        {
        }

        // GET: CheckEval
        public ActionResult Index()
        {
            return View();
        }

        // GET: CheckEval/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CheckEval/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckEval/Create
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

        // GET: CheckEval/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckEval/Edit/5
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

        // GET: CheckEval/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckEval/Delete/5
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
