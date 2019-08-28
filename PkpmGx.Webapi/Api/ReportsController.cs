using Nest;
using Pkpm.Contract.FileTransfer;
using Pkpm.Core;
using Pkpm.Core.ItemNameCore;
using Pkpm.Core.ReportCore;
using Pkpm.Core.UserRoleCore;
using Pkpm.Entity;
using Pkpm.Entity.DTO;
using Pkpm.Entity.ElasticSearch;
using Pkpm.Framework.Common;
using Pkpm.Framework.FileHandler;
using Pkpm.Framework.Repsitory;
using QZWebService.ServiceModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;

namespace PkpmGx.Webapi.Api
{
    /// <summary>
    /// 检测报告控制器
    /// </summary>
    [RoutePrefix("api/Reports")]
    public class ReportsController : ApiController
    {
        static readonly string PrintedSamplePhrase = WebConfigurationManager.AppSettings["PrintedSamplePhrase"];
        static readonly List<string> IMAGE_DATA_TYPES = new List<string> { "J", "S", "H" };

        IESRepsitory<es_t_bp_item> m_repReport;
        IUserService m_svcUser;
        IESRepsitory<es_t_bp_acs> m_repAcs;
        IAcsChartService m_svcAcsChart;
        IFileHandler m_fileHandler;
        IESRepsitory<es_t_bp_wordreport> m_wordESRep;
        IESRepsitory<es_extReportMange> m_wordExtESRep;
        IESRepsitory<es_t_pkpm_binaryReport> pkrESRep;
        IReportService m_svcReport;
        IItemNameService itemNameService;
        IESRepsitory<es_t_bp_modify_log> m_repLog;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repReport"></param>
        /// <param name="svcUser"></param>
        /// <param name="repAcs"></param>
        /// <param name="svcAcsChart"></param>
        /// <param name="fileHandler"></param>
        /// <param name="wordESRep"></param>
        /// <param name="wordExtESRep"></param>
        /// <param name="pkrESRep"></param>
        /// <param name="svcReport"></param>
        /// <param name="itemNameService"></param>
        public ReportsController(IESRepsitory<es_t_bp_item> repReport, IUserService svcUser, IESRepsitory<es_t_bp_acs> repAcs,
            IAcsChartService svcAcsChart, IFileHandler fileHandler, IESRepsitory<es_t_bp_wordreport> wordESRep,
            IESRepsitory<es_extReportMange> wordExtESRep, IESRepsitory<es_t_pkpm_binaryReport> pkrESRep, IReportService svcReport, IESRepsitory<es_t_bp_modify_log> m_repLog,
            IItemNameService itemNameService)
        {
            m_repReport = repReport;
            m_svcUser = svcUser;
            m_repAcs = repAcs;
            m_svcAcsChart = svcAcsChart;
            m_fileHandler = fileHandler;
            m_wordESRep = wordESRep;
            m_wordExtESRep = wordExtESRep;
            this.pkrESRep = pkrESRep;
            m_svcReport = svcReport;
            this.itemNameService = itemNameService;
            this.m_repLog = m_repLog;
        }
        public ReportsController()
        {

        }

        /// <summary>
        /// 根据报告ID取检测报告
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<controller>/5
        [HttpGet]
        [Route("Hello")]
        public string Get(int id)
        {
            return "暂未实现，敬请期待";
        }

        [HttpPost]
        [Route("ByTotalCode")]
        public AjaxResult ByTotalCode(ReportCheck reportCheck)
        {
            if (reportCheck == null || string.IsNullOrEmpty(reportCheck.reportnum))
            {
                return AjaxResult.Failure("报告编号为空!");
            }

            ReportViewModel result = m_svcReport.GetByReportNum(reportCheck);
            if (result != null && string.IsNullOrEmpty(result.QRCODEBAR))
            {
                result.QRCODEBAR = m_svcReport.GetQzQrcodeBar(result.ENTRUSTNUM);
            }

            if(result==null)
            {
                return AjaxResult.Failure("没有找到指定编号的报告");
            }

            return AjaxResult.WithData(result);
        } 

        /// <summary>
        /// 根据报告编号 REPORTNUM 和customId 查找检测报告
        /// reportNum中格式为 reportNum,customId
        /// </summary>
        /// <param name="reportNum"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ByCode/{reportNum}")]
        public AjaxResult ByCode(string reportNum)
        {
            if (reportNum == null)
            {
                return AjaxResult.Failure("报告编号为空!");
            }

            ReportViewModel result = new ReportViewModel();
            if (reportNum.Contains(","))
            {
                var reportNumWithCustomId = reportNum.Split(',');
                string realReportNum = reportNumWithCustomId[0];
                string customID = reportNumWithCustomId[1];
                result = m_svcReport.GetByReportNumAndCustomId(realReportNum, customID);

            }
            else
            {
                result = m_svcReport.GetByReportNum(reportNum);
            }


            if (result == null)
            {
                return AjaxResult.Failure("没有找到指定编号的报告");
            }
            else
            {
                if (string.IsNullOrEmpty(result.QRCODEBAR))
                {
                    result.QRCODEBAR = m_svcReport.GetQzQrcodeBar(result.ENTRUSTNUM); 
                }
            }
            return AjaxResult.WithData(result);
        } 


