using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using Pkpm.Framework.Cache;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using Pkpm.Framework.PkpmConfigService;
using Pkpm.Core.CheckUnitCore;
using Pkpm.Core.STCustomCore;
using Pkpm.Core.AreaCore;
using Pkpm.Core.SysDictCore;

namespace Pkpm.Core.UserRoleCore
{
    public class UserService : IUserService
    {

        ICache<List<t_sys_region>> areaCache;
        ICache<InstShortInfos> customSTCache;
        IRepsitory<t_sys_region> areaRep;
        IRepsitory<InstShortInfos> customSTRep;
        IRepsitory<Role> roleRep;
        IRepsitory<TestType_BM> qualificationRep;
        IRepsitory<Path> pathRep;
        IRepsitory<Entity.Action> actionRep;
        IRepsitory<SysLog> sysLogRep;
        IRepsitory<SysDict> dictRep;
        IRepsitory<UserInArea> userInAreaRep;
        ICache<List<Entity.Path>> cachePaths;
        ICache<List<Entity.Action>> cacheActions;
        ICache<Role> cacheRole;
        ICache<List<UserInArea>> cacheUserInArea;
        ICache<InstShortInfos> cacheInsts;
        ICache<List<TestType_BM>> cacheQualification;
        ICache<List<UIArea>> cacheAreas;
        ICache<UserInspect> cacheUserInspect;
        IRoleService roleService;
        IRepsitory<User> _repUser;
        IRepsitory<UserAsRole> userAsRoleRep;
        IAreaService areaService;
        IRepsitory<UserInCustom> userCustomrep;
        IRepsitory<WxUser> m_repWxUser;
        IRepsitory<t_bp_custom> _custom;
        ICheckUnitService checkUnitService;
        ISTCustomService stCustomService;
        IDbConnectionFactory dbFactory;
        IPkpmConfigService pkpmConfigService;
        ISysDictService sysDictService;
        IRepsitory<t_bp_custom> rep;

        public UserService(IRepsitory<Role> roleRep,
            IRepsitory<Path> pathRep,
             IRepsitory<TestType_BM> qualificationRep,
            IRepsitory<Entity.Action> actionRep,
            IRepsitory<SysLog> sysLogRep,
             ICache<List<TestType_BM>> cacheQualification,
            IRepsitory<SysDict> dictRep,
            IRepsitory<UserInArea> userInAreaRep,
            ICache<List<Entity.Path>> cachePaths,
            ICache<List<Entity.Action>> cacheActions,
            ICache<Role> cacheRole,
            ICache<List<UserInArea>> cacheUserInArea,
            ICache<InstShortInfos> cacheInsts,
            ICache<List<UIArea>> cacheAreas,
             ICache<UserInspect> cacheUserInspect,
             ISysDictService sysDictService,
            IRoleService roleService,
            IAreaService areaService,
            IRepsitory<User> repUser,
            IRepsitory<UserAsRole> userAsRoleRep,
            IRepsitory<UserInCustom> userCustomrep,
            IRepsitory<WxUser> repWxUser,
            IRepsitory<t_bp_custom> custom,
            ICheckUnitService checkUnitService,
            ICache<List<t_sys_region>> areaCache,
            IRepsitory<t_sys_region> areaRep,
            ICache<InstShortInfos> customSTCache,
            IRepsitory<InstShortInfos> customSTRep,
            IDbConnectionFactory dbFactory,
            ISTCustomService stCustomService,
            IRepsitory<t_bp_custom> rep,
            IPkpmConfigService pkpmConfigService)
        {
            this.customSTCache = customSTCache;
            this.customSTRep = customSTRep;
            this.qualificationRep = qualificationRep;
            this.areaCache = areaCache;
            this.areaRep = areaRep;
            this.cacheQualification = cacheQualification;
            this.roleRep = roleRep;
            this.pathRep = pathRep;
            this.actionRep = actionRep;
            this.sysLogRep = sysLogRep;
            this.dictRep = dictRep;
            this.areaService = areaService;
            this.userInAreaRep = userInAreaRep;
            this.cachePaths = cachePaths;
            this.cacheActions = cacheActions;
            this.cacheRole = cacheRole;
            this.cacheUserInArea = cacheUserInArea;
            this.cacheInsts = cacheInsts;
            this.cacheAreas = cacheAreas;
            this.cacheUserInspect = cacheUserInspect;
            this.roleService = roleService;
            this._repUser = repUser;
            this.userAsRoleRep = userAsRoleRep;
            this.userCustomrep = userCustomrep;
            this.m_repWxUser = repWxUser;
            this._custom = custom;
            this.checkUnitService = checkUnitService;
            this.dbFactory = dbFactory;
            this.stCustomService = stCustomService;
            this.pkpmConfigService = pkpmConfigService;
            this.sysDictService = sysDictService;
            this.rep = rep;
        }

