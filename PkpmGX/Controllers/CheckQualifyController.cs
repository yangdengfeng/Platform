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
using Dhtmlx.Model.TreeView;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class CheckQualifyController : PkpmController
    {
        ISysDictService sysDictServcie;
        ICheckUnitService checkUnitService;
        IPkpmConfigService pkpmConfigService;
        IRepsitory<t_bp_custom> rep;
        IRepsitory<t_bp_custom_tmp> repTmp;
        IRepsitory<t_bp_CusAchievement> CusAchievement;
        IRepsitory<t_bp_CusAward> CusAwards;
        IRepsitory<t_bp_CusPunish> CusPunish;
        IRepsitory<t_bp_CheckCustom> CheckCustom;
        IRepsitory<t_bp_CusChange> CusChange;
        IRepsitory<t_bp_People> peopleRep;
        IRepsitory<SupvisorJob> supvisorRep;
        IRepsitory<t_bp_Equipment> equRep;
        IRepsitory<t_bp_EquipmentTypeList> equipTypeList;
        IFileHandler fileHander;

        //private static string uploadInstStartWith = System.Configuration.ConfigurationManager.AppSettings["UploadInstStartWith"];
        // GET: CheckQualify
        public CheckQualifyController(ISysDictService sysDictServcie,
            ICheckUnitService checkUnitService,
            IRepsitory<t_bp_custom> rep,
            IRepsitory<t_bp_custom_tmp> repTmp,
            IRepsitory<t_bp_CusAchievement> CusAchievement,
            IRepsitory<t_bp_CusAward> CusAwards,
            IRepsitory<SupvisorJob> supvisorRep,
            IPkpmConfigService pkpmConfigService,
            IRepsitory<t_bp_CusPunish> CusPunish,
            IRepsitory<t_bp_CheckCustom> CheckCustom,
            IRepsitory<t_bp_CusChange> CusChange,
            IRepsitory<t_bp_People> peopleRep,
            IRepsitory<t_bp_Equipment> equRep,
            IRepsitory<t_bp_EquipmentTypeList> equipTypeList,
            IFileHandler fileHander,
            IUserService userService) : base(userService)
        {
            this.supvisorRep = supvisorRep;
            this.pkpmConfigService = pkpmConfigService;
            this.sysDictServcie = sysDictServcie;
            this.checkUnitService = checkUnitService;
            this.rep = rep;
            this.repTmp = repTmp;
            this.CusAchievement = CusAchievement;
            this.CusAwards = CusAwards;
            this.CusPunish = CusPunish;
            this.CheckCustom = CheckCustom;
            this.CusChange = CusChange;
            this.fileHander = fileHander;
            this.peopleRep = peopleRep;
            this.equRep = equRep;
            this.equipTypeList = equipTypeList;
            this.userService = userService;
        }


        // GET: CheckQualify
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
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Delete", "[停业]") { Img = "fa fa-times", Imgdis = "fa fa-times" });
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
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Screening", "[注销]") { Img = "fa fa-unlock-alt", Imgdis = "fa fa-unlock-alt" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("5"));
            }
            if (HaveButtonFromAll(buttons, "RelieveScreening"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("RelieveScreening", "[解除注销]") { Img = "fa fa-unlock", Imgdis = "fa fa-unlock" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("6"));
            }


            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult Search(CheckQualifySearchModel searchModel)
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
                DhtmlxGridRow row = new DhtmlxGridRow(custom.ID.ToString());
                row.AddCell(string.Empty);
                row.AddCell((pos + i + 1).ToString());//序号
                //row.AddCell("{0}({1})".Fmt(custom.NAME, custom.ID));
                if (custom.IsUse == 0)
                {
                    row.AddCell(new DhtmlxGridCell("{0}".Fmt(custom.NAME), false).AddCellAttribute("style", "color:red"));
                }
                else
                {
                    row.AddCell("{0}".Fmt(custom.NAME));
                }//机构名称
                row.AddCell(custom.DETECTNUM);//检测证书编号
                row.AddCell(custom.MEASNUM);//计量证书编号
                row.AddCell(custom.FR);//法人
                row.AddCell(custom.TEL);//联系电话
                row.AddCell(SysDictUtility.GetKeyFromDic(compayTypes, custom.APPROVALSTATUS));//状态

                //注释 by ydf 2019-04-02
                //Dictionary<string, string> dict = new Dictionary<string, string>();
                ////已递交	1
                //if (HaveButtonFromAll(buttons, "ApplyChange") && checkUnitService.CanApplyChangeCustom(custom.APPROVALSTATUS))
                //{
                //    dict.Add("[申请修改]", "applyChange({0},\"{1}\")".Fmt(custom.ID, custom.NAME));
                //}
                //if (HaveButtonFromAll(buttons, "ConfirmApplyChange") && (custom.APPROVALSTATUS == "5"))
                //{
                //    dict.Add("[审核申请修改]", "confirmApplyChange({0},\"{1}\")".Fmt(custom.ID, custom.NAME));
                //}
                //row.AddLinkJsCells(dict);

                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));

                //申请修改已批准 6。新增/审核未通过能编辑（小罗）。未递交也能编辑（老系统）。
                if (HaveButtonFromAll(buttons, "Edit") && custom.APPROVALSTATUS == "0")
                {
                    row.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                }
                else
                {
                    row.AddCell(string.Empty);
                }

                //对机构进行修改后，进行审核操作
                if (HaveButtonFromAll(buttons, "Audit") && custom.APPROVALSTATUS == "1")
                {
                    row.AddLinkJsCell("审核", "AuditCustom(\"{0}\")".Fmt(custom.ID));
                }
                else
                {
                    row.AddCell(string.Empty);
                }

                //row.AddLinkJsCell("打印设置", "setInstPrintConfig(\"{0}\",\"{1}\")".Fmt(custom.ID, custom.bgfs));
                if (!custom.ID.StartsWith(pkpmConfigService.UploadInstStartWith))
                {
                    row.AddLinkJsCell("转为正式账号", "setInstFormal(\"{0}\",\"{1}\")".Fmt(custom.ID, custom.NAME));
                }
                else
                {
                    row.AddCell(string.Empty);
                }
                if (HaveButtonFromAll(buttons, "UpdateName"))
                {
                    row.AddLinkJsCell("名称修改", "ChangeName(\"{0}\",\"{1}\")".Fmt(custom.ID, custom.NAME));
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

        private SearchResult<ChekQualityUIModel> GetSearchResult(CheckQualifySearchModel searchModel)
        {

            var viewModel = new CheckQualifyViewModels();
            viewModel.CompanyStatus = sysDictServcie.GetDictsByKey("customStatus");
            var predicate = PredicateBuilder.True<t_bp_custom>();

            #region 动态查询
            //过滤已经删除的机构
            predicate = predicate.And(tp => tp.data_status == null || tp.data_status != "-1");

            if(searchModel.IsUse == 0)//机构为注销状态
            {
                predicate = predicate.And(t => t.IsUse == searchModel.IsUse);
            }
            else if(searchModel.IsUse == 2)//机构为停业状态
            {
                predicate = predicate.And(t => t.IsUse == searchModel.IsUse);
            }
            else
            {
                predicate = predicate.And(t => t.IsUse != 0 && t.IsUse != 2);
            }

            if(searchModel.ParentId == -1 )
            {
                predicate = predicate.And(t => t.ParentId == -1 );
            }
            else if(searchModel.ParentId == 2)
            {
                predicate = predicate.And(t => t.ParentId != -1);
            }

            if (!searchModel.companyType.IsNullOrEmpty())
            {
                predicate = predicate.And(t => t.companytype == searchModel.companyType);
            }

            //if (!searchModel.CompanyQualification.IsNullOrEmpty())
            //{
            //var CompanyQualification = searchModel.CompanyQualification.Split(',').ToList();
            //foreach (var item in CompanyQualification)
            //{
            //    predicate = predicate.And(t => t.zzlbmc.Contains(item));
            //}
            //    predicate = predicate.And(t => searchModel.CompanyQualification.Contains(t.zzlbmc));
            //}

            if (!searchModel.CompanyQualification.IsNullOrEmpty())
            {
                var CompanyQualification = searchModel.CompanyQualification.Split(',').ToList();
                predicate = predicate.And(t => CompanyQualification.Contains(t.zzlbmc));
            }


            if (searchModel.Area != "-1" && !string.IsNullOrWhiteSpace(searchModel.Area))
            {
                var Areas = searchModel.Area.Split(',').ToList();
                //foreach (var item in Areas)
                //{
                //    predicate = predicate.And(t => t.area.Contains(item));
                //}
                predicate = predicate.And(t => Areas.Contains(t.area));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.status))
            {

                predicate = predicate.And(t => t.APPROVALSTATUS == searchModel.status);
            }
            if (!string.IsNullOrWhiteSpace(searchModel.CheckUnitName))
            {

                predicate = predicate.And(t => t.ID == searchModel.CheckUnitName);
            }
            if (!string.IsNullOrWhiteSpace(searchModel.CMANum))
            {

                predicate = predicate.And(t => t.MEASNUM != null && t.MEASNUM.Contains(searchModel.CMANum));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.CheckCertNum))
            {
                predicate = predicate.And(t => t.DETECTNUM != null && t.DETECTNUM.Contains(searchModel.CheckCertNum));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.LegalPeople))
            {
                predicate = predicate.And(t => t.FR == searchModel.LegalPeople);
            }
            if (!string.IsNullOrWhiteSpace(searchModel.QuantityCategory))
            {
                predicate = predicate.And(t => t.zzlbmc.Contains(searchModel.QuantityCategory));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.CheckCode))
            {
                predicate = predicate.And(t => t.wjlr.Contains(searchModel.CheckCode));
            }
            if (searchModel.CMADStartDt.HasValue)
            {
                predicate = predicate.And(t => t.measnumStartDate >= searchModel.CMADStartDt.Value);
            }
            if (searchModel.CMADEndDt.HasValue)
            {
                predicate = predicate.And(t => t.measnumEndDate <= searchModel.CMADEndDt.Value);
            }
            if (searchModel.CheckCertStartDt.HasValue)
            {
                predicate = predicate.And(t => t.detectnumStartDate >= searchModel.CheckCertStartDt.Value);
            }
            if (searchModel.CheckCertEndDt.HasValue)
            {
                predicate = predicate.And(t => t.detectnumEndDate <= searchModel.CheckCertEndDt.Value);
            }

            var instFilter = GetCurrentInstFilter();
            if (instFilter.NeedFilter && instFilter.FilterInstIds.Count() > 0)
            {
                predicate = predicate.And(t => instFilter.FilterInstIds.Contains(t.ID));
            }

            if (IsCurrentSuperVisor())
            {
                var area = GetCurrentAreas();
                var userInArea = checkUnitService.GetUnitByArea(area);
                var insts = userInArea.Select(t => t.Key).ToList();
                if (insts != null && insts.Count >= 0)
                {
                    predicate = predicate.And(t => insts.Contains(t.ID));
                }
            }
            #endregion

            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            int count = searchModel.count.HasValue ? searchModel.count.Value : 30;

            string[] columns = { "CheckBox", "SeqNo", "NAME", "DETECTNUM", "MEASNUM", "FR", "TEL", "APPROVALSTATUS" };
            string orderby = string.Empty;
            string orderByAcs = string.Empty;
            bool isDes = true;
            string sortProperty = "ID";//"DETECTNUM";
            if (searchModel.orderColInd.HasValue
                && !string.IsNullOrWhiteSpace(searchModel.direct)
                && searchModel.orderColInd.Value <= columns.Length)
            {
                if (searchModel.direct == "asc")
                {
                    sortProperty = orderby = columns[searchModel.orderColInd.Value];
                    isDes = false;
                }
                if (searchModel.direct == "des")
                {
                    sortProperty = orderByAcs = columns[searchModel.orderColInd.Value];
                    isDes = true;
                }
            }



            PagingOptions<t_bp_custom> pagingOption = new PagingOptions<t_bp_custom>(pos, count, sortProperty, isDes);


            var customs = rep.GetByConditionSort<ChekQualityUIModel>(predicate, r => new
            {
                r.ID,
                r.NAME,
                r.DETECTNUM,
                r.MEASNUM,
                r.FR,
                r.TEL,
                r.APPROVALSTATUS,
                r.bgfs,
                r.IsUse
            },
               pagingOption);

            return new SearchResult<Models.ChekQualityUIModel>(pagingOption.TotalItems, customs);
        }

        /// <summary>
        /// 单位业绩
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult cusachievement(string id)
        {
            var CustomCusAchievement = CusAchievement.GetByCondition(r => r.CustomId == id);
            DhtmlxGrid grid = new DhtmlxGrid();
            for (int i = 0; i < CustomCusAchievement.Count; i++)
            {

                var myObj = CustomCusAchievement[i];
                DhtmlxGridRow row = new DhtmlxGridRow(myObj.id.ToString());
                row.AddCell((i + 1).ToString());
                row.AddCell(myObj.AchievementTime);
                row.AddCell(myObj.AchievementContent);
                row.AddCell(myObj.AchievementRem);
                row.AddLinkJsCell("删除", "cusachievement_delete({0})".Fmt(myObj.id));
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
            var CustomCusAchievement = CusAchievement.GetByCondition(r => r.CustomId == id);
            for (int i = 0; i < CustomCusAchievement.Count; i++)
            {
                var cusAchievement = new CusachievementModel();
                var myObj = CustomCusAchievement[i];
                cusAchievement.Number = i + 1;
                cusAchievement.AchievementTime = myObj.AchievementTime;
                cusAchievement.AchievementContent = myObj.AchievementContent;
                cusAchievement.AchievementRem = myObj.AchievementRem;
                cusAchievement.Id = myObj.id.ToString();
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
            var myCusawards = CusAwards.GetByCondition(r => r.CustomId == id);
            DhtmlxGrid grid = new DhtmlxGrid();
            for (int i = 0; i < myCusawards.Count; i++)
            {
                var myObj = myCusawards[i];
                DhtmlxGridRow row = new DhtmlxGridRow(myObj.id.ToString());
                row.AddCell((i + 1).ToString());
                row.AddCell(myObj.AwaDate.HasValue ? myObj.AwaDate.Value.ToString("yyyy-MM-dd") : string.Empty);
                row.AddCell(myObj.AwaName);
                row.AddCell(myObj.AwaUnit);
                row.AddCell(myObj.AwaContent);
                row.AddCell(myObj.AwaRem);
                row.AddLinkJsCell("删除", "cusawards_delete({0})".Fmt(myObj.id));
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
        public List<CusawardsModel> CusawardsDetails(string id)
        {
            var myCusawards = CusAwards.GetByCondition(r => r.CustomId == id);
            List<CusawardsModel> cusAwards = new List<CusawardsModel>();
            for (int i = 0; i < myCusawards.Count; i++)
            {
                CusawardsModel model = new CusawardsModel();
                var myObj = myCusawards[i];
                model.Number = i + 1;
                model.AwaDate = myObj.AwaDate.HasValue ? myObj.AwaDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                model.AwaName = myObj.AwaName;
                model.AwaUnit = myObj.AwaUnit;
                model.AwaContent = myObj.AwaContent;
                model.AwaRem = myObj.AwaRem;
                model.Id = myObj.id.ToString();
                cusAwards.Add(model);
            }
            return cusAwards;
        }

        /// <summary>
        /// 处罚情况
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult cuspunish(string id)
        {

            var myCuspunish = CusPunish.GetByCondition(r => r.CustomId == id);
            DhtmlxGrid grid = new DhtmlxGrid();

            for (int i = 0; i < myCuspunish.Count; i++)
            {
                var myObj = myCuspunish[i];
                DhtmlxGridRow row = new DhtmlxGridRow(myObj.id.ToString());
                row.AddCell((i + 1).ToString());
                row.AddCell(myObj.PunDate.HasValue ? myObj.PunDate.Value.ToString("yyyy-MM-dd") : string.Empty);
                row.AddCell(myObj.PunName);
                row.AddCell(myObj.PunUnit);
                row.AddCell(myObj.PunContent);
                row.AddCell(myObj.PunRem);
                row.AddLinkJsCell("删除", "cuspunish_delete({0})".Fmt(myObj.id));
                //row.AddLinkJsCell("增加", "Cuspunish_add()");
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        /// <summary>
        /// 处罚情况（用于检测机构查看界面）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CuspunishModel> CuspunishDetails(string id)
        {
            var cusPunishs = new List<CuspunishModel>();
            var myCuspunish = CusPunish.GetByCondition(r => r.CustomId == id);
            for (int i = 0; i < myCuspunish.Count; i++)
            {
                var cusPunish = new CuspunishModel();
                var myObj = myCuspunish[i];
                cusPunish.Number = i + 1;
                cusPunish.Id = myObj.id.ToString();
                cusPunish.PunContent = myObj.PunContent;
                cusPunish.PunDate = myObj.PunDate.HasValue ? myObj.PunDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                cusPunish.PunName = myObj.PunName;
                cusPunish.PunUnit = myObj.PunUnit;
                cusPunish.PunRem = myObj.PunRem;
                cusPunishs.Add(cusPunish);
            }
            return cusPunishs;
        }

        /// <summary>
        /// 复查情况
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult checkcustom(string id)
        {
            var myCheckCustom = CheckCustom.GetByCondition(r => r.CustomId == id);
            DhtmlxGrid grid = new DhtmlxGrid();
            for (int i = 0; i < myCheckCustom.Count; i++)
            {
                var myObj = myCheckCustom[i];
                DhtmlxGridRow row = new DhtmlxGridRow(myObj.id.ToString());
                row.AddCell((i + 1).ToString());
                row.AddCell(myObj.CheDate.HasValue ? myObj.CheDate.Value.ToString("yyyy-MM-dd") : string.Empty);
                row.AddCell(myObj.CheResult);
                row.AddCell(myObj.CheRem);
                row.AddLinkJsCell("删除", "checkcustom_delete({0})".Fmt(myObj.id));
                row.AddLinkJsCell("增加", "Cuspunish_add()");
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        /// <summary>
        /// 复查情况（用于检测机构查看界面）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CheckcustomModel> CheckCustomDetails(string id)
        {
            List<CheckcustomModel> checkCustoms = new List<CheckcustomModel>();
            var myCheckCustom = CheckCustom.GetByCondition(r => r.CustomId == id);
            for (int i = 0; i < myCheckCustom.Count; i++)
            {
                CheckcustomModel checkCustom = new CheckcustomModel();
                var myObj = myCheckCustom[i];
                checkCustom.Id = myObj.id.ToString();
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
            var myCusChange = CusChange.GetByCondition(r => r.CustomId == id);
            DhtmlxGrid grid = new DhtmlxGrid();

            for (int i = 0; i < myCusChange.Count; i++)
            {

                var myObj = myCusChange[i];
                DhtmlxGridRow row = new DhtmlxGridRow(myObj.id.ToString());
                row.AddCell((i + 1).ToString());
                row.AddCell(myObj.ChaDate.HasValue ? myObj.ChaDate.Value.ToString("yyyy-MM-dd") : string.Empty);
                row.AddCell(myObj.ChaContent);
                row.AddCell(myObj.ChaRem);
                row.AddLinkJsCell("编辑", "cuschange_edit(\"{0}\" )".Fmt(myObj.id));
                row.AddLinkJsCell("删除", "cuschange_delete({0})".Fmt(myObj.id));
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
            var myCusChange = CusChange.GetByCondition(r => r.CustomId == id);
            for (int i = 0; i < myCusChange.Count; i++)
            {
                CuschangeModel cusChange = new CuschangeModel();
                var myObj = myCusChange[i];
                cusChange.Id = myObj.id.ToString();
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
            return View(model);
        }


        private CheckQualifyViewModels GetCheckQualifyViewModel(string id)
        {
            var viewModel = new CheckQualifyViewModels();
            viewModel.CompayTypeList = new Dictionary<string, List<SysDict>>();
            //viewModel.CompayTypeList.Add("CustomArea", sysDictServcie.GetDictsByKey("CustomArea"));
            viewModel.Region = userService.GetAllArea();
            viewModel.CompayTypeList.Add("unitQualifications", sysDictServcie.GetDictsByKey("unitQualifications"));
            viewModel.CompayTypeList.Add("personnelTitles", sysDictServcie.GetDictsByKey("personnelTitles"));
            viewModel.CompayTypeList.Add("yesNo", sysDictServcie.GetDictsByKey("yesNo"));
            viewModel.CompayTypeList.Add("CompanyType", sysDictServcie.GetDictsByKey("CompanyType"));
            viewModel.myT_bp_Custom = rep.GetSingleByCondition<CheckQuailityDetailModel>(r => r.ID == id, r => new
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

            //viewModel.myT_bp_Custom.JSTIILE = sysDictServcie.GetDictsByKey("personnelTitles", viewModel.myT_bp_Custom.JSTIILE);
            //viewModel.myT_bp_Custom.ZLTITLE = sysDictServcie.GetDictsByKey("personnelTitles", viewModel.myT_bp_Custom.ZLTITLE);
            //计算人员信息相关人数
            var predicate = PredicateBuilder.True<t_bp_People>();
            predicate = predicate.And(p => p.Customid == id);
            predicate = predicate.And(tp => tp.data_status == null || tp.data_status != "-1");
            var peoples = this.peopleRep.GetByCondition(predicate);
            int PERCOUNT = peoples.Count;//总人数
            int hasNumPerCount = peoples.Where(w => w.ishaspostnum == "1").ToList().Count;//持证人数
            int REGYTSTA = peoples.Where(w => w.iszcgccs == "1").ToList().Count;//注册岩土工程师人数
            int REGJGSTA = peoples.Where(w => w.iszcgccs == "2").ToList().Count;//注册结构工程师
            int HEIPERCOUNT = peoples.Where(w => w.Title == "3").ToList().Count;//高级职称
            int MIDPERCOUNT = peoples.Where(w => w.Title == "2").ToList().Count;//中级职称
            viewModel.myT_bp_Custom.PERCOUNT = PERCOUNT.ToString();
            viewModel.myT_bp_Custom.hasNumPerCount = hasNumPerCount.ToString();
            viewModel.myT_bp_Custom.REGJGSTA = REGJGSTA.ToString();
            viewModel.myT_bp_Custom.REGYTSTA = REGYTSTA.ToString();
            viewModel.myT_bp_Custom.MIDPERCOUNT = MIDPERCOUNT.ToString();
            viewModel.myT_bp_Custom.HEIPERCOUNT = HEIPERCOUNT.ToString();

            viewModel.myT_bp_Custom.JSTIILE = sysDictServcie.GetDictsByKey("personnelTitles", viewModel.myT_bp_Custom.JSTIILE);
            viewModel.myT_bp_Custom.ZLTITLE = sysDictServcie.GetDictsByKey("personnelTitles", viewModel.myT_bp_Custom.ZLTITLE);
            viewModel.myT_bp_Custom.instrumentNum = equRep.GetByCondition(p => p.customid == id).Count.ToString();//设备套数
            viewModel.Sex = sysDictServcie.GetDictsByKey("Sex");
            viewModel.CheckWork = sysDictServcie.GetDictsByKey("CheckWork");
            //viewModel.PersonnelStaff = sysDictServcie.GetDictsByKey("PersonnelStaff");
            viewModel.Cusachievement = CusachievementDetails(viewModel.myT_bp_Custom.ID);//单位业绩
            viewModel.Cusawards = CusawardsDetails(viewModel.myT_bp_Custom.ID);//获奖情况
            viewModel.Cuspunish = CuspunishDetails(viewModel.myT_bp_Custom.ID);//处罚情况
            viewModel.Checkcustom = CheckCustomDetails(viewModel.myT_bp_Custom.ID);//复查情况
            viewModel.Cuschange = CusChangeDetails(viewModel.myT_bp_Custom.ID);//变更情况
            viewModel.File = FileGrid(viewModel.myT_bp_Custom.ID,
                viewModel.myT_bp_Custom.businessnumPath,
                viewModel.myT_bp_Custom.DETECTPATH,
                viewModel.myT_bp_Custom.MEASNUMPATH,
                viewModel.myT_bp_Custom.instrumentpath,
                viewModel.myT_bp_Custom.shebaopeoplelistpath);

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



        // POST: CheckQualify/Edit/5

        public ActionResult addFRWorkExperience()
        {
            return View();
        }

        public ActionResult addJSWorkExperience()
        {
            return View();
        }

        public ActionResult AddCusAchievement()
        {
            return View();
        }

        public ActionResult AddCusAward()
        {
            return View();
        }

        public ActionResult AddCusPunish()
        {
            return View();
        }

        public ActionResult AddCheckCustom()
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

        public ActionResult UnitQualification(string id, string isSaveButton)
        {
            GetUnitQualificationModel model = new GetUnitQualificationModel();
            List<ChildrenModels> ChildrenModels = new List<ChildrenModels>();

            var custom = rep.GetSingleByCondition<CheckQuailifyShortModel>(r => r.ID == id, r => new { r.wjlr, r.zzlbmc });
            model.Wjlr = custom.wjlr;
            var zzlbmc = custom.zzlbmc;
            var ssss = sysDictServcie.GetDictsByKey("uniQualifications");
            foreach (var QualiFications in ssss)
            {
                ChildrenModels children = new ChildrenModels();
                bool isCheck = false;
                if (zzlbmc != null && zzlbmc != "")
                {
                    if (zzlbmc.IndexOf(QualiFications.Name) > -1)
                    {
                        isCheck = true;
                    }
                }
                children.label = QualiFications.Name;
                children.check = isCheck;
                children.spread = false;
                ChildrenModels.Add(children);
            }

            var ChildrenModel = ChildrenModels.ToJson();
            model.QualificationTree = ChildrenModels.ToJson();
            model.QualificationTree = model.QualificationTree.Replace("check", "checked");

            if (!isSaveButton.IsNullOrEmpty())
            {
                model.IsSaveButton = isSaveButton == "1" ? true : false;
            }

            model.CustomId = id;
            return View(model);
        }

        // GET: CheckQualify/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: CheckQualify/Delete/5
        //[HttpPost]
        //public ActionResult Delete(string selectedId, FormCollection collection)
        //{
        //    ControllerResult result = ControllerResult.SuccResult;
        //    try
        //    {
        //        string erroMsg = string.Empty;
        //        if (!checkUnitService.DeletePathsIntoUnit(selectedId, out erroMsg))
        //        {
        //            result = ControllerResult.FailResult;
        //            result.ErroMsg = erroMsg;
        //        }
        //        else
        //        {
        //            LogUserAction("对机构ID为{0}进行了删除操作".Fmt(selectedId));
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        result = ControllerResult.FailResult;
        //        result.ErroMsg = ex.Message;
        //    }

        //    return Content(result.ToJson());
        //}

        public List<FileGridModel> FileGrid(string id,
           string businessnumPath,
           string DETECTPATH,
           string MEASNUMPATH,
           string instrumentpath,
           string shebaopeoplelistpath)
        {
            var files = new List<FileGridModel>();
            var cuntom = rep.GetById(id);
            string[,] FileRangeAr = new string[5, 3];
            FileRangeAr[0, 0] = "工商营业执照号码";
            FileRangeAr[1, 0] = "检测机构资质证书";
            FileRangeAr[2, 0] = "计量认证证书";
            FileRangeAr[3, 0] = "仪器设备总（台）套数总台帐";
            FileRangeAr[4, 0] = "社保人员明细";

            FileRangeAr[0, 1] = cuntom.businessnumPath;
            FileRangeAr[1, 1] = cuntom.DETECTPATH;
            FileRangeAr[2, 1] = cuntom.MEASNUMPATH;
            FileRangeAr[3, 1] = cuntom.instrumentpath;
            FileRangeAr[4, 1] = cuntom.shebaopeoplelistpath;

            FileRangeAr[0, 2] = businessnumPath;
            FileRangeAr[1, 2] = DETECTPATH;
            FileRangeAr[2, 2] = MEASNUMPATH;
            FileRangeAr[3, 2] = instrumentpath;
            FileRangeAr[4, 2] = shebaopeoplelistpath;

            // DhtmlxGrid grid = new DhtmlxGrid();

            for (int i = 0; i < 5; i++)
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
                        model.Type = "DETECTPATH";
                        break;
                    case 2:
                        model.Type = "MEASNUMPATH";
                        break;
                    case 3:
                        model.Type = "instrumentpath";
                        model.Modify = 1;
                        break;
                    case 4:
                        model.Type = "shebaopeoplelistpath";
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


        //批量返回状态
        [HttpPost]
        public ActionResult SetInstState(string selectedId, string state, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkUnitService.SetInstSendState(selectedId, state, out erroMsg))
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


        /// <summary>
        /// 根据ID获取机构信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private CheckQuailityDetailModel GetCheckQualifyDetailModel(string id)
        {
            CheckQuailityDetailModel model = null;
            model = rep.GetSingleByCondition<CheckQuailityDetailModel>(r => r.ID == id, r => new
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
                r.shebaopeoplelistpath,
                r.APPROVALSTATUS,
                r.EquclassId
            });

            return model;
        }
        /// <summary>
        /// 根据ID获取临时机构信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private CheckQuailityDetailModel GetCheckQualifyDetailTmpModel(string id)
        {
            CheckQuailityDetailModel model = null;
            model = repTmp.GetSingleByCondition<CheckQuailityDetailModel>(r => r.ID == id, r => new
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
                r.shebaopeoplelistpath,
                r.EquclassId
            });

            return model;
        }

        /// <summary>
        /// 比较两实体的差异
        /// </summary>
        /// <param name="CustomId"></param>
        /// <returns></returns>
        public List<ColumnsDiffModel> CompareEntityDiff(string CustomId)
        {
            List<ColumnsDiffModel> cdmList = new List<ColumnsDiffModel>();

            try
            {
                CheckQuailityDetailModel cqdModel = GetCheckQualifyDetailModel(CustomId);
                CheckQuailityDetailModel cqdModelTmp = GetCheckQualifyDetailTmpModel(CustomId);
                if (cqdModelTmp == null) return cdmList;

                ColumnsDiffModel cdm = null;
                string get_old = string.Empty;
                string get_new = string.Empty;
                PropertyInfo[] mPi = typeof(CheckQuailityDetailModel).GetProperties();
                foreach (PropertyInfo pi in mPi)
                {
                    if (pi.Name.ToUpper().EndsWith("PATH") ||
                        pi.Name.ToUpper().Contains("MEASNUMSTARTDATE") ||
                        pi.Name.ToUpper().Contains("APPROVALSTATUS")) continue; //排除不需比较的字段

                    if (pi.Name.ToUpper().EndsWith("DATE"))
                    {
                        get_old = pi.GetValue(cqdModel, null) == null ? "" : Convert.ToDateTime(pi.GetValue(cqdModel, null)).ToString("yyyy-MM-dd");
                        get_new = pi.GetValue(cqdModelTmp, null) == null ? "" : Convert.ToDateTime(pi.GetValue(cqdModelTmp, null)).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        get_old = pi.GetValue(cqdModel, null) == null ? "" : pi.GetValue(cqdModel, null).ToString();
                        get_new = pi.GetValue(cqdModelTmp, null) == null ? "" : pi.GetValue(cqdModelTmp, null).ToString();
                    }

                    if (get_old != get_new)
                    {
                        cdm = new ColumnsDiffModel
                        {
                            Column = pi.Name, //字段
                            ColumnName = pi.GetCustomAttribute<DisplayNameAttribute>().DisplayName, //字段自定义名
                            OldValue = get_old, //字段旧值
                            NewValue = get_new //字段新值
                        };

                        cdmList.Add(cdm);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return cdmList;
        }

        /// <summary>
        /// 审核，获取修改值进行对比展示
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult AuditCustom(string Id)
        {
            string str = string.Empty;
            DhtmlxForm dForm = new DhtmlxForm();
            var itemcols = CompareEntityDiff(Id);
            if (itemcols.Count > 0)
            {

                var companyTypes = sysDictServcie.GetDictsByKey("CompanyType");
                var yesNo = sysDictServcie.GetDictsByKey("yesNo");
                var personnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");

                int index = 1;
                dForm.AddDhtmlxFormItem(new DhtmlxFormLabel("序号").AddStringItem("offsetLeft", "50"));
                foreach (var col in itemcols)
                {
                    dForm.AddDhtmlxFormItem(new DhtmlxFormInput(col.Column + "xuhao", "", (index++).ToString()).AddStringItem("offsetLeft", "50").AddStringItem("inputWidth", "50").AddStringItem("readonly", "true"));
                }

                dForm.AddDhtmlxFormItem(new DhtmlxFormNewcolumn());
                dForm.AddDhtmlxFormItem(new DhtmlxFormLabel("名称").AddStringItem("offsetLeft", "50"));
                foreach (var col in itemcols)
                {
                    dForm.AddDhtmlxFormItem(new DhtmlxFormInput(col.Column + "name", "", col.ColumnName).AddStringItem("offsetLeft", "50").AddStringItem("readonly", "true"));
                }
                dForm.AddDhtmlxFormItem(new DhtmlxFormButton("N", "审核不通过").AddStringItem("offsetLeft", "100"));

                dForm.AddDhtmlxFormItem(new DhtmlxFormNewcolumn());
                dForm.AddDhtmlxFormItem(new DhtmlxFormLabel("原始值").AddStringItem("offsetLeft", "50"));
                foreach (var col in itemcols)
                {
                    string value = string.Empty;
                    if (col.Column == "companytype" && !string.IsNullOrEmpty(col.OldValue))
                    {
                        value = companyTypes.Where(x => x.KeyValue == col.OldValue).ToList().FirstOrDefault().Name;
                    }
                    else if (col.Column == "ispile" && !string.IsNullOrEmpty(col.OldValue))
                    {
                        value = yesNo.Where(x => x.KeyValue == col.OldValue).ToList().FirstOrDefault().Name;
                    }
                    else if (col.Column == "JSTIILE" && !string.IsNullOrEmpty(col.OldValue))
                    {
                        value = personnelTitles.Where(x => x.KeyValue == col.OldValue).ToList().FirstOrDefault().Name;
                    }
                    else if (col.Column == "ZLTITLE" && !string.IsNullOrEmpty(col.OldValue))
                    {
                        value = personnelTitles.Where(x => x.KeyValue == col.OldValue).ToList().FirstOrDefault().Name;
                    }
                    else
                    {
                        value = col.OldValue;
                    }

                    dForm.AddDhtmlxFormItem(new DhtmlxFormInput(col.Column + "|old", "", value).AddStringItem("offsetLeft", "50").AddStringItem("readonly", "true"));
                }
                dForm.AddDhtmlxFormItem(new DhtmlxFormButton("Y", "审核通过").AddStringItem("offsetLeft", "115"));

                dForm.AddDhtmlxFormItem(new DhtmlxFormNewcolumn());
                dForm.AddDhtmlxFormItem(new DhtmlxFormLabel("修改值").AddStringItem("offsetLeft", "50"));
                foreach (var col in itemcols)
                {
                    string value = string.Empty;
                    if (col.Column == "companytype" && !string.IsNullOrEmpty(col.NewValue))
                    {
                        value = companyTypes.Where(x => x.KeyValue == col.NewValue).ToList().FirstOrDefault().Name;
                    }
                    else if (col.Column == "ispile" && !string.IsNullOrEmpty(col.NewValue))
                    {
                        value = yesNo.Where(x => x.KeyValue == col.NewValue).ToList().FirstOrDefault().Name;
                    }
                    else if (col.Column == "JSTIILE" && !string.IsNullOrEmpty(col.NewValue))
                    {
                        value = personnelTitles.Where(x => x.KeyValue == col.NewValue).ToList().FirstOrDefault().Name;
                    }
                    else if (col.Column == "ZLTITLE" && !string.IsNullOrEmpty(col.NewValue))
                    {
                        value = personnelTitles.Where(x => x.KeyValue == col.NewValue).ToList().FirstOrDefault().Name;
                    }
                    else
                    {
                        value = col.NewValue;
                    }

                    dForm.AddDhtmlxFormItem(new DhtmlxFormInput(col.Column + "|new", "", value).AddStringItem("offsetLeft", "50").AddStringItem("readonly", "true"));
                }
            }
            else
            {
                dForm.AddDhtmlxFormItem(new DhtmlxFormLabel("该机构信息无修改项，无需审核！").AddStringItem("offsetLeft", "300"));
            }

            str = dForm.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        /// <summary>
        /// 审核操作
        /// </summary>
        /// <param name="FormCol"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AuditCustom(FormCollection FormCol)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;
            string opr = FormCol["Operate"].ToString();

            var companyTypes = sysDictServcie.GetDictsByKey("CompanyType");
            var yesNo = sysDictServcie.GetDictsByKey("yesNo");
            var personnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");

            Dictionary<string, List<SysDict>> dic = new Dictionary<string, List<SysDict>>();
            dic.Add("companyTypes", companyTypes);
            dic.Add("yesNo", yesNo);
            dic.Add("personnelTitles", personnelTitles);

            var saveResult = checkUnitService.AuditCustom(FormCol, dic, out errorMsg);
            if (!saveResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("进行了机构信息审核操作：{0}，机构ID为{1}".Fmt(opr, FormCol["Id"].ToString()));
            }

            return Content(result.ToJson());
        }

        /// <summary>
        /// 检查机构信息是否能被修改
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CheckEdit(string Id)
        {
            var model = GetCheckQualifyDetailModel(Id);
            return Content(model.ToJson());
        }

        public ActionResult PrintSetConfig(CheckQualifyPrintSetConfigViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public ActionResult PrintSetConfig(string CustomId, string Cbgfs)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkUnitService.SetPrintConfig(CustomId, Cbgfs, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
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
                    ApproveType = ApproveType.ApproveCustom,
                    CreateBy = GetCurrentUserId(),
                    CustomId = applyChangeModel.SubmitId,
                    CreateTime = DateTime.Now,
                    NeedApproveId = applyChangeModel.SubmitId.ToString(),
                    NeedApproveStatus = NeedApproveStatus.CreateForChangeApply,
                    SubmitName = applyChangeModel.SubmitName,
                    SubmitText = applyChangeModel.SubmitText
                };


                string erroMsg = string.Empty;
                if (!checkUnitService.ApplyChangeForCustom(job, applyChangeModel.SubmitId, out erroMsg))
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
        [HttpPost]
        public ActionResult Screening(string selectedId, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkUnitService.SetInstScreeningState(selectedId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对机构ID为{0}进行了状态屏蔽操作".Fmt(selectedId));
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
        public ActionResult Stopping(string selectedId, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkUnitService.SetInstStoppingState(selectedId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对机构ID为{0}进行了停业操作".Fmt(selectedId));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }
        // POST: CheckQualify/Edit/5

        public ActionResult RelieveScreening(string selectedId, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkUnitService.SetInstRelieveScreeningSate(selectedId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对机构ID为{0}进行了状态解除屏蔽操作".Fmt(selectedId));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }
            return Content(result.ToJson());
        }

        // GET: CheckQualify/Edit/5
        public ActionResult Edit(string id)
        {
            var viewModel = GetCheckQualifyViewModel(id);

            return View(viewModel);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(CheckCustomSaveViewModel viewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            //CheckCustomSaveModel editModel = new CheckCustomSaveModel();
            CheckCustomTmpSaveModel editModel = new CheckCustomTmpSaveModel();
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
                editModel.custom = new t_bp_custom_tmp()
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
                    DETECTNUM = viewModel.DETECTNUM,
                    APPLDATE = viewModel.APPLDATE,
                    DETECTPATH = viewModel.DETECTPATH,
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
                    businessnumPath = viewModel.businessnumPath,
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
                    update_time = DateTime.Now,
                    EquclassId = viewModel.EquclassId
                };
                #endregion

                editModel.CusAchievement = new List<t_bp_CusAchievement>();
                editModel.CusPunish = new List<t_bp_CusPunish>();
                editModel.CusAward = new List<t_bp_CusAward>();
                editModel.CusChange = new List<t_bp_CusChange>();
                editModel.CheckCustom = new List<t_bp_CheckCustom>();

                if (viewModel.cusachievement != null)
                {
                    foreach (var cusAchieeve in viewModel.cusachievement)
                    {
                        cusAchieeve.CustomId = viewModel.ID;
                        editModel.CusAchievement.Add(cusAchieeve);
                    }
                }

                if (viewModel.cuspunish != null)
                {
                    foreach (var cusPunis in viewModel.cuspunish)
                    {
                        cusPunis.CustomId = viewModel.ID;
                        editModel.CusPunish.Add(cusPunis);
                    }
                }

                if (viewModel.cusawards != null)
                {
                    foreach (var cusAwar in viewModel.cusawards)
                    {
                        cusAwar.CustomId = viewModel.ID;
                        editModel.CusAward.Add(cusAwar);
                    }
                }

                if (viewModel.cuschange != null)
                {
                    foreach (var cusChan in viewModel.cuschange)
                    {
                        cusChan.CustomId = viewModel.ID;
                        editModel.CusChange.Add(cusChan);
                    }
                }

                if (viewModel.checkcustom != null)
                {
                    foreach (var cusCheck in viewModel.checkcustom)
                    {
                        cusCheck.CustomId = viewModel.ID;
                        editModel.CheckCustom.Add(cusCheck);
                    }
                }

                string erroMsg = string.Empty;
                if (!checkUnitService.EditCustom(editModel, out erroMsg))
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

        public ActionResult Export(CheckQualifySearchModel searchModel, int? fileFormat)
        {
            var data = GetSearchResult(searchModel);

            // 改动2：创建导出类实例（而非 DhtmlxGrid），设置列标题
            bool xlsx = (fileFormat ?? 2007) == 2007;
            ExcelExporter ee = new ExcelExporter("检测机构管理", xlsx);
            ee.SetColumnTitles("序号, 机构名称, 检测证书编号, 计量证书编号, 法人, 联系电话,  状态");
            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            var compayTypes = sysDictServcie.GetDictsByKey("customStatus");
            for (int i = 0; i < data.Results.Count; i++)
            {
                var custom = data.Results[i];
                ExcelRow row = ee.AddRow();
                row.AddCell((pos + i + 1).ToString());
                row.AddCell("{0}".Fmt(custom.NAME));
                row.AddCell(custom.DETECTNUM);
                row.AddCell(custom.MEASNUM);
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
            if(rep.GetByCondition(r => r.ID == CustomNo).Count>0)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = "数据上传编码不能重复！";
                return Content(result.ToJson());
            }
            try
            {
                string errorMsg = string.Empty;
                if (!checkUnitService.NewCustom(CustomNo, CustomName, out errorMsg))
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

        [HttpPost]
        public ActionResult UpdateName(CheckQualifyUpdateNameModel model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            if (model.id.IsNullOrEmpty())
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = "出现错误";
                return Content(result.ToJson());
            }
            if (model.CustomName.IsNullOrEmpty())
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = "机构名称不能为空";
                return Content(result.ToJson());
            }

            try
            {
                rep.UpdateOnly(new t_bp_custom { NAME = model.CustomName }, t => t.ID == model.id, t => new { t.NAME });
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }


        public ActionResult BuildDTressData(string customId, bool IsEdit = true)
        {
            DTreeResponse response = new DTreeResponse()
            {
                code = 200,
                msg = "",
                status = new Status { code = 200, message = "sucessd" },
                data = new List<DTree>()
            };
            var equClassId = rep.GetByConditon<string>(t => t.ID == customId, t => t.EquclassId).FirstOrDefault();
            var allEquipmentTypeList = equipTypeList.GetByCondition(t => true);
            var rootNodes = allEquipmentTypeList.DistinctBy(t => t.typcode).ToList();

            foreach (var rootNode in rootNodes)
            {
                DTree dtree = new DTree()
                {
                    parentId = "-1",
                    id = rootNode.typcode,
                    title = rootNode.typename,
                    children = new List<DTree>()
                };
                var secNodes = allEquipmentTypeList.Where(t => t.itemcode.Contains(rootNode.typcode)).DistinctBy(t => t.itemcode).ToList();
                foreach (var secNode in secNodes)
                {
                    DTree secDTtee = new DTree()
                    {
                        parentId = rootNode.typcode,
                        id = secNode.itemcode,
                        title = secNode.itemname,
                        children = new List<DTree>(),
                        checkArr = new List<CheckArr>(),
                    };

                    var lastNodes = allEquipmentTypeList.Where(t => t.parmcode.Contains(secNode.itemcode)).ToList();
                    foreach (var lastNode in lastNodes)
                    {
                        DTree lastTree = new DTree()
                        {
                            parentId = secNode.itemcode,
                            id = lastNode.parmcode,
                            title = lastNode.parmname,
                            last = true,
                            checkArr = new List<CheckArr>()
                        };
                        CheckArr checkarr = new CheckArr()
                        {
                            type = "0",
                            @checked = "0"
                        };
                        if (!equClassId.IsNullOrEmpty())
                        {
                            if (equClassId.Contains(lastNode.parmcode))
                            {
                                checkarr.@checked = "1";
                            }
                        }
                        lastTree.checkArr.Add(checkarr);

                        if (IsEdit)
                        {
                            secDTtee.children.Add(lastTree);
                        }
                        else if (checkarr.@checked == "1")
                        {
                            secDTtee.children.Add(lastTree);
                        }

                    }
                    if (secDTtee.children.Count > 0)
                    {
                        dtree.children.Add(secDTtee);
                    }
                }
                if (dtree.children.Count > 0)
                {
                    response.data.Add(dtree);
                }
            }
            return Content(response.ToJson());
        }

    }

}
