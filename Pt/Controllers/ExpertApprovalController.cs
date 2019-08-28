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
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using Pkpm.Core.CheckQualifyCore;

namespace PkpmGX.Controllers
{


    public class ExpertApprovalController : PkpmController
    {

        ISysDictService sysDictServcie;
        IPkpmConfigService pkpmConfigService;
        ICheckUnitService checkUnitService;
        ICheckQualifyService checkQualifyService;
        IRepsitory<t_D_UserTableOne> repOne;
        IRepsitory<t_D_UserTableTwo> repTwo;
        IRepsitory<t_D_UserExpertUnit> repExp;
        IRepsitory<t_D_userTableSH> repSH;
        IRepsitory<t_D_userTableSC> repSC;
        IRepsitory<t_bp_custom> repCustom;
        IFileHandler fileHander;

        public ExpertApprovalController(ISysDictService sysDictServcie,
            IPkpmConfigService pkpmConfigService,
            ICheckUnitService checkUnitService,
            ICheckQualifyService checkQualifyService,
            IRepsitory<t_D_UserTableOne> repOne,
            IRepsitory<t_D_UserExpertUnit> repExp,
            IRepsitory<t_D_UserTableTwo> repTwo,
            IRepsitory<t_bp_custom> repCustom,
            IRepsitory<t_D_userTableSH> repSH,
            IRepsitory<t_D_userTableSC> repSC,
            IFileHandler fileHander,
        IUserService userService) : base(userService)
        {
            this.repOne = repOne;
            this.repExp = repExp;
            this.repTwo = repTwo;
            this.repSH = repSH;
            this.repSC = repSC;
            this.repCustom = repCustom;
            this.fileHander = fileHander;
            this.sysDictServcie = sysDictServcie;
            this.pkpmConfigService = pkpmConfigService;
            this.checkUnitService = checkUnitService;
            this.checkQualifyService = checkQualifyService;
            this.userService = userService;
        }