        /// <summary>
        /// 根据报告 SYSPRIMARYKEY 取ACS试验记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("AcsItems/{key}")]
        public AjaxResult AcsItems(string key)
        {
            var resp = m_repAcs.Search(s => s.Index("gx-t-bp-acs").From(0).Size(999).Query(q => q.
                                                            Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(key))))));
            if (!resp.IsValid)
            {
                return AjaxResult.Failure("曲线数据无效：报告ID=" + key);
            }
            else
            {
                List<TBpAcsViewModel> acsData = new List<TBpAcsViewModel>();
                foreach (var item in resp.Documents)
                {
                    TBpAcsViewModel acs = new TBpAcsViewModel();
                    acs.PK = item.PK;
                    acs.MAXVALUE = item.MAXVALUE;
                    acs.UnitName = itemNameService.GetAcsAxisLabel(item.ITEMTABLENAME);
                    acs.ACSTIME = item.ACSTIME.HasValue ? item.ACSTIME.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;

                    acsData.Add(acs);
                }
                return AjaxResult.WithData(acsData);
            }
        }

        /// <summary>
        /// 根据试验记录主键 PK 取试验曲线数据
        /// </summary>
        /// <param name="acsPk">试验记录主键，来自 es_t_bp_acs.Pk，取值样例：2700511^27005110000000787297^JiXiangHeZai2</param>
        /// <returns></returns>
        /// <seealso cref="es_t_bp_acs"/>
        [HttpGet]
        [Route("AcsData/{acsPk}")]
        public AjaxResult AcsData(string acsPk)
        {
            IGetResponse<es_t_bp_acs> respAcs = m_repAcs.Get(new DocumentPath<es_t_bp_acs>(acsPk).Index("gx-t-bp-acs"));
            if (!respAcs.IsValid)
                return AjaxResult.Failure("内部错误：IESRepsitory请求无效，Pk=" + acsPk);

            es_t_bp_acs acsItem = respAcs.Source;
            if (acsItem == null)
                return AjaxResult.Failure("找不到对应的曲线记录，Pk=" + acsPk);
            if (string.IsNullOrWhiteSpace(acsItem.ACSDATAPATH))
                return AjaxResult.Failure("曲线记录不包含文件路径，Pk=" + acsPk);

            var filePath = acsItem.ACSDATAPATH;
            AcsChart chart = null;

            // 曲线数据是图片
            if (itemNameService.IsAcsPicItemType(acsItem.DATATYPES))
            {
                return AjaxResult.WithData(new { FilePath = HttpUtility.UrlEncode(filePath), IsImage = true });
            }
            // 曲线数据是URL地址
            if (acsItem.ACSDATAPATH.StartsWith("http"))
            {
                var bData = RavenFSHttpUtility.DownloadFile(acsItem.ACSDATAPATH);
                MemoryStream ms = new MemoryStream(bData, 0, bData.Length);
                chart = m_svcAcsChart.BuildAcsChart(acsItem, ms);
                if (chart.ChartItems.Count > 0)
                {
                    chart.yMaxValue = chart.ChartItems.Max(ci => ci.yValue).ToString("f2");
                    chart.xMaxValue = chart.ChartItems.Max(ci => ci.xTime).ToString("f2");
                }
            }
            // 其它：曲线数据是本地地址
            else
            {
                byte[] pkrData = m_svcReport.GetStoreFile(acsItem.ACSDATAPATH);
                MemoryStream ms = new MemoryStream(pkrData);
                chart = m_svcAcsChart.BuildAcsChart(acsItem, ms);
                if (chart.ChartItems.Count > 0)
                {
                    chart.yMaxValue = chart.ChartItems.Max(ci => ci.yValue).ToString("f2");
                    chart.xMaxValue = chart.ChartItems.Max(ci => ci.xTime).ToString("f2");
                }
            }
            return AjaxResult.WithData(new { FilePath = filePath, IsImage = false, Chart = chart });
        }

        [HttpGet]
        [Route("Image")]
        public HttpResponseMessage Image(string id)
        {
            string fileName = "graph.jpg";
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                //从图片中读取byte
                byte[] pkrData = m_svcReport.GetStoreFile(HttpUtility.UrlDecode(id));

                resp.Content = new ByteArrayContent(pkrData);
                resp.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));
                var content_disposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName//设置文件的默认名称
                };
                resp.Content.Headers.ContentDisposition = content_disposition;
            }
            catch (Exception ex)
            {
                resp = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                resp.Content = new StringContent(ex.Message, Encoding.UTF8, "text/json");
            }
            return resp;
        }

        protected InstFilter GetCurrentInstFilter()
        {
            var user = m_svcUser.GetUserByName(User.Identity.Name);
            if (user == null)
                throw new HttpException(403, "请先登录！");
            return m_svcUser.GetFilterInsts(user.Id, "");
        }

        private bool CanViewReport(string customId)
        {
            if (string.IsNullOrEmpty(customId))
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

        private HttpResponseMessage FileResponce(string filePath, string contentType = null, string fileName = null)
        {
            using (Stream sr = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] buff = new byte[sr.Length];
                int count = sr.Read(buff, 0, buff.Length);

                if (string.IsNullOrEmpty(fileName))
                    fileName = System.IO.Path.GetFileName(filePath);
                if (string.IsNullOrEmpty(contentType))
                    contentType = MimeMapping.GetMimeMapping(fileName);
                return FileResponce(buff, contentType, fileName);
            }
        }

        private HttpResponseMessage FileResponce(byte[] buf, string contentType, string fileName)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(buf);
            //设置文件的Content-Type（MIME）
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            //设置Disposition-type为attachment（附件）
            var content_disposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName//设置文件的默认名称
            };
            response.Content.Headers.ContentDisposition = content_disposition;
            return response;
        }

        /// <summary>
        /// 根据 SysPrimaryKey 取C类型报告附件
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CTypeReport/{pk}")]
        public HttpResponseMessage CTypeReport(string pk)
        {
            var wordReport = m_wordESRep.Search(s => s.Index("gx-t-bp-wordreport").From(0).Size(10).Query(q => q.
                                                          Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(pk))))));

            if (wordReport.IsValid
                && wordReport.Documents.Count > 0)
            //&& CanViewReport(wordReport.Documents.First().CUSTOMID))
            {
                var wordreport = wordReport.Documents.First();
                string fileName = System.IO.Path.GetFileName(wordreport.WORDREPORTPATH);
                if (!fileName.Contains("."))
                {
                    fileName = string.Format("{0}.doc", fileName);
                }
                string contentType = MimeMapping.GetMimeMapping(fileName);

                byte[] data = null;
                //if (wordreport.WORDREPORTPATH.StartsWith("http"))
                //{
                //    data = RavenFSHttpUtility.DownloadFile(wordreport.WORDREPORTPATH);

                //    MemoryStream datams = DocumentToPdf(ref fileName, data);

                //    return FileResponce(datams.ToArray(), contentType, fileName);
                //}
                //else
                {
                    //if (wordreport.ISLOCALSTORE == "1")
                    //{
                    //    var cReportFolder = ConfigurationManager.AppSettings["CReportTempFolder"];
                    //    string filePath = System.IO.Path.Combine(cReportFolder, fileName);
                    //    using (var proxy = new WcfProxy<IFileTransferService>())
                    //    {
                    //        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    //        {
                    //            proxy.Service.GetPKRReport(wordreport.WORDREPORTPATH).CopyTo(fs);
                    //        }
                    //    }
                    //    return this.FileResponce(filePath, contentType, fileName);
                    //}
                    //else
                    {
                        byte[] fileData = this.m_svcReport.GetStoreFile(wordreport.WORDREPORTPATH);
                        return this.FileResponce(fileData, contentType, fileName);
                    }
                }

            }
            else
            {
                var wordExtReport = m_wordESRep.Search(s => s.Index("gx-t-bp-wordreport").From(0).Size(10).Query(q => q.
                                                           Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(pk))))));

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

                    byte[] data = null;
                    //if (wordreport.WORDREPORTPATH.StartsWith("http"))
                    //{
                    //    data = RavenFSHttpUtility.DownloadFile(wordreport.WORDREPORTPATH);

                    //    MemoryStream datams = DocumentToPdf(ref fileName, data);

                    //    return FileResponce(datams.ToArray(), contentType, fileName);
                    //}
                    //else
                    {
                        //if (wordreport.ISLOCALSTORE == "1")
                        //{
                        //    var cReportFolder = ConfigurationManager.AppSettings["CReportTempFolder"];
                        //    string filePath = System.IO.Path.Combine(cReportFolder, fileName);
                        //    using (var proxy = new WcfProxy<IFileTransferService>())
                        //    {
                        //        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        //        {
                        //            proxy.Service.GetPKRReport(wordreport.WORDREPORTPATH).CopyTo(fs);
                        //        }
                        //    }
                        //    return FileResponce(filePath, contentType, fileName);
                        //}
                        //else
                        {
                            byte[] fileData = this.m_svcReport.GetStoreFile(wordreport.WORDREPORTPATH);
                            return FileResponce(fileData, contentType, fileName);
                        }
                    }
                }
                else
                {
                    HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.NotFound);
                    return result;
                }
            }
        }


        /// <summary>
        /// 根据 ReportNum 取普通类型报告附件
        /// </summary>
        /// <param name="reportNum"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CommonReport/{reportNum}")]
        public HttpResponseMessage CommonReport(string reportNum)
        {
            var customId = string.Empty;
            if (reportNum.Length > 7)
            {
                customId = reportNum.Substring(0, 7);
            }

            //最新的bs检测已经将customid和reportnum合并了，平台需要再此连接customid
            string newReportNum = string.Format("{0}{1}", customId, reportNum);

            Func<QueryContainerDescriptor<es_t_pkpm_binaryReport>, QueryContainer> filterQuery = q => (q.Terms(t => t.Field(f => f.REPORTNUM).Terms(reportNum))
                                                                                                          || q.Terms(t => t.Field(tf => tf.REPORTNUM).Terms(newReportNum)))
                                                                                                       && q.Exists(qe => qe.Field(qet => qet.REPORTPATH));

            var pkrResponse = pkrESRep.Search(s => s.Sort(ss => ss.Descending(sd => sd.UPLOADTIME)).Index("gx-t-pkpm-pkr").From(0).Size(10).Query(filterQuery));

            if (pkrResponse.IsValid
                && pkrResponse.Documents.Count > 0
                )
            {
                //var pkrFolder = ConfigurationManager.AppSettings["PKRTempFolder"];
                var pkrReport = pkrResponse.Documents.First();
                string reportPath = pkrReport.REPORTPATH;
                //string fileName = System.IO.Path.GetFileName(reportPath);
                //string filePath = System.IO.Path.Combine(pkrFolder, fileName);

                //byte[] data = null;
                //data = m_svcReport.GetStoreFile(reportPath);
                //System.IO.File.WriteAllBytes(filePath, data);

                var imageData = m_svcReport.GetImageFromPkr(reportPath);
                var imageFileName = string.Format("报告文件{0}-{1}.png", DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.Ticks);
                string contentType = MimeMapping.GetMimeMapping(imageFileName);
                return this.FileResponce(imageData, contentType, imageFileName);
            }
            else
            {
                var noImageFolder = ConfigurationManager.AppSettings["NoPKRImageFolder"];
                var fileData = System.IO.File.ReadAllBytes(noImageFolder);
                string imageFileName = "graph.png";
                string contentType = MimeMapping.GetMimeMapping(imageFileName);
                return FileResponce(fileData, contentType, imageFileName);
            }
        }

        //private static MemoryStream DocumentToPdf(ref string fileName, byte[] data)
        //{
        //    MemoryStream datams = new MemoryStream();
        //    if (fileName.Contains(".doc") || fileName.Contains(".docx"))
        //    {
        //        fileName = fileName.Split('.')[0] + ".pdf";
        //        Aspose.Words.Document doc = new Aspose.Words.Document(new MemoryStream(data));
        //        doc.Save(datams, Aspose.Words.SaveFormat.Pdf);

        //    }
        //    else if (fileName.Contains(".xls") || fileName.Contains(".xlsx"))
        //    {
        //        fileName = fileName.Split('.')[0] + ".pdf";
        //        Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook(new MemoryStream(data));
        //        workbook.Save(datams, Aspose.Cells.SaveFormat.Pdf);
        //    }
        //    else
        //    {
        //        datams = new MemoryStream(data);
        //    }

        //    return datams;
        //}


        /// <summary>
        /// 根据报告 SYSPRIMARYKEY 取修改日志
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ModifyLogs/{key}")]
        public AjaxResult ModifyLogs(string key)
        {
            var resp = m_repLog.Search(s => s.Index("gx-t-bp-modifylog").From(0).Size(999).Query(q => q.
                                                            Bool(b => b.Filter(f => f.Term(ft => ft.Field(ftf => ftf.SYSPRIMARYKEY).Value(key))))));
            if (!resp.IsValid)
            {
                return AjaxResult.Failure("修改记录数据无效：报告ID=" + key);
            }
            else
            {
                List<TBpModifyLogModel> logData = new List<TBpModifyLogModel>();
                foreach (var item in resp.Documents)
                {
                    TBpModifyLogModel log = new TBpModifyLogModel();
                    log.PK = item.PK;
                    log.FIELDNAME = item.FIELDNAME;
                    log.BEFOREMODIFYVALUES = item.BEFOREMODIFYVALUES;
                    log.AFTERMODIFYVALUES = item.AFTERMODIFYVALUES;
                    log.MODIFYDATETIME = item.MODIFYDATETIME.HasValue ? item.MODIFYDATETIME.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;

                    logData.Add(log);
                }
                return AjaxResult.WithData(logData);
            }
        }
    }


}
