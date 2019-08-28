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
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.ItemNameCore;
using Pkpm.Entity;
using ServiceStack;
using System.Linq.Expressions;
using Pkpm.Framework.Repsitory;
using Dhtmlx.Model.Grid;
using Pkpm.Framework.PkpmConfigService;
using Pkpm.Framework.Common;
using Pkpm.Core.CovrliistService;
using Pkpm.Core.ReportCore;
using Pkpm.Core.QrCodeCore;
using System.IO;
using System.Configuration;
using Pkpm.Core.SysDictCore;
using NLog;
using Pkpm.Framework.Logging;
using System.Xml.Linq;
using QZWebService.ServiceModel;
using ServiceStack.OrmLite;
using ServiceStack.Data;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using RestSharp;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class TotalSearchController : PkpmController
    {

        ICovrlistService covrlistService;
        ICheckUnitService checkUnitService;
        IItemNameService itemNameService;
        IESRepsitory<es_t_bp_item> tbpitemESRep;
        IPkpmConfigService PkpmConfigService;
        ISysDictService sysDictService;
        IReportService reportService;
        IReportQrCode reportQr;
        IESRepsitory<es_t_bp_acs> acsESRep;
        IESRepsitory<es_t_bp_wordreport> wordESRep;
        IESRepsitory<es_t_bp_question> questionESRep;
        IESRepsitory<es_extReportMange> wordExtESRep;
        IESRepsitory<es_t_sys_files> sysfilesEsRep;
        IESRepsitory<es_t_pkpm_binaryReport> pkrESRep;
        IESRepsitory<es_t_bp_modify_log> modifylogESRep;
        IRepsitory<t_bp_custom> customRep;
        public TotalSearchController(IUserService userService,
             ICheckUnitService checkUnitService,
             IItemNameService itemNameService,
             IESRepsitory<es_t_bp_item> tbpitemESRep,
             IPkpmConfigService PkpmConfigService,
             IReportService reportService,
             IReportQrCode reportQr,
              ISysDictService sysDictService,
             IESRepsitory<es_t_bp_question> questionESRep,
             IESRepsitory<es_t_bp_acs> acsESRep,
             IESRepsitory<es_t_bp_wordreport> wordESRep,
             IESRepsitory<es_extReportMange> wordExtESRep,
             IESRepsitory<es_t_sys_files> sysfilesEsRep,
             IESRepsitory<es_t_pkpm_binaryReport> pkrESRep,
             IESRepsitory<es_t_bp_modify_log> modifylogESRep,
             IRepsitory<t_bp_custom> customRep,
             ICovrlistService covrlistService) : base(userService)
        {
            this.checkUnitService = checkUnitService;
            this.itemNameService = itemNameService;
            this.tbpitemESRep = tbpitemESRep;
            this.PkpmConfigService = PkpmConfigService;
            this.reportService = reportService;
            this.reportQr = reportQr;
            this.sysDictService = sysDictService;
            this.questionESRep = questionESRep;
            this.acsESRep = acsESRep;
            this.wordESRep = wordESRep;
            this.wordExtESRep = wordExtESRep;
            this.sysfilesEsRep = sysfilesEsRep;
            this.pkrESRep = pkrESRep;
            this.modifylogESRep = modifylogESRep;
            this.covrlistService = covrlistService;
            this.customRep = customRep;
        }

        private static Pkpm.Framework.Logging.ILogger logger = new NLogLoggerFactory().CreateLogger("Pkpm.Web.Controllers.WelcomeController");


        // GET: TotalSearch
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ToolBar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("AdvSearch", "展开高级查询") { Img = "fa fa-chevron-circle-up", Imgdis = "fa fa-chevron-circle-up" });
            toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("4"));
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Export", "导出") { Img = "fa fa-file-excel-o", Imgdis = "fa fa-file-excel-o" });
            toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("5"));
            toolBar.AddToolbarItem(new DhtmlxToolbarTextItem("ReportNumText", "报告编号"));
            toolBar.AddToolbarItem(new DhtmlxToolbarInputItem("ReportNum", string.Empty) { Width = 140 });
            toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("2"));
            toolBar.AddToolbarItem(new DhtmlxToolbarTextItem("SampleNumText", "样品编号"));
            toolBar.AddToolbarItem(new DhtmlxToolbarInputItem("SampleNum", string.Empty) { Width = 140 });
            toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("3"));
            toolBar.AddToolbarItem(new DhtmlxToolbarTextItem("EntrustNumText", "委托编号"));
            toolBar.AddToolbarItem(new DhtmlxToolbarInputItem("EntrustNum", string.Empty) { Width = 140 });
            toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("3"));
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Search", "快速查询") { Img = "fa fa-search", Imgdis = "fa fa-search" });

            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }


        // GET: TotalSearch/Area
        public ActionResult Area()
        {
            return View();
        }

        // GET: TotalSearch/BH
        public ActionResult BH()
        {
            return View();
        }

        // GET: TotalSearch/Details/5
        public ActionResult Details(string id)
        {
            var model = new ReportDetailViewModel()
            {
                cReportModel = new List<TotalSearchCReportModel>(),
                acsTimeModel = new List<TotalSearchAcsTimeModel>(),
                commonReprtModel = new List<TotalSearchCommonReportModel>(),
                chuJianModel = new List<TotalSearchSampleNum>(),
                modifyModel = new List<TotalSearchModifyModel>()
            };

            var reportConclusion = sysDictService.GetDictsByKey("ReportConclusionCode");
            var getResponse = tbpitemESRep.Get(new DocumentPath<es_t_bp_item>(id));
            if (getResponse.IsValid)
            {
                model.MainItem = getResponse.Source;
                model.MainItem.AUDITINGDATE = null;
                model.CustomName = checkUnitService.GetCheckUnitById(model.MainItem.CUSTOMID);
                model.IsCTypeReport = itemNameService.IsCReport(model.MainItem.REPORTJXLB, model.MainItem.ITEMNAME);
                model.MainItem.CONCLUSIONCODE = SysDictUtility.GetKeyFromDic(reportConclusion, model.MainItem.CONCLUSIONCODE, "/");
                var allItems = itemNameService.GetAllItemName();

                model.ENTRUSTDATE = GetUIDtString(model.MainItem.ENTRUSTDATE);
                model.AUDITINGDATE = GetUIDtString(model.MainItem.AUDITINGDATE);
                model.APPROVEDATE = GetUIDtString(model.MainItem.APPROVEDATE);
                model.CHECKDATE = GetUIDtString(model.MainItem.CHECKDATE);
                model.PRINTDATE = GetUIDtString(model.MainItem.PRINTDATE);


                model.HasWxSchedule = false;


                model.ItemCNName = model.MainItem.ITEMCHNAME.IsNullOrEmpty() ?
                        itemNameService.GetItemCNNameFromAll(allItems, model.MainItem.REPORTJXLB, model.MainItem.ITEMNAME) : model.MainItem.ITEMCHNAME;


                if (model.MainItem.HAVEACS.HasValue && model.MainItem.HAVEACS.Value > 0)//曲线信息
                {
                    model.acsTimeModel = GetAcsTimeDetails(model.MainItem.SYSPRIMARYKEY, model.MainItem.SAMPLENUM);
                }

                if (model.IsCTypeReport)
                {
                    model.cReportModel = GetCReportDetails(model.MainItem.SYSPRIMARYKEY, model.MainItem.REPORTNUM);
                }
                else
                {
                    model.commonReprtModel = GetCommonReportDetails(model.MainItem.CUSTOMID, model.MainItem.REPORTNUM, model.MainItem.ITEMNAME, model.MainItem.SYSPRIMARYKEY);
                }

                if (model.MainItem.HAVELOG.HasValue && model.MainItem.HAVELOG.Value > 0)
                {
                    model.modifyModel = GetModifyDetails(model.MainItem.SYSPRIMARYKEY);
                }


                if (model.MainItem.QRCODEBAR.IsNullOrEmpty())
                {
                    model.MainItem.QRCODEBAR = reportService.GetQzQrcodeBar(model.MainItem.ENTRUSTNUM);
                }

            }
            return View(model);
        }

        public ActionResult TestCategoriesCombo()
        {
            XElement element = BuildTestCategoriesCombo();
            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        protected XElement BuildTestCategoriesCombo()
        {
            var Categorys = itemNameService.GetAllCategoryName();

            XElement element = new XElement("complete",
                 new XElement("template",
                    new XElement("input", new XCData("#name#")),
                    new XElement("header", true),
                    new XElement("columns",
                        new XElement("column",
                            new XAttribute("width", 320),
                            new XAttribute("header", "检测类别"),
                            new XAttribute("option", "#name#")))),
                 new XElement("option",
                           new XAttribute("value", string.Empty),
                           new XElement("text",
                                new XElement("name", "全部"))),
                 from kv in Categorys
                 select new XElement("option",
                            new XAttribute("value", kv.Key),
                            new XElement("text",
                                new XElement("name", kv.Value.IsNullOrEmpty() ? "无检测类别" : kv.Value))));

            return element;
        }

        public ActionResult CheckItemCombo()
        {
            XElement element = BuildCheckItemCombo();
            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult CheckItemChangeCombo( string value)
        {
            XElement element = BuildCheckItemChange(value);
            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }


        protected XElement BuildCheckItemCombo()
        {
            var Categorys = itemNameService.GetAllItemName();

            XElement element = new XElement("complete",
                 new XElement("template",
                    new XElement("input", new XCData("#name#")),
                    new XElement("header", true),
                    new XElement("columns",
                        new XElement("column",
                            new XAttribute("width", 320),
                            new XAttribute("header", "检测类别"),
                            new XAttribute("option", "#name#")))),
                 new XElement("option",
                           new XAttribute("value", string.Empty),
                           new XElement("text",
                                new XElement("name", "全部"))),
                 from kv in Categorys
                 select new XElement("option",
                            new XAttribute("value", kv.Key),
                            new XElement("text",
                                new XElement("name", kv.Value.IsNullOrEmpty() ? "无检测类别" : kv.Value))));

            return element;
        }

        protected XElement BuildCheckItemChange(string value)
        {
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            bool yorn = itemNameService.GetCheckItemChange(value, out dicData);
            XElement element = new XElement("complete",
                 new XElement("template",
                    new XElement("input", new XCData("#name#")),
                    new XElement("header", true),
                    new XElement("columns",
                        new XElement("column",
                            new XAttribute("width", 320),
                            new XAttribute("header", "检测项目"),
                            new XAttribute("option", "#name#")))),
                 //new XElement("option",
                 //          new XAttribute("value", string.Empty),
                 //          new XElement("text",
                 //               new XElement("name", "全部"))),
                 from kv in dicData
                 select new XElement("option",
                            new XAttribute("value", kv.Key),
                            new XElement("text",
                                new XElement("name", kv.Value.IsNullOrEmpty() ? "无检测项目" : kv.Value))));

            return element;
        }

        public ActionResult IsUseCombo()
        {
            XElement element = BuildIsUse();
            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        protected XElement BuildIsUse()
        {
            List<IsUseModel> insts = new List<IsUseModel>();
            insts = customRep.GetByConditon<IsUseModel>(r => r.IsUse == 2, r => new { r.ID, r.NAME });
            Dictionary<string, string> dicData = new Dictionary<string, string>();
            foreach (var item in insts)
            {
                dicData.Add(item.ID, item.NAME);
            }
            XElement element = new XElement("complete",
                new XElement("template",
                   new XElement("input", new XCData("#name#")),
                   new XElement("header", true),
                   new XElement("columns",
                       new XElement("column",
                           new XAttribute("width", 320),
                           new XAttribute("header", "机构名称"),
                           new XAttribute("option", "#name#")))),
                new XElement("option",
                          new XAttribute("value", string.Empty),
                          new XElement("text",
                               new XElement("name", "全部"))),
                from kv in dicData
                select new XElement("option",
                           new XAttribute("value", kv.Key),
                           new XElement("text",
                               new XElement("name", kv.Value.IsNullOrEmpty() ? "无机构名称" : kv.Value))));

            return element;
        }


        public ActionResult Search(SysSearchModel model)
        {
            model.modelType = SysSearchModelModelType.TotalSearch;
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            ISearchResponse<es_t_bp_item> response = GetSearchResult(model);

            int totalCount = (int)response.Total;
            DhtmlxGrid grid = new DhtmlxGrid();
            grid.AddPaging(totalCount, pos);
            int index = pos;
            var pkrReports = reportService.GetPkrReportNums(response.Documents);

            var reportConclusions = sysDictService.GetDictsByKey("ReportConclusionCode");
            var allItems = itemNameService.GetAllItemName();
            var allInsts = checkUnitService.GetAllCheckUnit();

            var noQrCodeBar = new List<string>();

            foreach (var item in response.Documents)
            {
                if (item.QRCODEBAR.IsNullOrEmpty())
                {
                    noQrCodeBar.Add(item.ENTRUSTNUM);
                }
            }

            var QrcodeBars = reportService.GetQzQrcodeBars(noQrCodeBar);

            var allCheckItems = itemNameService.GetAllItemName();

            foreach (var item in response.Documents)
            {
                DhtmlxGridRow row = new DhtmlxGridRow(item.SYSPRIMARYKEY);
                row.AddCell(index + 1);
                row.AddCell(item.PROJECTNAME);
                row.AddCell(SysDictUtility.GetKeyFromDic(reportConclusions, item.CONCLUSIONCODE, "/"));
                row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allInsts, item.CUSTOMID));
                row.AddCell(item.STRUCTPART);
                row.AddCell(itemNameService.GetItemCNNameFromAll(allCheckItems, item.REPORTJXLB, item.ITEMNAME));
                BuildReportNumRow(reportService, pkrReports, item, row);
                BuildQrCodeRow(QrcodeBars, item, row);

                row.AddCell(GetUIDtString(item.CHECKDATE, "yyyy-MM-dd"));
                row.AddCell(GetUIDtString(item.APPROVEDATE, "yyyy-MM-dd"));
                row.AddCell(reportService.GetReportDataStatus(item.SAMPLEDISPOSEPHASEORIGIN));
                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                index++;
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        private static void BuildQrCodeRow(Dictionary<string, string> QrcodeBars, es_t_bp_item item, DhtmlxGridRow row)
        {
            if (item.QRCODEBAR.IsNullOrEmpty())
            {
                //qrcodebar为空的情况下，在起重设备中查询到了相关的数据进入此判断

                if (QrcodeBars.IsNullOrEmpty())
                {
                    row.AddCell(string.Empty);
                }
                else
                {
                    var QzQrcodeBar = string.Empty;
                    QrcodeBars.TryGetValue(item.ENTRUSTNUM, out QzQrcodeBar);
                    if (!QzQrcodeBar.IsNullOrEmpty())//能找到这个二维码
                    {
                        row.AddLinkJsCell(QzQrcodeBar, "OpenQzQr(\"{0}\")".Fmt(QzQrcodeBar));
                    }
                    else
                    {
                        row.AddCell(string.Empty);
                    }
                }
            }
            else
            {
                row.AddLinkJsCell(item.QRCODEBAR, "OpenQr(\"{0}\")".Fmt(item.QRCODEBAR));
            }
        }

        private ISearchResponse<es_t_bp_item> GetSearchResult(SysSearchModel model)
        {
            model.modelType = SysSearchModelModelType.TotalSearch;
            var filterQuery = GetFilterQuery(checkUnitService, itemNameService, model, new Dictionary<string, string>(), new Dictionary<string, string>());

            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            if (model.orderColInd.HasValue && (model.orderColInd.Value >= 6 && model.orderColInd.Value <= 9) && !model.direct.IsNullOrEmpty())
            {
                Expression<Func<es_t_bp_item, object>> sortExp = GetSortExp(model.orderColInd.Value);
                if (model.direct == "asc")
                {
                    return tbpitemESRep.Search(s => s.Source(sf => sf.Includes(GetTotalUIField())).Sort(c => c.Ascending(sortExp)).From(pos).Size(count).Query(filterQuery));
                }
                else
                {
                    return tbpitemESRep.Search(s => s.Source(sf => sf.Includes(GetTotalUIField())).Sort(c => c.Descending(sortExp)).From(pos).Size(count).Query(filterQuery));
                }
            }
            else
            {
                if (model.direct == "asc")
                {

                    return tbpitemESRep.Search(s => s.Source(sf => sf.Includes(GetTotalUIField())).Sort(c => c.Ascending(sa => sa.APPROVEDATE)).From(pos).Size(count).Query(filterQuery));
                }
                else
                {

                    return tbpitemESRep.Search(s => s.Source(sf => sf.Includes(GetTotalUIField())).Sort(c => c.Descending(sa => sa.APPROVEDATE)).From(pos).Size(count).Query(filterQuery));
                }
            }
        }

        private Func<FieldsDescriptor<es_t_bp_item>, IPromise<Fields>> GetTotalUIField()
        {
            return sfi => sfi.Fields(
                                           f => f.PROJECTNAME,//工程名称
                                           f => f.CUSTOMID,//机构id
                                           f => f.STRUCTPART,//检测项目
                                           f => f.REPORTNUM,//报告编号
                                           f => f.SAMPLENUM,//样品编号
                                           f => f.ENTRUSTDATE,//委托日期
                                           f => f.CHECKDATE,//检测日期
                                           f => f.APPROVEDATE,//批准日期
                                           f => f.PRINTDATE,//报告日期
                                           f => f.SYSPRIMARYKEY,
                                           f => f.COLLATEDATE,//采集日期
                                           f => f.QRCODEBAR,//二维码
                                           f => f.REPORTJXLB, // itemcode
                                           f => f.ITEMNAME,//检测项目
                                           f => f.SAMPLEDISPOSEPHASEORIGIN,//数据状态
                                           f => f.CONCLUSIONCODE,//合格
                                           f => f.ENTRUSTNUM//委托单编号
                                       );
        }

        private Expression<Func<es_t_bp_item, object>> GetSortExp(int colIndex)
        {
            if (colIndex == 8)
            {
                return e => e.CHECKDATE;
            }
            else if (colIndex == 9)
            {
                return e => e.APPROVEDATE;
            }
            else if(colIndex == 6)
            {
                return e => e.REPSEQNO;
            }
            //默认 批准日期
            return e => e.CHECKDATE;
        }

        public ActionResult AcsGrid(string id, string SampleNum)
        {
            ISearchResponse<es_t_bp_acs> acsResponse = null;
            if (string.IsNullOrEmpty(SampleNum))
            {
                acsResponse = acsESRep.Search(s => s.Index("gx-t-bp-acs").From(0).Size(20).Sort(cs => cs.Ascending(csd => csd.ACSTIME)).Query(q => q.
                                                          Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(id))
                                                          ))));
            }
            else
            {
                acsResponse = acsESRep.Search(s => s.Index("gx-t-bp-acs").From(0).Size(20).Sort(cs => cs.Ascending(csd => csd.ACSTIME)).Query(q => q.Term(f => f.Field(ff => ff.SYSPRIMARYKEY).Value(id)) && q.Term(f => f.Field(ff => ff.SAMPLENUM).Value(SampleNum))));
            }

            DhtmlxGrid grid = new DhtmlxGrid();
            if (acsResponse.IsValid)
            {
                int index = 0;
                foreach (var item in acsResponse.Documents)
                {
                    index++;
                    DhtmlxGridRow row = new DhtmlxGridRow(item.PK);
                    row.AddCell(index.ToString());
                    row.AddCell(item.SAMPLENUM);
                    row.AddCell(GetUIDtString(item.ACSTIME, "yyyy-MM-dd HH:mm:ss"));
                    row.AddCell(item.MAXVALUE);
                    row.AddLinkJsCell("[查看]", "showAcsGraph(\"{0}\",\"{1}\")".Fmt(index, item.PK));
                    grid.AddGridRow(row);


                }
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult CTypeReport(string id)
        {
            var wordReport = wordESRep.Search(s => s.Index("gx-t-bp-wordreport").From(0).Size(10).Query(q => q.
                                                          Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(id))))));

            if (wordReport.IsValid
                && wordReport.Documents.Count > 0
                && CanViewReport(wordReport.Documents.First().CUSTOMID))
            {
                var wordreport = wordReport.Documents.First();
                string fileName = System.IO.Path.GetFileName(wordreport.WORDREPORTPATH);
                if (!fileName.Contains("."))
                {
                    fileName = string.Format("{0}.doc", fileName);
                }
                string contentType = MimeMapping.GetMimeMapping(fileName);
                string extension = System.IO.Path.GetExtension(fileName);
                //C类报告下载文件名换成报告编号，不要使用平台内部名称
                var tbpItemReport = tbpitemESRep.Search(s => s.Index("gx-tbpitem").From(0).Size(10).Query(q => q.
                                                          Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(id))))));
                if (tbpItemReport.IsValid && tbpItemReport.Documents.Count > 0)
                {
                    var tbpItem = tbpItemReport.Documents.First();
                    fileName = string.Format("{0}{1}", tbpItem.REPORTNUM, extension);
                }

                byte[] fileData = reportService.GetStoreFile(wordreport.WORDREPORTPATH);
                return File(fileData, contentType, fileName);
            }
            else
            {
                var wordExtReport = wordExtESRep.Search(s => s.Index("gx-t-bp-wordreport").From(0).Size(10).Query(q => q.
                                                           Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(id))))));

                if (wordExtReport.IsValid
                    && wordExtReport.Documents.Count > 0
                     && CanViewReport(wordExtReport.Documents.First().CUSTOMID))
                {
                    var wordreport = wordExtReport.Documents.First();
                    string fileName = System.IO.Path.GetFileName(wordreport.WORDREPORTPATH);
                    if (!fileName.Contains("."))
                    {
                        fileName = string.Format("{0}.doc", fileName);
                    }
                    string contentType = MimeMapping.GetMimeMapping(fileName);

                    byte[] fileData = reportService.GetStoreFile(wordreport.WORDREPORTPATH);
                    return File(fileData, contentType, fileName);

                }
                else
                {

                    var sysFileReport = sysfilesEsRep.Search(s => s.Index("gx-t-bp-wordreport").From(0).Size(10).Query(q => q.
                                                         Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(id))))));
                    if (sysFileReport.IsValid
                   && sysFileReport.Documents.Count > 0
                    && CanViewReport(sysFileReport.Documents.First().CUSTOMID))
                    {
                        var wordreport = sysFileReport.Documents.First();
                        string fileName = System.IO.Path.GetFileName(wordreport.REALDATAPATH);
                        if (!fileName.Contains("."))
                        {
                            fileName = string.Format("{0}.doc", fileName);
                        }
                        string contentType = MimeMapping.GetMimeMapping(fileName);

                        byte[] fileData = reportService.GetStoreFile(wordreport.REALDATAPATH);
                        return File(fileData, contentType, fileName);

                    }
                    else
                    {
                        throw new HttpException(404, "文件不存在");
                    }
                }
            }
        }

        public List<TotalSearchAcsTimeModel> GetAcsTimeDetails(string id, string SampleNum)
        {
            ISearchResponse<es_t_bp_acs> acsResponse = null;
            if (string.IsNullOrEmpty(SampleNum))
            {
                acsResponse = acsESRep.Search(s => s.Index("gx-t-bp-acs").From(0).Size(20).Sort(cs => cs.Ascending(csd => csd.ACSTIME)).Query(q => q.
                                                        Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(id))
                                                        ))));
            }
            else
            {
                acsResponse = acsESRep.Search(s => s.Index("gx-t-bp-acs").From(0).Size(20).Sort(cs => cs.Ascending(csd => csd.ACSTIME)).Query(q => q.Term(f => f.Field(ff => ff.SYSPRIMARYKEY).Value(id)) && q.Term(f => f.Field(ff => ff.SAMPLENUM).Value(SampleNum))));
            }
            List<TotalSearchAcsTimeModel> model = new List<TotalSearchAcsTimeModel>();

            if (acsResponse.IsValid)
            {
                int index = 1;
                foreach (var item in acsResponse.Documents)
                {
                    TotalSearchAcsTimeModel oneModel = new TotalSearchAcsTimeModel();
                    oneModel.SysPrimaryKey = item.PK;
                    oneModel.index = index.ToString();
                    oneModel.SAMPLENUM = item.SAMPLENUM;
                    oneModel.MAXVALUE = item.MAXVALUE;
                    oneModel.ACSTIME = GetUIDtString(item.ACSTIME, "yyyy-MM-dd HH:mm:ss");
                    model.Add(oneModel);
                    index++;
                }
            }
            return model;
        }

        private static MemoryStream DocumentToPdf(ref string fileName, byte[] data)
        {
            MemoryStream datams = new MemoryStream();
            if (fileName.Contains(".doc") || fileName.Contains(".docx"))
            {
                fileName = fileName.Split('.')[0] + ".pdf";
                Aspose.Words.Document doc = new Aspose.Words.Document(new MemoryStream(data));
                doc.Save(datams, Aspose.Words.SaveFormat.Pdf);

            }
            else if (fileName.Contains(".xls") || fileName.Contains(".xlsx"))
            {
                fileName = fileName.Split('.')[0] + ".pdf";
                Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(new MemoryStream(data));
                workbook.Save(datams, Aspose.Cells.SaveFormat.Pdf);
            }
            else
            {
                datams = new MemoryStream(data);
            }

            return datams;
        }

        private bool CanViewReport(string customId)
        {
            if (customId.IsNullOrEmpty())
            {
                return false;
            }

            var filterInsts = GetCurrentInstFilter();
            if (filterInsts.NeedFilter)
            {
                if (filterInsts.FilterInstIds.Count == 0)
                {
                    return false;
                }
                else
                {
                    return filterInsts.FilterInstIds.Contains(customId);
                }
            }
            else
            {
                return true;
            }
        }

        public ActionResult CommonReportImage(string id)
        {
            //已uploadtime排序
            var pkrResponse = pkrESRep.Search(s => s.Index("gx-t-pkpm-pkr").From(0).Size(10).Sort(cs => cs.Descending(sd => sd.UPLOADTIME)).Query(q => q.
                                                     Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.REPORTNUM).Value(id))))));

            if (pkrResponse.IsValid
                && pkrResponse.Documents.Count > 0
                && CanViewReport(pkrResponse.Documents.First().CUSTOMID))
            {
                var pkrFolder = ConfigurationManager.AppSettings["PKRTempFolder"];
                var pkrReport = pkrResponse.Documents.First();
                string reportPath = pkrReport.REPORTPATH;
                string fileName = System.IO.Path.GetFileName(reportPath);
                string filePath = System.IO.Path.Combine(pkrFolder, fileName);

                byte[] data = null;
                data = reportService.GetStoreFile(reportPath);
                System.IO.File.WriteAllBytes(filePath, data);

                var imageData = reportService.GetImageFromPkr(filePath);
                var imageFileName = "报告文件{0}-{1}.png".Fmt(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.Ticks);
                string contentType = MimeMapping.GetMimeMapping(imageFileName);
                return new FileContentResult(imageData, contentType);
            }
            else
            {
                var noImageFolder = ConfigurationManager.AppSettings["NoPKRImageFolder"];
                var fileData = System.IO.File.ReadAllBytes(noImageFolder);
                string imageFileName = "graph.png";
                string contentType = MimeMapping.GetMimeMapping(imageFileName);
                return new FileContentResult(fileData, contentType);
            }
        }

        public ActionResult CommonReportPdf(string id)
        {
            //已uploadtime排序
            var pkrResponse = pkrESRep.Search(s => s.Index("gx-t-pkpm-pkr").From(0).Size(10).Sort(cs => cs.Descending(sd => sd.UPLOADTIME)).Query(q => q.
                                                     Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.REPORTNUM).Value(id))))));

            if (pkrResponse.IsValid
                && pkrResponse.Documents.Count > 0
                && CanViewReport(pkrResponse.Documents.First().CUSTOMID))
            {
                var pkrFolder = ConfigurationManager.AppSettings["PKRTempFolder"];
                var pkrReport = pkrResponse.Documents.First();
                string reportPath = pkrReport.REPORTPATH;
                var pdfData = reportService.GetPdfFromPkr(reportPath);
                var imageFileName = "报告文件{0}-{1}.pdf".Fmt(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.Ticks);
                string contentType = MimeMapping.GetMimeMapping(imageFileName);
                return new FileContentResult(pdfData, contentType);
            }
            else
            {
                var noImageFolder = ConfigurationManager.AppSettings["NoPKRImageFolder"];
                var fileData = System.IO.File.ReadAllBytes(noImageFolder);
                string imageFileName = "graph.png";
                string contentType = MimeMapping.GetMimeMapping(imageFileName);
                return new FileContentResult(fileData, contentType);
            }
        }

        public ActionResult CommonCryptReportPdf(string id)
        {
            //已uploadtime排序
            var pkrReportNum = reportService.GetClearReportNum(id);

            var pkrResponse = pkrESRep.Search(s => s.Index("gx-t-pkpm-pkr").From(0).Size(10).Sort(cs => cs.Descending(sd => sd.UPLOADTIME)).Query(q => q.
                                                     Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.REPORTNUM).Value(pkrReportNum))))));

            if (pkrResponse.IsValid
                && pkrResponse.Documents.Count > 0)
            {
                var pkrFolder = ConfigurationManager.AppSettings["PKRTempFolder"];
                var pkrReport = pkrResponse.Documents.First();
                string reportPath = pkrReport.REPORTPATH;
                byte[] pdfData = reportService.GetPdfFromPkr(reportPath);

                var imageFileName = "报告文件{0}-{1}.pdf".Fmt(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.Ticks);
                string contentType = MimeMapping.GetMimeMapping(imageFileName);
                return new FileContentResult(pdfData, contentType);
            }
            else
            {
                var noImageFolder = ConfigurationManager.AppSettings["NoPKRImageFolder"];
                var fileData = System.IO.File.ReadAllBytes(noImageFolder);
                string imageFileName = "graph.png";
                string contentType = MimeMapping.GetMimeMapping(imageFileName);
                return new FileContentResult(fileData, contentType);
            }
        }

        public ActionResult CommonReport(string id)
        {
            var pkrResponse = pkrESRep.Search(s => s.Index("gx-t-pkpm-pkr").From(0).Size(50).Query(q => q.
                                                  Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.REPORTNUM).Value(id))))));

            if (pkrResponse.IsValid
                && pkrResponse.Documents.Count > 0
                && CanViewReport(pkrResponse.Documents.First().CUSTOMID))
            {
                string reportPath = pkrResponse.Documents.First().REPORTPATH;
                byte[] data = null;
                string fileName = System.IO.Path.GetFileName(reportPath);
                var pkrFolder = ConfigurationManager.AppSettings["PKRTempFolder"];
                string filePath = System.IO.Path.Combine(pkrFolder, fileName);

                data = reportService.GetStoreFile(reportPath);
                System.IO.File.WriteAllBytes(filePath, data);

                var imageData = reportService.GetImageFromPkr(filePath);
                var imageFileName = "报告文件{0}-{1}.png".Fmt(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.Ticks);
                string contentType = MimeMapping.GetMimeMapping(imageFileName);
                return new FileContentResult(imageData, contentType) { FileDownloadName = imageFileName };
            }
            else
            {
                throw new HttpException(404, "文件不存在");
            }
        }

        public ActionResult CommonReportByPath(string path)
        {
            string reportPath = SymCryptoUtility.Decrypt(path);

            var imageData = reportService.GetPdfFromPkr(reportPath);
            var imageFileName = "报告文件{0}-{1}.pdf".Fmt(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.Ticks);
            string contentType = MimeMapping.GetMimeMapping(imageFileName);
            return new FileContentResult(imageData, contentType);
        }

        public ActionResult CReportGrid(string id, string reportNum)
        {
            var wordReport = wordESRep.Search(s => s.Index("gx-t-bp-attach").From(0).Size(10).Query(q => q.
                                                         Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(id))))));

            DhtmlxGrid grid = new DhtmlxGrid();
            int index = 0;
            if (wordReport.IsValid)
            {

                foreach (var item in wordReport.Documents)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    DhtmlxGridRow row = new DhtmlxGridRow(item.SYSPRIMARYKEY);
                    row.AddCell((index + 1).ToString());
                    row.AddCell(GetUIDtString(item.UPLOADTIME));
                    row.AddCell(item.REPORTTYPES);
                    //row.AddLinkJsCell("点击查看", "viewCTypeReport(\"{0}\")".Fmt(item.SYSPRIMARYKEY));
                    dict.Add("点击查看", "viewCTypeReport(\"{0}\")".Fmt(item.SYSPRIMARYKEY));
                    dict.Add("防伪二维码", "buildBarCode(\"{0}\",\"{1}\")".Fmt(id, string.Empty));
                    row.AddLinkJsCells(dict);
                    index++;
                    grid.AddGridRow(row);
                }
            }

            var wordExtReport = wordExtESRep.Search(s => s.Index("gx-t-bp-attach").From(0).Size(10).Query(q => q.
                                                          Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(id))))));
            if (wordExtReport.IsValid)
            {
                foreach (var item in wordExtReport.Documents)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    DhtmlxGridRow row = new DhtmlxGridRow(item.SYSPRIMARYKEY);
                    row.AddCell((index + 1).ToString());
                    row.AddCell(GetUIDtString(item.UPLOADTIME));
                    row.AddCell(item.FILETYPE);
                    //row.AddLinkJsCell("点击查看", "viewCTypeReport(\"{0}\")".Fmt(item.SYSPRIMARYKEY));
                    dict.Add("点击查看", "viewCTypeReport(\"{0}\")".Fmt(item.SYSPRIMARYKEY));
                    dict.Add("防伪二维码", "buildBarCode(\"{0}\",\"{1}\")".Fmt(id, string.Empty));
                    row.AddLinkJsCells(dict);
                    index++;
                    grid.AddGridRow(row);
                }
            }

            var sysFiles = sysfilesEsRep.Search(s => s.Index("gx-t-bp-attach").From(0).Size(10).Query(q => q.Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(id))))));
            if (sysFiles.IsValid)
            {
                foreach (var item in sysFiles.Documents)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    DhtmlxGridRow row = new DhtmlxGridRow(item.SYSPRIMARYKEY);
                    row.AddCell((index + 1).ToString());
                    row.AddCell(GetUIDtString(item.UPLOADTIME));
                    row.AddCell(item.FILETYPE);
                    //row.AddLinkJsCell("点击查看", "viewCTypeReport(\"{0}\")".Fmt(item.SYSPRIMARYKEY));
                    dict.Add("点击查看", "viewCTypeReport(\"{0}\")".Fmt(item.SYSPRIMARYKEY));
                    dict.Add("防伪二维码", "buildBarCode(\"{0}\",\"{1}\")".Fmt(id, string.Empty));
                    row.AddLinkJsCells(dict);
                    index++;
                    grid.AddGridRow(row);
                }
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");

        }

        public List<TotalSearchCReportModel> GetCReportDetails(string id, string reportNum)
        {
            var wordReport = wordESRep.Search(s => s.Index("gx-t-bp-wordreport").From(0).Size(10).Query(q => q.
                                                         Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(id))))));
            List<TotalSearchCReportModel> model = new List<TotalSearchCReportModel>();
            int index = 1;
            if (wordReport.IsValid)
            {

                foreach (var item in wordReport.Documents)
                {
                    TotalSearchCReportModel oneModel = new TotalSearchCReportModel();
                    oneModel.index = index.ToString();
                    oneModel.UploadTime = GetUIDtString(item.UPLOADTIME);
                    oneModel.ReportTypes = item.REPORTTYPES;
                    oneModel.SysPrimartKey = item.SYSPRIMARYKEY;
                    oneModel.Id = id;
                    model.Add(oneModel);
                    index++;
                }
            }

            return model;
        }

        public ActionResult CommonReportGrid(string customId, string reportNum, string itemCode, string id)
        {
            Func<QueryContainerDescriptor<es_t_pkpm_binaryReport>, QueryContainer> filterQuery = reportService.GetPkrFilter(customId, itemCode, reportNum);

            var pkrResponse = pkrESRep.Search(s => s.Index("gx-t-pkpm-pkr").Sort(cs => cs.Descending(csd => csd.UPLOADTIME)).From(0).Size(50).Query(filterQuery));

            DhtmlxGrid grid = new DhtmlxGrid();
            if (pkrResponse.IsValid)
            {
                int index = 0;
                foreach (var item in pkrResponse.Documents)
                {
                    //过滤无附件的报表
                    if (item.REPORTPATH.IsNullOrEmpty())
                    {
                        continue;
                    }
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    DhtmlxGridRow row = new DhtmlxGridRow(Guid.NewGuid().ToString());
                    row.AddCell((index + 1).ToString());
                    row.AddCell(GetUIDtString(item.UPLOADTIME, "yyyy-MM-dd HH:mm:ss"));
                    row.AddCell(reportNum);
                    dict.Add("点击查看", "viewCommonReportBypath(\"{0}\")".Fmt(SymCryptoUtility.Encrypt(item.REPORTPATH)));
                    dict.Add("防伪二维码", "buildBarCode(\"{0}\",\"{1}\")".Fmt(id, string.Empty));
                    row.AddLinkJsCells(dict);
                    index++;
                    grid.AddGridRow(row);
                }
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");

        }

        public List<TotalSearchCommonReportModel> GetCommonReportDetails(string customId, string reportNum, string itemCode, string id)
        {
            Func<QueryContainerDescriptor<es_t_pkpm_binaryReport>, QueryContainer> filterQuery = reportService.GetPkrFilter(customId, itemCode, reportNum);

            var pkrResponse = pkrESRep.Search(s => s.Index("gx-t-pkpm-pkr").Sort(cs => cs.Descending(csd => csd.UPLOADTIME)).From(0).Size(50).Query(filterQuery));
            List<TotalSearchCommonReportModel> model = new List<TotalSearchCommonReportModel>();
            if (pkrResponse.IsValid)
            {
                int index = 1;
                foreach (var item in pkrResponse.Documents)
                {
                    //过滤无附件的报表
                    if (item.REPORTPATH.IsNullOrEmpty())
                    {
                        continue;
                    }
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    TotalSearchCommonReportModel oneModel = new TotalSearchCommonReportModel();
                    oneModel.index = index.ToString();
                    oneModel.UpLoadTime = GetUIDtString(item.UPLOADTIME, "yyyy-MM-dd HH:mm:ss");
                    oneModel.ReportNum = reportNum;
                    oneModel.ReportPath = SymCryptoUtility.Encrypt(item.REPORTPATH);
                    oneModel.Id = id;
                    index++;
                    model.Add(oneModel);
                }
            }
            return model;
        }

        public ActionResult Question(string id)
        {
            var questionResponse = questionESRep.Search(s => s.Index("gx-t-bp-subitem").From(0).Size(50).Query(q => q.
                                                   Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.QUESTIONPRIMARYKEY).Value(id))))));

            DhtmlxGrid grid = new DhtmlxGrid();
            if (questionResponse.IsValid)
            {
                var questionTypes = sysDictService.GetDictsByKey("questionTypes");

                int index = 0;
                foreach (var item in questionResponse.Documents)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(item.PK);
                    row.AddCell((index + 1).ToString());
                    row.AddCell(GetUIDtString(item.RECORDTIME, "yyyy-MM-dd HH:mm:ss"));
                    row.AddCell(reportService.GetReportDataStatus(item.RECORDINGPHASE));
                    row.AddCell(item.RECORDMAN);
                    row.AddCell(SysDictUtility.GetKeyFromDic(questionTypes, item.QUESTIONTYPES));
                    row.AddCell(item.CONTEXT);

                    index++;
                    grid.AddGridRow(row);
                }

                grid.Header = new DhtmlxGridHeader();
                grid.Header.AddColumn(new DhtmlxGridColumn("序号") { ColumnType = "ro", Width = "25", ColumnSort = "int" });
                grid.Header.AddColumn(new DhtmlxGridColumn("记录时间") { ColumnType = "ro", Width = "180", ColumnSort = "str" });
                grid.Header.AddColumn(new DhtmlxGridColumn("修改阶段") { ColumnType = "ro", Width = "180", ColumnSort = "str" });
                grid.Header.AddColumn(new DhtmlxGridColumn("修改人") { ColumnType = "ro", Width = "*", ColumnSort = "str" });
                grid.Header.AddColumn(new DhtmlxGridColumn("修改类别") { ColumnType = "ro", Width = "150", ColumnSort = "str" });
                grid.Header.AddColumn(new DhtmlxGridColumn(" 修改原因") { ColumnType = "ro", Width = "200", ColumnSort = "str" });
            }

            string str = grid.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult ModifyRecordDetails(string key)
        {
            ModifyRecordDetailsModel viewModel = new ModifyRecordDetailsModel()
            {
                ModifyDetailsModel = new List<Models.TotalSearchQuestionModel>()
            };
            var questionResponse = questionESRep.Search(s => s.Index("gx-t-bp-question").From(0).Size(50).Query(q => q.
                                                   Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.QUESTIONPRIMARYKEY).Value(key))))));//gx-t-bp-subitem

            List<TotalSearchQuestionModel> model = new List<TotalSearchQuestionModel>();
            if (questionResponse.IsValid)
            {
                var questionTypes = sysDictService.GetDictsByKey("questionTypes");

                int index = 1;
                foreach (var item in questionResponse.Documents)
                {
                    TotalSearchQuestionModel oneModel = new TotalSearchQuestionModel();
                    oneModel.index = index.ToString();
                    oneModel.RecordTime = GetUIDtString(item.RECORDTIME, "yyyy-MM-dd HH:mm:ss");
                    oneModel.RecordingPhase = reportService.GetReportDataStatus(item.RECORDINGPHASE);
                    oneModel.QuestionType = SysDictUtility.GetKeyFromDic(questionTypes, item.QUESTIONTYPES);
                    oneModel.Context = item.CONTEXT;
                    oneModel.RecordMan = item.RECORDMAN;
                    model.Add(oneModel);
                    index++;
                }

                //grid.Header = new DhtmlxGridHeader();
                //grid.Header.AddColumn(new DhtmlxGridColumn("序号") { ColumnType = "ro", Width = "25", ColumnSort = "int" });
                //grid.Header.AddColumn(new DhtmlxGridColumn("记录时间") { ColumnType = "ro", Width = "180", ColumnSort = "str" });
                //grid.Header.AddColumn(new DhtmlxGridColumn("修改阶段") { ColumnType = "ro", Width = "180", ColumnSort = "str" });
                //grid.Header.AddColumn(new DhtmlxGridColumn("修改人") { ColumnType = "ro", Width = "*", ColumnSort = "str" });
                //grid.Header.AddColumn(new DhtmlxGridColumn("修改类别") { ColumnType = "ro", Width = "150", ColumnSort = "str" });
                //grid.Header.AddColumn(new DhtmlxGridColumn(" 修改原因") { ColumnType = "ro", Width = "200", ColumnSort = "str" });
            }
            viewModel.ModifyDetailsModel = model;
            return View(viewModel);
        }

        public ActionResult Modify(string id)
        {
            var modifyResponse = modifylogESRep.Search(s => s.Index("gx-t-bp-subitem").From(0).Size(50).Query(q => q.
                                                           Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(id))))));

            DhtmlxGrid grid = new DhtmlxGrid();
            if (modifyResponse.IsValid)
            {
                int index = 0;
                foreach (var item in modifyResponse.Documents)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(item.PK);
                    row.AddCell("/TotalSearch/Question?id=" + item.QUESTIONPRIMARYKEY);
                    row.AddCell((index + 1).ToString());
                    row.AddCell(item.FIELDNAME);
                    row.AddCell(item.BEFOREMODIFYVALUES);
                    row.AddCell(item.AFTERMODIFYVALUES);
                    row.AddCell(GetUIDtString(item.MODIFYDATETIME, "yyyy-MM-dd HH:mm:ss"));
                    index++;

                    grid.AddGridRow(row);

                }
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public List<TotalSearchModifyModel> GetModifyDetails(string id)
        {
            var modifyResponse = modifylogESRep.Search(s => s.Index("gx-t-bp-modifylog").From(0).Size(50).Query(q => q.
                                                           Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(id))))));
            List<TotalSearchModifyModel> model = new List<TotalSearchModifyModel>();
            if (modifyResponse.IsValid)
            {
                int index = 1;
                foreach (var item in modifyResponse.Documents)
                {
                    TotalSearchModifyModel oneModel = new TotalSearchModifyModel();
                    oneModel.QuestionPrimaryKey = item.QUESTIONPRIMARYKEY;
                    oneModel.index = index.ToString();
                    oneModel.FieldName = item.FIELDNAME;
                    oneModel.BeforeModifyValues = item.BEFOREMODIFYVALUES;
                    oneModel.AfterModifyValues = item.AFTERMODIFYVALUES;
                    oneModel.ModifyDateTime = GetUIDtString(item.MODIFYDATETIME, "yyyy-MM-dd HH:mm:ss");
                    index++;
                    model.Add(oneModel);
                }
            }
            return model;
        }

        public ActionResult DetailsByReportNum(string reportNum)
        {
            ReportDetailViewModel model = new Models.ReportDetailViewModel();
            var reportConclusion = sysDictService.GetDictsByKey("ReportConclusionCode");
            var getResponse = tbpitemESRep.Search(s => s.From(0).Size(5).Sort(cs => cs.Descending(csd => csd.UPLOADTIME)).Query(q => q.
                                                      Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.REPORTNUM).Value(reportNum))))));
            //var getResponse = tbpitemESRep.Get(new DocumentPath<es_t_bp_item>(id));
            if (getResponse.IsValid)
            {
                var allItems = itemNameService.GetAllItemName();

                model.MainItem = getResponse.Documents.FirstOrDefault();

                model.MainItem.CONCLUSIONCODE = SysDictUtility.GetKeyFromDic(reportConclusion, model.MainItem.CONCLUSIONCODE, "/");
                model.ItemCNName = itemNameService.GetItemCNNameFromAll(allItems, model.MainItem.ITEMNAME, model.MainItem.CUSTOMID);
                model.IsCTypeReport = itemNameService.IsCReport(model.MainItem.REPORTJXLB, model.MainItem.ITEMNAME);
                //es_t_bp_item item = getResponse.Source;

                model.HasWxSchedule = false;
                List<string> curlistCodes = covrlistService.GetScheduleCodes(model.MainItem.SYSPRIMARYKEY);
                if (curlistCodes.Count > 0)
                {
                    model.HasWxSchedule = true;
                }

                model.HasQrCodeBar = !model.MainItem.CODEBAR.IsNullOrEmpty();
                string FujianNum = string.Empty;
                //if (!string.IsNullOrEmpty(model.MainItem.SAMPLENUM) && IsExistsFujian(model.MainItem.SAMPLENUM, model.MainItem.CUSTOMID, out FujianNum))
                //{
                //    model.FujianID = FujianNum;
                //}
            }

            return View(model);
        }

        public ActionResult Export(SysSearchModel model, int? fileFormat)
        {
            model.modelType = SysSearchModelModelType.TotalSearch;
            ISearchResponse<es_t_bp_item> response = GetSearchResult(model);

            var allItems = itemNameService.GetAllItemName();
            var allInsts = checkUnitService.GetAllCheckUnit();

            // 改动2：创建导出类实例（而非 DhtmlxGrid），设置列标题
            bool xlsx = (fileFormat ?? 2007) == 2007;
            ExcelExporter ee = new ExcelExporter("综合查询报告", xlsx);
            ee.SetColumnTitles("序号,状态, 机构名称, 工程名称, 结构部位, 项目名称, 委托日期, 检测日期, 批准日期, 样本编号, 报告编号, 检测结论");
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int index = pos;
            foreach (var item in response.Documents)
            {
                // 改动3：添加 ExcelRow 对象（而非 dhtmlxGridRow）
                ExcelRow row = ee.AddRow();

                row.AddCell((index + 1).ToString());
                row.AddCell(reportService.GetReportDataStatus(item.SAMPLEDISPOSEPHASE));
                row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allInsts, item.CUSTOMID));
                row.AddCell(item.PROJECTNAME);
                row.AddCell(item.STRUCTPART);

                string uiItemName = string.Empty;
                //C类报告显示181规范的项目
                //if (item.ISCREPORT.HasValue && item.ISCREPORT.Value == 1)
                //{
                //    var subItem = itemNameService.GetItemByItemCode(item.SUBITEMCODE);
                //    if (subItem != null)
                //    {
                //        uiItemName = subItem.itemname;
                //    }
                //}

                if (uiItemName.IsNullOrEmpty())
                {
                    uiItemName = item.ITEMCHNAME.IsNullOrEmpty() ?
                            itemNameService.GetItemCNNameFromAll(allItems, item.ITEMNAME, item.CUSTOMID) : item.ITEMCHNAME;
                }

                row.AddCell(uiItemName);
                row.AddCell(GetUIDtString(item.ENTRUSTDATE));
                row.AddCell(GetUIDtString(item.CHECKDATE));
                row.AddCell(GetUIDtString(item.APPROVEDATE));
                row.AddCell(item.SAMPLENUM);
                row.AddCell(item.REPORTNUM);
                //row.AddCell(item.REPORTNUM);
                row.AddCell(item.CHECKCONCLUSION);

                index++;
            }

            // 改动4：返回字节流
            return File(ee.GetAsBytes(), ee.MIME, ee.FileName);
        }

        public ActionResult BuildBarCode(string id, string qrinfo)
        {
            //int pixelsPerModule = 5;//可以控制生成的二维码尺寸大小
            byte[] QrCodeImgData = reportQr.Encode(id, qrinfo); // QrCodeGenerator.Generate(pixelsPerModule, id);
            string imageFileName = "barcode.png";
            string contentType = MimeMapping.GetMimeMapping(imageFileName);
            return new FileContentResult(QrCodeImgData, contentType);
        }

        public ActionResult SearchBH(ReportBHSearchModel searchModel)
        {
            var searchResult = GetSearchResultBH(searchModel);
            DhtmlxGrid grid = new DhtmlxGrid();
            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            grid.AddPaging(searchResult.TotalCount, pos);

            var listSysKeys = searchResult.Results.Select(s => s.SYSPRIMARYKEY).ToList();
            var newListSysKeys = listSysKeys.Select(s => GetRealSysKey(s)).ToList();
            var res = tbpitemESRep.Search(s => s.Size(100).Source(ss => ss.Includes(ssi => ssi.Field(ssif => ssif.SYSPRIMARYKEY))).Query(q => q.Terms(qt => qt.Field(qtf => qtf.SYSPRIMARYKEY).Terms(newListSysKeys))).Index("gx-tbpitem"));
            Dictionary<string, int> dicHaveReport = new Dictionary<string, int>();
            if (res.IsValid)
            {
                foreach (var item in res.Documents)
                {
                    dicHaveReport[item.SYSPRIMARYKEY] = 1;
                }
            }

            for (int i = 0; i < searchResult.Results.Count; i++)
            {
                var item = searchResult.Results[i];
                string SysKey = GetRealSysKey(item.SYSPRIMARYKEY);
                DhtmlxGridRow row = new DhtmlxGridRow(Guid.NewGuid().ToString());
                row.AddCell((i + 1));
                row.AddCell(item.DetectUnit);
                row.AddCell(item.projectname);
                row.AddCell(item.ProjPart);
                row.AddCell(item.itemname);
                if (dicHaveReport.ContainsKey(SysKey))
                {
                    row.AddLinkJsCell(item.reportnum, "ReportDetail(\"{0}\")".Fmt(SysKey));
                }
                else
                {
                    row.AddCell(item.reportnum);
                }

                if (item.qrinfo.IsNullOrEmpty())
                {
                    row.AddLinkJsCell("/", string.Empty);
                }
                else
                {
                    if (item.qrinfo.Contains("GXT"))
                    {
                        row.AddLinkJsCell(item.qrinfo, "openQzQrDetail(\"{0}\")".Fmt(item.qrinfo));
                    }
                    else
                    {
                        row.AddLinkJsCell(item.qrinfo, "openQrDetail(\"{0}\")".Fmt(item.qrinfo));
                    }
                }

                if (item.status.HasValue)
                {
                    if (item.status.Value == 1)
                    {
                        row.AddLinkJsCell("取样员已闭合", "bhDetail(\"{0}\")".Fmt(item.SYSPRIMARYKEY));
                    }
                    else if (item.status.Value == 2)
                    {
                        row.AddLinkJsCell("见证员已闭合", "bhDetail(\"{0}\")".Fmt(item.SYSPRIMARYKEY));
                    }
                    else
                    {
                        row.AddLinkJsCell("未闭合", string.Empty);
                    }
                }
                else
                {
                    row.AddLinkJsCell("未闭合", string.Empty);
                }
                row.AddCell(item.reportdate);
                row.AddCell(item.TAKESAMPLEMAN);
                row.AddCell(item.WITNESSMAN);
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        private static string GetRealSysKey(string bhSysPrimaryKey)
        {
            if (bhSysPrimaryKey.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var SysKey = bhSysPrimaryKey;
            int customIdLength = 7;
            int normalSysLength = 25;
            //闭合表中主键 可能有一个机构id，也有可能有2个机构id，也有可能两个机构id不一致，所以通过长度来判断
            //超过25 长度的自动截取前面7为机构id
            if (SysKey.Length > normalSysLength)
            {

                SysKey = SysKey.Substring(customIdLength);

            }

            return SysKey;
        }
        public ActionResult BHDetails(string primarykey)
        {
            HBDetailsModels model = new HBDetailsModels();
           // primarykey = "00001PA1900000043917";
            var bhDetailsClientUrl = ConfigurationManager.AppSettings["BHDetailsClient"];
            var bhDetailsImgUrl = ConfigurationManager.AppSettings["BHDetailsImg"];
            var baseUrl = bhDetailsClientUrl.Fmt("&");
            //var client = new RestClient("http://gxjzqy.jyjzqy.com/orders/ashx/GetResultRecordWithOutSession.ashx?tablename=view_reportbh&primarykey=sysprimarykey");
            var client = new RestClient(baseUrl);
            client.AddDefaultParameter("primaryvalue", primarykey);
            client.Proxy = null;
            var request = new RestRequest();
            IRestResponse<BHDetailsModel> response2 = client.Execute<BHDetailsModel>(request);
            model.BH = response2.Data;
            if (model.BH != null)
            {
                model.BH.closurebeforenum = GetStringByNum(model.BH.closurebeforenum);//"未变更";
                model.BH.closednum = GetStringByNum(model.BH.closednum);// + "次变更";
                model.BH.closureafternum = GetStringByNum(model.BH.closureafternum);// + "次变更";
                model.BH.closurebefore = bhDetailsImgUrl + model.BH.closurebefore;
                model.BH.closed = bhDetailsImgUrl + model.BH.closed;
                model.BH.closureafter = bhDetailsImgUrl + model.BH.closureafter;
            }
            
            return View(model);
        }
        public ActionResult BHChangeDetails(string key, string status)
        {
            var bhDetailsUrl = ConfigurationManager.AppSettings["BHChangeDetailsClient"];
            var bhDetailsImgUrl = ConfigurationManager.AppSettings["BHDetailsImg"];
            BHChangeDetailsModels model = new BHChangeDetailsModels();
            string str = "sysprimarykey=[[GGGGG]]{0}[[GGGGG]] and status =[[GGGGG]]{1}[[GGGGG]]".Fmt(key, status);//[[GGGGG]]00001PA1800000039861[[GGGGG]] and status =[[GGGGG]]0[[GGGGG]]
            var client = new RestClient(bhDetailsUrl);
            client.AddDefaultParameter("w", str);
            client.Proxy = null;
            var request = new RestRequest();

            IRestResponse<BHChangeDetailsModel> response2 = client.Execute<BHChangeDetailsModel>(request);
            model.BHChangeDetails = response2.Data;
            if (int.Parse(model.BHChangeDetails.total) > 0)
            {
                model.oldphoto = bhDetailsImgUrl + model.BHChangeDetails.rows.First().oldphoto;
                model.oldtime = model.BHChangeDetails.rows.First().oldtime;

                foreach (var item in model.BHChangeDetails.rows)
                {
                    item.oldphoto = bhDetailsImgUrl + item.oldphoto;
                    item.newphoto = bhDetailsImgUrl + item.newphoto;
                }
            }
            return View(model);
        }

        private string GetStringByNum(string num)
        {
            if (num.IsNullOrEmpty())
            {
                return "未变更";
            }
            else
            {
                return num + "次变更";
            }
        }

        private SearchResult<view_reportbh> GetSearchResultBH(ReportBHSearchModel searchModel)
        {

            var predicate = PredicateBuilder.True<view_reportbh>();

            #region 动态查询
            if (!string.IsNullOrWhiteSpace(searchModel.CheckInst))
            {

                predicate = predicate.And(t => t.customid == searchModel.CheckInst);
            }

            var instFilter = GetCurrentInstFilter();
            if (instFilter.NeedFilter && instFilter.FilterInstIds.Count() > 0)
            {

                predicate = predicate.And(t => instFilter.FilterInstIds.Contains(t.customid));
            }
            if (!string.IsNullOrEmpty(searchModel.ProjectName))
            {
                predicate = predicate.And(t => t.projectname.Contains(searchModel.ProjectName));
            }
            if (searchModel.StartDt.HasValue)
            {
                predicate = predicate.And(t => t.reportdate >= searchModel.StartDt);
            }
            if (searchModel.EndDt.HasValue)
            {
                predicate = predicate.And(t => t.reportdate <= searchModel.EndDt);
            }
            if (!string.IsNullOrEmpty(searchModel.EntrustNo))
            {
                predicate = predicate.And(t => t.entrustnum == searchModel.EntrustNo || t.samplenum == searchModel.EntrustNo || t.reportnum == searchModel.EntrustNo);
            }
            if (!string.IsNullOrEmpty(searchModel.ItemName))
            {

                if (searchModel.ItemName.Length == 8)
                {
                    var typeCode = searchModel.ItemName.Substring(0, 4);
                    var itemCode = searchModel.ItemName.Substring(4, 4);
                    predicate = predicate.And(t => t.itemcode == itemCode);
                }
                else
                {
                    predicate = predicate.And(t => t.itemcode == searchModel.ItemName);
                }
            }
            if (!string.IsNullOrWhiteSpace(searchModel.Area))
            {
                var areas = searchModel.Area.Split(',').ToList();
                if (!areas.Contains("全部"))
                {
                    Dictionary<string, string> CheckUnits = checkUnitService.GetUnitByArea(areas);
                    if (CheckUnits.Count > 0)
                    {
                        predicate = predicate.And(t => CheckUnits.Keys.ToList().Contains(t.customid));
                    }
                }

            }
            if (searchModel.IsBH.HasValue)
            {
                predicate = predicate.And(t => t.status == searchModel.IsBH);
            }
            #endregion

            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            int count = searchModel.count.HasValue ? searchModel.count.Value : 30;
            IDbConnectionFactory BHdbFactory = ServiceStackDBContext.BHDbFactory;
            using (var db = BHdbFactory.Open())
            {
                var q = db.From<view_reportbh>()
                        .Where(predicate)
                        .OrderBy(s => s.SYSPRIMARYKEY);
                var total = (int)db.Count(q);
                var result = db.Select<view_reportbh>(q).Skip(pos).Take(count).ToList();
                return new SearchResult<view_reportbh>(total, result);
            }

        }

        public ActionResult DetailQrCode(string qrinfo)
        {

            var DetailQrCodeUrl = ConfigurationManager.AppSettings["DetailQrCode"];
            var client = new RestClient(DetailQrCodeUrl);
            client.AddDefaultParameter("qrinfo", qrinfo);
            client.Proxy = null;
            var request = new RestRequest();
            IRestResponse<QrCodelResultsModel> results = client.Execute<QrCodelResultsModel>(request);
            var itemJson = JsonConvert.DeserializeObject<QrCodelItemJsonModel>(results.Data.reason.Iteminfo.ITEMJSON);
            var type = itemJson.GetType();
            System.Reflection.PropertyInfo[] CurProperties = type.GetProperties();
            List<SampleInfoNameAndKeyt> sampleInfo = new List<SampleInfoNameAndKeyt>();
            foreach (var item in results.Data.reason.columnsinfo)
            {
                var key = item.Columns;//如：shejidengji
                var name = item.Names;//如：设计等级
                for (int i = 0; i < CurProperties.Length; i++)
                {
                    if (CurProperties[i].Name == key)
                    {
                        string CurValue = CurProperties[i].GetValue(itemJson, null) == null ? "" : CurProperties[i].GetValue(itemJson, null).ToString();
                        SampleInfoNameAndKeyt oneSampleInfo = new SampleInfoNameAndKeyt()
                        {
                            Name = name,
                            Value = CurValue
                        };
                        sampleInfo.Add(oneSampleInfo);
                        break;
                    }
                }
            }
            results.Data.reason.sampleInfo = sampleInfo;

            foreach (var item in results.Data.reason.updatedata)
            {
                if (item.date.HasValue)
                {
                    item.DateStr = GetUIDtString(item.date);
                }
            }
            results.Data.reason.Iteminfo.entrustdate = GetUIDtString(results.Data.reason.Iteminfo.ENTRUSTDATE,"yyyy-MM-dd");
            results.Data.reason.Iteminfo.sldate = GetUIDtString(results.Data.reason.Iteminfo.SLDATE, "yyyy-MM-dd");
            results.Data.reason.Iteminfo.spndate = GetUIDtString(results.Data.reason.Iteminfo.SPNDATE, "yyyy-MM-dd");
            results.Data.reason.Iteminfo.remdate = GetUIDtString(results.Data.reason.Iteminfo.REMDATE, "yyyy-MM-dd");

            if (!results.Data.reason.Iteminfo.SLDATE.HasValue)//SLDATE没有值
            {
                results.Data.reason.Iteminfo.SLIMGPEOPLE = GetImageSrc(results.Data.reason.Iteminfo.SLIMGPEOPLE, new DateTime());
                results.Data.reason.Iteminfo.SLIMGQR = GetImageSrc(results.Data.reason.Iteminfo.SLIMGQR, new DateTime());
                results.Data.reason.Iteminfo.SLIMGPHONE = GetImageSrc(results.Data.reason.Iteminfo.SLIMGPHONE, new DateTime());
                results.Data.reason.Iteminfo.SPNIMGQR = GetImageSrc(results.Data.reason.Iteminfo.SPNIMGQR, new DateTime());
                results.Data.reason.Iteminfo.SPNIMGPEOPLE = GetImageSrc(results.Data.reason.Iteminfo.SPNIMGPEOPLE, new DateTime());
                results.Data.reason.Iteminfo.SPNIMGPHONE = GetImageSrc(results.Data.reason.Iteminfo.SPNIMGPHONE, new DateTime());
                results.Data.reason.Iteminfo.REMIMGPEOPLE = GetImageSrc(results.Data.reason.Iteminfo.REMIMGPEOPLE, new DateTime());
                results.Data.reason.Iteminfo.REMIMGPHONE = GetImageSrc(results.Data.reason.Iteminfo.REMIMGPHONE, new DateTime());

            }
            else//SLDATE有值
            {
                var dateStrs = results.Data.reason.Iteminfo.sldate;
                if (results.Data.reason.Iteminfo.SLDATE.HasValue)
                {
                    //var dateStr = dateStrs[0];
                    var date = DateTime.Parse(dateStrs);
                    results.Data.reason.Iteminfo.SLIMGPEOPLE = GetImageSrc(results.Data.reason.Iteminfo.SLIMGPEOPLE, date);
                    results.Data.reason.Iteminfo.SLIMGQR = GetImageSrc(results.Data.reason.Iteminfo.SLIMGQR, date);
                    results.Data.reason.Iteminfo.SLIMGPHONE = GetImageSrc(results.Data.reason.Iteminfo.SLIMGPHONE, date);
                    results.Data.reason.Iteminfo.SPNIMGQR = GetImageSrc(results.Data.reason.Iteminfo.SPNIMGQR, date);
                    results.Data.reason.Iteminfo.SPNIMGPEOPLE = GetImageSrc(results.Data.reason.Iteminfo.SPNIMGPEOPLE, date);
                    results.Data.reason.Iteminfo.SPNIMGPHONE = GetImageSrc(results.Data.reason.Iteminfo.SPNIMGPHONE, date);
                    results.Data.reason.Iteminfo.REMIMGPEOPLE = GetImageSrc(results.Data.reason.Iteminfo.REMIMGPEOPLE, date);
                    results.Data.reason.Iteminfo.REMIMGPHONE = GetImageSrc(results.Data.reason.Iteminfo.REMIMGPHONE, date);

                }
                else
                {
                    results.Data.reason.Iteminfo.SLIMGPEOPLE = GetImageSrc(results.Data.reason.Iteminfo.SLIMGPEOPLE, new DateTime());
                    results.Data.reason.Iteminfo.SLIMGQR = GetImageSrc(results.Data.reason.Iteminfo.SLIMGQR, new DateTime());
                    results.Data.reason.Iteminfo.SLIMGPHONE = GetImageSrc(results.Data.reason.Iteminfo.SLIMGPHONE, new DateTime());
                    results.Data.reason.Iteminfo.SPNIMGQR = GetImageSrc(results.Data.reason.Iteminfo.SPNIMGQR, new DateTime());
                    results.Data.reason.Iteminfo.SPNIMGPEOPLE = GetImageSrc(results.Data.reason.Iteminfo.SPNIMGPEOPLE, new DateTime());
                    results.Data.reason.Iteminfo.SPNIMGPHONE = GetImageSrc(results.Data.reason.Iteminfo.SPNIMGPHONE, new DateTime());
                    results.Data.reason.Iteminfo.REMIMGPEOPLE = GetImageSrc(results.Data.reason.Iteminfo.REMIMGPEOPLE, new DateTime());
                    results.Data.reason.Iteminfo.REMIMGPHONE = GetImageSrc(results.Data.reason.Iteminfo.REMIMGPHONE, new DateTime());
                }
            }
            return View(results.Data.reason);
        }

        public string GetImageSrc(string img, DateTime? slDate)
        {
            var bhDetailsImgUrl = ConfigurationManager.AppSettings["BHDetailsImg"];//BHGetImageSrc
            var bHGetImageSrcUrl1 = ConfigurationManager.AppSettings["BHGetImageSrc1"];
            var bHGetImageSrcUrl2 = ConfigurationManager.AppSettings["BHGetImageSrc2"];
            string endImageSrc = string.Empty;
            if (img.IsNullOrEmpty() && !slDate.HasValue)
            {
                return endImageSrc;
            }
            if(img.IsNullOrEmpty() && slDate.HasValue)
            {
                return endImageSrc;
            }
            if (slDate.Value < new DateTime(2019, 05, 01) && slDate.Value > new DateTime(2019, 03, 31) && img.Contains("/QR/"))
            {
                string strsrc = bHGetImageSrcUrl1 + img;
                string stronerror = "this.src='{0}".Fmt(bhDetailsImgUrl) + img + "';this.onerror=null";
                endImageSrc = strsrc + "|" + stronerror;
            }
            else
            {
                string strsrc = bHGetImageSrcUrl2 + img;
                string stronerror = "this.src='{0}".Fmt(bhDetailsImgUrl) + img + "';this.onerror=null";
                endImageSrc = strsrc + "|" + stronerror;
            }

            return endImageSrc;
        }

    }
}
