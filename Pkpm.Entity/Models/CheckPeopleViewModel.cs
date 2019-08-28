using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.Models
{
    public class CheckPeopleViewModel
    {
        public List<SysDict> CompayTypes { get; set; }
        public List<SysDict> HasCerts { get; set; }
        public List<SysDict> WorkStatus { get; set; }
        public List<SysDict> PersonnelTitles { get; set; }
        public List<SysDict> EngineersType { get; set; }
        public List<SysDict> PersonnelStatus { get; set; }
        public List<SysDict> PersonnelStaff { get; set; }
        public List<SysDict> YesNo { get; set; }
        public List<SysDict> Sex { get; set; }
        //public List<SysDict> CompanyTypes { get; set; }

        public t_bp_People people { get; set; }
        public string iszcgccs { get; set; }
        public string postTypeView { get; set; }
        public string CustomName { get; set; }
        public Dictionary<string, string> CheckUnitNames { get; set; }
        public bool IsAdmin { get; set; }
        public List<CheckPeopleChangeModel> CheckPeopleChange { get; set; }       
        public List<CheckPeopleEducationModel> CheckPeopleEducation { get; set; }
        public List<CheckPeopleAwardsModel> CheckPeopleAwards { get; set; }
        public List<CheckPeoplePunishModel> CheckPeoplePunish { get; set; }

        public List<CheckPeopleFileModel> CheckPeopleFile { get; set; }

        public List<CheckPeopleVerificationModel> CheckPeopleVerification { get; set; }
        public Dictionary<string, string> AllUnitIdAndName { get; set; }

    }

    public class CheckPeopleUIModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Customid { get; set; }
        public string SelfNum { get; set; }
        public string PostNum { get; set; }
        public string zw { get; set; }
        public string Title { get; set; }
        public string iscb { get; set; }
        public string Approvalstatus { get; set; }
        public DateTime? postdate { get; set; }
        public int IsUse { get; set; }
    }

    public class CheckPeopleSearchModel
    {
        public string CheckUnitName { get; set; }
        public string Name { get; set; }
        public string IDNum { get; set; }
        public string HasCert { get; set; }
        public string PostCertNum { get; set; }
        public string UnitCategory { get; set; }
        public string PeopleStatus { get; set; }
        public string PositionCategory { get; set; }
        public DateTime? AgeStartDt { get; set; }
        public DateTime? AgeEndDt { get; set; }
        public int? AgeStart { get; set; }
        public int? AgeEnd { get; set; }
        public string TechTitle { get; set; }
        public string IsTech { get; set; }
        public string Status { get; set; }
        public string PubSeqNo { get; set; }
        public string Position { get; set; }
        public string SearchRule { get; set; }
        public string postTypeCode { get; set; }
        public string CompanyType { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class CheckPeopleSaveViewModel : t_bp_People
    {
        public string changeGrid { get; set; }
        public string verficationGrid { get; set; }
        public string educationGrid { get; set; }
        public string awardsGrid { get; set; }
        public string punishGrid { get; set; }
        public t_bp_PeoAward[] peoAwards { get; set; }
        public t_bp_PeoChange[] peoChange { get; set; }
        public t_bp_PeoEducation[] peoEducation { get; set; }
        public t_bp_PeoPunish[] peoPunish { get; set; }
        //public t_bp_PeopleN[] peoVerification { get; set; }
        public string postnumdate { get; set; }
        public string zcgccszhdate { get; set; }

    }

    public class CheckPeopleApplyChange
    {
        public string SubmitName { get; set; }
        public string SubmitText { get; set; }
        public int SubmitId { get; set; }
        public string SubmitCustomId { get; set; }
    }

    public class CheckPeopleSaveModel
    {
        public int OpUserId { get; set; }
        public string CustomName { get; set; }
        public t_bp_People people { get; set; }
        public List<t_bp_PeoChange> PeoChanges { get; set; }
        //public List<t_bp_PeopleN> PeopleNs { get; set; }
        public List<t_bp_PeoEducation> PeoEducations { get; set; }
        public List<t_bp_PeoAward> PeoAwards { get; set; }
        public List<t_bp_PeoPunish> PeoPunishs { get; set; }
        public User user { get; set; }
    }

    public class CheckPeopleTmpSaveModel
    {
        public int OpUserId { get; set; }
        public string CustomName { get; set; }
        public t_bp_People_tmp people { get; set; }
        public List<t_bp_PeoChange> PeoChanges { get; set; }
        //public List<t_bp_PeopleN> PeopleNs { get; set; }
        public List<t_bp_PeoEducation> PeoEducations { get; set; }
        public List<t_bp_PeoAward> PeoAwards { get; set; }
        public List<t_bp_PeoPunish> PeoPunishs { get; set; }
        public User user { get; set; }
    }

    public class CheckPeopleCompareModel
    {
        [DisplayName("人员ID")]
        public int id { get; set; }
        [DisplayName("机构ID")]
        public string Customid { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("性别")]
        public string Sex { get; set; }
        [DisplayName("出生年月")]
        public DateTime? Birthday { get; set; }
        [DisplayName("照片")]
        public string PhotoPath { get; set; }
        [DisplayName("身份证号")]
        public string SelfNum { get; set; }
        [DisplayName("最高学历")]
        public string Education { get; set; }
        [DisplayName("毕业学校")]
        public string School { get; set; }
        [DisplayName("专业")]
        public string Professional { get; set; }
        [DisplayName("职称")]
        public string Title { get; set; }
        [DisplayName("联系电话")]
        public string Tel { get; set; }
        [DisplayName("电子邮箱")]
        public string Email { get; set; }
        [DisplayName("社保号码")]
        public string SBNum { get; set; }
        [DisplayName("岗位资质类别")]
        public string PostType { get; set; }
        [DisplayName("岗位证书编号")]
        public string PostNum { get; set; }
        [DisplayName("岗位资质照片")]
        public string PostPath { get; set; }
        [DisplayName("岗位证书获得时间")]
        public DateTime? PostDate { get; set; }
        [DisplayName("PosrPeople")]
        public string PosrPeople { get; set; }
        [DisplayName("PostIs")]
        public string PostIs { get; set; }
        [DisplayName("Approvalstatus")]
        public string Approvalstatus { get; set; }
        [DisplayName("岗位证书延续注册情况")]
        public string postDelayReg { get; set; }
        [DisplayName("身份证照片")]
        public string selfnumPath { get; set; }
        [DisplayName("岗位证书有效起始日期")]
        public DateTime? postnumstartdate { get; set; }
        [DisplayName("岗位证书有效截止日期")]
        public DateTime? postnumenddate { get; set; }
        [DisplayName("approveadvice")]
        public string approveadvice { get; set; }
        [DisplayName("age")]
        public int? age { get; set; }
        [DisplayName("postTypeCode")]
        public string postTypeCode { get; set; }
        [DisplayName("职称照片")]
        public string titlepath { get; set; }
        [DisplayName("职务")]
        public string zw { get; set; }
        [DisplayName("是否为注册工程师")]
        public string iszcgccs { get; set; }
        [DisplayName("注册工程师证书号")]
        public string zcgccszh { get; set; }
        [DisplayName("注册工程师证书照片")]
        public string zcgccszhpath { get; set; }
        [DisplayName("学历照片")]
        public string educationpath { get; set; }
        [DisplayName("注册工程师证书有效起始日期")]
        public DateTime? zcgccszhstartdate { get; set; }
        [DisplayName("注册工程师证书有效截止日期")]
        public DateTime? zcgccszhenddate { get; set; }
        [DisplayName("是否注册在本单位")]
        public string isreghere { get; set; }
        [DisplayName("在职状况")]
        public string iscb { get; set; }
        [DisplayName("ismanager")]
        public string ismanager { get; set; }
        [DisplayName("isjs")]
        public string isjs { get; set; }
        [DisplayName("issy")]
        public string issy { get; set; }
        [DisplayName("posttime")]
        public DateTime? posttime { get; set; }
        [DisplayName("postname")]
        public string postname { get; set; }
        [DisplayName("ispostvalid")]
        public string ispostvalid { get; set; }
        [DisplayName("是否有岗位证书")]
        public string ishaspostnum { get; set; }
        [DisplayName("titleother")]
        public string titleother { get; set; }
        [DisplayName("sbnumpath")]
        public string sbnumpath { get; set; }
        [DisplayName("nameshouzimu")]
        public string nameshouzimu { get; set; }
        [DisplayName("ispile")]
        public string ispile { get; set; }
        [DisplayName("dzsj")]
        public DateTime? dzsj { get; set; }
        [DisplayName("djtime")]
        public DateTime? djtime { get; set; }
        [DisplayName("data_status")]
        public string data_status { get; set; }
        [DisplayName("update_time")]
        public DateTime? update_time { get; set; }
        [DisplayName("IsUse")]
        public int IsUse { get; set; }
        [DisplayName("工作年限")]
        public string gznx { get; set; }
    }

    public class CheckPeoplePostType
    {
        public string PostType { get; set; }
        public string postTypeCode { get; set; }
        public string userGrade { get; set; }
        public string displaytype { get; set; }
    }

    public class CheckPeopleChangeModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string ChaDate { get; set; }
        public string ChaContent { get; set; }
        public string PeopleId { get; set; }
    }

    public class CheckPeopleVerificationModel
    {
        public string Id { get; set; }
        public string AddTime { get; set; }
        public string Pcontext { get; set; }
        public string PeopleId { get; set; }
        public int Number { get; set; }

    }

    public class CheckPeopleEducationModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string TrainDate { get; set; }
        public string TrainContent { get; set; }
        public string TrainUnit { get; set; }
        public string TestDate { get; set; }
        public string PeopleId { get; set; }
    }

    public class CheckPeopleAwardsModel
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string AwaDate { get; set; }
        public string AwaContent { get; set; }
        public string AwaUnit { get; set; }
        public string PeopleId { get; set; }

    }

    public class CheckPeoplePunishModel
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string PunDate { get; set; }
        public string PunName { get; set; }
        public string PunContent { get; set; }
        public string PunUnit { get; set; }
        public string PeopleId { get; set; }
    }

    public class CheckPeopleFileModel
    {
        public int Number { get; set; }
        public string FileName { get; set; }
        public string FileState { get; set; }
        public string Path { get; set; }
    }

    public class CheckPeopleEditAttachFileModel
    {
        public string FilePath { get; set; }
        public string[] Paths { get; set; }
        public string Name { get; set; } //存储当前查看项目名称
        public string id { get; set; }
    }

    public class PeopleFieldModel
    {
        public int Id { get; set; }
        public string CustomId { get; set; }
        public string PostTypeCode { get; set; }
    }

    public class PeopleUnitChangeModel
    {
        public string PeopleName { get; set; }
        public string PeopleId { get; set; }
        public Dictionary<string,string> AllUnit { get; set; }
    }


}
