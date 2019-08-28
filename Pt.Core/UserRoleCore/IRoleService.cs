using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.UserRoleCore
{
   public interface IRoleService
    {
        bool AddNewRole(Entity.Role role, out string errorMsg);
        bool EditRole(Entity.Role role, out string errorMsg);
        bool DeleteRole(int roleId, out string errormsg);

        bool AddUserIntoRole(int userId, int roleId,out string errorMsg);
        bool AddRolesIntoUser(int userId, List<int> roleIds, out string errorMsg);
        bool AddUsersIntoRole(List<int> userIds, int roleId, out string errorMsg); 

        bool AddActionIntoRole(int actionId, int roleId, out string errorMsg);
        bool AddActionsIntoRole(List<int> actionIds, int roleId, out string errorMsg);
        List<Entity.Action> GetActionsByRole(int roleId);
        List<Entity.Action> GetActionsByRoles(List<int> roleIds);
        List<Entity.Action> GetActionsByPathId(int pathId);
        List<Entity.Action> GetActionsByPathIds(List<int> pathIds);
        List<Entity.Action> GetActionsByRolePathId(int roleId,int pathId);
        List<Entity.Action> GetActionsByRolePathIds(int roleId, List<int> pathIds);
        List<Entity.Action> GetActionsByRolesPathId(List<int> roleIds, int pathId);
        List<Entity.Action> GetActionsByRolesPathIds(List<int> roleIds, List<int> pathIds);

        bool AddPathIntoRole(int pathId, int roleId, out string errorMsg);
        bool AddPathsIntoRole(List<int> pathIds, int roleId, out string errorMsg);
        List<Entity.Path> GetAllCategory();
        List<Entity.Path> GetPathsByRole(int roleId);
        List<Entity.Path> GetPathsByRoles(List<int> roleIds);

        Role GetDefaultRole();
        bool IsAdmin(List<Role> roles);
        bool IsAdmin(Role role);
        bool IsSuperVisor(List<Role> roles);
        bool IsSuperVisor(Role role);
        bool IsQr(Role role);
        bool IsSample(Role role);
        bool IsWitnes(Role role);
        bool IsInstUser(Role role);
        Role GetRoleByCode(string Code);
        bool IsReport(Role role);
        bool IsAllQYYAndJYY(Role role);
        bool IsCheckPeople(Role role);
    }
    
}
