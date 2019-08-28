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
using Pkpm.Entity;
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

namespace PkpmGX.Controllers
{
    [Authorize]
    public class STCheckQualifyController : PkpmController
    {
        IRepsitory<t_bp_custom_ST> rep;
        IRepsitory<t_bp_People_ST> peopleRep;
        IRepsitory<t_bp_pumpvehicle> pumpVehicleRep;
        IRepsitory<t_bp_pumpsystem> pumpSysRep;
        IRepsitory<t_bp_carriervehicle> carrieRvehicleRep;
        IRepsitory<t_bp_CusAchievement_ST> cusachieveRep;
        IRepsitory<t_bp_CusAwards_ST> cusAwardsRep;
        IRepsitory<t_bp_CusChange_ST> cusChangeRep;
        IRepsitory<t_bp_CusPunish_ST> cusPunishRep;
        IRepsitory<t_bp_CheckCustom_ST> checkCustomRep;
        IRepsitory<SupvisorJob> supvisorRep;
        IRepsitory<t_bp_CusCheckParams_ST> cusCheckParamRep;
        ISysDictService sysDictServcie;
        ISTCustomService stCustomService;
        //ICheckUnitService checkUnitService;
        IAreaService areaService;
        IFileHandler fileHander;
        public STCheckQualifyController(IUserService userService,
             ISysDictService sysDictServcie,
             IRepsitory<t_bp_CusAchievement_ST> cusachieveRep,
             IRepsitory<t_bp_CusAwards_ST> cusAwardsRep,
             IRepsitory<t_bp_CusChange_ST> cusChangeRep,
             IRepsitory<t_bp_pumpvehicle> pumpVehicleRep,
              IRepsitory<t_bp_pumpsystem> pumpSysRep,
               IRepsitory<t_bp_People_ST> peopleRep,
                IRepsitory<SupvisorJob> supvisorRep,
              IAreaService areaService,
              IRepsitory<t_bp_CusPunish_ST> cusPunishRep,
               IRepsitory<t_bp_CheckCustom_ST> checkCustomRep,
                IRepsitory<t_bp_carriervehicle> carrieRvehicleRep,
             IRepsitory<t_bp_CusCheckParams_ST> cusCheckParamRep,
             IRepsitory<t_bp_custom_ST> rep,
             ISTCustomService stCustomService,
             IFileHandler fileHander) : base(userService)
        {
            this.rep = rep;
            this.sysDictServcie = sysDictServcie;
            this.cusachieveRep = cusachieveRep;
            this.cusAwardsRep = cusAwardsRep;
            this.cusChangeRep = cusChangeRep;
            this.cusCheckParamRep = cusCheckParamRep;
            this.pumpVehicleRep = pumpVehicleRep;
            this.pumpSysRep = pumpSysRep;
            this.carrieRvehicleRep = carrieRvehicleRep;
            this.fileHander = fileHander;
            this.stCustomService = stCustomService;
            this.areaService = areaService;
            this.peopleRep = peopleRep;
            this.supvisorRep = supvisorRep;
            this.checkCustomRep = checkCustomRep;
            this.cusPunishRep = cusPunishRep;
        }

        // GET: STCheckQualify
        public ActionResult Index()
        {
            var viewModel = new CheckQualifyViewModels();
            viewModel.CompanyStatus = sysDictServcie.GetDictsByKey("customStatus");
            viewModel.CompanyTypes = sysDictServcie.GetDictsByKey("CompanyType");
            viewModel.CheckInsts = new Dictionary<string, string>();// checkUnitService.GetAllCheckUnit();
            int adminId = GetCurrentUserId();
            viewModel.IsAdmin = userService.IsAdmin(adminId);
            return View(viewModel);
        }


        public ActionResult Search(STQualifyModel searchModel)
        {
            var data = GetSearchResult(searchModel);

            DhtmlxGrid grid = new DhtmlxGrid();
            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            grid.AddPaging(data.TotalCount, pos);
            var buttons = GetCurrentUserPathActions();
            var compayTypes = sysDictServcie.GetDictsByKey("customStatus");
            for (int i = 0; i < data.Results.Count; i++)
            {

                var custom = data.Results[i];
                DhtmlxGridRow row = new DhtmlxGridRow(custom.Id.ToString());
                row.AddCell(string.Empty);
                row.AddCell((pos + i + 1).ToString());//序号
                                                      //row.AddCell("{0}({1})".Fmt(custom.NAME, custom.ID));

                row.AddCell("{0}({1})".Fmt(custom.NAME, custom.Id));
                //机构名称
                row.AddCell(custom.businesspermit);
                row.AddCell(custom.cerfno);//检测证书编号
                row.AddCell(custom.FR);//法人
                row.AddCell(custom.TEL);//联系电话
                row.AddCell(SysDictUtility.GetKeyFromDic(compayTypes, custom.APPROVALSTATUS));//状态
                Dictionary<string, string> dict = new Dictionary<string, string>();
                //已递交	1

                if (HaveButtonFromAll(buttons, "ApplyChange") && stCustomService.CanApplyChangeCustom(custom.APPROVALSTATUS))
                {
                    dict.Add("[申请修改]", "applyChange(\"{0}\",\"{1}\")".Fmt(custom.Id, custom.NAME));
                }

                if (HaveButtonFromAll(buttons, "ConfirmApplyChange") && (custom.APPROVALSTATUS == "5"))
                {
                    dict.Add("[审核申请修改]", "confirmApplyChange(\"{0}\",\"{1}\")".Fmt(custom.Id, custom.NAME));
                }
                row.AddLinkJsCells(dict);
                //申请修改已批准 6。新增/审核未通过能编辑（小罗）。未递交也能编辑（老系统）。
                if (HaveButtonFromAll(buttons, "Edit"))// && checkUnitService.CanEditCustom(custom.APPROVALSTATUS))
                {
                    row.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                }
                else
                {
                    row.AddCell(string.Empty);
                }
                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                //row.AddLinkJsCell("打印设置", "setInstPrintConfig(\"{0}\",\"{1}\")".Fmt(custom.ID, custom.bgfs));
                //if (!custom.ID.StartsWith(pkpmConfigService.UploadInstStartWith))
                //{
                //    row.AddLinkJsCell("转为正式账号", "setInstFormal(\"{0}\",\"{1}\")".Fmt(custom.ID, custom.NAME));
                //}
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        private SearchResult<STChekQualityUIModel> GetSearchResult(STQualifyModel searchModel)
        {

            var viewModel = new CheckQualifyViewModels();
            viewModel.CompanyStatus = sysDictServcie.GetDictsByKey("customStatus");
            var predicate = PredicateBuilder.True<t_bp_custom_ST>();

            #region 动态查询
            //过滤已经删除的机构
            predicate = predicate.And(tp => tp.data_status == null || tp.data_status != "-1");

            if (!searchModel.CompanyType.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.companytype == searchModel.CompanyType);
            }

            if (!searchModel.CustomId.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.Id == searchModel.CustomId);
            }
            if (!searchModel.FR.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.FR.Contains(searchModel.FR));
            }
            if (!searchModel.Status.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.APPROVALSTATUS == searchModel.Status);
            }
            #endregion

            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            int count = searchModel.count.HasValue ? searchModel.count.Value : 30;

