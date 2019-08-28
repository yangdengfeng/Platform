using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Framework.Repsitory;
using Pkpm.Entity.ElasticSearch;
using PkpmGX.Models;
using Nest;
using ServiceStack;
using Dhtmlx.Model.Grid;
using Pkpm.Core.CheckUnitCore;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class CheckStatisSJKYController : PkpmController
    {
        IESRepsitory<SJKY_CUR> sjkyESRep;
        ICheckUnitService checkUnitService;
        public CheckStatisSJKYController(IESRepsitory<SJKY_CUR> sjkyESRep, ICheckUnitService checkUnitService, IUserService userService) : base(userService)
        {
            this.sjkyESRep = sjkyESRep;
            this.checkUnitService = checkUnitService;
        }

        // GET: CheckStatisSJKY
        public ActionResult Index()
        {
            return View();
        }

        // GET: CheckStatisSJKY/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CheckStatisSJKY/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckStatisSJKY/Create
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

        // GET: CheckStatisSJKY/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckStatisSJKY/Edit/5
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

        // GET: CheckStatisSJKY/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckStatisSJKY/Delete/5
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


        public ActionResult Search(CheckStatisSJKYSearchModel model)
        {
            var response = GetResponse(model);
            var totalCount = response.Total;

            int pos = model.posStart ?? 0;

            DhtmlxGrid grid = new DhtmlxGrid();

            grid.AddPaging((int)totalCount, pos);

            var allCustoms = checkUnitService.GetAllCheckUnit();
            var index = 1;

            foreach (var item in response.Documents)
            {
                DhtmlxGridRow row = new DhtmlxGridRow(item.SYSPRIMARYKEY);
                row.AddCell(index++);
                row.AddCell(item.PROJECTNAME);
                row.AddCell(item.STRUCTPART);
                row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allCustoms, item.CUSTOMID));
                row.AddLinkJsCell(item.REPORTNUM, "detailsReport(\"{0}\")".Fmt(item.SYSPRIMARYKEY));
                row.AddCell(item.SHEJIQIANGDUDENGJI);
                row.AddCell(item.QIANGDUDAIBIAOZHI);
                if (model.qiangduwuxiao.HasValue && model.qiangduwuxiao == 1)
                {
                    row.AddCell(string.Empty);
                }
                else
                {
                    row.AddCell(item.QIANGDUBAIFENBI.ToString());
                }
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");

        }

        private ISearchResponse<SJKY_CUR> GetResponse(CheckStatisSJKYSearchModel model)
        {
            var filter = GetFilterQuery(model);
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            return sjkyESRep.Search(t => t.Query(filter).From(pos).Size(count).Index("gx-t-bp-sjky").Source(sf => sf.Includes(ss => ss.Fields(tt => tt.SYSPRIMARYKEY,
                tt => tt.PROJECTNAME,
                ttt => ttt.STRUCTPART,
                ttt => ttt.CUSTOMID,
                ttt => ttt.REPORTNUM,
                ttt => ttt.SHEJIQIANGDUDENGJI,
                ttt => ttt.QIANGDUDAIBIAOZHI,
                ttt => ttt.QIANGDUBAIFENBI))));

        }

        protected Func<QueryContainerDescriptor<SJKY_CUR>, QueryContainer> GetFilterQuery(CheckStatisSJKYSearchModel model)
        {
            Func<QueryContainerDescriptor<SJKY_CUR>, QueryContainer> filterQuery = q =>
            {
                string dtFormatStr = "yyyy-MM-dd'T'HH:mm:ss";

                QueryContainer initQuery = q.Exists(qe => qe.Field(qef => qef.SYSPRIMARYKEY));

                string startDtStr = model.ReportStartDate.HasValue ? model.ReportStartDate.Value.ToString(dtFormatStr) : string.Empty;
                string endDtStr = model.ReportEndDate.HasValue ? model.ReportEndDate.Value.AddDays(1).ToString(dtFormatStr) : string.Empty;
                if (!startDtStr.IsNullOrEmpty())
                {
                    initQuery = initQuery && +q.DateRange(d => d.Field(f => f.CHECKDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                }
                if (!endDtStr.IsNullOrEmpty())
                {
                    initQuery = initQuery && +q.DateRange(d => d.Field(f => f.CHECKDATE).LessThan(DateMath.FromString(endDtStr)));
                }

                if (model.qiangduwuxiao.HasValue && model.qiangduwuxiao == 1)
                {
                    initQuery = initQuery && q.Term(t => t.Field(tt => tt.QIANGDUDAIBIAOZHI).Value("无效"));
                }

                if (model.qiangduhigh.HasValue)
                {
                    initQuery = initQuery && q.Range(t => t.Field(tt => tt.QIANGDUBAIFENBI).LessThanOrEquals(model.qiangduhigh));
                }

                if (model.qiangdulow.HasValue)
                {
                    initQuery = initQuery && q.Range(t => t.Field(tt => tt.QIANGDUBAIFENBI).GreaterThanOrEquals(model.qiangdulow));
                }

                if (!model.Area.IsNullOrEmpty())
                {
                    var areas = model.Area.Split(',').ToList();

                    Dictionary<string, string> CheckUnits = checkUnitService.GetUnitByArea(areas);
                    if (CheckUnits.Count > 0)
                    {
                        initQuery = initQuery && +q.Terms(t => t.Field(f => f.CUSTOMID).Terms(CheckUnits.Keys.ToList()));
                    }
                    else
                    {
                        initQuery = initQuery && +q.Terms(t => t.Field(f => f.CUSTOMID).Terms("asdfghj"));
                    }
                }

                if (!model.projectName.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.QueryString(m => m.DefaultField(f => f.PROJECTNAME).Query("{0}{1}{0}".Fmt("*", model.projectName)));
                }

                if (!model.qiangdudengji.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.SHEJIQIANGDUDENGJI).Value(model.qiangdudengji));
                }

                if (!model.yanghutiaojian.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Term(t => t.Field(tt => tt.YANGHUTIAOJIAN).Value(model.yanghutiaojian));
                }

                if (!model.CustomId.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Term(t => t.Field(tt => tt.CUSTOMID).Value(model.CustomId));
                }

                return initQuery;
            };

            return filterQuery;
        }




    }
}