        //private  string defaultInst = pkpmConfigService.DefaultInst; //System.Configuration.ConfigurationManager.AppSettings["SysDefaultInst"];
        // private static string defaultInpsect = System.Configuration.ConfigurationManager.AppSettings["SysDefaultInspect"];

        public List<t_sys_region> GetAllArea()
        {
            var cacheKey = PkPmCacheKeys.AllArea;
            var allArea = areaCache.Get(cacheKey);
            if (allArea != null)
            {
                return allArea;
            }
            else
            {
                var areas = areaRep.GetByCondition(t => true);
                areaCache.Add(cacheKey, areas);
                return areas;
            }
        }

        public InstShortInfos GetUserInstsST(int userId)
        {

            var instSTs = cacheInsts.Get(PkPmCacheKeys.CustomsByUserIdSTFmt.Fmt(userId));
            if (instSTs != null)
            {
                return instSTs;
            }
            else
            {
                var user = _repUser.GetById(userId);
                //数据权限检测人员全部从 customid字段取，去掉usercode
                string customId = user.CustomId;

                if (customId.IsNullOrEmpty())
                {
                    customId = pkpmConfigService.DefaultInst;
                }


                var role = GetUserRole(userId);

                //管理员 监督 查看报告 见证取样 的默认为全部
                if (roleService.IsAdmin(role) || roleService.IsReport(role) || roleService.IsSuperVisor(role) || roleService.IsAllQYYAndJYY(role))
                {
                    instSTs = stCustomService.GetAllCustomST();
                }
                else
                {
                    instSTs = stCustomService.GetSubInstSTs(customId);
                }

                if (instSTs.Count == 0)
                {
                    instSTs = stCustomService.GetDefaultInstSTs();
                }

                cacheInsts.Put(PkPmCacheKeys.CustomsByUserIdSTFmt.Fmt(userId), instSTs);
                return instSTs;

            }
        }

        public List<TestType_BM> GetAllCompanyQualification()
        {
            var companyQualification = cacheQualification.Get(PkPmCacheKeys.CompanyQualification);
            if (companyQualification == null)
            {
                companyQualification = qualificationRep.GetByCondition(t => t.pid == 0 && t.FTestTypeName != "市政工程类" && t.FTestTypeName != "桥梁工程类" && t.FTestTypeName != "预拌商品混凝土");
                cacheQualification.Put(PkPmCacheKeys.CompanyQualification, companyQualification);
            }
            return companyQualification;

        }

        public Role GetUserRole(int userId)
        {
            string cacheKey = PkPmCacheKeys.RoleByUserIdFmt.Fmt(userId);
            var role = cacheRole.Get(cacheKey);
            if (role != null)
            {
                return role;
            }
            else
            {
                var roles = roleRep.GetMasterDetailConditionSort<UserInRole>(null, ur => ur.UserId == userId, new SortingOptions<Role>(u => new { u.Id }));
                if (roles != null && roles.Count > 0)
                {
                    cacheRole.Put(cacheKey, roles.First());
                    return roles.First();
                }
                else
                {
                    role = roleService.GetDefaultRole();
                    cacheRole.Put(cacheKey, role);
                    return role;
                }
            }
        }

        public List<User> GetSysExpertRoleUsers()
        {
            //string cacheKey = PkPmCacheKeys.SysExpertRoleUsers;
            //var role = cacheRole.Get(cacheKey);

            List<User> users = new List<User>();

            var role = roleRep.GetByCondition(r => r.Code == "ZJRY").FirstOrDefault();
            using (var db = dbFactory.Open())
            {
                var q = db.From<User>().Join<UserInRole>((u, ur) => u.Id == ur.UserId)
                    .Where<User, UserInRole>((u, ur) => ur.RoleId == role.Id)
                    .Select<User>(r => new { r.Id, r.CustomId, r.UserName, r.UserDisplayName, r.Sex, r.PhoneNumber, r.Email, r.photopath, r.UnitName });
                users = db.Select<User>(q.Limit(0, 10));
            }
            if (users == null || users.Count == 0)
            {
                return new List<User>();
            }

            return users;
        }


