using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using PkpmGX.Models;
using Pkpm.Framework.Repsitory;
using Pkpm.Entity.ElasticSearch;
using Nest;
using ServiceStack;
using Dhtmlx.Model.Grid;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.ItemNameCore;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class CheckStatisDataController : PkpmController
    {
        IESRepsitory<es_t_bp_item> tbpItemESRep;
        ICheckUnitService checkUnitService;
        IItemNameService itemNameService;
        public CheckStatisDataController(IESRepsitory<es_t_bp_item> tbpItemESRep, ICheckUnitService checkUnitService, IItemNameService itemNameService, IUserService userService) : base(userService)
        {
            this.tbpItemESRep = tbpItemESRep;
            this.checkUnitService = checkUnitService;
            this.itemNameService = itemNameService;
        }

        // GET: CheckStatisData
        public ActionResult Index()
        {
            return View();
        }

        // GET: CheckStatisData/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CheckStatisData/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckStatisData/Create
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

        // GET: CheckStatisData/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CheckStatisData/Edit/5
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

        // GET: CheckStatisData/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckStatisData/Delete/5
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
        protected Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> GetFilterQuery(CheckStatisDataSearchModel model)
        {
            Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> filterQuery = q =>
            {
                string dtFormatStr = "yyyy-MM-dd'T'HH:mm:ss";
                var startDtStr = model.StartTime.HasValue ? model.StartTime.Value.ToString(dtFormatStr) : string.Empty;
                var endDtStr = model.EndTime.HasValue ? model.EndTime.Value.ToString(dtFormatStr) : string.Empty;

                QueryContainer initQuery = q.Exists(qe => qe.Field(qef => qef.SYSPRIMARYKEY));

                if (model.Type == "0")
                {
                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.PRINTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                    }

                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.PRINTDATE).LessThanOrEquals(DateMath.FromString(endDtStr)));
                    }
                }
                else if (model.Type == "1")
                {
                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.APPROVEDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                    }

                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.APPROVEDATE).LessThanOrEquals(DateMath.FromString(endDtStr)));
                    }
                }
                else if (model.Type == "2")
                {
                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.CHECKDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                    }

                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.CHECKDATE).LessThanOrEquals(DateMath.FromString(endDtStr)));
                    }
                }
                else if (model.Type == "3")
                {
                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.UPLOADTIME).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                    }

                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.UPLOADTIME).LessThanOrEquals(DateMath.FromString(endDtStr)));
                    }
                }


                return initQuery;
            };

            return filterQuery;

        }

        protected Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> GetGridSearchFilterQuery(CheckStatisDataGridSearchModel model)
        {
            Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> filterQuery = q =>
            {
                string dtFormatStr = "yyyy-MM-dd'T'HH:mm:ss";
                DateTime? StartTime = new DateTime();
                DateTime? EndTime = new DateTime();
                DateTime date = DateTime.Now;
                if (model.SearchType == "0")
                {
                    StartTime = DateTime.Today;
                    EndTime = DateTime.Today.AddDays(1);
                }
                else if (model.SearchType == "1")
                {
                    StartTime = date.AddDays(1 - Convert.ToInt32(date.DayOfWeek.ToString("d"))); //本周周一
                    EndTime = StartTime.Value.AddDays(7);
                }
                else if (model.SearchType == "2")
                {
                    StartTime = date.AddDays(1 - date.Day);  //本月月初  
                    EndTime = StartTime.Value.AddMonths(1);  //本月月末  
                }
                else if (model.SearchType == "3")
                {
                    StartTime = date.AddMonths(0 - (date.Month - 1) % 3).AddDays(1 - date.Day);  //本季度初  
                    EndTime = StartTime.Value.AddMonths(3);
                }
                else if (model.SearchType == "4")
                {
                    StartTime = new DateTime(date.Year, 1, 1);  //本年年初  
                    EndTime = new DateTime(date.Year, 12, 31).AddDays(1);  //本年年末  
                }
                else if (model.SearchType == "5")
                {
                    StartTime = new DateTime(2000, 1, 1);
                    EndTime = new DateTime(2099, 1, 1);
                }


                var startDtStr = StartTime.HasValue ? StartTime.Value.ToString(dtFormatStr) : string.Empty;
                var endDtStr = EndTime.HasValue ? EndTime.Value.ToString(dtFormatStr) : string.Empty;

                QueryContainer initQuery = q.Exists(qe => qe.Field(qef => qef.SYSPRIMARYKEY));

                if (model.Type == "0")
                {
                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.PRINTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                    }

                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.PRINTDATE).LessThanOrEquals(DateMath.FromString(endDtStr)));
                    }
                }
                else if (model.Type == "1")
                {
                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.APPROVEDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                    }

                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.APPROVEDATE).LessThanOrEquals(DateMath.FromString(endDtStr)));
                    }
                }
                else if (model.Type == "2")
                {
                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.CHECKDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                    }

                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.CHECKDATE).LessThanOrEquals(DateMath.FromString(endDtStr)));
                    }
                }
                else if (model.Type == "3")
                {
                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.UPLOADTIME).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                    }

                    if (!startDtStr.IsNullOrEmpty())
                    {
                        initQuery = initQuery && +q.DateRange(d => d.Field(f => f.UPLOADTIME).LessThanOrEquals(DateMath.FromString(endDtStr)));
                    }
                }

                if (!model.CustomId.IsNullOrEmpty())
                {
                    initQuery = initQuery && q.Term(tt => tt.Field(ttt => ttt.CUSTOMID).Value(model.CustomId));
                }

                return initQuery;
            };

            return filterQuery;

        }


        public ActionResult Search(CheckStatisDataSearchModel model)
        {
            List<CheckStatisDataSearchResultModel> resultmodel = GetSearchData(model);
            DhtmlxGrid grid = new DhtmlxGrid();

            var allInsts = checkUnitService.GetAllCheckUnit();

            int index = 1;
            string customName = string.Empty;


            foreach (var item in resultmodel)
            {
                if(item.CustomId.IsNullOrEmpty())
                {
                    continue;
                }
                DhtmlxGridRow row = new DhtmlxGridRow(item.CustomId);
                row.AddCell(index++);
                row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allInsts,item.CustomId));
                if (item.DayCount > 0)
                {
                    row.AddLinkJsCell(item.DayCount, "Details(\"{0}\",\"{1}\")".Fmt(item.CustomId, 0));
                }
                else
                {
                    row.AddCell(item.DayCount);
                }
                if (item.WeekCount > 0)
                {
                    row.AddLinkJsCell(item.WeekCount, "Details(\"{0}\",\"{1}\")".Fmt(item.CustomId, 1));
                }
                else
                {
                    row.AddCell(item.WeekCount);
                }

                if (item.MonthCount > 0)
                {
                    row.AddLinkJsCell(item.MonthCount, "Details(\"{0}\",\"{1}\")".Fmt(item.CustomId, 2));
                }
                else
                {
                    row.AddCell(item.MonthCount);
                }

                if (item.QuarterCount > 0)
                {
                    row.AddLinkJsCell(item.QuarterCount, "Details(\"{0}\",\"{1}\")".Fmt(item.CustomId, 3));
                }
                else
                {
                    row.AddCell(item.QuarterCount);
                }

                if (item.YearCount > 0)
                {
                    row.AddLinkJsCell(item.YearCount, "Details(\"{0}\",\"{1}\")".Fmt(item.CustomId, 4));
                }
                else
                {
                    row.AddCell(item.YearCount);
                }

                if (item.TotalCount > 0)
                {
                    row.AddLinkJsCell(item.TotalCount, "Details(\"{0}\",\"{1}\")".Fmt(item.CustomId, 5));
                }
                else
                {
                    row.AddCell(item.TotalCount);
                }


                //row.AddCell(item.YearCount);
                //row.AddCell(item.TotalCount);

                grid.AddGridRow(row);

            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");

        }

        private List<CheckStatisDataSearchResultModel> GetSearchData(CheckStatisDataSearchModel model)
        {
            var date = DateTime.Now;
            DateTime startYear = new DateTime(date.Year, 1, 1);  //本年年初  
            DateTime endYear = new DateTime(date.Year, 12, 31).AddDays(1);  //本年年末  
            DateTime startWeek = date.AddDays(1 - Convert.ToInt32(date.DayOfWeek.ToString("d"))); //本周周一
            DateTime endWeek = startWeek.AddDays(7);

            string dtFormatStr = "yyyy-MM-dd'T'HH:mm:ss";


            model.StartTime = startYear;
            model.EndTime = endYear;

            //获取当前年份，季度，月数据信息
            ISearchResponse<es_t_bp_item> yearResponse = GetYearSearchResponse(model);

            model.StartTime = startWeek;
            model.EndTime = endWeek;

            //获取当前周信息
            ISearchResponse<es_t_bp_item> weekResponse = GetWeekSearchResponse(model);

            //获取所有机构上传报告总数
            ISearchResponse<es_t_bp_item> totalResponse = GetTotalSearchResponse(model);



            DateTime startQuarter = date.AddMonths(0 - (date.Month - 1) % 3).AddDays(1 - date.Day);  //本季度初  
            var startQuaStr = startQuarter.ToString("yyyy-MM-dd");

            var startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
            var startMonthStr = startMonth.ToString("yyyy-MM-dd");
            var toDayStr = DateTime.Today.ToString("yyyy-MM-dd");


            List<CheckStatisDataSearchResultModel> resultmodel = new List<CheckStatisDataSearchResultModel>();

            if (yearResponse.IsValid)
            {
                var buckets = yearResponse.Aggs.Terms("CustomID").Buckets;
                foreach (var bucket in buckets)
                {
                    var customId = bucket.Key;
                    int yearCount = 0;
                    int qualCount = 0;
                    int monthCount = 0;

                    var yearBuckets = bucket.DateHistogram("year").Buckets;

                    foreach (var yearBucket in yearBuckets)
                    {
                        yearCount = (int)yearBucket.DocCount;
                        var quaBuckets = yearBucket.DateHistogram("qua").Buckets;
                        foreach (var quaBucket in quaBuckets)
                        {
                            var quaKeyAsStrings = quaBucket.KeyAsString.Split('T');

                            if (startQuaStr == quaKeyAsStrings[0])//只需要本季度数据，同时在本季度存在数据的情况下才获取本月数据
                            {
                                qualCount = (int)quaBucket.DocCount;
                                var monthBuckets = quaBucket.DateHistogram("month").Buckets;
                                foreach (var monthBucket in monthBuckets)
                                {
                                    var monthKeyAsStrings = monthBucket.KeyAsString.Split('T');
                                    if (startMonthStr == monthKeyAsStrings[0])//获取本月数据
                                    {
                                        monthCount = (int)monthBucket.DocCount;
                                    }
                                }
                            }

                        }
                    }
                    resultmodel.Add(new CheckStatisDataSearchResultModel()
                    {
                        CustomId = customId,
                        YearCount = yearCount,
                        QuarterCount = qualCount,
                        MonthCount = monthCount
                    });
                }
            }

            if (weekResponse.IsValid)
            {
                var buckets = weekResponse.Aggs.Terms("CustomID").Buckets;

                foreach (var bucket in buckets)
                {
                    var customId = bucket.Key;
                    var weekBuckets = bucket.DateHistogram("week").Buckets;
                    foreach (var weekBucket in weekBuckets)
                    {
                        var weekCount = weekBucket.DocCount;

                        var dayBuckets = weekBucket.DateHistogram("day").Buckets;
                        foreach (var dayBucket in dayBuckets)
                        {
                            var dayKeys = dayBucket.KeyAsString.Split('T');
                            if (toDayStr == dayKeys[0])//只需要当天数据
                            {
                                var dayCount = dayBucket.DocCount;

                                var oneResultModels = resultmodel.Where(t => t.CustomId == customId);
                                if (oneResultModels != null)
                                {
                                    if (oneResultModels.Count() > 0) //和上面的不等于null判断一起判断结果数组中是不是存在本机构，如果存在，直接修改相应值，不存在则新增
                                    {
                                        var oneResultModel = oneResultModels.First();
                                        oneResultModel.DayCount = (int)dayCount;
                                        oneResultModel.WeekCount = (int)weekCount;
                                    }
                                    else
                                    {
                                        resultmodel.Add(new CheckStatisDataSearchResultModel()
                                        {
                                            DayCount = (int)dayCount,
                                            WeekCount = (int)weekCount
                                        });
                                    }
                                }
                                else
                                {
                                    resultmodel.Add(new CheckStatisDataSearchResultModel()
                                    {
                                        DayCount = (int)dayCount,
                                        WeekCount = (int)weekCount
                                    });
                                }

                            }
                        }
                    }
                }
            }

            if (totalResponse.IsValid)
            {
                var buckets = totalResponse.Aggs.Terms("CustomID").Buckets;
                foreach (var bucket in buckets)
                {
                    var customId = bucket.Key;
                    var totalCount = bucket.DocCount;

                    var oneResultModels = resultmodel.Where(t => t.CustomId == customId);
                    if (oneResultModels != null)
                    {
                        if (oneResultModels.Count() > 0)//和上面的不等于null判断一起判断结果数组中是不是存在本机构，如果存在，直接修改相应值，不存在则新增
                        {
                            var oneResultModel = oneResultModels.First();
                            oneResultModel.TotalCount = (int)totalCount;
                        }
                        else
                        {
                            resultmodel.Add(new CheckStatisDataSearchResultModel()
                            {
                                CustomId = customId,
                                TotalCount = (int)totalCount
                            });
                        }
                    }
                    else
                    {
                        resultmodel.Add(new CheckStatisDataSearchResultModel()
                        {
                            CustomId = customId,
                            TotalCount = (int)totalCount
                        });
                    }
                }
            }

            return resultmodel;
        }

        private ISearchResponse<es_t_bp_item> GetYearSearchResponse(CheckStatisDataSearchModel model)
        {
            var filterQuery = GetFilterQuery(model);
            ISearchResponse<es_t_bp_item> response;
            if (model.Type == "0")
            {

                response = tbpItemESRep.Search(t => t.Size(0).Query(filterQuery)
                .Aggregations(ttty => ttty.Terms("CustomID", tttty => tttty.Field(tgh => tgh.CUSTOMID)
                    .Aggregations(tt => tt.DateHistogram("year", tst => tst.Field(tss => tss.PRINTDATE).Interval(DateInterval.Year).MinimumDocumentCount(1)
                    .Aggregations(tsst => tsst.DateHistogram("qua", ttt => ttt.Field(tstt => tstt.PRINTDATE).Interval(DateInterval.Quarter).MinimumDocumentCount(0)
                    .Aggregations(ty => ty.DateHistogram("month", tty => tty.Field(ttyy => ttyy.PRINTDATE).Interval(DateInterval.Month).MinimumDocumentCount(0))))))))));
            }
            else if (model.Type == "1")
            {
                response = tbpItemESRep.Search(t => t.Size(0).Query(filterQuery)
                .Aggregations(ttty => ttty.Terms("CustomID", tttty => tttty.Field(tgh => tgh.CUSTOMID)

                   .Aggregations(tt => tt.DateHistogram("year", tst => tst.Field(tss => tss.APPROVEDATE).Interval(DateInterval.Year).MinimumDocumentCount(1)
                   .Aggregations(tsst => tsst.DateHistogram("qua", ttt => ttt.Field(tstt => tstt.APPROVEDATE).Interval(DateInterval.Quarter).MinimumDocumentCount(0)
                   .Aggregations(ty => ty.DateHistogram("month", tty => tty.Field(ttyy => ttyy.APPROVEDATE).Interval(DateInterval.Month).MinimumDocumentCount(0))))))))));
            }
            else if (model.Type == "2")
            {
                response = tbpItemESRep.Search(t => t.Size(0).Query(filterQuery)
                .Aggregations(ttty => ttty.Terms("CustomID", tttty => tttty.Field(tgh => tgh.CUSTOMID)

                   .Aggregations(tt => tt.DateHistogram("year", tst => tst.Field(tss => tss.CHECKDATE).Interval(DateInterval.Year).MinimumDocumentCount(1)
                   .Aggregations(tsst => tsst.DateHistogram("qua", ttt => ttt.Field(tstt => tstt.CHECKDATE).Interval(DateInterval.Quarter).MinimumDocumentCount(0)
                   .Aggregations(ty => ty.DateHistogram("month", tty => tty.Field(ttyy => ttyy.CHECKDATE).Interval(DateInterval.Month).MinimumDocumentCount(0))))))))));
            }
            else if (model.Type == "3")
            {
                response = tbpItemESRep.Search(t => t.Size(0).Query(filterQuery)
                .Aggregations(ttty => ttty.Terms("CustomID", tttty => tttty.Field(tgh => tgh.CUSTOMID)

                   .Aggregations(tt => tt.DateHistogram("year", tst => tst.Field(tss => tss.UPLOADTIME).Interval(DateInterval.Year).MinimumDocumentCount(1)
                   .Aggregations(tsst => tsst.DateHistogram("qua", ttt => ttt.Field(tstt => tstt.UPLOADTIME).Interval(DateInterval.Quarter).MinimumDocumentCount(0)
                   .Aggregations(ty => ty.DateHistogram("month", tty => tty.Field(ttyy => ttyy.UPLOADTIME).Interval(DateInterval.Month).MinimumDocumentCount(0))))))))));
            }
            else
            {
                response = null;
            }
            return response;

        }

        private ISearchResponse<es_t_bp_item> GetWeekSearchResponse(CheckStatisDataSearchModel model)
        {
            var filterQuery = GetFilterQuery(model);
            ISearchResponse<es_t_bp_item> response;
            if (model.Type == "0")
            {

                response = tbpItemESRep.Search(t => t.Size(0).Query(filterQuery)
                .Aggregations(ttty => ttty.Terms("CustomID", tttty => tttty.Field(tgh => tgh.CUSTOMID)
                    .Aggregations(tt => tt.DateHistogram("week", tst => tst.Field(tss => tss.PRINTDATE).Interval(DateInterval.Week).MinimumDocumentCount(1)
                    .Aggregations(tsst => tsst.DateHistogram("day", ttt => ttt.Field(tstt => tstt.PRINTDATE).Interval(DateInterval.Day).MinimumDocumentCount(0))))))));
            }
            else if (model.Type == "1")
            {
                response = tbpItemESRep.Search(t => t.Size(0).Query(filterQuery)
                .Aggregations(ttty => ttty.Terms("CustomID", tttty => tttty.Field(tgh => tgh.CUSTOMID)

                   .Aggregations(tt => tt.DateHistogram("week", tst => tst.Field(tss => tss.APPROVEDATE).Interval(DateInterval.Week).MinimumDocumentCount(1)
                   .Aggregations(ty => ty.DateHistogram("day", tty => tty.Field(ttyy => ttyy.APPROVEDATE).Interval(DateInterval.Day).MinimumDocumentCount(0))))))));
            }
            else if (model.Type == "2")
            {
                response = tbpItemESRep.Search(t => t.Size(0).Query(filterQuery)
                .Aggregations(ttty => ttty.Terms("CustomID", tttty => tttty.Field(tgh => tgh.CUSTOMID)
                   .Aggregations(tt => tt.DateHistogram("week", tst => tst.Field(tss => tss.CHECKDATE).Interval(DateInterval.Week).MinimumDocumentCount(1)
                   .Aggregations(ty => ty.DateHistogram("day", tty => tty.Field(ttyy => ttyy.CHECKDATE).Interval(DateInterval.Day).MinimumDocumentCount(0))))))));
            }
            else if (model.Type == "3")
            {
                response = tbpItemESRep.Search(t => t.Size(0).Query(filterQuery)
                .Aggregations(ttty => ttty.Terms("CustomID", tttty => tttty.Field(tgh => tgh.CUSTOMID)

                   .Aggregations(tt => tt.DateHistogram("week", tst => tst.Field(tss => tss.UPLOADTIME).Interval(DateInterval.Week).MinimumDocumentCount(1)
                   .Aggregations(ty => ty.DateHistogram("day", tty => tty.Field(ttyy => ttyy.UPLOADTIME).Interval(DateInterval.Day).MinimumDocumentCount(0))))))));
            }
            else
            {
                response = null;
            }
            return response;
        }

        private ISearchResponse<es_t_bp_item> GetTotalSearchResponse(CheckStatisDataSearchModel model)
        {
            ISearchResponse<es_t_bp_item> response;
            if (model.Type == "0")
            {
                response = tbpItemESRep.Search(t => t.Size(0)
                .Aggregations(ttty => ttty.Terms("CustomID", tttty => tttty.Field(tgh => tgh.CUSTOMID).Size(1000))));
            }
            else if (model.Type == "1")
            {
                response = tbpItemESRep.Search(t => t.Size(0)
                .Aggregations(ttty => ttty.Terms("CustomID", tttty => tttty.Field(tgh => tgh.CUSTOMID).Size(1000))));

            }
            else if (model.Type == "2")
            {
                response = tbpItemESRep.Search(t => t.Size(0)
                .Aggregations(ttty => ttty.Terms("CustomID", tttty => tttty.Field(tgh => tgh.CUSTOMID).Size(1000))));
            }
            else if (model.Type == "3")
            {
                response = tbpItemESRep.Search(t => t.Size(0)
                .Aggregations(ttty => ttty.Terms("CustomID", tttty => tttty.Field(tgh => tgh.CUSTOMID).Size(1000))));
            }
            else
            {
                response = null;
            }
            return response;
        }

        private ISearchResponse<es_t_bp_item> GetGridSerarchResponse(CheckStatisDataGridSearchModel model)
        {

            var filterQuery = GetGridSearchFilterQuery(model);

            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            return tbpItemESRep.Search(t => t.Query(filterQuery).From(pos).Size(count)
                            .Source(tt => tt.Includes(ttt => ttt.Fields(tst => tst.CUSTOMID,
                                                                        tst => tst.SYSPRIMARYKEY,
                                                                        tst => tst.REPORTNUM,
                                                                        tst => tst.ITEMNAME,
                                                                        tst => tst.PRINTDATE,
                                                                        tst => tst.CHECKDATE,
                                                                        tst => tst.PROJECTNAME))));
        }

        public ActionResult GridSearch(CheckStatisDataGridSearchModel model)
        {
            var response = GetGridSerarchResponse(model);
            DhtmlxGrid grid = new DhtmlxGrid();

            var allInsts = checkUnitService.GetAllCheckUnit();
            var allItems = itemNameService.GetAllItemName();
            var pos = model.posStart.HasValue ? model.posStart.Value : 0; 
            int index = pos + 1; 


            if (response.IsValid)
            {
                var totalCount = (int)response.Total;
                grid.AddPaging(totalCount, pos);
                foreach (var item in response.Documents)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(item.SYSPRIMARYKEY);
                    row.AddCell(index++);
                    row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allInsts,item.CUSTOMID));
                    row.AddCell(item.PROJECTNAME);
                    row.AddCell(itemNameService.GetItemCNNameFromAll(allItems, item.REPORTJXLB, item.ITEMNAME));
                    row.AddCell(GetUIDtString(item.CHECKDATE));
                    row.AddCell(GetUIDtString(item.PRINTDATE));
                    row.AddLinkJsCell(item.REPORTNUM, "getPKRReport(\"{0}\")".Fmt(item.REPORTNUM));
                    row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));

                    grid.AddGridRow(row);
                }
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text /xml");
        }
    }
}