        // GET: ExpertApproval
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Search(ApplyQualifySearchModel searchModel)
        {
            var data = GetSearchResult(searchModel);

            DhtmlxGrid grid = new DhtmlxGrid();
            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            grid.AddPaging(data.TotalCount, pos);
            var buttons = GetCurrentUserPathActions();
            var currentUserRole = GetCurrentUserRole();

            for (int i = 0; i < data.Results.Count; i++)
            {
                var custom = data.Results[i];
                DhtmlxGridRow row = new DhtmlxGridRow(custom.ID.ToString());
                row.AddCell(string.Empty);
                row.AddCell((pos + i + 1).ToString());//序号

                row.AddLinkJsCell(custom.UNITNAME + "/" + custom.NAME, "DetailQualifyOne(\"{0}\")".Fmt(custom.ID));
                row.AddLinkJsCell("[查看]", "DetailQualifyTwo(\"{0}\",\"{1}\")".Fmt(custom.ID, custom.UNITCODE)); //机构基本情况表
                row.AddLinkJsCell("[查看]", "DetailQualifyThree(\"{0}\",\"{1}\")".Fmt(custom.ID, custom.UNITCODE)); //法定代表人基本情况
                row.AddLinkJsCell("[查看]", "DetailQualifyFour(\"{0}\",\"{1}\")".Fmt(custom.ID, custom.UNITCODE)); //技术负责人基本情况
                row.AddLinkJsCell("[查看]", "DetailQualifyFive(\"{0}\")".Fmt(custom.ID)); //检测类别、内容、具备相应注册工程师资格人员情况
                row.AddLinkJsCell("[查看]", "DetailQualifySix(\"{0}\")".Fmt(custom.ID)); //专业技术人员情况总表
                row.AddLinkJsCell("[查看]", "DetailQualifySeven(\"{0}\",\"{1}\")".Fmt(custom.ID, custom.UNITCODE)); //主要仪器设备（检测项目）及其检定/校准一览表

                row.AddCell(custom.Type == "0" ? "新申请" : (custom.Type == "1" ? "资质增项" : "资质延续")); //申请类型

                //对机构进行修改后，进行审核操作
                if (HaveButtonFromAll(buttons, "Approval"))
                {
                    var ueu = repExp.GetByCondition(r => r.pid == Convert.ToInt32(custom.ID) && r.userid == GetCurrentUserId()).FirstOrDefault();
                    if(ueu != null && ueu.needUnitBuildingQualify == 1)
                    {
                        if (ueu.qualifystatus != 1)
                        {
                            row.AddLinkJsCell("[建设工程质量检测机构资质审核表]", "ApprovalUnitBuildingQualify(\"{0}\")".Fmt(custom.ID));
                        }
                        else
                        {
                            row.AddLinkJsCell("[建设工程质量检测机构资质审核表-已提交]", "DetailUnitBuildingQualify(\"{0}\",\"{1}\")".Fmt(ueu.shid, custom.ID));
                        }
                    }
                    else
                    {
                        row.AddCell(string.Empty);
                    }

                    if (ueu != null && ueu.needSpecialQualify == 1)
                    {
                        if (ueu.speicalstatus != 1)
                        {
                            row.AddLinkJsCell("[专项检测备案审核表]", "ApprovalSpecialQualify(\"{0}\")".Fmt(custom.ID));
                        }
                        else
                        {
                            row.AddLinkJsCell("[专项检测备案审核表-已提交]", "DetailSpecialQualify(\"{0}\",\"{1}\")".Fmt(ueu.scid, custom.ID));
                        }
                    }
                    else
                    {
                        row.AddCell(string.Empty);
                    }
                }
                else
                {
                    row.AddCell(string.Empty);
                }

                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        private SearchResult<ApplyQualityUIModel> GetSearchResult(ApplyQualifySearchModel searchModel)
        {

            var predicate = PredicateBuilder.True<t_D_UserTableOne>();

            //获取分配给当前用户专家审批的资质机构
            var expertUnits = repExp.GetByCondition(p => p.userid == GetCurrentUserId());

            #region 动态查询

            if (expertUnits != null && expertUnits.Count > 0)
            {
                var pids = expertUnits.Select(e => e.pid).ToList();
                predicate = predicate.And(t => pids.Contains(t.id));
            }
            
            if (searchModel.Area != "-1" && !string.IsNullOrWhiteSpace(searchModel.Area))
            {
                predicate = predicate.And(t => t.area == searchModel.Area);
            }
            if (!string.IsNullOrWhiteSpace(searchModel.CheckUnitName))
            {
                predicate = predicate.And(t => t.unitCode == searchModel.CheckUnitName);
            }
            if (searchModel.CMADStartDt.HasValue)
            {
                predicate = predicate.And(t => t.time >= searchModel.CMADStartDt.Value);
            }
            if (searchModel.CMADEndDt.HasValue)
            {
                predicate = predicate.And(t => t.time <= searchModel.CMADEndDt.Value);
            }

            #endregion

            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            int count = searchModel.count.HasValue ? searchModel.count.Value : 30;

            string[] columns = { "CheckBox", "SeqNo", "NAME", "UNITNAME" };
            string orderby = string.Empty;
            string orderByAcs = string.Empty;
            bool isDes = true;
            string sortProperty = "addtime";

            PagingOptions<t_D_UserTableOne> pagingOption = new PagingOptions<t_D_UserTableOne>(pos, count, sortProperty, isDes);

            var customs = repOne.GetByConditionSort<ApplyQualityUIModel>(predicate, r => new
            {
                r.id,
                r.name,
                r.unitname,
                r.unitCode,
                r.onepath_zl,
                r.twopath_zl,
                r.threepath_zl,
                r.Fourpath_zl,
                r.fivepath_zl,
                r.Sixpath_zl,
                r.Sevenpath_zl,
                r.type
            },
               pagingOption);

            return new SearchResult<Models.ApplyQualityUIModel>(pagingOption.TotalItems, customs);
        }



        public ActionResult UnitBuildingQualify(int? id)
        {

            var model = new UnitQualifyUIModel();
            var tbOne = repOne.GetById(id);

            model.ID = id.ToString();
            model.UnitName = tbOne.unitname;
            model.FRDB = tbOne.name;

            var custom = repCustom.GetById(tbOne.unitCode);
            if (tbOne.type != "0")
            {
                model.YYJCZZ = custom.zzlbmc; //原有检测资质
            }

            var tbTwo = repTwo.GetByCondition(r => r.pid == id).FirstOrDefault();
            if(tbTwo != null)
            {
                model.OnesSb = tbTwo.measnum; //计量认证编号
                model.MeasnumPath = tbTwo.measnumpath;
                model.sqjcyw = tbTwo.sqjcyw;//现有检测资质
            }
            
            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult UnitBuildingQualify(ExpertUnitQualifySaveModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                result.IsSucc = true;
                t_D_userTableSH saveModel = new t_D_userTableSH()
                {
                    unitName = model.unitName,
                    frdb = model.frdb,
                    yyjczz = model.yyjczz,
                    onesjz = model.onesjz,
                    onedbqk = model.onedbqk,
                    twosjz = model.twosjz,
                    twodbqk = model.twodbqk,
                    twodjjcsjz = model.twodjjcsjz,
                    twodjjcdbqk = model.twodjjcdbqk,
                    twodztjgsjz = model.twodztjgsjz,
                    twoztjgdbqk = model.twoztjgdbqk,
                    twojzmqsjz = model.twojzmqsjz,
                    twojzmqdbqk = model.twojzmqdbqk,
                    twogjgsjz = model.twogjgsjz,
                    twogjgdbqk = model.twogjgdbqk,
                    twojzqysjz = model.twojzqysjz,
                    twojzqydbqk = model.twojzqydbqk,
                    threesjz = model.threesjz,
                    threedbqk = model.threedbqk,
                    foursjz = model.foursjz,
                    fourdbqk = model.fourdbqk,
                    shyj = model.shyj,
                    username = model.username,
                    createtime = model.createtime,
                    addtime = DateTime.Now,
                    pid = model.pid
                };
                if (!checkQualifyService.SaveExpertUnitQualify(saveModel, GetCurrentUserId(), out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = false;
                }
                else
                {
                    LogUserAction("对单位：{0} 进行了[建设工程质量检测机构资质]审批操作".Fmt(model.unitName));
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

       
        public ActionResult SpecialQualify(int id)
        {

            var model = new UnitQualifyUIModel();

            var tbOne = repOne.GetById(id);
            model.ID = id.ToString();
            model.UnitName = tbOne.unitname;

            var tbTwo = repTwo.GetByCondition(r => r.pid == id).FirstOrDefault();

            if (tbTwo != null)
            {
                model.FRDB = tbTwo.fr;
                model.SSFZR = tbTwo.jsfzr;
                model.YYJCZZ = tbTwo.sqjcyw;//现有检测资质
                model.OnesSb = tbTwo.measnum; //计量认证编号
                model.MeasnumPath = tbTwo.measnumpath;
            }

            return View(model);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SpecialQualify(SpecialQualifySaveModel model)
        {

            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                result.IsSucc = true;
                t_D_userTableSC saveModel = new t_D_userTableSC()
                {
                    unitName = model.unitName,
                    frdb = model.frdb,
                    sqnr = model.sqnr,
                    ssfzr = model.ssfzr,
                    onesjz = model.onesjz,
                    onedbqk = model.onedbqk,
                    twosjz = model.twosjz,
                    twodbqk = model.twodbqk,
                    threesjz = model.threesjz,
                    threedbqk = model.threedbqk,
                    foursjz = model.foursjz,
                    fourdbqk = model.fourdbqk,
                    fivesjzone = model.fivesjzone,
                    fivedbqkone = model.fivedbqkone,
                    fivesjztwo = model.fivesjztwo,
                    fivedbqktwo = model.fivedbqktwo,
                    sixsjz = model.sixsjz,
                    sixdbqk = model.sixdbqk,
                    sevensjz = model.sevensjz,
                    sevendbqk = model.sevendbqk,
                    shyj = model.shyj,
                    username = model.username,
                    createtime = model.createtime,
                    addtime = DateTime.Now,
                    pid = model.pid
                };
                if (!checkQualifyService.SaveSpeicalQualify(saveModel, GetCurrentUserId(), out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                    result.IsSucc = false;
                }
                else
                {
                    LogUserAction("对单位：{0} 进行了[专项检测备案]审批操作".Fmt(model.unitName));
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


        public ActionResult DetailUnitBuildingQualify(int? id, int? pid)
        {

            var model = repSH.GetById(id);
            var tbTwo = repTwo.GetByCondition(r => r.pid == pid).FirstOrDefault();
            var modelUI = new ExpertUnitQualifySaveModel()
            {
                pid = pid,
                unitName = model.unitName,
                frdb = model.frdb,
                yyjczz = model.yyjczz,
                onesjz = model.onesjz,
                onedbqk = model.onedbqk,
                twosjz = model.twosjz,
                twodbqk = model.twodbqk,
                twodjjcsjz = model.twodjjcsjz,
                twodjjcdbqk = model.twodjjcdbqk,
                twodztjgsjz = model.twodztjgsjz,
                twoztjgdbqk = model.twoztjgdbqk,
                twojzmqsjz = model.twojzmqsjz,
                twojzmqdbqk = model.twojzmqdbqk,
                twogjgsjz = model.twogjgsjz,
                twogjgdbqk = model.twogjgdbqk,
                twojzqysjz = model.twojzqysjz,
                twojzqydbqk = model.twojzqydbqk,
                threesjz = model.threesjz,
                threedbqk = model.threedbqk,
                foursjz = model.foursjz,
                fourdbqk = model.fourdbqk,
                shyj = model.shyj,
                username = model.username,
                createtime = model.createtime
            };

            if (tbTwo != null)
            {
                modelUI.OnesSb = tbTwo.measnum; //计量认证编号
                modelUI.MeasnumPath = tbTwo.measnumpath;
                modelUI.sqjcyw = tbTwo.sqjcyw;//现有检测资质
            }
            

            return View(modelUI);
        }

        public ActionResult DetailSpecialQualify(int? id, int? pid)
        {

            var model = repSC.GetById(id);
            var tbTwo = repTwo.GetByCondition(r => r.pid == pid).FirstOrDefault();
            var modelUI = new SpecialQualifySaveModel()
            {
                pid = pid,
                unitName = model.unitName,
                frdb = model.frdb,
                sqnr = model.sqnr,
                ssfzr = model.ssfzr,
                onesjz = model.onesjz,
                onedbqk = model.onedbqk,
                twosjz = model.twosjz,
                twodbqk = model.twodbqk,
                threesjz = model.threesjz,
                threedbqk = model.threedbqk,
                foursjz = model.foursjz,
                fourdbqk = model.fourdbqk,
                fivesjzone = model.fivesjzone,
                fivedbqkone = model.fivedbqkone,
                fivesjztwo = model.fivesjztwo,
                fivedbqktwo = model.fivedbqktwo,
                sixsjz = model.sixsjz,
                sixdbqk = model.sixdbqk,
                sevensjz = model.sevensjz,
                sevendbqk = model.sevendbqk,
                shyj = model.shyj,
                username = model.username,
                createtime = model.createtime
            };

            modelUI.OnesSb = tbTwo.measnum; //计量认证编号
            modelUI.MeasnumPath = tbTwo.measnumpath;

            return View(modelUI);
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

    }










}