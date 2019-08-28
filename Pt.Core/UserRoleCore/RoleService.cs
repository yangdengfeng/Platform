using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pkpm.Entity;
using Pkpm.Framework.Repsitory;
using Pkpm.Framework.Cache;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack;
using Pkpm.Framework.PkpmConfigService;

namespace Pkpm.Core.UserRoleCore
{
    public class RoleService : IRoleService
    {
        IRepsitory<UserInRole> userInRoleRep; 
        IRepsitory<PathInRole> pathInRoleRep;
        IRepsitory<ActionInRole> actionInRoleRep;
        IRepsitory<Role> roleRep;
        IRepsitory<Path> pathRep;
        IRepsitory<Entity.Action> actionRep;
        ICache<InstShortInfos> cacheInsts;
        ICache<Role> cacheRole;
        ICache<List<Path>> cachePaths;
        ICache<List<Entity.Action>> cacheActions;
        IDbConnectionFactory dbFactory;
        IPkpmConfigService pkpmConfigService;

     
        public RoleService(IRepsitory<UserInRole> userInRoleRep, 
           IRepsitory<PathInRole> pathInRoleRep,
           IRepsitory<ActionInRole> actionInRoleRep,
           IRepsitory<Role> roleRep,
           IRepsitory<Path> pathRep,
           IRepsitory<Entity.Action> actionRep,
           ICache<Role> cacheRole,
           ICache<InstShortInfos> cacheInsts,
           ICache<List<Path>> cachePaths,
           ICache<List<Entity.Action>> cacheActions,
           IDbConnectionFactory dbFactory,
           IPkpmConfigService pkpmConfigService)
        {
            this.userInRoleRep = userInRoleRep; 
            this.pathInRoleRep = pathInRoleRep;
            this.actionInRoleRep = actionInRoleRep;
            this.roleRep = roleRep;
            this.pathRep = pathRep;
            this.actionRep = actionRep;
            this.cacheInsts = cacheInsts;
            this.cacheRole = cacheRole;
            this.cachePaths = cachePaths;
            this.cacheActions = cacheActions;
            this.dbFactory = dbFactory;
            this.pkpmConfigService = pkpmConfigService;
        }