            string orderby = string.Empty;
            string orderByAcs = string.Empty;
            bool isDes = false;
            string sortProperty = "ID";

            PagingOptions<t_bp_custom_ST> pagingOption = new PagingOptions<t_bp_custom_ST>(pos, count, sortProperty, isDes);


            var customs = rep.GetByConditionSort<STChekQualityUIModel>(predicate, r => new
            {
                r.Id,
                r.NAME,
                r.businesspermit,
                r.cerfno,
                r.FR,
                r.TEL,
                r.APPROVALSTATUS
            },
               pagingOption);

            return new SearchResult<Models.STChekQualityUIModel>(pagingOption.TotalItems, customs);
        }

        public ActionResult ToolBar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            var buttons = GetCurrentUserPathActions();
            if (HaveButtonFromAll(buttons, "Create"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("NewCustom", "新增机构") { Img = "fa fa-plus", Imgdis = "fa fa-plus" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("1"));
            }

            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Export", "导出") { Img = "fa fa-file-excel-o", Imgdis = "fa fa-file-excel-o" });
            toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("2"));
            if (HaveButtonFromAll(buttons, "Delete"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("DeleteCustom", "[删除]") { Img = "fa fa-times", Imgdis = "fa fa-times" });
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

            if (HaveButtonFromAll(buttons, "Screening"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Screening", "[屏蔽]") { Img = "fa fa-unlock-alt", Imgdis = "fa fa-unlock-alt" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("5"));
            }
            if (HaveButtonFromAll(buttons, "RelieveScreening"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("RelieveScreening", "[解除屏蔽]") { Img = "fa fa-unlock", Imgdis = "fa fa-unlock" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("6"));
            }


            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        /// <summary>
        /// 单位业绩
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult cusachievement(string id)
        {
            var CustomCusAchievement = cusachieveRep.GetByCondition(r => r.CustomId == id);
            DhtmlxGrid grid = new DhtmlxGrid();
            for (int i = 0; i < CustomCusAchievement.Count; i++)
            {

                var myObj = CustomCusAchievement[i];
                DhtmlxGridRow row = new DhtmlxGridRow(myObj.Id.ToString());
                row.AddCell((i + 1).ToString());
                row.AddCell(myObj.AchievementTime);
                row.AddCell(myObj.AchievementContent);
                row.AddCell(myObj.AchievementRem);
                row.AddLinkJsCell("删除", "cusachievement_delete({0})".Fmt(myObj.Id));
                //row.AddLinkJsCell("增加", "cusachievement_add()");
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        /// <summary>
        /// 单位业绩(用于检测机构查看界面)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CusachievementModel> CusachievementDetails(string id)
        {
            var cusAchievements = new List<CusachievementModel>();
            var CustomCusAchievement = cusachieveRep.GetByCondition(r => r.CustomId == id);
            for (int i = 0; i < CustomCusAchievement.Count; i++)
            {
                var cusAchievement = new CusachievementModel();
                var myObj = CustomCusAchievement[i];
                cusAchievement.Number = i + 1;
                cusAchievement.AchievementTime = myObj.AchievementTime;
                cusAchievement.AchievementContent = myObj.AchievementContent;
                cusAchievement.AchievementRem = myObj.AchievementRem;
                cusAchievement.Id = myObj.Id.ToString();
                cusAchievements.Add(cusAchievement);
            }
            return cusAchievements;
        }

        /// <summary>
        /// 获奖情况
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult cusawards(string id)
        {
            var myCusawards = cusAwardsRep.GetByCondition(r => r.CustomId == id);
            DhtmlxGrid grid = new DhtmlxGrid();
            for (int i = 0; i < myCusawards.Count; i++)
            {
                var myObj = myCusawards[i];
                DhtmlxGridRow row = new DhtmlxGridRow(myObj.Id.ToString());
                row.AddCell((i + 1).ToString());
                row.AddCell(myObj.AwaDate.HasValue ? myObj.AwaDate.Value.ToString("yyyy-MM-dd") : string.Empty);
                row.AddCell(myObj.AwaName);
                row.AddCell(myObj.AwaUnit);
                row.AddCell(myObj.AwaContent);
                row.AddCell(myObj.AwaRem);
                row.AddLinkJsCell("删除", "cusawards_delete({0})".Fmt(myObj.Id));
                //row.AddLinkJsCell("增加", "cusawards_add()");
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml"); ;
        }

        /// <summary>
        /// 获奖情况（用于检测机构查看界面）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<STCusawardsModel> CusawardsDetails(string id)
        {
            var myCusawards = cusAwardsRep.GetByCondition(r => r.CustomId == id);
            List<STCusawardsModel> cusAwards = new List<STCusawardsModel>();
            for (int i = 0; i < myCusawards.Count; i++)
            {
                STCusawardsModel model = new STCusawardsModel();
                var myObj = myCusawards[i];
                model.Number = i + 1;
                model.AwaDate = myObj.AwaDate.HasValue ? myObj.AwaDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                model.AwaName = myObj.AwaName;
                model.AwaUnit = myObj.AwaUnit;
                model.AwaContent = myObj.AwaContent;
                model.AwaRem = myObj.AwaRem;
                model.Id = myObj.Id.ToString();
                cusAwards.Add(model);
            }
            return cusAwards;
        }

        ///// <summary>
        ///// 处罚情况
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public ActionResult cuspunish(string id)
        //{

        //    var myCuspunish = CusPunish.GetByCondition(r => r.CustomId == id);
        //    DhtmlxGrid grid = new DhtmlxGrid();

        //    for (int i = 0; i < myCuspunish.Count; i++)
        //    {
        //        var myObj = myCuspunish[i];
        //        DhtmlxGridRow row = new DhtmlxGridRow(myObj.id.ToString());
        //        row.AddCell((i + 1).ToString());
        //        row.AddCell(myObj.PunDate.HasValue ? myObj.PunDate.Value.ToString("yyyy-MM-dd") : string.Empty);
        //        row.AddCell(myObj.PunName);
        //        row.AddCell(myObj.PunUnit);
        //        row.AddCell(myObj.PunContent);
        //        row.AddCell(myObj.PunRem);
        //        row.AddLinkJsCell("删除", "cuspunish_delete({0})".Fmt(myObj.id));
        //        //row.AddLinkJsCell("增加", "Cuspunish_add()");
        //        grid.AddGridRow(row);
        //    }
        //    string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
        //    return Content(str, "text/xml");
        //}

        /// <summary>
        /// 处罚情况（用于检测机构查看界面）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<STCuspunishModel> CuspunishDetails(string id)
        {
            var cusPunishs = new List<STCuspunishModel>();
            var myCuspunish = cusPunishRep.GetByCondition(r => r.CustomId == id);
            for (int i = 0; i < myCuspunish.Count; i++)
            {
                var cusPunish = new STCuspunishModel();
                var myObj = myCuspunish[i];
                cusPunish.Number = i + 1;
                cusPunish.Id = myObj.Id.ToString();
                cusPunish.PunContent = myObj.PunContent;
                cusPunish.PunDate = myObj.PunDate.HasValue ? myObj.PunDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                cusPunish.PunName = myObj.PunName;
                cusPunish.PunUnit = myObj.PunUnit;
                cusPunish.PunRem = myObj.PunRem;
                cusPunishs.Add(cusPunish);
            }
            return cusPunishs;
        }

        ///// <summary>
        ///// 复查情况
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public ActionResult checkcustom(string id)
        //{
        //    var myCheckCustom = CheckCustom.GetByCondition(r => r.CustomId == id);
        //    DhtmlxGrid grid = new DhtmlxGrid();
        //    for (int i = 0; i < myCheckCustom.Count; i++)
        //    {
        //        var myObj = myCheckCustom[i];
        //        DhtmlxGridRow row = new DhtmlxGridRow(myObj.id.ToString());
        //        row.AddCell((i + 1).ToString());
        //        row.AddCell(myObj.CheDate.HasValue ? myObj.CheDate.Value.ToString("yyyy-MM-dd") : string.Empty);
        //        row.AddCell(myObj.CheResult);
        //        row.AddCell(myObj.CheRem);
        //        row.AddLinkJsCell("删除", "checkcustom_delete({0})".Fmt(myObj.id));
        //        row.AddLinkJsCell("增加", "Cuspunish_add()");
        //        grid.AddGridRow(row);
        //    }
        //    string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
        //    return Content(str, "text/xml");
        //}

        /// <summary>
        /// 复查情况（用于检测机构查看界面）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<STCheckcustomModel> CheckCustomDetails(string id)
        {
            List<STCheckcustomModel> checkCustoms = new List<STCheckcustomModel>();
            var myCheckCustom = checkCustomRep.GetByCondition(r => r.CustomId == id);
            for (int i = 0; i < myCheckCustom.Count; i++)
            {
                STCheckcustomModel checkCustom = new STCheckcustomModel();
                var myObj = myCheckCustom[i];
                checkCustom.Id = myObj.Id.ToString();
                checkCustom.Number = i + 1;
                checkCustom.CheDate = myObj.CheDate.HasValue ? myObj.CheDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                checkCustom.CheResult = myObj.CheResult;
                checkCustom.CheRem = myObj.CheRem;
                checkCustoms.Add(checkCustom);
            }
            return checkCustoms;
        }

        /// <summary>
        /// 变更情况
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult cuschange(string id)
        {
            List<CuschangeModel> cusChanges = new List<CuschangeModel>();
            var myCusChange = cusChangeRep.GetByCondition(r => r.CustomId == id);
            DhtmlxGrid grid = new DhtmlxGrid();

            for (int i = 0; i < myCusChange.Count; i++)
            {

                var myObj = myCusChange[i];
                DhtmlxGridRow row = new DhtmlxGridRow(myObj.Id.ToString());
                row.AddCell((i + 1).ToString());
                row.AddCell(myObj.ChaDate.HasValue ? myObj.ChaDate.Value.ToString("yyyy-MM-dd") : string.Empty);
                row.AddCell(myObj.ChaContent);
                row.AddCell(myObj.ChaRem);
                row.AddLinkJsCell("编辑", "cuschange_edit(\"{0}\" )".Fmt(myObj.Id));
                row.AddLinkJsCell("删除", "cuschange_delete({0})".Fmt(myObj.Id));
                //  row.AddLinkJsCell("增加", "Cuspunish_add()");
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        /// <summary>
        /// 变更情况（用于检测结构查看界面）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CuschangeModel> CusChangeDetails(string id)
        {
            List<CuschangeModel> cusChanges = new List<CuschangeModel>();
            var myCusChange = cusChangeRep.GetByCondition(r => r.CustomId == id);
            for (int i = 0; i < myCusChange.Count; i++)
            {
                CuschangeModel cusChange = new CuschangeModel();
                var myObj = myCusChange[i];
                cusChange.Id = myObj.Id.ToString();
                cusChange.Number = i + 1;
                cusChange.ChaDate = myObj.ChaDate.HasValue ? myObj.ChaDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                cusChange.ChaContent = myObj.ChaContent;
                cusChange.ChaRem = myObj.ChaRem;
                cusChanges.Add(cusChange);
            }
            return cusChanges;
        }

        // GET: CheckQualify/Details/5
        public ActionResult Details(string id)
        {
            var model = GetCheckQualifyViewModel(id);
            var sysDict = model.CompayTypeList["SandFactoryStatus"].Where(t => t.KeyValue == model.myT_bp_Custom.sslczk).FirstOrDefault();
            if (sysDict != null)
            {
                model.myT_bp_Custom.sslczk = sysDict.Name;
            }
            var companyType = model.CompayTypeList["CompanyType"].Where(t => t.KeyValue == model.myT_bp_Custom.companytype).FirstOrDefault();
            if(companyType!=null)
            {
                model.myT_bp_Custom.companytype = companyType.Name;
            }
            return View(model);
        }

        private STQualifyViewModels GetCheckQualifyViewModel(string id)
        {
            var viewModel = new STQualifyViewModels()
            {
                Cusawards = new List<STCusawardsModel>(),
                CusPunish = new List<STCuspunishModel>(),
                Cusachievement = new List<STCusachievementModel>(),
                CheckCustom = new List<STCheckcustomModel>(),
                File = new List<FileGridModel>(),
                Area = new Dictionary<string, string>()
            };
            viewModel.CompayTypeList = new Dictionary<string, List<SysDict>>();
            //viewModel.CompayTypeList.Add("CustomArea", sysDictServcie.GetDictsByKey("CustomArea"));
            viewModel.Region = userService.GetAllArea();
            viewModel.CompayTypeList.Add("unitQualifications", sysDictServcie.GetDictsByKey("unitQualifications"));
            viewModel.CompayTypeList.Add("personnelTitles", sysDictServcie.GetDictsByKey("personnelTitles"));
            viewModel.CompayTypeList.Add("yesNo", sysDictServcie.GetDictsByKey("yesNo"));
            viewModel.CompayTypeList.Add("CompanyType", sysDictServcie.GetDictsByKey("CompanyType"));
            viewModel.CompayTypeList.Add("SandFactoryStatus", sysDictServcie.GetDictsByKey("SandFactoryStatus"));
            viewModel.myT_bp_Custom = rep.GetByCondition(t => t.Id == id).First();

            //viewModel.myT_bp_Custom.JSTIILE = sysDictServcie.GetDictsByKey("personnelTitles", viewModel.myT_bp_Custom.JSTIILE);
            //viewModel.myT_bp_Custom.ZLTITLE = sysDictServcie.GetDictsByKey("personnelTitles", viewModel.myT_bp_Custom.ZLTITLE);
            //计算人员信息相关人数
            var predicate = PredicateBuilder.True<t_bp_People>();
            predicate = predicate.And(p => p.Customid == id);
            predicate = predicate.And(tp => tp.data_status == null || tp.data_status != "-1");
            //var peoples = this.peopleRep.GetByCondition(predicate);
            //int PERCOUNT = peoples.Count;//总人数
            //int hasNumPerCount = peoples.Where(w => w.ishaspostnum == "1").ToList().Count;//持证人数
            //int REGYTSTA = peoples.Where(w => w.iszcgccs == "1").ToList().Count;//注册岩土工程师人数
            //int REGJGSTA = peoples.Where(w => w.iszcgccs == "2").ToList().Count;//注册结构工程师
            //int HEIPERCOUNT = peoples.Where(w => w.Title == "3").ToList().Count;//高级职称
            //int MIDPERCOUNT = peoples.Where(w => w.Title == "2").ToList().Count;//中级职称
            //viewModel.myT_bp_Custom.PERCOUNT = PERCOUNT.ToString();
            //viewModel.myT_bp_Custom.hasNumPerCount = hasNumPerCount.ToString();
            //viewModel.myT_bp_Custom.REGJGSTA = REGJGSTA.ToString();
            //viewModel.myT_bp_Custom.REGYTSTA = REGYTSTA.ToString();
            //viewModel.myT_bp_Custom.MIDPERCOUNT = MIDPERCOUNT.ToString();
            //viewModel.myT_bp_Custom.HEIPERCOUNT = HEIPERCOUNT.ToString();
            viewModel.myT_bp_Custom.JSTIILE = sysDictServcie.GetDictsByKey("personnelTitles", viewModel.myT_bp_Custom.JSTIILE);
            viewModel.myT_bp_Custom.ZLTITLE = sysDictServcie.GetDictsByKey("personnelTitles", viewModel.myT_bp_Custom.ZLTITLE);
            //viewModel.myT_bp_Custom.instrumentNum = equRep.GetByCondition(p => p.customid == id).Count.ToString();//设备套数
            //viewModel.Cusachievement = CusachievementDetails(viewModel.myT_bp_Custom.ID);//单位业绩
            viewModel.Cusawards = CusawardsDetails(viewModel.myT_bp_Custom.Id);//获奖情况
            viewModel.CusPunish = CuspunishDetails(viewModel.myT_bp_Custom.Id);//处罚情况
            viewModel.CheckCustom = CheckCustomDetails(viewModel.myT_bp_Custom.Id);//复查情况
            //viewModel.Cuschange = CusChangeDetails(viewModel.myT_bp_Custom.ID);//变更情况
            viewModel.File = FileGrid(viewModel.myT_bp_Custom.Id,
                viewModel.myT_bp_Custom.businesspermitpath,
                viewModel.myT_bp_Custom.cerfnopath,
                viewModel.myT_bp_Custom.zzbgpath);
            viewModel.Area = areaService.GetAllDictArea();

            return viewModel;
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

        // GET: CheckQualify/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CheckQualify/Create
        [HttpPost]
        public ActionResult Create(string Id, string Name)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string error = string.Empty;
            if (!stCustomService.NewCustom(Id, Name, out error))
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = error;
            }
            return Content(result.ToJson());
        }

        // GET: CheckQualify/Edit/5
        public ActionResult Edit(string id)
        {
            var viewModel = GetCheckQualifyViewModel(id);
            var allCustomPeoples = peopleRep.GetByCondition(t => t.Customid == id);
            viewModel.PerCount = allCustomPeoples.Count();
            viewModel.ManagerCount = allCustomPeoples.Count(t => t.ismanager == "1");
            viewModel.TestCount = allCustomPeoples.Count(t => t.issy == "1");
            viewModel.JsglCount = allCustomPeoples.Count(t => t.isjs == "1");
            viewModel.PumpSystems = pumpSysRep.GetByCondition(t => t.customid == id);
            viewModel.Pumpvehicles = pumpVehicleRep.GetByCondition(t => t.customid == id);
            viewModel.Carriervehicles = carrieRvehicleRep.GetByCondition(t => t.customid == id);
            return View(viewModel);
        }

        // POST: CheckQualify/Edit/5

        public ActionResult AddCusAchievement()
        {
            return View();
        }

        public ActionResult AddSTCusAward()
        {
            return View();
        }

        public ActionResult AddSTCusPunish()
        {
            return View();
        }

        public ActionResult AddSTCheckCustom()
        {
            return View();
        }

        public ActionResult AddCusChange()
        {
            return View();
        }

        public ActionResult EditCusChange(string cusChangTable)
        {
            t_bp_CusChange model = new t_bp_CusChange();
            if (!cusChangTable.IsNullOrEmpty())
            {
                var cuschangTds = cusChangTable.Split('\t');
                int index = 0;
                foreach (var item in cuschangTds)
                {
                    switch (index)
                    {
                        case 0:
                            model.ChaDate = DateTime.Parse(item);
                            break;
                        case 1:
                            model.ChaContent = item;
                            break;
                        case 2:
                            model.ChaRem = item;
                            break;
                    }
                    index++;
                }
            }
            return View(model);
        }


        public ActionResult EditCheckQualify(string id)
        {
            var viewModel = GetCheckQualifyViewModel(id);
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



        // GET: CheckQualify/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CheckQualify/Delete/5
        [HttpPost]
        public ActionResult Delete(string selectedId, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!stCustomService.DeleteCustom(selectedId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对机构ID为{0}进行了删除操作".Fmt(selectedId));
                }

            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        public List<FileGridModel> FileGrid(string id,
           string businesspermitpath,
           string cerfnopath,
           string zzbgpath
           )
        {
            var files = new List<FileGridModel>();
            var cuntom = rep.GetById(id);
            string[,] FileRangeAr = new string[3, 3];
            FileRangeAr[0, 0] = "工商营业执照号码";
            FileRangeAr[1, 0] = "商砼企业资质证书";
            FileRangeAr[2, 0] = "预拌混凝土生产企业检查表格";


            FileRangeAr[0, 1] = cuntom.businesspermitpath;
            FileRangeAr[1, 1] = cuntom.cerfnopath;
            FileRangeAr[2, 1] = cuntom.zzbgpath;


            FileRangeAr[0, 2] = businesspermitpath;
            FileRangeAr[1, 2] = cerfnopath;
            FileRangeAr[2, 2] = zzbgpath;


            // DhtmlxGrid grid = new DhtmlxGrid();

            for (int i = 0; i < 3; i++)
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
                        model.Type = "businesspermitpath";
                        break;
                    case 1:
                        model.Type = "cerfnopath";
                        break;
                    case 2:
                        model.Type = "zzbgpath";
                        model.Modify = 1;
                        break;
                }
                model.Id = i.ToString();
                files.Add(model);
            }
            return files;
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

        //[HttpPost]
        //public ActionResult DeleteImage(ImageViewUpload model)
        //{
        //    ControllerResult result = ControllerResult.SuccResult;
        //    string fileName = Regex.Replace(model.itemValue, Regex.Escape("/userfiles"), "", RegexOptions.IgnoreCase);
        //    try
        //    {
        //        fileHander.DeleteFile("userfiles", fileName);
        //        string errMsg = string.Empty;
        //        var custom = rep.GetById(model.itemId);
        //        string pathvalues = string.Empty;
        //        switch (model.itemName)
        //        {
        //            case "businessnumPath":
        //                pathvalues = custom.businessnumPath;
        //                break;
        //            case "DETECTPATH":
        //                pathvalues = custom.DETECTPATH;
        //                break;
        //            case "MEASNUMPATH":
        //                pathvalues = custom.MEASNUMPATH;
        //                break;
        //            case "instrumentpath":
        //                pathvalues = custom.instrumentpath;
        //                break;
        //            case "shebaopeoplelistpath":
        //                pathvalues = custom.shebaopeoplelistpath;
        //                break;
        //        }
        //        var queryStr = from str in pathvalues.Split('|')
        //                       where str != model.itemValue
        //                       select str;
        //        //model.itemName存的是字段名
        //        bool success = this.checkUnitService.UpdateAttachPathsIntoCustom(model.itemId, model.itemName, queryStr.Join("|"), out errMsg);
        //        if (success)
        //        {
        //            LogUserAction("对机构id为{0}的{1}字段进行了存储地址的更新操作".Fmt(model.itemId, model.itemName));
        //        }
        //        else
        //        {
        //            LogUserAction("对机构id为{0}的{1}字段进行了存储地址的更新,操作失败".Fmt(model.itemId, model.itemName));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = ControllerResult.FailResult;
        //        result.ErroMsg = "删除失败:" + ex.Message;
        //    }

        //    return Content(result.ToJson());
        //}

        public ActionResult AttachFileDownload(string id)
        {
            string fileName = Regex.Replace(id, Regex.Escape("/userfiles"), "", RegexOptions.IgnoreCase);//model.itemValue;
            string mimeType = MimeMapping.GetMimeMapping(fileName);
            Stream stream = fileHander.LoadFile("userfiles", fileName);
            return File(stream, mimeType, fileName);
        }

        //[HttpPost]
        //public ActionResult UpdatePaths(ImageViewUpload model)
        //{
        //    ControllerResult result = ControllerResult.SuccResult;
        //    string errMsg = string.Empty;
        //    bool success = this.checkUnitService.UpdateAttachPathsIntoCustom(model.itemId, model.itemName, model.itemValue, out errMsg);
        //    if (!success)
        //    {
        //        result = ControllerResult.FailResult;
        //        result.ErroMsg = errMsg;
        //        LogUserAction("对机构id为{0}的{1}字段进行了存储地址的更新,操作失败".Fmt(model.itemId, model.itemName));
        //    }
        //    else
        //    {
        //        LogUserAction("对机构id为{0}的{1}字段进行了存储地址的更新操作".Fmt(model.itemId, model.itemName));
        //    }
        //    return Content(result.ToJson());
        //}

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



        //批量返回状态
        [HttpPost]
        public ActionResult SetInstState(string selectedId, string state, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!stCustomService.SetInstSendState(selectedId, state, out erroMsg))
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




        public ActionResult Applychange(string ID)
        {
            var model = new CheckQualifyApplyChangeViewModel();
            model.ID = ID;
            return View(model);

        }

        [HttpPost]
        public ActionResult ApplyChange(CheckCustomApplyChange applyChangeModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                SupvisorJob job = new SupvisorJob()
                {
                    ApproveType = ApproveType.STApproveCustom,
                    CreateBy = GetCurrentUserId(),
                    CustomId = applyChangeModel.SubmitId,
                    CreateTime = DateTime.Now,
                    NeedApproveId = applyChangeModel.SubmitId.ToString(),
                    NeedApproveStatus = NeedApproveStatus.CreateForChangeApply,
                    SubmitName = applyChangeModel.SubmitName,
                    SubmitText = applyChangeModel.SubmitText
                };


                string erroMsg = string.Empty;
                if (!stCustomService.ApplyChangeForCustom(job, applyChangeModel.SubmitId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("进行了申请修改操作，申请人为{0}，申请原因为{1}".Fmt(applyChangeModel.SubmitName, applyChangeModel.SubmitText));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }
        //[HttpPost]
        //public ActionResult Screening(string selectedId, FormCollection collection)
        //{
        //    ControllerResult result = ControllerResult.SuccResult;
        //    try
        //    {
        //        string erroMsg = string.Empty;
        //        if (!checkUnitService.SetInstScreeningState(selectedId, out erroMsg))
        //        {
        //            result = ControllerResult.FailResult;
        //            result.ErroMsg = erroMsg;
        //        }
        //        else
        //        {
        //            LogUserAction("对机构ID为{0}进行了状态屏蔽操作".Fmt(selectedId));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = ControllerResult.FailResult;
        //        result.ErroMsg = ex.Message;
        //    }

        //    return Content(result.ToJson());
        //}
        //// POST: CheckQualify/Edit/5

        //public ActionResult RelieveScreening(string selectedId, FormCollection collection)
        //{
        //    ControllerResult result = ControllerResult.SuccResult;
        //    try
        //    {
        //        string erroMsg = string.Empty;
        //        if (!checkUnitService.SetInstRelieveScreeningSate(selectedId, out erroMsg))
        //        {
        //            result = ControllerResult.FailResult;
        //            result.ErroMsg = erroMsg;
        //        }
        //        else
        //        {
        //            LogUserAction("对机构ID为{0}进行了状态解除屏蔽操作".Fmt(selectedId));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result = ControllerResult.FailResult;
        //        result.ErroMsg = ex.Message;
        //    }
        //    return Content(result.ToJson());
        //}

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(stCustomSaveViewModel viewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            STCustomSaveModel editModel = new STCustomSaveModel()
            {
                stCusPunishs = new List<t_bp_CusPunish_ST>(),
                stCusAwards = new List<t_bp_CusAwards_ST>(),
                stCustomReChecks = new List<t_bp_CheckCustom_ST>(),
                PumpSystems = new List<t_bp_pumpsystem>(),
                Pumpvehicles = new List<t_bp_pumpvehicle>(),
                Carrievechicles = new List<t_bp_carriervehicle>()
            };
            try
            {
                editModel.OpUserId = GetCurrentUserId();
                DateTime? detectstartDateTime = null;//, measnumStartDateTime = null;
                DateTime? detectendDateTime = null;//, measnumEndDateTime = null;
                CommonUtils.GetLayuiDateRange(viewModel.detectnumdate, out detectstartDateTime, out detectendDateTime);
                viewModel.detectnumStartDate = detectstartDateTime;
                viewModel.detectnumEndDate = detectendDateTime;

                // CommonUtils.GetLayuiDateRange(viewModel.measnumDate, out measnumStartDateTime, out measnumEndDateTime);
                // viewModel.measnumStartDate = measnumStartDateTime;
                if (!viewModel.measnumDate.IsNullOrEmpty())
                {
                    viewModel.measnumEndDate = DateTime.Parse(viewModel.measnumDate);
                }
                #region 对附件路径处理
                if (!viewModel.businessnumPath.IsNullOrEmpty())
                {
                    if (viewModel.businessnumPath.Contains("||"))
                    {
                        viewModel.businessnumPath.Replace("||", "|");
                    }
                }
                #endregion

                #region 
                editModel.custom = new t_bp_custom_ST()
                {
                    Id = viewModel.Id,
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
                    DETECTNUM = viewModel.DETECTNUM,
                    APPLDATE = viewModel.APPLDATE,
                    QUAINFO = viewModel.QUAINFO,
                    APPROVALSTATUS = "0",
                    ADDDATE = viewModel.ADDDATE,
                    phone = viewModel.phone,
                    detectnumStartDate = viewModel.detectnumStartDate,
                    detectnumEndDate = viewModel.detectnumEndDate,
                    measnumStartDate = viewModel.measnumStartDate,
                    measnumEndDate = viewModel.measnumEndDate,
                    hasNumPerCount = viewModel.hasNumPerCount,
                    instrumentNum = viewModel.instrumentNum,
                    approveadvice = viewModel.approveadvice,
                    subunitnum = viewModel.subunitnum,
                    issubunit = viewModel.issubunit,
                    supunitcode = viewModel.supunitcode,
                    subunitdutyman = viewModel.subunitdutyman,
                    area = viewModel.area,
                    detectunit = viewModel.detectunit,
                    detectappldate = viewModel.detectappldate,
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
                    zzbgpath = viewModel.zzbgpath,
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
                    workercount = viewModel.workercount,
                    zgcount = viewModel.zgcount,
                    datatype = viewModel.datatype,
                    REGJGSTA = viewModel.REGJGSTA,
                    data_status = "2"
                };
                #endregion
                //editModel.CusAchievement = new List<t_bp_CusAchievement_ST>();
                editModel.stCusAwards = new List<t_bp_CusAwards_ST>();
                //editModel.CusChange = new List<t_bp_CusChange_ST>();
                //editModel.CheckCustom = new List<t_bp_CheckCustom>();




                if (viewModel.stCusAwards != null)
                {
                    foreach (var cusAwar in viewModel.stCusAwards)
                    {
                        cusAwar.CustomId = viewModel.Id;
                        editModel.stCusAwards.Add(cusAwar);
                    }
                }

                if (viewModel.stCusPunishs != null)
                {
                    foreach (var cusAwar in viewModel.stCusPunishs)
                    {
                        cusAwar.CustomId = viewModel.Id;
                        editModel.stCusPunishs.Add(cusAwar);
                    }
                }


                if (viewModel.stCustomReChecks != null)
                {
                    foreach (var cusCheck in viewModel.stCustomReChecks)
                    {
                        cusCheck.CustomId = viewModel.Id;
                        editModel.stCustomReChecks.Add(cusCheck);
                    }
                }

                if (viewModel.PumpSystems != null)
                {
                    foreach (var item in viewModel.PumpSystems)
                    {
                        item.customid = viewModel.Id;
                        editModel.PumpSystems.Add(item);

                    }
                }

                if (viewModel.Pumpvehicles != null)
                {
                    foreach (var item in viewModel.Pumpvehicles)
                    {
                        item.customid = viewModel.Id;
                        editModel.Pumpvehicles.Add(item);
                    }
                }

                if (viewModel.Carrievechicles != null)
                {
                    foreach (var item in viewModel.Carrievechicles)
                    {
                        item.customid = viewModel.Id;
                        editModel.Carrievechicles.Add(item);
                    }
                }

                string erroMsg = string.Empty;
                if (!stCustomService.EditCustom(editModel, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("进行了商砼企业信息修改操作，机构id为{0}".Fmt(viewModel.Id));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }


        public ActionResult Export(STQualifyModel searchModel, int? fileFormat)
        {
            var data = GetSearchResult(searchModel);

            // 改动2：创建导出类实例（而非 DhtmlxGrid），设置列标题
            bool xlsx = (fileFormat ?? 2007) == 2007;
            ExcelExporter ee = new ExcelExporter("商砼企业信息", xlsx);
            ee.SetColumnTitles("序号, 企业名称, 营业执照, 资质证书号, 法人, 联系电话,  状态");
            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            var compayTypes = sysDictServcie.GetDictsByKey("customStatus");
            for (int i = 0; i < data.Results.Count; i++)
            {
                var custom = data.Results[i];
                ExcelRow row = ee.AddRow();
                row.AddCell((pos + i + 1).ToString());
                row.AddCell("{0}({1})".Fmt(custom.NAME, custom.Id));
                row.AddCell(custom.businesspermit);
                row.AddCell(custom.cerfno);
                row.AddCell(custom.FR);
                row.AddCell(custom.TEL);
                row.AddCell(SysDictUtility.GetKeyFromDic(compayTypes, custom.APPROVALSTATUS));

            }

            // 改动4：返回字节流
            return File(ee.GetAsBytes(), ee.MIME, ee.FileName);
        }


        public ActionResult NewCustom()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewCustom(string CustomName, string CustomNo)
        {
            ControllerResult result = ControllerResult.SuccResult;
            if (string.IsNullOrEmpty(CustomName))
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = "机构名称不能为空！";
                return Content(result.ToJson());
            }
            if (string.IsNullOrEmpty(CustomNo))
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = "数据上传编码不能为空！";
                return Content(result.ToJson());
            }
            try
            {
                string errorMsg = string.Empty;
                if (!stCustomService.NewCustom(CustomNo, CustomName, out errorMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = errorMsg;
                }
                else
                {
                    LogUserAction("创建了新的机构{0}[{1}]".Fmt(CustomName, CustomNo));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        //[HttpPost]
        //public ActionResult SetInstFormal(string customid, string formalCustomId)
        //{
        //    ControllerResult result = ControllerResult.SuccResult;
        //    string errMsg = string.Empty;
        //    if (!stCustomService.SetInstFormal(customid, formalCustomId, out errMsg))
        //    {
        //        result = ControllerResult.FailResult;
        //        result.ErroMsg = errMsg;
        //    }
        //    else
        //    {
        //        LogUserAction("将机构编号为{0}的临时机构转换为正式机构,正式机构编号为{1}".Fmt(customid, formalCustomId));
        //        LogUserAction("将机构编号为{0}的人员，设备信息的机构编号更新为{1}".Fmt(customid, formalCustomId));
        //    }
        //    return Content(result.ToJson());
        //}

        public ActionResult setInstFormal(string customid)
        {
            ViewBag.Id = customid;
            return View();
        }

        //[HttpPost]
        //public string GetWjlr(string customId)
        //{
        //    string result = string.Empty;
        //    var custom = rep.GetById(customId);
        //    if (custom != null)
        //    {
        //        result = custom.wjlr;
        //    }
        //    return result;//Content(result);
        //}

        //[ValidateInput(false)]
        //[HttpPost]
        //public ActionResult UpdateWjlrAndZzlbmc(string wjlr, string zzlbmc, string customid)
        //{
        //    ControllerResult result = ControllerResult.SuccResult;
        //    string errMsg = string.Empty;
        //    if (customid.IsNullOrEmpty())
        //    {
        //        result = ControllerResult.FailResult;
        //        result.ErroMsg = "机构编号不能为空";
        //        return Content(result.ToJson());
        //    }
        //    if (!checkUnitService.UpdateWjlrAndZzlbmc(wjlr, zzlbmc, customid, out errMsg))
        //    {
        //        result = ControllerResult.FailResult;
        //        result.ErroMsg = errMsg;
        //    }
        //    else
        //    {
        //        LogUserAction("进行了更新机构id为{0}的资质信息".Fmt(customid));
        //    }
        //    return Content(result.ToJson());
        //}

        //public ActionResult GetTable(AddtableModel model)
        //{
        //    int dbCount = 0;
        //    var predicate = PredicateBuilder.True<t_bp_People>();
        //    predicate = predicate.And(tp => tp.data_status == null || tp.data_status != "-1");
        //    if (!string.IsNullOrEmpty(model.CheckUnitName))
        //    {
        //        predicate = predicate.And(t => t.Customid == model.CheckUnitName);
        //    }
        //    if (!string.IsNullOrEmpty(model.HasCert) && model.HasCert != "null")
        //    {
        //        if (model.HasCert == "1")
        //        { predicate = predicate.And(t => t.PostNum != null); }
        //        else if (model.HasCert == "0")
        //        { predicate = predicate.And(t => t.PostNum == null || t.PostNum == ""); }
        //    }
        //    if (!string.IsNullOrEmpty(model.PeopleStatus) && model.PeopleStatus != "null")
        //    {
        //        predicate = predicate.And(t => t.iscb == model.PeopleStatus);
        //    }
        //    if (!string.IsNullOrEmpty(model.TechTitle) && model.TechTitle != "null")
        //    {

        //        if (model.TechTitle == "6")
        //        { predicate = predicate.And(t => t.Title == "4" || t.Title == "2" || t.Title == "3"); }
        //        else
        //        { predicate = predicate.And(t => t.Title == model.TechTitle); }
        //    }

        //    if (!string.IsNullOrEmpty(model.IsTech) && model.IsTech != "null")
        //    {

        //        predicate = predicate.And(t => t.iszcgccs == model.IsTech);
        //    }

        //    dbCount = (int)peopleRep.GetCountByCondtion(predicate);



        //    var peoples = peopleRep.GetByConditionSort<CheckPeopleUIModel>(predicate,
        //        p => new
        //        {
        //            p.id,
        //            p.Name,
        //            p.Customid,
        //            p.SelfNum,
        //            p.PostNum,
        //            p.zw,
        //            p.Title,
        //            p.iscb,
        //            p.Approvalstatus,
        //            p.PostDate,
        //            p.IsUse
        //        }, null);

        //    var instDicts = checkUnitService.GetAllCheckUnit();
        //    var personnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");
        //    var workStatus = sysDictServcie.GetDictsByKey("workStatus");
        //    var personnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");
        //    AddTablePostModle viewModel = new AddTablePostModle();
        //    var CheckPeopl = new List<CheckPeoplModel>();
        //    var number = 1;
        //    LayUIGrid Grid = new LayUIGrid();
        //    foreach (var item in peoples)
        //    {
        //        var checkPeopl = new CheckPeoplModel()
        //        {
        //            number = number++,
        //            id = item.id,
        //            Name = item.Name,
        //            CustomName = checkUnitService.GetCheckUnitByIdFromAll(instDicts, item.Customid),
        //            SelfNum = item.SelfNum,
        //            zw = item.zw,
        //            Title = SysDictUtility.GetKeyFromDic(personnelTitles, item.Title),
        //            iscb = SysDictUtility.GetKeyFromDic(workStatus, item.iscb),
        //            Approvalstatus = SysDictUtility.GetKeyFromDic(personnelStatus, item.Approvalstatus),
        //            postdate = item.postdate,
        //            IsUse = item.IsUse
        //        };
        //        CheckPeopl.Add(checkPeopl);
        //    }
        //    viewModel.Count = dbCount;
        //    viewModel.CheckPeopl = CheckPeopl.ToJson();
        //    return View(viewModel);
        //}

        public ActionResult ConfirmApplyChange(string Id)
        {
            SupvisorJob SupvisorJob = new SupvisorJob();
            var SupvisorJobs = supvisorRep.GetByCondition(w => w.CustomId == Id && w.NeedApproveId == Id && w.ApproveType == ApproveType.STApproveCustom).OrderByDescending(o => o.CreateTime);
            if (SupvisorJobs != null && SupvisorJobs.Count() > 0)
            {
                SupvisorJob = SupvisorJobs.FirstOrDefault();
            }
            return View(SupvisorJob);// Content(Result.ToJson());

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
                if (!stCustomService.UpdateCustomStatus(applyChangeModel.SubmitId, applyChangeModel.Result, "", out erroMsg))
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


        public ActionResult PumpSystem(string CustomId)
        {
            STPumpSystemModel model = new STPumpSystemModel()
            {
                PumpSystems = new List<t_bp_pumpsystem>()
            };
            model.PumpSystems = pumpSysRep.GetByCondition(t => t.customid == CustomId);
            return View(model);
        }


        public ActionResult Carriervehicle(string CustomId)
        {
            STCarriervehicleModel model = new STCarriervehicleModel()
            {
                Carriervehicles = new List<t_bp_carriervehicle>()
            };
            model.Carriervehicles = carrieRvehicleRep.GetByCondition(t => t.customid == CustomId);
            return View(model);
        }


        public ActionResult Pumpvehicle(string CustomId)
        {
            STPumpvehicleModel model = new STPumpvehicleModel()
            {
                Pumpvehicles = new List<t_bp_pumpvehicle>()
            };
            model.Pumpvehicles = pumpVehicleRep.GetByCondition(t => t.customid == CustomId);
            return View(model);
        }


        public ActionResult AddPumpSystem()
        {
            return View();
        }

        public ActionResult AddCarriervehicle()
        {
            return View();

        }

        public ActionResult AddPumpvehicle()
        {
            return View();
        }

        public ActionResult CreateCustom()
        {
            return View();
        }


    }
}
