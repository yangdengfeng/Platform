using Nest;
using Pkpm.Contract.FileTransfer;
using Pkpm.Contract.PRKReport;
using Pkpm.Core.ItemNameCore;
using Pkpm.Core.UserRoleCore;
using Pkpm.Entity;
using Pkpm.Entity.ElasticSearch;
using Pkpm.Framework.Common;
using Pkpm.Framework.FileHandler;
using Pkpm.Framework.Logging;
using Pkpm.Framework.PkpmConfigService;
using Pkpm.Framework.Repsitory;
using QZWebService.ServiceModel;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.ReportCore
{
    public class ReportViewModel : es_t_bp_item
    {

        public ReportViewModel() { }
        public ReportViewModel(es_t_bp_item report)
        {
            XUtils.Assign(this, report);
        }

        public string CheckItemName { get; set; }
        public string CheckParamName { get; set; }

        public string ENTRUSTDATEStr { get; set; }
        public string CHECKDATEStr { get; set; }
        public string AUDITINGDATEStr { get; set; }
        public string APPROVEDATEStr { get; set; }
        public string PRINTDATEStr { get; set; }
        public string COLLATEDATEStr { get; set; }
        public string VERIFYDATEStr { get; set; }
        public string EXTENDDATEStr { get; set; }
        public string ACSTIMEStr { get; set; }

        public static void SetDateTimeToStr(ReportViewModel reportViewModel, es_t_bp_item report)
        {
            if (report == null)
            {
                return;
            }

            reportViewModel.ENTRUSTDATEStr = CommonUtils.GetDateTimeStr(report.ENTRUSTDATE);
            reportViewModel.CHECKDATEStr = CommonUtils.GetDateTimeStr(report.CHECKDATE);
            reportViewModel.AUDITINGDATEStr = CommonUtils.GetDateTimeStr(report.AUDITINGDATE);
            reportViewModel.APPROVEDATEStr = CommonUtils.GetDateTimeStr(report.APPROVEDATE);
            reportViewModel.PRINTDATEStr = CommonUtils.GetDateTimeStr(report.PRINTDATE);
            reportViewModel.COLLATEDATEStr = CommonUtils.GetDateTimeStr(report.COLLATEDATE);
            reportViewModel.VERIFYDATEStr = CommonUtils.GetDateTimeStr(report.VERIFYDATE);
            reportViewModel.EXTENDDATEStr = CommonUtils.GetDateTimeStr(report.EXTENDDATE);
            reportViewModel.ACSTIMEStr = CommonUtils.GetDateTimeStr(report.ACSTIME);

        }

        public static ReportViewModel BuildReportVieModelFromES(es_t_bp_item report)
        {
            var reportViewModel = report.ConvertTo<ReportViewModel>();

            SetDateTimeToStr(reportViewModel, report);

            return reportViewModel;
        }

    }

    public interface IReportService
    {
        ReportViewModel GetByReportNum(string reportNum);
        ReportViewModel GetByReportNum(ReportCheck reportCheck);
        ReportViewModel GetByReportNumAndDate(string reportNum, DateTime PrintDate);
        ReportViewModel GetByReportNumAndCustomId(string reportNum, string customId);
        List<ReportViewModel> GetUnqualifiedReports(int userId, string stationName = "", string projectName = "", string checkItemCode = "", int days = 1, int pageIndex = 0, int pageSize = 10);
        Dictionary<string, int> GetPkrReportNums(IEnumerable<es_t_bp_item> tbpItems);
        Func<QueryContainerDescriptor<es_t_pkpm_binaryReport>, QueryContainer> GetPkrFilter(string customId, string itemCode, string reportNum);
        string GetCryptPkrReportNumFromDict(Dictionary<string, int> prkReports, es_t_bp_item item);
        string GetClearReportNum(string cryptPkrReportNum);
        byte[] GetStoreFile(string filePath);
        byte[] GetPdfFromPkr(string pkrPath);
        byte[] GetImageFromPkr(string pkrPath);
        bool AddUploadItem(string customId, DateTime dt);
        string GetLargeFilePath(string sFilePath, string dFilePath);
        List<string> GetByReportNumLikeQuery(List<string> ExistsSysPrimaryKeys, string reportNum);
        string GetReportDataStatus(string SamplePhrase);

        /// <summary>
        /// 从起重设备检测方案中获取二维码信息 服务器 209
        /// </summary>
        /// <param name="entrNums"></param>
        /// <returns></returns>
        Dictionary<string, string> GetQzQrcodeBars(List<string> entrNums);

        string GetQzQrcodeBar(string entrNums);
    }

    public class ReportService : IReportService
    {

        IESRepsitory<es_t_bp_item> m_repReport;
        IUserService m_svcUser;
        IESRepsitory<es_t_bp_acs> m_repAcs;
        IESRepsitory<es_t_bp_wordreport> m_wordESRep;
        IESRepsitory<es_extReportMange> m_wordExtESRep;
        IESRepsitory<es_t_pkpm_binaryReport> pkrESRep;
        IRepsitory<AddUploadItem> uploadItemRep;
        IItemNameService m_svcItemName;
        ItemShortInfos m_oldItemNames = null;
        IPkpmConfigService PkpmConfigService;
        string GetSceneDataUrl;
        static Dictionary<string, string> shortItemCodes = new Dictionary<string, string>();

        static ReportService()
        {
            shortItemCodes.Add("R", "COVR");
        }

        public ReportService(IESRepsitory<es_t_bp_item> repReport, IUserService svcUser, IESRepsitory<es_t_bp_acs> repAcs,
            IFileHandler fileHandler, IESRepsitory<es_t_bp_wordreport> wordESRep, IRepsitory<AddUploadItem> uploadItemRep,
           IESRepsitory<es_extReportMange> wordExtESRep, IESRepsitory<es_t_pkpm_binaryReport> pkrESRep,
           IItemNameService svcItemName, IPkpmConfigService PkpmConfigService)
        {
            this.m_repReport = repReport;
            this.m_svcUser = svcUser;
            this.m_repAcs = repAcs;
            this.uploadItemRep = uploadItemRep;
            this.m_wordESRep = wordESRep;
            this.m_wordExtESRep = wordExtESRep;
            this.pkrESRep = pkrESRep;
            this.m_svcItemName = svcItemName;
            this.PkpmConfigService = PkpmConfigService;
            GetSceneDataUrl = PkpmConfigService.GetSceneDataUrl;

        }

        public ReportViewModel GetByReportNum(ReportCheck reportCheck)
        {
            Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> numQuery = qz =>
            {
                var initquery = qz.Term(qr => qr.Field(qrf => qrf.REPORTNUM).Value(reportCheck.reportnum));
                if (!reportCheck.customid.IsNullOrEmpty())
                {
                    initquery = initquery && qz.Term(tt => tt.Field(ttt => ttt.CUSTOMID).Value(reportCheck.customid));
                }
                return initquery;
            };

            ISearchResponse<es_t_bp_item> response = m_repReport.Search(s => s.From(0).Size(10).Query(numQuery).Sort(cs => cs.Descending(csd => csd.UPLOADTIME)));
            if (!response.IsValid || response.Documents == null || response.Documents.Count == 0)
            {
                return null;
            }

            es_t_bp_item chooseTBpItem = new es_t_bp_item();
            if (response.Documents.Count == 1)
            {
                chooseTBpItem = response.Documents.First();
            }
            else
            {
                bool hasFound = false;
                var existDocs = response.Documents.ToList();

                //查 工程名称
                if (!reportCheck.projectname.IsNullOrEmpty())
                {
                    existDocs = existDocs.Where(e => e.PROJECTNAME.Contains(reportCheck.projectname.Trim())).ToList();

                    if (existDocs.IsNullOrEmpty())
                    {
                        hasFound = true;
                        chooseTBpItem = response.Documents.First();
                    }
                    else if (existDocs.Count() == 1)
                    {
                        hasFound = true;
                        chooseTBpItem = existDocs.First();
                    }

                }

                //查 结构部位
                if (!hasFound && !reportCheck.structpart.IsNullOrEmpty())
                {
                    existDocs = existDocs.Where(e => e.STRUCTPART.Contains(reportCheck.structpart.Trim())).ToList();

                    if (existDocs.IsNullOrEmpty())
                    {
                        hasFound = true;
                        chooseTBpItem = response.Documents.First();
                    }
                    else if (existDocs.Count() == 1)
                    {
                        hasFound = true;
                        chooseTBpItem = existDocs.First();
                    }
                }

                if (!hasFound)
                {
                    chooseTBpItem = response.Documents.First();
                }

            }

            ReportViewModel rvm = ReportViewModel.BuildReportVieModelFromES(chooseTBpItem);
            FillInCheckItem(rvm);
            return rvm;
        }

        public ReportViewModel GetByReportNum(string reportNum)
        {
            Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> numQuery = qz =>
            {
                var initquery = qz.Term(qr => qr.Field(qrf => qrf.REPORTNUM).Value(reportNum));
                return initquery;
            };

            ISearchResponse<es_t_bp_item> response = m_repReport.Search(s => s.From(0).Size(10).Query(numQuery).Sort(cs => cs.Descending(csd => csd.UPLOADTIME)));
            if (response.Documents.Count == 0)
            {
                return null;
            }

            ReportViewModel rvm = ReportViewModel.BuildReportVieModelFromES(response.Documents.First());
            FillInCheckItem(rvm);
            return rvm;
        }

        public ReportViewModel GetByReportNumAndDate(string reportNum, DateTime PrintDate)
        {
            Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> numQuery = qz =>
            {
                string dtFormatStr = "yyyy-MM-dd'T'HH:mm:ss";

                var initquery = qz.Term(qr => qr.Field(qrf => qrf.REPORTNUM).Value(reportNum));
                initquery = initquery && qz.DateRange(d => d.Field(f => f.PRINTDATE).GreaterThanOrEquals(DateMath.FromString(PrintDate.ToString(dtFormatStr))));

                return initquery;
            };

            ISearchResponse<es_t_bp_item> response = m_repReport.Search(s => s.From(0).Size(10).Query(numQuery).Sort(cs => cs.Descending(csd => csd.UPDATETIME)));
            if (response.Documents.Count == 0)
            {
                return null;
            }

            ReportViewModel rvm = ReportViewModel.BuildReportVieModelFromES(response.Documents.First());
            FillInCheckItem(rvm);
            return rvm;
        }

        public ReportViewModel GetByReportNumAndCustomId(string reportNum, string customId)
        {
            Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> numQuery = qz =>
            {
                var initquery = qz.Term(qr => qr.Field(qrf => qrf.REPORTNUM).Value(reportNum));

                if (!customId.IsNullOrEmpty())
                {
                    initquery = initquery && qz.Term(tt => tt.Field(ttt => ttt.CUSTOMID).Value(customId));
                }

                return initquery;
            };

            ISearchResponse<es_t_bp_item> response = m_repReport.Search(s => s.From(0).Size(2).Query(numQuery).Sort(cs => cs.Descending(csd => csd.UPDATETIME)));
            if (response.Documents.Count == 0)
            {
                return null;
            }

            ReportViewModel rvm = ReportViewModel.BuildReportVieModelFromES(response.Documents.First());
            FillInCheckItem(rvm);
            return rvm;
        }

        public List<string> GetByReportNumLikeQuery(List<string> ExistsSysPrimaryKeys, string reportNum)
        {
            List<string> SysPrimaryKeys = new List<string>();
            Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> numQuery = q =>
            {
                var initquery = q.Terms(qr => qr.Field(qrf => qrf.SYSPRIMARYKEY).Terms(ExistsSysPrimaryKeys)) && (q.Term(qr => qr.Field(qrf => qrf.REPORTNUM).Value(reportNum)) || q.QueryString(qrm => qrm.DefaultField(qrmf => qrmf.REPORTNUM).Query("{0}{1}{0}".Fmt("*", reportNum))));

                return initquery;
            };

            ISearchResponse<es_t_bp_item> response = m_repReport.Search(s => s.From(0).Size(1000).Query(numQuery));
            if (response.Documents.Count > 0)
            {
                foreach (var item in response.Documents)
                {
                    SysPrimaryKeys.Add(item.SYSPRIMARYKEY);
                }
            }
            return SysPrimaryKeys;
        }

        private void FillInCheckItem(ReportViewModel rvm)
        {
            if (rvm == null)
            {
                return;
            }

            // 检测软件那边只要是GE开头的 全部获取SAMPLENAME
            if (!rvm.ITEMNAME.IsNullOrEmpty() && rvm.ITEMNAME.StartsWith("GE"))
            {
                rvm.CheckItemName = rvm.SAMPLENAME;
                rvm.CheckParamName = "(无)";
                return;
            }

            subitemparm item = null;

            //if (!string.IsNullOrEmpty(rvm.PARMCODE))
            //{
            //    item = m_svcItemName.GetItemByParamCode(rvm.PARMCODE);
            //}
            //else if (!string.IsNullOrEmpty(rvm.SUBITEMCODE))
            //{
            //    item = m_svcItemName.GetItemByItemCode(rvm.SUBITEMCODE);
            //}
            if (item != null)
            {
                rvm.CheckItemName = item.itemname;
                rvm.CheckParamName = item.parmname;
            }
            else
            {
                // 旧体系
                if (m_oldItemNames == null)
                    m_oldItemNames = m_svcItemName.GetAllItemName();
                var oldItemName = m_svcItemName.GetItemCNNameFromAll(m_oldItemNames, rvm.REPORTJXLB, rvm.ITEMNAME);
                rvm.CheckItemName = oldItemName;
                rvm.CheckParamName = "(无)";
            }
        }

        public List<ReportViewModel> GetUnqualifiedReports(int userId, string stationName = "", string projectName = "", string checkItemCode = "", int days = 1, int pageIndex = 0, int pageSize = 10)
        {
            string dtFormatStr = "yyyy-MM-dd'T'HH:mm:ss";
            string startDtStr = DateTime.Today.AddDays(-days).ToString(dtFormatStr);
            string endDtStr = DateTime.Today.AddDays(1).ToString(dtFormatStr);

            Func<QueryContainerDescriptor<es_t_bp_item>, QueryContainer> qcd = q =>
            {
                var qc = q.Term(t => t.Field(f => f.CONCLUSIONCODE).Value("N"))
                    && q.DateRange(qdr => qdr.Field(qdrf => qdrf.PRINTDATE).GreaterThanOrEquals(DateMath.FromString(startDtStr)))
                    && q.DateRange(qddr => qddr.Field(qddrf => qddrf.PRINTDATE).LessThanOrEquals(DateMath.FromString(endDtStr)))
                    && q.Match(t => t.Field(f => f.SAMPLEDISPOSEPHASE).Query(PkpmConfigService.PrintedSamplePhrase));

                var instFilter = m_svcUser.GetFilterInsts(userId, "");
                if (instFilter.NeedFilter && instFilter.FilterInstIds.Count() > 0)
                {
                    qc = qc && +q.Terms(qtm => qtm.Field(qtmf => qtmf.CUSTOMID).Terms(instFilter.FilterInstIds));
                }

                if (!string.IsNullOrEmpty(checkItemCode))
                {
                    qc = qc && +q.Term(qt => qt.Field(qtf => qtf.ITEMNAME).Value(checkItemCode));
                }

                if (!string.IsNullOrEmpty(stationName))
                    qc = qc && q.Match(t => t.Field(f => f.SUPERUNIT).Query(stationName));

                if (!string.IsNullOrEmpty(projectName))
                {
                    // && + 只过滤，不分词
                    qc = qc && +q.Term(qt => qt.Field(qtf => qtf.PROJECTNAME[0].Suffix("PROJECTNAMERAW")).Value(projectName));
                }

                return qc;
            };
            ISearchResponse<es_t_bp_item> response = m_repReport.Search(s => s.Sort(cs => cs.Descending(sd => sd.PRINTDATE)).From(pageIndex * pageSize).Size(pageSize).Query(qcd));

            List<ReportViewModel> result = new List<ReportViewModel>();
            foreach (var d in response.Documents)
            {
                var r = ReportViewModel.BuildReportVieModelFromES(d);
                FillInCheckItem(r);
                result.Add(r);
            }
            return result;
        }

        public Dictionary<string, int> GetPkrReportNums(IEnumerable<es_t_bp_item> tbpItems)
        {
            Dictionary<string, int> pkrReports = new Dictionary<string, int>();
            if (tbpItems == null || tbpItems.Count() == 0)
            {
                return pkrReports;
            }

            List<string> pkrReportNums = new List<string>();

            foreach (var item in tbpItems)
            {
                var esPkrReportNum = BuildPkrReportNum(item);

                pkrReportNums.Add(esPkrReportNum.BSKey);
                pkrReportNums.Add(esPkrReportNum.NormalKey);
            }

            int aggCount = pkrReportNums.Count * 2;
            string pkrAggkey = "pkrAggKey";

            Func<QueryContainerDescriptor<es_t_pkpm_binaryReport>, QueryContainer> filterQuery = q => (q.Terms(t => t.Field(f => f.REPORTNUM).Terms(pkrReportNums)))
                                                                                                    && q.Exists(qe => qe.Field(qet => qet.REPORTPATH));

            var pkrResponse = pkrESRep.Search(s => s.Size(0).Query(filterQuery)
                                          .Aggregations(af => af.Terms(pkrAggkey, item => item.Field(iif => iif.REPORTNUM).Size(aggCount))).Index("gx-t-pkpm-pkr"));

            foreach (var item in pkrResponse.Aggs.Terms(pkrAggkey).Buckets)
            {
                int count = item.DocCount.HasValue ? (int)item.DocCount.Value : 0;
                pkrReports[item.Key] = count;
            }

            return pkrReports;
        }

        public Func<QueryContainerDescriptor<es_t_pkpm_binaryReport>, QueryContainer> GetPkrFilter(string customId, string itemCode, string reportNum)
        {

            var esPKRReporNum = BuildPkrReportNum(customId, itemCode, reportNum);


            Func<QueryContainerDescriptor<es_t_pkpm_binaryReport>, QueryContainer> filterQuery = q => (q.Term(t => t.Field(f => f.REPORTNUM).Value(esPKRReporNum.NormalKey))
                                                                                                          || q.Term(t => t.Field(tf => tf.REPORTNUM).Value(esPKRReporNum.BSKey)));


            return filterQuery;

        }

        public string GetCryptPkrReportNumFromDict(Dictionary<string, int> prkReports, es_t_bp_item item)
        {
            if (prkReports == null || prkReports.Count == 0 || item == null)
            {
                return string.Empty;
            }

            var esPKRReporNum = BuildPkrReportNum(item);

            if (prkReports.ContainsKey(esPKRReporNum.NormalKey) && prkReports[esPKRReporNum.NormalKey] > 0)
            {
                return SymCryptoUtility.Encrypt(esPKRReporNum.NormalKey);
            }
            else if (prkReports.ContainsKey(esPKRReporNum.BSKey) && prkReports[esPKRReporNum.BSKey] > 0)
            {
                return SymCryptoUtility.Encrypt(esPKRReporNum.BSKey);
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetClearReportNum(string cryptPkrReportNum)
        {
            var pkrReportNum = SymCryptoUtility.Decrypt(cryptPkrReportNum);
            return pkrReportNum;
        }

        public byte[] GetStoreFile(string filePath)
        {
            byte[] data = null;
            if (PkpmConfigService.IsWcfEnabled.IsNullOrEmpty() ? true : PkpmConfigService.IsWcfEnabled == "1")
            {

                //var rootPath =   System.IO.Path.GetPathRoot(filePath);
                //  //var patth = filePath - rootPath;
                //  //var startPath = filePath.Substring(0, 3);
                //  //filePath = PathsDict[startPath] + filePath.Substring(3);
                //  System.IO.Path.Combine(PathsDict[rootPath], filePath.Substring(3));
                //  using (var proxy = new WcfProxy<ICloudFileTransferService>())
                //  {
                //      data = proxy.Service.GetCloudStoreFile(filePath);
                //  }
                using (var proxy = new WcfProxy<IFileTransferService>())
                {
                    data = proxy.Service.GetStoreFile(filePath);
                }

            }
            else
            {
                data = System.IO.File.ReadAllBytes(filePath);
            }

            return data;
        }

        public string GetLargeFilePath(string sFilePath, string dFilePath)
        {
            if (PkpmConfigService.IsWcfEnabled.IsNullOrEmpty() ? true : PkpmConfigService.IsWcfEnabled == "1")
            {
                using (var proxy = new WcfProxy<IFileTransferService>())
                {
                    using (FileStream fs = new FileStream(dFilePath, FileMode.Create))
                    {
                        proxy.Service.GetPKRReport(sFilePath).CopyTo(fs);
                    }

                    return dFilePath;
                }
            }
            else
            {
                return sFilePath;
            }

        }

        public byte[] GetPdfFromPkr(string pkrPath)
        {
            using (var proxy = new WcfProxy<IPKRReport>())
            {
                return proxy.Service.BuildPdfFromPKR(pkrPath);
            }
        }

        public byte[] GetImageFromPkr(string pkrPath)
        {
            using (var proxy = new WcfProxy<IPKRReport>())
            {
                return proxy.Service.BuildImageFromPKR(pkrPath);
            }
        }

        public bool AddUploadItem(string customId, DateTime dt)
        {
            var uploadId = uploadItemRep.Insert(new Entity.AddUploadItem() { CustomId = customId, AddDt = dt });

            return uploadId > 0;
        }

        public string GetReportDataStatus(string SamplePhrase)
        {
            if (string.IsNullOrWhiteSpace(SamplePhrase))
            {
                return string.Empty;
            }

            if (SamplePhrase.Substring(10, 1) == "1")
            {
                return "无退费作废";
            }
            if (SamplePhrase.Substring(10, 1) == "2")
            {
                return "有退费作废";
            }
            if (SamplePhrase.Substring(14, 1) == "1")
            {
                return "授权修改";
            }
            if (SamplePhrase.Substring(10, 1) == "1")
            {
                return "有异常";
            }
            if (SamplePhrase.Substring(32, 1) != "0" && SamplePhrase.Substring(32, 1).IsInt())
            {
                return "！自动采集锁定";
            }
            if (SamplePhrase.Substring(35, 1) == "1")
            {
                return "已自动采集";
            }
            if (SamplePhrase.Substring(19, 1) == "1")
            {
                return "审批回退";
            }
            if (SamplePhrase.Substring(16, 1) != "0")
            {
                return "已打印原始记录";
            }
            if (SamplePhrase.Substring(13, 1) == "1")
            {
                return "调档处理";
            }

            if (SamplePhrase.Substring(11, 1) == "1")
            {
                return "安定性已试验";
            }
            if (SamplePhrase.Substring(8, 1) == "1")
            {
                return "已归档";
            }
            if (SamplePhrase.Substring(7, 1) == "1")
            {
                return "已发放";
            }
            if (SamplePhrase.Substring(6, 1) != "0")
            {
                return "已打印";
            }
            if (SamplePhrase.Substring(5, 1) == "1")
            {
                return "已批准";
            }
            if (SamplePhrase.Substring(4, 1) == "1")
            {
                return "已审核";
            }
            if (SamplePhrase.Substring(3, 1) == "1")
            {
                return "已校核";
            }
            if (SamplePhrase.Substring(2, 1) == "1")
            {
                return "已检测";
            }
            if (SamplePhrase.Substring(1, 1) == "1")
            {
                return "已复核";
            }
            if (SamplePhrase.Substring(0, 1) == "1")
            {
                return "已收样";
            }

            return string.Empty;
        }

        private ESPkrReportNum BuildPkrReportNum(string customId, string itemCode, string reportNum)
        {
            var itemname = GetTrueItemCode(itemCode);

            return new ESPkrReportNum
            {
                NormalKey = "{0}{1}{2}".Fmt(customId, itemname, reportNum),
                BSKey = "{0}{0}{1}{2}".Fmt(customId, itemname, reportNum)
            };
        }

        private ESPkrReportNum BuildPkrReportNum(es_t_bp_item item)
        {
            var itemCode = GetTrueItemCode(item.ITEMNAME);

            return new ESPkrReportNum()
            {
                NormalKey = "{0}{1}{2}".Fmt(item.CUSTOMID, itemCode, item.REPORTNUM),
                BSKey = "{0}{0}{1}{2}".Fmt(item.CUSTOMID, itemCode, item.REPORTNUM)
            };
        }

        private string GetTrueItemCode(string itemCode)
        {
            if (itemCode.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var itemname = itemCode;
            if (shortItemCodes.ContainsKey(itemCode))
            {
                itemname = shortItemCodes[itemCode];
            }

            return itemname;
        }

        /// <summary>
        /// 从起重数据库中获取二维码信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetQzQrcodeBars(List<string> entrNums)
        {
            if (entrNums.IsNullOrEmpty())
            {
                return new Dictionary<string, string>();
            }


            var client = new JsonServiceClient(GetSceneDataUrl);

            QzQrcodeBars request = new QzQrcodeBars();
            request.entrNums = entrNums;
            var responses = client.Get(request);
            if (responses.IsSucc)//存在没有二维码的数据信息，去209查
            {
                return responses.Datas;
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// 通过委托编号从起重数据库中获取二维码信息
        /// </summary>
        /// <param name="entrNums"></param>
        /// <returns></returns>
        public string GetQzQrcodeBar(string entrNums)
        {
            if (entrNums.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var client = new ServiceStack.JsonServiceClient(GetSceneDataUrl);

            QzQrcodeBar request = new QzQrcodeBar();
            request.entrNum = entrNums;
            var responses = client.Get(request);
            if (responses.IsSucc)
            {
                return responses.qrinfo;
            }
            else
            {
                return string.Empty;
            }

        }

    }

    internal class ESPkrReportNum
    {
        public string NormalKey { get; set; }

        public string BSKey { get; set; }

    }

}
