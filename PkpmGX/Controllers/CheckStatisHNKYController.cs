using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Dhtmlx.Model.Toolbar;
using PkpmGX.Models;
using Nest;
using Pkpm.Entity.ElasticSearch;
using ServiceStack;
using Pkpm.Framework.Repsitory;
using Pkpm.Entity;
using Pkpm.Core.CheckUnitCore;
using Dhtmlx.Model.Grid;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class CheckStatisHNKYController : PkpmController
    {
        IRepsitory<t_bp_custom> customRep;
        IESRepsitory<HNKY_CUR> Rep;
        ICheckUnitService checkUnitService;
        public CheckStatisHNKYController(IUserService userService, 
            IRepsitory<t_bp_custom> customRep,
            IESRepsitory<HNKY_CUR> Rep,
             ICheckUnitService checkUnitService
            ) : base(userService)
        {
            this.customRep = customRep;
            this.checkUnitService = checkUnitService;
            this.Rep = Rep;
        }

        // GET: CheckStatisHNKY
        public ActionResult Index()
        {
            return View();
        }

        // GET: CheckStatisHNKY/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CheckStatisHNKY/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult ToolBar()
        {
            var buttons = GetCurrentUserPathActions();
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Export", "导出") { Img = "fa fa-clone", Imgdis = "fa fa-clone" });
            toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("1"));
            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult Search(CheckStatisHNKYSearchModel model)
        {
            var response = GetResult(model);
            var totalCount = response.Total;
            int pos = model.posStart ?? 0;
            DhtmlxGrid grid = new DhtmlxGrid();

            grid.AddPaging((int)totalCount, pos);

            var allCustoms = checkUnitService.GetAllCheckUnit();
            var index = 1;

            foreach (var item in response.Documents)
            {
                var customName = string.Empty;
                allCustoms.TryGetValue(item.CUSTOMID, out customName);
                DhtmlxGridRow row = new DhtmlxGridRow(item.SYSPRIMARYKEY);
                row.AddCell(index++);
                row.AddCell(item.PROJECTNAME);
                row.AddCell(item.STRUCTPART);
                row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allCustoms,item.CUSTOMID));
                row.AddCell(GetUIDtString(item.CHECKDATE, "yyyy-MM-dd"));
                row.AddLinkJsCell(item.REPORTNUM, "detailsReport(\"{0}\")".Fmt(item.SYSPRIMARYKEY));
                row.AddCell(item.LINQI);
                row.AddCell(item.SHEJIDENGJI);
                row.AddCell(item.QIANGDUDAIBIAOZHI);
                if (model.qiangduwuxiao.HasValue && model.qiangduwuxiao == 1)
                {
                    row.AddCell(string.Empty);
                }
                else
                {
                    row.AddCell(item.BAIFENGBI.ToString());
                }
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        private ISearchResponse<HNKY_CUR> GetResult(CheckStatisHNKYSearchModel model)
        {
            var filterQuery = FilterQuery(model);
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            return Rep.Search(t => t.Query(filterQuery).From(pos).Size(count).Index("gx-t-bp-hnky").Source(sf => sf.Includes(ss => ss.Fields(
                    tt => tt.SYSPRIMARYKEY,
                    tt => tt.PROJECTNAME,
                    tt => tt.STRUCTPART,
                    tt => tt.CUSTOMID,
                    tt => tt.REPORTNUM,
                    tt => tt.SHEJIDENGJI,
                    tt => tt.BAIFENGBI,
                    tt => tt.LINQI,
                    tt => tt.CHECKDATE,
                    tt => tt.QIANGDUDAIBIAOZHI))));
        }

        private Func<QueryContainerDescriptor<HNKY_CUR>, QueryContainer> FilterQuery(CheckStatisHNKYSearchModel model)
        {
            List<string> customIds = new List<string>();
            if (!model.Area.IsNullOrEmpty())
            {
                var areas = model.Area.Split(',').ToList();
                foreach (var item in areas)
                {
                    var customId = customRep.GetByConditon<string>(t => t.area == item, t => new { t.ID });
                    for(int i = 0;i<customId.Count;i++)
                    {
                        customIds.Add(customId[i]);
                    }
                }
                //customIds = customRep.GetByConditon<string>(t => t.area == model.Area, t => new { t.ID });// = customRep.GetByConditonPage<string>(t => t.area == model.Area, r => new { r.Id }, null, null);
            }
            Func<QueryContainerDescriptor<HNKY_CUR>, QueryContainer> filterQuery = q =>
            {
                QueryContainer initQuery = q.Exists(qe => qe.Field(qef => qef.SYSPRIMARYKEY));

                string dtFormatStr = "yyyy-MM-dd'T'HH:mm:ss";
                string startDtStr = model.StartDt.HasValue ? model.StartDt.Value.ToString(dtFormatStr) : string.Empty;
                string endDtStr = model.EndDt.HasValue ? model.EndDt.Value.AddDays(1).ToString(dtFormatStr) : string.Empty;

                if (!startDtStr.IsNullOrEmpty())
                {
                    initQuery = initQuery && +q.DateRange(d => d.Field(f => f.CHECKDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                }
                if (!endDtStr.IsNullOrEmpty())
                {
                    initQuery = initQuery && +q.DateRange(d => d.Field(f => f.CHECKDATE).LessThan(DateMath.FromString(endDtStr)));
                }
                if (!model.CUSTOMID.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Term(qt => qt.Field(qtf => qtf.CUSTOMID).Value(model.CUSTOMID));
                }
                if (!model.YANGHUTIAOJIAN.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Term(qt => qt.Field(qtf => qtf.YANGHUTIAOJIAN).Value(model.YANGHUTIAOJIAN));
                }
                if (!customIds.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Terms(qt => qt.Field(qtf => qtf.CUSTOMID).Terms(customIds));
                }
                if (!model.SHEJIDENGJI.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Term(qt => qt.Field(qtf => qtf.SHEJIDENGJI).Value(model.SHEJIDENGJI));
                }
                if (!model.ProjectName.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.QueryString(m => m.DefaultField(f => f.PROJECTNAME).Query("{0}{1}{0}".Fmt("*", model.ProjectName)));
                }
                if (model.qiangduwuxiao.HasValue && model.qiangduwuxiao == 1)
                {
                    initQuery = initQuery && q.Term(t => t.Field(tt => tt.QIANGDUDAIBIAOZHI).Value("无效"));
                }
                if (!model.BAIFENBIStart.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Range(t => t.Field(tt => tt.BAIFENGBI).GreaterThanOrEquals(double.Parse(model.BAIFENBIStart)));
                }
                if (!model.BAIFENBIEnd.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Range(t => t.Field(tt => tt.BAIFENGBI).LessThanOrEquals(double.Parse(model.BAIFENBIEnd)));
                }
                if (!model.LINQIStart.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Range(t => t.Field(tt => tt.LINQI).GreaterThanOrEquals(double.Parse(model.LINQIStart)));
                }
                if (!model.LINQIEnd.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Range(t => t.Field(tt => tt.LINQI).LessThanOrEquals(double.Parse(model.LINQIEnd)));
                }
                return initQuery;
            };
            return filterQuery;

        }

        // POST: CheckStatisHNKY/Create
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

        // GET: CheckStatisHNKY/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckStatisHNKY/Edit/5
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

        // GET: CheckStatisHNKY/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckStatisHNKY/Delete/5
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
