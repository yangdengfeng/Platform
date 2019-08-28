using PkpmGX.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pkpm.Core.UserRoleCore;
using Pkpm.Framework.FileHandler;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.SysDictCore;
using Pkpm.Framework.Repsitory;
using Pkpm.Entity;
using Pkpm.Core.CheckPeopleManagerCore;
using Pkpm.Framework.Common;
using Pkpm.Entity.Models;
using ServiceStack;
using Pkpm.Entity.DTO;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Dhtmlx.Model.Grid;
using Dhtmlx.Model.TreeView;
using System.Xml.Linq;
using Dhtmlx.Model.Menu;
using Dhtmlx.Model.Toolbar;
using PkpmGX.Models;
using ServiceStack.OrmLite;
using Dhtmlx.Model.Form;
using System.ComponentModel;
using System.Reflection;

namespace PkpmGX.Controllers
{
    [Authorize]
    public class CheckPeopleManagerController : PkpmController
    {
        IFileHandler fileHander;
        ISysDictService sysDictServcie;
        ICheckUnitService checkUnitService;
        ICheckPeopleManagerService checkPeopleManagerService;
        IRepsitory<t_bp_People> rep;
        IRepsitory<t_bp_People_tmp> repTmp;
        IRepsitory<t_bp_custom> customRep;
        IRepsitory<t_bp_PeoChange> peoChangeRep;
        //IRepsitory<t_bp_PeopleN> peopleNRep;
        IRepsitory<t_bp_PeoEducation> eduRep;
        IRepsitory<t_bp_PeoAward> peoAwardRep;
        IRepsitory<t_bp_PeoPunish> punishRep;
        IRepsitory<t_bp_postType> postTypeRep;
        IRepsitory<t_sys_cellModelParam> cellRep;
        IRepsitory<User> userRep;
        string CryptPwd = string.Empty;//新增用户默认密码123
        public CheckPeopleManagerController(ISysDictService sysDictServcie,
            ICheckUnitService checkUnitService,
            ICheckPeopleManagerService checkPeopleManagerService,
            IRepsitory<t_bp_People> rep,
            IRepsitory<t_bp_People_tmp> repTmp,
            IRepsitory<t_bp_PeoChange> peoChangeRep,
            //IRepsitory<t_bp_PeopleN> peopleNRep,
            IRepsitory<t_bp_PeoEducation> eduRep,
            IRepsitory<t_bp_PeoAward> peoAwardRep,
            IRepsitory<t_bp_PeoPunish> punishRep,
             IRepsitory<User> userRep,
            IRepsitory<t_bp_postType> postTypeRep,
            IRepsitory<t_sys_cellModelParam> cellRep,
            IRepsitory<t_bp_custom> customRep,
            IFileHandler fileHander,
            IUserService userService) : base(userService)
        {
            this.sysDictServcie = sysDictServcie;
            this.checkUnitService = checkUnitService;
            this.checkPeopleManagerService = checkPeopleManagerService;
            this.rep = rep;
            this.repTmp = repTmp;
            this.peoChangeRep = peoChangeRep;
            //this.peopleNRep = peopleNRep;
            this.eduRep = eduRep;
            this.peoAwardRep = peoAwardRep;
            this.punishRep = punishRep;
            this.postTypeRep = postTypeRep;
            this.cellRep = cellRep;
            this.fileHander = fileHander;
            this.userRep = userRep;
            this.customRep = customRep;
            CryptPwd = HashUtility.MD5HashHexStringFromUTF8String("123") + "|";
        }

        // GET: CheckPeopleManager
        public ActionResult Index()
        {
            CheckPeopleViewModel viewModel = new CheckPeopleViewModel();
            viewModel.CompayTypes = sysDictServcie.GetDictsByKey("companyType");
            viewModel.CheckUnitNames = new Dictionary<string, string>();// checkUnitService.GetAllCheckUnit();
            viewModel.HasCerts = sysDictServcie.GetDictsByKey("haveNone");
            viewModel.WorkStatus = sysDictServcie.GetDictsByKey("workStatus");
            viewModel.PersonnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");
            viewModel.EngineersType = sysDictServcie.GetDictsByKey("engineersType");
            viewModel.PersonnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");
            viewModel.PersonnelStaff = sysDictServcie.GetDictsByKey("personnelStaff");
            //viewModel.CompayTypes = sysDictServcie.GetDictsByKey("CompanyType");
            int CurrentUserId = GetCurrentUserId();
            viewModel.IsAdmin = userService.IsAdmin(CurrentUserId);

            return View(viewModel);
        }


        public ActionResult PostType(int id, string postTypeCode, string displaytype)
        {
            CheckPeoplePostType viewModel = new CheckPeoplePostType();
            if (id > 0)
            {
                var people = rep.GetById(id);
                viewModel.PostType = people.PostType;
            }
            else
            {
                viewModel.PostType = postTypeCode.Replace("|", ";<br>"); ;
            }

            viewModel.postTypeCode = postTypeCode;

            viewModel.displaytype = displaytype;
            if (IsCurrentAdmin() == true)
            {
                viewModel.userGrade = "00";
            }
            return View(viewModel);
        }



        [HttpPost]
        public ActionResult getPostType()
        {
            var paths = postTypeRep.GetByCondition(r => r.Id != null).OrderBy(p => Convert.ToInt32(p.Id));
            return Content(paths.ToJson());
        }

