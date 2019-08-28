using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class STCheckPeopleModels
    {
    }
    public class STCheckPeopleViewModel
    {
        public List<SysDict> Status { get; set; }
    }

    public class STCheckPeopleSearchModel
    {
        public string CheckUnitName { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public DateTime? AgeStartDt { get; set; }
        public DateTime? AgeEndDt { get; set; }
        public int? AgeStart { get; set; }
        public int? AgeEnd { get; set; }
        public string PositionCategory { get; set; }
        public string PostCertNum { get; set; }
        public string TechTitle { get; set; }
        public string IDnum { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsCheck { get; set; }
        public bool IsTitle { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }

    }

    public class STCheckPeopleUIModel
    {
        public int Id { get; set; }
        public string Customid { get; set; }
        public string Name { get; set; }
        public string SelfNum { get; set; }
        public string PostNum { get; set; }
        public string Tel { get; set; }
        public string Approvalstatus { get; set; }
    }

    public class STCheckPeopleApplyChangeViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class STCheckPeopleApplyChange
    {
        public string SubmitName { get; set; }
        public string SubmitText { get; set; }
        public string SubmitId { get; set; }
        public string Status { get; set; }
        public bool Result { get; set; }
    }

    public class STCheckPeopleDetails
    {
        public t_bp_People_ST People { get; set; }
        public string UnitName { get; set; }
        public List<STCheckPeopleChangeModel> STCheckPeopleChange { get; set; }
        public List<STCheckPeopleEducationModel> STCheckPeopleEducation { get; set; }
        public List<STCheckPeopleAwardsModel> STCheckPeopleAwards { get; set; }
        public List<STCheckPeoplePunishModel> STCheckPeoplePunish { get; set; }
        public List<STCheckPeopleGridFileModel> STCheckPeopleFile { get; set; }
    }

    public class STCheckPeoplePunishModel
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string PunDate { get; set; }
        public string PunName { get; set; }
        public string PunContent { get; set; }
        public string PunUnit { get; set; }
        public string PeopleId { get; set; }
    }

    public class STCheckPeopleAwardsModel
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string AwaDate { get; set; }
        public string AwaContent { get; set; }
        public string AwaUnit { get; set; }
        public string PeopleId { get; set; }

    }

    public class STCheckPeopleEducationModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string TrainDate { get; set; }
        public string TrainContent { get; set; }
        public string TrainUnit { get; set; }
        public string TestDate { get; set; }
        public string PeopleId { get; set; }
    }

    public class STCheckPeopleChangeModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string ChaDate { get; set; }
        public string ChaContent { get; set; }
        public string PeopleId { get; set; }
    }

    public class STCheckPeopleGridFileModel
    {
        public int Number { get; set; }
        public string FileName { get; set; }
        public string FileState { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
    }

    public class STCheckPeopleEditViewModel
    {
        public InstShortInfos allSTUnit { get; set; }
        public t_bp_People_ST People { get; set; }
        public string UnitName { get; set; }
        public List<SysDict> Sex { get; set; }
        public List<SysDict> NoYes { get; set; }
        public List<SysDict> STPersonnelTitles { get; set; }
        public string Birthday { get; set; }
        public string Postnumdate { get; set; }
        public string PostDate { get; set; }
        public List<STCheckPeopleChangeModel> STCheckPeopleChange { get; set; }
        public List<STCheckPeopleEducationModel> STCheckPeopleEducation { get; set; }
        public List<STCheckPeopleAwardsModel> STCheckPeopleAwards { get; set; }
        public List<STCheckPeoplePunishModel> STCheckPeoplePunish { get; set; }
        public List<STCheckPeopleGridFileModel> STCheckPeopleFile { get; set; }
    }

    public class STCheckPeopleSaveViewModel : t_bp_People_ST
    {

        public t_bp_PeoAwards_ST[] peoAward { get; set; }
        public t_bp_PeoPunish_ST[] peoPunish { get; set; }
        public t_bp_PeoEducation_ST[] peoEducation { get; set; }
        public t_bp_PeoChange_ST[] peoChange { get; set; }
        public string Postnumdate { get; set; }


    }

    public class STPeopleDetailsViewModel
    {
        public string CustomId { get; set; }
        public string IsAdmin { get; set; }
        public string IsTitle { get; set; }
        public string IsCheck { get; set; }
    }


}