using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using PkpmGX.Models;
using Pkpm.Framework.Repsitory;
using Dhtmlx.Model.Grid;
using ServiceStack;
using ServiceStack.OrmLite;
using Pkpm.Framework.Common;
using Pkpm.Core.SysDictCore;
using Pkpm.Core.CheckUnitCore;
using Dhtmlx.Model.Toolbar;
using Dhtmlx.Model.Form;
using Pkpm.Entity.DTO;
using Pkpm.Framework.FileHandler;
using System.Text.RegularExpressions;
using System.IO;
using Pkpm.Core.STCustomCore;
using Pkpm.Core.AreaCore;
using Nest;
using System.Configuration;
using Pkpm.Entity.ST;
using Pkpm.Core.ReportCore;
using Pkpm.Core.QrCodeCore;
using System.Xml.Linq;
using Pkpm.Core.ItemNameCore;
using Pkpm.Core.SHItemNameCore;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class TBpItemCheckController : PkpmController
    {
        IESRepsitory<es_ut_rpm> utrpmRep;
        IESRepsitory<es_rmcr> rmcrRep;
        IESRepsitory<es_t_bp_item> rep;
        IESRepsitory<es_t_bp_acs> acsESRep;
        IESRepsitory<es_t_pkpm_binaryReport> pkrESRep;
        IESRepsitory<es_t_bp_modify_log> modifylogESRep;
        IESRepsitory<es_t_bp_question> questionESRep;
        ISysDictService sysDictService;
        IReportQrCode reportQrCode;
        ISHItemNameService sHItemNameService;
        IReportService reportService;
        CheckUnitService checkUnitService;

        public TBpItemCheckController(IESRepsitory<es_t_bp_item> rep, IESRepsitory<es_t_bp_acs> acsESRep,
            IESRepsitory<es_t_pkpm_binaryReport> pkrESRep, IESRepsitory<es_t_bp_modify_log> modifylogESRep,
            IESRepsitory<es_t_bp_question> questionESRep,
            ISHItemNameService sHItemNameService,
        ISysDictService sysDictService, CheckUnitService checkUnitService,
             IReportQrCode reportQrCode, IReportService reportService, IESRepsitory<es_rmcr> rmcrRep, IESRepsitory<es_ut_rpm> utrpmRep,
            IUserService userService) : base(userService)
        {
            this.rep = rep;
            this.acsESRep = acsESRep;
            this.pkrESRep = pkrESRep;
            this.sHItemNameService = sHItemNameService;
            this.modifylogESRep = modifylogESRep;
            this.questionESRep = questionESRep;
            this.sysDictService = sysDictService;
            this.reportQrCode = reportQrCode;
            this.reportService = reportService;
            this.checkUnitService = checkUnitService;
            this.rmcrRep = rmcrRep;
            this.utrpmRep = utrpmRep;
        }
        public ActionResult STInstCombo()
        {
            XElement element = BuildSTInstCombo();
            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }
        protected XElement BuildSTInstCombo()
        {
            var insts = checkUnitService.GetAllSTCheckUnit();

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
                 from kv in insts
                 select new XElement("option",
                            new XAttribute("value", kv.Key),
                            new XElement("text",
                                new XElement("name", kv.Value.IsNullOrEmpty() ? "无机构名称" : kv.Value))));

            return element;
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
        // GET: TBpItemCheck
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(STSysSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            ISearchResponse<es_t_bp_item> response = GetSearchResult(model);

            int totalCount = (int)response.Total;
            DhtmlxGrid grid = new DhtmlxGrid();
            grid.AddPaging(totalCount, pos);
            int index = pos;
            var allCheckItems = sHItemNameService.GetAllSHItemName();
            var reportConclusions = sysDictService.GetDictsByKey("ReportConclusionCode");

            foreach (var item in response.Documents)
            {
                DhtmlxGridRow row = new DhtmlxGridRow(item.SYSPRIMARYKEY);
                row.AddCell(index + 1);
                row.AddCell(item.PROJECTNAME);
                row.AddCell(SysDictUtility.GetKeyFromDic(reportConclusions, item.CONCLUSIONCODE, "/"));
                row.AddCell(checkUnitService.GetSTCheckUnitById( item.CUSTOMID));
                row.AddCell(item.STRUCTPART);
                row.AddCell(sHItemNameService.GetItemSHNameFromAll(allCheckItems,item.ITEMNAME));
              
                row.AddCell(item.REPORTNUM);
                row.AddCell(item.QRCODEBAR);
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

        protected Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> GetFilterQuerySH(
            STSysSearchModel model)
        {
         
            Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> filterQuery = q =>
            {
                string dtFormatStr = "yyyy-MM-dd'T'HH:mm:ss";
                string startDtStr = model.StartDt.HasValue ? model.StartDt.Value.ToString(dtFormatStr) : string.Empty;
                string endDtStr = model.EndDt.HasValue ? model.EndDt.Value.AddDays(1).ToString(dtFormatStr) : string.Empty;
                QueryContainer initQuery = null;
                var date = DateTime.Today;
                if (model.StartDt.HasValue || model.EndDt.HasValue)
                {

                    if (model.DtType == "CheckDt")
                    {
                        if (!startDtStr.IsNullOrEmpty())
                        {
                            initQuery = initQuery && +q.DateRange(d => d.Field(f => f.CHECKDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                        }
                        if (!endDtStr.IsNullOrEmpty())
                        {
                            initQuery = initQuery && +q.DateRange(d => d.Field(f => f.CHECKDATE).LessThan(DateMath.FromString(endDtStr)));
                        }
                    }
                    else if (model.DtType == "ReportDt")
                    {
                        if (!startDtStr.IsNullOrEmpty())
                        {
                            initQuery = initQuery && +q.DateRange(d => d.Field(f => f.PRINTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                        }
                        if (!endDtStr.IsNullOrEmpty())
                        {
                            initQuery = initQuery && +q.DateRange(d => d.Field(f => f.PRINTDATE).LessThan(DateMath.FromString(endDtStr)));
                        }
                    }
                    else if (model.DtType == "UploadDt")
                    {
                        if (!startDtStr.IsNullOrEmpty())
                        {
                            initQuery = initQuery && +q.DateRange(d => d.Field(f => f.UPLOADTIME).GreaterThanOrEquals(DateMath.FromString(startDtStr)));
                        }
                        if (!endDtStr.IsNullOrEmpty())
                        {
                            initQuery = initQuery && +q.DateRange(d => d.Field(f => f.UPLOADTIME).LessThan(DateMath.FromString(endDtStr)));
                        }
                    }
                }
               

                ////页面显示已打印的报告
                //if ("1" == System.Configuration.ConfigurationManager.AppSettings["ShowPrinted"])
                //{
                //    initQuery = initQuery && q.Match(m => m.Field(f => f.SAMPLEDISPOSEPHASE).Query(printedPhrase));
                //}


                var instFilter = GetCurrentInstFilter();
                if (instFilter.NeedFilter && instFilter.FilterInstIds.Count() > 0)
                {
                    initQuery = initQuery && +q.Terms(qtm => qtm.Field(qtmf => qtmf.CUSTOMID).Terms(instFilter.FilterInstIds));
                }

                if (!string.IsNullOrWhiteSpace(model.ProjectName))
                {
                    //initQuery = initQuery && q.QueryString(m => m.DefaultField(f => f.PROJECTNAME[0].Suffix("PROJECTNAMERAW")).Query("{0}{1}{0}".Fmt("*", model.ProjectName)));
                    initQuery = initQuery && q.Wildcard(qw => qw.Field(qwf => qwf.PROJECTNAME).Value("{0}{1}{0}".Fmt("*", model.ProjectName)));
                }

                if (!string.IsNullOrWhiteSpace(model.EntrustUnit))
                {
                    initQuery = initQuery && q.Match(m => m.Field(f => f.ENTRUSTUNIT).Query(model.EntrustUnit));
                }
                if (!string.IsNullOrWhiteSpace(model.EntrustUnit))
                {
                    initQuery = initQuery && q.Match(m => m.Field(f => f.ENTRUSTUNIT).Query(model.EntrustUnit));
                }
                if (!string.IsNullOrWhiteSpace(model.ReportNum))
                {
                    initQuery = initQuery && q.Match(m => m.Field(f => f.REPORTNUM).Query(model.ReportNum));
                }
                if (!string.IsNullOrWhiteSpace(model.EntrustNum))
                {
                    initQuery = initQuery && q.Match(m => m.Field(f => f.ENTRUSTNUM).Query(model.EntrustNum));
                }


                if (!string.IsNullOrWhiteSpace(model.CheckItem))//.CheckItemCodes))
                {
                    var allItemcodes = model.CheckItem.Split(',');//.CheckItemCodes.Split(',');
                    if (allItemcodes.Length > 0)
                    {
                        initQuery = initQuery && q.Terms(qt => qt.Field(qtf => qtf.ITEMNAME).Terms(allItemcodes));
                    }
                }

                if (model.IsChanged.HasValue && model.IsChanged.Value != -1)
                {
                    if (model.IsChanged.Value == 1)
                    {
                        initQuery = initQuery && +q.Range(r => r.Field(f => f.HAVELOG).GreaterThanOrEquals(1));
                    }
                    else if (model.IsChanged.Value == 0)
                    {
                        initQuery = initQuery && (!q.Exists(e => e.Field(f => f.HAVELOG)) || q.Term(t => t.Field(f => f.HAVELOG).Value(0)));
                    }
                }

                if (model.HasArc.HasValue && model.HasArc.Value != -1)
                {
                    if (model.HasArc.Value == 1)
                    {
                        initQuery = initQuery && +q.Range(r => r.Field(f => f.HAVEACS).GreaterThanOrEquals(1)) && q.Exists(r => r.Field(f => f.ACSTIME));
                    }
                    else if (model.HasArc.Value == 0)
                    {
                        initQuery = initQuery && (!q.Exists(e => e.Field(f => f.HAVEACS)) || q.Term(t => t.Field(f => f.HAVEACS).Value(0)));
                    }
                }

                if (!string.IsNullOrWhiteSpace(model.CheckStatus) && model.CheckStatus != "A")
                {
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.CONCLUSIONCODE).Value(model.CheckStatus));
                }
                if (!string.IsNullOrEmpty(model.SampleNum))
                {

                    initQuery = initQuery && q.QueryString(qrm => qrm.DefaultField(qrmf => qrmf.SAMPLENUM).Query("{0}{1}{0}".Fmt("*", model.SampleNum)));
                    //+ q.Term(t => t.Field(f => f.SAMPLENUM).Value(model.SampleNum));
                }

              
                if (!model.ItemName.IsNullOrEmpty())
                {
                    initQuery = initQuery && +q.Term(t => t.Field(f => f.ITEMNAME).Value(model.ItemName));
                }
                return initQuery;
            };



            return filterQuery;
        }

        private ISearchResponse<es_t_bp_item> GetSearchResult(STSysSearchModel model)
        {
            var filterQuery = GetFilterQuerySH(model);

            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;

            
            var aaaa= rep.Search(s => s.Source(sf => sf.Includes(sfi => sfi.Fields(
                           f => f.SYSPRIMARYKEY,
                           f => f.CUSTOMID,
                           f => f.PROJECTNAME,
                           f => f.STRUCTPART,
                           f => f.ITEMNAME,
                            f => f.ITEMCHNAME,
                           f => f.ACSTIME,
                           f => f.ENTRUSTDATE,
                           f => f.CHECKDATE,
                           f => f.PRINTDATE,
                           f => f.SAMPLENUM,
                           f => f.REPORTNUM,
                            f => f.QRCODEBAR,
                           //f => f.HAVREPORT,
                           f => f.ISCREPORT,
                           f => f.SUBITEMCODE,
                           f => f.SAMPLEDISPOSEPHASE,
                           f => f.SAMPLEDISPOSEPHASEORIGIN,
                           f => f.CHECKCONCLUSION))).Sort(c => c.Descending(cd => cd.PRINTDATE)).From(pos).Size(count).Query(filterQuery).Index("sh-tbpitem"));

            return aaaa;
        }


        public ActionResult Details(string id)
        {
            STReportDetailViewModel model = new Models.STReportDetailViewModel()
            {
                acsTimeModel = new List<TotalSearchAcsTimeModel>(),
                commonReprtModel = new List<TotalSearchCommonReportModel>(),
                modifyModel = new List<TotalSearchModifyModel>(),
                RawMaterialModel = new List<Models.TotalSearchRawMaterialModel>()
            };
            var reportConclusion = sysDictService.GetDictsByKey("ReportConclusionCode");
            var getResponse = rep.Get(new DocumentPath<es_t_bp_item>(id).Index("sh-tbpitem"));
            if (getResponse.IsValid && getResponse.Source != null)
            {

                model.MainItem = getResponse.Source;
                var CustomId = model.MainItem.CUSTOMID;
                var SysPrimaryKey = model.MainItem.SYSPRIMARYKEY;
                var ItemName = model.MainItem.ITEMNAME;
                var ReportNum = model.MainItem.REPORTNUM;

                model.CutomName = checkUnitService.GetSTCheckUnitById(CustomId);
                model.MainItem.CONCLUSIONCODE = SysDictUtility.GetKeyFromDic(reportConclusion, model.MainItem.CONCLUSIONCODE, "/");

                es_t_bp_item item = getResponse.Source;

                if (model.MainItem.HAVEACS.HasValue && model.MainItem.HAVEACS.Value > 0)
                {
                    model.acsTimeModel = GetAcsTimeDetails(SysPrimaryKey, model.MainItem.SAMPLENUM);
                }

                model.commonReprtModel = GetCommonReportDetails(CustomId, ReportNum, ItemName, SysPrimaryKey);

                if (model.MainItem.HAVELOG.HasValue && model.MainItem.HAVELOG.Value > 0)
                {
                    model.modifyModel = GetModifyDetails(SysPrimaryKey);
                }
                model.RawMaterialModel = GetRawMaterials(SysPrimaryKey, ReportNum, ItemName, CustomId);
            }
            if (model.MainItem == null)
            {
                return Redirect("/TBpItemCheck/NoReport");
            }
            return View(model);
        }

        public ActionResult NoReport()
        {
            return View();
        }

        private List<TotalSearchRawMaterialModel> GetRawMaterials(string id, string ReportNum, string ItemName, string CustomId)
        {
            List<TotalSearchRawMaterialModel> models = new List<TotalSearchRawMaterialModel>();
            var rmcmRes = rmcrRep.Search(s => s.Query(q => q.Term(qt => qt.Field(qtf => qtf.SYSPRIMARYKEY).Value(id)) && q.Exists(qe => qe.Field(qef => qef.JYPC)) && q.Term(qt => qt.Field(qtf => qtf.REPORTNUM).Value(ReportNum))).Source(ss => ss.Includes(si => si.Fields(sif => sif.JYPC, sif => sif.SAMPLENUM))).Size(10).Index("sh-rmcr"));
            if (rmcmRes.IsValid && rmcmRes.Documents.Count > 0)
            {
                var Jypcs = rmcmRes.Documents.Select(s => s.JYPC).ToList();
                if (Jypcs.Count > 0)
                {
                    int index = 0;
                    Func<QueryContainerDescriptor<es_ut_rpm>, QueryContainer> filterQuery = q => {
                        QueryContainer initQuery = q.Exists(e => e.Field(f => f.JYPC));
                        initQuery = initQuery && q.Terms(qt => qt.Field(qtf => qtf.JYPC).Terms(Jypcs));
                        if (!ItemName.IsNullOrEmpty())
                        {
                            initQuery = initQuery && q.Term(qt => qt.Field(qtf => qtf.ITEMNAME).Value(ItemName));
                        }
                        var SampleNums = rmcmRes.Documents.Select(s => s.SAMPLENUM).ToList();
                        if (SampleNums.Count > 0)
                        {
                            initQuery = initQuery && q.Terms(qt => qt.Field(qtf => qtf.SAMPLENUM).Terms(SampleNums));
                        }
                        if (!CustomId.IsNullOrEmpty())
                        {
                            initQuery = initQuery && q.Term(qt => qt.Field(qtf => qtf.CUSTOMID).Value(CustomId));
                        }
                        return initQuery;
                    };
                    var response = utrpmRep.Search(s => s.Query(filterQuery).Size(100).Index("sh-ut-rpm"));
                    if (response.IsValid && response.Documents.Count > 0)
                    {
                        foreach (var item in response.Documents)
                        {
                            TotalSearchRawMaterialModel rm = new TotalSearchRawMaterialModel();
                            rm.index = (++index).ToString();
                            rm.Name = GetRawMaterialItemName(ItemName);
                            rm.PRODUCEFACTORY = item.PRODUCEFACTORY;
                            rm.ENTRYAMOUNT = item.ENTRYAMOUNT.HasValue ? Math.Round(item.ENTRYAMOUNT.Value, 2).ToString() : "";
                            rm.ENTRYDATE = GetUIDtString(item.ENTRYDATE);
                            rm.JYPC = item.JYPC;
                            models.Add(rm);
                        }
                    }
                }

            }
            return models;
        }

        private string GetRawMaterialItemName(string ItemName)
        {
            string RawMaterialName = string.Empty;
            switch (ItemName)
            {
                case "CEMT":
                    RawMaterialName = "水泥";
                    break;
                case "HPZJ":
                    RawMaterialName = "膨胀剂";
                    break;
                case "SIIH":
                    RawMaterialName = "石子";
                    break;
                case "SZIH":
                    RawMaterialName = "砂子";
                    break;
                case "HJSJ":
                    RawMaterialName = "外加剂";
                    break;
                case "CFMH":
                    RawMaterialName = "粉煤灰";
                    break;
                case "GLKZ":
                    RawMaterialName = "矿渣粉";
                    break;
                case "JBSH":
                    RawMaterialName = "石灰石粉";
                    break;
            }
            return RawMaterialName;
        }

        private List<TotalSearchAcsTimeModel> GetAcsTimeDetails(string id, string SampleNum)
        {
            ISearchResponse<es_t_bp_acs> acsResponse = null;
            if (string.IsNullOrEmpty(SampleNum))
            {
                acsResponse = acsESRep.Search(s => s.Index("sh-t-bp-attach").From(0).Size(20).Sort(cs => cs.Ascending(csd => csd.ACSTIME)).Query(q => q.
                                                          Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(id))
                                                          ))));
            }
            else
            {
                acsResponse = acsESRep.Search(s => s.Index("sh-t-bp-attach").From(0).Size(20).Sort(cs => cs.Ascending(csd => csd.ACSTIME)).Query(q => q.Term(f => f.Field(ff => ff.SYSPRIMARYKEY).Value(id)) && q.Term(f => f.Field(ff => ff.SAMPLENUM).Value(SampleNum))));
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

        private List<TotalSearchCommonReportModel> GetCommonReportDetails(string customId, string reportNum, string itemCode, string id)
        {
            string key = string.Format("{0}{1}{2}", customId, itemCode, reportNum);

            Func<QueryContainerDescriptor<es_t_pkpm_binaryReport>, QueryContainer> filterQuery = q => (q.Terms(t => t.Field(f => f.REPORTNUM).Terms(key)))
                                                                                                        && q.Exists(qe => qe.Field(qet => qet.REPORTPATH));

            var pkrResponse = pkrESRep.Search(s => s.Index("sh-t-pkpm-pkr").Sort(cs => cs.Descending(csd => csd.UPLOADTIME)).From(0).Size(50).Query(filterQuery));
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

        private List<TotalSearchModifyModel> GetModifyDetails(string id)
        {
            var modifyResponse = modifylogESRep.Search(s => s.Index("sh-t-bp-subitem").From(0).Size(50).Query(q => q.
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

        public ActionResult BuildBarCode(string id, string qrinfo)
        {

            byte[] QrCodeImgData = reportQrCode.Encode(id, qrinfo); // QrCodeGenerator.Generate(pixelsPerModule, id);
            string imageFileName = "barcode.png";
            string contentType = MimeMapping.GetMimeMapping(imageFileName);
            return new FileContentResult(QrCodeImgData, contentType);
        }


        public ActionResult ModifyRecordDetails(string key)
        {
            ModifyRecordDetailsModel viewModel = new ModifyRecordDetailsModel()
            {
                ModifyDetailsModel = new List<Models.TotalSearchQuestionModel>()
            };
            var questionResponse = questionESRep.Search(s => s.Index("sh-t-bp-subitem").From(0).Size(50).Query(q => q.
                                                   Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.QUESTIONPRIMARYKEY).Value(key))))));

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
                    oneModel.RecordingPhase = RenderSampleDisposePhase(item.RECORDINGPHASE);
                    oneModel.QuestionType = SysDictUtility.GetKeyFromDic(questionTypes, item.QUESTIONTYPES);
                    oneModel.Context = item.CONTEXT;
                    oneModel.RecordMan = item.RECORDMAN;
                    model.Add(oneModel);
                    index++;
                }

            }
            viewModel.ModifyDetailsModel = model;
            return View(viewModel);
        }

        public ActionResult CommonReportByPath(string path)
        {
            string reportPath = SymCryptoUtility.Decrypt(path);
            var imageData = reportService.GetPdfFromPkr(reportPath);
            var imageFileName = "报告文件{0}-{1}.pdf".Fmt(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.Ticks);
            string contentType = MimeMapping.GetMimeMapping(imageFileName);
            return new FileContentResult(imageData, contentType);
        }

        public ActionResult CommonReport(string id)
        {
            var pkrResponse = pkrESRep.Search(s => s.Index("sh-t-pkpm-pkr").From(0).Size(50).Query(q => q.
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


        // GET: TBpItemCheck/Create
        public ActionResult Create()
        {
            return View();
        }
        public string RenderSampleDisposePhase(string recordPhase)
        {
            if (string.IsNullOrWhiteSpace(recordPhase))
            {
                return string.Empty;
            }

            if (recordPhase.Substring(10, 1) == "1")
            {
                return "无退费作废";
            }
            if (recordPhase.Substring(10, 1) == "2")
            {
                return "有退费作废";
            }
            if (recordPhase.Substring(14, 1) == "1")
            {
                return "授权修改";
            }
            if (recordPhase.Substring(10, 1) == "1")
            {
                return "有异常";
            }
            if (recordPhase.Substring(32, 1) != "0" && recordPhase.Substring(32, 1).IsInt())
            {
                return "！自动采集锁定";
            }
            if (recordPhase.Substring(35, 1) == "1")
            {
                return "已自动采集";
            }
            if (recordPhase.Substring(19, 1) == "1")
            {
                return "审批回退";
            }
            if (recordPhase.Substring(16, 1) != "0")
            {
                return "已打印原始记录";
            }
            if (recordPhase.Substring(13, 1) == "1")
            {
                return "调档处理";
            }

            if (recordPhase.Substring(11, 1) == "1")
            {
                return "安定性已试验";
            }
            if (recordPhase.Substring(8, 1) == "1")
            {
                return "已归档";
            }
            if (recordPhase.Substring(7, 1) == "1")
            {
                return "已发放";
            }
            if (recordPhase.Substring(6, 1) != "0")
            {
                return "已打印";
            }
            if (recordPhase.Substring(5, 1) == "1")
            {
                return "已批准";
            }
            if (recordPhase.Substring(4, 1) == "1")
            {
                return "已审核";
            }
            if (recordPhase.Substring(3, 1) == "1")
            {
                return "已校核";
            }
            if (recordPhase.Substring(2, 1) == "1")
            {
                return "已检测";
            }
            if (recordPhase.Substring(1, 1) == "1")
            {
                return "已复核";
            }
            if (recordPhase.Substring(0, 1) == "1")
            {
                return "已收样";
            }

            return string.Empty;
        }

    }
}
