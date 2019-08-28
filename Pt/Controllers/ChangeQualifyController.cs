using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Core.SysDictCore;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Framework.PkpmConfigService;
using Pkpm.Framework.Repsitory;
using Pkpm.Entity;
using Pkpm.Framework.FileHandler;
using Dhtmlx.Model.Toolbar;
using PkpmGX.Models;
using Dhtmlx.Model.Grid;
using ServiceStack;
using Pkpm.Framework.Common;
using ServiceStack.OrmLite;
using Dhtmlx.Model.Form;
using Pkpm.Entity.DTO;
using Pkpm.Entity.Models;
using System.Text.RegularExpressions;
using System.IO;
using Pkpm.Core.CheckQualifyCore;
using Newtonsoft.Json;
using System.Drawing;
using System.Text;
using ThoughtWorks.QRCode.Codec;


namespace PkpmGX.Controllers
{
    [Authorize]
    public class ChangeQualifyController : PkpmController
    {
        ISysDictService sysDictServcie;
        ICheckUnitService checkUnitService;
        ICheckQualifyService checkQualifyService;
        IPkpmConfigService pkpmConfigService;
        IRepsitory<t_D_UserTableOne> repOne;
        IRepsitory<t_D_UserTableTwo> repTwo;
        IRepsitory<t_D_UserTableTen> repTen;
        IRepsitory<t_D_UserTableThree> repThree;
        IRepsitory<t_D_UserTableFour> repFour;
        IRepsitory<t_bp_custom> rep;
        IRepsitory<t_D_UserChange> repChange;
        IRepsitory<t_bp_CusAchievement> CusAchievement;
        IRepsitory<t_bp_CusAward> CusAwards;
        IRepsitory<t_D_UserTableFive> repFive;
        IRepsitory<t_bp_CusPunish> CusPunish;
        IRepsitory<t_bp_CheckCustom> CheckCustom;
        IRepsitory<t_bp_CusChange> CusChange;
        IRepsitory<t_bp_People> peopleRep;
        IRepsitory<SupvisorJob> supvisorRep;
        IRepsitory<t_bp_Equipment> equRep;
        IRepsitory<User> repUser;
        IFileHandler fileHander;
        //private static string uploadInstStartWith = System.Configuration.ConfigurationManager.AppSettings["UploadInstStartWith"];
        // GET: CheckQualify
        public ChangeQualifyController(ISysDictService sysDictServcie,
            ICheckUnitService checkUnitService,
            IRepsitory<t_D_UserTableOne> repOne,
            IRepsitory<t_D_UserTableTwo> repTwo,
             IRepsitory<t_D_UserTableFive> repFive,
             IRepsitory<t_D_UserTableTen> repTen,
            IRepsitory<t_bp_custom> rep,
            IRepsitory<t_D_UserChange> repChange,
            IRepsitory<t_bp_CusAchievement> CusAchievement,
            IRepsitory<t_bp_CusAward> CusAwards,
            IRepsitory<SupvisorJob> supvisorRep,
            IPkpmConfigService pkpmConfigService,
            IRepsitory<t_bp_CusPunish> CusPunish,
            IRepsitory<t_D_UserTableThree> repThree,
            IRepsitory<t_D_UserTableFour> repFour,
            IRepsitory<t_bp_CheckCustom> CheckCustom,
            IRepsitory<t_bp_CusChange> CusChange,
            IRepsitory<t_bp_People> peopleRep,
            IRepsitory<t_bp_Equipment> equRep,
            IRepsitory<User> repUser,
            IFileHandler fileHander,
            IUserService userService,
            ICheckQualifyService checkQualifyService) : base(userService)
        {
            this.supvisorRep = supvisorRep;
            this.pkpmConfigService = pkpmConfigService;
            this.sysDictServcie = sysDictServcie;
            this.checkUnitService = checkUnitService;
            this.repOne = repOne;
            this.repTwo = repTwo;
            this.rep = rep;
            this.repChange = repChange;
            this.repTen = repTen;
            this.repThree = repThree;
            this.repFour = repFour;
            this.repFive = repFive;
            this.CusAchievement = CusAchievement;
            this.CusAwards = CusAwards;
            this.CusPunish = CusPunish;
            this.CheckCustom = CheckCustom;
            this.CusChange = CusChange;
            this.fileHander = fileHander;
            this.peopleRep = peopleRep;
            this.equRep = equRep;
            this.repUser = repUser;
            this.userService = userService;
            this.checkQualifyService = checkQualifyService;
        }


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FiveToolBar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Custom", "导入") { Img = "fa fa-plus", Imgdis = "fa fa-plus" });
            toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("1"));
            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult ToolBar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            var buttons = GetCurrentUserPathActions();
            if (HaveButtonFromAll(buttons, "Create"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Create", "新增") { Img = "fa fa-plus", Imgdis = "fa fa-plus" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("1"));
            }

            //toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Export", "导出") { Img = "fa fa-file-excel-o", Imgdis = "fa fa-file-excel-o" });
            //toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("2"));

            if (HaveButtonFromAll(buttons, "Delete"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Delete", "[删除]") { Img = "fa fa-times", Imgdis = "fa fa-times" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("3"));
            }

            if (HaveButtonFromAll(buttons, "Submit"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("SetState", "[递交]") { Img = "fa fa-paper-plane", Imgdis = "fa fa-paper-plane" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("4"));
            }

            if (HaveButtonFromAll(buttons, "ReturnStatus"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("ReturnState", "[状态返回]") { Img = "fa fa-undo", Imgdis = "fa fa-undo" });
            }

            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult Search(ChangeQualifySearchModel searchModel)
        {
            var data = GetSearchResult(searchModel);

            DhtmlxGrid grid = new DhtmlxGrid();
            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            grid.AddPaging(data.TotalCount, pos);
            var buttons = GetCurrentUserPathActions();
            var currentUserRole = GetCurrentUserRole();

            for (int i = 0; i < data.Results.Count; i++)
            {
                var rowdata = data.Results[i];
                DhtmlxGridRow row = new DhtmlxGridRow(rowdata.id.ToString());
                row.AddCell(string.Empty);
                row.AddCell((pos + i + 1).ToString());//序号
                row.AddCell(rowdata.unitname);
                row.AddCell(rowdata.bgnr);
                row.AddCell(rowdata.bgq);
                row.AddCell(rowdata.bgh);
                row.AddCell(rowdata.time.HasValue? rowdata.time.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
                row.AddLinkJsCell("扫描件", "LookPhoto(\"{0}\")".Fmt(rowdata.bgclpath));
                row.AddCell(rowdata.cbr);
                row.AddCell(rowdata.SQname + "/" + rowdata.SQTel);
                if (rowdata.@static == 0)
                {
                    row.AddCell("新增");
                }
                else if (rowdata.@static == 1)
                {
                    row.AddCell("已递交【待审批】");
                }
                else if (rowdata.@static == 2)
                {
                    row.AddCell("审核不通过");
                }
                else if (rowdata.@static == 3)
                {
                    row.AddCell("审核通过");
                }

                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                if (HaveButtonFromAll(buttons, "Edit") && (rowdata.@static == 0 || rowdata.@static == 1))
                {
                    row.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                }
                else
                {
                    row.AddCell("");
                }

                if (HaveButtonFromAll(buttons, "Approval") && rowdata.@static == 1)
                {
                    row.AddLinkJsCell("审批", "Approval(\"{0}\")".Fmt(rowdata.id));
                }
                else if (HaveButtonFromAll(buttons, "Print") && rowdata.@static == 3)
                {
                    row.AddLinkJsCell("打印通知单", "Print(\"{0}\")".Fmt(rowdata.id));
                }
                else
                {
                    row.AddCell("");
                }
               

                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        private SearchResult<ChangeQualifyUIModel> GetSearchResult(ChangeQualifySearchModel searchModel)
        {

            var predicate = PredicateBuilder.True<t_D_UserChange>();

            #region 动态查询

            if (!string.IsNullOrWhiteSpace(searchModel.CheckUnitName))
            {
                predicate = predicate.And(t => t.unitCode == searchModel.CheckUnitName);
            }

            var instFilter = GetCurrentInstFilter();
            if (instFilter.NeedFilter && instFilter.FilterInstIds.Count() > 0)
            {
                predicate = predicate.And(t => instFilter.FilterInstIds.Contains(t.unitCode));
            }

            if (IsCurrentSuperVisor())
            {
                var area = GetCurrentAreas();
                var userInArea = checkUnitService.GetUnitByArea(area);
                var insts = userInArea.Select(t => t.Key).ToList();
                if (insts != null && insts.Count >= 0)
                {
                    predicate = predicate.And(t => insts.Contains(t.unitCode));
                }
            }

            #endregion

            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            int count = searchModel.count.HasValue ? searchModel.count.Value : 30;

            //string[] columns = { "CheckBox", "SeqNo", "NAME", "UNITNAME" };
            string orderby = string.Empty;
            string orderByAcs = string.Empty;
            bool isDes = true;
            string sortProperty = "time";

            //if (searchModel.orderColInd.HasValue
            //    && !string.IsNullOrWhiteSpace(searchModel.direct)
            //    && searchModel.orderColInd.Value <= columns.Length)
            //{
            //    if (searchModel.direct == "asc")
            //    {
            //        sortProperty = orderby = columns[searchModel.orderColInd.Value];
            //        isDes = false;
            //    }
            //    if (searchModel.direct == "des")
            //    {
            //        sortProperty = orderByAcs = columns[searchModel.orderColInd.Value];
            //        isDes = true;
            //    }
            //}

            PagingOptions<t_D_UserChange> pagingOption = new PagingOptions<t_D_UserChange>(pos, count, sortProperty, isDes);

            var customs = repChange.GetByConditionSort<ChangeQualifyUIModel>(predicate, r => new
            {
                r.id,
                r.unitname,
                r.area,
                r.bgnr,
                r.bgq,
                r.bgh,
                r.time,
                r.unitCode,
                r.@static,
                r.fh,
                r.sp,
                r.outstaticinfo,
                r.bgclpath,
                r.cbr,
                r.SQname,
                r.SQTel,
                r.YZZPath,
                r.EndTime
                },
               pagingOption);

            return new SearchResult<ChangeQualifyUIModel>(pagingOption.TotalItems, customs);
        }


        public ActionResult Details(string id)
        {
            var model = GetApplyQualifyTwoViewModel(id, "");
            return View(model);
        }


        public ActionResult GetFile(string path, int IsModify)
        {
            var model = new AddFileModel()
            {
                Ids = new Dictionary<string, int>()
            };
            var number = 1;
            if (!path.IsNullOrEmpty())
            {
                var paths = path.Split('|');
                foreach (var item in paths)
                {
                    if (item.IsNullOrEmpty())
                    {
                        continue;
                    }
                    //var img=Image(item)
                    model.Ids.Add(item, number++);
                    //model.Number = number++;
                }
            }
            model.Modify = IsModify;
            return View(model);
        }

        /// <summary>
        /// 编辑附件信息
        /// </summary>
        /// <param name="path">附件信息的路径</param>
        /// <param name="IsFile">标识是否是文件，文件即非图片</param>
        /// <returns></returns>
        public ActionResult EditAttachFile(string id, string path, string IsFile)
        {
            EditCheckQulifyAttachFileModel model = new EditCheckQulifyAttachFileModel()
            {
                Id = string.Empty,
                IsFile = 0,
                paths = new Dictionary<int, string>()
            };

            if (!path.IsNullOrEmpty())
            {
                model.path = path;
                int i = 1;
                var paths = path.Split('|').ToList();
                foreach (var item in paths)
                {
                    if (!item.IsNullOrEmpty())
                    {
                        model.paths.Add(i++, item);
                    }
                }
            }
            if (!IsFile.IsNullOrEmpty())
            {
                model.IsFile = int.Parse(IsFile);
            }
            model.Id = id;
            return View(model);
        }


        public ActionResult Approval(int id)
        {
            var model = new ChangeQualifyUIModel()
            {
                id = id,
            };
            return View(model);
        }

        public ActionResult Print(int id)
        {
            var userChange = repChange.GetById(id);
            var model = new ChangeQualifyUIModel()
            {
                unitname = userChange.unitname,
                area = userChange.area,
                bgnr = userChange.bgnr,
                bgq = userChange.bgq,
                bgh = userChange.bgh,
                time = DateTime.Now,
                EndTime = userChange.EndTime,
                bgclpath = userChange.bgclpath,
                cbr = userChange.cbr,
                SQname = userChange.SQname,
                SQTel = userChange.SQTel,
                YZZPath = userChange.YZZPath
            };

            model.ImageUrl = CreateQrcode(userChange.unitCode);

            return View(model);
        }

        private string CreateQrcode(string unitcode)
        {
            string ImageUrl = string.Empty;
            Bitmap bt;
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeVersion = 0;
            string enCodeString = Request.Url.ToString(); //"内容";
            bt = qrCodeEncoder.Encode(enCodeString, Encoding.UTF8);

            string filename = Guid.NewGuid().ToString();
            string murl = Server.MapPath("/UserFiles/nrImage/");
            string savePathUser = unitcode + "\\" + DateTime.Now.ToShortDateString() + "\\" + DateTime.Now.Hour.ToString() + "\\";
            string savePath = murl + "\\" + savePathUser;

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            System.Drawing.Bitmap objNewPic = new System.Drawing.Bitmap(bt, 210, 200);//图片保存的大小尺寸  
            objNewPic.SetResolution(96, 96);

            objNewPic.Save(savePath + filename + ".jpg", System.Drawing.Imaging.ImageFormat.Gif);
            murl = savePath + filename + ".jpg";
            ImageUrl = "/UserFiles/nrImage/" + savePathUser + filename + ".jpg";

            return ImageUrl;
        }

        [HttpPost]
        public ActionResult Approval(ChangeQualifyUIModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;

            var saveModel = new t_D_UserChange
            {
                id = model.id,
                @static = model.@static,
                EndTime = DateTime.Now.ToString("yyyy年MM月dd日")
            };

            var saveResult = checkQualifyService.Approval(saveModel, out errorMsg);
            if (!saveResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("进行了资质证书变更申请操作，机构为{0}".Fmt(model.unitname));
            }

            return Content(result.ToJson());
        }

        public ActionResult Create()
        {
            var model = new ChangeQualifyCreateViewModel();
            model.Region = userService.GetAllArea();
            var custom = GetCurrentCustomInfo();
            if (custom != null)
            {
                model.UnitCode = custom.ID;
                model.UnitName = custom.NAME;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ChangeQualifyUIModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;

            var saveModel = new t_D_UserChange
            {
                unitname = model.unitname,
                area = model.area,
                bgnr = model.bgnr,
                bgq = model.bgq,
                bgh = model.bgh,
                time = model.time.HasValue ? model.time : DateTime.Now,
                unitCode = model.unitCode,
                @static = 0,
                bgclpath = model.bgclpath,
                cbr = repUser.GetById(GetCurrentUserId()).UserDisplayName,
                SQname = model.SQname,
                SQTel = model.SQTel,
                YZZPath = model.YZZPath
            };

            var saveResult = checkQualifyService.SaveUserChange(saveModel, out errorMsg);
            if (!saveResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("进行了资质证书变更申请操作，机构为{0}".Fmt(model.unitname));
            }

            return Content(result.ToJson());
        }

        public ActionResult Detail(int id)
        {
            var userChange = repChange.GetById(id);
            var model = new ChangeQualifyUIModel()
            {
                id = id,
                unitname = userChange.unitname,
                area = userChange.area,
                time = userChange.time,
                bgnr = userChange.bgnr,
                bgq = userChange.bgq,
                bgh = userChange.bgh,
                bgclpath = userChange.bgclpath,
                cbr = userChange.cbr,
                SQname = userChange.SQname,
                SQTel = userChange.SQTel,
                YZZPath = userChange.YZZPath
            };

            model.Region = userService.GetAllArea();

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var userChange = repChange.GetById(id);
            var model = new ChangeQualifyUIModel()
            {
                id = id,
                unitname = userChange.unitname,
                area = userChange.area,
                bgnr = userChange.bgnr,
                bgq = userChange.bgq,
                bgh = userChange.bgh,
                bgclpath = userChange.bgclpath,
                cbr = userChange.cbr,
                SQname = userChange.SQname,
                SQTel = userChange.SQTel,
                YZZPath = userChange.YZZPath
            };

            model.Region = userService.GetAllArea();

            return View(model);
        }


        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(ChangeQualifyUIModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;

            var saveModel = new t_D_UserChange
            {
                id = model.id,
                unitname = model.unitname,
                area = model.area,
                bgnr = model.bgnr,
                bgq = model.bgq,
                bgh = model.bgh,
                bgclpath = model.bgclpath,
                SQname = model.SQname,
                SQTel = model.SQTel,
                YZZPath = model.YZZPath
            };

            var saveResult = checkQualifyService.EditUserChange(saveModel, out errorMsg);
            if (!saveResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("进行了资质证书变更修改操作，机构为{0}".Fmt(model.unitname));
            }

            return Content(result.ToJson());
        }



        public ActionResult addFRWorkExperience()
        {
            return View();
        }

        public ActionResult addJSWorkExperience()
        {
            return View();
        }

        public ActionResult EditCheckQualify(string id)
        {
            var viewModel = GetApplyQualifyTwoViewModel(id, "");
            return View(viewModel);
        }

        public ActionResult UnitQualificaiton()
        {
            DhtmlxForm dForm = new DhtmlxForm();

            var allQualifications = sysDictServcie.GetDictsByKey("unitQualifications");
            int halfCount = allQualifications.Count / 2;

            for (int i = 0; i < halfCount; i++)
            {
                dForm.AddDhtmlxFormItem(new DhtmlxFormCheckbox(allQualifications[i].Id.ToString(), allQualifications[i].Name, allQualifications[i].Name, false).AddStringItem("position", "label-right").AddStringItem("offsetLeft", "15"));
            }

            dForm.AddDhtmlxFormItem(new DhtmlxFormNewcolumn());

            for (int i = halfCount; i < allQualifications.Count; i++)
            {
                dForm.AddDhtmlxFormItem(new DhtmlxFormCheckbox(allQualifications[i].Id.ToString(), allQualifications[i].Name, allQualifications[i].Name, false).AddStringItem("position", "label-right").AddStringItem("offsetLeft", "15"));
            }

            string str = dForm.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }


        [HttpPost]
        public ActionResult Delete(int id)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!(repChange.DeleteById(id) > 0))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对资质变更ID为{0}进行了删除操作".Fmt(id));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }



        [HttpPost]
        [AllowAnonymous]
        public ActionResult attachUpload(string id)
        {
            string realname = Request.Files["file"].FileName;

            var extensionName = System.IO.Path.GetExtension(realname);

            string filename = "/{0}/{1}{2}".Fmt(id, Guid.NewGuid().ToString().Replace("-", ""), extensionName);

            fileHander.UploadFile(Request.Files["file"].InputStream, "userfiles", filename);

            DhtmlxUploaderResult uploader = new DhtmlxUploaderResult() { state = true, name = filename };
            string str = uploader.ToJson();
            return Content(str);
        }

        [AllowAnonymous]
        public FileResult Image(ImageViewDownload model)
        {
            //导过来的历史数据，路径都带有"/userfiles"，文件系统里面的路径没有"/userfiles"，所以要去掉
            string fileName = Regex.Replace(model.itemValue, Regex.Escape("/userfiles"), "", RegexOptions.IgnoreCase);//model.itemValue;
            string mimeType = MimeMapping.GetMimeMapping(fileName);
            Stream stream = fileHander.LoadFile("userfiles", fileName);
            return new FileStreamResult(stream, mimeType);
        }

        [HttpPost]
        public ActionResult DeleteImage(ImageViewUpload model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string fileName = Regex.Replace(model.itemValue, Regex.Escape("/userfiles"), "", RegexOptions.IgnoreCase);
            try
            {
                fileHander.DeleteFile("userfiles", fileName);
                string errMsg = string.Empty;
                var custom = rep.GetById(model.itemId);
                string pathvalues = string.Empty;
                switch (model.itemName)
                {
                    case "businessnumPath":
                        pathvalues = custom.businessnumPath;
                        break;
                    case "DETECTPATH":
                        pathvalues = custom.DETECTPATH;
                        break;
                    case "MEASNUMPATH":
                        pathvalues = custom.MEASNUMPATH;
                        break;
                    case "instrumentpath":
                        pathvalues = custom.instrumentpath;
                        break;
                    case "shebaopeoplelistpath":
                        pathvalues = custom.shebaopeoplelistpath;
                        break;
                }
                var queryStr = from str in pathvalues.Split('|')
                               where str != model.itemValue
                               select str;
                //model.itemName存的是字段名
                bool success = this.checkUnitService.UpdateAttachPathsIntoCustom(model.itemId, model.itemName, queryStr.Join("|"), out errMsg);
                if (success)
                {
                    LogUserAction("对机构id为{0}的{1}字段进行了存储地址的更新操作".Fmt(model.itemId, model.itemName));
                }
                else
                {
                    LogUserAction("对机构id为{0}的{1}字段进行了存储地址的更新,操作失败".Fmt(model.itemId, model.itemName));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = "删除失败:" + ex.Message;
            }

            return Content(result.ToJson());
        }

        public ActionResult AttachFileDownload(string id)
        {
            string fileName = Regex.Replace(id, Regex.Escape("/userfiles"), "", RegexOptions.IgnoreCase);//model.itemValue;
            string mimeType = MimeMapping.GetMimeMapping(fileName);
            Stream stream = fileHander.LoadFile("userfiles", fileName);
            return File(stream, mimeType, fileName);
        }

        [HttpPost]
        public ActionResult UpdatePaths(ImageViewUpload model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errMsg = string.Empty;
            bool success = this.checkUnitService.UpdateAttachPathsIntoCustom(model.itemId, model.itemName, model.itemValue, out errMsg);
            if (!success)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errMsg;
                LogUserAction("对机构id为{0}的{1}字段进行了存储地址的更新,操作失败".Fmt(model.itemId, model.itemName));
            }
            else
            {
                LogUserAction("对机构id为{0}的{1}字段进行了存储地址的更新操作".Fmt(model.itemId, model.itemName));
            }
            return Content(result.ToJson());
        }

        public ActionResult myAttachGrid(string id)
        {
            var custom = rep.GetById(id);
            DhtmlxGrid grid = new DhtmlxGrid();
            if (!string.IsNullOrWhiteSpace(custom.instrumentpath))
            {
                var path = custom.instrumentpath.Split('|');
                for (int i = 0; i < path.Length; i++)
                {
                    DhtmlxGridRow row = new DhtmlxGridRow(path[i]);
                    row.AddCell((i + 1).ToString());
                    row.AddCell(path[i]);

                    row.AddLinkJsCell("[删除]", "attach_delete(\"{0}\")".Fmt(path[i]));
                    row.AddLinkJsCell("[下载]", "attach_download(\"{0}\")".Fmt(path[i]));
                    grid.AddGridRow(row);
                }
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }


        [HttpPost]
        public ActionResult SetInstState(string selectedId, string state, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                foreach(var id in selectedId.Split(','))
                {
                    var userChange = repChange.GetById(id);
                    if (userChange.@static != 0)
                    {
                        result = ControllerResult.FailResult;
                        result.ErroMsg = "记录当前状态不能进行递交操作";
                        return Content(result.ToJson());
                    }
                }
                
                if (!checkQualifyService.SetInstSendState(selectedId, state, "t_D_UserChange", out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对机构ID为{0}进行了状态返回操作".Fmt(selectedId));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }


        public ActionResult Audit(int id)
        {
            var model = new ApplyQualifyOneAuditViewModel { pid = id, slbh = "SL-" + DateTime.Now.ToString("yyyyMMddHHmmss") };

            return View(model);
        }

        /// <summary>
        /// 受理操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Audit(ApplyQualifyOneAuditViewModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;

            var saveModel = new t_D_UserTableTen
            {
                pid = model.pid,
                slbh = model.slbh,
                slr = repUser.GetById(GetCurrentUserId()).UserDisplayName,
                sltime = model.sltime,
                @static = 1
            };

            var saveResult = checkQualifyService.Audit(saveModel, out errorMsg);
            if (!saveResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("对单位资质进行受理操作，机构ID为{0}".Fmt(model.pid));
            }

            return Content(result.ToJson());
        }

        public ActionResult AddQualifyTwo(string id, string customid)
        {
            var viewModel = GetApplyQualifyTwoViewModel(id, customid);  //添加

            return View(viewModel);
        }

        public ActionResult EditQualifyTwo(string id, string customid)
        {
            var viewModel = GetApplyQualifyTwoDetailViewModel(id, customid); //编辑

            return View(viewModel);
        }

        public ActionResult DetailQualifyTwo(string id, string customid)
        {
            var viewModel = GetApplyQualifyTwoDetailViewModel(id, customid); //查看

            return View(viewModel);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditQualifyTwo(ApplyQualifyTwoSaveModels model)
        {
            ControllerResult result = ControllerResult.SuccResult;

            try
            {
                var savemodel = new t_D_UserTableTwo
                {
                    pid = model.pid,
                    unitcode = model.unitcode,
                    name = model.name,
                    time = model.time,
                    address = model.address,
                    tel = model.tel,
                    postcode = model.postcode,
                    Fax = model.Fax,
                    Email = model.Email,
                    businessnum = model.businessnum,
                    businessnumpath = model.businessnumpath,
                    businessnumunit = model.businessnumunit,
                    regaprice = model.regaprice,
                    economicnature = model.economicnature,
                    measnum = model.measnum,
                    measunit = model.measunit,
                    fr = model.fr,
                    frzw = model.frzw,
                    frzc = model.frzc,
                    jsfzr = model.jsfzr,
                    jsfzrzw = model.jsfzrzw,
                    jsfzrzc = model.jsfzrzc,
                    zbryzs = model.zbryzs,
                    zyjsrys = model.zyjsrys,
                    zjzcrs = model.zjzcrs,
                    gjzcrs = model.gjzcrs,
                    yqsbzs = model.yqsbzs,
                    yqsbgtzc = model.yqsbgtzc,
                    gzmj = model.gzmj,
                    fwjzmj = model.fwjzmj,
                    sqjcyw = model.sqjcyw,
                    @static = 0,
                    measnumpath = model.measnumpath,
                    zzjgdm = model.zzjgdm,
                    YZZPath = model.YZZPath,
                    SQname = model.SQname,
                    SQTel = model.SQTel,
                    bgcszmPath = model.bgcszmPath,
                    zxzmPath = model.zxzmPath,
                    gqzmPath = model.gqzmPath,
                    zzscwjPath = model.zzscwjPath,
                    glscwjPath = model.glscwjPath
                };

                string errorMsg = string.Empty;
                if (!checkQualifyService.SaveUserTableTwo(savemodel, out errorMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = errorMsg;
                }
                else
                {
                    LogUserAction("编辑检测机构：{0}".Fmt(model.name));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }




        private ApplyQualifyTwoUIModels GetApplyQualifyTwoViewModel(string id, string customid)
        {
            var viewModel = new ApplyQualifyTwoUIModels();

            viewModel.Region = userService.GetAllArea();
            viewModel.CompayTypeList = new Dictionary<string, List<SysDict>>();
            viewModel.CompayTypeList.Add("personnelTitles", sysDictServcie.GetDictsByKey("personnelTitles"));
            viewModel.CompayTypeList.Add("yesNo", sysDictServcie.GetDictsByKey("yesNo"));

            viewModel.myT_bp_Custom = rep.GetSingleByCondition<ApplyQuailityDetailModel>(r => r.ID == customid, r => new
            {
                r.ID,
                r.NAME,
                r.area,
                r.CREATETIME,
                r.ispile,
                r.BUSINESSNUMUNIT,
                r.BUSINESSNUM,
                r.ADDRESS,
                r.phone,
                r.ECONOMICNATURE,
                r.FR,
                r.DETECTNUM,
                r.MEASUNIT,
                r.detectunit,
                r.MEASNUM,
                r.HOUSEAREA,
                r.detectnumStartDate,
                r.detectnumEndDate,
                r.detectappldate,
                r.APPLDATE,
                r.measnumStartDate,
                r.measnumEndDate,
                r.DETECTAREA,
                r.instrumentNum,
                r.INSTRUMENTPRICE,
                r.TEL,
                r.EMAIL,
                r.REGAPRICE,
                r.shebaopeoplenum,
                r.POSTCODE,
                r.zzxmgs,
                r.zzlbgs,
                r.zzcsgs,
                r.JSNAME,
                r.JSTIILE,
                r.JSYEAR,
                r.ZLNAME,
                r.ZLTITLE,
                r.ZLYEAR,
                r.PERCOUNT,
                r.MIDPERCOUNT,
                r.HEIPERCOUNT,
                r.hasNumPerCount,
                r.REGJGSTA,
                r.companytype,
                r.businessnumPath,
                r.DETECTPATH,
                r.MEASNUMPATH,
                r.instrumentpath,
                r.shebaopeoplelistpath
            });

            viewModel.myT_bp_Custom.pid = id;

            //计算人员信息相关人数
            var predicate = PredicateBuilder.True<t_bp_People>();
            predicate = predicate.And(p => p.Customid == customid);
            predicate = predicate.And(tp => tp.data_status == null || tp.data_status != "-1");
            var peoples = this.peopleRep.GetByCondition(predicate);
            int PERCOUNT = peoples.Count;//总人数
            int hasNumPerCount = peoples.Where(w => w.ishaspostnum == "1").ToList().Count;//持证人数
            int HEIPERCOUNT = peoples.Where(w => w.Title == "3").ToList().Count;//高级职称
            int MIDPERCOUNT = peoples.Where(w => w.Title == "2").ToList().Count;//中级职称

            viewModel.myT_bp_Custom.PERCOUNT = PERCOUNT.ToString();
            viewModel.myT_bp_Custom.hasNumPerCount = hasNumPerCount.ToString();
            viewModel.myT_bp_Custom.MIDPERCOUNT = MIDPERCOUNT.ToString();
            viewModel.myT_bp_Custom.HEIPERCOUNT = HEIPERCOUNT.ToString();

            viewModel.myT_bp_Custom.JSTIILE = sysDictServcie.GetDictsByKey("personnelTitles", viewModel.myT_bp_Custom.JSTIILE);
            viewModel.myT_bp_Custom.FRTITLE = sysDictServcie.GetDictsByKey("personnelTitles", viewModel.myT_bp_Custom.FRTITLE);
            viewModel.myT_bp_Custom.instrumentNum = equRep.GetByCondition(p => p.customid == customid).Count.ToString();//设备套数
            viewModel.Sex = sysDictServcie.GetDictsByKey("Sex");
            viewModel.CheckWork = sysDictServcie.GetDictsByKey("CheckWork");
            viewModel.PersonnelStaff = sysDictServcie.GetDictsByKey("PersonnelStaff");


            viewModel.File = FileGrid(id,
                viewModel.myT_bp_Custom.businessnumPath,
                viewModel.myT_bp_Custom.MEASNUMPATH,
                viewModel.myT_bp_Custom.bgcszmPath,
                viewModel.myT_bp_Custom.zxzmPath,
                viewModel.myT_bp_Custom.gqzmPath,
                viewModel.myT_bp_Custom.zzscwjPath,
                viewModel.myT_bp_Custom.glscwjPath);

            return viewModel;
        }


        private ApplyQualifyTwoUIModels GetApplyQualifyTwoDetailViewModel(string id, string customid)
        {
            var viewModel = new ApplyQualifyTwoUIModels();

            viewModel.Region = userService.GetAllArea();
            viewModel.CompayTypeList = new Dictionary<string, List<SysDict>>();
            viewModel.CompayTypeList.Add("personnelTitles", sysDictServcie.GetDictsByKey("personnelTitles"));
            viewModel.CompayTypeList.Add("yesNo", sysDictServcie.GetDictsByKey("yesNo"));

            viewModel.myT_D_UserTableTwo = repTwo.GetSingleByCondition<ApplyQualifyTwoSaveModels>(r => r.pid == Convert.ToInt32(id), r => new
            {
                r.id,
                r.pid,
                r.unitcode,
                r.time,
                r.name,
                r.address,
                r.tel,
                r.postcode,
                r.Fax,
                r.Email,
                r.businessnum,
                r.businessnumpath,
                r.businessnumunit,
                r.regaprice,
                r.economicnature,
                r.measnum,
                r.measunit,
                r.fr,
                r.frzw,
                r.frzc,
                r.jsfzr,
                r.jsfzrzw,
                r.jsfzrzc,
                r.zbryzs,
                r.zyjsrys,
                r.zjzcrs,
                r.gjzcrs,
                r.yqsbzs,
                r.yqsbgtzc,
                r.gzmj,
                r.fwjzmj,
                r.sqjcyw,
                r.@static,
                r.measnumpath,
                r.zzjgdm,
                r.YZZPath,
                r.SQname,
                r.SQTel,
                r.bgcszmPath,
                r.zxzmPath,
                r.gqzmPath,
                r.zzscwjPath,
                r.glscwjPath
            });

            viewModel.myT_D_UserTableTwo.pid = Convert.ToInt32(id);

            //计算人员信息相关人数
            var predicate = PredicateBuilder.True<t_bp_People>();
            predicate = predicate.And(p => p.Customid == customid);
            predicate = predicate.And(tp => tp.data_status == null || tp.data_status != "-1");
            var peoples = this.peopleRep.GetByCondition(predicate);
            int PERCOUNT = peoples.Count;//总人数
            int hasNumPerCount = peoples.Where(w => w.ishaspostnum == "1").ToList().Count;//持证人数
            int HEIPERCOUNT = peoples.Where(w => w.Title == "3").ToList().Count;//高级职称
            int MIDPERCOUNT = peoples.Where(w => w.Title == "2").ToList().Count;//中级职称

            viewModel.myT_D_UserTableTwo.zbryzs = PERCOUNT;
            viewModel.myT_D_UserTableTwo.zyjsrys = hasNumPerCount;
            viewModel.myT_D_UserTableTwo.zjzcrs = MIDPERCOUNT;
            viewModel.myT_D_UserTableTwo.gjzcrs = HEIPERCOUNT;

            viewModel.myT_D_UserTableTwo.jsfzrzc = sysDictServcie.GetDictsByKey("personnelTitles", viewModel.myT_D_UserTableTwo.jsfzrzc);
            viewModel.myT_D_UserTableTwo.yqsbzs = equRep.GetByCondition(p => p.customid == customid).Count;//设备套数
            viewModel.Sex = sysDictServcie.GetDictsByKey("Sex");
            viewModel.CheckWork = sysDictServcie.GetDictsByKey("CheckWork");
            viewModel.PersonnelStaff = sysDictServcie.GetDictsByKey("PersonnelStaff");


            viewModel.File = FileGrid(id,
                viewModel.myT_D_UserTableTwo.businessnumpath,
                viewModel.myT_D_UserTableTwo.measnumpath,
                viewModel.myT_D_UserTableTwo.bgcszmPath,
                viewModel.myT_D_UserTableTwo.zxzmPath,
                viewModel.myT_D_UserTableTwo.gqzmPath,
                viewModel.myT_D_UserTableTwo.zzscwjPath,
                viewModel.myT_D_UserTableTwo.glscwjPath);

            return viewModel;
        }


        public List<FileGridModel> FileGrid(string id,
           string businessnumPath,
           string MEASNUMPATH,
           string bgcszmPath,
           string zxzmPath,
           string gqzmPath,
           string zzscwjPath,
           string glscwjPath)
        {
            var files = new List<FileGridModel>();
            var cuntom = repTwo.GetByCondition(r => r.pid == Convert.ToInt16(id)).FirstOrDefault();

            string[,] FileRangeAr = new string[7, 3];
            FileRangeAr[0, 0] = "工商营业执照";
            FileRangeAr[1, 0] = "计量认证证书";
            FileRangeAr[2, 0] = "办公场所证明";
            FileRangeAr[3, 0] = "资信证明文件";
            FileRangeAr[4, 0] = "股权证明文件";
            FileRangeAr[5, 0] = "质量手册（word文档）";
            FileRangeAr[6, 0] = "管理手册（word文档）";

            if (cuntom != null)
            {
                FileRangeAr[0, 1] = cuntom.businessnumpath;
                FileRangeAr[1, 1] = cuntom.measnumpath;
                FileRangeAr[2, 1] = cuntom.bgcszmPath;
                FileRangeAr[3, 1] = cuntom.zxzmPath;
                FileRangeAr[4, 1] = cuntom.gqzmPath;
                FileRangeAr[5, 1] = cuntom.zzscwjPath;
                FileRangeAr[6, 1] = cuntom.glscwjPath;
            }
            else
            {
                FileRangeAr[0, 1] = "";
                FileRangeAr[1, 1] = "";
                FileRangeAr[2, 1] = "";
                FileRangeAr[3, 1] = "";
                FileRangeAr[4, 1] = "";
                FileRangeAr[5, 1] = "";
                FileRangeAr[6, 1] = "";
            }


            FileRangeAr[0, 2] = businessnumPath;
            FileRangeAr[1, 2] = MEASNUMPATH;
            FileRangeAr[2, 2] = bgcszmPath;
            FileRangeAr[3, 2] = zxzmPath;
            FileRangeAr[4, 2] = gqzmPath;
            FileRangeAr[5, 2] = zzscwjPath;
            FileRangeAr[6, 2] = glscwjPath;

            for (int i = 0; i < 7; i++)
            {
                var model = new FileGridModel()
                {
                    Modify = 0
                };
                model.Name = FileRangeAr[i, 0];
                model.State = string.IsNullOrEmpty(FileRangeAr[i, 1]) ? "未上传" : "已上传";
                model.Url = FileRangeAr[i, 2];
                switch (i)
                {
                    case 0:
                        model.Type = "businessnumPath";
                        break;
                    case 1:
                        model.Type = "MEASNUMPATH";
                        break;
                    case 2:
                        model.Type = "bgcszmPath";
                        break;
                    case 3:
                        model.Type = "zxzmPath";
                        break;
                    case 4:
                        model.Type = "gqzmPath";
                        break;
                    case 5:
                        model.Type = "zzscwjPath";
                        model.Modify = 1;
                        break;
                    case 6:
                        model.Type = "glscwjPath";
                        model.Modify = 1;
                        break;
                }

                model.Id = i.ToString();
                files.Add(model);
            }
            return files;
        }


        
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditCheckQualify(CheckCustomSaveViewModel viewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            CheckCustomSaveModel creatModel = new CheckCustomSaveModel();
            try
            {
                creatModel.OpUserId = GetCurrentUserId();

                if (!viewModel.detectnumdate.IsNullOrEmpty())
                {
                    DateTime? detectnumStartDate, detectnumEndDate;
                    CommonUtils.GetLayuiDateRange(viewModel.detectnumdate, out detectnumStartDate, out detectnumEndDate);
                    viewModel.detectnumStartDate = detectnumStartDate;
                    viewModel.detectnumEndDate = detectnumEndDate;
                }
                if (!viewModel.measnumDate.IsNullOrEmpty())
                {
                    //DateTime? measnumStartDate, measnumEndDate;
                    //CommonUtils.GetLayuiDateRange(viewModel.measnumDate, out measnumStartDate, out measnumEndDate);
                    //viewModel.measnumStartDate = measnumStartDate;
                    viewModel.measnumEndDate = DateTime.Parse(viewModel.measnumDate);// measnumEndDate;
                }

                #region 
                creatModel.custom = new t_bp_custom()
                {
                    ID = viewModel.ID,
                    NAME = viewModel.NAME,
                    STATIONID = viewModel.STATIONID,
                    POSTCODE = viewModel.POSTCODE,
                    TEL = viewModel.TEL,
                    FAX = viewModel.FAX,
                    ADDRESS = viewModel.ADDRESS,
                    CREATETIME = viewModel.CREATETIME,
                    EMAIL = viewModel.EMAIL,
                    BUSINESSNUM = viewModel.BUSINESSNUM,
                    BUSINESSNUMUNIT = viewModel.BUSINESSNUMUNIT,
                    REGAPRICE = viewModel.REGAPRICE,
                    ECONOMICNATURE = viewModel.ECONOMICNATURE,
                    MEASNUM = viewModel.MEASNUM,
                    MEASUNIT = viewModel.MEASUNIT,
                    MEASNUMPATH = viewModel.MEASNUMPATH,
                    FR = viewModel.FR,
                    JSNAME = viewModel.JSNAME,
                    JSTIILE = viewModel.JSTIILE,
                    JSYEAR = viewModel.JSYEAR,
                    ZLNAME = viewModel.ZLNAME,
                    ZLTITLE = viewModel.ZLTITLE,
                    ZLYEAR = viewModel.ZLYEAR,
                    PERCOUNT = viewModel.PERCOUNT,
                    MIDPERCOUNT = viewModel.MIDPERCOUNT,
                    HEIPERCOUNT = viewModel.HEIPERCOUNT,
                    REGYTSTA = viewModel.REGYTSTA,
                    INSTRUMENTPRICE = viewModel.INSTRUMENTPRICE,
                    HOUSEAREA = viewModel.HOUSEAREA,
                    DETECTAREA = viewModel.DETECTAREA,
                    DETECTTYPE = viewModel.DETECTTYPE,
                    //DETECTNUM = viewModel.DETECTNUM,
                    APPLDATE = viewModel.APPLDATE,
                    DETECTPATH = viewModel.DETECTPATH,
                    QUAINFO = viewModel.QUAINFO,
                    APPROVALSTATUS = "0",
                    ADDDATE = viewModel.ADDDATE,
                    phone = viewModel.phone,
                    detectnumStartDate = viewModel.detectnumStartDate,
                    detectnumEndDate = viewModel.detectnumEndDate,
                    //measnumStartDate = viewModel.measnumStartDate,
                    //measnumEndDate = viewModel.measnumEndDate,
                    hasNumPerCount = viewModel.hasNumPerCount,
                    instrumentNum = viewModel.instrumentNum,
                    businessnumPath = viewModel.businessnumPath,
                    approveadvice = viewModel.approveadvice,
                    subunitnum = viewModel.subunitnum,
                    issubunit = viewModel.issubunit,
                    supunitcode = viewModel.supunitcode,
                    subunitdutyman = viewModel.subunitdutyman,
                    area = viewModel.area,
                    // detectunit = viewModel.detectunit,
                    // detectappldate = viewModel.detectappldate,
                    shebaopeoplenum = viewModel.shebaopeoplenum,
                    captial = viewModel.captial,
                    credit = viewModel.credit,
                    companytype = viewModel.companytype,
                    floorarea = viewModel.floorarea,
                    yearplanproduce = viewModel.yearplanproduce,
                    preyearproduce = viewModel.preyearproduce,
                    businesspermit = viewModel.businesspermit,
                    businesspermitpath = viewModel.businesspermitpath,
                    enterprisemanager = viewModel.enterprisemanager,
                    financeman = viewModel.financeman,
                    director = viewModel.director,
                    cerfgrade = viewModel.cerfgrade,
                    cerfno = viewModel.cerfno,
                    cerfnopath = viewModel.cerfnopath,
                    sslcmj = viewModel.sslcmj,
                    sslczk = viewModel.sslczk,
                    szssccnl = viewModel.szssccnl,
                    fmhccnl = viewModel.fmhccnl,
                    chlccnl = viewModel.chlccnl,
                    ytwjjccnl = viewModel.ytwjjccnl,
                    managercount = viewModel.managercount,
                    jsglcount = viewModel.jsglcount,
                    testcount = viewModel.testcount,
                    sysarea = viewModel.sysarea,
                    yharea = viewModel.yharea,
                    shebaopeoplelistpath = viewModel.shebaopeoplelistpath,
                    workercount = viewModel.workercount,
                    zgcount = viewModel.zgcount,
                    instrumentpath = viewModel.instrumentpath,
                    datatype = viewModel.datatype,
                    ispile = viewModel.ispile,
                    NETADDRESS = viewModel.NETADDRESS,
                    REGMONEYS = viewModel.REGMONEYS,
                    PERP = viewModel.PERP,
                    CMANUM = viewModel.CMANUM,
                    CMAUNIT = viewModel.CMAUNIT,
                    CMANUMCERF = viewModel.CMANUMCERF,
                    AVAILABILITYTIME = viewModel.AVAILABILITYTIME,
                    GMANAGER = viewModel.GMANAGER,
                    GFA = viewModel.GFA,
                    GFB = viewModel.GFB,
                    TMANAGER = viewModel.TMANAGER,
                    TFA = viewModel.TFA,
                    TFB = viewModel.TFB,
                    ALLMANS = viewModel.ALLMANS,
                    TMANS = viewModel.TMANS,
                    MLEVELS = viewModel.MLEVELS,
                    HLEVELS = viewModel.HLEVELS,
                    EQUIPMENTS = viewModel.EQUIPMENTS,
                    EQMONEYS = viewModel.EQMONEYS,
                    WORKAREA = viewModel.WORKAREA,
                    CMANUMENDDATE = viewModel.CMANUMENDDATE,
                    CMAENDDATE = viewModel.CMAENDDATE,
                    USEENDDATE = viewModel.USEENDDATE,
                    SELECTTEL = viewModel.SELECTTEL,
                    APPEALTEL = viewModel.APPEALTEL,
                    APPEALEMAIL = viewModel.APPEALEMAIL,
                    zzlbgs = viewModel.zzlbgs,
                    zzxmgs = viewModel.zzxmgs,
                    zzcsgs = viewModel.zzcsgs,
                    certCode = viewModel.certCode,
                    REGJGSTA = viewModel.REGJGSTA,
                    data_status = "2",
                    update_time = DateTime.Now
                };
                #endregion
                creatModel.CusAchievement = new List<t_bp_CusAchievement>();

                creatModel.CusPunish = new List<t_bp_CusPunish>();
                creatModel.CusAward = new List<t_bp_CusAward>();
                creatModel.CusChange = new List<t_bp_CusChange>();
                creatModel.CheckCustom = new List<t_bp_CheckCustom>();
                if (viewModel.cusachievement != null)
                {
                    foreach (var cusAchieeve in viewModel.cusachievement)
                    {
                        cusAchieeve.CustomId = viewModel.ID;
                        creatModel.CusAchievement.Add(cusAchieeve);
                    }
                }

                if (viewModel.cuspunish != null)
                {
                    foreach (var cusPunis in viewModel.cuspunish)
                    {
                        cusPunis.CustomId = viewModel.ID;
                        creatModel.CusPunish.Add(cusPunis);
                    }
                }

                if (viewModel.cusawards != null)
                {
                    foreach (var cusAwar in viewModel.cusawards)
                    {
                        cusAwar.CustomId = viewModel.ID;
                        creatModel.CusAward.Add(cusAwar);
                    }
                }

                if (viewModel.cuschange != null)
                {
                    foreach (var cusChan in viewModel.cuschange)
                    {
                        cusChan.CustomId = viewModel.ID;
                        creatModel.CusChange.Add(cusChan);
                    }
                }

                if (viewModel.checkcustom != null)
                {
                    foreach (var cusCheck in viewModel.checkcustom)
                    {
                        cusCheck.CustomId = viewModel.ID;
                        creatModel.CheckCustom.Add(cusCheck);
                    }
                }



                string erroMsg = string.Empty;
                if (!checkUnitService.EditCustomCheckQualify(creatModel, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("进行了机构信息修改操作，机构id为{0}".Fmt(viewModel.ID));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        //public ActionResult Export(CheckQualifySearchModel searchModel, int? fileFormat)
        //{
        //    var data = GetSearchResult(searchModel);

        //    // 改动2：创建导出类实例（而非 DhtmlxGrid），设置列标题
        //    bool xlsx = (fileFormat ?? 2007) == 2007;
        //    ExcelExporter ee = new ExcelExporter("检测机构管理", xlsx);
        //    ee.SetColumnTitles("序号, 机构名称, 检测证书编号, 计量证书编号, 法人, 联系电话,  状态");
        //    int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
        //    var compayTypes = sysDictServcie.GetDictsByKey("customStatus");
        //    for (int i = 0; i < data.Results.Count; i++)
        //    {
        //        var custom = data.Results[i];
        //        ExcelRow row = ee.AddRow();
        //        row.AddCell((pos + i + 1).ToString());
        //        row.AddCell("{0}({1})".Fmt(custom.NAME, custom.ID));
        //        row.AddCell(custom.DETECTNUM);
        //        row.AddCell(custom.MEASNUM);
        //        row.AddCell(custom.FR);
        //        row.AddCell(custom.TEL);
        //        row.AddCell(SysDictUtility.GetKeyFromDic(compayTypes, custom.APPROVALSTATUS));
        //    }

        //    // 改动4：返回字节流
        //    return File(ee.GetAsBytes(), ee.MIME, ee.FileName);
        //}

        [HttpPost]
        public ActionResult SetInstFormal(string customid, string formalCustomId)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errMsg = string.Empty;
            if (!checkUnitService.SetInstFormal(customid, formalCustomId, out errMsg))
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errMsg;
            }
            else
            {
                LogUserAction("将机构编号为{0}的临时机构转换为正式机构,正式机构编号为{1}".Fmt(customid, formalCustomId));
                LogUserAction("将机构编号为{0}的人员，设备信息的机构编号更新为{1}".Fmt(customid, formalCustomId));
            }
            return Content(result.ToJson());
        }

        public ActionResult setInstFormal(string customid)
        {
            ViewBag.Id = customid;
            return View();
        }

        [HttpPost]
        public string GetWjlr(string customId)
        {
            string result = string.Empty;
            var custom = rep.GetById(customId);
            if (custom != null)
            {
                result = custom.wjlr;
            }
            return result;//Content(result);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult UpdateWjlrAndZzlbmc(string wjlr, string zzlbmc, string customid)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errMsg = string.Empty;
            if (customid.IsNullOrEmpty())
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = "机构编号不能为空";
                return Content(result.ToJson());
            }
            if (!checkUnitService.UpdateWjlrAndZzlbmc(wjlr, zzlbmc, customid, out errMsg))
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errMsg;
            }
            else
            {
                LogUserAction("进行了更新机构id为{0}的资质信息".Fmt(customid));
            }
            return Content(result.ToJson());
        }

        public ActionResult GetTable(AddtableModel model)
        {
            int dbCount = 0;
            var predicate = PredicateBuilder.True<t_bp_People>();
            predicate = predicate.And(tp => tp.data_status == null || tp.data_status != "-1");
            if (!string.IsNullOrEmpty(model.CheckUnitName))
            {
                predicate = predicate.And(t => t.Customid == model.CheckUnitName);
            }
            if (!string.IsNullOrEmpty(model.HasCert) && model.HasCert != "null")
            {
                if (model.HasCert == "1")
                { predicate = predicate.And(t => t.PostNum != null); }
                else if (model.HasCert == "0")
                { predicate = predicate.And(t => t.PostNum == null || t.PostNum == ""); }
            }
            if (!string.IsNullOrEmpty(model.PeopleStatus) && model.PeopleStatus != "null")
            {
                predicate = predicate.And(t => t.iscb == model.PeopleStatus);
            }
            if (!string.IsNullOrEmpty(model.TechTitle) && model.TechTitle != "null")
            {

                if (model.TechTitle == "6")
                { predicate = predicate.And(t => t.Title == "4" || t.Title == "2" || t.Title == "3"); }
                else
                { predicate = predicate.And(t => t.Title == model.TechTitle); }
            }

            if (!string.IsNullOrEmpty(model.IsTech) && model.IsTech != "null")
            {

                predicate = predicate.And(t => t.iszcgccs == model.IsTech);
            }

            dbCount = (int)peopleRep.GetCountByCondtion(predicate);



            var peoples = peopleRep.GetByConditionSort<CheckPeopleUIModel>(predicate,
                p => new
                {
                    p.id,
                    p.Name,
                    p.Customid,
                    p.SelfNum,
                    p.PostNum,
                    p.zw,
                    p.Title,
                    p.iscb,
                    p.Approvalstatus,
                    p.PostDate,
                    p.IsUse
                }, null);

            var instDicts = checkUnitService.GetAllCheckUnit();
            var personnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");
            var workStatus = sysDictServcie.GetDictsByKey("workStatus");
            var personnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");
            AddTablePostModle viewModel = new AddTablePostModle();
            var CheckPeopl = new List<CheckPeoplModel>();
            var number = 1;
            LayUIGrid Grid = new LayUIGrid();
            foreach (var item in peoples)
            {
                var checkPeopl = new CheckPeoplModel()
                {
                    number = number++,
                    id = item.id,
                    Name = item.Name,
                    CustomName = checkUnitService.GetCheckUnitByIdFromAll(instDicts, item.Customid),
                    SelfNum = item.SelfNum,
                    zw = item.zw,
                    Title = SysDictUtility.GetKeyFromDic(personnelTitles, item.Title),
                    iscb = SysDictUtility.GetKeyFromDic(workStatus, item.iscb),
                    Approvalstatus = SysDictUtility.GetKeyFromDic(personnelStatus, item.Approvalstatus),
                    postdate = item.postdate,
                    IsUse = item.IsUse
                };
                CheckPeopl.Add(checkPeopl);
            }
            viewModel.Count = dbCount;
            viewModel.CheckPeopl = CheckPeopl.ToJson();
            return View(viewModel);
        }

        public ActionResult GetSuperJob(string Id)
        {
            SupvisorJob Result = new SupvisorJob();
            var model = supvisorRep.GetByCondition(w => w.CustomId == Id && w.NeedApproveId == Id && w.ApproveType == ApproveType.ApproveCustom).OrderByDescending(o => o.CreateTime);
            //if (SupvisorJobs != null && SupvisorJobs.Count() > 0)
            //{
            //    Result = SupvisorJobs.FirstOrDefault();
            //}
            return View(model.FirstOrDefault());// Content(Result.ToJson());

        }

        [HttpPost]
        public ActionResult ConfirmApplyChange(CheckCustomApplyChange applyChangeModel)
        {
            var Status = applyChangeModel.Status == "yes" ? "0" : "7";
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                result.IsSucc = true;
                if (!checkUnitService.UpdateCustomStatus(applyChangeModel.SubmitId, Status, "", out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = false;
                }
                else
                {
                    LogUserAction("进行了审核申请修改操作");
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
                result.IsSucc = true;
            }

            return Content(result.ToJson());
        }





        public ActionResult AddQualifyOne()
        {
            var model = new ApplyQualifyOneViewModel();
            model.Region = userService.GetAllArea();
            var custom = GetCurrentCustomInfo();
            if (custom != null)
            {
                model.UnitCode = custom.ID;
                model.UnitName = custom.NAME;
                model.Name = custom.FR;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult AddQualifyOne(ApplyQualifySaveModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;

            try
            {
                model.Type = "2";
                string errorMsg = string.Empty;
                if (!checkQualifyService.SaveApplyQualifyOne(model, out errorMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = errorMsg;
                }
                else
                {
                    LogUserAction("创建了新的检测机构法定代表人声明{0}[{1}]".Fmt(model.UnitName, model.Name));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        public ActionResult EditQualifyOne(int id)
        {
            var model = new ApplyQualifyOneViewModel();
            model.Region = userService.GetAllArea();
            var oneQualifyModel = repOne.GetById(id);
            if (oneQualifyModel != null)
            {
                model.Id = oneQualifyModel.id;
                model.UnitCode = oneQualifyModel.unitCode;
                model.UnitName = oneQualifyModel.unitname;
                model.Name = oneQualifyModel.name;
                model.Area = oneQualifyModel.area;
                model.Selfnum = oneQualifyModel.selfnum;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EditQualifyOne(ApplyQualifySaveModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;

            try
            {
                string errorMsg = string.Empty;
                if (!checkQualifyService.EditApplyQualifyOne(model, out errorMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = errorMsg;
                }
                else
                {
                    LogUserAction("更新了新的检测机构法定代表人声明{0}[{1}]".Fmt(model.UnitName, model.Name));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }


        public ActionResult DetailQualifyOne(int id)
        {
            var model = new ApplyQualifyOneViewModel();
            model.Region = userService.GetAllArea();
            var oneQualifyModel = repOne.GetById(id);
            if (oneQualifyModel != null)
            {
                model.Id = oneQualifyModel.id;
                model.UnitCode = oneQualifyModel.unitCode;
                model.UnitName = oneQualifyModel.unitname;
                model.Name = oneQualifyModel.name;
                model.Area = oneQualifyModel.area;
                model.Selfnum = oneQualifyModel.selfnum;
            }

            return View(model);
        }

        public ActionResult DetailQualifyFive(string id)
        {
            ViewBag.Id = id;
            return View();
        }


        public ActionResult EditQualifyFive(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult DetailQualifyFiveSearch(DetailQualifyFiveSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            PagingOptions<t_D_UserTableFive> pagingOption = new PagingOptions<t_D_UserTableFive>(pos, count, t => new { t.id });
            DhtmlxGrid grid = new DhtmlxGrid();
            var userTableFive = repFive.GetByConditonPage(r => r.pid == model.pid, pagingOption);
            grid.AddPaging(pagingOption.TotalItems, pos);
            int index = pos + 1;
            foreach (var item in userTableFive)
            {

                var data = JsonConvert.DeserializeObject<DetailQualifyFiveSearchViewModel>(item.gzjl);
                DhtmlxGridRow row = new DhtmlxGridRow(index);
                row.AddCell(index);
                row.AddCell(data.jclb);
                row.AddCell(data.jclr);
                row.AddCell(data.name);
                row.AddCell(data.zgzsh);
                row.AddCell(data.sgzsh);
                row.AddCell(data.zcdwmc);
                row.AddCell(data.zc);
                row.AddCell(data.zy);
                row.AddCell(data.xl);
                row.AddCell(data.jcnx);
                row.AddCell(data.detectnumstartdate + "至" + data.detectnumenddate);
                row.AddCell(data.bz);
                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                index++;
                grid.AddGridRow(row);


            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult EditQualifyFiveSearch(DetailQualifyFiveSearchModel model)
        {
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int count = model.count.HasValue ? model.count.Value : 30;
            PagingOptions<t_D_UserTableFive> pagingOption = new PagingOptions<t_D_UserTableFive>(pos, count, t => new { t.id });
            DhtmlxGrid grid = new DhtmlxGrid();
            var userTableFive = repFive.GetByConditonPage(r => r.pid == model.pid, pagingOption);
            grid.AddPaging(pagingOption.TotalItems, pos);
            int index = pos + 1;
            foreach (var item in userTableFive)
            {
                // var gzjls = item.gzjl.Split(',');

                // foreach (var gzjl in gzjls)
                //  {
                var data = JsonConvert.DeserializeObject<DetailQualifyFiveSearchViewModel>(item.gzjl);
                DhtmlxGridRow row = new DhtmlxGridRow(item.id);
                row.AddCell(index);
                row.AddCell(data.jclb);
                row.AddCell(data.jclr);
                row.AddCell(data.name);
                row.AddCell(data.zgzsh);
                row.AddCell(data.sgzsh);
                row.AddCell(data.zcdwmc);
                row.AddCell(data.zc);
                row.AddCell(data.zy);
                row.AddCell(data.xl);
                row.AddCell(data.jcnx);
                row.AddCell(data.detectnumstartdate + "至" + data.detectnumenddate);
                row.AddCell(data.bz);
                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                row.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                row.AddCell(new DhtmlxGridCell("删除", false).AddCellAttribute("title", "删除"));
                index++;
                grid.AddGridRow(row);
                //  }

            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }


        public ActionResult CreateQualifyFive(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ActionResult CreateQualifyFiveSearch(DetailQualifyFiveSearchModel model)
        {
            var instDicts = checkUnitService.GetAllCheckUnit();
            var workStatus = sysDictServcie.GetDictsByKey("workStatus");
            var personnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");
            var personnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");
            var unitcode = repOne.GetById(model.pid);
            var peoples = peopleRep.GetByCondition(r => r.Customid == unitcode.unitCode);
            DhtmlxGrid grid = new DhtmlxGrid();
            string customname = string.Empty;
            int pos = model.posStart.HasValue ? model.posStart.Value : 0;
            int index = pos;
            foreach (var item in peoples)
            {
                customname = checkUnitService.GetCheckUnitByIdFromAll(instDicts, item.Customid);
                DhtmlxGridRow row = new DhtmlxGridRow(item.id.ToString());
                row.AddCell("");
                row.AddCell((++index).ToString());
                row.AddCell(item.Name);
                row.AddCell(customname);
                row.AddCell(item.SelfNum);
                row.AddCell(item.PostNum);
                row.AddCell(item.zw);
                row.AddCell(SysDictUtility.GetKeyFromDic(personnelTitles, item.Title));
                row.AddCell(SysDictUtility.GetKeyFromDic(workStatus, item.iscb));
                row.AddCell(SysDictUtility.GetKeyFromDic(personnelStatus, item.Approvalstatus));
                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult CreateSeave(string selectedId, string pid, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            var selectids = selectedId.Split(',').ToList();
            var peoples = peopleRep.GetByCondition(r => selectids.Contains(r.id.ToString()));
            var insertids = new List<long>();
            try
            {
                foreach (var item in peoples)
                {
                    DetailQualifyFiveSearchViewModel model = new DetailQualifyFiveSearchViewModel()
                    {
                        name = item.Name,
                        zgzsh = item.zcgccszh,
                        zgzshpath = item.zcgccszhpath,
                        zc = item.Title,
                        zcpath = item.titlepath,
                        xl = item.Education,
                        xlpath = item.educationpath,
                        zy = item.Professional,
                        sgzsh = item.PostNum,
                        sgzshpath = item.PostPath,
                    };
                    t_D_UserTableFive usertablefive = new t_D_UserTableFive()
                    {
                        gzjl = model.ToJson(),
                        pid = pid,
                        staitc = 0,
                        unitcode = repOne.GetById(pid).unitCode,
                    };
                    insertids.Add(repFive.Insert(usertablefive));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
                foreach (var item in insertids)
                {
                    repFive.DeleteById(item);
                }
            }
            return Content(result.ToJson());
        }


        public ActionResult DeteleFive(string id)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                repFive.DeleteById(id);
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }
            return Content(result.ToJson());
        }

        [HttpPost]
        public ActionResult QualifyFivePeopleEdit(UserTableFiveModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            DateTime? detectnumstartdate = null, detectnumenddate = null;
            CommonUtils.GetLayuiDateRange(model.detectnumstartdate, out detectnumstartdate, out detectnumenddate);
            DetailQualifyFiveSearchViewModel str = new DetailQualifyFiveSearchViewModel()
            {
                name = model.name,
                zgzsh = model.zgzsh,
                zgzshpath = model.zgzshpath,
                zcdwmc = model.zcdwmc,
                zc = model.zc,
                zy = model.zy,
                xl = model.xl,
                jcnx = model.jcnx,
                jclb = model.jclb,
                jclr = model.jclr,
                xlpath = model.xlpath,
                zcpath = model.zcpath,
                bz = model.bz,
                detectnumenddate = detectnumenddate.HasValue ? detectnumenddate.Value.ToString("yyyy-MM-dd") : string.Empty,
                detectnumstartdate = detectnumstartdate.HasValue ? detectnumstartdate.Value.ToString("yyyy-MM-dd") : string.Empty,
                sgzsh = model.sgzsh,
                sgzshpath = model.sgzshpath
            };
            t_D_UserTableFive five = new t_D_UserTableFive()
            {
                id = model.id,
                pid = model.pid,
                unitcode = model.unitcode,
                staitc = model.Staitc,
                gzjl = str.ToJson()
            };
            try
            {
                repFive.Update(five);
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }
            return Content(result.ToJson());
        }


        public ActionResult QualifyFivePeopleDetalify(string id)
        {
            UserTableFiveModel model = new UserTableFiveModel();
            var data = repFive.GetById(id);
            if (data != null)
            {
                var gzjl = JsonConvert.DeserializeObject<DetailQualifyFiveSearchViewModel>(data.gzjl);
                model.zc = gzjl.zc;
                model.name = gzjl.name;
                model.zgzsh = gzjl.zgzsh;
                model.zgzshpath = gzjl.zgzshpath;
                model.zcdwmc = gzjl.zcdwmc;
                model.zy = gzjl.zy;
                model.xl = gzjl.xl;
                model.xlpath = gzjl.xlpath;
                model.jcnx = gzjl.jcnx;
                model.jclb = gzjl.jclb;
                model.jclr = gzjl.jclr;
                model.zcpath = gzjl.zcpath;
                model.bz = gzjl.bz;
                model.detectnumenddate = gzjl.detectnumenddate;
                model.detectnumstartdate = gzjl.detectnumstartdate;
                model.sgzsh = gzjl.sgzsh;
                model.sgzshpath = gzjl.sgzshpath;
                model.id = data.id;
                model.Staitc = data.staitc;
                model.pid = data.pid;
                model.unitcode = data.unitcode;
            }
            model.file = ApplyQualifyFileModel(model);
            return View(model);
        }

        public ActionResult QualifyFivePeopleEdit(string id)
        {
            UserTableFiveModel model = new UserTableFiveModel();
            var data = repFive.GetById(id);
            if (data != null)
            {
                var gzjl = JsonConvert.DeserializeObject<DetailQualifyFiveSearchViewModel>(data.gzjl);
                model.zc = gzjl.zc;
                model.name = gzjl.name;
                model.zgzsh = gzjl.zgzsh;
                model.zgzshpath = gzjl.zgzshpath;
                model.zcdwmc = gzjl.zcdwmc;
                model.zy = gzjl.zy;
                model.xl = gzjl.xl;
                model.xlpath = gzjl.xlpath;
                model.jcnx = gzjl.jcnx;
                model.jclb = gzjl.jclb;
                model.jclr = gzjl.jclr;
                model.zcpath = gzjl.zcpath;
                model.bz = gzjl.bz;
                model.detectnumenddate = gzjl.detectnumenddate;
                model.detectnumstartdate = gzjl.detectnumstartdate;
                model.sgzsh = gzjl.sgzsh;
                model.sgzshpath = gzjl.sgzshpath;
                model.id = data.id;
                model.Staitc = data.staitc;
                model.pid = data.pid;
                model.unitcode = data.unitcode;
            }
            model.file = ApplyQualifyFileModel(model);
            return View(model);
        }


        private List<ApplyQualifyFileModel> ApplyQualifyFileModel(UserTableFiveModel model)
        {
            string[,] fileRangAr = new string[4, 2];
            fileRangAr[0, 0] = "注册资格证书号";
            fileRangAr[0, 1] = model.zgzshpath;
            fileRangAr[1, 0] = "上岗资格证书号";
            fileRangAr[1, 1] = model.sgzshpath;
            fileRangAr[2, 0] = "职称";
            fileRangAr[2, 1] = model.zcpath;
            fileRangAr[3, 0] = "学历";
            fileRangAr[3, 1] = model.xlpath;

            List<ApplyQualifyFileModel> ApplyQualifyFiles = new List<Models.ApplyQualifyFileModel>();
            for (int i = 0; i < 4; i++)
            {
                ApplyQualifyFileModel ApplyQualifyFile = new Models.ApplyQualifyFileModel();

                ApplyQualifyFile.FileName = fileRangAr[i, 0];
                ApplyQualifyFile.Number = i;
                ApplyQualifyFile.FileState = string.IsNullOrEmpty(fileRangAr[i, 1]) ? "未上传" : "已上传";
                ApplyQualifyFile.Path = ApplyQualifyFile.FileState == "已上传" ? fileRangAr[i, 1] : string.Empty;
                ApplyQualifyFiles.Add(ApplyQualifyFile);
            }
            return ApplyQualifyFiles;
        }






        public ActionResult DetailsAttachFile(string filePath, string Name)
        {
            DetailsAttachFileModel model = new DetailsAttachFileModel();
            if (!filePath.IsNullOrEmpty())
            {
                var index = filePath.LastIndexOf('|');
                if (index == filePath.Length - 1)
                {
                    filePath = filePath.Substring(0, filePath.Length - 1);
                }

                model.Paths = filePath.Split('|');
            }
            model.Name = Name;
            return View(model);
        }

        private void GetFj(DetailQualifyThreeVeiwModel model)
        {
            string[,] FileRangeAr = new string[4, 3];
            FileRangeAr[0, 0] = "职称附件";
            FileRangeAr[1, 0] = "学历附件";
            FileRangeAr[2, 0] = "照片附件";
            FileRangeAr[3, 0] = "岗位证书附件";
            FileRangeAr[0, 1] = model.zcpath;
            FileRangeAr[1, 1] = model.xlpath;
            FileRangeAr[2, 1] = model.photopath;
            FileRangeAr[3, 1] = model.postNumpath;
            FileRangeAr[0, 2] = model.zcpath.IsNullOrEmpty() ? "未上传" : "已上传";
            FileRangeAr[1, 2] = model.xlpath.IsNullOrEmpty() ? "未上传" : "已上传";
            FileRangeAr[2, 2] = model.photopath.IsNullOrEmpty() ? "未上传" : "已上传";
            FileRangeAr[3, 2] = model.postNumpath.IsNullOrEmpty() ? "未上传" : "已上传";
            for (int i = 0; i < 4; i++)
            {
                var Model = new DetailQualifyThreeVeiwPhotoModel();
                Model.name = FileRangeAr[i, 0];
                Model.path = FileRangeAr[i, 1];
                Model.state = FileRangeAr[i, 2];
                model.Fj.Add(Model);
            }
        }

        private List<DetailQualifyThreeVeiwDataModel> GetGzjl(string gzjl)
        {
            var Model = new List<DetailQualifyThreeVeiwDataModel>();
            if (!gzjl.IsNullOrEmpty())
            {
                var gzjl1 = gzjl.Split('|');
                for (int i = 0; i < gzjl1.Count(); i++)
                {
                    DetailQualifyThreeVeiwDataModel date = new DetailQualifyThreeVeiwDataModel();
                    var item = gzjl1[i].Split('&');
                    if (item.Count() == 1)
                    {
                        date.Date = item[0];
                        date.Type = string.Empty;
                    }
                    else
                    {
                        date.Date = item[0];
                        date.Type = item[1];
                    }
                    Model.Add(date);
                }
            }
            return Model;
        }

        public ActionResult DetailQualifyThree(string pid, string customid)
        {
            var DetailQualifyThree = repThree.GetByCondition(p => p.pid == pid);
            DetailQualifyThreeVeiwModel model = new DetailQualifyThreeVeiwModel()
            {
                Gzjl = new List<DetailQualifyThreeVeiwDataModel>(),
                Fj = new List<DetailQualifyThreeVeiwPhotoModel>()
            };

            if (DetailQualifyThree != null && DetailQualifyThree.Count() > 0)
            {
                model.name = DetailQualifyThree.First().name;
                model.sex = DetailQualifyThree.First().sex;
                model.time = DetailQualifyThree.First().time;
                model.zw = DetailQualifyThree.First().zw;
                model.zc = DetailQualifyThree.First().zc;
                model.xl = DetailQualifyThree.First().xl;
                model.hshxhzy = DetailQualifyThree.First().hshxhzy;
                model.postNum = DetailQualifyThree.First().postNum;
                model.jcgzlx = DetailQualifyThree.First().jcgzlx;
                model.bgdh = DetailQualifyThree.First().bgdh;
                model.yddh = DetailQualifyThree.First().yddh;
                model.xlpath = DetailQualifyThree.First().xlpath;
                model.postNumpath = DetailQualifyThree.First().postNumpath;
                model.zcpath = DetailQualifyThree.First().zcpath;
                model.gzjl = DetailQualifyThree.First().gzjl;
                model.Gzjl = GetGzjl(model.gzjl);
            }
            GetFj(model);
            return View(model);
        }

        public ActionResult DetailQualifyFour(string pid, string customid)
        {
            var DetailQualifyThree = repFour.GetByCondition(p => p.pid == pid);
            DetailQualifyThreeVeiwModel model = new DetailQualifyThreeVeiwModel()
            {
                Gzjl = new List<DetailQualifyThreeVeiwDataModel>(),
                Fj = new List<DetailQualifyThreeVeiwPhotoModel>()
            };
            if (DetailQualifyThree != null && DetailQualifyThree.Count() > 0)
            {
                model.name = DetailQualifyThree.First().name;
                model.sex = DetailQualifyThree.First().sex;
                model.time = DetailQualifyThree.First().time;
                model.zw = DetailQualifyThree.First().zw;
                model.zc = DetailQualifyThree.First().zc;
                model.xl = DetailQualifyThree.First().xl;
                model.hshxhzy = DetailQualifyThree.First().hshxhzy;
                model.postNum = DetailQualifyThree.First().postNum;
                model.jcgzlx = DetailQualifyThree.First().jcgzlx;
                model.bgdh = DetailQualifyThree.First().bgdh;
                model.yddh = DetailQualifyThree.First().yddh;
                model.xlpath = DetailQualifyThree.First().xlpath;
                model.postNumpath = DetailQualifyThree.First().postNumpath;
                model.zcpath = DetailQualifyThree.First().zcpath;
                model.gzjl = DetailQualifyThree.First().gzjl;
                model.Gzjl = GetGzjl(model.gzjl);
            }
            GetFj(model);
            return View(model);
        }



        private void GetFjThree(EditQualifyThreeVeiwModel model)
        {
            string[,] FileRangeAr = new string[4, 4];
            FileRangeAr[0, 0] = "职称附件";
            FileRangeAr[1, 0] = "学历附件";
            FileRangeAr[2, 0] = "照片附件";
            FileRangeAr[3, 0] = "岗位证书附件";

            FileRangeAr[0, 1] = model.zcpath;
            FileRangeAr[1, 1] = model.xlpath;
            FileRangeAr[2, 1] = model.photopath;
            FileRangeAr[3, 1] = model.postNumpath;
            FileRangeAr[0, 2] = model.zcpath.IsNullOrEmpty() ? "未上传" : "已上传";
            FileRangeAr[1, 2] = model.xlpath.IsNullOrEmpty() ? "未上传" : "已上传";
            FileRangeAr[2, 2] = model.photopath.IsNullOrEmpty() ? "未上传" : "已上传";
            FileRangeAr[3, 2] = model.postNumpath.IsNullOrEmpty() ? "未上传" : "已上传";
            FileRangeAr[0, 3] = "zcpath";
            FileRangeAr[1, 3] = "xlpath";
            FileRangeAr[2, 3] = "photopath";
            FileRangeAr[3, 3] = "postNumpath";
            for (int i = 0; i < 4; i++)
            {
                var Model = new FileGridModel();
                Model.Name = FileRangeAr[i, 0];
                Model.Url = FileRangeAr[i, 1];
                Model.State = FileRangeAr[i, 2];
                Model.Type = FileRangeAr[i, 3];
                Model.Id = i.ToString();
                model.File.Add(Model);
            }
        }

        private void GetSex(EditQualifyThreeVeiwModel model)
        {
            string[] FileRangeAr = new string[2];
            FileRangeAr[0] = "女";
            FileRangeAr[1] = "男";
            for (int i = 0; i < 2; i++)
            {
                var sex = new DetailQualifyThreeVeiwDataModel();
                sex.Date = FileRangeAr[i];
                model.Sex.Add(sex);
            }
        }

        public ActionResult EditQualifyThree(string pid, string customid)
        {

            var EditQualifyThree = repThree.GetByCondition(p => p.pid == pid);
            EditQualifyThreeVeiwModel model = new EditQualifyThreeVeiwModel()
            {
                PersonnelStaff = new List<SysDict>(),
                File = new List<FileGridModel>(),
                CompayTypeList = new Dictionary<string, List<SysDict>>(),
                Sex = new List<DetailQualifyThreeVeiwDataModel>()
            };
            if (EditQualifyThree.Count > 0)
            {
                model.name = EditQualifyThree.First().name;
                model.sex = EditQualifyThree.First().sex;
                model.time = EditQualifyThree.First().time;
                model.zw = EditQualifyThree.First().zw;
                model.zc = EditQualifyThree.First().zc;
                model.xl = EditQualifyThree.First().xl;
                model.hshxhzy = EditQualifyThree.First().hshxhzy;
                model.postNum = EditQualifyThree.First().postNum;
                model.jcgzlx = EditQualifyThree.First().jcgzlx;
                model.bgdh = EditQualifyThree.First().bgdh;
                model.yddh = EditQualifyThree.First().yddh;
                model.xlpath = EditQualifyThree.First().xlpath;
                model.postNumpath = EditQualifyThree.First().postNumpath;
                model.photopath = EditQualifyThree.First().photopath;
                model.xlpath = EditQualifyThree.First().xlpath;
                model.zcpath = EditQualifyThree.First().zcpath;
                model.gzjl = EditQualifyThree.First().gzjl;
                model.Gzjl = GetGzjl(model.gzjl);

            }
            GetFjThree(model);
            GetSex(model);
            model.pid = pid;
            model.PersonnelStaff = sysDictServcie.GetDictsByKey("PersonnelStaff");
            model.CompayTypeList.Add("personnelTitles", sysDictServcie.GetDictsByKey("personnelTitles"));
            return View(model);
        }

        [HttpPost]
        public ActionResult EditQualifyThreee(EditQualifyThreeSaveMdoel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            if (model.gzjls != null)
            {
                model.gzjl = model.gzjls.Select(r => r.WorkExperienceTime + "&" + r.WorkExperienceContent).ToList().Join("|");
            }
            t_D_UserTableThree Model = new t_D_UserTableThree()
            {
                name = model.name,
                sex = model.sex,
                time = model.time,
                hshxhzy = model.hshxhzy,
                postNum = model.postNum,
                zw = model.zw,
                zc = model.zc,
                xl = model.xl,
                zcpath = model.zcpath,
                xlpath = model.xlpath,
                photopath = model.photopath,
                postNumpath = model.postNumpath,
                pid = model.pid,
                gzjl = model.gzjl,
                yddh = model.yddh,
                bgdh = model.bgdh,
                jcgzlx = model.jcgzlx
            };
            try
            {
                string erroMsg = string.Empty;
                if (!checkQualifyService.EditQualifyThree(Model, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("进行了机构法人信息修改操作，机构id为{0}".Fmt(model.pid));
                }

            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }
            return Content(result.ToJson());
        }




        public ActionResult EditQualifyFour(string pid, string customid)
        {
            var EditQualifyFour = repFour.GetByCondition(p => p.pid == pid);
            EditQualifyThreeVeiwModel model = new EditQualifyThreeVeiwModel()
            {
                PersonnelStaff = new List<SysDict>(),
                File = new List<FileGridModel>(),
                CompayTypeList = new Dictionary<string, List<SysDict>>(),
                Sex = new List<DetailQualifyThreeVeiwDataModel>()
            };
            if (EditQualifyFour.Count > 0)
            {
                model.name = EditQualifyFour.First().name;
                model.sex = EditQualifyFour.First().sex;
                model.time = EditQualifyFour.First().time;
                model.zw = EditQualifyFour.First().zw;
                model.zc = EditQualifyFour.First().zc;
                model.xl = EditQualifyFour.First().xl;
                model.hshxhzy = EditQualifyFour.First().hshxhzy;
                model.postNum = EditQualifyFour.First().postNum;
                model.jcgzlx = EditQualifyFour.First().jcgzlx;
                model.bgdh = EditQualifyFour.First().bgdh;
                model.yddh = EditQualifyFour.First().yddh;
                model.xlpath = EditQualifyFour.First().xlpath;
                model.postNumpath = EditQualifyFour.First().postNumpath;
                model.zcpath = EditQualifyFour.First().zcpath;
                model.gzjl = EditQualifyFour.First().gzjl;
                model.Gzjl = GetGzjl(model.gzjl);
            }
            model.pid = pid;
            GetFjThree(model);
            GetSex(model);
            model.PersonnelStaff = sysDictServcie.GetDictsByKey("PersonnelStaff");
            model.CompayTypeList.Add("personnelTitles", sysDictServcie.GetDictsByKey("personnelTitles"));
            return View(model);
        }

        [HttpPost]
        public ActionResult EditQualifyFours(EditQualifyThreeSaveMdoel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            if (model.gzjls != null)
            {
                model.gzjl = model.gzjls.Select(r => r.WorkExperienceTime + "&" + r.WorkExperienceContent).ToList().Join("|");
            }
            t_D_UserTableFour save = new t_D_UserTableFour()
            {
                name = model.name,
                sex = model.sex,
                time = model.time,
                hshxhzy = model.hshxhzy,
                postNum = model.postNum,
                zw = model.zw,
                zc = model.zc,
                xl = model.xl,
                zcpath = model.zcpath,
                xlpath = model.xlpath,
                photopath = model.photopath,
                postNumpath = model.postNumpath,
                pid = model.pid,
                gzjl = model.gzjl,
                jcgzlx = model.jcgzlx,
                yddh = model.yddh,
                bgdh = model.bgdh
            };
            try
            {
                string erroMsg = string.Empty;
                if (!checkQualifyService.EditQualifyFours(save, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("进行了机构技术人员信息修改操作，机构id为{0}".Fmt(model.pid));
                }

            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }
            return Content(result.ToJson());
        }

    }

}
