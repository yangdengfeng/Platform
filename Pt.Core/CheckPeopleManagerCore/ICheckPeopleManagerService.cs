using Pkpm.Entity;
using Pkpm.Entity.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.CheckPeopleManagerCore
{
    public interface ICheckPeopleManagerService
    {
        bool ApplyChangePeople(SupvisorJob job, int peopleId, out string errorMsg);
        bool EditPeople(CheckPeopleSaveModel editModel, out string erroMsg);
        bool EditPeople(CheckPeopleTmpSaveModel editModel, out string erroMsg);
        bool AuditPeople(NameValueCollection formCol, Dictionary<string, List<SysDict>> dic, out string erroMsg);
        bool CreatPeople(CheckPeopleSaveModel createModel, out string erroMsg);
        bool SetPosttype(string id, string mv, out string erroMsg);
        bool DeletePeople(string selectedId, out string erroMsg);
        bool SendPeople(string selectedId, out string erroMsg);
        bool ReturnStatePeople(string selectedId, out string erroMsg);
        bool AnnualTestPeople(string selectedId, out string erroMsg);
        bool FirePeople(string selectedId, out string erroMsg);
        bool SetPeopleScreeningState(string selectedId, out string erroMsg);
        bool SetPeopleRelieveScreeningSate(string selectId, out string erroMsg);
        bool ChangePeople(string peopleId, string Customid, out string erroMsg);
        List<CheckPeopleUIModel> GetRegisterOnJobPeople(Expression<Func<t_bp_People, bool>> predicate, List<string> visibleInstIds, int? pot, int? count, out int dbCount);
        List<CheckPeopleUIModel> GetNotRegisterPeople(Expression<Func<t_bp_People, bool>> predicate, List<string> visibleInstIds, int? pot, int? count, out int dbCount);
        List<CheckPeopleUIModel> GetRegisterLeaveJobPeople(Expression<Func<t_bp_People, bool>> predicate, List<string> visibleInstIds, int? pot, int? count, out int dbCount);
        List<CheckPeopleUIModel> GetLogoutPeople(Expression<Func<t_bp_People, bool>> predicate, List<string> visibleInstIds, int? pot, int? count, out int dbCount);
        bool UpdateAttachPathsIntoPeople(int id, string fieldname, string pathname, out string erroMsg);
        bool UpdatecellModelParams(string id, string value, out string erroMsg);
        bool UpdatePostNumDate(int id, DateTime PostDate, DateTime PostNumStartDate, DateTime PostNumEndDate, out string erroMsg);
        //List<AutoType> GetAutoType(string UserId);
        bool UpdatePostType(string PostType, string postTypeCode, string selfnum, out string erroMsg);
        bool CanEditPeople(string approvalstatus);
        bool CanApplyChangePeople(string approvalstatus);
        bool EditPeopleField(CheckPeopleSaveModel editModel, out string erroMsg);
    }
}
