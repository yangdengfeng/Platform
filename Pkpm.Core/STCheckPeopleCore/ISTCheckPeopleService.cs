using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Core.STCheckPeopleCore
{
    public interface ISTCheckPeopleService
    {
        bool DeletePeople(string ids, out string erroMsg);

        bool SetInstSendState(string selectedId, string state, out string erroMsg);

        bool CanApplyChangeCustom(string approvalstatus);

        bool ApplyChangeForCustom(SupvisorJob job, string Id, out string errorMsg);

        bool UpdateCustomStatus(string Id, string STEuqipStatus, string Reason, out string errorMsg);

        bool EditPeople(STCheckPeopleSaveModel model, out string erroMsg);

        bool CreatePeople(STCheckPeopleSaveModel model, out string erroMsg);
    }
}
