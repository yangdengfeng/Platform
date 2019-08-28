using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pkpm.Entity;
using System.Collections.Specialized;

namespace Pkpm.Core.CheckUnitCore
{
    public interface ICheckUnitService
    {
        InstShortInfos GetAllSTCheckUnit();
        string GetSTCheckUnitById(string Customid);
        InstShortInfos GetAllCheckUnit();

        Dictionary<string, string> GetAllCustomInArea();

        bool UpdateCustomStatus(string customId, string CustomStatus, string Reason, out string errorMsg);

        InstShortInfos GetAllFormalCheckUnit();

        Dictionary<string, string> GetAllMasterCheckUnit();

        bool IsCanUploadUnit(string customId);

        Dictionary<string, string> GetCanUploadCheckUnit();

        InstShortInfos GetPartCheckUnit(string customPart);



        InstShortInfos GetSubInsts(string customId);

        InstShortInfos GetUnitById(string customId);

        InstShortInfos GetDefaultInsts();


        List<InstWithArea> GetInstAreasByName(string name);

        List<InstWithArea> GetAllInstAreas();

        bool IsMasterCustomId(string customId);       

        string GetMasterCustomId(string customId);

        InstShortInfos GetUnitByArea(List<UserInArea> areas);

        string GetCheckUnitByIdFromAll(InstShortInfos allInsts, string customId);

        string GetCustomCert(string customId);

        string GetCheckUnitById(string customId);

        string GetCheckUnitByName(string customName);

        string GetSubInstFlag(string customId);

        bool DeletePathsIntoUnit(string selectedId, out string erroMsg);

        bool SetInstSendState(string selectedId, string state, out string erroMsg);

        bool SetInstScreeningState(string selectedId, out string erroMsg);

        bool SetInstRelieveScreeningSate(string selectId, out string erroMsg);

        void ClearCheckUnitCache();

        int CountCardChanges(string name, string customIDs);

        List<UICardChange> GetCardChanges(string name, string customIDs, int skip, int count);

        bool ApplyChangeForCustom(SupvisorJob job, string customId, out string errorMsg);

        bool SetPrintConfig(string customId, string cbgfs, out string errorMsg);

        bool EditCustom(CheckCustomSaveModel editModel, out string erroMsg);

        bool EditCustom(ApplyCustomSaveModel editModel, out string erroMsg);

        bool EditCustom(CheckCustomTmpSaveModel editModel, out string errorMsg);

        bool AuditCustom(NameValueCollection formCol, Dictionary<string, List<SysDict>> dic, out string erroMsg);

        bool EditCustomCheckQualify(CheckCustomSaveModel editModel, out string erroMsg);

        bool EditEquipment(CheckEquipSaveModel editModel, out string erroMsg);

        bool EditEquipment(CheckEquipTmpSaveModel editModel, out string erroMsg);

        bool AuditEquipment(NameValueCollection formCol, Dictionary<string, List<SysDict>> dic, out string erroMsg);

        bool CreatEquipment(CheckEquipSaveModel createModel, out string erroMsg);

        bool DeleteEquipments(string selectedId, out string erroMsg);

        bool ApplyChangeForEquip(SupvisorJob job, int equipId, out string errorMsg);

        bool SendStatueForEquips(string selectedId, out string erroMsg);

        bool ReturnStateFroEquips(string selectedId, out string erroMsg);

        bool UpdateAttachPathsIntoCustom(string customid, string fieldname, string pathname, out string erroMsg);

        bool UpdateAttachPathsIntoEquip(int id, string fieldname, string pathname, out string erroMsg);

        InstShortInfos GetCustomDetectNum();

        bool NotifyCustomOffline(string customId, out string erroMsg);

        bool NewCustom(string customId,string customName, out string erroMsg);

        bool IsCustomIdExist(string customid);

        bool SetInstFormal(string customid, string FormalCustomId, out string errMsg);

        bool UpdateWjlrAndZzlbmc(string wjlr, string zzlbmc, string customid, out string ErrMsg);

        bool NoReportNumCanSearch();

        bool CanEditEquip(string approvalstatus);
        bool CanApplyChangeEquip(string approvalstatus);

        bool CanEditCustom(string approvalstatus);
        bool CanApplyChangeCustom(string approvalstatus);
        InstShortInfos GetCheckUnitByListCustomId(List<string> CustomId);
        InstShortInfos GetUnitByArea(List<string> areas);
        InstShortInfos GetAllCheckUnitEliminateIsUer();

        bool SetInstStoppingState(string selectedId, out string erroMsg);
    }
}
