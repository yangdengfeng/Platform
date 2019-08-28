using Pkpm.Entity; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.UserRoleCore
{
    public interface IUserService
    {
        Entity.Role GetUserRole(int userId);
        List<Entity.Path> GetUserPaths(int userId);
        List<Entity.Action> GetUserActions(int userId, int pathId);
        List<t_sys_region> GetAllArea();
        List<Entity.Path> GetPersonaliztionPaths(int userId);
        List<Entity.Action> GetPersonaliztionActions(int userId);
        List<Entity.Action> GetPersonaliztionActionsPathId(int userId, int pathId);
        List<UserInArea> GetUserAreas(int userId);
        List<UIArea> GetAllAreas();
        List<TestType_BM> GetAllCompanyQualification();
        string GetUserDisplayName(int userId); 

        InstShortInfos GetUserInsts(int userId, string canCheckAllCustomRoles);
        InstShortInfos GetUserInstsST(int userId);
        InstShortInfos GetUserFormalInsts(int userId); 
        InstFilter GetFilterInsts(int userId, string canCheckAllCustomRoles);
        InspectFilter GetFilterInspect(int userId);
        IEnumerable<User> GetUsers();
        t_bp_custom GetCustomInsts(int userId);
        List<User> GetSysExpertRoleUsers();

        User GetUserByName(string name);

        void AddUserEvent(Entity.SysLog sysLog);
        bool AddAreasIntoUser(int userId, List<string> areas, out string errorMsg);
        bool AddPathsIntoUser(int userId, List<int> pathIds, out string errorMsg);
        bool AddActionsIntoUser(int userId, List<int> actionIds, out string errorMsg);
        bool EditUser(User user, int? roleId, out string errorMsg);
        bool DeleteUser(int userId, out string errorMsg);
        bool IsAdmin(int userId);
        bool IsInspector(int userId);
        bool IsInstUser(int userId);
        bool IsSample(int userId);
        bool IsWitnes(int userId);
        bool IsCheckPeople(int userId);
        bool IsAllQYYAndJYY(int userId);
        /// <summary>
        /// 把微信OpenId绑定到账号
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="openId"></param>
        /// <param name="single">一个账号只允许绑定一个OpenId</param>
        void BindOpenId(int userId, string openId, bool single);
        UserAsRole GetByWxOpenId(string openId);
        bool CurrentAccountIsCardNo(int userId);
        /// <summary>
        /// 检测软件调webservice进行见证取样人员注册
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        int ApiCreateUser(User user, int RoleId, out string ErrorMsg);
        List<User> ApiGetUsers(string DisplayName, int RoleId, int? Start, int? Size, out int dbCount);
        List<User> ApiGetUserByUserName(string name, List<int> roleId);
        Dictionary<int, string> ApiGetUserNameByUserIds(List<int> userIds);
        Dictionary<int, string> ApiGetUserNameByUserIds(List<string> userIds);

        string InstUserCustomId(int UserId);

        string GetAreasByArea(string Area);

        InstShortInfos GetDictByKey(string key);
        InstShortInfos GetUserEliminateIsUerInsts(int userId);
        InstShortInfos GetAllCheckUnitEliminateIsUer();
    }
}
