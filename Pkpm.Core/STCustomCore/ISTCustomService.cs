using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.STCustomCore
{
    public interface ISTCustomService
    {
        bool DeleteCustom(string selectedId, out string erroMsg);
        bool SetInstSendState(string selectedId, string state, out string erroMsg);
        bool ApplyChangeForCustom(SupvisorJob job, string customId, out string errorMsg);
        bool EditCustom(STCustomSaveModel editModel, out string erroMsg);
        bool NewCustom(string customId, string customName, out string erroMsg);
        bool UpdateCustomStatus(string customId, bool rsult, string Reason, out string errorMsg);
        bool CanApplyChangeCustom(string approvalstatus);
        string GetSTUnitByIdFromAll(InstShortInfos allInsts, string customId);
        InstShortInfos GetAllCustomST();
        InstShortInfos GetSubInstSTs(string customId);
        InstShortInfos GetPartCheckUnitST(string customPart);
        InstShortInfos GetUnitByIdST(string customId);
        InstShortInfos GetDefaultInstSTs();
    }
}
