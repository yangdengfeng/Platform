using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.STCheckEquipCore
{
    public interface ISTCheckEquipService
    {
        bool EditCheckEquip(STCheckEquipEditServiceModel model, out string erroMsg);

        bool CreateCheckEquip(STCheckEquipEditServiceModel model, out string erroMsg);

        bool DeleteEquip(string ids, out string erroMsg);

        bool SetInstSendState(string selectedId, string state, out string erroMsg);

        bool CanApplyChangeCustom(string approvalstatus);

        bool ApplyChangeForCustom(SupvisorJob job, string Id, out string errorMsg);

        bool UpdateCustomStatus(string Id, string STEuqipStatus, string Reason, out string errorMsg);
    }
}
