using FluentValidation;
using Newtonsoft.Json;
using Ninject;
using NLog;
using Pkpm.Core;
using Pkpm.Core.HtyService;
using Pkpm.Entity;
using Pkpm.Entity.DTO;
using Pkpm.Entity.Models;
using Pkpm.Framework.Cache;
using Pkpm.Framework.Common;
using Pkpm.Framework.Repsitory;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace PkpmGx.WebService
{
    /// <summary>
    /// PkpmWeb 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class PkpmWeb : System.Web.Services.WebService
    {
        static string HtyUploadFilePath = System.Configuration.ConfigurationManager.AppSettings["HtyUploadFilePath"];
        private static Logger logger = NLog.LogManager.GetLogger("PkpmWeb.asmx");
        private static StandardKernel kernel;
        IRepsitory<t_hty_Image> htyImageRep;
        IRepsitory<tab_hty_programme> HTYRep;
        IRepsitory<tab_hty_gj> gjRep;
        IRepsitory<tab_hty_gjcq> gjcqRep;
        IRepsitory<t_bp_People> peopleRep;
        IHtyService svc;
        IPileService pileService;
        IRepsitory<t_prog_Image> progImageRep;
        IRepsitory<tab_pile_programme> programmeRep;
        static string uploadFolder = ConfigurationManager.AppSettings["ZJUploadFilePath"];
        static string ZJFileFolder = string.Format("{0}\\{1}\\",
                         uploadFolder,
                         DateTime.Now.ToString("yyyy-MM"));

        static PkpmWeb()
        {
            kernel = new StandardKernel();
            kernel.Bind(typeof(ICache<>)).To(typeof(MemoryCache<>)).InSingletonScope();
            kernel.Bind<IDbConnectionFactory>().ToMethod(x => ServiceStackDBContext.DbFactory).InSingletonScope();
            kernel.Bind(typeof(IRepsitory<>)).To(typeof(ServiceStackRepsitory<>));
            kernel.Bind<IHtyService>().To<HtyService>();
            kernel.Bind<IPileService>().To<PileService>();
            JsConfig<DateTime?>.SerializeFn = Dt => Dt.HasValue ? Dt.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
        }
        public PkpmWeb()
        {
            this.HTYRep = kernel.Get<IRepsitory<tab_hty_programme>>();
            this.gjRep = kernel.Get<IRepsitory<tab_hty_gj>>();
            this.gjcqRep = kernel.Get<IRepsitory<tab_hty_gjcq>>();
            this.htyImageRep = kernel.Get<IRepsitory<t_hty_Image>>();
            this.peopleRep = kernel.Get<IRepsitory<t_bp_People>>();
            this.progImageRep = kernel.Get<IRepsitory<t_prog_Image>>();
            this.programmeRep = kernel.Get<IRepsitory<tab_pile_programme>>();
            this.svc = kernel.Get<IHtyService>();
            this.pileService = kernel.Get<IPileService>();
        }
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// 上传回弹仪方案
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="fileData"></param>
        /// <param name="hqfileData"></param>
        /// <returns></returns>
        [WebMethod]
        public string UploadZNHTY(string strJson, byte[] fileData, byte[] hqfileData)
        {
            logger.Debug("UploadZNHTY:{0}".Fmt(strJson));
            var ErrorMsg = string.Empty;
            ControllerResult cResult = ControllerResult.SuccResult;
            tab_hty_programme pileModel = JsonConvert.DeserializeObject<tab_hty_programme>(strJson);
            if (string.IsNullOrEmpty(pileModel.checknum))
            {
                cResult = ControllerResult.FailResult;
                cResult.ErroMsg = "检测流水号不能为空";
                return cResult.ToJson();
            }
            string errMsg = string.Empty;
            if (fileData != null)
            {
                var storeFilename = StoreFileFromBytes(HtyUploadFilePath, pileModel.filename, fileData, out ErrorMsg);

                if (string.IsNullOrWhiteSpace(ErrorMsg))
                {
                    pileModel.filepath = storeFilename;
                }
                else
                {
                    cResult = ControllerResult.FailResult;
                    cResult.ErroMsg = ErrorMsg;

                    return cResult.ToJson();

                }
            }

            if (hqfileData != null)
            {
                var storeFilename = StoreFileFromBytes(HtyUploadFilePath, pileModel.hqfilename, hqfileData, out ErrorMsg);

                if (string.IsNullOrWhiteSpace(ErrorMsg))
                {
                    pileModel.hqfilepath = storeFilename;
                }
                else
                {
                    cResult = ControllerResult.FailResult;
                    cResult.ErroMsg = ErrorMsg;

                    return cResult.ToJson();
                }
            }

            var success = svc.UploadZNHTY(pileModel, out ErrorMsg);
            if (!success)
            {
                cResult = ControllerResult.FailResult;
                cResult.ErroMsg = ErrorMsg;
            }

            return cResult.ToJson();
        }
        /// <summary>
        /// 将上传的二进制保存
        /// </summary>
        /// <param name="folder">路径前缀</param>
        /// <param name="storefileName">文件名</param>
        /// <param name="byteData">二进制文件</param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private static string StoreFileFromBytes(string folder, string storefileName, byte[] byteData, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                string suffix = string.Empty;
                if (!string.IsNullOrWhiteSpace(storefileName))
                {
                    suffix = System.IO.Path.GetExtension(storefileName);
                }

                if (string.IsNullOrWhiteSpace(suffix))
                {
                    suffix = ".doc";
                }

                var fileName = "{0}{1}".Fmt(Snowflake.Instance().GetId().ToString().Replace("-", ""), suffix);
                errMsg = string.Empty;
                //COSUtility.UploadObjectFromBytes(buckName, fileName, byteData, out errMsg);

                string filePathAll = System.IO.Path.Combine(folder, fileName);
                string fileFolder = System.IO.Path.GetDirectoryName(filePathAll);
                if (!Directory.Exists(fileFolder))
                {
                    Directory.CreateDirectory(fileFolder);
                }
                try
                {
                    File.WriteAllBytes(filePathAll, byteData);
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                    throw;
                }
                return filePathAll;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return string.Empty;
            }
        }


        /// <summary>
        /// 上传回弹仪构件，测区数据
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [WebMethod]
        public string UploadZNHTYGJ(string strJson)
        {
            logger.Debug("UploadZNHTYGJ:{0}".Fmt(strJson));
            var ErrorMsg = string.Empty;
            ControllerResult cResult = ControllerResult.SuccResult;
            ZNHTYGjDataModel pileModel = JsonConvert.DeserializeObject<ZNHTYGjDataModel>(strJson);
            if (string.IsNullOrEmpty(pileModel.checknum))
            {
                cResult = ControllerResult.FailResult;
                cResult.ErroMsg = "检测流水号不能为空";
                return cResult.ToJson();
            }
            var success = svc.UploadZNHTYGJ(pileModel, out ErrorMsg);
            if (!success)
            {
                cResult = ControllerResult.FailResult;
                cResult.ErroMsg = ErrorMsg;
            }

            return cResult.ToJson();
        }

        /// <summary>
        /// 根据条件查询回弹仪方案-构件-测区信息
        /// </summary>
        /// <param name="checknum">流水号</param>
        /// <param name="projectnum">方案id</param>
        /// <returns></returns>
        [WebMethod]
        public string GetHTYInfo(string checknum, int? progid, string projectnum, string unitcode, string area, int? posStart, int? Count)
        {
            int pos = posStart.HasValue ? posStart.Value : 0;
            int count = Count.HasValue ? Count.Value : 30;
            count = count > 30 ? 30 : count;
            HTYFangAnWebServiceModel model = new HTYFangAnWebServiceModel();
            var predicate = PredicateBuilder.True<tab_hty_programme>();
            if (!checknum.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.checknum == (checknum));
            }
            if (!projectnum.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.projectnum == (projectnum));
            }
            if (!unitcode.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.unitcode == (unitcode));
            }
            if (!area.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.area == (area));
            }
            if (progid.HasValue)
            {
                predicate = predicate.And(t => t.id == progid);
            }
            try
            {
                PagingOptions<tab_hty_programme> pagingOption = new PagingOptions<tab_hty_programme>(pos, count, t => new { t.id });
                var datas = HTYRep.GetByConditonPage(predicate, pagingOption);
                model.Data = new List<ZNHTYWebServiceModel>();
                foreach (var item in datas)
                {
                    model.Data.Add(item.ConvertTo<ZNHTYWebServiceModel>());
                }

                model.Count = model.Data.Count;
                model.IsSucc = true;
                foreach (var item in model.Data)
                {
                    List<string> peopleNames = new List<string>();
                    try
                    {
                        var peopleids = item.testingpeople.Split(',');
                        foreach (var itemid in peopleids)
                        {

                            var people = peopleRep.GetById(itemid);
                            if (people != null)
                            {
                                peopleNames.Add(people.Name);
                            }
                            else
                            {
                                peopleNames.Add(itemid);
                            }
                        }
                    }
                    catch
                    {
                        peopleNames.Add("");
                    }
                    item.testingpeople = peopleNames.Join(",");

                    //取构件数据
                    var gjDatas = gjRep.GetByCondition(w => w.checknum == item.checknum);
                    item.gjDatas = new List<ZNHTYGjDataModel>();
                    foreach (var gj in gjDatas)
                    {
                        ZNHTYGjDataModel gjmodel = gj.ConvertTo<ZNHTYGjDataModel>();
                        var gjcqDatas = gjcqRep.GetByCondition(w => w.gjid == gj.id);//构件测区
                        gjmodel.gjcqDatas = gjcqDatas;
                        item.gjDatas.Add(gjmodel);
                    }
                }
            }
            catch (Exception ex)
            {
                model.Msg = ex.Message;
                model.IsSucc = false;
            }
            return model.ToJson();
        }

        /// <summary>
        /// 根据检测流水号查询回弹仪方案-构件-测区信息
        /// </summary>
        /// <param name="checknum"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetHTYInfoWithchecknum(string checknum)
        {

            HTYFangAnWebWithchecknumServiceModel model = new HTYFangAnWebWithchecknumServiceModel();
            var predicate = PredicateBuilder.True<tab_hty_programme>();
            if (!checknum.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.checknum == (checknum));
            }
            else
            {
                model.IsSucc = false;
                model.Msg = "检测流水号不能为空";
                return model.ToJson();
            }

            try
            {
                var datas = HTYRep.GetByCondition(predicate);
                model.Data = new ZNHTYWebServiceModel();
                if (datas != null && datas.Count > 0)
                {
                    var fangan = datas.First();
                    var item = (fangan.ConvertTo<ZNHTYWebServiceModel>());
                    model.Data = item;
                    List<string> peopleNames = new List<string>();
                    try
                    {
                        var peopleids = item.testingpeople.Split(',');
                        foreach (var itemid in peopleids)
                        {

                            var people = peopleRep.GetById(itemid);
                            if (people != null)
                            {
                                peopleNames.Add(people.Name);
                            }
                            else
                            {
                                peopleNames.Add(itemid);
                            }
                        }
                    }
                    catch
                    {
                        peopleNames.Add("");
                    }
                    item.testingpeople = peopleNames.Join(",");

                    //取构件数据
                    var gjDatas = gjRep.GetByCondition(w => w.checknum == item.checknum);
                    item.gjDatas = new List<ZNHTYGjDataModel>();
                    foreach (var gj in gjDatas)
                    {
                        ZNHTYGjDataModel gjmodel = gj.ConvertTo<ZNHTYGjDataModel>();
                        var gjcqDatas = gjcqRep.GetByCondition(w => w.gjid == gj.id);//构件测区
                        gjmodel.gjcqDatas = gjcqDatas;
                        item.gjDatas.Add(gjmodel);
                    }
                }

                model.IsSucc = true;

            }
            catch (Exception ex)
            {
                model.Msg = ex.Message;
                model.IsSucc = false;
            }
            return model.ToJson();
        }

        /// <summary>
        /// 回弹仪厂商调用，获取方案信息,主要取方案中的检测流水号
        /// </summary>
        /// <param name="projectname"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetHTYFangAnWithProjectName(string projectname)
        {
            HTYFangAnModel model = new HTYFangAnModel();
            model.IsSucc = true;
            if (string.IsNullOrEmpty(projectname))
            {
                model.IsSucc = false;
                model.Msg = "工程名称为空";
                return model.ToJson();
            }
            try
            {

                model.Datas = HTYRep.GetByCondition(w => w.projectname == projectname);
                model.Count = model.Datas.Count;
            }
            catch (Exception ex)
            {
                model.IsSucc = false;
                model.Msg = ex.Message;
            }
            return model.ToJson();
        }

        /// <summary>
        /// 构件回弹检测后上传图片
        /// </summary>
        /// <param name="proId">方案id</param>
        /// <param name="checkNum">检测流水号</param>
        /// <param name="gjNo">构件编号</param>
        /// <param name="data">base64</param>
        /// <returns></returns>
        [WebMethod]
        public string UploadHTYGjImage(int proId, string checkNum, string gjNo, string data, out string errMsg)
        {
            ControllerResult result = ControllerResult.SuccResult;
            errMsg = string.Empty;
            var imageBytes = Convert.FromBase64String(data);
            string fileName = StoreFileFromBytes(HtyUploadFilePath, "noname.jpg", imageBytes, out errMsg);
            if (!errMsg.IsNullOrEmpty())
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errMsg;
                return result.ToJson();
            }

            var model = new t_hty_Image()
            {
                ProgId = proId,
                CheckNum = checkNum,
                gjNo = gjNo,
                Path = fileName
            };

            //验证
            t_hty_ImageValidator validator = new t_hty_ImageValidator();
            var vResult = validator.Validate(model);
            string errorMsg = string.Empty;
            if (!vResult.IsValid)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = vResult.Errors.Select(e => e.ErrorMessage).Join(",");
                return result.ToJson();
            }

            htyImageRep.Insert(model);

            return result.ToJson();
        }

        /// <summary>
        /// 获取回弹仪构件图片
        /// </summary>
        /// <param name="checkNum"></param>
        /// <param name="gjNo"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetHTYGjImage(string checkNum, string gjNo)
        {
            HTYGjImagesModel model = new HTYGjImagesModel();
            model.IsSucc = true;
            model.Msg = "";
            model.Count = 0;
            model.Datas = new List<HTYGjImage>();
            if (string.IsNullOrWhiteSpace(checkNum) || string.IsNullOrWhiteSpace(gjNo))
            {
                model.IsSucc = false;
                model.Msg = "入参不能为空";
                return model.ToJson();
            }
            try
            {
                var images = htyImageRep.GetByCondition(w => w.CheckNum == checkNum && w.gjNo == gjNo);
                if (images != null && images.Count > 0)
                {
                    foreach (var img in images)
                    {
                        if (!string.IsNullOrWhiteSpace(img.Path))
                        {
                            HTYGjImage imgData = new HTYGjImage();
                            var dataByte = File.ReadAllBytes(img.Path);
                            var Base64Str = Convert.ToBase64String(dataByte);
                            imgData.ImageData = Base64Str;

                            model.Datas.Add(imgData);
                        }
                    }
                    model.Count = model.Datas.Count;
                }
            }
            catch (Exception ex)
            {
                model.IsSucc = false;
                model.Msg = ex.Message;
            }

            return model.ToJson();
        }


        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="byteData"></param>
        /// <returns></returns>
        [WebMethod]
        public string UploadFile(string FileName, byte[] byteData)
        {
            string Remark = string.Empty;
            string IsSucc = "true";
            var uploadFolder = ConfigurationManager.AppSettings["PortWebUploadImgUrl"];
            string FileFolder = string.Format("{0}\\{1}\\",
                            uploadFolder,
                            DateTime.Now.ToString("yyyy-MM"));
            //防止文件名重复
            FileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), System.IO.Path.GetExtension(FileName));

            string FilePath = System.IO.Path.Combine(FileFolder, FileName);

            if (!Directory.Exists(FileFolder))
            {
                Directory.CreateDirectory(FileFolder);
            }

            try
            {
                File.WriteAllBytes(FilePath, byteData);
            }
            catch (Exception ex)
            {
                IsSucc = "false";
                Remark = ex.Message;
            }
            StringBuilder sb = new StringBuilder(64);
            sb.Append("{");
            sb.Append("\"IsSucc\":\"");
            sb.Append(IsSucc);
            sb.Append("\",\"FilePath\":\"");
            //sb.Append(FilePath.Replace(@"\", @"\\"));
            sb.Append(FilePath);
            sb.Append("\",\"Remark\":\"");
            sb.Append(Remark);
            sb.Append("\"}");
            return sb.ToString();
        }

        [WebMethod]
        public string GetCheckPeople(string customId)
        {
            CheckPeopleWeoXinViewModel model = new CheckPeopleWeoXinViewModel();
            model.Data = new List<CheckPeopleServiceModel>();
            try
            {
                var people = peopleRep.GetByCondition(p => p.Customid == customId);//
                var nowData = DateTime.Now;
                foreach (var item in people)
                {
                    var Onepeople = item.ConvertTo<CheckPeopleServiceModel>();
                    if (item.Approvalstatus == "3" && (item.postnumenddate > nowData || item.postnumenddate > nowData.AddDays(-30)))
                    {
                        Onepeople.IsNormal = 1;
                    }
                    model.Data.Add(Onepeople);
                }
                model.Count = model.Data.Count;
                model.IsSucc = true;
            }
            catch (Exception ex)
            {
                model.IsSucc = false;
                model.Msg = ex.Message;
            }
            return model.ToJson();
        }

        [WebMethod]
        public string GetNormalCheckPeople(string customId)
        {
            CheckPeopleWeoXinViewModel model = new CheckPeopleWeoXinViewModel();
            model.Data = new List<CheckPeopleServiceModel>();
            try
            {
                var people = peopleRep.GetByCondition(p => p.Customid == customId && p.postnumenddate > DateTime.Now);//
                var nowData = DateTime.Now;
                foreach (var item in people)
                {
                    var Onepeople = item.ConvertTo<CheckPeopleServiceModel>();
                    model.Data.Add(Onepeople);
                }
                model.Count = model.Data.Count;
                model.IsSucc = true;
            }
            catch (Exception ex)
            {
                model.IsSucc = false;
                model.Msg = ex.Message;
            }
            return model.ToJson();
        }

        /// <summary>
        /// 根据条件查询方案信息
        /// </summary>
        /// <param name="checknum">流水号</param>
        /// <param name="projectnum">工程编号</param>
        /// <param name="unitcode">机构编码</param>
        /// <param name="area">地区</param>
        /// <returns></returns>
        [WebMethod]
        public string GetZhuangJiFangAn(string checknum, string projectnum, string unitcode, string area, int? posStart, int? Count)
        {
            int pos = posStart.HasValue ? posStart.Value : 0;
            int count = Count.HasValue ? Count.Value : 30;
            count = count > 30 ? 30 : count;
            ZhuangjiFangAnWebServiceModel model = new ZhuangjiFangAnWebServiceModel();
            var predicate = PredicateBuilder.True<tab_pile_programme>();
            if (!checknum.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.checknum.Contains(checknum));
            }
            if (!projectnum.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.projectnum.Contains(projectnum));
            }
            if (!unitcode.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.unitcode.Contains(unitcode));
            }
            if (!area.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.Area == area);
            }
            try
            {
                model.Data = programmeRep.GetByCondition(predicate, null, null, pos, count);
                model.Count = model.Data.Count;
                model.IsSucc = true;
                foreach (var item in model.Data)
                {
                    List<string> peopleNames = new List<string>();
                    try
                    {
                        var peopleids = item.testingpeople.Split(',');
                        foreach (var itemid in peopleids)
                        {

                            var people = peopleRep.GetById(itemid);
                            if (people != null)
                            {
                                peopleNames.Add(people.Name);
                            }
                            else
                            {
                                peopleNames.Add(itemid);
                            }
                        }
                    }
                    catch
                    {
                        peopleNames.Add("");
                    }
                    item.testingpeople = peopleNames.Join(",");
                }
            }
            catch (Exception ex)
            {
                model.Msg = ex.Message;
                model.IsSucc = false;
            }
            return model.ToJson();
        }

        /// <summary>
        /// 上传桩基方案
        /// </summary>
        /// <param name="strJson"></param>
        /// <param name="fileData"></param>
        /// <param name="hqfileData"></param>
        /// <returns></returns>
        [WebMethod]
        public string UploadZhuangji(string strJson, byte[] fileData, byte[] hqfileData)
        {
            logger.Debug("UploadZhuangji:{0}".Fmt(strJson));
            var ErrorMsg = string.Empty;
            ControllerResult cResult = ControllerResult.SuccResult;
            pile_programme_model pileModel = JsonConvert.DeserializeObject<pile_programme_model>(strJson);
            if (string.IsNullOrEmpty(pileModel.checknum))
            {
                cResult = ControllerResult.FailResult;
                cResult.ErroMsg = "检测流水号不能为空";
                return cResult.ToJson();

            }
            //关于实行地基基础承载力检测数据自动采集和实时上传系统设计需求  中规定  by 振华 19.7.10
            if (string.IsNullOrEmpty(pileModel.testingpeople) || pileModel.testingpeople.Split(',').Count() < 2)
            {
                cResult = ControllerResult.FailResult;
                cResult.ErroMsg = "检测人员不能少于两人";
                return cResult.ToJson();
            }
            //关于实行地基基础承载力检测数据自动采集和实时上传系统设计需求  中规定  by 振华 19.7.10
            if (string.IsNullOrEmpty(pileModel.testingequipment))
            {
                cResult = ControllerResult.FailResult;
                cResult.ErroMsg = "检测设备不能为空";
                return cResult.ToJson();
            }

            string errMsg = string.Empty;
            if (fileData != null)
            {
                var storeFilename = StoreFileFromBytes(ZJFileFolder, pileModel.filename, fileData, out ErrorMsg);

                if (string.IsNullOrWhiteSpace(ErrorMsg))
                {
                    pileModel.filepath = storeFilename;
                }
                else
                {
                    cResult = ControllerResult.FailResult;
                    cResult.ErroMsg = ErrorMsg;
                    return cResult.ToJson();

                }
            }

            if (hqfileData != null)
            {
                var storeFilename = StoreFileFromBytes(ZJFileFolder, pileModel.hqfilename, hqfileData, out ErrorMsg);

                if (string.IsNullOrWhiteSpace(ErrorMsg))
                {
                    pileModel.hqfilepath = storeFilename;
                }
                else
                {
                    cResult = ControllerResult.FailResult;
                    cResult.ErroMsg = ErrorMsg;

                    return cResult.ToJson();
                }
            }

            var success = pileService.UploadZhuangji(pileModel, out ErrorMsg);
            if (!success)
            {
                cResult = ControllerResult.FailResult;
                cResult.ErroMsg = ErrorMsg;
            }

            return cResult.ToJson();
        }

        /// <summary>
        /// 获取桩基现场（历史）检测数据中的详情信息（平台上点击最左边才展示的那部分）
        /// </summary>
        /// <param name="checkNum">检测流水号</param>
        /// <param name="Info_Status">区分现场还是历史，0现场，1历史</param>
        /// <returns></returns>
        [WebMethod]
        public string GetZJSceneTestData(string checkNum, int Info_Status)
        {
            logger.Debug("获取桩基现场/历史数据：GetZJSceneTestData,checkNum：{0}，Info_Status：{1}".Fmt(checkNum, Info_Status));
            ZJSceneTestDataWebServiceModel model = new ZJSceneTestDataWebServiceModel()
            {
                IsSucc = true,
                Data = new List<ZJSceneTestDetailModel>()
            };
            var datas = new List<ZJSceneTestDetailModel>();
            if (!checkNum.IsNullOrEmpty())
            {
                model.Data = pileService.GetZJSceneTestDetailData(Info_Status, checkNum);
                model.Count = model.Data.Count;
            }
            else
            {
                model.IsSucc = false;
                model.Msg = "流水号不能为空";
            }

            return model.ToJson();

        }

        /// <summary>
        /// 获取现场方案图片信息
        /// </summary>
        /// <param name="programId">现场检测方案编号</param>
        /// <param name="checknum">检测流水号</param>
        /// <param name="STAKE_ID">试桩编号</param>
        /// <returns></returns>
        [WebMethod]
        public string GetZJProImage(int programId, string checknum, string STAKE_ID)
        {
            HTYGjImagesModel model = new HTYGjImagesModel()
            {
                Datas = new List<HTYGjImage>(),
                IsSucc = true
            };

            if (!checknum.IsNullOrEmpty() && !STAKE_ID.IsNullOrEmpty())
            {

                var imageList = this.progImageRep.GetByCondition(t => t.ProgId == programId && t.CheckNum == checknum && t.jzsynos == STAKE_ID).OrderBy(o => o.Status).ToList();
                foreach (var item in imageList)
                {
                    if (!item.Path.IsNullOrEmpty())
                    {
                        HTYGjImage imgData = new HTYGjImage();
                        string FilePath = System.IO.Path.Combine(ZJFileFolder, item.Path);
                        var dataByte = File.ReadAllBytes(FilePath);
                        //var dataByte = COSUtility.DownLoadObject(item.Path, buckName);
                        var Base64Str = Convert.ToBase64String(dataByte);
                        imgData.ImageData = Base64Str;
                        model.Datas.Add(imgData);
                    }
                }
                //model.Data = imageList;
                model.Count = model.Datas.Count();
            }
            else
            {
                model.IsSucc = false;
                model.Msg = "获取桩基方案照片失败，原因：方案流水号或试桩编号为空";
            }

            return model.ToJson();
        }

        /// <summary>
        /// 把传入的base64的文件转换为byte 存储
        /// </summary>
        /// <param name="data"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private static string StoreImage(string data, string uploadFileName, out string errMsg)
        {
            errMsg = string.Empty;

            try
            {
                if (data.IsNullOrEmpty())
                {
                    return string.Empty;
                }
                //后缀名
                var extend = string.Empty;
                if (uploadFileName.IsNullOrEmpty())
                {
                    extend = ".jpg";
                }
                else
                {
                    extend = System.IO.Path.GetExtension(uploadFileName);
                }

                var fileName = "{0}{1}".Fmt(Snowflake.Instance().GetId().ToString().Replace("-", ""), extend);
                var imageBytes = Convert.FromBase64String(data);
                errMsg = string.Empty;
                //var uploadFolder = ConfigurationManager.AppSettings["ZJUploadFilePath"];
                //string FileFolder = string.Format("{0}\\{1}\\",
                //                uploadFolder,
                //                DateTime.Now.ToString("yyyy-MM"));
                string FilePath = System.IO.Path.Combine(ZJFileFolder, fileName);
                if (!Directory.Exists(ZJFileFolder))
                {
                    Directory.CreateDirectory(ZJFileFolder);
                }
                File.WriteAllBytes(FilePath, imageBytes);
                return fileName;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
                return string.Empty;
            }
        }

        /// <summary>
        /// 异常情况上传
        /// </summary>
        /// <param name="unitcode">单位编码</param>
        /// <param name="projectnum">工程编码</param>
        /// <param name="checknum">检测流水号</param>
        /// <param name="pileno">试桩编号</param>
        /// <param name="people">上报人</param>
        /// <param name="typeinfo">异常类别 1为检测数据传输中断或无法传输 2为检测过程中人员、设备有变更  3为检测过程中设备损坏一时无法修复</param>
        /// <param name="content">异常情况说明</param>
        /// <param name="photo1">照片1 的base64字符串</param>
        /// <param name="photo2">照片2 的base64字符串</param>
        /// <param name="photo3">照片3 的base64字符串</param>
        /// <returns></returns>
        [WebMethod]
        public string UploadException(string unitcode, string projectnum, string checknum, string pileno, string people, string typeinfo, string content,
            string photo1, string photo2, string photo3)
        {
            ControllerResult result = ControllerResult.SuccResult;

            string[] rawPhotos = new string[] { photo1, photo2, photo3 };

            List<string> photoPaths = new List<string>();

            string errMsg = string.Empty;

            foreach (var item in rawPhotos)
            {
                if (!item.IsNullOrEmpty())
                {
                    string photoName = StoreImage(item, string.Empty, out errMsg);
                    if (errMsg.IsNullOrEmpty())
                    {
                        photoPaths.Add(photoName);
                    }
                    else
                    {
                        result = ControllerResult.FailResult;
                        result.ErroMsg = errMsg;

                        return result.ToJson();
                    }
                }
            }


            tab_pile_exception model = new tab_pile_exception()
            {
                unitcode = unitcode,
                projectnum = projectnum,
                checknum = checknum,
                pileno = pileno,
                people = people,
                typeinfo = typeinfo,
                content = content,
                photo = string.Join(",", photoPaths)
            };

            //验证
            tab_pile_exceptionValidator validator = new tab_pile_exceptionValidator();
            var vResult = validator.Validate(model);
            string errorMsg = string.Empty;
            if (!vResult.IsValid)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = vResult.Errors.Select(e => e.ErrorMessage).Join(",");
                return result.ToJson();
            }

            model.createTime = DateTime.Now;

            int id = 0;
            if (!pileService.CreateException(model, out id, out errMsg))
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errMsg;
            }
            else
            {
                result.AdditonMsg = id.ToString();
            }

            return result.ToJson();
        }

        /// <summary>
        /// 异常信息审核
        /// </summary>
        /// <param name="Id">审核Id</param>
        /// <param name="handlePeople">审批人</param>
        /// <param name="handleContent">审批意见</param>
        /// <returns></returns>
        [WebMethod]
        public string UploadExceptionApprove(int Id, string handlePeople, string handleContent)
        {
            ControllerResult result = ControllerResult.SuccResult;

            string errMsg = string.Empty;

            if (handlePeople.IsNullOrEmpty() || handleContent.IsNullOrEmpty())
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = "审核内容和审核人不能为空，请核对后重新提交";
                return result.ToJson();
            }

            if (Id <= 0)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = "异常编号不能为空，请核对后重新提交";
                return result.ToJson();
            }

            if (!pileService.ApproveException(Id, handlePeople, handleContent, out errMsg))
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errMsg;
            }

            return result.ToJson();
        }

        /// <summary>
        /// 获取桩基数据
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="checknum"></param>
        /// <param name="jzsynos"></param>
        /// <param name="pos"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetZhuangJi(string projectName, string checknum, string jzsynos, string area, int pos, int count)
        {
            GetZhuangJiModel model = new GetZhuangJiModel()
            {
                ProjectName = projectName,
                checknum = checknum,
                jzsynos = jzsynos,
                posStart = pos,
                count = count
            };


            var datas = pileService.GetZhuangji(projectName, checknum, jzsynos, area, pos, count);
            return datas.ToJson();
        }

        /// <summary>
        /// 多个检测流水号，用英文逗号隔开
        /// 6 桩基报告和检测报告关联接口（类似重庆现场检测）--方案里面的 检测流水号，tbpitem的sysprimarykey
        /// </summary>
        /// <param name="PileCheckNums"></param>
        /// <param name="SysPrimaryKey"></param>
        /// <returns></returns>
        [WebMethod]
        public string UploadPileReportLink(string PileCheckNums, string SysPrimaryKey)
        {
            var ErrorMsg = string.Empty;
            ControllerResult cResult = ControllerResult.SuccResult;
            bool result = pileService.UploadPileReportLink(PileCheckNums, SysPrimaryKey, out ErrorMsg);
            if (!result)
            {
                cResult = ControllerResult.FailResult;
                cResult.ErroMsg = ErrorMsg;
            }
            return cResult.ToJson();
        }

        /// <summary>
        /// 上传方案图片
        /// </summary>
        /// <param name="proId">方案编号</param>
        /// <param name="checkNum">检测流水号</param>
        /// <param name="jzsynos">基桩号</param>
        /// <param name="status">状态  123456</param>
        /// <param name="takeTime">拍摄时间</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">维度</param>
        /// <param name="description">照片描述</param>
        /// <param name="data">base64的字符串</param>
        /// 
        /// <returns></returns>
        ///
        [WebMethod]
        public string UploadProgramme(int proId, string checkNum, string jzsynos, int status, DateTime? takeTime, string longitude, string latitude, string description, string data)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errMsg = string.Empty;
            string fileName = StoreImage(data, string.Empty, out errMsg);
            if (!errMsg.IsNullOrEmpty())
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errMsg;
                return result.ToJson();
            }

            var model = new t_prog_Image()
            {
                ProgId = proId,
                CheckNum = checkNum,
                jzsynos = jzsynos,
                Status = status,
                Path = fileName,
                Latitude = decimal.Parse(latitude),
                Longitude = decimal.Parse(longitude),
                Description = description,
                TakeTime = takeTime,
                UploadTime = DateTime.Now
            };

            //验证
            t_prog_ImageValidator validator = new t_prog_ImageValidator();
            var vResult = validator.Validate(model);
            string errorMsg = string.Empty;
            if (!vResult.IsValid)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = vResult.Errors.Select(e => e.ErrorMessage).Join(",");
                return result.ToJson();
            }

            progImageRep.Insert(model);

            return result.ToJson();
        }

        /// <summary>
        /// 取工程gps信息
        /// </summary>
        /// <param name="checknum"></param>
        /// <param name="stakeid"></param>
        /// <param name="infostatus"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetZJProjectGPS(string checknum, string stakeid, int? infostatus)
        {
            ZJProjectGPSWebServiceModel model = new ZJProjectGPSWebServiceModel();
            model.IsSucc = true;
            if (checknum.IsNullOrEmpty() || stakeid.IsNullOrEmpty() || @infostatus.HasValue)
            {
                model.IsSucc = false;
                model.Msg = "参数不能为空";
                return model.ToJson();
            }
            try
            {
                var data = pileService.GetZJProjectGPS(checknum, stakeid, infostatus.Value);
                model.Data = data;
            }
            catch (Exception ex)
            {
                model.IsSucc = false;
                model.Msg = ex.Message;
                return model.ToJson();
            }
            return model.ToJson();
        }

        /// <summary>
        /// 获取桩基异常记录
        /// </summary>
        /// <param name="checknum"></param>
        /// <param name="projectname"></param>
        /// <param name="stakeid"></param>
        /// <param name="posStart"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [WebMethod]
        public string GetZJException(string checknum, string projectname, string stakeid, int? posStart, int? pageSize)
        {
            ZJExceptionWebServiceModel model = new ZJExceptionWebServiceModel();
            model.IsSucc = true;

            try
            {
                var data = pileService.GetZJException(checknum, projectname, stakeid, posStart, pageSize);
                model.Datas = data;
            }
            catch (Exception ex)
            {
                model.IsSucc = false;
                model.Msg = ex.Message;
                return model.ToJson();
            }
            return model.ToJson();
        }
    }
    public class t_hty_ImageValidator : AbstractValidator<t_hty_Image>
    {
        /// <summary>
        /// 
        /// </summary>
        public t_hty_ImageValidator()
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(esp => esp.ProgId).NotEmpty().WithMessage("方案编号不能为空");
            RuleFor(esp => esp.CheckNum).NotEmpty().WithMessage("检测流水号不能为空");
            RuleFor(esp => esp.gjNo).NotEmpty().WithMessage("构件编号不能为空");
            //RuleFor(esp => esp.Status).NotEmpty().WithMessage("状态不能为空");

        }
    }

    public class tab_pile_exceptionValidator : AbstractValidator<tab_pile_exception>
    {
        /// <summary>
        /// 
        /// </summary>
        public tab_pile_exceptionValidator()
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(esp => esp.unitcode).NotEmpty().WithMessage("单位编码不能为空");
            RuleFor(esp => esp.projectnum).NotEmpty().WithMessage("工程编码 不能为空");
            RuleFor(esp => esp.checknum).NotEmpty().WithMessage("检测流水号不能为空");
            RuleFor(esp => esp.pileno).NotEmpty().WithMessage("试桩编号不能为空");
            RuleFor(esp => esp.typeinfo).NotEmpty().WithMessage("异常类别不能为空");
            RuleFor(esp => esp.people).NotEmpty().WithMessage("上报人不能为空");

        }
    }
    public class t_prog_ImageValidator : AbstractValidator<t_prog_Image>
    {
        /// <summary>
        /// 
        /// </summary>
        public t_prog_ImageValidator()
        {
            ValidatorOptions.CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(esp => esp.ProgId).NotEmpty().WithMessage("方案编号不能为空");
            RuleFor(esp => esp.CheckNum).NotEmpty().WithMessage("检测流水号不能为空");
            RuleFor(esp => esp.jzsynos).NotEmpty().WithMessage("基桩号不能为空");
            RuleFor(esp => esp.Status).NotEmpty().WithMessage("状态不能为空");

        }
    }

}
