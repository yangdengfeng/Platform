using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using PkpmGX.Models;
using ServiceStack.OrmLite;
using Dhtmlx.Model.Grid;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class SysLogController : PkpmController
    {
        IRepsitory<SysLog> rep;
        public SysLogController(IUserService userService,IRepsitory<SysLog> rep) : base(userService)
        {
            this.rep = rep;
        }

        // GET: SysLog
        public ActionResult Index()
        {
            return View();
        }

        // GET: SysLog/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SysLog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SysLog/Create
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

        // GET: SysLog/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SysLog/Edit/5
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

        // GET: SysLog/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SysLog/Delete/5
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

        public ActionResult Search(SysLogViewModels searchModel)
        {
            var predicate = PredicateBuilder.True<SysLog>();
            if (!string.IsNullOrWhiteSpace(searchModel.LogUserName))
            {
                predicate = predicate.And(p => p.UerName.Contains(searchModel.LogUserName));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.LogType))
            {
                predicate = predicate.And(p => p.LogEvent.Contains(searchModel.LogType));
            }
            if (searchModel.LogStartDt.HasValue)
            {
                predicate = predicate.And(p => p.LogTime >= searchModel.LogStartDt.Value);
            }
            if (searchModel.LogEndDt.HasValue)
            {

                predicate = predicate.And(p => p.LogTime <= searchModel.LogEndDt.Value.AddDays(1));
            }

            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            int count = searchModel.count.HasValue ? searchModel.count.Value : 30;

            PagingOptions<SysLog> pagingOption = new PagingOptions<SysLog>(pos, count, u => new { u.LogTime }, true);
            var logs = rep.GetByConditonPage(predicate, pagingOption);

            DhtmlxGrid grid = new DhtmlxGrid();
            grid.AddPaging(pagingOption.TotalItems, pos);
            for (int i = 0; i < logs.Count; i++)
            {
                var log = logs[i];
                DhtmlxGridRow row = new DhtmlxGridRow(log.Id.ToString());
                row.AddCell((pos + i + 1).ToString());
                row.AddCell(log.UerName);
                row.AddCell(log.IpAddress);
                row.AddCell(log.LogEvent);
                row.AddCell(log.LogType);
                row.AddCell(log.LogTime.ToString("yyyy-MM-dd HH:mm:ss"));
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");

        }
    }
}
