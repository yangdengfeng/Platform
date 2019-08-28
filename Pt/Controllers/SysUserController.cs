using Dhtmlx.Model.DataView;
using Dhtmlx.Model.Form;
using Dhtmlx.Model.Grid;
using Dhtmlx.Model.Menu;
using Dhtmlx.Model.TreeView;
using Microsoft.AspNet.Identity.Owin;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.SysDictCore;
using Pkpm.Core.UserCustomize;
using Pkpm.Core.UserRoleCore;
using Pkpm.Entity;
using Pkpm.Entity.DTO;
using Pkpm.Framework.Common;
using Pkpm.Framework.FileHandler;
using Pkpm.Framework.PkpmConfigService;
using Pkpm.Framework.Repsitory;
using PkpmGX;
using PkpmGX.Architecture;
using PkpmGX.Models;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Pkpm.Web.Controllers
{
    [Authorize]
    public class SysUserController : PkpmController
    {
        IRepsitory<User> userRep;
       
        IRepsitory<Role> roleRep;
        IRepsitory<Entity.Path> menuRep;
        IRepsitory<UserInCustom> userCustomrep;
        IRepsitory<UserInArea> userInAreRep;
        IRepsitory<UserInRole> userInRoleRep;
        IRepsitory<UserAsRole> userAsRoleRep;
        ICheckUnitService checkUnitService;
        ISysDictService sysDictService;
        IRoleService roleService;
        IUserCustomize userCustomizeService;
        IFileHandler fileHander;
        IRepsitory<t_bp_custom> t_bp_customRep;
        IPkpmConfigService pkpmConfigService;

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private static string superVisorRoleCode = System.Configuration.ConfigurationManager.AppSettings["SuperVisorRoleCode"];
        private static string QrRoleCode = System.Configuration.ConfigurationManager.AppSettings["QrRoleCode"];

        public SysUserController(IRepsitory<User> userRep,
            IRepsitory<Role> roleRep,
            IRepsitory<Entity.Path> menuRep,
            IRepsitory<UserInCustom> userCustomrep,
            IRepsitory<UserInArea> userInAreRep,
            IRepsitory<UserAsRole> userAsRoleRep,
            IRepsitory<t_bp_custom> t_bp_customRep,
            ICheckUnitService checkUnitService,
            ISysDictService sysDictService,
            IUserService userService,
            IUserCustomize userCustomizeService,
            IRepsitory<UserInRole> userInRoleRep,
            IFileHandler fileHander,
            IPkpmConfigService pkpmConfigService,
            IRoleService roleService) : base(userService)
        {
            this.userRep = userRep;
            this.roleRep = roleRep;
            this.menuRep = menuRep;
            this.userCustomrep = userCustomrep;
            this.userInAreRep = userInAreRep;
            this.checkUnitService = checkUnitService;
            this.sysDictService = sysDictService;
            this.roleService = roleService;
            this.userCustomizeService = userCustomizeService;
            this.userInRoleRep = userInRoleRep;
            this.fileHander = fileHander;
            this.t_bp_customRep = t_bp_customRep;
            this.userAsRoleRep = userAsRoleRep;
            this.pkpmConfigService = pkpmConfigService;
        }

        // GET: SysUser
        public ActionResult Index()
        {
            SysViewModels viewModel = new SysViewModels();
            viewModel.CheckInsts = new Dictionary<string, string>();// checkUnitService.GetAllCheckUnit();
            return View(viewModel);
        }

        // GET: SysUser/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SysUser/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Roles(int id)
        {
            var userRole = userService.GetUserRole(id);
            var allRoles = roleRep.GetByCondition(r => r.Id > 0);

            DhtmlxForm dForm = new DhtmlxForm();
            dForm.AddDhtmlxFormItem(new DhtmlxFormLabel("角色"));
            dForm.AddDhtmlxFormItem(new DhtmlxFormSettings().AddStringItem("offsetLeft", "50"));
            foreach (var role in allRoles)
            {
                bool isChecked = false;
                if (userRole != null && userRole.Id == role.Id)
                {
                    isChecked = true;
                }
                if (isChecked)
                {
                    dForm.AddDhtmlxFormItem(new DhtmlxFormRadio("Role", role.Id.ToString(), role.Name, true));
                }
                else
                {


                    dForm.AddDhtmlxFormItem(new DhtmlxFormRadio("Role", role.Id.ToString(), role.Name, false));
                }
            }
            dForm.AddDhtmlxFormItem(new DhtmlxFormButton("Save", "确定"));

            string str = dForm.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");
        }

        [HttpPost]
        public ActionResult AddUserIntoRole(int id, int roleId)
        {
            ControllerResult result = ControllerResult.SuccResult;


            string errorMsg = string.Empty;
            bool addResult = roleService.AddUserIntoRole(id, roleId, out errorMsg);
            if (!addResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            else
            {
                LogUserAction("对用户ID为{0}进行了角色设置操作，角色为{1}".Fmt(id, roleId));
            }
            return Content(result.ToJson());
        }

        public ActionResult Areas(int id)
        {
            var allArea = sysDictService.GetDictsByKey("CustomArea");
            var userAreas = userService.GetUserAreas(id);

            DhtmlxTreeView treeView = new DhtmlxTreeView();
            DhtmlxTreeViewItem item = new DhtmlxTreeViewItem("Main", "地区", true, false, DhtmlxTreeViewCheckbox.hidden);
            foreach (var area in allArea)
            {
                bool isCheck = false;
                if (userAreas.Count > 0 && userAreas.Count(r => r.Area == area.Name) > 0)
                {
                    isCheck = true;
                }
                DhtmlxTreeViewItem subTreeItem = new DhtmlxTreeViewItem("{0}|{1}".Fmt(area.Name, area.Id), area.Name, false, isCheck, DhtmlxTreeViewCheckbox.NotSet);
                item.AddTreeViewItem(subTreeItem);
            }
            treeView.AddTreeViewItem(item);

            string str = treeView.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");

        }

        [HttpPost]
        public ActionResult Areas(int id, string areaIds)
        {
            ControllerResult result = ControllerResult.SuccResult;
            List<string> areas = areaIds.Split(',').Select(s => s.Split('|').First()).ToList();

            string errorMsg = string.Empty;
            bool addResult = userService.AddAreasIntoUser(id, areas, out errorMsg);
            if (!addResult)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = errorMsg;
            }
            return Content(result.ToJson());
        }

        // POST: SysUser/Create
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Create(SysInsertViewModel viewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                //判断账号是否已注册
                var ExistsUser = userRep.GetByCondition(r => r.UserName == viewModel.UserName);
                if (ExistsUser != null && ExistsUser.Count > 0)
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = "新建会员失败，账号已经存在！";
                    return Content(result.ToJson());
                }
                Entity.User user = new Entity.User()
                {
                    CustomId = viewModel.InstId,
                    UserDisplayName = viewModel.DisplayName,
                    UserName = viewModel.UserName,
                    Id = viewModel.UserId,
                    Sex = viewModel.UserSex,
                    PhoneNumber = viewModel.Phone,
                    Email = viewModel.Email,
                    Status = viewModel.Status,
                    Grade = viewModel.Grade,
                    CheckStatus = viewModel.RoleId.Value == 2 ? "1" : "1",
                    UserCode = viewModel.UserCode,
                    Valie = viewModel.Valie,
                    CreateTime =DateTime.Now
                    //TODO 是否需要将后台新增的所有用户的状态都设置为已审核

                };

                var createResult = await UserManager.CreateAsync(user, viewModel.UserPwd);
                var CurUser = userRep.GetByCondition(r => r.UserName == viewModel.UserName);
                if (CurUser != null && CurUser.Count > 0)
                {
                    UserInRole uir = new UserInRole
                    {
                        UserId = CurUser[0].Id,
                        RoleId = viewModel.RoleId.Value
                    };
                    userInRoleRep.Insert(uir);
                }
                else
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = "新建会员失败";
                }

                if (!createResult.Succeeded)
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = "新建会员失败," + createResult.Errors.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetCreate(SysInsertViewModel viewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                //判断账号是否已注册
                var ExistsUser = userRep.GetByCondition(r => r.UserName == viewModel.UserName);
                if (ExistsUser != null && ExistsUser.Count > 0)
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = "新建会员失败，账号已经存在！";
                    return Content("successCallback(" + result.ToJson() + ")");
                }
                Entity.User user = new Entity.User()
                {
                    CustomId = viewModel.InstId,
                    UserDisplayName = viewModel.DisplayName,
                    UserName = viewModel.UserName,
                    Id = viewModel.UserId,
                    Sex = viewModel.UserSex,
                    PhoneNumber = viewModel.Phone,
                    Email = viewModel.Email,
                    Status = viewModel.Status,
                    Grade = viewModel.Grade,
                    CheckStatus = viewModel.RoleId.Value == 2 ? "1" : "0",
                    Valie = viewModel.Valie,
                    CreateTime = DateTime.Now
                };

                var createResult = await UserManager.CreateAsync(user, viewModel.UserPwd);
                var CurUser = userRep.GetByCondition(r => r.UserName == viewModel.UserName);
                if (CurUser != null && CurUser.Count > 0)
                {
                    UserInRole uir = new UserInRole
                    {
                        UserId = CurUser[0].Id,
                        RoleId = viewModel.RoleId.Value
                    };
                    userInRoleRep.Insert(uir);
                }
                else
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = "新建会员失败";
                }

                if (!createResult.Succeeded)
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = "新建会员失败";
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content("successCallback(" + result.ToJson() + ")");
        }

        public ActionResult Search(SysViewSearchModels searchModel)
        {
            var predicate = PredicateBuilder.True<UserAsRole>();
            var predicateRole = PredicateBuilder.True<UserInRole>();
            if (!string.IsNullOrWhiteSpace(searchModel.CheckUnitName))
            {
                predicate = predicate.And(t => t.CustomId == searchModel.CheckUnitName);
            }
            if (!string.IsNullOrWhiteSpace(searchModel.UserDisplayName))
            {
                predicate = predicate.And(t => t.UserDisplayName.Contains(searchModel.UserDisplayName));
            }
            if (!string.IsNullOrWhiteSpace(searchModel.UserName))
            {
                predicate = predicate.And(t => t.UserName == searchModel.UserName);
            }
            if (!string.IsNullOrWhiteSpace(searchModel.CheckStatus))
            {
                predicate = predicate.And(t => t.CheckStatus == searchModel.CheckStatus);
            }
            if (!string.IsNullOrEmpty(searchModel.RoleNames))
            {
                var allRoles = roleRep.GetByCondition(r => searchModel.RoleNames.Split(',').ToList().Contains(r.Name)).Select(s => s.Id).ToList();
                predicate = predicate.And(t => allRoles.Contains(t.RoleId));
            }
            else
            {

                var allRoles = roleRep.GetByCondition(t => t.Code != "QYY" && t.Code != "JYY").Select(s => s.Id).ToList();
                predicate = predicate.And(t => allRoles.Contains(t.RoleId));
            }
            //if(string.IsNullOrWhiteSpace(searchModel.Valie))
            //{
            //    predicate = predicate.And(t => t.Valie == searchModel.Valie);
            //}
            int pos = searchModel.posStart.HasValue ? searchModel.posStart.Value : 0;
            int count = searchModel.count.HasValue ? searchModel.count.Value : 30;


            PagingOptions<UserAsRole> pagingOption = new PagingOptions<UserAsRole>(pos, count, u => new { u.Id });
            var users = userAsRoleRep.GetByConditonPage(predicate, pagingOption);
            var userTypes = sysDictService.GetDictsByKey("UserType");
            var userStatus = sysDictService.GetDictsByKey("UserStatus");
            var allInsts = checkUnitService.GetAllCheckUnit();
            var userValie = sysDictService.GetDictsByKey("Valid");
            var allRole = roleRep.GetByCondition(u => true);

            DhtmlxGrid grid = new DhtmlxGrid();
            grid.AddPaging(pagingOption.TotalItems, pos);
            for (int i = 0; i < users.Count; i++)
            {
                var oneUser = users[i];
                DhtmlxGridRow row = new DhtmlxGridRow(oneUser.Id);
                row.AddCell((pos + i + 1).ToString());
                row.AddCell(oneUser.UserDisplayName);
                if (oneUser.RoleId == 3 || oneUser.RoleId == 5)
                {
                    row.AddCell(oneUser.UnitName);
                }
                else if (string.IsNullOrWhiteSpace(oneUser.CustomId) || !allInsts.ContainsKey(oneUser.CustomId.ToUpper()))
                {
                    row.AddCell("系统用户");
                }
                else
                {
                    row.AddCell(allInsts[oneUser.CustomId]);
                }

                row.AddCell(oneUser.UserName);
                row.AddCell(SysDictUtility.GetKeyFromDic(userValie, oneUser.Valie));
                row.AddCell(SysDictUtility.GetKeyFromDic(userStatus, oneUser.Status, "禁止"));
                var a = allRole.Where(u => u.Id == oneUser.RoleId);
                row.AddCell(allRole.Where(u => u.Id == oneUser.RoleId).Select(u => u.Name).FirstOrDefault().ToString());
                //row.AddCell(oneUser.CheckStatus == "1" ? "已审核" : "未审核");

                row.AddLinkJsCell("自定义模块", "personModule({0})".Fmt(oneUser.Id));
                row.AddLinkJsCell("自定义按钮", "personAction({0})".Fmt(oneUser.Id));
                if (superVisorRoleCode.Contains(userService.GetUserRole(oneUser.Id).Code))
                {
                    row.AddLinkJsCell("自定义机构", "personInst({0},\"{1}\")".Fmt(oneUser.Id, oneUser.UserDisplayName));
                }
                else
                {
                    row.AddCell(string.Empty);
                }
                row.AddLinkJsCell("密码重置", "PaswordRest({0})".Fmt(oneUser.Id));
                row.AddLinkJsCells(GetOpDicts(oneUser));
                grid.AddGridRow(row);
            }
            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);

            return Content(str, "text/xml");


        }

        private string GetUserType(List<SysDict> sysDicts, string key)
        {
            SysDict dict = sysDicts.Where(s => s.KeyValue == key).FirstOrDefault();
            if (dict == null)
            {
                return string.Empty;
            }
            else
            {
                return dict.Name;
            }
        }

        private Dictionary<string, string> GetOpDicts(UserAsRole user)
        {
            string id = user.Id.ToString();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            if (pkpmConfigService.QrRoleCode.Contains(userService.GetUserRole(int.Parse(id)).Code))
            {
                if (user.CheckStatus == null || user.CheckStatus == "0" || user.CheckStatus == "")
                {
                    dict.Add("[审核]", "CheckStatus({0})".Fmt(id));
                }
            }
            else
            {
                dict.Add(string.Empty, string.Empty);
            }
            dict.Add("[角色]", "roleUser({0})".Fmt(id));
            dict.Add("[修改]", "modifyUser({0})".Fmt(id));
            dict.Add("[删除]", "deleteUser({0})".Fmt(id));
            if (user.RoleId == 4)
            {
                dict.Add("[地区]", "areaUser({0})".Fmt(id));
            }
            return dict;
        }

        public  FileResult Image(ImageViewDownload model)
        {

            string fileName = string.Empty;
            //model.itemValue;
            if (model == null || model.itemValue == null || model.itemValue == "" || model.itemValue == "undefined")
            {
                fileName = string.Empty;
            }
            else
            {
                try
                {
                    fileName = SymCryptoUtility.Decrypt(model.itemValue);
                }
                catch (Exception)
                {

                    fileName = model.itemValue;
                }
            }


            try//照片路径不存在报错（新老数据问题）
            {
                string mimeType = GetMimeMapping(fileName);
                Stream stream =  fileHander.LoadFile(string.Empty, fileName);
                return new FileStreamResult(stream, mimeType);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetMimeMapping(string fileName)
        {
            if (fileName.IsNullOrEmpty())
            {
                fileName = "nopic.png";
            }

            return MimeMapping.GetMimeMapping(fileName);
        }

        public ActionResult StatisUserCustomTree(int id)
        {
            var userCustoms = userCustomrep.GetByCondition(r => r.UserId == id && r.UserCustomType == UserCustomType.UserLogCustom);
            XElement rootElem = BuildInstAreaTree(checkUnitService, userCustoms.Select(uc => uc.CustomId).ToList());

            string str = rootElem.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult StatisUserCustomGrid(int id)
        {
            DhtmlxGrid grid = new DhtmlxGrid();
            var userCustoms = userCustomrep.GetByCondition(r => r.UserId == id && r.UserCustomType == UserCustomType.UserLogCustom);
            var allInsts = checkUnitService.GetAllCheckUnit();
            foreach (var userCustom in userCustoms)
            {
                DhtmlxGridRow row = new DhtmlxGridRow(userCustom.CustomId);
                row.AddCell(checkUnitService.GetCheckUnitByIdFromAll(allInsts, userCustom.CustomId));
                grid.AddGridRow(row);
            }

            string str = grid.BuildRowXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        [HttpPost]
        public ActionResult AddUserStatisCustom(int id, string customIds)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                var userInCustoms = customIds.Split(',').Select(c => new UserInCustom()
                {
                    UserId = id,
                    CustomId = c,
                    UserCustomType = UserCustomType.UserLogCustom
                }).Where(uc => !uc.CustomId.IsNullOrEmpty()).ToList();

                string errorMsg = string.Empty;
                var addResult = userCustomizeService.SetUserCustom(id, userInCustoms, UserCustomType.UserLogCustom, out errorMsg);
                if (!addResult)
                {
                    result.IsSucc = addResult;
                    result.ErroMsg = errorMsg;
                }
                else
                {
                    LogUserAction("对会员管理进行了自定义机构设置操作");
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }


        // GET: SysUser/Edit/5
        public ActionResult Edit(int id)
        {
            var user = userRep.GetById(id);
            var userRole = userInRoleRep.GetByCondition(r => r.UserId == id);
            user.UserRoles = userRole;
            string str = user.ToJson();
            return Content(str);
        }

        public ActionResult CheckUser(int id)
        {
            var user = userRep.GetById(id);
            var userRole = userInRoleRep.GetByCondition(r => r.UserId == id);
            user.UserRoles = userRole;
            var custom = t_bp_customRep.GetByCondition(u => u.ID == user.CustomId).Select(u => u.NAME).FirstOrDefault();
            if (user.Email == null || user.Email == "")
            {
                user.Email = "空";
            }
            if (custom != null && custom != "")
            {
                user.CustomId = custom.ToString();
            }
            else
            {
                user.CustomId = "系统用户";
            }
            var usertype = sysDictService.GetDictsByKey("UserType").Where(u => u.KeyValue == user.Grade).Select(u => u.Name).FirstOrDefault();
            if (usertype != null && usertype != "")
            {
                user.Grade = usertype.ToString();
            }
            else
            {
                user.Grade = "检测中心用户";
            }
            string str = user.ToJson();
            return Content(str);
        }



        public ActionResult AllRoles()
        {
            var allRoles = roleRep.GetByCondition(r => true);
            string str = allRoles.ToJson();
            return Content(str);
        }

        public ActionResult RoleCombo()
        {
            XElement element = BuildRoleCombo();
            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        protected XElement BuildRoleCombo()
        {
            var allRoles = roleRep.GetByCondition(r => true);

            XElement element = new XElement("complete",
                from a in allRoles
                select new XElement("option",
                    new XAttribute("value", a.Id),
                    new XText(a.Name)));

            return element;
        }

        public ActionResult RoleComboName()
        {
            XElement element = BuildRoleComboName();
            string str = element.ToString(SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        protected XElement BuildRoleComboName()
        {
            var allRoles = roleRep.GetByCondition(r => true);
            allRoles = allRoles.Where(t => t.Code != "QYY" && t.Code != "JYY").ToList();
            XElement element = new XElement("complete",
                from a in allRoles
                select new XElement("option",
                new XAttribute("value", a.Name),
                new XText(a.Name)));

            return element;
        }

        public ActionResult RoleMenu()
        {
            var allRoles = roleRep.GetByCondition(r => true);
            DhtmlxMenu menu = new DhtmlxMenu();
            foreach (Role role in allRoles)
            {
                menu.AddItem(new DhtmlxMenuContainerItem(role.Id.ToString() + "|" + role.Code, role.Name));
                menu.AddItem(new DhtmlxMenuSeparatorItem("s4"));
            }

            string str = menu.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public async Task<ActionResult> ChangePwd()
        {
            ViewBag.Code = await UserManager.GeneratePasswordResetTokenAsync(GetCurrentUserId());
            ViewBag.UserId = GetCurrentUserId();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ChangedPwd(SysChangedPwdViewModel viewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {

                if (viewModel.ResetPwd.IsNullOrEmpty()
                    || viewModel.PwdConfirm.IsNullOrEmpty())
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = "修改的密码为空";
                }
                else
                {
                    if (viewModel.ResetPwd.Trim() != viewModel.PwdConfirm.Trim())
                    {
                        result = ControllerResult.FailResult;
                        result.ErroMsg = "两次密码输入不一致";
                    }
                }

                if (result.IsSucc)
                {
                    var resetResult = await UserManager.ResetPasswordAsync(viewModel.UserId, viewModel.UserCode, viewModel.ResetPwd);
                    if (!resetResult.Succeeded)
                    {
                        result = ControllerResult.FailResult;
                        result.ErroMsg = "密码修改失败";
                    }
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
        public async Task<ActionResult> ResetPwd(SysResetPwdViewModel viewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                string code = await UserManager.GeneratePasswordResetTokenAsync(viewModel.UserId);
                var resetResult = await UserManager.ResetPasswordAsync(viewModel.UserId, code, viewModel.ResetPwd);
                if (!resetResult.Succeeded)
                {
                    result = ControllerResult.FailResult;
                    result.ErroMsg = "重置修改失败";
                    result.ErroMsg = resetResult.Errors.Join(",");
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        // POST: SysUser/Edit/5
        [HttpPost]
        public ActionResult Edit(SysEditViewModel viewModel)
        {
            ControllerResult result = ControllerResult.SuccResult;
            //判断账号是否已注册
            //var ExistsUser = userRep.GetByCondition(r => r.UserName == viewModel.UserName);
            //if (ExistsUser != null && ExistsUser.Count > 0)
            //{
            //    result = ControllerResult.FailResult;
            //    result.ErroMsg = "修改会员信息失败，账号已经存在！";
            //    return Content(result.ToJson());
            //}
            Entity.User user = new Entity.User()
            {
                CustomId = viewModel.InstId,
                UserDisplayName = viewModel.DisplayName,
                UserName = viewModel.UserName,
                Id = viewModel.UserId,
                Sex = viewModel.UserSex,
                Email = viewModel.Email,
                Status = viewModel.Status,
                Mobile = viewModel.Phone,
                Grade = viewModel.Grade,
                UserCode = viewModel.UserCode,
                Valie = viewModel.Valie,
            };

            int? roleId = viewModel.RoleId;

            string errorMsg = string.Empty;
            var editResult = userService.EditUser(user, roleId, out errorMsg);

            if (!editResult)
            {
                result.IsSucc = false;
                result.ErroMsg = errorMsg;
            }

            return Content(result.ToJson());
        }

        // GET: SysUser/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SysUser/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            ControllerResult result = ControllerResult.SuccResult;
            string errorMsg = string.Empty;
            var delResult = userService.DeleteUser(id, out errorMsg);
            if (!delResult)
            {
                result.IsSucc = false;
                result.ErroMsg = errorMsg;
            }

            return Content(result.ToJson());
        }

        public ActionResult MenuAction(int id)
        {
            List<Entity.Path> userPaths = userService.GetPersonaliztionPaths(id);
            List<Entity.Action> pathActons = roleService.GetActionsByPathIds(userPaths.Select(rp => rp.Id).ToList());
            List<Entity.Action> roleActions = roleService.GetActionsByRole(id);

            DhtmlxTreeView treeView = new DhtmlxTreeView();
            foreach (var item in userPaths.Where(m => !m.IsCategory))
            {
                DhtmlxTreeViewItem categoryItem = new DhtmlxTreeViewItem("Path{0}".Fmt(item.Id), item.Name, false, false, DhtmlxTreeViewCheckbox.hidden);
                bool hasAction = false;
                foreach (var subItem in pathActons.Where(ra => ra.PathId == item.Id))
                {
                    bool hasPath = roleActions.Exists(r => r.Id == subItem.Id);

                    DhtmlxTreeViewItem subTreeItem = new DhtmlxTreeViewItem(subItem.Id.ToString(), subItem.Name, false, hasPath, DhtmlxTreeViewCheckbox.NotSet);
                    categoryItem.AddTreeViewItem(subTreeItem);
                    hasAction = true;
                }
                if (hasAction)
                {
                    treeView.AddTreeViewItem(categoryItem);
                }
            }

            string str = treeView.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        public ActionResult Menu(int id)
        {
            List<Entity.Path> rolePaths = userService.GetPersonaliztionPaths(id);

            var menus = menuRep.GetByCondition(m => m.Status == 1);
            DhtmlxTreeView treeView = new DhtmlxTreeView();
            foreach (var item in menus.Where(m => m.IsCategory))
            {
                DhtmlxTreeViewItem categoryItem = new DhtmlxTreeViewItem(item.Id.ToString(), item.Name, false, false, DhtmlxTreeViewCheckbox.hidden);
                foreach (var subItem in menus.Where(su => su.CategoryId == item.Id))
                {
                    bool hasPath = rolePaths.Exists(r => r.Id == subItem.Id);

                    DhtmlxTreeViewItem subTreeItem = new DhtmlxTreeViewItem(subItem.Id.ToString(), subItem.Name, false, hasPath, DhtmlxTreeViewCheckbox.NotSet);
                    categoryItem.AddTreeViewItem(subTreeItem);
                }
                treeView.AddTreeViewItem(categoryItem);
            }

            string str = treeView.BuildDhtmlXml().ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
            return Content(str, "text/xml");
        }

        [HttpPost]
        public ActionResult AddMenu(int id, string menuIds)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                // TODO: Add update logic here
                List<int> pathIds = new List<int>();
                foreach (var item in menuIds.Split(','))
                {
                    pathIds.Add(int.Parse(item));
                }
                string errorMsg = string.Empty;
                var addResult = userService.AddPathsIntoUser(id, pathIds, out errorMsg);
                if (!addResult)
                {
                    result.IsSucc = addResult;
                    result.ErroMsg = errorMsg;
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
        public ActionResult AddMenuAction(int id, string actionIds)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {

                List<int> actions = new List<int>();
                foreach (var item in actionIds.Split(','))
                {
                    actions.Add(int.Parse(item));
                }
                string errorMsg = string.Empty;
                var addResult = userService.AddActionsIntoUser(id, actions, out errorMsg);
                if (!addResult)
                {
                    result.IsSucc = addResult;
                    result.ErroMsg = errorMsg;
                }
            }
            catch (Exception ex)
            {
                result = ControllerResult.FailResult;
                result.ErroMsg = ex.Message;
            }

            return Content(result.ToJson());
        }

        public ActionResult UpdateCheckStatus(int id)
        {
            ControllerResult result = ControllerResult.SuccResult;
            try
            {
                var user = userRep.GetById(id);
                if (user.CheckStatus == null || user.CheckStatus != "1")
                {
                    user.CheckStatus = "1";
                    userRep.UpdateOnly(user, u => u.Id == id, f => new { f.CheckStatus });
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