        public bool AddNewRole(Entity.Role role, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                roleRep.Insert(role);
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool EditRole(Entity.Role role, out string errorMsg)
        {
            errorMsg = string.Empty;

            try
            {
                int updateCount = roleRep.UpdateOnly(role, r => r.Id == role.Id, r => new { role.Name, role.Code, role.Description });
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool DeleteRole(int roleId, out string errormsg)
        {
            errormsg = string.Empty;
            try
            {
                int deleteCount = roleRep.DeleteById(roleId);
                return true;
            }
            catch (Exception ex)
            {
                errormsg = ex.Message;
                return false;
            }
        }


        #region Action
        public bool AddActionIntoRole(int actionId, int roleId, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                if (actionInRoleRep.GetCountByCondtion(ar => ar.ActionId == actionId && ar.RoleId == roleId) == 0)
                {
                    actionInRoleRep.Insert(new ActionInRole() { ActionId = actionId, RoleId = roleId });
                }
                cacheActions.Remove(PkPmCacheKeys.ActionsByRoleIdFmt.Fmt(roleId));
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool AddActionsIntoRole(List<int> actionIds, int roleId, out string errorMsg)
        {
            errorMsg = string.Empty;
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        db.Delete<ActionInRole>(pr => pr.RoleId == roleId);

                        foreach (var item in actionIds)
                        {
                            db.Insert(new ActionInRole() { ActionId = item, RoleId = roleId });
                        }
                        dbTrans.Commit();
                        cacheActions.Remove(PkPmCacheKeys.ActionsByRoleIdFmt.Fmt(roleId));
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

        public List<Entity.Action> GetActionsByPathId(int pathId)
        {
            var actions = actionRep.GetByCondition(a => a.PathId == pathId);
            return actions;
        }

        public List<Entity.Action> GetActionsByPathIds(List<int> pathIds)
        {
            var actions = actionRep.GetByCondition(a => pathIds.Contains(a.PathId));
            return actions;
        }

        public List<Entity.Action> GetActionsByRole(int roleId)
        {
            string cacheKey = PkPmCacheKeys.ActionsByRoleIdFmt.Fmt(roleId);
            var actions = cacheActions.Get(cacheKey);
            if (actions != null)
            {
                return actions;
            }
            else
            {
                actions = actionRep.GetMasterDetailConditionSort<ActionInRole>(null, ar => ar.RoleId == roleId, new SortingOptions<Entity.Action>(a => new { a.Id }));
                cacheActions.Put(cacheKey, actions);
                return actions;
            }
        }

        public List<Entity.Action> GetActionsByRoles(List<int> roleIds)
        {
            if (roleIds == null || roleIds.Count == 0)
            {
                return new List<Entity.Action>();
            }
            else
            {
                List<Entity.Action> actions = new List<Entity.Action>();
                foreach (var item in roleIds)
                {
                    var roleActions = GetActionsByRole(item);
                    if (actions.Count == 0)
                    {
                        actions.AddRange(roleActions);
                    }
                    else
                    {
                        foreach (var roleAction in roleActions)
                        {
                            if (!actions.Exists(a => a.Id == roleAction.Id))
                            {
                                actions.Add(roleAction);
                            }
                        }
                    }
                }

                return actions;
            }
        }

        public List<Entity.Action> GetActionsByRolePathId(int roleId, int pathId)
        {
            var actions = GetActionsByRole(roleId);
            return actions.Where(a => a.PathId == pathId).ToList();
        }

        public List<Entity.Action> GetActionsByRolePathIds(int roleId, List<int> pathIds)
        {
            var actions = GetActionsByRole(roleId);
            return actions.Where(a => pathIds.Contains(a.PathId)).ToList();
        }

        public List<Entity.Action> GetActionsByRolesPathId(List<int> roleIds, int pathId)
        {
            var actions = GetActionsByRoles(roleIds);
            return actions.Where(a => a.PathId == pathId).ToList();
        }

        public List<Entity.Action> GetActionsByRolesPathIds(List<int> roleIds, List<int> pathIds)
        {
            var actions = GetActionsByRoles(roleIds);
            return actions.Where(a => pathIds.Contains(a.PathId)).ToList();
        }

        #endregion

       

        #region Paths
        public bool AddPathIntoRole(int pathId, int roleId, out string errorMsg)
        {
            errorMsg = string.Empty;
            try
            {
                if (pathInRoleRep.GetCountByCondtion(pr => pr.PathId == pathId && pr.RoleId == roleId) == 0)
                {
                    pathInRoleRep.Insert(new PathInRole() { PathId = pathId, RoleId = roleId });
                }
                cachePaths.Remove(PkPmCacheKeys.PathsByRoleIdFmt.Fmt(roleId));
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        public bool AddPathsIntoRole(List<int> pathIds, int roleId, out string errorMsg)
        {
            errorMsg = string.Empty;
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        db.Delete<PathInRole>(pr => pr.RoleId == roleId);

                        foreach (var item in pathIds)
                        {
                            db.Insert(new PathInRole() { PathId = item, RoleId = roleId });
                        }
                        dbTrans.Commit();
                        cachePaths.Remove(PkPmCacheKeys.PathsByRoleIdFmt.Fmt(roleId));
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

        public List<Path> GetPathsByRole(int roleId)
        {
            //string cacheKey = PkPmCacheKeys.PathsByRoleIdFmt.Fmt(roleId);
            //var paths = cachePaths.Get(cacheKey);
            //if (paths != null)
            //{
            //    return paths;
            //}
            //else
            //{
            //    paths = pathRep.GetByMasterDetailCondition<PathInRole>(null, pr => pr.RoleId == roleId, null, null);
            //    cachePaths.Put(cacheKey, paths);
            //    return paths;
            //}

            var paths = pathRep.GetMasterDetailConditionSort<PathInRole>(null, pr => pr.RoleId == roleId, new SortingOptions<Path>(p => new { p.Id }));
            return paths;
        }

        public List<Path> GetPathsByRoles(List<int> roleIds)
        {
            if (roleIds == null || roleIds.Count == 0)
            {
                return new List<Path>();
            }
            else
            {
                List<Entity.Path> paths = new List<Entity.Path>();
                foreach (var item in roleIds)
                {
                    var rolePaths = GetPathsByRole(item);
                    if (paths.Count == 0)
                    {
                        paths.AddRange(rolePaths);
                    }
                    else
                    {
                        foreach (var rolePath in rolePaths)
                        {
                            if (!paths.Exists(a => a.Id == rolePath.Id))
                            {
                                paths.Add(rolePath);
                            }
                        }
                    }
                }

                return paths;
            }
        }

        public List<Entity.Path> GetAllCategory()
        {
            var paths = cachePaths.Get(PkPmCacheKeys.CategoryPaths);
            if (paths != null)
            {
                return paths;
            }
            else
            {
                paths = pathRep.GetByCondition(p => p.IsCategory && p.Status == 1);
                cachePaths.Put(PkPmCacheKeys.CategoryPaths, paths);
                return paths;
            }
        }
        #endregion

        #region Users
        public bool AddUserIntoRole(int userId, int roleId, out string errorMsg)
        {
            errorMsg = string.Empty;
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        db.Delete<UserInRole>(ur => ur.UserId == userId);
                        db.Insert<UserInRole>(new UserInRole() { UserId = userId, RoleId = roleId });

                        dbTrans.Commit();
                        cacheRole.Remove(PkPmCacheKeys.RoleByUserIdFmt.Fmt(userId));
                        cacheInsts.Remove(PkPmCacheKeys.CustomsByUserIdFmt.Fmt(userId));
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

        public bool AddUsersIntoRole(List<int> userIds, int roleId, out string errorMsg)
        {
            errorMsg = string.Empty;
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        db.Delete<UserInRole>(ur => userIds.Contains(ur.UserId));

                        List<UserInRole> userInRoles = new List<UserInRole>();

                        foreach (var item in userIds)
                        {
                            db.Insert<UserInRole>(new UserInRole() { UserId = item, RoleId = roleId });
                        }
                        dbTrans.Commit();
                        foreach (var userId in userIds)
                        {
                            cacheRole.Remove(PkPmCacheKeys.RoleByUserIdFmt.Fmt(userId));
                            cacheInsts.Remove(PkPmCacheKeys.CustomsByUserIdFmt.Fmt(userId));
                        }
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

        public bool AddRolesIntoUser(int userId, List<int> roleIds, out string errorMsg)
        {
            errorMsg = string.Empty;
            using (var db = dbFactory.Open())
            {
                using (var dbTrans = db.OpenTransaction())
                {
                    try
                    {
                        db.Delete<UserInRole>(ur => ur.UserId == userId);

                        List<UserInRole> userInRoles = new List<UserInRole>();

                        foreach (var item in roleIds)
                        {
                            db.Insert<UserInRole>(new UserInRole() { UserId = userId, RoleId = item });
                        }
                        dbTrans.Commit();
                        cacheRole.Remove(PkPmCacheKeys.RoleByUserIdFmt.Fmt(userId));
                        cacheInsts.Remove(PkPmCacheKeys.CustomsByUserIdFmt.Fmt(userId));
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



        #endregion

        public bool IsAdmin(List<Role> roles)
        {
            if (roles == null || roles.Count() == 0)
            {
                return false;
            }

            return roles.Exists(r => r.Code == pkpmConfigService.AdminRoleCode);
        }

        public bool IsSuperVisor(List<Role> roles)
        {
            if (roles == null || roles.Count() == 0)
            {
                return false;
            }

            return roles.Exists(r => r.Code == pkpmConfigService.SuperVisorRoleCode);
        }

        public Role GetDefaultRole()
        {
            var roles = roleRep.GetByCondition(r => r.Code == pkpmConfigService.InstRoleCode);
            if (roles != null && roles.Count > 0)
            {
                return roles.First();
            }
            else
            {
                return roleRep.GetByCondition(r => true).First();
            }
        }

        public bool IsAdmin(Role role)
        {
            if (role == null)
            {
                return false;
            }
            else
            {
                return role.Code == pkpmConfigService.AdminRoleCode;
            }
        }

        public bool IsSample(Role role)
        {
            if (role == null || pkpmConfigService.QrRoleCode.IsNullOrEmpty())
            {
                return false;
            }
            else
            {
                if (pkpmConfigService.QrRoleCode.Contains(","))
                {
                    return string.Compare(role.Code, pkpmConfigService.QrRoleCode.Split(',')[0], true) == 0;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsWitnes(Role role)
        {
            if (role == null || pkpmConfigService.QrRoleCode.IsNullOrEmpty())
            {
                return false;
            }
            else
            {
                if (pkpmConfigService.QrRoleCode.Contains(","))
                {
                    return string.Compare(role.Code, pkpmConfigService.QrRoleCode.Split(',')[1], true) == 0;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool IsQr(Role role)
        {
            if (role == null || pkpmConfigService.QrRoleCode.IsNullOrEmpty())
            {
                return false;
            }
            else
            {
                return pkpmConfigService.QrRoleCode.Contains(role.Code);
            }
        }

        public bool IsReport(Role role)
        {
            if (role == null || pkpmConfigService.ReportRoleCode.IsNullOrEmpty())
            {
                return false;
            }
            else
            {
                return pkpmConfigService.ReportRoleCode.Contains(role.Code);
            }
        }

        public bool IsSuperVisor(Role role)
        {
            if (role == null)
            {
                return false;
            }
            else
            {
                return role.Code == pkpmConfigService.SuperVisorRoleCode;
            }
        }

        public bool IsInstUser(Role role)
        {
            if (role == null)
            {
                return true;
            }
            else
            {
                return role.Code == pkpmConfigService.InstRoleCode;
            }
        }

        public bool IsCheckPeople(Role role)
        {
            if (role == null)
            {
                return true;
            }
            else
            {
                return role.Code == pkpmConfigService.CheckPeopleRoleCode;
            }
        }

        public bool IsAllQYYAndJYY(Role role)
        {
            if (role == null)
            {
                return false;
            }
            else
            {
                return role.Code == pkpmConfigService.AllQYYAndJYYCode;
            }
        }

        public Role GetRoleByCode(string Code)
        {
            var cacheKey = PkPmCacheKeys.GetRoleByRoleCode.Fmt(Code);
            var role = cacheRole.Get(cacheKey);
            if (role != null)
            {
                return role;
            }
            var roles = roleRep.GetByCondition(r => r.Code == Code);
            if (roles.Count == 0)
            {
                return new Role() { Id = -1, Name = "不存在" };
            }
            else
            {
                cacheRole.Add(cacheKey, roles[0]);
                return roles[0];
            }
        }
    }
}