        public ActionResult Menu()
        {
            DhtmlxMenu menu = new DhtmlxMenu();
            menu.AddItem(new DhtmlxMenuContainerItem("AllSearch", "所有人员信息") { Img = "fa fa-search", Imgdis = "fa fa-search" });
            menu.AddItem(new DhtmlxMenuSeparatorItem("s4"));
            //menu.AddItem(new DhtmlxMenuContainerItem("RegisterOnJob", "注册在职人员信息") { Img = "fa fa-search", Imgdis = "fa fa-search" });
            //menu.AddItem(new DhtmlxMenuSeparatorItem("s1"));
            //menu.AddItem(new DhtmlxMenuContainerItem("NotRegister", "未注册人员信息") { Img = "fa fa-search", Imgdis = "fa fa-search" });
            //menu.AddItem(new DhtmlxMenuSeparatorItem("s2"));
            //menu.AddItem(new DhtmlxMenuContainerItem("RegisterLeaveJob", "注册离职人员信息") { Img = "fa fa-search", Imgdis = "fa fa-search" });
            //menu.AddItem(new DhtmlxMenuSeparatorItem("s3"));
            //menu.AddItem(new DhtmlxMenuContainerItem("Logout", "注销人员信息") { Img = "fa fa-search", Imgdis = "fa fa-search" });
            string str = menu.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult ToolBar()
        {
            DhtmlxToolbar toolBar = new DhtmlxToolbar();
            var buttons = GetCurrentUserPathActions();
            if (HaveButtonFromAll(buttons, "Create"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Create", "新增人员") { Img = "fa fa-plus", Imgdis = "fa fa-plus" });
                toolBar.AddToolbarItem(new DhtmlxToolbarSeparatorItem("5"));
            }
            toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Export", "导出") { Img = "fa fa-file-excel-o", Imgdis = "fa fa-file-excel-o" });
            if (HaveButtonFromAll(buttons, "Delete"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("deleteById", "[删除]") { Img = "fa fa-times", Imgdis = "fa fa-times" });
            }
            if (HaveButtonFromAll(buttons, "Submit"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Send", "[递交]") { Img = "fa fa-paper-plane", Imgdis = "fa fa-paper-plane" });
            }
            if (HaveButtonFromAll(buttons, "AnnualTest"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("AnnualTest", "[年审]") { Img = "fa fa-life-ring", Imgdis = "fa fa-life-ring" });
            }
            if (HaveButtonFromAll(buttons, "ReturnStatus"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("ReturnState", "[返回状态]") { Img = "fa fa-undo", Imgdis = "fa fa-undo" });
            }
            if (HaveButtonFromAll(buttons, "Fire"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Fire", "[离职]") { Img = "fa fa-undo", Imgdis = "fa fa-undo" });
            }
            if (HaveButtonFromAll(buttons, "Screening"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("Screening", "[屏蔽]") { Img = "fa fa-unlock-alt", Imgdis = "fa fa-unlock-alt" });
            }
            if (HaveButtonFromAll(buttons, "RelieveScreening"))
            {
                toolBar.AddToolbarItem(new DhtmlxToolbarButtonItem("RelieveScreening", "[解除屏蔽]") { Img = "fa fa-unlock", Imgdis = "fa fa-unlock" });
            }

            string str = toolBar.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        /// <summary>
        /// 查询主方法
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public ActionResult Search(CheckPeopleSearchModel searchModel)
        {
            var peopleResult = GetSearchResult(searchModel);

            var instDicts = checkUnitService.GetAllCheckUnit();
            var workStatus = sysDictServcie.GetDictsByKey("workStatus");
            var personnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");
            var personnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");

            DhtmlxGrid grid = new DhtmlxGrid();
            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            string customname = string.Empty;
            grid.AddPaging(peopleResult.TotalCount, pos);
            var buttons = GetCurrentUserPathActions();
            for (int i = 0; i < peopleResult.Results.Count; i++)
            {
                var people = peopleResult.Results[i];
                customname = checkUnitService.GetCheckUnitByIdFromAll(instDicts, people.Customid);

                //Dictionary<string, string> dict = new Dictionary<string, string>();
                //if (HaveButtonFromAll(buttons, "PrintCert"))
                //{
                //    dict.Add("[打证]", "playCard(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\")".Fmt(people.Name, people.id, GetUIDtString(people.postdate.HasValue ? people.postdate.Value : DateTime.Now), people.Customid, customname));
                //}
                //if (HaveButtonFromAll(buttons, "UnitChange"))
                //{
                //    dict.Add("[单位变更]", "unitChange(\"{0}\",\"{1}\")".Fmt(people.Name, people.id.ToString()));
                //}
                //if (HaveButtonFromAll(buttons, "UnitSubChange"))
                //{
                //    dict.Add("[变动]", "change(\"{0}\",\"{1}\",\"{2}\")".Fmt(people.Name, people.id.ToString(), people.Customid));
                //}
                //已递交	
                //if (HaveButtonFromAll(buttons, "ApplyChange") && checkPeopleManagerService.CanApplyChangePeople(people.Approvalstatus))
                //{
                //    dict.Add("[申请修改]", "applyChange({0},\"{1}\",\"{2}\")".Fmt(people.id, people.Name, people.Customid));
                //}

                DhtmlxGridRow row = new DhtmlxGridRow(people.id.ToString());
                row.AddCell("");
                row.AddCell((pos + i + 1).ToString());
                row.AddCell(people.Name);
                row.AddCell(customname);
                row.AddCell(people.SelfNum);
                row.AddCell(people.PostNum);
                row.AddCell(people.zw);
                row.AddCell(SysDictUtility.GetKeyFromDic(personnelTitles, people.Title));
                row.AddCell(SysDictUtility.GetKeyFromDic(workStatus, people.iscb));
                row.AddCell(SysDictUtility.GetKeyFromDic(personnelStatus, people.Approvalstatus));
                //row.AddLinkJsCells(dict);

                row.AddCell(new DhtmlxGridCell("查看", false).AddCellAttribute("title", "查看"));
                if (HaveButtonFromAll(buttons, "Edit") && people.Approvalstatus == "0")
                {
                    row.AddCell(new DhtmlxGridCell("编辑", false).AddCellAttribute("title", "编辑"));
                }
                else
                {
                    row.AddCell(string.Empty);
                }

                //对人员进行修改后，进行审核操作
                if (HaveButtonFromAll(buttons, "Audit") && people.Approvalstatus == "1")
                {
                    row.AddLinkJsCell("审核", "AuditPeople(\"{0}\")".Fmt(people.id));
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

        private SearchResult<CheckPeopleUIModel> GetSearchResult(CheckPeopleSearchModel searchModel)
        {
            int dbCount = 0;

            var peoples = new List<CheckPeopleUIModel>();
            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            int count = searchModel.count.HasValue ? searchModel.count.Value : 30;
            var predicate = PredicateBuilder.True<t_bp_People>();
            var instFilter = GetCurrentInstFilter();

            #region 动态查询
            //过滤掉删除的
            predicate = predicate.And(tp => tp.data_status == null || tp.data_status != "-1" && tp.IsUse != 2);

            if (!string.IsNullOrWhiteSpace(searchModel.postTypeCode))
            {

                var siArray = searchModel.postTypeCode.Split('|');
                for (var i = 0; i < siArray.Length; i++)
                {
                    string array = siArray[i];
                    predicate = predicate.Or(t => t.postTypeCode.Contains(array));
                }
            }
            if (!string.IsNullOrWhiteSpace(searchModel.CheckUnitName))
            {

                predicate = predicate.And(t => t.Customid == searchModel.CheckUnitName);
            }
            if (!string.IsNullOrWhiteSpace(searchModel.Name))
            {

                predicate = predicate.And(t => t.Name != null && t.Name.Contains(searchModel.Name));
            }

            if (!string.IsNullOrWhiteSpace(searchModel.IDNum))
            {

                predicate = predicate.And(t => t.SelfNum != null && t.SelfNum.Contains(searchModel.IDNum));
            }

            if (!string.IsNullOrWhiteSpace(searchModel.HasCert))
            {

                if (searchModel.HasCert == "1")
                { predicate = predicate.And(t => t.PostNum != null); }
                else if (searchModel.HasCert == "0")
                { predicate = predicate.And(t => t.PostNum == null || t.PostNum == ""); }
            }

            if (!string.IsNullOrWhiteSpace(searchModel.PostCertNum))
            {

                predicate = predicate.And(t => t.PostNum != null && t.PostNum.Contains(searchModel.PostCertNum));
            }


            if (instFilter.NeedFilter && instFilter.FilterInstIds.Count() > 0)
            {

                predicate = predicate.And(t => instFilter.FilterInstIds.Contains(t.Customid));
            }

            if (IsCurrentCheckPeople())
            {
                var currentUserId = GetCurrentUserId();
                var userName = userRep.GetColumnByCondition<string>(t => t.Id == currentUserId, t => new { t.UserName }).FirstOrDefault();
                if (!userName.IsNullOrEmpty())
                {
                    predicate = predicate.And(t => t.SelfNum == userName);
                }

            }

            //TODO:单位性质
            //if (!string.IsNullOrWhiteSpace(searchModel.UnitCategory))
            //{
            //    hasPredicate = true;
            //    predicate = predicate.And(t => t.ishaspostnum == searchModel.UnitCategory);
            //}

            if (!string.IsNullOrWhiteSpace(searchModel.PeopleStatus) && searchModel.SearchRule != "RegisterOnJob" && searchModel.SearchRule != "RegisterLeaveJob" && searchModel.SearchRule != "Logout")
            {

                predicate = predicate.And(t => t.iscb == searchModel.PeopleStatus);
            }
            if (searchModel.AgeStartDt.HasValue)
            {

                predicate = predicate.And(t => t.Birthday.HasValue && t.Birthday.Value >= searchModel.AgeStartDt.Value);
            }

            if (searchModel.AgeEndDt.HasValue)
            {

                predicate = predicate.And(t => t.Birthday.HasValue && t.Birthday.Value <= searchModel.AgeEndDt.Value);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.TechTitle))
            {

                if (searchModel.TechTitle == "6")
                { predicate = predicate.And(t => t.Title == "4" || t.Title == "2" || t.Title == "3"); }
                else
                { predicate = predicate.And(t => t.Title == searchModel.TechTitle); }
            }

            if (!string.IsNullOrWhiteSpace(searchModel.IsTech))
            {

                predicate = predicate.And(t => t.iszcgccs == searchModel.IsTech);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Status))
            {

                predicate = predicate.And(t => t.Approvalstatus == searchModel.Status);
            }
            //TODO:公式序号
            if (!string.IsNullOrWhiteSpace(searchModel.PubSeqNo))
            {

                predicate = predicate.And(t => t.PostType != null && t.PostType.Contains(searchModel.PubSeqNo));
            }

            if (!string.IsNullOrWhiteSpace(searchModel.Position))
            {

                predicate = predicate.And(t => t.zw != null && t.zw.Contains(searchModel.Position));
            }

            //输入 20 - 30   则相应日期小于2018-20 大于 2018-30 
            if (searchModel.AgeStart.HasValue)
            {
                var ageStarDateTime = DateTime.Now.AddYears(0 - searchModel.AgeStart.Value);
                predicate = predicate.And(t => t.Birthday.HasValue && t.Birthday.Value <= ageStarDateTime);
            }

            if (searchModel.AgeEnd.HasValue)
            {
                var ageEndDateTime = DateTime.Now.AddYears(0 - searchModel.AgeEnd.Value);
                predicate = predicate.And(t => t.Birthday.HasValue && t.Birthday.Value >= ageEndDateTime);
            }

            if (!string.IsNullOrEmpty(searchModel.CompanyType))
            {
                var customs = customRep.GetByConditon<string>(t => t.companytype == searchModel.CompanyType, r => r.ID);
                predicate = predicate.And(t => customs.Contains(t.Customid));
            }
            #endregion

            if (searchModel.SearchRule == "RegisterOnJob")
            {
                peoples = checkPeopleManagerService.GetRegisterOnJobPeople(predicate, instFilter.FilterInstIds, pos, count, out dbCount);
            }
            else if (searchModel.SearchRule == "NotRegister")
            {
                peoples = checkPeopleManagerService.GetNotRegisterPeople(predicate, instFilter.FilterInstIds, pos, count, out dbCount);
            }
            else if (searchModel.SearchRule == "RegisterLeaveJob")
            {
                peoples = checkPeopleManagerService.GetRegisterLeaveJobPeople(predicate, instFilter.FilterInstIds, pos, count, out dbCount);
            }
            else if (searchModel.SearchRule == "Logout")
            {
                peoples = checkPeopleManagerService.GetLogoutPeople(predicate, instFilter.FilterInstIds, pos, count, out dbCount);
            }
            else
            {
                PagingOptions<t_bp_People> pagingOption = new PagingOptions<t_bp_People>(pos, count, s => new { s.id });


                peoples = rep.GetByConditonPage<CheckPeopleUIModel>(predicate, p => new
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
                },
                    pagingOption);

                dbCount = pagingOption.TotalItems;
            }

            return new SearchResult<CheckPeopleUIModel>(dbCount, peoples);
        }

        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="searchModel">查询参数</param>
        /// <param name="fileFormat">文件格式,2003/2007</param>
        /// <returns>用于下载的Excel文件内容</returns>
        public ActionResult Export(CheckPeopleSearchModel searchModel, int? fileFormat)
        {
            // 改动1：（向上看）复制Search为新动作Export，添加参数fileFormat

            var peopleResult = GetSearchResult(searchModel);

            var instDicts = checkUnitService.GetAllCheckUnit();
            var workStatus = sysDictServcie.GetDictsByKey("workStatus");
            var personnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");
            var personnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");

            // 改动2：创建导出类实例（而非 DhtmlxGrid），设置列标题
            bool xlsx = (fileFormat ?? 2007) == 2007;
            ExcelExporter ee = new ExcelExporter("检测人员管理", xlsx);
            ee.SetColumnTitles("序号, 姓名, 所属机构名称, 身份证号, 岗位证书编号, 职务, 职称, 在职状况, 状态");
            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            for (int i = 0; i < peopleResult.Results.Count; i++)
            {
                var people = peopleResult.Results[i];

                // 改动3：添加 ExcelRow 对象（而非 dhtmlxGridRow）
                ExcelRow row = ee.AddRow();

                row.AddCell(pos + i + 1);
                row.AddCell(people.Name);
                row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(instDicts, people.Customid));
                row.AddCell(people.SelfNum);
                row.AddCell(people.PostNum);
                row.AddCell(people.zw);
                row.AddCell(SysDictUtility.GetKeyFromDic(personnelTitles, people.Title));
                row.AddCell(SysDictUtility.GetKeyFromDic(workStatus, people.iscb));
                row.AddCell(SysDictUtility.GetKeyFromDic(personnelStatus, people.Approvalstatus));
            }

            // 改动4：返回字节流
            return File(ee.GetAsBytes(), ee.MIME, ee.FileName);
        }
        // GET: CheckPeopleManager/Details/5
        public ActionResult Details(int id)
        {
            CheckPeopleViewModel viewModel = new CheckPeopleViewModel();
            viewModel.people = rep.GetById(id);
            viewModel.people.ishaspostnum = sysDictServcie.GetDictsByKey("yesNo", viewModel.people.ishaspostnum); 
            viewModel.people.isreghere = sysDictServcie.GetDictsByKey("yesNo", viewModel.people.isreghere);
            viewModel.people.Title = sysDictServcie.GetDictsByKey("personnelTitles", viewModel.people.Title);
            viewModel.people.iscb = sysDictServcie.GetDictsByKey("workStatus", viewModel.people.iscb);
            viewModel.people.Approvalstatus = sysDictServcie.GetDictsByKey("personnelStatus", viewModel.people.Approvalstatus);
            viewModel.CustomName = checkUnitService.GetCheckUnitById(viewModel.people.Customid);
            viewModel.EngineersType = sysDictServcie.GetDictsByKey("EngineersType");

            string iszcgcc = "";
            if (!string.IsNullOrEmpty(viewModel.people.iszcgccs))
            {
                var siArrays = viewModel.people.iszcgccs.Split('、');
                iszcgcc = sysDictServcie.GetDictsByKey("EngineersType", siArrays[0]);
                for (int i = 1; i < siArrays.Length; i++)
                {
                    iszcgcc = iszcgcc + "," + sysDictServcie.GetDictsByKey("EngineersType", siArrays[i]);
                }
            }
            viewModel.iszcgccs = iszcgcc;

            string PostTypes = "";
            if (!string.IsNullOrEmpty(viewModel.people.PostType))
            {
                var siArrays = viewModel.people.PostType.Split('>');

                for (int i = 0; i < siArrays.Length; i++)
                {
                    siArrays[i] = siArrays[i].Replace("[X]", "(公示序号：");
                    siArrays[i] = siArrays[i].Replace("[T]", "(");
                    siArrays[i] = siArrays[i].Replace("[/X]", ")");
                    siArrays[i] = siArrays[i].Replace("[/T]", ")");
                    siArrays[i] = siArrays[i].Replace(";<br", ";\\n");
                }
                for (int i = 0; i < siArrays.Length; i++)
                {
                    PostTypes = PostTypes + siArrays[i];
                }
            }
            viewModel.postTypeView = PostTypes;

            viewModel.CheckPeoplePunish = CheckPeoplePunish(viewModel.people.id);
            viewModel.CheckPeopleAwards = CheckPeopleAwards(viewModel.people.id);
            viewModel.CheckPeopleChange = CheckPeopleChange(viewModel.people.id);
            //viewModel.CheckPeopleVerification = CheckPeopleVerification(viewModel.people.id);
            viewModel.CheckPeopleEducation = CheckPeopleEducation(viewModel.people.id);
            viewModel.CheckPeopleFile = CheckPeopleFile(viewModel.people.id,
                viewModel.people.PhotoPath,
                viewModel.people.selfnumPath,
                viewModel.people.educationpath,
                viewModel.people.zcgccszhpath,
                viewModel.people.PostPath,
                viewModel.people.titlepath);

            return View(viewModel);
        }

        // GET: CheckPeopleManager/Create
        public ActionResult Create()
        {
            CheckPeopleViewModel viewModel = new CheckPeopleViewModel();
            viewModel.PersonnelStaff = sysDictServcie.GetDictsByKey("PersonnelStaff");
            // viewModel.CheckUnitNames = checkUnitService.GetAllCheckUnit();
            viewModel.YesNo = sysDictServcie.GetDictsByKey("yesNo");
            viewModel.PersonnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");
            viewModel.WorkStatus = sysDictServcie.GetDictsByKey("workStatus");
            viewModel.PersonnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");
            viewModel.EngineersType = sysDictServcie.GetDictsByKey("EngineersType");
            viewModel.Sex = sysDictServcie.GetDictsByKey("Sex");
            viewModel.CheckPeopleFile = CheckPeopleFile(-1, null, null, null, null, null, null);
            viewModel.CheckUnitNames = GetCurrentInsts();
            return View(viewModel);
        }

        // POST: CheckPeopleManager/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CheckPeopleSaveViewModel viewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            CheckPeopleSaveModel creatModel = new CheckPeopleSaveModel();
            try
            {
                //判断身份证号是否重复
                if (!string.IsNullOrEmpty(viewModel.SelfNum))
                {
                    var ExistsPeople = rep.GetByCondition(r => r.SelfNum == viewModel.SelfNum).FirstOrDefault();
                    //var status =rep.GetCountByCondtion(r => r.data_status != "-1"); 
                    if (ExistsPeople != null)
                    {
                        if (ExistsPeople.data_status == "-1")
                        {
                            //当人员表中存在数据时，再次新增直接删除，同时删除用户表中的数据
                            rep.DeleteById(ExistsPeople.id);
                            userRep.DeleteByCondition(t => t.UserName == viewModel.SelfNum);
                        }
                        else
                        {
                            result = ControllerResult.FailResult;
                            result.ErroMsg = "新增失败，该身份证号已经存在！";
                            return Content(result.ToJson());
                        }
                    }

                }
                var deletePeopleCount = rep.GetCountByCondtion(r => r.SelfNum == viewModel.SelfNum && r.data_status == "-1");
                if (deletePeopleCount > 0)
                {
                    //rep.DeleteById<t_bp_People>(r => r.SelfNum == viewModel.SelfNum ,rep= r.data_status == "-1")
                }

                if (!checkUnitService.IsCustomIdExist(viewModel.Customid))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = "新增失败，该机构系统中不存在！";
                    return Content(result.ToJson());
                }


                string iszcgcc = "";
                if (!string.IsNullOrEmpty(viewModel.iszcgccs))
                {
                    var siArrays = viewModel.iszcgccs.Split(',');
                    iszcgcc = sysDictServcie.GetDictsByName("EngineersType", siArrays[0]);
                    for (int i = 1; i < siArrays.Length; i++)
                    {
                        iszcgcc = iszcgcc + "、" + sysDictServcie.GetDictsByName("EngineersType", siArrays[i]);
                    }
                }

                if (!viewModel.postnumdate.IsNullOrEmpty())
                {
                    var postnumdates = viewModel.postnumdate.Split(' ');
                    var index = 0;
                    foreach (var postnumdate in postnumdates)
                    {
                        switch (index)
                        {
                            case 0:
                                viewModel.postnumstartdate = DateTime.Parse(postnumdate);
                                break;
                            case 2:
                                viewModel.postnumenddate = DateTime.Parse(postnumdate);
                                break;
                        }
                        index++;
                    }
                }
                if (!viewModel.zcgccszhdate.IsNullOrEmpty())
                {
                    var zcgccszhdates = viewModel.zcgccszhdate.Split(' ');
                    var index = 0;
                    foreach (var zcgccszhdate in zcgccszhdates)
                    {
                        switch (index)
                        {
                            case 0:
                                viewModel.zcgccszhstartdate = DateTime.Parse(zcgccszhdate);
                                break;
                            case 2:
                                viewModel.zcgccszhenddate = DateTime.Parse(zcgccszhdate);
                                break;
                        }
                        index++;
                    }
                }

                if (!string.IsNullOrEmpty(viewModel.PostType))
                {
                    viewModel.PostType = viewModel.PostType.Replace(";&lt;br&gt;", ";<br>");
                }
                creatModel.people = new t_bp_People()
                {
                    Customid = viewModel.Customid,
                    Name = viewModel.Name,
                    SelfNum = viewModel.SelfNum,
                    ishaspostnum = viewModel.ishaspostnum,
                    PostDate = viewModel.PostDate,
                    Education = viewModel.Education,
                    Professional = viewModel.Professional,
                    zw = viewModel.zw,
                    iszcgccs = iszcgcc,
                    isreghere = viewModel.isreghere,
                    zcgccszh = viewModel.zcgccszh,
                    Sex = viewModel.Sex,//string.IsNullOrEmpty(viewModel.Sex) ? "" : sysDictServcie.GetDictsByKey("Sex", viewModel.Sex),
                    Birthday = viewModel.Birthday,
                    PostNum = viewModel.PostNum,
                    postnumstartdate = viewModel.postnumstartdate,
                    postnumenddate = viewModel.postnumenddate,
                    School = viewModel.School,
                    Title = viewModel.Title,
                    Tel = viewModel.Tel,
                    Email = viewModel.Email,
                    iscb = viewModel.iscb,
                    SBNum = viewModel.SBNum,
                    zcgccszhstartdate = viewModel.zcgccszhstartdate,
                    zcgccszhenddate = viewModel.zcgccszhenddate,
                    PostType = viewModel.PostType,
                    postTypeCode = viewModel.postTypeCode,
                    postDelayReg = viewModel.postDelayReg,
                    approveadvice = viewModel.approveadvice,
                    Approvalstatus = "0", //未递交
                    PhotoPath = viewModel.PhotoPath,
                    selfnumPath = viewModel.selfnumPath,
                    educationpath = viewModel.educationpath,
                    zcgccszhpath = viewModel.zcgccszhpath,
                    PostPath = viewModel.PostPath,
                    titlepath = viewModel.titlepath,
                    data_status = "1",
                    update_time = DateTime.Now,
                    IsUse = 1,
                    gznx = viewModel.gznx

                };


                creatModel.PeoChanges = new List<t_bp_PeoChange>();
                if (viewModel.peoChange != null)
                {
                    foreach (var item in viewModel.peoChange)
                    {
                        t_bp_PeoChange peochang = new t_bp_PeoChange();
                        peochang.ChaContent = item.ChaContent;
                        peochang.ChaDate = item.ChaDate;
                        creatModel.PeoChanges.Add(peochang);
                    }
                }

                //creatModel.PeopleNs = new List<t_bp_PeopleN>();
                //if (viewModel.peoVerification != null)
                //{
                //    foreach (var item in viewModel.peoVerification)
                //    {
                //        t_bp_PeopleN PeopleN = new t_bp_PeopleN();
                //        PeopleN.addtime = item.addtime;
                //        PeopleN.Pcontext = item.Pcontext;
                //        creatModel.PeopleNs.Add(PeopleN);
                //    }
                //}

                creatModel.PeoEducations = new List<t_bp_PeoEducation>();

                if (viewModel.peoEducation != null)
                {
                    foreach (var item in viewModel.peoEducation)
                    {
                        t_bp_PeoEducation peoEducation = new t_bp_PeoEducation()
                        {
                            TrainContent = item.TrainContent,
                            TrainDate = item.TrainDate,
                            TrainUnit = item.TrainUnit,
                            TestDate = item.TestDate
                        };
                        creatModel.PeoEducations.Add(peoEducation);

                    }
                }


                creatModel.PeoAwards = new List<t_bp_PeoAward>();

                if (viewModel.peoAwards != null)
                {
                    foreach (var item in viewModel.peoAwards)
                    {
                        t_bp_PeoAward peoAward = new t_bp_PeoAward()
                        {
                            AwaContent = item.AwaContent,
                            AwaDate = item.AwaDate,
                            AwaUnit = item.AwaUnit
                        };
                        creatModel.PeoAwards.Add(peoAward);

                    }

                }

                creatModel.PeoPunishs = new List<t_bp_PeoPunish>();
                if (viewModel.peoPunish != null)
                {
                    foreach (var item in viewModel.peoPunish)
                    {
                        t_bp_PeoPunish peoPunish = new t_bp_PeoPunish()
                        {
                            PunContent = item.PunContent,
                            PunDate = item.PunDate,
                            PunName = item.PunName,
                            PunUnit = item.PunUnit
                        };
                        creatModel.PeoPunishs.Add(peoPunish);
                    }
                }

                creatModel.user = new User()
                {
                    CustomId = viewModel.Customid,
                    UserDisplayName = viewModel.Name,
                    UserName = viewModel.SelfNum,
                    PasswordHash = CryptPwd + viewModel.SelfNum + "|",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    // Id = viewModel.UserId,
                    Sex = viewModel.Sex,
                    PhoneNumber = viewModel.Tel,
                    Email = viewModel.Email,
                    Status = "00",
                    Grade = "02",
                    CheckStatus = "1"
                };
                string erroMsg = string.Empty;
                if (!checkPeopleManagerService.CreatPeople(creatModel, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("进行了新增人员的操作");
                }


            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        // GET: CheckPeopleManager/Edit/5
        public ActionResult Edit(int id)
        {
            CheckPeopleViewModel viewModel = new CheckPeopleViewModel();
            viewModel.people = rep.GetById(id);
            string PostTypes = "";
            if (!string.IsNullOrEmpty(viewModel.people.PostType))
            {
                var siArrays = viewModel.people.PostType.Split('>');

                for (int i = 0; i < siArrays.Length; i++)
                {
                    siArrays[i] = siArrays[i].Replace("[X]", "(公示序号：");
                    siArrays[i] = siArrays[i].Replace("[T]", "(");
                    siArrays[i] = siArrays[i].Replace("[/X]", ")");
                    siArrays[i] = siArrays[i].Replace("[/T]", ")");
                    siArrays[i] = siArrays[i].Replace(";<br", ";\\n");
                }
                for (int i = 0; i < siArrays.Length; i++)
                {
                    PostTypes = PostTypes + siArrays[i];
                }
            }
            viewModel.postTypeView = PostTypes;
            viewModel.CheckUnitNames = GetCurrentInsts();
            viewModel.YesNo = sysDictServcie.GetDictsByKey("yesNo");
            viewModel.PersonnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");
            viewModel.EngineersType = sysDictServcie.GetDictsByKey("EngineersType");
            viewModel.WorkStatus = sysDictServcie.GetDictsByKey("workStatus");
            viewModel.PersonnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");
            viewModel.Sex = sysDictServcie.GetDictsByKey("Sex");
            viewModel.AllUnitIdAndName = checkUnitService.GetAllCheckUnit();
            viewModel.PersonnelStaff = sysDictServcie.GetDictsByKey("PersonnelStaff");
            viewModel.CheckPeoplePunish = CheckPeoplePunish(viewModel.people.id);
            viewModel.CheckPeopleAwards = CheckPeopleAwards(viewModel.people.id);
            viewModel.CheckPeopleChange = CheckPeopleChange(viewModel.people.id);
            viewModel.CheckPeopleEducation = CheckPeopleEducation(viewModel.people.id);
            viewModel.CheckPeopleFile = CheckPeopleFile(viewModel.people.id,
                viewModel.people.PhotoPath,
                viewModel.people.selfnumPath,   
                viewModel.people.educationpath,
                viewModel.people.zcgccszhpath,
                viewModel.people.PostPath,
                viewModel.people.titlepath);

            return View(viewModel);
        }


        public ActionResult EditPeople(int id)
        {
            CheckPeopleViewModel viewModel = new CheckPeopleViewModel();

            viewModel.people = rep.GetById(id);

            string PostTypes = "";
            if (!string.IsNullOrEmpty(viewModel.people.PostType))
            {
                var siArrays = viewModel.people.PostType.Split('>');

                for (int i = 0; i < siArrays.Length; i++)
                {
                    siArrays[i] = siArrays[i].Replace("[X]", "(公示序号：");
                    siArrays[i] = siArrays[i].Replace("[T]", "(");
                    siArrays[i] = siArrays[i].Replace("[/X]", ")");
                    siArrays[i] = siArrays[i].Replace("[/T]", ")");
                    siArrays[i] = siArrays[i].Replace(";<br", ";\\n");
                }
                for (int i = 0; i < siArrays.Length; i++)
                {
                    PostTypes = PostTypes + siArrays[i];
                }
            }
            viewModel.postTypeView = PostTypes;
            viewModel.CheckUnitNames = GetCurrentInsts();
            viewModel.YesNo = sysDictServcie.GetDictsByKey("yesNo");
            viewModel.PersonnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");
            viewModel.EngineersType = sysDictServcie.GetDictsByKey("EngineersType");
            viewModel.WorkStatus = sysDictServcie.GetDictsByKey("workStatus");
            viewModel.PersonnelStatus = sysDictServcie.GetDictsByKey("personnelStatus");
            viewModel.Sex = sysDictServcie.GetDictsByKey("Sex");
            viewModel.AllUnitIdAndName = checkUnitService.GetAllCheckUnit();
            viewModel.PersonnelStaff = sysDictServcie.GetDictsByKey("PersonnelStaff");
            viewModel.CheckPeoplePunish = CheckPeoplePunish(viewModel.people.id);
            viewModel.CheckPeopleAwards = CheckPeopleAwards(viewModel.people.id);
            viewModel.CheckPeopleChange = CheckPeopleChange(viewModel.people.id);
            //viewModel.CheckPeopleVerification = CheckPeopleVerification(viewModel.people.id);
            viewModel.CheckPeopleEducation = CheckPeopleEducation(viewModel.people.id);
            viewModel.CheckPeopleFile = CheckPeopleFile(viewModel.people.id,
                viewModel.people.PhotoPath,
                viewModel.people.selfnumPath,
                viewModel.people.educationpath,
                viewModel.people.zcgccszhpath,
                viewModel.people.PostPath,
                viewModel.people.titlepath);

            return View(viewModel);

        }

        // POST: CheckPeopleManager/Edit/5
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CheckPeopleSaveViewModel viewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            //判断身份证号是否重复
            if (!string.IsNullOrEmpty(viewModel.SelfNum) && viewModel.id > 0)
            {
                var ExistsPeople = rep.GetByCondition(r => r.SelfNum == viewModel.SelfNum && r.id != viewModel.id);
                if (ExistsPeople.Count > 0)
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = "修改失败，该身份证号已经存在！";
                    return Content(result.ToJson());
                }
            }
            string iszcgcc = string.Empty;
            if (!viewModel.iszcgccs.IsNullOrEmpty())
            {
                iszcgcc = viewModel.iszcgccs.Replace(",", "、");
            }
            //if (!string.IsNullOrEmpty(viewModel.iszcgccs))
            //{
            //    var siArrays = viewModel.iszcgccs.Split(',');
            //    iszcgcc = sysDictServcie.GetDictsByName("EngineersType", siArrays[0]);
            //    for (int i = 1; i < siArrays.Length; i++)
            //    {
            //        iszcgcc = iszcgcc + "、" + sysDictServcie.GetDictsByName("EngineersType", siArrays[i]);
            //    }
            //}
            if (!string.IsNullOrEmpty(viewModel.zw))
            {
                viewModel.zw = viewModel.zw.Replace(",", "、");
            }
            if (!string.IsNullOrEmpty(viewModel.PostType))
            {
                viewModel.PostType = viewModel.PostType.Replace(";&lt;br&gt;", ";<br>");
            }

            CheckPeopleTmpSaveModel editModel = new CheckPeopleTmpSaveModel();
            try
            {
                editModel.OpUserId = GetCurrentUserId();
                //editModel.CustomName = checkUnitService.GetCheckUnitByName(viewModel.Customid);
                editModel.people = new t_bp_People_tmp()
                {
                    id = viewModel.id,
                    Customid = viewModel.Customid,
                    Name = viewModel.Name,
                    SelfNum = viewModel.SelfNum,
                    ishaspostnum = viewModel.ishaspostnum,
                    PostDate = viewModel.PostDate,
                    Education = viewModel.Education,
                    Professional = viewModel.Professional,
                    zw = viewModel.zw,
                    iszcgccs = iszcgcc,
                    isreghere = viewModel.isreghere,
                    zcgccszh = viewModel.zcgccszh,
                    Sex = viewModel.Sex,
                    Birthday = viewModel.Birthday,
                    PostNum = viewModel.PostNum,
                    postnumstartdate = viewModel.postnumstartdate,
                    postnumenddate = viewModel.postnumenddate,
                    School = viewModel.School,
                    Title = viewModel.Title,
                    Tel = viewModel.Tel,
                    Email = viewModel.Email,
                    iscb = viewModel.iscb,
                    SBNum = viewModel.SBNum,
                    zcgccszhstartdate = viewModel.zcgccszhstartdate,
                    zcgccszhenddate = viewModel.zcgccszhenddate,
                    PostType = viewModel.PostType,
                    postTypeCode = viewModel.postTypeCode,
                    postDelayReg = viewModel.postDelayReg,
                    Approvalstatus = viewModel.Approvalstatus,
                    PhotoPath = viewModel.PhotoPath,
                    selfnumPath = viewModel.selfnumPath,
                    educationpath = viewModel.educationpath,
                    zcgccszhpath = viewModel.zcgccszhpath,
                    PostPath = viewModel.PostPath,
                    titlepath = viewModel.titlepath,
                    update_time = DateTime.Now,
                    gznx = viewModel.gznx,
                    data_status = "2"
                };

                DateTime? postnumstartdate = null, postnumenddate = null;
                DateTime? zcgccszhstartdate = null, zcgccszhenddate = null;
                CommonUtils.GetLayuiDateRange(viewModel.postnumdate, out postnumstartdate, out postnumenddate);
                CommonUtils.GetLayuiDateRange(viewModel.zcgccszhdate, out zcgccszhstartdate, out zcgccszhenddate);

                editModel.people.postnumstartdate = postnumstartdate;
                editModel.people.postnumenddate = postnumenddate;
                editModel.people.zcgccszhstartdate = zcgccszhstartdate;
                editModel.people.zcgccszhenddate = zcgccszhenddate;

                #region 关联表

                editModel.PeoChanges = new List<t_bp_PeoChange>();
                if (viewModel.peoChange != null)
                {
                    foreach (var peoChange in viewModel.peoChange)
                    {
                        peoChange.PeopleId = viewModel.id;
                        editModel.PeoChanges.Add(peoChange);
                    }
                }

                //editModel.PeopleNs = new List<t_bp_PeopleN>();
                //if (viewModel.peoVerification != null)
                //{
                //    foreach (var peoVerification in viewModel.peoVerification)
                //    {
                //        peoVerification.PeopleID = viewModel.id.ToString();
                //        editModel.PeopleNs.Add(peoVerification);
                //    }
                //}

                editModel.PeoEducations = new List<t_bp_PeoEducation>();
                if (viewModel.peoEducation != null)
                {
                    foreach (var peoEducation in viewModel.peoEducation)
                    {
                        peoEducation.PeopleId = viewModel.id;
                        editModel.PeoEducations.Add(peoEducation);
                    }
                }

                editModel.PeoAwards = new List<t_bp_PeoAward>();
                if (viewModel.peoAwards != null)
                {
                    foreach (var peoAwards in viewModel.peoAwards)
                    {
                        peoAwards.PeopleId = viewModel.id;
                        editModel.PeoAwards.Add(peoAwards);
                    }
                }

                editModel.PeoPunishs = new List<t_bp_PeoPunish>();
                if (viewModel.peoPunish != null)
                {
                    foreach (var peoPunish in viewModel.peoPunish)
                    {
                        peoPunish.PeopleId = viewModel.id;
                        editModel.PeoPunishs.Add(peoPunish);
                    }
                }

                #endregion

                string erroMsg = string.Empty;
                if (!checkPeopleManagerService.EditPeople(editModel, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对id为{0}的人员信息进行了修改操作".Fmt(viewModel.id));
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
        /// 比较两实体的差异
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<ColumnsDiffModel> CompareEntityDiff(string id)
        {
            List<ColumnsDiffModel> cdmList = new List<ColumnsDiffModel>();

            try
            {
                CheckPeopleCompareModel people = GetCheckPeopleCompareModel(rep.GetById(id), null);
                CheckPeopleCompareModel peopleTmp = GetCheckPeopleCompareModel(null, repTmp.GetById(id));
                if (peopleTmp == null) return cdmList;

                ColumnsDiffModel cdm = null;
                string get_old = string.Empty;
                string get_new = string.Empty;
                PropertyInfo[] mPi = typeof(CheckPeopleCompareModel).GetProperties();
                foreach (PropertyInfo pi in mPi)
                {
                    string displayName = pi.GetCustomAttribute<DisplayNameAttribute>().DisplayName; 
                    if (IsNumAndEnCh(displayName)) continue;

                    if (pi.Name.ToUpper().EndsWith("DATE") || pi.Name == "Birthday")
                    {
                        get_old = pi.GetValue(people, null) == null ? "" : Convert.ToDateTime(pi.GetValue(people, null)).ToString("yyyy-MM-dd");
                        get_new = pi.GetValue(peopleTmp, null) == null ? "" : Convert.ToDateTime(pi.GetValue(peopleTmp, null)).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        get_old = pi.GetValue(people, null) == null ? "" : pi.GetValue(people, null).ToString().Trim();
                        get_new = pi.GetValue(peopleTmp, null) == null ? "" : pi.GetValue(peopleTmp, null).ToString().Trim();
                        if (pi.Name == "zw") //职务的排序变了，重新对比下
                        {
                            var zws_old = get_old.Split('、');
                            var zws_new = get_new.Split('、');
                            if(zws_old.Count() == zws_new.Count())
                            {
                                var count = 0;
                                foreach(var zwo in zws_old)
                                {
                                    foreach(var zwn in zws_new)
                                    {
                                        if(zwo == zwn)
                                        {
                                            count++;
                                        }
                                    }
                                }
                                if(count == zws_old.Count())
                                {
                                    continue;
                                }
                            }
                        }
                        else if(pi.Name == "PostType") //需要特殊处理下，否则前端Post请求失败
                        {
                            get_old = get_old.Replace(";<br>", " ");
                            get_new = get_new.Replace(";<br>", " ");
                        }
                    }

                    if (get_old != get_new)
                    {
                        cdm = new ColumnsDiffModel
                        {
                            Column = pi.Name, //字段
                            ColumnName = displayName, //字段自定义名
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
        public ActionResult AuditPeople(string Id)
        {
            string str = string.Empty;
            DhtmlxForm dForm = new DhtmlxForm();
            var itemcols = CompareEntityDiff(Id);
            if (itemcols.Count > 0)
            {
                var checkUnits = checkUnitService.GetAllCheckUnit();
                var personnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");
                var engineersTypes = sysDictServcie.GetDictsByKey("EngineersType");
                var workStatus = sysDictServcie.GetDictsByKey("workStatus");
                var yesNo = sysDictServcie.GetDictsByKey("yesNo");

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
                    if (col.Column == "Customid" && !string.IsNullOrEmpty(col.OldValue))
                    {
                        value = checkUnits[col.OldValue];
                    }
                    else if (col.Column == "Title" && !string.IsNullOrEmpty(col.OldValue))
                    {
                        value = personnelTitles.Where(x => x.KeyValue == col.OldValue).ToList().FirstOrDefault().Name;
                    }
                    else if (col.Column == "iscb" && !string.IsNullOrEmpty(col.OldValue))
                    {
                        value = workStatus.Where(x => x.KeyValue == col.OldValue).ToList().FirstOrDefault().Name;
                    }
                    else if (col.Column == "isreghere" && !string.IsNullOrEmpty(col.OldValue))
                    {
                        value = yesNo.Where(x => x.KeyValue == col.OldValue).ToList().FirstOrDefault().Name;
                    }
                    else if (col.Column == "iszcgccs" && !string.IsNullOrEmpty(col.OldValue))
                    {
                        value = engineersTypes.Where(x => x.KeyValue == col.OldValue).ToList().FirstOrDefault().Name;
                    }
                    else if (col.Column == "ishaspostnum" && !string.IsNullOrEmpty(col.OldValue))
                    {
                        value = yesNo.Where(x => x.KeyValue == col.OldValue).ToList().FirstOrDefault().Name;
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
                    if (col.Column == "Customid" && !string.IsNullOrEmpty(col.NewValue))
                    {
                        value = checkUnits[col.NewValue];
                    }
                    else if (col.Column == "Title" && !string.IsNullOrEmpty(col.NewValue))
                    {
                        value = personnelTitles.Where(x => x.KeyValue == col.NewValue).ToList().FirstOrDefault().Name;
                    }
                    else if (col.Column == "iscb" && !string.IsNullOrEmpty(col.NewValue))
                    {
                        value = workStatus.Where(x => x.KeyValue == col.NewValue).ToList().FirstOrDefault().Name;
                    }
                    else if (col.Column == "isreghere" && !string.IsNullOrEmpty(col.NewValue))
                    {
                        value = yesNo.Where(x => x.KeyValue == col.NewValue).ToList().FirstOrDefault().Name;
                    }
                    else if (col.Column == "iszcgccs" && !string.IsNullOrEmpty(col.NewValue))
                    {
                        value = engineersTypes.Where(x => x.KeyValue == col.NewValue).ToList().FirstOrDefault().Name;
                    }
                    else if (col.Column == "ishaspostnum" && !string.IsNullOrEmpty(col.NewValue))
                    {
                        value = yesNo.Where(x => x.KeyValue == col.NewValue).ToList().FirstOrDefault().Name;
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
                dForm.AddDhtmlxFormItem(new DhtmlxFormLabel("该人员信息无修改项，无需审核！").AddStringItem("offsetLeft", "300"));
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
        public ActionResult AuditPeople(FormCollection FormCol)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;
            string opr = FormCol["Operate"].ToString();

            var personnelTitles = sysDictServcie.GetDictsByKey("personnelTitles");
            var engineersTypes = sysDictServcie.GetDictsByKey("EngineersType");
            var workStatus = sysDictServcie.GetDictsByKey("workStatus");
            var yesNo = sysDictServcie.GetDictsByKey("yesNo");

            Dictionary<string, List<SysDict>> dic = new Dictionary<string, List<SysDict>>();
            dic.Add("personnelTitles", personnelTitles);
            dic.Add("engineersTypes", engineersTypes);
            dic.Add("workStatus", workStatus);
            dic.Add("yesNo", yesNo);

            var saveResult = checkPeopleManagerService.AuditPeople(FormCol, dic, out errorMsg);
            if (!saveResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("进行了人员信息审核操作：{0}，人员ID为{1}".Fmt(opr, FormCol["Id"].ToString()));
            }

            return Content(result.ToJson());
        }

        /// <summary>
        /// 检查人员信息是否能被修改
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult CheckEdit(string Id)
        {
            var model = rep.GetById(Id);
            return Content(model.ToJson());
        }

        /// <summary>
        /// 统一人员信息比较实体
        /// </summary>
        /// <param name="people"></param>
        /// <param name="peopleTmp"></param>
        /// <returns></returns>
        public CheckPeopleCompareModel GetCheckPeopleCompareModel(t_bp_People people, t_bp_People_tmp peopleTmp)
        {
            CheckPeopleCompareModel model = null;
            if (people != null)
            {
                return new CheckPeopleCompareModel()
                {
                    id = people.id,
                    Customid = people.Customid,
                    Name = people.Name,
                    SelfNum = people.SelfNum,
                    ishaspostnum = people.ishaspostnum,
                    PostDate = people.PostDate,
                    Education = people.Education,
                    Professional = people.Professional,
                    zw = people.zw,
                    iszcgccs = people.iszcgccs,
                    isreghere = people.isreghere,
                    zcgccszh = people.zcgccszh,
                    Sex = people.Sex,
                    Birthday = people.Birthday,
                    PostNum = people.PostNum,
                    postnumstartdate = people.postnumstartdate,
                    postnumenddate = people.postnumenddate,
                    School = people.School,
                    Title = people.Title,
                    Tel = people.Tel,
                    Email = people.Email,
                    iscb = people.iscb,
                    SBNum = people.SBNum,
                    zcgccszhstartdate = people.zcgccszhstartdate,
                    zcgccszhenddate = people.zcgccszhenddate,
                    PostType = people.PostType,
                    postTypeCode = people.postTypeCode,
                    postDelayReg = people.postDelayReg,
                    Approvalstatus = people.Approvalstatus,
                    PhotoPath = people.PhotoPath,
                    selfnumPath = people.selfnumPath,
                    educationpath = people.educationpath,
                    zcgccszhpath = people.zcgccszhpath,
                    PostPath = people.PostPath,
                    titlepath = people.titlepath,
                    gznx = people.gznx,
                    data_status = "2"
                };
            }

            if (peopleTmp != null)
            {
                return new CheckPeopleCompareModel()
                {
                    id = peopleTmp.id,
                    Customid = peopleTmp.Customid,
                    Name = peopleTmp.Name,
                    SelfNum = peopleTmp.SelfNum,
                    ishaspostnum = peopleTmp.ishaspostnum,
                    PostDate = peopleTmp.PostDate,
                    Education = peopleTmp.Education,
                    Professional = peopleTmp.Professional,
                    zw = peopleTmp.zw,
                    iszcgccs = peopleTmp.iszcgccs,
                    isreghere = peopleTmp.isreghere,
                    zcgccszh = peopleTmp.zcgccszh,
                    Sex = peopleTmp.Sex,
                    Birthday = peopleTmp.Birthday,
                    PostNum = peopleTmp.PostNum,
                    postnumstartdate = peopleTmp.postnumstartdate,
                    postnumenddate = peopleTmp.postnumenddate,
                    School = peopleTmp.School,
                    Title = peopleTmp.Title,
                    Tel = peopleTmp.Tel,
                    Email = peopleTmp.Email,
                    iscb = peopleTmp.iscb,
                    SBNum = peopleTmp.SBNum,
                    zcgccszhstartdate = peopleTmp.zcgccszhstartdate,
                    zcgccszhenddate = peopleTmp.zcgccszhenddate,
                    PostType = peopleTmp.PostType,
                    postTypeCode = peopleTmp.postTypeCode,
                    postDelayReg = peopleTmp.postDelayReg,
                    Approvalstatus = peopleTmp.Approvalstatus,
                    PhotoPath = peopleTmp.PhotoPath,
                    selfnumPath = peopleTmp.selfnumPath,
                    educationpath = peopleTmp.educationpath,
                    zcgccszhpath = peopleTmp.zcgccszhpath,
                    PostPath = peopleTmp.PostPath,
                    titlepath = peopleTmp.titlepath,
                    gznx = peopleTmp.gznx,
                    data_status = "2"
                };
            }

            return model;
        }

        /// <summary>
        /// 判断输入的字符串是否只包含数字和英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumAndEnCh(string input)
        {
            string pattern = @"^[A-Za-z0-9]+$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(input);
        } 


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPeople(CheckPeopleSaveViewModel viewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            //判断身份证号是否重复
            if (!string.IsNullOrEmpty(viewModel.SelfNum) && viewModel.id > 0)
            {
                var ExistsPeople = rep.GetByCondition(r => r.SelfNum == viewModel.SelfNum && r.id != viewModel.id);
                if (ExistsPeople.Count > 0)
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = "修改失败，该身份证号已经存在！";
                    return Content(result.ToJson());
                }
            }
            string iszcgcc = string.Empty;
            if (!viewModel.iszcgccs.IsNullOrEmpty())
            {
                iszcgcc = viewModel.iszcgccs.Replace(",", "、");
            }
            //if (!string.IsNullOrEmpty(viewModel.iszcgccs))
            //{
            //    var siArrays = viewModel.iszcgccs.Split(',');
            //    iszcgcc = sysDictServcie.GetDictsByName("EngineersType", siArrays[0]);
            //    for (int i = 1; i < siArrays.Length; i++)
            //    {
            //        iszcgcc = iszcgcc + "、" + sysDictServcie.GetDictsByName("EngineersType", siArrays[i]);
            //    }
            //}
            if (!string.IsNullOrEmpty(viewModel.zw))
            {
                viewModel.zw = viewModel.zw.Replace(",", "、");
            }
            if (!string.IsNullOrEmpty(viewModel.PostType))
            {
                viewModel.PostType = viewModel.PostType.Replace(";&lt;br&gt;", ";<br>");
            }

            CheckPeopleSaveModel editModel = new CheckPeopleSaveModel();
            try
            {
                editModel.OpUserId = GetCurrentUserId();
                //editModel.CustomName = checkUnitService.GetCheckUnitByName(viewModel.Customid);
                editModel.people = new t_bp_People()
                {
                    id = viewModel.id,
                    Customid = viewModel.Customid,
                    Name = viewModel.Name,
                    SelfNum = viewModel.SelfNum,
                    Education = viewModel.Education,
                    Professional = viewModel.Professional,
                    zw = viewModel.zw,
                    iszcgccs = iszcgcc,
                    isreghere = viewModel.isreghere,
                    zcgccszh = viewModel.zcgccszh,
                    Sex = viewModel.Sex,
                    Birthday = viewModel.Birthday,
                    School = viewModel.School,
                    Title = viewModel.Title,
                    Tel = viewModel.Tel,
                    Email = viewModel.Email,
                    iscb = viewModel.iscb,
                    SBNum = viewModel.SBNum,
                    zcgccszhstartdate = viewModel.zcgccszhstartdate,
                    zcgccszhenddate = viewModel.zcgccszhenddate,
                    PostType = viewModel.PostType,
                    postTypeCode = viewModel.postTypeCode,
                    postDelayReg = viewModel.postDelayReg,
                    Approvalstatus = viewModel.Approvalstatus,
                    PhotoPath = viewModel.PhotoPath,
                    selfnumPath = viewModel.selfnumPath,
                    educationpath = viewModel.educationpath,
                    zcgccszhpath = viewModel.zcgccszhpath,
                    PostPath = viewModel.PostPath,
                    titlepath = viewModel.titlepath,
                    update_time = DateTime.Now,
                    data_status = "2"
                };

                DateTime? postnumstartdate = null, postnumenddate = null;
                DateTime? zcgccszhstartdate = null, zcgccszhenddate = null;
                CommonUtils.GetLayuiDateRange(viewModel.postnumdate, out postnumstartdate, out postnumenddate);
                CommonUtils.GetLayuiDateRange(viewModel.zcgccszhdate, out zcgccszhstartdate, out zcgccszhenddate);

                editModel.people.postnumstartdate = postnumstartdate;
                editModel.people.postnumenddate = postnumenddate;
                editModel.people.zcgccszhstartdate = zcgccszhstartdate;
                editModel.people.zcgccszhenddate = zcgccszhenddate;


                editModel.PeoChanges = new List<t_bp_PeoChange>();
                if (viewModel.peoChange != null)
                {
                    foreach (var peoChange in viewModel.peoChange)
                    {
                        peoChange.PeopleId = viewModel.id;
                        editModel.PeoChanges.Add(peoChange);
                    }
                }

                //editModel.PeopleNs = new List<t_bp_PeopleN>();
                //if (viewModel.peoVerification != null)
                //{
                //    foreach (var peoVerification in viewModel.peoVerification)
                //    {
                //        peoVerification.PeopleID = viewModel.id.ToString();
                //        editModel.PeopleNs.Add(peoVerification);
                //    }
                //}

                editModel.PeoEducations = new List<t_bp_PeoEducation>();
                if (viewModel.peoEducation != null)
                {
                    foreach (var peoEducation in viewModel.peoEducation)
                    {
                        peoEducation.PeopleId = viewModel.id;
                        editModel.PeoEducations.Add(peoEducation);
                    }
                }

                editModel.PeoAwards = new List<t_bp_PeoAward>();
                if (viewModel.peoAwards != null)
                {
                    foreach (var peoAwards in viewModel.peoAwards)
                    {
                        peoAwards.PeopleId = viewModel.id;
                        editModel.PeoAwards.Add(peoAwards);
                    }
                }

                editModel.PeoPunishs = new List<t_bp_PeoPunish>();
                if (viewModel.peoPunish != null)
                {
                    foreach (var peoPunish in viewModel.peoPunish)
                    {
                        peoPunish.PeopleId = viewModel.id;
                        editModel.PeoPunishs.Add(peoPunish);
                    }
                }
                string erroMsg = string.Empty;
                if (!checkPeopleManagerService.EditPeopleField(editModel, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对id为{0}的人员信息进行了修改操作".Fmt(viewModel.id));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        // GET: CheckPeopleManager/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Screening(string selectedId, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkPeopleManagerService.SetPeopleScreeningState(selectedId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对人员ID为{0}进行了状态屏蔽操作".Fmt(selectedId));
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
        public ActionResult RelieveScreening(string selectedId, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkPeopleManagerService.SetPeopleRelieveScreeningSate(selectedId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对人员ID为{0}进行了状态解除屏蔽操作".Fmt(selectedId));
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
        public ActionResult setPosttypeInfo(string id, string mv)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkPeopleManagerService.SetPosttype(id, mv, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对id为{0}的人员的Posttype字段进行了修改操作，修改内容为{1}".Fmt(id, mv));
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
        //public ActionResult AutoTypePeople(string userId)
        //{
        //    var result = "";
        //    if (userId != null)
        //    {
        //        try
        //        {
        //            var AutoType = checkPeopleManagerService.GetAutoType(userId);
        //            if (AutoType.Count != 0)
        //            {
        //                var people = rep.GetByCondition(p => p.SelfNum == AutoType[0].CardNo).First();

        //                if (people != null)
        //                {
        //                    string myPostType = people.PostType;//t_bp_people 人员信息中公示序号;
        //                    string postTypeCode = people.postTypeCode;//t_bp_people postTypeCode 人员信息中公示序号;

        //                    foreach (var item in AutoType)
        //                    {
        //                        //t_bp_people posttype 人员信息中公示序号
        //                        if (myPostType.IndexOf(item.ff) != -1)
        //                        {
        //                            int Beg = myPostType.IndexOf(item.ff);
        //                            int End = myPostType.Substring(Beg).IndexOf(";<br>");
        //                            string ValueString = myPostType.Substring(Beg).Substring(0, End);
        //                            if (ValueString.IndexOf("[X]") != -1)
        //                            {
        //                                string XValue = ValueString.Substring(ValueString.IndexOf("[X]"), ValueString.Substring(ValueString.IndexOf("[X]")).IndexOf("[/X]")) + "[/X]";
        //                                string ValueStringN = ValueString.Replace(XValue, "[X]" + item.listnum + "[/X]");
        //                                myPostType = myPostType.Replace(ValueString, ValueStringN);
        //                            }
        //                            else
        //                            {
        //                                myPostType = myPostType.Replace(ValueString, ValueString + "[X]" + item.listnum + "[/X]");
        //                            }
        //                        }
        //                        else
        //                        {
        //                            myPostType = myPostType + "" + item.ff + "[X]" + item.listnum + "[/X];<br>";
        //                        }

        //                        //t_bp_people postTypeCode 人员信息中公示序号;
        //                        if (postTypeCode.IndexOf(item.code) == -1)
        //                        {
        //                            postTypeCode = postTypeCode + "|" + item.code;
        //                        }
        //                    }
        //                    string erroMsg = string.Empty;
        //                    if (!checkPeopleManagerService.UpdatePostType(myPostType, postTypeCode, AutoType[0].CardNo, out erroMsg))
        //                    {
        //                        result = erroMsg;
        //                    }
        //                    else
        //                    {
        //                        LogUserAction("对身份证为{0}的人员资质进行了修改操作，修改内容为PostType：{1}，postTypeCode：{2}".Fmt(AutoType[0].CardNo, myPostType, postTypeCode));
        //                    }
        //                    result = "自动生成成功！";
        //                }
        //                else { result = "自动生成失败！已通过考试中无此人信息！"; }
        //            }
        //            else { result = "自动生成失败！已通过考试中无此人信息！"; }
        //        }
        //        catch (Exception ex)
        //        {
        //            result = ex.Message;
        //        }
        //    }



        //    return Content(result.ToJson());
        //}

        // POST: CheckPeopleManager/Delete/5

        [HttpPost]
        public ActionResult Delete(string selectedId)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkPeopleManagerService.DeletePeople(selectedId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对id为{0}的人员进行了删除操作".Fmt(selectedId));
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
        public ActionResult Send(string selectedId)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkPeopleManagerService.SendPeople(selectedId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对id为{0}的人员进行了递交操作".Fmt(selectedId));
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
        public ActionResult ReturnState(string selectedId)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkPeopleManagerService.ReturnStatePeople(selectedId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对id为{0}的人员进行了状态返回操作".Fmt(selectedId));
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
        public ActionResult AnnualTest(string selectedId)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkPeopleManagerService.AnnualTestPeople(selectedId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对id为{0}的人员进行了年审操作".Fmt(selectedId));
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
        public ActionResult FirePeople(string selectedId)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkPeopleManagerService.FirePeople(selectedId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对id为{0}的人员进行了离职操作".Fmt(selectedId));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        public ActionResult UnitChange(string peopleId, string peopleName)
        {
            PeopleUnitChangeModel model = new PeopleUnitChangeModel()
            {
                PeopleId = peopleId,
                PeopleName = peopleName,
                AllUnit = new Dictionary<string, string>()
            };
            model.AllUnit = checkUnitService.GetAllCheckUnit();
            return View(model);
        }

        [HttpPost]
        public ActionResult unitChange(string peopleId, string Customid)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string erroMsg = string.Empty;
                if (!checkPeopleManagerService.ChangePeople(peopleId, Customid, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("对id为{0}的人员进行了机构变更操作，变更为{1}".Fmt(peopleId, Customid));
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
        public ActionResult ApplyChange(CheckPeopleApplyChange applyChangeModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                SupvisorJob job = new SupvisorJob()
                {
                    ApproveType = ApproveType.ApprovePeople,
                    CreateBy = GetCurrentUserId(),
                    CustomId = applyChangeModel.SubmitCustomId,
                    CreateTime = DateTime.Now,
                    NeedApproveId = applyChangeModel.SubmitId.ToString(),
                    NeedApproveStatus = NeedApproveStatus.CreateForChangeApply,
                    SubmitName = applyChangeModel.SubmitName,
                    SubmitText = applyChangeModel.SubmitText
                };


                string erroMsg = string.Empty;
                if (!checkPeopleManagerService.ApplyChangePeople(job, applyChangeModel.SubmitId, out erroMsg))
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = erroMsg;
                }
                else
                {
                    LogUserAction("进行了申请修改操作，申请人{0}，申请原因{1}".Fmt(applyChangeModel.SubmitName, applyChangeModel.SubmitText));
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        public ActionResult ChangeGrid(int id)
        {
            var peoChange = peoChangeRep.GetByCondition(r => r.PeopleId == id);
            DhtmlxGrid grid = new DhtmlxGrid();

            for (int i = 0; i < peoChange.Count; i++)
            {
                var change = peoChange[i];
                DhtmlxGridRow row = new DhtmlxGridRow(change.id.ToString());
                row.AddCell((i + 1).ToString());
                row.AddCell(change.ChaDate.HasValue ? change.ChaDate.Value.ToString("yyyy-MM-dd") : string.Empty);
                row.AddCell(change.ChaContent);
                row.AddCell(change.id.ToString());
                row.AddCell(change.PeopleId.ToString());
                row.AddLinkJsCell("编辑", "changeGrid_edit({0})".Fmt(change.id));
                row.AddLinkJsCell("删除", "changeGrid_delete({0})".Fmt(change.id));

                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }



        //public ActionResult VerificationGrid(int id)
        //{
        //    var peopleN = peopleNRep.GetByCondition(r => r.PeopleID == id.ToString());
        //    DhtmlxGrid grid = new DhtmlxGrid();

        //    for (int i = 0; i < peopleN.Count; i++)
        //    {
        //        var peopleNs = peopleN[i];
        //        DhtmlxGridRow row = new DhtmlxGridRow(peopleNs.id.ToString());
        //        row.AddCell((i + 1).ToString());
        //        row.AddCell(peopleNs.addtime.HasValue ? peopleNs.addtime.Value.ToString("yyyy-MM-dd") : string.Empty);
        //        row.AddCell(peopleNs.Pcontext);
        //        row.AddCell(peopleNs.id.ToString());
        //        row.AddCell(peopleNs.PeopleID);
        //        row.AddLinkJsCell("删除", "verificationGrid_delete({0})".Fmt(peopleNs.id.ToString()));
        //        grid.AddGridRow(row);
        //    }

        //    string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

        //    return Content(str, "text/xml");
        //}

        public ActionResult EducationGrid(int id)
        {
            var education = eduRep.GetByCondition(r => r.PeopleId == id);
            DhtmlxGrid grid = new DhtmlxGrid();

            for (int i = 0; i < education.Count; i++)
            {
                var edu = education[i];
                DhtmlxGridRow row = new DhtmlxGridRow(edu.id.ToString());
                row.AddCell((i + 1).ToString());
                row.AddCell(edu.TrainDate.HasValue ? edu.TrainDate.Value.ToString("yyyy-MM-dd") : string.Empty);
                row.AddCell(edu.TrainContent);
                row.AddCell(edu.TrainUnit);
                row.AddCell(edu.TestDate.HasValue ? edu.TestDate.Value.ToString("yyyy-MM-dd") : string.Empty);
                row.AddCell(edu.id.ToString());
                row.AddCell(edu.PeopleId.ToString());
                row.AddLinkJsCell("删除", "educationGrid_delete({0})".Fmt(edu.id.ToString()));
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult AwardsGrid(int id)
        {
            var peoAward = peoAwardRep.GetByCondition(r => r.PeopleId == id);
            DhtmlxGrid grid = new DhtmlxGrid();

            for (int i = 0; i < peoAward.Count; i++)
            {
                var peoAwa = peoAward[i];
                DhtmlxGridRow row = new DhtmlxGridRow(peoAwa.id.ToString());
                row.AddCell((i + 1).ToString());
                row.AddCell(peoAwa.AwaDate.HasValue ? peoAwa.AwaDate.Value.ToString("yyyy-MM-dd") : string.Empty);
                row.AddCell(peoAwa.AwaContent);
                row.AddCell(peoAwa.AwaUnit);
                row.AddCell(peoAwa.id.ToString());
                row.AddCell(peoAwa.PeopleId.ToString());
                row.AddLinkJsCell("删除", "awardsGrid_delete({0})".Fmt(peoAwa.id.ToString()));
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult PunishGrid(int id)
        {
            var punish = punishRep.GetByCondition(r => r.PeopleId == id);
            DhtmlxGrid grid = new DhtmlxGrid();

            for (int i = 0; i < punish.Count; i++)
            {
                var pu = punish[i];
                DhtmlxGridRow row = new DhtmlxGridRow(pu.id.ToString());
                row.AddCell((i + 1).ToString());
                row.AddCell(pu.PunDate.HasValue ? pu.PunDate.Value.ToString("yyyy-MM-dd") : string.Empty);
                row.AddCell(pu.PunName);
                row.AddCell(pu.PunUnit);
                row.AddCell(pu.PunContent);
                row.AddCell(pu.id.ToString());
                row.AddCell(pu.PeopleId.ToString());
                row.AddLinkJsCell("删除", "punishGrid_delete({0})".Fmt(pu.id.ToString()));
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        /// <summary>
        /// 查看界面处罚情况表格
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CheckPeoplePunishModel> CheckPeoplePunish(int id)
        {
            var CheckPeoplePunishs = new List<CheckPeoplePunishModel>();
            var punish = punishRep.GetByCondition(r => r.PeopleId == id);
            for (int i = 0; i < punish.Count; i++)
            {
                var pu = punish[i];
                CheckPeoplePunishModel CheckPeoplePunish = new CheckPeoplePunishModel();
                CheckPeoplePunish.Number = (i + 1).ToString();
                CheckPeoplePunish.PunDate = pu.PunDate.HasValue ? pu.PunDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                CheckPeoplePunish.PunName = pu.PunName;
                CheckPeoplePunish.PunUnit = pu.PunUnit;
                CheckPeoplePunish.PunContent = pu.PunContent;
                CheckPeoplePunish.Id = pu.id.ToString();
                CheckPeoplePunish.PeopleId = pu.PeopleId.ToString();
                CheckPeoplePunishs.Add(CheckPeoplePunish);
            }
            return CheckPeoplePunishs;
        }

        /// <summary>
        /// 查看界面获奖情况表格
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CheckPeopleAwardsModel> CheckPeopleAwards(int id)
        {
            List<CheckPeopleAwardsModel> CheckPeopleAwards = new List<CheckPeopleAwardsModel>();
            var peoAward = peoAwardRep.GetByCondition(r => r.PeopleId == id);
            for (int i = 0; i < peoAward.Count; i++)
            {
                var peoAwa = peoAward[i];
                CheckPeopleAwardsModel model = new CheckPeopleAwardsModel();
                model.Number = (i + 1).ToString();
                model.AwaDate = peoAwa.AwaDate.HasValue ? peoAwa.AwaDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                model.AwaContent = peoAwa.AwaContent;
                model.AwaUnit = peoAwa.AwaUnit;
                model.Id = peoAwa.id.ToString();
                model.PeopleId = peoAwa.PeopleId.ToString();
                CheckPeopleAwards.Add(model);
            }
            return CheckPeopleAwards;
        }

        /// <summary>
        /// 查看界面变更情况表格
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CheckPeopleChangeModel> CheckPeopleChange(int id)
        {
            List<CheckPeopleChangeModel> CheckPeopleChanges = new List<CheckPeopleChangeModel>();
            var peoChange = peoChangeRep.GetByCondition(r => r.PeopleId == id);
            for (int i = 0; i < peoChange.Count; i++)
            {
                var change = peoChange[i];
                CheckPeopleChangeModel CheckPeopleChange = new CheckPeopleChangeModel();
                CheckPeopleChange.Number = i + 1;
                CheckPeopleChange.ChaDate = change.ChaDate.HasValue ? change.ChaDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                CheckPeopleChange.ChaContent = change.ChaContent;
                CheckPeopleChange.Id = change.id.ToString();
                CheckPeopleChange.PeopleId = change.PeopleId.ToString();
                CheckPeopleChanges.Add(CheckPeopleChange);
            }
            return CheckPeopleChanges;
        }

        /// <summary>
        /// 查看界面年审
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //public List<CheckPeopleVerificationModel> CheckPeopleVerification(int id)
        //{
        //    List<CheckPeopleVerificationModel> CheckPeopleVerifications = new List<CheckPeopleVerificationModel>();
        //    var peopleN = peopleNRep.GetByCondition(r => r.PeopleID == id.ToString());
        //    for (int i = 0; i < peopleN.Count; i++)
        //    {
        //        var peopleNs = peopleN[i];
        //        CheckPeopleVerificationModel CheckPeopleVerification = new CheckPeopleVerificationModel();
        //        CheckPeopleVerification.Number = i + 1;
        //        CheckPeopleVerification.AddTime = peopleNs.addtime.HasValue ? peopleNs.addtime.Value.ToString("yyyy-MM-dd") : string.Empty;
        //        CheckPeopleVerification.Pcontext = peopleNs.Pcontext;
        //        CheckPeopleVerification.Id = peopleNs.id.ToString();
        //        CheckPeopleVerification.PeopleId = peopleNs.PeopleID;
        //        CheckPeopleVerifications.Add(CheckPeopleVerification);
        //    }
        //    return CheckPeopleVerifications;
        //}

        /// <summary>
        /// 查看界面继续教育情况
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CheckPeopleEducationModel> CheckPeopleEducation(int id)
        {
            List<CheckPeopleEducationModel> CheckPeopleEducations = new List<CheckPeopleEducationModel>();
            var education = eduRep.GetByCondition(r => r.PeopleId == id);
            for (int i = 0; i < education.Count; i++)
            {
                var edu = education[i];
                CheckPeopleEducationModel CheckPeopleEducation = new CheckPeopleEducationModel();
                CheckPeopleEducation.Number = i + 1;
                CheckPeopleEducation.TrainDate = edu.TrainDate.HasValue ? edu.TrainDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                CheckPeopleEducation.TrainContent = edu.TrainContent;
                CheckPeopleEducation.TrainUnit = edu.TrainUnit;
                CheckPeopleEducation.TestDate = edu.TestDate.HasValue ? edu.TestDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                CheckPeopleEducation.Id = edu.id.ToString();
                CheckPeopleEducation.PeopleId = edu.PeopleId.ToString();
                CheckPeopleEducations.Add(CheckPeopleEducation);
            }
            return CheckPeopleEducations;
        }

        /// <summary>
        /// 查看界面附件上传
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CheckPeopleFileModel> CheckPeopleFile(int id,
            string PhotoPath,
            string selfnumPath,
            string educationpath,
            string zcgccszhpath,
            string PostPath,
            string titlepath)
        {
            string[,] FileRangeAr = new string[6, 3];
            FileRangeAr[0, 0] = "证件照";
            FileRangeAr[1, 0] = "身份证";
            FileRangeAr[2, 0] = "最高学历证件";
            FileRangeAr[3, 0] = "注册工程师证书";
            FileRangeAr[4, 0] = "岗位证书";
            FileRangeAr[5, 0] = "职称证书";

            if (id > 0)
            {
                var people = rep.GetById(id);
                FileRangeAr[0, 1] = people.PhotoPath;
                FileRangeAr[1, 1] = people.selfnumPath;
                FileRangeAr[2, 1] = people.educationpath;
                FileRangeAr[3, 1] = people.zcgccszhpath;
                FileRangeAr[4, 1] = people.PostPath;
                FileRangeAr[5, 1] = people.titlepath;
            }
            else
            {
                FileRangeAr[0, 1] = "";
                FileRangeAr[1, 1] = "";
                FileRangeAr[2, 1] = "";
                FileRangeAr[3, 1] = "";
                FileRangeAr[4, 1] = "";
                FileRangeAr[5, 1] = "";
            }
            FileRangeAr[0, 2] = PhotoPath;
            FileRangeAr[1, 2] = selfnumPath;
            FileRangeAr[2, 2] = educationpath;
            FileRangeAr[3, 2] = zcgccszhpath;
            FileRangeAr[4, 2] = PostPath;
            FileRangeAr[5, 2] = titlepath;

            List<CheckPeopleFileModel> CheckPeopleFiles = new List<CheckPeopleFileModel>();
            for (int i = 0; i < 6; i++)
            {
                CheckPeopleFileModel CheckPeopleFile = new CheckPeopleFileModel();
                CheckPeopleFile.FileName = FileRangeAr[i, 0];
                CheckPeopleFile.Number = i;
                CheckPeopleFile.FileState = string.IsNullOrEmpty(FileRangeAr[i, 1]) ? "未上传" : "已上传";
                CheckPeopleFile.Path = FileRangeAr[i, 2];
                CheckPeopleFiles.Add(CheckPeopleFile);
            }
            return CheckPeopleFiles;
        }


        public ActionResult FileGrid(int id)
        {

            string[,] FileRangeAr = new string[6, 2];
            FileRangeAr[0, 0] = "证件照";
            FileRangeAr[1, 0] = "身份证";
            FileRangeAr[2, 0] = "最高学历证件";
            FileRangeAr[3, 0] = "注册工程师证书";
            FileRangeAr[4, 0] = "岗位证书";
            FileRangeAr[5, 0] = "职称证书";

            if (id > 0)
            {
                var people = rep.GetById(id);
                FileRangeAr[0, 1] = people.PhotoPath;
                FileRangeAr[1, 1] = people.selfnumPath;
                FileRangeAr[2, 1] = people.educationpath;
                FileRangeAr[3, 1] = people.zcgccszhpath;
                FileRangeAr[4, 1] = people.PostPath;
                FileRangeAr[5, 1] = people.titlepath;
            }
            else
            {
                FileRangeAr[0, 1] = "";
                FileRangeAr[1, 1] = "";
                FileRangeAr[2, 1] = "";
                FileRangeAr[3, 1] = "";
                FileRangeAr[4, 1] = "";
                FileRangeAr[5, 1] = "";
            }


            DhtmlxGrid grid = new DhtmlxGrid();

            for (int i = 0; i < 6; i++)
            {
                DhtmlxGridRow row = new DhtmlxGridRow(i.ToString());
                row.AddCell(FileRangeAr[i, 0]);
                row.AddCell(string.IsNullOrEmpty(FileRangeAr[i, 1]) ? "未上传" : "已上传");
                row.AddLinkJsCell("查看", "fileGrid_look(\"{0}\")".Fmt(i));
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult SeleteGrid()
        {
            var paths = postTypeRep.GetByCondition(r => r.Id != null);
            //var actions = postTypeRep.GetByCondition(ar => paths.Select(p => p.Id).ToList().Contains(ar.parentId));
            var pathsOne = paths.Where(a => a.parentId == "0").ToList();

            DhtmlxGrid grid = new DhtmlxGrid();


            foreach (var item in pathsOne.OrderBy(p => p.Id))
            {
                DhtmlxGridRow row = new DhtmlxGridRow(item.Id.ToString());

                row.AddCell(item.postType);

                var pathsTwos = paths.Where(a => a.parentId == item.Id).ToList();

                foreach (var pathAction in pathsTwos.OrderBy(p => p.Id))
                {
                    DhtmlxGridRow actionRow = new DhtmlxGridRow(pathAction.Id);
                    actionRow.AddCell(pathAction.postType);
                    var pathsLasts = paths.Where(a => a.parentId == pathAction.Id).ToList();

                    foreach (var pathsLast in pathsLasts.OrderBy(p => p.Id))
                    {
                        DhtmlxGridRow lastRow = new DhtmlxGridRow(pathsLast.Id);
                        lastRow.AddCell(pathsLast.postType);
                        lastRow.AddCell(string.Empty);
                        lastRow.AddCell(pathsLast.code);
                        lastRow.AddCell(string.Empty);
                        lastRow.AddCell(string.Empty);

                        actionRow.AddRow(lastRow);
                    }

                    row.AddRow(actionRow);
                }

                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult Category()
        {
            List<t_bp_postType> paths = postTypeRep.GetByCondition(r => r.parentId == "0");

            DhtmlxTreeView treeView = new DhtmlxTreeView();
            DhtmlxTreeViewItem item = new DhtmlxTreeViewItem("Main", "资格类别", true, false, DhtmlxTreeViewCheckbox.NotSet);
            foreach (var menuItem in paths)
            {
                item.AddTreeViewItem(menuItem.Id.ToString(), menuItem.postType);
            }
            treeView.AddTreeViewItem(item);

            string str = treeView.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        public ActionResult Change(string Customid)
        {
            string peoplePart = Customid.Substring(0, Customid.Length - 1);
            var peopleParts = checkUnitService.GetPartCheckUnit(peoplePart);

            XElement element = new XElement("complete",
                from kv in peopleParts
                select new XElement("option", new XAttribute("value", kv.Key), new XText(kv.Value)));
            return Content(element.ToString(System.Xml.Linq.SaveOptions.DisableFormatting), "text/xml");
        }

        [HttpPost]
        public ActionResult attachUpload(string customName)
        {
            string customid = checkUnitService.GetCheckUnitByName(customName);

            string realname = Request.Files["file"].FileName;

            var extensionName = System.IO.Path.GetExtension(realname);

            string filename = "/{0}/{1}{2}".Fmt(customid, Guid.NewGuid().ToString().Replace("-", ""), extensionName);

            fileHander.UploadFile(Request.Files["file"].InputStream, "userfiles", filename);

            DhtmlxUploaderResult uploader = new DhtmlxUploaderResult() { state = true, name = filename };
            string str = uploader.ToJson();
            return Content(str);
        }

        public FileResult Image(ImageViewDownload model)
        {
            if (!model.itemValue.IsNullOrEmpty() && model.itemValue.Contains('|'))
            {
                model.itemValue.Replace("|", string.Empty);
                int? lastIndex = model.itemValue.LastIndexOf('|');
                if (lastIndex == 0)
                {
                    model.itemValue = model.itemValue.Substring(1);
                }
                else if (lastIndex == model.itemValue.Length - 1)
                {
                    model.itemValue = model.itemValue.Substring(0, model.itemValue.Length - 1);
                }
            }
            //导过来的历史数据，路径都带有"/userfiles"，文件系统里面的路径没有"/userfiles"，所以要去掉
            string fileName = Regex.Replace(model.itemValue, Regex.Escape("/userfiles"), "", RegexOptions.IgnoreCase);//model.itemValue;
            string mimeType = MimeMapping.GetMimeMapping(fileName);
            Stream stream = fileHander.LoadFile("userfiles", fileName);
            return new FileStreamResult(stream, mimeType);


        }


        [HttpPost]
        public ActionResult UpdatePaths(ImageViewUpload model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errMsg = string.Empty;
            bool success = this.checkPeopleManagerService.UpdateAttachPathsIntoPeople(model.id, model.itemName, model.itemValue, out errMsg);
            if (!success)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errMsg;
                LogUserAction("对人员id为{0}的{1}字段进行了存储地址的更新,操作失败".Fmt(model.itemId, model.itemName));
            }
            else
            {
                LogUserAction("对人员id为{0}的{1}字段进行了存储地址的更新".Fmt(model.itemId, model.itemName));
            }
            return Content(result.ToJson());
        }

        [HttpPost]
        public ActionResult DeleteImage(ImageViewUpload model)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string fileName = Regex.Replace(model.itemValue, Regex.Escape("/userfiles"), "", RegexOptions.IgnoreCase);
            try
            {
                fileHander.DeleteFile("userfiles", fileName);
                if (model.id > 0)
                {
                    string errMsg = string.Empty;
                    var people = rep.GetById(model.id);
                    string pathvalues = string.Empty;
                    switch (model.itemName)
                    {
                        case "PhotoPath":
                            pathvalues = people.PhotoPath;
                            break;
                        case "selfnumPath":
                            pathvalues = people.selfnumPath;
                            break;
                        case "educationpath":
                            pathvalues = people.educationpath;
                            break;
                        case "zcgccszhpath":
                            pathvalues = people.zcgccszhpath;
                            break;
                        case "PostPath":
                            pathvalues = people.PostPath;
                            break;
                        case "titlepath":
                            pathvalues = people.titlepath;
                            break;
                    }
                    var queryStr = from str in pathvalues.Split('|')
                                   where str != model.itemValue
                                   select str;
                    //model.itemName存的是字段名
                    bool success = this.checkPeopleManagerService.UpdateAttachPathsIntoPeople(model.id, model.itemName, queryStr.Join("|"), out errMsg);
                    if (success)
                    {
                        LogUserAction("对人员id为{0}的{1}字段进行了存储地址的更新操作".Fmt(model.itemId, model.itemName));
                    }
                    else
                    {
                        LogUserAction("对人员id为{0}的{1}字段进行了存储地址的更新,操作失败".Fmt(model.itemId, model.itemName));
                    }
                }

            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = "删除失败:" + ex.Message;
            }

            return Content(result.ToJson());
        }

        public ActionResult PrintPost(PrintPostViewModel postModel)
        {
            //保存postnumstartdate，postnumenddate
            if (!string.IsNullOrWhiteSpace(postModel.postdate))
            {
                DateTime PostNumStartDate = DateTime.Now;
                DateTime.TryParse(postModel.postdate, out PostNumStartDate);
                DateTime PostNumEndDate = PostNumStartDate.AddYears(3);
                string errMsg = string.Empty;
                bool success = this.checkPeopleManagerService.UpdatePostNumDate(int.Parse(postModel.peopleId), PostNumStartDate, PostNumStartDate, PostNumEndDate, out errMsg);
                if (success)
                {
                    LogUserAction("对id为{0}的资质时间的更新操作".Fmt(postModel.peopleId));
                }
                else
                {
                    LogUserAction("对id为{0}的资质时间的更新操作,操作失败".Fmt(postModel.peopleId));
                }
            }

            return View(postModel);
        }

        public ActionResult EditReportModel(PrintPostViewModel postModel)
        {
            return View(postModel);
        }

        [HttpPost]
        public ActionResult IsExistFile(string id)
        {
            string FileStr = @"/printcard/html/peoplePostBook/{0}.html".Fmt(id);
            FileStr = Server.MapPath(FileStr);
            ControllerResult result = ControllerResult.FailResult;
            if (System.IO.File.Exists(FileStr))
            {
                result = ControllerResult.SuccResult;
            }
            else { result.ErroMsg = "没出错，只是文件不存在。"; }
            return Content(result.ToJson());
        }

        [HttpPost]
        public ActionResult GetPrintCardPeople(string id)
        {
            var people = rep.GetById(id);
            List<t_bp_People> peoplelist = new List<t_bp_People>();
            peoplelist.Add(people);
            //把json的key转成小写。打证的前台用的小写。
            StringBuilder JsonData = new StringBuilder();
            JsonData.Append(peoplelist.ToJson());
            MatchCollection ms = Regex.Matches(JsonData.ToString(), "\\\"[a-zA-Z0-9]+\\\"\\s*:");
            foreach (Match item in ms)
            {
                JsonData.Replace(item.Value, item.Value.ToLower());
            }
            //把JSON的Date类型转成字符串
            ms = Regex.Matches(JsonData.ToString(), @"\\/Date\((\d+)\-\d+\)\\/");
            foreach (Match item in ms)
            {
                JsonData.Replace(item.Value, ConvertJsonDateToDateString(item));
            }
            return Content(JsonData.ToString());

        }

        /// <summary>
        /// 将Json序列化的时间由/Date(1304931520336+0800)/转为字符串
        /// </summary>
        private string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        [HttpPost]
        public ActionResult GetcellModelParams(string id)
        {
            var cellParams = cellRep.GetByCondition(r => r.id == id);
            return Content(cellParams.ToJson());
        }

        [HttpPost]
        public ActionResult GetPeoChange(int id)
        {
            var peoChange = peoChangeRep.GetByCondition(r => r.PeopleId == id);
            StringBuilder JsonData = new StringBuilder();
            JsonData.Append(peoChange.ToJson());
            MatchCollection ms = Regex.Matches(JsonData.ToString(), "\\\"[a-zA-Z0-9]+\\\"\\s*:");
            foreach (Match item in ms)
            {
                JsonData.Replace(item.Value, item.Value.ToLower());
            }
            return Content(JsonData.ToString());
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SavePeoplePostBook(string id)
        {
            string mV = string.Empty;
            ControllerResult result = ControllerResult.SuccResult;
            if (Request.QueryString["HNT"] != null)
                mV = Request.QueryString["HNT"];
            byte[] b = new byte[Request.InputStream.Length];
            Request.InputStream.Read(b, 0, b.Length);
            string FileStr = @"/printcard/html/peoplePostBook/{0}.html".Fmt(id);
            FileStr = Server.MapPath(FileStr);
            try
            {
                System.IO.File.WriteAllBytes(FileStr, b);
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
                System.IO.File.WriteAllBytes(FileStr, b);
            }
            return Content(result.ToJson());
        }

        /// <summary>
        /// 保存模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult SaveReportModel(string id)
        {
            string mV = string.Empty;
            ControllerResult result = ControllerResult.SuccResult;
            if (Request.QueryString["HNT"] != null)
                mV = Request.QueryString["HNT"];
            byte[] b = new byte[Request.InputStream.Length];
            Request.InputStream.Read(b, 0, b.Length);
            string FileStr = @"/printcard/reportModel/published{0}.html".Fmt(id);
            FileStr = Server.MapPath(FileStr);
            try
            {
                System.IO.File.WriteAllBytes(FileStr, b);
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
                System.IO.File.WriteAllBytes(FileStr, b);
            }
            return Content(result.ToJson());
        }

        /// <summary>
        /// 更新t_sys_cellModelParam.modelParams
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ActionResult WriteVariables(string id, string value)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errMsg = string.Empty;
            bool success = this.checkPeopleManagerService.UpdatecellModelParams(id, value, out errMsg);

            if (!success)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errMsg;
                LogUserAction("对id为{0}的模板类别信息的更新操作失败".Fmt(id));
            }
            else
            {
                LogUserAction("对id为{0}的模板类别信息的更新操作".Fmt(id));
            }
            return Content(result.ToJson());
        }

        public ActionResult ChangeAddCheckItem()
        {
            return View();
        }

        public ActionResult VerificationAddCheckItem()
        {
            return View();
        }

        public ActionResult EducationAddCheckItem()
        {
            return View();
        }

        public ActionResult AwardsAddCheckItem()
        {
            return View();
        }

        public ActionResult PunishAddCheckItem()
        {
            return View();
        }

        public ActionResult EditFile(string filePath, string id, string name)
        {
            CheckPeopleEditAttachFileModel model = new CheckPeopleEditAttachFileModel();
            model.FilePath = filePath;
            model.id = id;
            if (!filePath.IsNullOrEmpty())
            {
                var index = filePath.LastIndexOf('|');
                if (index == filePath.Length - 1)
                {
                    filePath = filePath.Substring(0, filePath.Length - 1);
                }

                model.Paths = filePath.Split('|');
            }
            model.Name = name;
            return View(model);
        }

        public ActionResult EditChange(string ChangeTable)
        {
            t_bp_PeoChange model = new t_bp_PeoChange();
            if (!ChangeTable.IsNullOrEmpty())
            {
                var changeTable = ChangeTable.Split('\t');
                int index = 0;
                foreach (var item in changeTable)
                {
                    switch (index)
                    {
                        case 0:
                            model.ChaDate = DateTime.Parse(item);
                            break;
                        case 1:
                            model.ChaContent = item;
                            break;
                    }
                    index++;
                }
            }
            return View(model);
        }

        public ActionResult GetFile(string path, int IsModify)
        {
            var model = new FileModel()
            {
                Paths = new Dictionary<string, int>()
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
                    model.Paths.Add(item, number++);
                    //model.Number = number++;
                }
            }
            model.Modify = IsModify;
            return View(model);
        }


    }
}