        public List<Entity.Path> GetUserPaths(int userId)
        {
            Role role = GetUserRole(userId);
            //获取角色的模块
            var paths = roleService.GetPathsByRole(role.Id);
            //获取用户自定义的模块
            var userPaths = GetPersonaliztionPaths(userId);
            //合并模块
            foreach (var userPath in userPaths)
            {
                if (!paths.Exists(p => p.Id == userPath.Id))
                {
                    paths.Add(userPath);
                }
            }
            var pathCategories = roleService.GetAllCategory();
            foreach (var item in pathCategories)
            {
                if (paths.Exists(p => p.CategoryId == item.Id))
                {
                    paths.Add(item);
                }
            }
            return paths;
        }

        public List<Entity.Action> GetUserActions(int userId, int pathId)
        {
            Role role = GetUserRole(userId);
            var roleActions = roleService.GetActionsByRolePathId(role.Id, pathId);
            var userActions = GetPersonaliztionActionsPathId(userId, pathId);

            foreach (var item in userActions)
            {
                if (roleActions.Exists(r => r.Id == item.Id))
                {
                    continue;
                }

                roleActions.Add(item);
            }

            return roleActions;
        }

        public List<Entity.Path> GetPersonaliztionPaths(int userId)
        {
            var cacheKey = PkPmCacheKeys.PathsByUserIdFmt.Fmt(userId);
            var paths = cachePaths.Get(cacheKey);
            if (paths != null)
            {
                return paths;
            }
            else
            {
                paths = pathRep.GetMasterDetailCondition<PathInUser>(null, up => up.UserId == userId);
                cachePaths.Put(cacheKey, paths);
                return paths;
            }
        }

        public List<Entity.Action> GetPersonaliztionActions(int userId)
        {

            var cacheKey = PkPmCacheKeys.ActionsByUserIdFmt.Fmt(userId);
            var actions = cacheActions.Get(cacheKey);
            if (actions != null)
            {
                return actions;
            }
            else
            {
                actions = actionRep.GetMasterDetailCondition<ActionInUser>(null, up => up.UserId == userId);
                cacheActions.Put(cacheKey, actions);
                return actions;
            }
        }

        public List<Entity.Action> GetPersonaliztionActionsPathId(int userId, int pathId)
        {
            var actions = actionRep.GetMasterDetailCondition<ActionInUser>(a => a.PathId == pathId, up => up.UserId == userId);
            return actions;
        }

        public string InstUserCustomId(int UserId)
        {
            string CustomId = string.Empty;
            if (IsInstUser(UserId))
            {
                var user = _repUser.GetById(UserId);
                CustomId = user.CustomId;
            }
            return CustomId;
        }

        public List<UIArea> GetAllAreas()
        {
            string cacheKey = PkPmCacheKeys.AllAreasFromSysDict;
            var areas = cacheAreas.Get(cacheKey);
            if (areas == null)
            {
                List<SysDict> result = new List<SysDict>();
                var dictCategory = dictRep.GetByCondition(r => r.KeyValue == "CustomArea" && r.CategoryId == -1 && r.Status == 1);
                if (dictCategory != null && dictCategory.Count > 0)
                {
                    result = dictRep.GetByConditionSort(r => r.CategoryId == dictCategory[0].Id, new SortingOptions<SysDict>(r => new { r.OrderNo }));
                }
                areas = result.Where(e => e.KeyValue != "500100" || !e.Name.Contains("重庆市")).Select(r => new UIArea() { AreaCode = r.KeyValue, AreaName = r.Name }).ToList();
                cacheAreas.Put(cacheKey, areas);
                return areas;
            }
            else
            {

                return areas;
            }
        }

        public List<UserInArea> GetUserAreas(int userId)
        {
            string cacheKey = PkPmCacheKeys.UserInAreByUserIdFmt.Fmt(userId);
            var areas = cacheUserInArea.Get(cacheKey);
            if (areas == null)
            {
                areas = userInAreaRep.GetByCondition(r => r.UserId == userId);
                cacheUserInArea.Put(cacheKey, areas);
                return areas;
            }
            else
            {
                return areas;
            }
        }

