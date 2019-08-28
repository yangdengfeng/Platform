using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.CheckQualifyCore
{
    public interface ICheckQualifyService
    {

        bool SaveApplyQualifyOne(ApplyQualifySaveModel saveModel, out string errorMsg);

        bool EditApplyQualifyOne(ApplyQualifySaveModel saveModel, out string errorMsg);

        bool SaveDistributeExpert(t_D_UserTableTen model, out string errorMsg);

        bool SaveUserTableTwo(t_D_UserTableTwo model, out string errorMsg);

        bool EditQualifyThree(t_D_UserTableThree model, out string errorMsg);

        bool EditQualifyFours(t_D_UserTableFour model, out string errorMsg);

        bool SetInstSendState(string selectedId, string state, string table, out string erroMsg);

        bool Audit(t_D_UserTableTen model, out string errorMsg);

        bool SaveExpertUnitQualify(t_D_userTableSH saveModel, int userid, out string errorMsg);

        bool SaveSpeicalQualify(t_D_userTableSC saveModel, int userid, out string errorMsg);

        bool SaveCBRUnit(t_D_UserTableCS saveModel, int userid, out string errorMsg);

        bool SaveCBRUnitApproval(t_D_UserTableTen saveModel, out string errorMsg);

        bool SaveUserChange(t_D_UserChange saveModel, out string errorMsg);

        bool EditUserChange(t_D_UserChange saveModel, out string errorMsg);

        bool Approval(t_D_UserChange saveModel, out string errorMsg);
    }
}