        public bool AddAreasIntoUser(int userId, List<string> areas, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                using (var db = dbFactory.Open())
                {
                    using (var dbTrans = db.OpenTransaction())
                    {
                        try
                        {
                            db.Delete<UserInArea>(ua => ua.UserId == userId);

                            foreach (var item in areas)
                            {
                                db.Insert(new UserInArea() { UserId = userId, Area = item });
                            }
                            dbTrans.Commit();
                            cacheUserInArea.Remove(PkPmCacheKeys.UserInAreByUserIdFmt.Fmt(userId));
                            return true;
                        }
                        catch (Exception ex)
                        {
                            errorMsg = ex.Message;
                            dbTrans.Rollback();
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool AddPathsIntoUser(int userId, List<int> pathIds, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                using (var db = dbFactory.Open())
                {
                    using (var dbTrans = db.OpenTransaction())
                    {
                        try
                        {
                            db.Delete<PathInUser>(ua => ua.UserId == userId);

                            foreach (var item in pathIds)
                            {
                                db.Insert(new PathInUser() { UserId = userId, PathId = item });
                            }
                            dbTrans.Commit();
                            cachePaths.Remove(PkPmCacheKeys.PathsByUserIdFmt.Fmt(userId));
                            return true;
                        }
                        catch (Exception ex)
                        {
                            errorMsg = ex.Message;
                            dbTrans.Rollback();
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool AddActionsIntoUser(int userId, List<int> actionIds, out string errorMsg)
        {
            errorMsg = string.Empty;
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        db.Delete<ActionInUser>(pr => pr.UserId == userId);

                        foreach (var item in actionIds)
                        {
                            db.Insert(new ActionInUser() { ActionId = item, UserId = userId });
                        }
                        dbTrans.Commit();
                        cacheActions.Remove(PkPmCacheKeys.ActionsByUserIdFmt.Fmt(userId));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        errorMsg = ex.Message;
                        dbTrans.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool EditUser(User user, int? roleId, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                using (var db = dbFactory.Open())
                {
                    using (var dbTrans = db.OpenTransaction())
                    {
                        try
                        {
                            errorMsg = string.Empty;

                            db.UpdateOnly(() => new User()
                            {
                                CustomId = user.CustomId,
                                UserDisplayName = user.UserDisplayName,
                                UserName = user.UserName,
                                Sex = user.Sex,
                                Mobile = user.Mobile,
                                Email = user.Email,
                                Status = user.Status,
                                Grade = user.Grade,
                                UserCode = user.UserCode,
                                Valie = user.Valie
                            }, u => u.Id == user.Id);
                            db.UpdateOnly(() => new UserInRole() { RoleId = roleId.Value }, ur => ur.UserId == user.Id);

                            dbTrans.Commit();
                            cacheInsts.Remove(PkPmCacheKeys.CustomsByUserIdFmt.Fmt(user.Id));
                            cacheRole.Remove(PkPmCacheKeys.RoleByUserIdFmt.Fmt(user.Id));
                            cacheUserInspect.Remove(PkPmCacheKeys.InspectByUserIdFmt.Fmt(user.Id));
                            return true;

                        }
                        catch (Exception ex)
                        {
                            dbTrans.Rollback();
                            errorMsg = ex.Message;
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public void AddUserEvent(Entity.SysLog sysLog)
        {
            try
            {
                sysLogRep.Insert(sysLog);
            }
            catch (Exception)
            {
                //igonre
            }
        }

        public bool DeleteUser(int userId, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                int deleteCount = _repUser.DeleteById(userId);
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public string GetUserDisplayName(int userId)
        {
            var data = _repUser.GetColumnByCondition<string>(r => r.Id == userId, r => new { r.UserDisplayName });
            if (data != null && data.Count > 0)
            {
                return data.First();
            }
            else
            {
                return string.Empty;
            }
        }

        public IEnumerable<User> GetUsers()
        {
            IList<User> users = _repUser.GetByCondition(r => true);
            return users;
        }

        public User GetUserByName(string name)
        {
            List<User> users = _repUser.GetByCondition(r => r.UserName == name);
            if (users.Count == 0)
            {
                return null;
            }
            return users[0];
        }

        public UserAsRole GetByWxOpenId(string openId)
        {
            List<WxUser> wxUsers = m_repWxUser.GetByCondition(r => r.OpenId == openId);
            if (wxUsers.Count == 0)
            {
                return null;
            }
            else
            {
                return userAsRoleRep.GetById(wxUsers[0].UserId);

            }
        }



        public InstShortInfos GetUserInsts(int userId, string canCheckAllCustomRoles)
        {
            var insts = cacheInsts.Get(PkPmCacheKeys.CustomsByUserIdFmt.Fmt(userId));
            if (insts != null)
            {
                return insts;
            }
            else
            {
                var user = _repUser.GetById(userId);
                //数据权限检测人员全部从 customid字段取，去掉usercode
                string customId = string.Empty;
                if (user != null)
                {
                    customId = user.CustomId;
                }

                if (customId.IsNullOrEmpty())
                {
                    customId = pkpmConfigService.DefaultInst;
                }


                Role role = GetUserRole(userId);

                //专家 受理人 承办人 管理员 监督 查看报告 见证取样 的默认为全部
                if (canCheckAllCustomRoles.Contains(role.Code) ||  roleService.IsAdmin(role) || roleService.IsReport(role) || roleService.IsSuperVisor(role) || roleService.IsAllQYYAndJYY(role))
                {
                    insts = checkUnitService.GetAllCheckUnit();
                }
                else
                {
                    insts = checkUnitService.GetSubInsts(customId);
                }

                if (insts.Count == 0)
                {
                    insts = checkUnitService.GetDefaultInsts();
                }

                cacheInsts.Put(PkPmCacheKeys.CustomsByUserIdFmt.Fmt(userId), insts);
                return insts;

            }
        }


        public t_bp_custom GetCustomInsts(int userId)
        {
            var user = _repUser.GetById(userId);
            //数据权限检测人员全部从 customid字段取，
            string customId = string.Empty;
            if (user != null)
            {
                customId = user.CustomId;
            }

            if (customId.IsNullOrEmpty())
            {
                customId = pkpmConfigService.DefaultInst;
            }


            return _custom.GetById(customId);
        }

        public InstShortInfos GetUserFormalInsts(int userId)
        {

            var user = _repUser.GetById(userId);
            //数据权限检测人员全部从 customid字段取，去掉usercode
            string customId = user.CustomId;

            if (customId.IsNullOrEmpty())
            {
                customId = pkpmConfigService.DefaultInst;// defaultInst;
            }


            var role = GetUserRole(userId);
            InstShortInfos insts = new InstShortInfos();
            //管理员
            if (roleService.IsAdmin(role) || roleService.IsReport(role))
            {
                insts = checkUnitService.GetAllFormalCheckUnit();
            }
            else if (roleService.IsSuperVisor(role)) //监督
            {

                var userArea = GetUserAreas(userId);

                //有地区先从地区获取数据
                if (userArea != null && userArea.Count > 0)
                {
                    insts = checkUnitService.GetUnitByArea(userArea);
                }
                else
                {
                    var userCustoms = userCustomrep.GetByCondition(r => r.UserId == userId && r.UserCustomType == UserCustomType.UserLogCustom);
                    //无地区从人员机构表中获取特定的机构
                    if (userCustoms != null && userCustoms.Count > 0)
                    {
                        var allInsts = checkUnitService.GetAllFormalCheckUnit();
                        Dictionary<string, string> dicUserCustoms = new Dictionary<string, string>();
                        foreach (var item in userCustoms)
                        {
                            string value = checkUnitService.GetCheckUnitByIdFromAll(allInsts, item.CustomId);
                            dicUserCustoms[item.CustomId] = value;
                        }

                        insts = InstShortInfos.FromDictonary(dicUserCustoms);
                    }
                    else
                    {
                        insts = checkUnitService.GetAllFormalCheckUnit();
                    }
                }
            }
            else
            {
                insts = checkUnitService.GetSubInsts(customId);
            }

            if (insts.Count == 0)
            {
                insts = checkUnitService.GetDefaultInsts();
            }

            cacheInsts.Put(PkPmCacheKeys.CustomsByUserIdFmt.Fmt(userId), insts);
            return insts;

        }


        public InstFilter GetFilterInsts(int userId, string canCheckAllCustomRoles)
        {
            var role = GetUserRole(userId);
            //管理员和见证取样 不按照机构过滤
            if (canCheckAllCustomRoles.Contains(role.Code) || roleService.IsAdmin(role) || roleService.IsQr(role) || roleService.IsReport(role) || roleService.IsSuperVisor(role))
            {
                return new InstFilter()
                {
                    NeedFilter = false,
                    FilterInstIds = new List<string>()
                };
            }
            else
            {
                var insts = GetUserInsts(userId, canCheckAllCustomRoles);
                InstFilter result = new InstFilter() { NeedFilter = true, FilterInstIds = new List<string>() };
                foreach (var item in insts)
                {
                    result.FilterInstIds.Add(item.Key);
                }

                return result;
            }
        }

        public InspectFilter GetFilterInspect(int userId)
        {
            var role = GetUserRole(userId);
            if (roleService.IsSuperVisor(role))
            {
                var inspectId = GetSuperVisorUserInspect(userId);
                return new InspectFilter
                {
                    NeedFilter = true,
                    UserInspect = inspectId
                };
            }
            else
            {
                return new InspectFilter
                {
                    NeedFilter = false,
                    UserInspect = new UserInspect()
                };
            }
        }

        private UserInspect GetSuperVisorUserInspect(int userId)
        {
            var userInspect = cacheUserInspect.Get(PkPmCacheKeys.InspectByUserIdFmt.Fmt(userId));
            if (userInspect != null)
            {
                return userInspect;
            }
            else
            {
                var user = _repUser.GetById(userId);
                //数据权限监督人员全部从 usercode 获取
                string inspectId = user.UserCode;

                if (inspectId.IsNullOrEmpty())
                {
                    inspectId = pkpmConfigService.DefaultInpsect;// defaultInpsect;
                }

                UserInspect newUserInspect = new UserInspect()
                {
                    InspectId = inspectId
                };

                cacheUserInspect.Put(PkPmCacheKeys.InspectByUserIdFmt.Fmt(userId), newUserInspect);
                return newUserInspect;
            }
        }

        public bool IsInspector(int userId)
        {
            var role = this.GetUserRole(userId);
            return roleService.IsSuperVisor(role);
        }

        public bool IsAdmin(int userId)
        {
            var role = this.GetUserRole(userId);
            return roleService.IsAdmin(role);
        }

        public bool IsAllQYYAndJYY(int userId)
        {
            var role = this.GetUserRole(userId);
            return roleService.IsAllQYYAndJYY(role);
        }

        public bool IsSample(int userId)
        {
            var role = this.GetUserRole(userId);
            return roleService.IsSample(role);
        }

        public bool IsWitnes(int userId)
        {
            var role = this.GetUserRole(userId);
            return roleService.IsWitnes(role);
        }

        public bool IsInstUser(int userId)
        {
            var role = this.GetUserRole(userId);
            return roleService.IsInstUser(role);
        }

        public bool IsCheckPeople(int userId)
        {
            var role = this.GetUserRole(userId);
            return roleService.IsCheckPeople(role);
        }

        public bool CurrentAccountIsCardNo(int userId)
        {
            var user = _repUser.GetById(userId);
            if (user == null)
            {
                return true;
            }
            if (user.UserName.Length > 7 && user.UserName.Length == 18)
            {
                return true;
            }
            return false;
        }

        public void BindOpenId(int userId, string openId, bool single = false)
        {
            List<WxUser> list;
            if (single)
                list = m_repWxUser.GetByCondition(r => r.UserId == userId);
            else
                list = m_repWxUser.GetByCondition(r => r.UserId == userId && r.OpenId == openId);

            WxUser user = new WxUser() { UserId = userId, OpenId = openId, NoMsg = false };
            if (list.Count == 0)
            {
                m_repWxUser.Insert(user);
            }
            else if (single)
            {
                m_repWxUser.DeleteByCondition(r => r.UserId == userId);
                m_repWxUser.Insert(user);
            }
        }

        public int ApiCreateUser(User user, int RoleId, out string ErrorMsg)
        {
            int UserId = 0;
            ErrorMsg = string.Empty;
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        UserId = (int)db.Insert(user, true);
                        if (UserId > 0)
                        {
                            UserInRole UserRole = new UserInRole()
                            {
                                UserId = UserId,
                                RoleId = RoleId
                            };
                            var UserRoleId = db.Insert(UserRole, true);
                            dbTrans.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        ErrorMsg = ex.Message;
                        UserId = 0;
                    }
                }
            }

            return UserId;
        }

        public List<User> ApiGetUsers(string DisplayName, int RoleId, int? Start, int? Size, out int dbCount)
        {
            Size = Start.HasValue ? Size : 0;
            Size = Start.HasValue ? Size : 30;
            using (var db = dbFactory.Open())
            {
                var q = db.From<User>()
                        .Join<UserInRole>((u, ur) => u.Id == ur.UserId)
                        .Where<User, UserInRole>((u, ur) => u.UserDisplayName.Contains(DisplayName) && ur.RoleId == RoleId)
                        .Select<User>(r => new { r.Id, r.CustomId, r.UserName, r.UserDisplayName, r.Sex, r.PhoneNumber, r.Email, r.photopath, r.UnitName });
                dbCount = (int)db.RowCount(q);
                var result = db.Select<User>(q.Limit(Start.Value, Size.Value));
                return result;
            }
        }

        public List<User> ApiGetUserByUserName(string name, List<int> roleId)
        {
            List<User> users = new List<User>(); // _repUser.GetByCondition(t => t.UserName.Contains(name));

            using (var db = dbFactory.Open())
            {
                var q = db.From<User>().Join<UserInRole>((u, ur) => u.Id == ur.UserId)
                    .Where<User, UserInRole>((u, ur) => u.UserName.Contains(name) && roleId.Contains(ur.RoleId))
                    .Select<User>(r => new { r.Id, r.CustomId, r.UserName, r.UserDisplayName, r.Sex, r.PhoneNumber, r.Email, r.photopath, r.UnitName });
                users = db.Select<User>(q.Limit(0, 10));
            }
            if (users == null || users.Count == 0)
            {
                return new List<User>();
            }
            return users;
        }

        public Dictionary<int, string> ApiGetUserNameByUserIds(List<int> userIds)
        {
            return _repUser.GetDictByCondition<int, string>(t => userIds.Contains(t.Id), t => new { t.Id, t.UserName });
        }

        public Dictionary<int, string> ApiGetUserNameByUserIds(List<string> userIds)
        {
            return _repUser.GetDictByCondition<int, string>(t => userIds.Contains(t.Id.ToString()), t => new { t.Id, t.UserDisplayName });
        }

        /// <summary>
        /// 根据传入的地区名字，判断是否有下级区域，如果有，则以名字，名字返回
        /// </summary>
        /// <param name="Area"></param>
        /// <returns></returns>
        public string GetAreasByArea(string Area)
        {
            return areaService.GetAreasByArea(Area);
        }

        public InstShortInfos GetDictByKey(string key)
        {
            var aaa = sysDictService.GetDictsByKey(key);
            var dict = aaa.ToDictionary(t => t.KeyValue, t => t.Name);
            return InstShortInfos.FromDictonary(dict);
        }

        public InstShortInfos GetUserEliminateIsUerInsts(int userId)
        {
            var insts = cacheInsts.Get(PkPmCacheKeys.CustomsByUserIdEliminateIsUerFmt.Fmt(userId));
            if (insts != null)
            {
                return insts;
            }
            else
            {
                var user = _repUser.GetById(userId);
                //数据权限检测人员全部从 customid字段取，去掉usercode
                string customId = user.CustomId;

                if (customId.IsNullOrEmpty())
                {
                    customId = pkpmConfigService.DefaultInst;
                }


                var role = GetUserRole(userId);

                //管理员 监督 查看报告 见证取样 的默认为全部
                if (roleService.IsAdmin(role) || roleService.IsReport(role) || roleService.IsSuperVisor(role) || roleService.IsAllQYYAndJYY(role))
                {
                    insts = checkUnitService.GetAllCheckUnitEliminateIsUer();
                }
                else
                {
                    insts = checkUnitService.GetSubInsts(customId);
                }

                if (insts.Count == 0)
                {
                    insts = checkUnitService.GetDefaultInsts();
                }

                cacheInsts.Put(PkPmCacheKeys.CustomsByUserIdFmt.Fmt(userId), insts);
                return insts;

            }
        }

        public InstShortInfos GetAllCheckUnitEliminateIsUer()
        {
            var data = cacheInsts.Get(PkPmCacheKeys.AllCheckUnitEliminateIsUser);

            if (data == null)
            {
                var dicData = rep.GetDictByCondition<string, string>(r => r.ID != null && r.data_status != "-1" && r.IsUse != 0, r => new { r.ID, r.NAME });
                data = InstShortInfos.FromDictonary(dicData);
                cacheInsts.Put(PkPmCacheKeys.AllCheckUnitEliminateIsUser, data);

            }
            return data;
        }
    }
}
