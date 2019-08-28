using Pkpm.Entity.Auth;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;
using System.ComponentModel;

namespace Pkpm.Entity
{
    [Alias("Action")]
    [Serializable]
    public partial class Action : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public int PathId { get; set; }
        [Required]
        public int Status { get; set; }
    }

    [Alias("ActionInRole")]
    public partial class ActionInRole : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int ActionId { get; set; }
    }

    [Alias("ActionInUser")]
    public partial class ActionInUser : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int ActionId { get; set; }
        [Required]
        public int UserId { get; set; }
    }

    [Alias("CreditLevel")]
    public partial class CreditLevel : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string CreditLevelName { get; set; }
        [Required]
        public decimal CreditScoreStart { get; set; }
        [Required]
        public decimal CreditScoreEnd { get; set; }
        public string CreditRemark { get; set; }
        [Required]
        public DateTime CreateDt { get; set; }
        [Required]
        public DateTime UpdateDt { get; set; }
    }

    public class AddUploadItem
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string CustomId { get; set; }
        public DateTime AddDt { get; set; }
    }

    [Alias("CreditStandard")]
    public partial class CreditStandard
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int StandardCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal? Score { get; set; }
        [Required]
        public string Remark { get; set; }
        [Required]
        public DateTime CreateDt { get; set; }
        [Required]
        public DateTime UpdateDt { get; set; }
    }

    [Alias("curTestAuto")]
    public partial class curTestAuto
    {
        [Required]
        public int ID { get; set; }
        public string curTestID { get; set; }
        public string TestTypeCode { get; set; }
        public double? TheoryNum { get; set; }
        public double? PracticeNum { get; set; }
        public DateTime? InData { get; set; }
    }

    [Alias("curTestErea")]
    public partial class curTestErea
    {
        [Required]
        public int FIndex { get; set; }
        public int? FSemesterNo { get; set; }
        public int? FAreaCode { get; set; }
        public string FAreaName { get; set; }
    }

    [Alias("curTestRegister")]
    public partial class curTestRegister
    {
        [Required]
        public int id { get; set; }
        public string FCardNo { get; set; }
        public string FName { get; set; }
        public int? Fage { get; set; }
        public string Ftechnical { get; set; }
        public string FEducationalBackground { get; set; }
        public string FSpecialty { get; set; }
        public string FWork { get; set; }
        public string photoPath { get; set; }
        public DateTime? Ctime { get; set; }
        public int? Status { get; set; }
        public string curTest { get; set; }
        public string userid { get; set; }
        public string fsex { get; set; }
        public string approvaltext { get; set; }
        public string credentialsId { get; set; }
        public string GroupingId { get; set; }
        public string selfNumPath { get; set; }
        public string titlePath { get; set; }
        public string TypeCode { get; set; }
        public string credentiais { get; set; }
        public string educationpath { get; set; }
        public string text { get; set; }
        public string credentiaisStatic { get; set; }
        public string contact { get; set; }
        public string telephone { get; set; }
        public string J { get; set; }
        public int? JNUM { get; set; }
        public DateTime? addtime { get; set; }
    }

    [Alias("curTestRegisterApproval")]
    public partial class curTestRegisterApproval
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string userid { get; set; }
        public int? status { get; set; }
        public string approvaltext { get; set; }
        public int? curtest { get; set; }
    }

    [Alias("curTestRoom")]
    public partial class curTestRoom
    {
        [Required]
        public int FRoomNo { get; set; }
        public int? FSemesterNo { get; set; }
        public int? FRoomCode { get; set; }
        public string FRoomName { get; set; }
    }

    [Alias("curTestSubject")]
    public partial class curTestSubject
    {
        [Required]
        public int FSubjectNo { get; set; }
        public int? FSemesterNo { get; set; }
        public string FSubjectCode { get; set; }
        public string FSubjectName { get; set; }
    }

    [Alias("CustomCreditRate")]
    public partial class CustomCreditRate : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string CustomId { get; set; }
        [Required]
        public decimal StandardScore { get; set; }
        [Required]
        public decimal AddtionScore { get; set; }
        [Required]
        public decimal ActualScore { get; set; }
        [Required]
        public DateTime CreditStartDt { get; set; }
        [Required]
        public DateTime CreditEndDt { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public string CustomCreditRateStatus { get; set; }
        public string Remark { get; set; }
        [Required]
        public int CreateBy { get; set; }
    }

    [Alias("CustomCreditRateItem")]
    public partial class CustomCreditRateItem : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string CustomId { get; set; }
        [Required]
        public int CustomCreditRateId { get; set; }
        [Required]
        public DateTime CreditTime { get; set; }
        [Required]
        public int CreateUserId { get; set; }
        [Required]
        public string CreditRemark { get; set; }
        [Required]
        public string ActualResult { get; set; }
        public string CreditStatus { get; set; }
    }

    [Alias("CustomCreditRateItemAttach")]
    public partial class CustomCreditRateItemAttach : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int CustomCreditRateItemId { get; set; }
        [Required]
        public string AttachFileName { get; set; }
        [Required]
        public string AttachUrl { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
    }

    [Alias("CustomCreditRateItemLog")]
    public partial class CustomCreditRateItemLog : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int CustomCreditRateItemId { get; set; }
        [Required]
        public string Remark { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public int CreateUserId { get; set; }
    }

    [Alias("CustomCreditRateItemStandard")]
    public partial class CustomCreditRateItemStandard : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int CustomCreditRateItemId { get; set; }
        [Required]
        public int CreditStandardId { get; set; }
        [Required]
        public int topStandardId { get; set; }
        [Required]
        public int secondStandardId { get; set; }
        [Required]
        public decimal Score { get; set; }
        [Required]
        public decimal ActualScore { get; set; }
        [Required]
        public bool IsInclude { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
    }

    [Alias("CustomCreditRateLog")]
    public partial class CustomCreditRateLog
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int CustomCreditRateId { get; set; }
        [Required]
        public string Remark { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
    }

    [Alias("CustomSelfCredit")]
    public partial class CustomSelfCredit : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string CustomId { get; set; }
        [Required]
        public decimal CreditScore { get; set; }
        [Required]
        public DateTime CreditStartDt { get; set; }
        [Required]
        public DateTime CreditEndDt { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public int CreateBy { get; set; }
        public string Remark { get; set; }
        [Required]
        public int UpateBy { get; set; }
        [Required]
        public DateTime UpdateTime { get; set; }
    }

    [Alias("CustomSelfCreditAttach")]
    public partial class CustomSelfCreditAttach
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int CustomSelfCreditId { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FileUrl { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
    }

    [Alias("Path")]
    [Serializable]
    public partial class Path : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        [Required]
        public bool IsCategory { get; set; }
        [Required]
        public int OrderNo { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int Status { get; set; }
        public string Icon { get; set; }
    }

    [Alias("PathInRole")]
    public partial class PathInRole : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int RoleId { get; set; }
        [Required]
        public int PathId { get; set; }
    }

    [Alias("PathInUser")]
    public partial class PathInUser : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int PathId { get; set; }
    }

    [Alias("ProjectUnify")]
    public partial class ProjectUnify : IHasId<string>
    {
        [Alias("PROJECTNUM")]
        [Required]
        public string Id { get; set; }
        public string PROJECTNAME { get; set; }
        public string ENTRUSTUNIT { get; set; }
        public string CONSTRACTUNIT { get; set; }
        public string CONSTRACTIONUNIT { get; set; }
        public string SUPERUNIT { get; set; }
        public string DESIGNUNIT { get; set; }
        public string INSPECTUNIT { get; set; }
        public string GpsLocation { get; set; }
        public string PROJECTADDRESS { get; set; }
        public string PROJECTAREA { get; set; }
    }

    [Alias("ProjectUnifyPeople")]
    public partial class ProjectUnifyPerson : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string PROJECTNUM { get; set; }
        [Required]
        public string PeopleType { get; set; }
        public string IDCard { get; set; }
        public string MobilePhone { get; set; }
        public string CertNum { get; set; }
    }

    [Alias("ProjectUnifySample")]
    public partial class ProjectUnifySample : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string SampleQrCode { get; set; }
        public string SampleGpsLocation { get; set; }
        public DateTime? SampleDt { get; set; }
        public string SampleDesc { get; set; }
        public string SampleImageUrl { get; set; }
        public string SamplePeopleImageUrl { get; set; }
        public string WitnessGpsLocation { get; set; }
        public DateTime? WitnessDt { get; set; }
        public string WitnesImageUrl { get; set; }
        public string WitnessPeopleImageUrl { get; set; }
    }



    [Alias("SequenceNumber")]
    public partial class SequenceNumber : IHasId<string>
    {
        [AutoIncrement]
        public int ID { get; set; }
        [Alias("Code")]
        [Required]
        public string Id { get; set; }
        public string Prefix { get; set; }
        public string DateType { get; set; }
        public string Infix { get; set; }
        public int? IndexLength { get; set; }
        public string Suffix { get; set; }
        public string MaxDate { get; set; }
        public int? MaxIndex { get; set; }
        [Compute]
        public string CurrentMaxValue { get; set; }
    }

    [Alias("subItemConclusionKey")]
    public partial class subItemConclusionKey
    {
        [Required]
        public string itemTableName { get; set; }
        [Required]
        public string subItemName { get; set; }
        [Required]
        public string KeyWords { get; set; }
        public string dataType { get; set; }
        public string isNeed { get; set; }
    }

    [Alias("subItemList")]
    public partial class subItemList
    {
        [Required]
        public string itemTableName { get; set; }
        [Required]
        public string subItemName { get; set; }
        public string subItemChName { get; set; }
        public string UNITCODE { get; set; }
        public string ISDEFAULT { get; set; }
        public double? MONEYS { get; set; }
        public string JGBH { get; set; }
        public string SORTID { get; set; }
        public string C1 { get; set; }
        public string C2 { get; set; }
        public string C3 { get; set; }
        public string C4 { get; set; }
        public string C5 { get; set; }
        public string T2 { get; set; }
        public string T3 { get; set; }
        public string T4 { get; set; }
        public string T1 { get; set; }
        public string T5 { get; set; }
        public string D2 { get; set; }
        public string D3 { get; set; }
        public string D4 { get; set; }
        public string D1 { get; set; }
        public string D5 { get; set; }
        public string V1 { get; set; }
        public string V2 { get; set; }
        public string V3 { get; set; }
        public string V4 { get; set; }
        public string V5 { get; set; }
        public string L1 { get; set; }
        public string L2 { get; set; }
        public string L3 { get; set; }
        public string L4 { get; set; }
        public string L5 { get; set; }
        public string CGCS { get; set; }
    }

    [Alias("subitemlistforcount")]
    public partial class subitemlistforcount
    {
        [Required]
        public string itemtablename { get; set; }
        [Required]
        public string subitemcolumn { get; set; }
        public string subitemname { get; set; }
        public string subitempdcolumn { get; set; }
    }

    [Alias("subitemparm")]
    public partial class subitemparm
    {
        [Required]
        public string typcode { get; set; }
        public string typename { get; set; }
        [Required]
        public string itemcode { get; set; }
        public string itemname { get; set; }
        [Required]
        public string parmcode { get; set; }
        public string parmname { get; set; }
    }

    [Alias("subitempd")]
    public partial class subitempd
    {
        [Required]
        public string sysprimarykey { get; set; }
        [Required]
        public string subitemcolumn { get; set; }
        public string subitemname { get; set; }
        public string pdjg { get; set; }
    }

    [Alias("SubjectSet")]
    public partial class SubjectSet
    {
        public string FTestTypeNo { get; set; }
        public double? TheoryNum { get; set; }
        public double? PracticeNum { get; set; }
        public int? curtestregisterID { get; set; }
        [Required]
        public int id { get; set; }
        public string curTestId { get; set; }
        public int? stateN { get; set; }
    }

    [Alias("SupvisorJob")]
    public partial class SupvisorJob : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string CustomId { get; set; }
        [Required]
        public ApproveType ApproveType { get; set; }
        [Required]
        public string NeedApproveId { get; set; }
        [Required]
        public string SubmitText { get; set; }
        [Required]
        public string SubmitName { get; set; }
        [Required]
        public NeedApproveStatus NeedApproveStatus { get; set; }
        [Required]
        public int CreateBy { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
    }

    public enum ApproveType
    {
        /// <summary>
        /// 机构审核
        /// </summary>
        ApproveCustom,

        /// <summary>
        /// 人员审核
        /// </summary>
        ApprovePeople,

        /// <summary>
        /// 设备审核
        /// </summary>
        ApproveEquip,

        /// <summary>
        /// 商砼机构审核
        /// </summary>
        STApproveCustom,

        /// <summary>
        /// 商砼人员审核
        /// </summary>
        STApprovePeople,

        /// <summary>
        /// 商砼设备审核
        /// </summary>
        STApproveEquip,


    }


    public enum NeedApproveStatus
    {
        /// <summary>
        /// 申请修改
        /// </summary>
        CreateForChangeApply,

        /// <summary>
        /// 申请审核
        /// </summary>
        CreateForChange,

        /// <summary>
        /// 申请修改审核通过
        /// </summary>
        ApproveChangeApply,

        /// <summary>
        /// 申请审核 通过
        /// </summary>
        ApproveChange,

        /// <summary>
        /// 申请修改 审核不通过
        /// </summary>
        ChangeApplyDeny,

        /// <summary>
        /// 修改 审核不通过
        /// </summary>
        ChangeDeny
    }

    [Alias("SupvisorJobLog")]
    public partial class SupvisorJobLog : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int SupvisorJobId { get; set; }
        [Required]
        public string Remark { get; set; }
        [Required]
        public int CreateBy { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
    }

    [Alias("sysCustomInfomation")]
    public partial class sysCustomInfomation
    {
        [Required]
        public string id { get; set; }
        public string name { get; set; }
    }

    [Alias("SysDict")]
    [Serializable]
    public partial class SysDict
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string KeyValue { get; set; }
        [Required]
        public int OrderNo { get; set; }
        [Required]
        public int Status { get; set; }
    }

    [Alias("sysitemtypelist")]
    public partial class sysitemtypelist
    {
        [Required]
        public string typeCode { get; set; }
        public string typename { get; set; }
    }

    [Alias("sysitemtypes")]
    public partial class sysitemtype
    {
        public string itemtablename { get; set; }
        public string itemchname { get; set; }
        public string itemtypes { get; set; }
        public string onlyManageReport { get; set; }
    }

    [Alias("SysLog")]
    public partial class SysLog : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string UerName { get; set; }
        [Required]
        public string IpAddress { get; set; }
        [Required]
        public string LogEvent { get; set; }
        [Required]
        public DateTime LogTime { get; set; }
        public string LogType { get; set; }
    }

    [Alias("t_bp_acsinterface")]
    public partial class t_bp_acsinterface
    {
        [Required]
        public string UNITCODE { get; set; }
        [Required]
        public string SYSPRIMARYKEY { get; set; }
        [Required]
        public string COLUMNNAME { get; set; }
        public string ITEMTABLENAME { get; set; }
        public string SAMPLENUM { get; set; }
        public int? MAXLC { get; set; }
        public double? TIMES { get; set; }
        public double? MAXVALUE { get; set; }
        public double? QFVALUE { get; set; }
        public DateTime? ACSTIME { get; set; }
        public byte[] ACSDATA { get; set; }
        public string DATATYPES { get; set; }
        public double? A { get; set; }
        public double? B { get; set; }
        public double? A1 { get; set; }
        public double? B1 { get; set; }
        public double? A2 { get; set; }
        public double? B2 { get; set; }
        public byte[] CDACSDATA { get; set; }
        public double? YSN { get; set; }
        public byte[] ACSData1 { get; set; }
        public int? ISUPLOADED { get; set; }
        public string OPERATIONUSERNUM { get; set; }
        public string ACSDATApath { get; set; }
        public string ACSDATA1path { get; set; }
        public string CHECKMAN { get; set; }
        public double? CJSJ { get; set; }
        public string INSTRUMENTNUM { get; set; }
        public string INSTRUMENTNAME { get; set; }
        public double? SPEED { get; set; }
        public double? BZC { get; set; }
        public double? BYXS { get; set; }
        public double? PJZ { get; set; }
        public double? XGXS { get; set; }
        public string DGSJPD { get; set; }
        public double? HQFVALUE { get; set; }
        public string DHBJ { get; set; }
        public string MACNUM { get; set; }
        public int? CHEATS { get; set; }
    }

    [Serializable]
    [Alias("t_bp_area")]
    public partial class t_bp_area
    {
        [Required]
        public string AREACODE { get; set; }
        public string PAREACODE { get; set; }
        [Required]
        public string AREANAME { get; set; }
        [Required]
        public string CODETYPE { get; set; }
        public string MEMO { get; set; }
        public string INNERCODE { get; set; }
    }

    [Alias("t_bp_autoAcsItem")]
    public partial class t_bp_autoAcsItem
    {
        [Required]
        public string itemTableName { get; set; }
    }

    [Alias("t_bp_autocolumn")]
    public partial class t_bp_autocolumn
    {
        [Required]
        public string ITEMTABLENAME { get; set; }
        [Required]
        public string COLUMNNAME { get; set; }
        public int? ISAUTO { get; set; }
        [Required]
        public string unitCode { get; set; }
    }

    [Alias("t_bp_carriervehicle")]
    public partial class t_bp_carriervehicle
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        [Required]
        public string customid { get; set; }
        public string spec { get; set; }
        public int? num { get; set; }
        public string issubunit { get; set; }
    }

    [Alias("t_bp_checkCerf")]
    public partial class t_bp_checkCerf
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string Cerf { get; set; }
        [Required]
        public string parentId { get; set; }
        public string code { get; set; }
    }

    [Alias("t_bp_CheckCustom")]
    public partial class t_bp_CheckCustom
    {
        [Required]
        public int id { get; set; }
        public string CustomId { get; set; }
        public DateTime? CheDate { get; set; }
        public string CheResult { get; set; }
        public string CheRem { get; set; }
        public string cheunit { get; set; }
    }

    [Alias("t_bp_checkParams")]
    public partial class t_bp_checkParam
    {
        [Required]
        public long id { get; set; }
        [Required]
        public string code { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string sampleName { get; set; }
        [Required]
        public string standardName { get; set; }
        [Required]
        public string checkParams { get; set; }
        [Required]
        public string reportName { get; set; }
    }

    [Alias("t_bp_Contract")]
    public partial class t_bp_Contract
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string customid { get; set; }
        public string issubunit { get; set; }
        public string projectname { get; set; }
        public string contractunit { get; set; }
        public string contractionunit { get; set; }
        public string superunit { get; set; }
        public string hntyongling { get; set; }
        public string hetongjiner { get; set; }
        public DateTime? gonghuoshijian { get; set; }
        public string contractnum { get; set; }
        public string address { get; set; }
        public string approveadvice { get; set; }
        public string approvalstatus { get; set; }
        public string EntrustUnit { get; set; }
        public string ConstractionUnit { get; set; }
        public string PayMethod { get; set; }
        public string ProjectItemAttach { get; set; }
        public string InspectSerialNo { get; set; }
    }

    [Alias("t_bp_contractAttach")]
    public partial class t_bp_contractAttach
    {
        [Required]
        public int id { get; set; }
        public string filePath { get; set; }
        [Required]
        public int contractId { get; set; }
        public string fileName { get; set; }
    }

    [Alias("t_bp_ContractFile")]
    public partial class t_bp_ContractFile : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int ContractId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }

    [Alias("t_bp_ContractItem")]
    public partial class t_bp_ContractItem : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int ContractId { get; set; }
        [Required]
        public string ItemTableName { get; set; }
        public string ItemChName { get; set; }
        public string ItemType { get; set; }
        public string ItemStandard { get; set; }
    }

    [Alias("t_bp_CusAchievement")]
    public partial class t_bp_CusAchievement
    {
        [Required]
        public long id { get; set; }
        [Required]
        public string CustomId { get; set; }
        public string AchievementTime { get; set; }
        public string AchievementContent { get; set; }
        public string AchievementRem { get; set; }
    }

    [Alias("t_bp_CusAwards")]
    public partial class t_bp_CusAward
    {
        [Required]
        public int id { get; set; }
        public string CustomId { get; set; }
        public string AwaName { get; set; }
        public string AwaUnit { get; set; }
        public string AwaContent { get; set; }
        public DateTime? AwaDate { get; set; }
        public string AwaRem { get; set; }
    }

    [Alias("t_bp_CusChange")]
    public partial class t_bp_CusChange
    {
        [Required]
        public int id { get; set; }
        public string CustomId { get; set; }
        public string ChaContent { get; set; }
        public DateTime? ChaDate { get; set; }
        public string ChaAppUnit { get; set; }
        public string ChaRem { get; set; }
    }

    [Alias("t_bp_CusCheckParams")]
    public partial class t_bp_CusCheckParam
    {
        [Required]
        public long id { get; set; }
        [Required]
        public string customId { get; set; }
        public string topCheckType { get; set; }
        public string checkType { get; set; }
        public string checkItem { get; set; }
        public string checkParams { get; set; }
    }

    [Alias("t_bp_cuspost")]
    public partial class t_bp_cuspost
    {
        [Required]
        public int id { get; set; }
        public string posttype { get; set; }
        public int? parentid { get; set; }
        public string code { get; set; }
        public string postTypeTime { get; set; }
    }

    [Alias("t_bp_CusPunish")]
    public partial class t_bp_CusPunish
    {
        [Required]
        public int id { get; set; }
        public string CustomId { get; set; }
        public string PunName { get; set; }
        public string PunUnit { get; set; }
        public string PunContent { get; set; }
        public DateTime? PunDate { get; set; }
        public string PunRem { get; set; }
    }

    [Alias("t_bp_custom")]
    public partial class t_bp_custom
    {
        [Required]
        public string ID { get; set; }
        public string NAME { get; set; }
        public string STATIONID { get; set; }
        public string POSTCODE { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string ADDRESS { get; set; }
        public DateTime? CREATETIME { get; set; }
        public string EMAIL { get; set; }
        public string BUSINESSNUM { get; set; }
        public string BUSINESSNUMUNIT { get; set; }
        public string REGAPRICE { get; set; }
        public string ECONOMICNATURE { get; set; }
        public string MEASNUM { get; set; }
        public string MEASUNIT { get; set; }
        public string MEASNUMPATH { get; set; }
        public string FR { get; set; }
        public string JSNAME { get; set; }
        public string JSTIILE { get; set; }
        public string JSYEAR { get; set; }
        public string ZLNAME { get; set; }
        public string ZLTITLE { get; set; }
        public string ZLYEAR { get; set; }
        public string data_status { get; set; }
        public string PERCOUNT { get; set; }
        public string MIDPERCOUNT { get; set; }
        public string HEIPERCOUNT { get; set; }
        public string REGYTSTA { get; set; }
        public string REGJGSTA { get; set; }
        public string INSTRUMENTPRICE { get; set; }
        public string HOUSEAREA { get; set; }
        public string DETECTAREA { get; set; }
        public string DETECTTYPE { get; set; }
        public string DETECTNUM { get; set; }
        public DateTime? APPLDATE { get; set; }
        public string DETECTPATH { get; set; }
        public string QUAINFO { get; set; }
        public string APPROVALSTATUS { get; set; }
        public string ADDDATE { get; set; }
        public string phone { get; set; }
        public DateTime? detectnumStartDate { get; set; }
        public DateTime? detectnumEndDate { get; set; }
        public DateTime? measnumStartDate { get; set; }
        public DateTime? measnumEndDate { get; set; }
        public string hasNumPerCount { get; set; }
        public string instrumentNum { get; set; }
        public string businessnumPath { get; set; }
        public string approveadvice { get; set; }
        public string subunitnum { get; set; }
        public string issubunit { get; set; }
        public string supunitcode { get; set; }
        public string subunitdutyman { get; set; }
        public string area { get; set; }
        public string detectunit { get; set; }
        public DateTime? detectappldate { get; set; }
        public string shebaopeoplenum { get; set; }
        public string captial { get; set; }
        public string credit { get; set; }
        public string companytype { get; set; }
        public string floorarea { get; set; }
        public string yearplanproduce { get; set; }
        public string preyearproduce { get; set; }
        public string businesspermit { get; set; }
        public string businesspermitpath { get; set; }
        public string enterprisemanager { get; set; }
        public string financeman { get; set; }
        public string director { get; set; }
        public string cerfgrade { get; set; }
        public string cerfno { get; set; }
        public string cerfnopath { get; set; }
        public string sslcmj { get; set; }
        public string sslczk { get; set; }
        public string szssccnl { get; set; }
        public string fmhccnl { get; set; }
        public string chlccnl { get; set; }
        public string ytwjjccnl { get; set; }
        public string managercount { get; set; }
        public string jsglcount { get; set; }
        public string testcount { get; set; }
        public string sysarea { get; set; }
        public string yharea { get; set; }
        public string shebaopeoplelistpath { get; set; }
        public string workercount { get; set; }
        public string zgcount { get; set; }
        public string instrumentpath { get; set; }
        public int? datatype { get; set; }
        public string ispile { get; set; }
        public string NETADDRESS { get; set; }
        public string REGMONEYS { get; set; }
        public string PERP { get; set; }
        public string CMANUM { get; set; }
        public string CMAUNIT { get; set; }
        public string CMANUMCERF { get; set; }
        public string AVAILABILITYTIME { get; set; }
        public string GMANAGER { get; set; }
        public string GFA { get; set; }
        public string GFB { get; set; }
        public string TMANAGER { get; set; }
        public string TFA { get; set; }
        public string TFB { get; set; }
        public string ALLMANS { get; set; }
        public string TMANS { get; set; }
        public string MLEVELS { get; set; }
        public string HLEVELS { get; set; }
        public string EQUIPMENTS { get; set; }
        public string EQMONEYS { get; set; }
        public string WORKAREA { get; set; }
        public DateTime? CMANUMENDDATE { get; set; }
        public DateTime? CMAENDDATE { get; set; }
        public string USEENDDATE { get; set; }
        public string SELECTTEL { get; set; }
        public string APPEALTEL { get; set; }
        public string APPEALEMAIL { get; set; }
        public string zzlbgs { get; set; }
        public string zzxmgs { get; set; }
        public string zzcsgs { get; set; }
        public string certCode { get; set; }
        public string zzlbmc { get; set; }
        public string wjlr { get; set; }
        public int IsUse { get; set; }
        public string bgfs { get; set; }
        public DateTime? measnumstarttime { get; set; }
        public DateTime? detectnumstarttime { get; set; }
        public DateTime? djtime { get; set; }
        public DateTime? update_time { get; set; }
        public int ParentId { get; set; }
        public string EquclassId { get; set; }
    }
    /// <summary>
    /// 创建机构临时表 add by ydf 2019-04-03
    /// </summary>
    [Alias("t_bp_custom_tmp")]
    public partial class t_bp_custom_tmp
    {
        [Required]
        public string ID { get; set; }
        public string NAME { get; set; }
        public string STATIONID { get; set; }
        public string POSTCODE { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string ADDRESS { get; set; }
        public DateTime? CREATETIME { get; set; }
        public string EMAIL { get; set; }
        public string BUSINESSNUM { get; set; }
        public string BUSINESSNUMUNIT { get; set; }
        public string REGAPRICE { get; set; }
        public string ECONOMICNATURE { get; set; }
        public string MEASNUM { get; set; }
        public string MEASUNIT { get; set; }
        public string MEASNUMPATH { get; set; }
        public string FR { get; set; }
        public string JSNAME { get; set; }
        public string JSTIILE { get; set; }
        public string JSYEAR { get; set; }
        public string ZLNAME { get; set; }
        public string ZLTITLE { get; set; }
        public string ZLYEAR { get; set; }
        public string data_status { get; set; }
        public string PERCOUNT { get; set; }
        public string MIDPERCOUNT { get; set; }
        public string HEIPERCOUNT { get; set; }
        public string REGYTSTA { get; set; }
        public string REGJGSTA { get; set; }
        public string INSTRUMENTPRICE { get; set; }
        public string HOUSEAREA { get; set; }
        public string DETECTAREA { get; set; }
        public string DETECTTYPE { get; set; }
        public string DETECTNUM { get; set; }
        public DateTime? APPLDATE { get; set; }
        public string DETECTPATH { get; set; }
        public string QUAINFO { get; set; }
        public string APPROVALSTATUS { get; set; }
        public string ADDDATE { get; set; }
        public string phone { get; set; }
        public DateTime? detectnumStartDate { get; set; }
        public DateTime? detectnumEndDate { get; set; }
        public DateTime? measnumStartDate { get; set; }
        public DateTime? measnumEndDate { get; set; }
        public string hasNumPerCount { get; set; }
        public string instrumentNum { get; set; }
        public string businessnumPath { get; set; }
        public string approveadvice { get; set; }
        public string subunitnum { get; set; }
        public string issubunit { get; set; }
        public string supunitcode { get; set; }
        public string subunitdutyman { get; set; }
        public string area { get; set; }
        public string detectunit { get; set; }
        public DateTime? detectappldate { get; set; }
        public string shebaopeoplenum { get; set; }
        public string captial { get; set; }
        public string credit { get; set; }
        public string companytype { get; set; }
        public string floorarea { get; set; }
        public string yearplanproduce { get; set; }
        public string preyearproduce { get; set; }
        public string businesspermit { get; set; }
        public string businesspermitpath { get; set; }
        public string enterprisemanager { get; set; }
        public string financeman { get; set; }
        public string director { get; set; }
        public string cerfgrade { get; set; }
        public string cerfno { get; set; }
        public string cerfnopath { get; set; }
        public string sslcmj { get; set; }
        public string sslczk { get; set; }
        public string szssccnl { get; set; }
        public string fmhccnl { get; set; }
        public string chlccnl { get; set; }
        public string ytwjjccnl { get; set; }
        public string managercount { get; set; }
        public string jsglcount { get; set; }
        public string testcount { get; set; }
        public string sysarea { get; set; }
        public string yharea { get; set; }
        public string shebaopeoplelistpath { get; set; }
        public string workercount { get; set; }
        public string zgcount { get; set; }
        public string instrumentpath { get; set; }
        public int? datatype { get; set; }
        public string ispile { get; set; }
        public string NETADDRESS { get; set; }
        public string REGMONEYS { get; set; }
        public string PERP { get; set; }
        public string CMANUM { get; set; }
        public string CMAUNIT { get; set; }
        public string CMANUMCERF { get; set; }
        public string AVAILABILITYTIME { get; set; }
        public string GMANAGER { get; set; }
        public string GFA { get; set; }
        public string GFB { get; set; }
        public string TMANAGER { get; set; }
        public string TFA { get; set; }
        public string TFB { get; set; }
        public string ALLMANS { get; set; }
        public string TMANS { get; set; }
        public string MLEVELS { get; set; }
        public string HLEVELS { get; set; }
        public string EQUIPMENTS { get; set; }
        public string EQMONEYS { get; set; }
        public string WORKAREA { get; set; }
        public DateTime? CMANUMENDDATE { get; set; }
        public DateTime? CMAENDDATE { get; set; }
        public string USEENDDATE { get; set; }
        public string SELECTTEL { get; set; }
        public string APPEALTEL { get; set; }
        public string APPEALEMAIL { get; set; }
        public string zzlbgs { get; set; }
        public string zzxmgs { get; set; }
        public string zzcsgs { get; set; }
        public string certCode { get; set; }
        public string zzlbmc { get; set; }
        public string wjlr { get; set; }
        public int IsUse { get; set; }
        public string bgfs { get; set; }
        public DateTime? measnumstarttime { get; set; }
        public DateTime? detectnumstarttime { get; set; }
        public DateTime? djtime { get; set; }
        public DateTime? update_time { get; set; }
        public string EquclassId { get; set; }
    }

    [Alias("t_bp_custom_apply")]
    public partial class t_bp_custom_apply
    {
        [Required]
        public string ID { get; set; }
        public string NAME { get; set; }
        public string STATIONID { get; set; }
        public string POSTCODE { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string ADDRESS { get; set; }
        public DateTime? CREATETIME { get; set; }
        public string EMAIL { get; set; }
        public string BUSINESSNUM { get; set; }
        public string BUSINESSNUMUNIT { get; set; }
        public string REGAPRICE { get; set; }
        public string ECONOMICNATURE { get; set; }
        public string MEASNUM { get; set; }
        public string MEASUNIT { get; set; }
        public string MEASNUMPATH { get; set; }
        public string FR { get; set; }
        public string JSNAME { get; set; }
        public string JSTIILE { get; set; }
        public string JSYEAR { get; set; }
        public string ZLNAME { get; set; }
        public string ZLTITLE { get; set; }
        public string ZLYEAR { get; set; }
        public string data_status { get; set; }
        public string PERCOUNT { get; set; }
        public string MIDPERCOUNT { get; set; }
        public string HEIPERCOUNT { get; set; }
        public string REGYTSTA { get; set; }
        public string REGJGSTA { get; set; }
        public string INSTRUMENTPRICE { get; set; }
        public string HOUSEAREA { get; set; }
        public string DETECTAREA { get; set; }
        public string DETECTTYPE { get; set; }
        public string DETECTNUM { get; set; }
        public DateTime? APPLDATE { get; set; }
        public string DETECTPATH { get; set; }
        public string QUAINFO { get; set; }
        public string APPROVALSTATUS { get; set; }
        public string ADDDATE { get; set; }
        public string phone { get; set; }
        public DateTime? detectnumStartDate { get; set; }
        public DateTime? detectnumEndDate { get; set; }
        public DateTime? measnumStartDate { get; set; }
        public DateTime? measnumEndDate { get; set; }
        public string hasNumPerCount { get; set; }
        public string instrumentNum { get; set; }
        public string businessnumPath { get; set; }
        public string approveadvice { get; set; }
        public string subunitnum { get; set; }
        public string issubunit { get; set; }
        public string supunitcode { get; set; }
        public string subunitdutyman { get; set; }
        public string area { get; set; }
        public string detectunit { get; set; }
        public DateTime? detectappldate { get; set; }
        public string shebaopeoplenum { get; set; }
        public string captial { get; set; }
        public string credit { get; set; }
        public string companytype { get; set; }
        public string floorarea { get; set; }
        public string yearplanproduce { get; set; }
        public string preyearproduce { get; set; }
        public string businesspermit { get; set; }
        public string businesspermitpath { get; set; }
        public string enterprisemanager { get; set; }
        public string financeman { get; set; }
        public string director { get; set; }
        public string cerfgrade { get; set; }
        public string cerfno { get; set; }
        public string cerfnopath { get; set; }
        public string sslcmj { get; set; }
        public string sslczk { get; set; }
        public string szssccnl { get; set; }
        public string fmhccnl { get; set; }
        public string chlccnl { get; set; }
        public string ytwjjccnl { get; set; }
        public string managercount { get; set; }
        public string jsglcount { get; set; }
        public string testcount { get; set; }
        public string sysarea { get; set; }
        public string yharea { get; set; }
        public string shebaopeoplelistpath { get; set; }
        public string workercount { get; set; }
        public string zgcount { get; set; }
        public string instrumentpath { get; set; }
        public int? datatype { get; set; }
        public string ispile { get; set; }
        public string NETADDRESS { get; set; }
        public string REGMONEYS { get; set; }
        public string PERP { get; set; }
        public string CMANUM { get; set; }
        public string CMAUNIT { get; set; }
        public string CMANUMCERF { get; set; }
        public string AVAILABILITYTIME { get; set; }
        public string GMANAGER { get; set; }
        public string GFA { get; set; }
        public string GFB { get; set; }
        public string TMANAGER { get; set; }
        public string TFA { get; set; }
        public string TFB { get; set; }
        public string ALLMANS { get; set; }
        public string TMANS { get; set; }
        public string MLEVELS { get; set; }
        public string HLEVELS { get; set; }
        public string EQUIPMENTS { get; set; }
        public string EQMONEYS { get; set; }
        public string WORKAREA { get; set; }
        public DateTime? CMANUMENDDATE { get; set; }
        public DateTime? CMAENDDATE { get; set; }
        public string USEENDDATE { get; set; }
        public string SELECTTEL { get; set; }
        public string APPEALTEL { get; set; }
        public string APPEALEMAIL { get; set; }
        public string zzlbgs { get; set; }
        public string zzxmgs { get; set; }
        public string zzcsgs { get; set; }
        public string certCode { get; set; }
        public string zzlbmc { get; set; }
        public string wjlr { get; set; }
        public int IsUse { get; set; }
        public string bgfs { get; set; }
        public DateTime? measnumstarttime { get; set; }
        public DateTime? detectnumstarttime { get; set; }
        public DateTime? djtime { get; set; }
        public DateTime? update_time { get; set; }

        public string SQname { get; set; }
        public string SQTel { get; set; }
        public string sqjcyw { get; set; }

        public string FRSex { get; set; }
        public string FRBirth { get; set; }
        public string FRTITLE { get; set; }
        public string FRYEAR { get; set; }
        public string FRGraduationTime { get; set; }
        public string FRCollege { get; set; }
        public string FREducation { get; set; }
        public string FRSubject { get; set; }
        public string FRTel { get; set; }
        public string FRMobile { get; set; }
        public string frgzjl { get; set; }

        public string JSSex { get; set; }
        public string JSBirth { get; set; }
        public string JSGraduationTime { get; set; }
        public string JSCollege { get; set; }
        public string JSEducation { get; set; }
        public string JSSubject { get; set; }
        public string JSTel { get; set; }
        public string JSMobile { get; set; }
        public string jsgzjl { get; set; }

        public string bgcszmPath { get; set; }
        public string zxzmPath { get; set; }
        public string gqzmPath { get; set; }
        public string zzscwjPath { get; set; }
        public string glscwjPath { get; set; }

        public string frzcpath { get; set; }
        public string frxlpath { get; set; }
        public string jszcpath { get; set; }
        public string jsxlpath { get; set; }
    }

    [Alias("t_bp_customYC")]
    public partial class t_bp_customYC
    {
        [Required]
        public string ID { get; set; }
        public string NAME { get; set; }
        public string STATIONID { get; set; }
        public string POSTCODE { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string ADDRESS { get; set; }
        public DateTime? CREATETIME { get; set; }
        public string EMAIL { get; set; }
        public string BUSINESSNUM { get; set; }
        public string BUSINESSNUMUNIT { get; set; }
        public string REGAPRICE { get; set; }
        public string ECONOMICNATURE { get; set; }
        public string MEASNUM { get; set; }
        public string MEASUNIT { get; set; }
        public string MEASNUMPATH { get; set; }
        public string FR { get; set; }
        public string JSNAME { get; set; }
        public string JSTIILE { get; set; }
        public string JSYEAR { get; set; }
        public string ZLNAME { get; set; }
        public string ZLTITLE { get; set; }
        public string ZLYEAR { get; set; }
        public string PERCOUNT { get; set; }
        public string MIDPERCOUNT { get; set; }
        public string HEIPERCOUNT { get; set; }
        public string REGYTSTA { get; set; }
        public string REGJGSTA { get; set; }
        public string INSTRUMENTPRICE { get; set; }
        public string HOUSEAREA { get; set; }
        public string DETECTAREA { get; set; }
        public string DETECTTYPE { get; set; }
        public string DETECTNUM { get; set; }
        public DateTime? APPLDATE { get; set; }
        public string DETECTPATH { get; set; }
        public string QUAINFO { get; set; }
        public string APPROVALSTATUS { get; set; }
        public string ADDDATE { get; set; }
        public string phone { get; set; }
        public DateTime? detectnumStartDate { get; set; }
        public DateTime? detectnumEndDate { get; set; }
        public DateTime? measnumStartDate { get; set; }
        public DateTime? measnumEndDate { get; set; }
        public string hasNumPerCount { get; set; }
        public string instrumentNum { get; set; }
        public string businessnumPath { get; set; }
        public string approveadvice { get; set; }
        public string subunitnum { get; set; }
        public string issubunit { get; set; }
        public string supunitcode { get; set; }
        public string subunitdutyman { get; set; }
        public string area { get; set; }
        public string detectunit { get; set; }
        public DateTime? detectappldate { get; set; }
        public string shebaopeoplenum { get; set; }
        public string captial { get; set; }
        public string credit { get; set; }
        public string companytype { get; set; }
        public string floorarea { get; set; }
        public string yearplanproduce { get; set; }
        public string preyearproduce { get; set; }
        public string businesspermit { get; set; }
        public string businesspermitpath { get; set; }
        public string enterprisemanager { get; set; }
        public string financeman { get; set; }
        public string director { get; set; }
        public string cerfgrade { get; set; }
        public string cerfno { get; set; }
        public string cerfnopath { get; set; }
        public string sslcmj { get; set; }
        public string sslczk { get; set; }
        public string szssccnl { get; set; }
        public string fmhccnl { get; set; }
        public string chlccnl { get; set; }
        public string ytwjjccnl { get; set; }
        public string managercount { get; set; }
        public string jsglcount { get; set; }
        public string testcount { get; set; }
        public string sysarea { get; set; }
        public string yharea { get; set; }
        public string shebaopeoplelistpath { get; set; }
        public string workercount { get; set; }
        public string zgcount { get; set; }
        public string instrumentpath { get; set; }
        public int? datatype { get; set; }
        public string ispile { get; set; }
        public string NETADDRESS { get; set; }
        public string REGMONEYS { get; set; }
        public string PERP { get; set; }
        public string CMANUM { get; set; }
        public string CMAUNIT { get; set; }
        public string CMANUMCERF { get; set; }
        public string AVAILABILITYTIME { get; set; }
        public string GMANAGER { get; set; }
        public string GFA { get; set; }
        public string GFB { get; set; }
        public string TMANAGER { get; set; }
        public string TFA { get; set; }
        public string TFB { get; set; }
        public string ALLMANS { get; set; }
        public string TMANS { get; set; }
        public string MLEVELS { get; set; }
        public string HLEVELS { get; set; }
        public string EQUIPMENTS { get; set; }
        public string EQMONEYS { get; set; }
        public string WORKAREA { get; set; }
        public DateTime? CMANUMENDDATE { get; set; }
        public DateTime? CMAENDDATE { get; set; }
        public string USEENDDATE { get; set; }
        public string SELECTTEL { get; set; }
        public string APPEALTEL { get; set; }
        public string APPEALEMAIL { get; set; }
        public string zzlbgs { get; set; }
        public string zzxmgs { get; set; }
        public string zzcsgs { get; set; }
        public string certCode { get; set; }
        public string zzlbmc { get; set; }
        public string wjlr { get; set; }
        public DateTime? measnumstarttime { get; set; }
        public DateTime? detectnumstarttime { get; set; }
        public DateTime? djtime { get; set; }
    }

    [Alias("t_bp_department")]
    public partial class t_bp_department
    {
        [Required]
        public string DEPARTNUM { get; set; }
        [Required]
        public string UNITCODE { get; set; }
        public string DEPARTNAME { get; set; }
        public string FULLNAME { get; set; }
        public string DEPARTFMAN { get; set; }
        public string DEPARTTEL { get; set; }
    }

    [Alias("t_bp_deptip")]
    public partial class t_bp_deptip
    {
        [Required]
        public string DEPARTNUM { get; set; }
        [Required]
        public string ITEMLIST { get; set; }
    }

    [Alias("t_bp_Equipment")]
    public partial class t_bp_Equipment
    {
        [Required]
        public int id { get; set; }
        public string customid { get; set; }
        public string EquName { get; set; }
        public string checkitem { get; set; }
        public string equtype { get; set; }
        public string equspec { get; set; }
        public string testrange { get; set; }
        public string degree { get; set; }
        public string uncertainty { get; set; }
        public string checkunit { get; set; }
        public string repairunit { get; set; }
        public string checkcerfnum { get; set; }
        public string repaircerfnum { get; set; }
        public string checkcerfnumpath { get; set; }
        public string repaircerfnumpath { get; set; }
        public DateTime? checkstartdate { get; set; }
        public DateTime? checkenddate { get; set; }
        public DateTime? repairstartdate { get; set; }
        public DateTime? repairenddate { get; set; }
        public string selfcheckitem { get; set; }
        public string selfrepairitem { get; set; }
        public string selfcheckstandardname { get; set; }
        public string selfchecknum { get; set; }
        public string selfrepairstandardname { get; set; }
        public string selfrepairnum { get; set; }
        public string explain { get; set; }
        public string isautoacs { get; set; }
        public string autoacsprovider { get; set; }
        public string approvalstatus { get; set; }
        public string approveadvice { get; set; }
        public string supcustomid { get; set; }
        public string equnum { get; set; }
        public string equclass { get; set; }
        public DateTime? buytime { get; set; }
        public DateTime? timestart { get; set; }
        public DateTime? timeend { get; set; }
        public string sysarea { get; set; }
        public string yharea { get; set; }
        public DateTime? checktime { get; set; }
        public string ispile { get; set; }
        public DateTime? djtime { get; set; }
        public string equclassId { get; set; }
    }

    [Alias("t_bp_Equipment_tmp")]
    public partial class t_bp_Equipment_tmp
    {
        [Required]
        public int id { get; set; }
        public string customid { get; set; }
        public string EquName { get; set; }
        public string checkitem { get; set; }
        public string equtype { get; set; }
        public string equspec { get; set; }
        public string testrange { get; set; }
        public string degree { get; set; }
        public string uncertainty { get; set; }
        public string checkunit { get; set; }
        public string repairunit { get; set; }
        public string checkcerfnum { get; set; }
        public string repaircerfnum { get; set; }
        public string checkcerfnumpath { get; set; }
        public string repaircerfnumpath { get; set; }
        public DateTime? checkstartdate { get; set; }
        public DateTime? checkenddate { get; set; }
        public DateTime? repairstartdate { get; set; }
        public DateTime? repairenddate { get; set; }
        public string selfcheckitem { get; set; }
        public string selfrepairitem { get; set; }
        public string selfcheckstandardname { get; set; }
        public string selfchecknum { get; set; }
        public string selfrepairstandardname { get; set; }
        public string selfrepairnum { get; set; }
        public string explain { get; set; }
        public string isautoacs { get; set; }
        public string autoacsprovider { get; set; }
        public string approvalstatus { get; set; }
        public string approveadvice { get; set; }
        public string supcustomid { get; set; }
        public string equnum { get; set; }
        public string equclass { get; set; }
        public DateTime? buytime { get; set; }
        public DateTime? timestart { get; set; }
        public DateTime? timeend { get; set; }
        public string sysarea { get; set; }
        public string yharea { get; set; }
        public DateTime? checktime { get; set; }
        public string ispile { get; set; }
        public DateTime? djtime { get; set; }
        public string equclassId { get; set; }
    }

    [Alias("t_bp_equItemSubItemList")]
    public partial class t_bp_equItemSubItemList
    {
        [Required]
        public long id { get; set; }
        [Required]
        public int equId { get; set; }
        public string itemTableName { get; set; }
        public string subItemList { get; set; }
        public string subItemCodeList { get; set; }
        public string itemChName { get; set; }
    }

    [Alias("t_bp_EquipmentTypeList")]
    public partial class t_bp_EquipmentTypeList
    {
        [Required]
        public string typcode { get; set; }
        public string typename { get; set; }
        [Required]
        public string itemcode { get; set; }
        public string itemname { get; set; }
        [Required]
        public string parmcode { get; set; }
        public string parmname { get; set; }

    }

    [Alias("t_bp_HolderPeople")]
    public partial class t_bp_HolderPerson
    {
        [Required]
        public int id { get; set; }
        public string customid { get; set; }
        public int? peopleid { get; set; }
        public string PeoName { get; set; }
        public string CerNum { get; set; }
        public DateTime? CerYear { get; set; }
    }

    [Alias("t_bp_infoModifyRequest")]
    public partial class t_bp_infoModifyRequest
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string customId { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string requestMan { get; set; }
        public DateTime? requestTime { get; set; }
        public string approveMan { get; set; }
        public DateTime? approveTime { get; set; }
        public int? isApprove { get; set; }
        public string approveAdvice { get; set; }
        public string reason { get; set; }
        [Required]
        public string modifykey { get; set; }
        public string approvalstatus { get; set; }
    }

    [Alias("t_bp_itemcolumn")]
    public partial class t_bp_itemcolumn
    {
        [Required]
        public string itemTableName { get; set; }
        [Required]
        public string columnName { get; set; }
        public string columnType { get; set; }
        public string columnChName { get; set; }
        public int? columnOrder { get; set; }
        public string columnTypes { get; set; }
    }

    [Alias("t_bp_itemlist")]
    public partial class t_bp_itemlist
    {
        [Required]
        public string UNITCODE { get; set; }
        [Required]
        public string ITEMTABLENAME { get; set; }
        public string ITEMCHNAME { get; set; }
        public string ITEMTYPES { get; set; }
        public string ITEMSTANDARDS { get; set; }
        public int? REPORTCONLINES { get; set; }
        public int? RECORDCONLINES { get; set; }
        public int? ENTRFORMCONLINES { get; set; }
        public int? REPORTORGI { get; set; }
        public int? RECORDORGI { get; set; }
        public string TESTEQUIPMENTNUM { get; set; }
        public string TESTEQUIPMENTNAME { get; set; }
        public int? REPORTCONSENTDAYS { get; set; }
        public int? REPORTCONSENTCWORKDAYS { get; set; }
        public string KEYS { get; set; }
        public int? USEFREQ { get; set; }
        public int? CHECKMANCOUNT { get; set; }
        public string PRINTDATEMODE { get; set; }
        public string STANDARDNAMEMODE { get; set; }
        public int? ISHAVEGJ { get; set; }
        public int? ONLYMANAGEREPORT { get; set; }
        public int? CHECKDATESTYLE { get; set; }
        public string ISUSE { get; set; }
        public int? CHECKDAYS { get; set; }
        public int? REPORTDAYS { get; set; }
        public string JX_LBDM { get; set; }
        public string JX_ITEMCODE { get; set; }
        public string JX_LB { get; set; }
        public string JX_ITEMCHNAME { get; set; }
        public string JGCODE { get; set; }
        public string HEBEI_ITEMCODE { get; set; }
        public string HEBEI_TABLENAME { get; set; }
        public string HEBEI_ITEMCHNAME { get; set; }
        public DateTime? STARTTIME { get; set; }
        public DateTime? VALIDDATE { get; set; }
        public string PDCOLUMNS { get; set; }
        public string BGSKBH { get; set; }
        public string JLSKBH { get; set; }
        public string WTSKBH { get; set; }
        public string PKPMSOFTVERSION { get; set; }
    }

    [Alias("t_bp_itemlistTwo")]
    public partial class t_bp_itemlistTwo
    {
        [Required]
        public string UNITCODE { get; set; }
        [Required]
        public string ITEMTABLENAME { get; set; }
        public string ITEMCHNAME { get; set; }
        public string ITEMTYPES { get; set; }
        public string ITEMSTANDARDS { get; set; }
        public int? REPORTCONLINES { get; set; }
        public int? RECORDCONLINES { get; set; }
        public int? ENTRFORMCONLINES { get; set; }
        public int? REPORTORGI { get; set; }
        public int? RECORDORGI { get; set; }
        public string TESTEQUIPMENTNUM { get; set; }
        public string TESTEQUIPMENTNAME { get; set; }
        public int? REPORTCONSENTDAYS { get; set; }
        public int? REPORTCONSENTCWORKDAYS { get; set; }
        public string KEYS { get; set; }
        public int? USEFREQ { get; set; }
        public int? CHECKMANCOUNT { get; set; }
        public string PRINTDATEMODE { get; set; }
        public string STANDARDNAMEMODE { get; set; }
        public int? ISHAVEGJ { get; set; }
        public int? ONLYMANAGEREPORT { get; set; }
        public int? CHECKDATESTYLE { get; set; }
        public string ISUSE { get; set; }
        public int? CHECKDAYS { get; set; }
        public int? REPORTDAYS { get; set; }
        public string JX_LBDM { get; set; }
        public string JX_ITEMCODE { get; set; }
        public string JX_LB { get; set; }
        public string JX_ITEMCHNAME { get; set; }
        public string JGCODE { get; set; }
        public string HEBEI_ITEMCODE { get; set; }
        public string HEBEI_TABLENAME { get; set; }
        public string HEBEI_ITEMCHNAME { get; set; }
        public DateTime? STARTTIME { get; set; }
        public DateTime? VALIDDATE { get; set; }
        public string PDCOLUMNS { get; set; }
        public string BGSKBH { get; set; }
        public string JLSKBH { get; set; }
        public string WTSKBH { get; set; }
        public string PKPMSOFTVERSION { get; set; }
    }

    [Alias("t_bp_maintanTable")]
    public partial class t_bp_maintanTable
    {
        [Required]
        public string tableSource { get; set; }
        public string tableLocking { get; set; }
        [Required]
        public string tableWorking { get; set; }
        public string tableLocking1 { get; set; }
        public string tableWorking1 { get; set; }
    }

    [Alias("t_bp_notice")]
    public partial class t_bp_notice
    {
        [Required]
        public long id { get; set; }
        public string noticeTitle { get; set; }
        public string noticeContent { get; set; }
        public DateTime? noticeTime { get; set; }
        public int? looktimes { get; set; }
        public string relativeTableName { get; set; }
        public string relativePrimaryKeyValue { get; set; }
    }

    [Alias("t_bp_noticeAttach")]
    public partial class t_bp_noticeAttach
    {
        [Required]
        public long id { get; set; }
        public string filePath { get; set; }
        [Required]
        public long noticeId { get; set; }
        public string fileName { get; set; }
    }

    [Alias("t_bp_operationLog")]
    public partial class t_bp_operationLog
    {
        [Required]
        public long id { get; set; }
        public string operationUserUnitName { get; set; }
        public string operationUser { get; set; }
        public string operationIP { get; set; }
        public string operationContent { get; set; }
        public string operationDataTable { get; set; }
        public string operationDataCondition { get; set; }
        public string operationUrl { get; set; }
        public DateTime? operationTime { get; set; }
    }

    [Alias("t_bp_PeoAwards")]
    public partial class t_bp_PeoAward
    {
        [Required]
        public int id { get; set; }
        public int? PeopleId { get; set; }
        public string AwaContent { get; set; }
        public string AwaUnit { get; set; }
        public DateTime? AwaDate { get; set; }
    }

    [Alias("t_bp_PeoChange")]
    public partial class t_bp_PeoChange
    {
        [Required]
        public int id { get; set; }
        public int? PeopleId { get; set; }
        public string ChaContent { get; set; }
        public DateTime? ChaDate { get; set; }
    }

    [Alias("t_bp_PeoEducation")]
    public partial class t_bp_PeoEducation
    {
        [Required]
        public int id { get; set; }
        public int? PeopleId { get; set; }
        public string TrainContent { get; set; }
        public DateTime? TrainDate { get; set; }
        public string TrainUnit { get; set; }
        public DateTime? TestDate { get; set; }
        public string TestResult { get; set; }
    }

    [Alias("t_bp_People")]
    public partial class t_bp_People
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public string Customid { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string PhotoPath { get; set; }
        public string SelfNum { get; set; }
        public string Education { get; set; }
        public string School { get; set; }
        public string Professional { get; set; }
        public string Title { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string SBNum { get; set; }
        public string PostType { get; set; }
        public string PostNum { get; set; }
        public string PostPath { get; set; }
        public DateTime? PostDate { get; set; }
        public string PosrPeople { get; set; }
        public string PostIs { get; set; }
        public string Approvalstatus { get; set; }
        public string postDelayReg { get; set; }
        public string selfnumPath { get; set; }
        public DateTime? postnumstartdate { get; set; }
        public DateTime? postnumenddate { get; set; }
        public string approveadvice { get; set; }
        public int? age { get; set; }
        public string postTypeCode { get; set; }
        public string titlepath { get; set; }
        public string zw { get; set; }
        public string iszcgccs { get; set; }
        public string zcgccszh { get; set; }
        public string zcgccszhpath { get; set; }
        public string educationpath { get; set; }
        public DateTime? zcgccszhstartdate { get; set; }
        public DateTime? zcgccszhenddate { get; set; }
        public string isreghere { get; set; }
        public string iscb { get; set; }
        public string ismanager { get; set; }
        public string isjs { get; set; }
        public string issy { get; set; }
        public DateTime? posttime { get; set; }
        public string postname { get; set; }
        public string ispostvalid { get; set; }
        public string ishaspostnum { get; set; }
        public string titleother { get; set; }
        public string sbnumpath { get; set; }
        public string nameshouzimu { get; set; }
        public string ispile { get; set; }
        public DateTime? dzsj { get; set; }
        public DateTime? djtime { get; set; }
        public string data_status { get; set; }
        public DateTime? update_time { get; set; }
        public int IsUse { get; set; }
        public string gznx { get; set; }
    }

    [Alias("t_bp_People_tmp")]
    public partial class t_bp_People_tmp
    {
        [Required]
        public int id { get; set; }
        public string Customid { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string PhotoPath { get; set; }
        public string SelfNum { get; set; }
        public string Education { get; set; }
        public string School { get; set; }
        public string Professional { get; set; }
        public string Title { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string SBNum { get; set; }
        public string PostType { get; set; }
        public string PostNum { get; set; }
        public string PostPath { get; set; }
        public DateTime? PostDate { get; set; }
        public string PosrPeople { get; set; }
        public string PostIs { get; set; }
        public string Approvalstatus { get; set; }
        public string postDelayReg { get; set; }
        public string selfnumPath { get; set; }
        public DateTime? postnumstartdate { get; set; }
        public DateTime? postnumenddate { get; set; }
        public string approveadvice { get; set; }
        public int? age { get; set; }
        public string postTypeCode { get; set; }
        public string titlepath { get; set; }
        public string zw { get; set; }
        public string iszcgccs { get; set; }
        public string zcgccszh { get; set; }
        public string zcgccszhpath { get; set; }
        public string educationpath { get; set; }
        public DateTime? zcgccszhstartdate { get; set; }
        public DateTime? zcgccszhenddate { get; set; }
        public string isreghere { get; set; }
        public string iscb { get; set; }
        public string ismanager { get; set; }
        public string isjs { get; set; }
        public string issy { get; set; }
        public DateTime? posttime { get; set; }
        public string postname { get; set; }
        public string ispostvalid { get; set; }
        public string ishaspostnum { get; set; }
        public string titleother { get; set; }
        public string sbnumpath { get; set; }
        public string nameshouzimu { get; set; }
        public string ispile { get; set; }
        public DateTime? dzsj { get; set; }
        public DateTime? djtime { get; set; }
        public string data_status { get; set; }
        public DateTime? update_time { get; set; }
        public int IsUse { get; set; }
        public string gznx { get; set; }
    }



    [Alias("t_bp_PeoPunish")]
    public partial class t_bp_PeoPunish
    {
        [Required]
        public int id { get; set; }
        public int? PeopleId { get; set; }
        public string PunName { get; set; }
        public string PunUnit { get; set; }
        public string PunContent { get; set; }
        public DateTime? PunDate { get; set; }
    }

    [Alias("t_bp_postType")]
    public partial class t_bp_postType
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string postType { get; set; }
        [Required]
        public string parentId { get; set; }
        public string code { get; set; }
        public string postTypeTime { get; set; }
    }

    [Alias("t_bp_postTypeName")]
    public partial class t_bp_postTypeName
    {
        [Required]
        public string id { get; set; }
        [Required]
        public string postTypeName { get; set; }
    }

    [Alias("t_bp_project")]
    public partial class t_bp_project
    {
        [Required]
        public string PROJECTNUM { get; set; }
        public string UNITCODE { get; set; }
        public string PROJECTNAME { get; set; }
        public string ENTRUSTUNIT { get; set; }
        public string SENDSAMPLEMAN { get; set; }
        public string SENDSAMPLEMANTEL { get; set; }
        public string ENTRUSTUNITLINKMAN { get; set; }
        public string ENTRUSTUNITLINKMANTEL { get; set; }
        public string CONSTRACTUNIT { get; set; }
        public string CONSTRACTIONUNIT { get; set; }
        public string WITNESSUNIT { get; set; }
        public string WITNESSMAN { get; set; }
        public string WITNESSMANNUM { get; set; }
        public string WITNESSMANTEL { get; set; }
        public string SUPERUNIT { get; set; }
        public string SUPERMAN { get; set; }
        public string DESIGNUNIT { get; set; }
        public string INSPECTUNIT { get; set; }
        public string INSPECTMAN { get; set; }
        public string INVESTIGATEUNIT { get; set; }
        public string TAKESAMPLEMAN { get; set; }
        public string TAKESAMPLEMANNUM { get; set; }
        public string TAKESAMPLEMANTEL { get; set; }
        public string TAKESAMPLEUNIT { get; set; }
        public string PROJECTADDRESS { get; set; }
        public string PROJECTAREA { get; set; }
        public string PROJECTSTATUS { get; set; }
        public string PROJECTCHARGETYPE { get; set; }
        public DateTime? CREATEDATE { get; set; }
        public DateTime? DESTROYDATE { get; set; }
        public string STRUTTYPE { get; set; }
        public string PROJECTID { get; set; }
        public string CONTRACTNUM { get; set; }
        public string EMAIL { get; set; }
        public string AUTOSENDREPORT { get; set; }
        public string CREATEMAN { get; set; }
        public double? ACCOUNTBALANCE { get; set; }
        public double? TOTALCONSUMEDMONEY { get; set; }
        public double? TOTALFAVOURABLEMONEY { get; set; }
        public string UNITCREDITLEVEL { get; set; }
        public string DEFAULTCONSUMETYPE { get; set; }
        public string ISUSERICCARD { get; set; }
        public string ENTRUSTUNITADDRESS { get; set; }
        public string ENTRUSTUNITACCOUNTNUM { get; set; }
        public string ENTRUSTUNITNUM { get; set; }
        public string CONSTRACTUNITID { get; set; }
        public string DESIGNUNITID { get; set; }
        public string INVESTIGATEUNITID { get; set; }
        public string SUPERUNITID { get; set; }
        public string CONSTRACTIONUNITID { get; set; }
        public string INSPECTUNITID { get; set; }
        public string AREAID { get; set; }
        public string ENTRUSTUNITID { get; set; }
        public string TAKESAMPLEUNITID { get; set; }
        public string WITNESSUNITID { get; set; }
        public string SUPERMANID { get; set; }
        public string INSPECTMANID { get; set; }
        public string SUPERMANTEL { get; set; }
        public string INSPECTMANTEL { get; set; }
        public string GCBH { get; set; }
        public string CONSTRACTUNITMANID { get; set; }
        public string CONSTRACTUNITMAN { get; set; }
        public string CONSTRACTUNITMANTEL { get; set; }
        public string CONSTRACTIONUNITMANID { get; set; }
        public string CONSTRACTIONUNITMAN { get; set; }
        public string CONSTRACTIONUNITMANTEL { get; set; }
        public string DESIGNUNITMANID { get; set; }
        public string DESIGNUNITMAN { get; set; }
        public string DESIGNUNITMANTEL { get; set; }
        public string INVESTIGATEUNITMANID { get; set; }
        public string INVESTIGATEUNITMAN { get; set; }
        public string INVESTIGATEUNITMANTEL { get; set; }
        public string ANQUANREGID { get; set; }
        public DateTime? PLANENDDATE { get; set; }
        public DateTime? PLANSTARTDATE { get; set; }
        public double? PROTECTSQUARE { get; set; }
        public double? SQUARE { get; set; }
        public string STRUCTLEVLES { get; set; }
        public double? TOTALPRICE { get; set; }
        public string SENDSAMPLEMANNUM { get; set; }
        public string JGLX { get; set; }
        public DateTime? CHECKTIME { get; set; }
        public string SUPERMANTEL1 { get; set; }
        public string FPROJECTNAME { get; set; }
        public string JX_PROJECTID { get; set; }
        public string JX_ITEMID { get; set; }
        public string INSPECTUNITNUM { get; set; }
        public string CONTRACTIONPERMITNUM { get; set; }
        public string INSPECTPROJECTNUM { get; set; }
        public string HAINANXIANGMUBIAOSHI { get; set; }
        public string HAINANXIANGMUBIANHAO { get; set; }
        public string HAINANXIANGMUMINGCHEN { get; set; }
        public string HAINANGONGCHENGBIAOSHI { get; set; }
        public string HAINANGONGCHENGBIANHAO { get; set; }
        public string HAINANSHIGONGDANWEIBIAOSHI { get; set; }
        public int? HAINANISVARIFYINFO { get; set; }
        public string HAINANDANWEIGONGCHENGMING { get; set; }
        public string RECKONER { get; set; }
        public double? DEPOSITRATE { get; set; }
        public string PROJECTKEY { get; set; }
        public string USERDEFINEBAR { get; set; }
        public long? ICARD { get; set; }
        public string SALEMAN { get; set; }
        public string PROJECTMANAGER { get; set; }
        public string PROJECTMANAGERID { get; set; }
        public string ENGINEER { get; set; }
        public string ENGINEERID { get; set; }
        public string SUPERMANNUM { get; set; }
        public string SUPERUNITDJ { get; set; }
        public string CONSTRACTIONUNITMANNUM { get; set; }
        public string CONSTRACTIONUNITMANANQUAN { get; set; }
    }

    [Alias("t_bp_question")]
    public partial class t_bp_question
    {
        [Required]
        public string QUESTIONPRIMARYKEY { get; set; }
        [Required]
        public string SYSPRIMARYKEY { get; set; }
        public string ITEMTABLENAME { get; set; }
        public string SAMPLENUM { get; set; }
        public string RECORDMAN { get; set; }
        public DateTime? RECORDTIME { get; set; }
        public string RECORDINGPHASE { get; set; }
        public string AUDITINGMAN { get; set; }
        public DateTime? AUDITINGTIME { get; set; }
        public string ISAUDITING { get; set; }
        public string APPROVEMAN { get; set; }
        public DateTime? APPROVETIME { get; set; }
        public string ISAPPROVE { get; set; }
        public string NEEDPROCMAN { get; set; }
        public string ISPROCED { get; set; }
        public DateTime? NEEDPROCTIME { get; set; }
        public string QUESTIONTYPES { get; set; }
        public string CONTEXT { get; set; }
        public string AUDITINGCONTEXT { get; set; }
        public string APPROVECONTEXT { get; set; }
        public int? MODIFYRANGE { get; set; }
        public string ISMODIFIED { get; set; }
        public string ISREFUNDMONEY { get; set; }
        public int? ISUPLOADED { get; set; }
        public int? ISCONSOLEAPPROVED { get; set; }
        public string UNITCODE { get; set; }
        public string ISAPPROVED { get; set; }
    }

    [Alias("t_bp_searchColumnList")]
    public partial class t_bp_searchColumnList
    {
        [Required]
        public string tableName { get; set; }
        [Required]
        public string columnName { get; set; }
        [Required]
        public string columnChName { get; set; }
        [Required]
        public string columnTypes { get; set; }
    }

    [Alias("t_bp_sms")]
    public partial class t_bp_sm
    {
        [Required]
        public int id { get; set; }
        public string tel { get; set; }
        public string TeMes { get; set; }
        public DateTime? addtime { get; set; }
    }

    [Alias("t_bp_specialCheckScheme")]
    public partial class t_bp_specialCheckScheme
    {
        [Required]
        public string SysPrimaryKey { get; set; }
        public string ProjectNum { get; set; }
        public string ProjectName { get; set; }
        public string ProjectAdress { get; set; }
        public string BuildName { get; set; }
        public string BuildSize { get; set; }
        public string ConstructUnit { get; set; }
        public string Construct { get; set; }
        public string supervisorUnit { get; set; }
        public string BaseForm { get; set; }
        public string CustomID { get; set; }
        public string CustomName { get; set; }
        public string supervisorRegID { get; set; }
        public string CompareCustomName { get; set; }
        public string CheckContract { get; set; }
        public string CheckProject { get; set; }
        public string CheckSchemeID { get; set; }
        public DateTime? CheckSchemeRecordDate { get; set; }
        public string CheckSchemeRecordID { get; set; }
        public string CheckCount { get; set; }
        public DateTime? CheckDate { get; set; }
        public string CheckMan { get; set; }
        public string CheckSchemePath { get; set; }
        public string CheckReportPath { get; set; }
        public string CheckScheme { get; set; }
        public string CheckReport { get; set; }
        public string CheckResult { get; set; }
        public string CheckResultDetail { get; set; }
        public string CheckType { get; set; }
        public string Remark { get; set; }
        public byte[] CheckSchemeData { get; set; }
        public string CHECKSCHEMEDATApath { get; set; }
    }

    [Alias("t_bp_specialreport")]
    public partial class t_bp_specialreport
    {
        [Required]
        public int id { get; set; }
        public string customid { get; set; }
        public string customname { get; set; }
        public int? tid { get; set; }
        public string projectname { get; set; }
        public string fabah { get; set; }
        public int? isunusual { get; set; }
        public string stype { get; set; }
        public string reason { get; set; }
        public string wtreportnum { get; set; }
        public string wtfilename { get; set; }
        public string wtfilepath { get; set; }
        public string btreportnum { get; set; }
        public string btfilename { get; set; }
        public string btfilepath { get; set; }
        public DateTime? addtime { get; set; }
        public int? wtfiletype { get; set; }
        public string wtfiledescription { get; set; }
        public int? btfiletype { get; set; }
        public string btfiledescription { get; set; }
        public int? approvalStatus { get; set; }
        public int? delStuas { get; set; }
    }

    [Alias("t_bp_specialtestting")]
    public partial class t_bp_specialtestting
    {
        [Required]
        public int id { get; set; }
        public string projectname { get; set; }
        public string customname { get; set; }
        public string customid { get; set; }
        public string stype { get; set; }
        public string falsh { get; set; }
        public string jdzch { get; set; }
        public string fabah { get; set; }
        public string filename { get; set; }
        public string filepath { get; set; }
        public int? stuas { get; set; }
        public DateTime? addtime { get; set; }
        public string babname { get; set; }
        public string babpath { get; set; }
        public int? delStuas { get; set; }
    }

    [Alias("t_bp_station")]
    public partial class t_bp_station
    {
        [Required]
        public string STATIONID { get; set; }
        [Required]
        public string NAME { get; set; }
        public string FULLNAME { get; set; }
        public string ADDRESS { get; set; }
        public string PSTATIONID { get; set; }
        public int? SEQ { get; set; }
        public string AREACODE { get; set; }
        [Required]
        public string SIDX { get; set; }
    }

    [Alias("t_bp_tempFile")]
    public partial class t_bp_tempFile
    {
        [Required]
        public int id { get; set; }
        public string tempFilePath { get; set; }
        public string toFilePath { get; set; }
    }

    [Alias("t_bp_videoAccount")]
    public partial class t_bp_videoAccount
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string UnitCode { get; set; }
        public string acccountName { get; set; }
        public string accountPwd { get; set; }
    }

    [Alias("t_D_UserChange")]
    public partial class t_D_UserChange
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public string unitname { get; set; }
        public string area { get; set; }
        public string bgnr { get; set; }
        public string bgq { get; set; }
        public string bgh { get; set; }
        public DateTime? time { get; set; }
        public string unitCode { get; set; }
        [Alias("static")]
        public int? @static { get; set; }
        public string fh { get; set; }
        public string sp { get; set; }
        public string outstaticinfo { get; set; }
        public string bgclpath { get; set; }
        public string cbr { get; set; }
        public string SQname { get; set; }
        public string SQTel { get; set; }
        public string YZZPath { get; set; }
        public string EndTime { get; set; }
    }

    [Alias("t_D_UserOutInfo")]
    public partial class t_D_UserOutInfo
    {
        [Required]
        public int id { get; set; }
        public int? pid { get; set; }
        public string outstaticinfo { get; set; }
        public int? outstatic { get; set; }
        public DateTime? time { get; set; }
    }

    [Alias("t_D_UserTableCS")]
    public partial class t_D_UserTableC
    {
        [Required]
        public int id { get; set; }
        public int? Pid { get; set; }
        public string content { get; set; }
        public DateTime? addtime { get; set; }
    }

    [Alias("t_D_UserTableFive")]
    public partial class t_D_UserTableFive
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public string gzjl { get; set; }
        public int? staitc { get; set; }
        public string pid { get; set; }
        public string unitcode { get; set; }
    }

    [Alias("t_D_UserTableFour")]
    public partial class t_D_UserTableFour
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public DateTime? time { get; set; }
        public string zw { get; set; }
        public string zc { get; set; }
        public string xl { get; set; }
        public string hshxhzy { get; set; }
        public string jcgzlx { get; set; }
        public string bgdh { get; set; }
        public string yddh { get; set; }
        public string gzjl { get; set; }
        public int? staitc { get; set; }
        public decimal? mtype { get; set; }
        public string photopath { get; set; }
        public string pid { get; set; }
        public string zcpath { get; set; }
        public string postNumpath { get; set; }
        public string postNum { get; set; }
        public string xlpath { get; set; }
    }

    
    [Alias("t_D_UserTableOne")]
    public partial class t_D_UserTableOne
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public string Fel { get; set; }
        public DateTime? addtime { get; set; }
        public string unitCode { get; set; }
        public DateTime? time { get; set; }
        public string selfnum { get; set; }
        [Alias("static")]
        public int? @static { get; set; }
        public string unitname { get; set; }
        public string onepath_zl { get; set; }
        public string twopath_zl { get; set; }
        public string threepath_zl { get; set; }
        public string Fourpath_zl { get; set; }
        public string fivepath_zl { get; set; }
        public string Sixpath_zl { get; set; }
        public string Sevenpath_zl { get; set; }
        /// <summary>
        /// 0：新申请 1：增项 2：延续
        /// </summary>
        public string type { get; set; }
        public string area { get; set; }
    }

    [Alias("t_D_userTableSC")]
    public partial class t_D_userTableSC
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public string unitName { get; set; }
        public string frdb { get; set; }
        public string sqnr { get; set; }
        public string ssfzr { get; set; }
        public string onesjz { get; set; }
        public string onedbqk { get; set; }
        public string twosjz { get; set; }
        public string twodbqk { get; set; }
        public string threesjz { get; set; }
        public string threedbqk { get; set; }
        public string foursjz { get; set; }
        public string fourdbqk { get; set; }
        public string fivesjzone { get; set; }
        public string fivedbqkone { get; set; }
        public string fivesjztwo { get; set; }
        public string fivedbqktwo { get; set; }
        public string sixsjz { get; set; }
        public string sixdbqk { get; set; }
        public string sevensjz { get; set; }
        public string sevendbqk { get; set; }
        public string shyj { get; set; }
        public string username { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? addtime { get; set; }
        public int? pid { get; set; }
        public string usercode { get; set; }
        public int? zjtj { get; set; }
    }

    [Alias("t_D_UserTableSeven")]
    public partial class t_D_UserTableSeven
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public int? EquipId { get; set; }
        public string gzjl { get; set; }
        public int? staitc { get; set; }
        public string pid { get; set; }
        public string unitcode { get; set; }
    }

    [Alias("t_D_userTableSH")]
    public partial class t_D_userTableSH
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public string unitName { get; set; }
        public string frdb { get; set; }
        public string yyjczz { get; set; }
        public string onesjz { get; set; }
        public string onedbqk { get; set; }
        public string twosjz { get; set; }
        public string twodbqk { get; set; }
        public string twodjjcsjz { get; set; }
        public string twodjjcdbqk { get; set; }
        public string twodztjgsjz { get; set; }
        public string twoztjgdbqk { get; set; }
        public string twojzmqsjz { get; set; }
        public string twojzmqdbqk { get; set; }
        public string twogjgsjz { get; set; }
        public string twogjgdbqk { get; set; }
        public string twojzqysjz { get; set; }
        public string twojzqydbqk { get; set; }
        public string threesjz { get; set; }
        public string threedbqk { get; set; }
        public string foursjz { get; set; }
        public string fourdbqk { get; set; }
        public string shyj { get; set; }
        public string username { get; set; }
        public DateTime? createtime { get; set; }
        public DateTime? addtime { get; set; }
        public int? pid { get; set; }
        public string usercode { get; set; }
        public int? zjtj { get; set; }
    }


    [Alias("t_D_UserTableSixFile")]
    public partial class t_D_UserTableSixFile
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public int pid { get; set; }
        public string unitcode { get; set; }
        public string filepath { get; set; }
    }

    [Alias("t_D_UserTableSix")]
    public partial class t_D_UserTableSix
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public string gzjl { get; set; }
        public int? staitc { get; set; }
        public string pid { get; set; }
        public string unitcode { get; set; }
        public int? PeopleId { get; set; }
    }

    [Alias("t_D_UserTableTen")]
    public partial class t_D_UserTableTen
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public string Oneyj { get; set; }
        public DateTime? OneyjTime { get; set; }
        public string OneFZr { get; set; }
        public string Twory { get; set; }
        public string TwoSb { get; set; }
        public string TwoHJ { get; set; }
        public string TwoGL { get; set; }
        public string TwoJS { get; set; }
        public string TwoZH { get; set; }
        public string TwoZZ { get; set; }
        public string TwoZY { get; set; }
        public DateTime? TwoTime { get; set; }
        public string ThreeYJ { get; set; }
        public string ThreeFZr { get; set; }
        public DateTime? ThreeTime { get; set; }
        public string ThreeZZZSBH { get; set; }
        public DateTime? ThreeYXQBegin { get; set; }
        public DateTime? ThreeYXQEnd { get; set; }
        public int? pid { get; set; }
        [Alias("static")]
        public int? @static { get; set; }
        public string outstaticinfo { get; set; }
        public int? outstatic { get; set; }
        public DateTime? addtime { get; set; }

        public string slbh { get; set; }
        public DateTime? sltime { get; set; }
        public string zjsp1 { get; set; }
        public string zjsp2 { get; set; }
        public DateTime? printtime { get; set; }
        public DateTime? gstime { get; set; }
        public string sswordpath { get; set; }
        public string cbr { get; set; }
        public string ssfhinfo { get; set; }
        public string slr { get; set; }
        public string sszjsp1 { get; set; }
        public string sszjsp2 { get; set; }
    }

    [Alias("t_D_UserTableThree")]
    public partial class t_D_UserTableThree
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public DateTime? time { get; set; }
        public string zw { get; set; }
        public string zc { get; set; }
        public string xl { get; set; }
        public string hshxhzy { get; set; }
        public string jcgzlx { get; set; }
        public string bgdh { get; set; }
        public string yddh { get; set; }
        public string gzjl { get; set; }
        public int? staitc { get; set; }
        public decimal? mtype { get; set; }
        public string photopath { get; set; }
        public string pid { get; set; }
        public string zcpath { get; set; }
        public string postNumpath { get; set; }
        public string postNum { get; set; }
        public string xlpath { get; set; }
    }

    [Alias("t_D_UserTableTwo")]
    public partial class t_D_UserTableTwo
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public int? pid { get; set; }
        public string unitcode { get; set; }
        public string name { get; set; }
        public DateTime? time { get; set; }
        public string address { get; set; }
        public string tel { get; set; }
        public string postcode { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string businessnum { get; set; }
        public string businessnumpath { get; set; }
        public string businessnumunit { get; set; }
        public double? regaprice { get; set; }
        public string economicnature { get; set; }
        public string measnum { get; set; }
        public string measunit { get; set; }
        public string fr { get; set; }
        public string frzw { get; set; }
        public string frzc { get; set; }
        public string jsfzr { get; set; }
        public string jsfzrzw { get; set; }
        public string jsfzrzc { get; set; }
        public double? zbryzs { get; set; }
        public double? zyjsrys { get; set; }
        public double? zjzcrs { get; set; }
        public double? gjzcrs { get; set; }
        public int? yqsbzs { get; set; }
        public double? yqsbgtzc { get; set; }
        public double? gzmj { get; set; }
        public double? fwjzmj { get; set; }
        public string sqjcyw { get; set; }
        [Alias("static")]
        [Required]
        public int @static { get; set; }
        public string measnumpath { get; set; }
        public string zzjgdm { get; set; }
        public string YZZPath { get; set; }
        public string SQname { get; set; }
        public string SQTel { get; set; }
        public string bgcszmPath { get; set; }
        public string zxzmPath { get; set; }
        public string gqzmPath { get; set; }
        public string zzscwjPath { get; set; }
        public string glscwjPath { get; set; }
    }

    [Alias("t_D_UserDistributeExpert")]
    public partial class t_D_UserDistributeExpert
    {
        [Required]
        public int id { get; set; }
        public string unitname { get; set; }
        public string unitcode { get; set; }
        public string distributedexpert { get; set; }
        public string pid { get; set; }
        public DateTime? addtime { get; set; }
        public DateTime? updatetime { get; set; }
        public int? updateuser { get; set; }
        public int? status { get; set; }
    }

    [Alias("t_D_UserExpertUnit")]
    public partial class t_D_UserExpertUnit
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public int? userid { get; set; }
        public int? pid { get; set; }
        public DateTime? addtime { get; set; }
        public int? status { get; set; }
        public int? qualifystatus { get; set; }
        public DateTime? qualifychecktime { get; set; }
        public int? speicalstatus { get; set; }
        public DateTime? speicalchecktime { get; set; }
        public long? shid { get; set; }
        public long? scid { get; set; }
        public int? needUnitBuildingQualify { get; set; }
        public int? needSpecialQualify { get; set; }
    }


    [Alias("t_D_UserTableCS")]
    public partial class t_D_UserTableCS
    {
        [Required]
        [AutoIncrement]
        public int id { get; set; }
        public int? pid { get; set; }
        public string content { get; set; }
        public DateTime? addtime { get; set; }
        public string csbh { get; set; }
    }



    [Alias("T_InstDownload")]
    public partial class T_InstDownload : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string Customid { get; set; }
        public string Versionid { get; set; }
        public DateTime? DownloadTime { get; set; }
    }

    [Alias("T_softWareVersion")]
    public partial class T_softWareVersion : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string SoftwareVersion { get; set; }
        public string UpdateContent { get; set; }
        public DateTime? UploadTime { get; set; }
        public string AttachFilePath { get; set; }
    }

    [Alias("t_sys_cellModelParams")]
    public partial class t_sys_cellModelParam
    {
        [Required]
        public string id { get; set; }
        public string modelParams { get; set; }
    }

    [Alias("t_sys_config")]
    public partial class t_sys_config
    {
        [Required]
        public string CODE { get; set; }
        [Required]
        public string NAME { get; set; }
        [Required]
        public string VAL { get; set; }
    }

    [Alias("t_sys_folder")]
    public partial class t_sys_folder
    {
        [Required]
        public int FOLDERID { get; set; }
        [Required]
        public int ROOTID { get; set; }
        [Required]
        public string USERCODE { get; set; }
        [Required]
        public string NAME { get; set; }
        [Required]
        public string PATH { get; set; }
        [Required]
        public string STATUS { get; set; }
    }

    [Alias("t_sys_icon")]
    public partial class t_sys_icon
    {
        [Required]
        public int id { get; set; }
        public string iconname { get; set; }
    }

    [Alias("t_sys_menu")]
    public partial class t_sys_menu
    {
        [Required]
        public int MENUID { get; set; }
        [Required]
        public string NAME { get; set; }
        public int? PID { get; set; }
        public int? MODULEID { get; set; }
        public int? SEQ { get; set; }
        [Required]
        public string ISSYS { get; set; }
        public string icon { get; set; }
    }

    [Alias("t_sys_module")]
    public partial class t_sys_module
    {
        [Required]
        public int MODULEID { get; set; }
        [Required]
        public string NAME { get; set; }
        public string LINK { get; set; }
        public string CODE { get; set; }
        [Required]
        public string ISSYS { get; set; }
        public string TYPE { get; set; }
    }

    [Alias("t_sys_muser")]
    public partial class t_sys_muser
    {
        [Required]
        public int MODULEID { get; set; }
        [Required]
        public string USERCODE { get; set; }
    }

    [Alias("t_sys_pagelist")]
    public partial class t_sys_pagelist
    {
        [Required]
        public int id { get; set; }
        public string pagename { get; set; }
        public string pagetext { get; set; }
    }

    [Alias("t_sys_parmList")]
    public partial class t_sys_parmList
    {
        [Required]
        public int ID { get; set; }
        public string parmName { get; set; }
        public string parmValue { get; set; }
        public string explain { get; set; }
        public string parmCode { get; set; }
    }

    [Alias("t_sys_region")]
    [Serializable]
    public partial class t_sys_region
    {
        [Required]
        public string regionid { get; set; }
        [Required]
        public string regionname { get; set; }
    }

    [Alias("t_sys_role")]
    public partial class t_sys_role
    {
        [Required]
        public int ROLEID { get; set; }
        [Required]
        public string NAME { get; set; }
        [Required]
        public string CODE { get; set; }
        [Required]
        public string ISSYS { get; set; }
    }

    [Alias("t_sys_roleright")]
    public partial class t_sys_roleright
    {
        [Required]
        public int ROLEID { get; set; }
        [Required]
        public int MODULEID { get; set; }
        [Required]
        public string ISSYS { get; set; }
    }

    [Alias("t_sys_root")]
    public partial class t_sys_root
    {
        [Required]
        public int ROOTID { get; set; }
        [Required]
        public string NAME { get; set; }
        public string CODE { get; set; }
        [Required]
        public string PATH { get; set; }
    }

    [Alias("t_sys_sqlList")]
    public partial class t_sys_sqlList
    {
        [Required]
        public int ID { get; set; }
        public string reportCode { get; set; }
        public string reportName { get; set; }
        public string sql { get; set; }
        public string aspx { get; set; }
        public string js { get; set; }
        public string explain { get; set; }
        public string countSql { get; set; }
    }

    [Alias("t_sys_user")]
    public partial class t_sys_user
    {
        [Required]
        public string USERCODE { get; set; }
        [Required]
        public string USERNAME { get; set; }
        public string SEX { get; set; }
        public string STATIONID { get; set; }
        public string CUSTOMID { get; set; }
        [Required]
        public string ACCOUNT { get; set; }
        [Required]
        public string PASSWORD { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string MOBILENO { get; set; }
        public string GRADE { get; set; }
        public string ISSYS { get; set; }
        public string STATUS { get; set; }
        public string ISPUBLIC { get; set; }
        public string STATIONIDX { get; set; }
        public int? SEQ { get; set; }
        public string unitPower { get; set; }
        public string unitType { get; set; }
        public string unitName { get; set; }
        public string address { get; set; }
        public int? userrole { get; set; }
        public string regionName { get; set; }
        public DateTime? addtime { get; set; }
    }

    [Alias("t_sys_userLoginInfo")]
    public partial class t_sys_userLoginInfo
    {
        [Required]
        public int id { get; set; }
        public string username { get; set; }
        public string userUnit { get; set; }
        public DateTime? logintime { get; set; }
        public string loginIp { get; set; }
    }

    [Alias("t_sys_userright")]
    public partial class t_sys_userright
    {
        [Required]
        public string USERCODE { get; set; }
        [Required]
        public int MODULEID { get; set; }
        [Required]
        public string ISCLOSE { get; set; }
    }

    [Alias("t_sys_Version")]
    public partial class t_sys_Version
    {
        [Required]
        public int id { get; set; }
        public string usercode { get; set; }
        public string name { get; set; }
        public string FileVersion { get; set; }
        public string FileVersionDate { get; set; }
        public string EndDate { get; set; }
    }

    [Alias("t_web_infoclass")]
    public partial class t_web_infoclass
    {
        [Required]
        public int id { get; set; }
        public string chaName { get; set; }
        public string location { get; set; }
        public string type { get; set; }
        public int? seq { get; set; }
        public string pagename { get; set; }
        public int? classRank { get; set; }
        public int? isShowInMainMenu { get; set; }
        public string articleListPagePicture { get; set; }
    }

    [Alias("t_web_infocontent")]
    public partial class t_web_infocontent
    {
        [Required]
        public int id { get; set; }
        public int? classid { get; set; }
        public string author { get; set; }
        public string picFrom { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime? addtime { get; set; }
        public string isval { get; set; }
        public int? ishot { get; set; }
        public int? isShowInFlash { get; set; }
    }

    [Alias("t_web_Link")]
    public partial class t_web_Link
    {
        [Required]
        public int id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string linkUrl { get; set; }
        public int? seq { get; set; }
        public DateTime? addtime { get; set; }
    }

    [Alias("t_web_loginUser")]
    public partial class t_web_loginUser
    {
        [Required]
        public int id { get; set; }
        public string username { get; set; }
        public string account { get; set; }
        public string password { get; set; }
        public DateTime? regtime { get; set; }
        public string phone { get; set; }
        public int? status { get; set; }
    }

    [Alias("t_web_onlineItem")]
    public partial class t_web_onlineItem
    {
        [Required]
        public int id { get; set; }
        public string itemname { get; set; }
        public string itemcode { get; set; }
        public string itemtype { get; set; }
        public int? seq { get; set; }
        public int? itemseq { get; set; }
        public string tabletitle { get; set; }
        public string skbh { get; set; }
    }

    [Alias("t_web_WebInfo")]
    public partial class t_web_WebInfo
    {
        [AutoIncrement]
        public int id { get; set; }
        public string Brief { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string QQ { get; set; }
        public string Email { get; set; }
        public string address { get; set; }
        public string Copyright { get; set; }
        public string recordId { get; set; }
        public string support { get; set; }
        public string imgPath { get; set; }
        public int? WebCount { get; set; }
    }

    [Alias("Teacher")]
    public partial class Teacher
    {
        public string FName { get; set; }
        public string FCode { get; set; }
        public string FCardNo { get; set; }
        [Required]
        public int id { get; set; }
        public string sex { get; set; }
        public string unit { get; set; }
        public string duties { get; set; }
        public string title { get; set; }
        public string academy { get; set; }
        public string course { get; set; }
    }

    [Alias("TestArea")]
    public partial class TestArea
    {
        [Required]
        public int FAreaCode { get; set; }
        public string FAreaName { get; set; }
        public string FAddress { get; set; }
        public string FDriveBus { get; set; }
    }

    [Alias("TestRoom")]
    public partial class TestRoom
    {
        [Required]
        public int FRoomCode { get; set; }
        public int? FAreaCode { get; set; }
        public string FRoomName { get; set; }
    }

    [Alias("TestType")]
    public partial class TestType
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string FTestTypeCode { get; set; }
        [Required]
        public string FTestTypeName { get; set; }
        public DateTime? Ftime { get; set; }
        public int? TestNum { get; set; }
        public string mShow { get; set; }
    }

    [Alias("TestType_BM")]
    public partial class TestType_BM
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string FTestTypeCode { get; set; }
        [Required]
        public string FTestTypeName { get; set; }
        public DateTime? Ftime { get; set; }
        public int? TestNum { get; set; }
        public int? pid { get; set; }
        public int? showType { get; set; }
        public int? theoryPrice { get; set; }
        public int? PracticalPrice { get; set; }
    }

    [Alias("TestTypeList")]
    public partial class TestTypeList
    {
        [Required]
        public int id { get; set; }
        public string name { get; set; }
        public int? pid { get; set; }
    }





    [Alias("UserInArea")]
    public partial class UserInArea : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Area { get; set; }
    }

    [Alias("UserInCustom")]
    public partial class UserInCustom
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string CustomId { get; set; }
        [Required]
        public UserCustomType UserCustomType { get; set; }
    }

    [Alias("UserInItem")]
    public partial class UserInItem
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public string ItemTableName { get; set; }
        [Required]
        public UserItemType UserItemType { get; set; }
    }


    [Alias("WxCheckItem")]
    public partial class WxCheckItem
    {
        [Required]
        public int Id { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public string TypeText { get; set; }
    }

    [Alias("WxInspectUnit")]
    public partial class WxInspectUnit
    {
        public string isdefult { get; set; }
        public string unitName { get; set; }
        public string unitcode { get; set; }
    }

    [Alias("WxMaxNum")]
    public partial class WxMaxNum
    {
        [Required]
        public string unitcode { get; set; }
        public int? maxnum { get; set; }
        public int? projectmaxnum { get; set; }
        public int? TaskMaxNum { get; set; }
    }

    [Alias("WxProjectInfo")]
    public partial class WxProjectInfo
    {
        public string InspectSerialNo { get; set; }
        public string SerialNo { get; set; }
        public string Name { get; set; }
        public string Builder { get; set; }
        public string Supervisor { get; set; }
        public string Coordinates { get; set; }
        public string CustomId { get; set; }
        public string InspectUnitCode { get; set; }
        [AutoIncrement]
        public int Id { get; set; }
        public string Site { get; set; }
        public DateTime? DeclareDate { get; set; }
    }

    [Alias("WxSchedule")]
    public partial class WxSchedule
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string ProjectSerialNo { get; set; }
        public string ProjectName { get; set; }
        public string CustomId { get; set; }
        public string CheckParamCode { get; set; }
        public DateTime? DeclareDate { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TimePeriod { get; set; }
        [Required]
        public int Status { get; set; }
        public string Declarant { get; set; }
        public string DeclarantIDNo { get; set; }
        public DateTime? StartTime { get; set; }
        public string Starter { get; set; }
        public string StartIDNo { get; set; }
        public DateTime? FinishTime { get; set; }
        public string Finisher { get; set; }
        public string FinishIDNo { get; set; }
        public string Remark { get; set; }
        public string Coordinates { get; set; }
        public string Images { get; set; }
        public string Videos { get; set; }
        public string Witness { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }
        public string CheckTypeCode { get; set; }
        public string CheckTypeName { get; set; }
        public string CheckItemCode { get; set; }
        public string CheckItemName { get; set; }
        public string CheckParamName { get; set; }
    }

    [Alias("WxScheduleLog")]
    public partial class WxScheduleLog
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public DateTime Time { get; set; }
        public string Name { get; set; }
        public string IDNo { get; set; }
        public string Content { get; set; }
    }

    [Alias("WxScheduleTester")]
    public partial class WxScheduleTester
    {
        [Required]
        public string ScheduleCode { get; set; }
        [Required]
        public string IDNo { get; set; }
        public string Name { get; set; }
    }

    [Alias("WxUser")]
    public partial class WxUser
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string OpenId { get; set; }
        [Required]
        public bool NoMsg { get; set; }
    }

    [Serializable]
    [Alias("TotalItem")]
    public partial class TotalItem
    {
        [Required]
        public int ID { get; set; }
        public string ITEMTABLENAME { get; set; }
        public string ITEMCHNAME { get; set; }
        public string ITEMTYPES { get; set; }
        public string ITEMSTANDARDS { get; set; }
        public int? ITEMSYSAUTO { get; set; }
        public string JSONVALUE { get; set; }
        public int? SAMPLENUM { get; set; }
        public int? ISUSE { get; set; }
        public int? ordernum { get; set; }
    }


    [Serializable]
    [Alias("CheckItemType")]
    public partial class CheckItemType
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string CheckItemCode { get; set; }
        public string CheckItemName { get; set; } 
    }

    [Alias("t_bp_PkpmJCRU_PushFive")]
    public partial class t_bp_PkpmJCRU_PushFive
    {
        [AutoIncrement]
        public int ID { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string type { get; set; }
        public string content { get; set; }
        public DateTime? addtime { get; set; }
        public string Show { get; set; }
    }

    [Alias("t_bp_PkpmJCRU_isRead")]
    public partial class t_bp_PkpmJCRU_isRead
    {
        [Required]
        public int msgId { get; set; }
        [Required]
        public string unitCode { get; set; }
        public int? readTimes { get; set; }
    }

    [Alias("t_bp_PkpmJCRU")]
    public partial class t_bp_PkpmJCRU
    {
        [AutoIncrement]
        public int ID { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string content { get; set; }
        public string addtime { get; set; }
    }

    [Alias("t_bp_area_ST")]
    public partial class t_bp_area_ST : IHasId<string>
    {
        [Alias("AREACODE")]
        [Required]
        public string Id { get; set; }
        public string PAREACODE { get; set; }
        [Required]
        public string AREANAME { get; set; }
        [Required]
        public string CODETYPE { get; set; }
        public string MEMO { get; set; }
        public string INNERCODE { get; set; }
    }

    [Alias("t_bp_CheckCustom_ST")]
    public partial class t_bp_CheckCustom_ST : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string CustomId { get; set; }
        public DateTime? CheDate { get; set; }
        public string CheResult { get; set; }
        public string CheRem { get; set; }
        public string cheunit { get; set; }
    }

    [Alias("t_bp_CusAchievement_ST")]
    public partial class t_bp_CusAchievement_ST : IHasId<long>
    {
        [Alias("id")]
        [AutoIncrement]
        public long Id { get; set; }
        [Required]
        public string CustomId { get; set; }
        public string AchievementTime { get; set; }
        public string AchievementContent { get; set; }
        public string AchievementRem { get; set; }
    }

    [Alias("t_bp_CusAwards_ST")]
    public partial class t_bp_CusAwards_ST : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string CustomId { get; set; }
        public string AwaName { get; set; }
        public string AwaUnit { get; set; }
        public string AwaContent { get; set; }
        public DateTime? AwaDate { get; set; }
        public string AwaRem { get; set; }
    }

    [Alias("t_bp_CusChange_ST")]
    public partial class t_bp_CusChange_ST : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string CustomId { get; set; }
        public string ChaContent { get; set; }
        public DateTime? ChaDate { get; set; }
        public string ChaAppUnit { get; set; }
        public string ChaRem { get; set; }
    }

    [Alias("t_bp_CusCheckParams_ST")]
    public partial class t_bp_CusCheckParams_ST : IHasId<long>
    {
        [Alias("id")]
        [AutoIncrement]
        public long Id { get; set; }
        [Required]
        public string customId { get; set; }
        public string topCheckType { get; set; }
        public string checkType { get; set; }
        public string checkItem { get; set; }
        public string checkParams { get; set; }
    }

    [Alias("t_bp_CusPunish_ST")]
    public partial class t_bp_CusPunish_ST : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string CustomId { get; set; }
        public string PunName { get; set; }
        public string PunUnit { get; set; }
        public string PunContent { get; set; }
        public DateTime? PunDate { get; set; }
        public string PunRem { get; set; }
    }

    [Alias("t_bp_custom_bak_ST")]
    public partial class t_bp_custom_bak_ST : IHasId<string>
    {
        [Alias("ID")]
        [Required]
        public string Id { get; set; }
        public string NAME { get; set; }
        public string STATIONID { get; set; }
        public string POSTCODE { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string ADDRESS { get; set; }
        public DateTime? CREATETIME { get; set; }
        public string EMAIL { get; set; }
        public string BUSINESSNUM { get; set; }
        public string BUSINESSNUMUNIT { get; set; }
        public string REGAPRICE { get; set; }
        public string ECONOMICNATURE { get; set; }
        public string MEASNUM { get; set; }
        public string MEASUNIT { get; set; }
        public string MEASNUMPATH { get; set; }
        public string FR { get; set; }
        public string JSNAME { get; set; }
        public string JSTIILE { get; set; }
        public string JSYEAR { get; set; }
        public string ZLNAME { get; set; }
        public string ZLTITLE { get; set; }
        public string ZLYEAR { get; set; }
        public string PERCOUNT { get; set; }
        public string MIDPERCOUNT { get; set; }
        public string HEIPERCOUNT { get; set; }
        public string REGYTSTA { get; set; }
        public string REGJGSTA { get; set; }
        public string INSTRUMENTPRICE { get; set; }
        public string HOUSEAREA { get; set; }
        public string DETECTAREA { get; set; }
        public string DETECTTYPE { get; set; }
        public string DETECTNUM { get; set; }
        public DateTime? APPLDATE { get; set; }
        public string DETECTPATH { get; set; }
        public string QUAINFO { get; set; }
        public string APPROVALSTATUS { get; set; }
        public string ADDDATE { get; set; }
        public string phone { get; set; }
        public DateTime? detectnumStartDate { get; set; }
        public DateTime? detectnumEndDate { get; set; }
        public DateTime? measnumStartDate { get; set; }
        public DateTime? measnumEndDate { get; set; }
        public string hasNumPerCount { get; set; }
        public string instrumentNum { get; set; }
        public string businessnumPath { get; set; }
        public string approveadvice { get; set; }
        public string subunitnum { get; set; }
        public string issubunit { get; set; }
        public string supunitcode { get; set; }
        public string subunitdutyman { get; set; }
        public string area { get; set; }
        public string detectunit { get; set; }
        public DateTime? detectappldate { get; set; }
        public string shebaopeoplelistpath { get; set; }
        public string sysarea { get; set; }
        public string yharea { get; set; }
        public string captial { get; set; }
        public string credit { get; set; }
        public string companytype { get; set; }
        public string floorarea { get; set; }
        public string yearplanproduce { get; set; }
        public string preyearproduce { get; set; }
        public string businesspermit { get; set; }
        public string businesspermitpath { get; set; }
        public string enterprisemanager { get; set; }
        public string financeman { get; set; }
        public string director { get; set; }
        public string cerfgrade { get; set; }
        public string cerfno { get; set; }
        public string cerfnopath { get; set; }
        public string sslcmj { get; set; }
        public string sslczk { get; set; }
        public string szssccnl { get; set; }
        public string fmhccnl { get; set; }
        public string chlccnl { get; set; }
        public string ytwjjccnl { get; set; }
        public string managercount { get; set; }
        public string jsglcount { get; set; }
        public string testcount { get; set; }
        public string shebaopeoplenum { get; set; }
        public string workercount { get; set; }
        public string zgcount { get; set; }
        public string instrumentpath { get; set; }
        public int? datatype { get; set; }
        public string cmanumcerf { get; set; }
        public string SelectTel { get; set; }
        public string AppealTel { get; set; }
        public string AppealEmail { get; set; }
        public string areaqx { get; set; }
        public string zzbgpath { get; set; }
    }

    [Alias("t_bp_custom_ST")]
    public partial class t_bp_custom_ST : IHasId<string>
    {
        [Alias("ID")]
        [Required]
        public string Id { get; set; }
        public string NAME { get; set; }
        public string STATIONID { get; set; }
        public string POSTCODE { get; set; }
        public string TEL { get; set; }
        public string FAX { get; set; }
        public string ADDRESS { get; set; }
        public DateTime? CREATETIME { get; set; }
        public string EMAIL { get; set; }
        public string BUSINESSNUM { get; set; }
        public string BUSINESSNUMUNIT { get; set; }
        public string REGAPRICE { get; set; }
        public string ECONOMICNATURE { get; set; }
        public string MEASNUM { get; set; }
        public string MEASUNIT { get; set; }
        public string MEASNUMPATH { get; set; }
        public string FR { get; set; }
        public string JSNAME { get; set; }
        public string JSTIILE { get; set; }
        public string JSYEAR { get; set; }
        public string ZLNAME { get; set; }
        public string ZLTITLE { get; set; }
        public string ZLYEAR { get; set; }
        public string PERCOUNT { get; set; }
        public string MIDPERCOUNT { get; set; }
        public string HEIPERCOUNT { get; set; }
        public string REGYTSTA { get; set; }
        public string REGJGSTA { get; set; }
        public string INSTRUMENTPRICE { get; set; }
        public string HOUSEAREA { get; set; }
        public string DETECTAREA { get; set; }
        public string DETECTTYPE { get; set; }
        public string DETECTNUM { get; set; }
        public DateTime? APPLDATE { get; set; }
        public string DETECTPATH { get; set; }
        public string QUAINFO { get; set; }
        public string APPROVALSTATUS { get; set; }
        public string ADDDATE { get; set; }
        public string phone { get; set; }
        public DateTime? detectnumStartDate { get; set; }
        public DateTime? detectnumEndDate { get; set; }
        public DateTime? measnumStartDate { get; set; }
        public DateTime? measnumEndDate { get; set; }
        public string hasNumPerCount { get; set; }
        public string instrumentNum { get; set; }
        public string businessnumPath { get; set; }
        public string approveadvice { get; set; }
        public string subunitnum { get; set; }
        public string issubunit { get; set; }
        public string supunitcode { get; set; }
        public string subunitdutyman { get; set; }
        public string area { get; set; }
        public string detectunit { get; set; }
        public DateTime? detectappldate { get; set; }
        public string shebaopeoplelistpath { get; set; }
        public string sysarea { get; set; }
        public string yharea { get; set; }
        public string captial { get; set; }
        public string credit { get; set; }
        public string companytype { get; set; }
        public string floorarea { get; set; }
        public string yearplanproduce { get; set; }
        public string preyearproduce { get; set; }
        public string businesspermit { get; set; }
        public string businesspermitpath { get; set; }
        public string enterprisemanager { get; set; }
        public string financeman { get; set; }
        public string director { get; set; }
        public string cerfgrade { get; set; }
        public string cerfno { get; set; }
        public string cerfnopath { get; set; }
        public string sslcmj { get; set; }
        public string sslczk { get; set; }
        public string szssccnl { get; set; }
        public string fmhccnl { get; set; }
        public string chlccnl { get; set; }
        public string ytwjjccnl { get; set; }
        public string managercount { get; set; }
        public string jsglcount { get; set; }
        public string testcount { get; set; }
        public string shebaopeoplenum { get; set; }
        public string workercount { get; set; }
        public string zgcount { get; set; }
        public string instrumentpath { get; set; }
        public int? datatype { get; set; }
        public string cmanumcerf { get; set; }
        public string SelectTel { get; set; }
        public string AppealTel { get; set; }
        public string AppealEmail { get; set; }
        public string areaqx { get; set; }
        public string zzbgpath { get; set; }
        public string qt1 { get; set; }
        public string qt2 { get; set; }
        public string jzs1 { get; set; }
        public string jzs2 { get; set; }
        public string jhhs1 { get; set; }
        public string jhhs2 { get; set; }
        public string hs1 { get; set; }
        public string hs2 { get; set; }
        public string data_status { get; set; }
    }

    [Alias("t_bp_customServer_ST")]
    public partial class t_bp_customServer_ST : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string unitcode { get; set; }
        public string mac { get; set; }
    }

    [Alias("t_bp_department_ST")]
    public partial class t_bp_department_ST : IHasId<string>
    {
        [Alias("DEPARTNUM")]
        [Required]
        public string Id { get; set; }
        [Required]
        public string UNITCODE { get; set; }
        public string DEPARTNAME { get; set; }
        public string FULLNAME { get; set; }
        public string DEPARTFMAN { get; set; }
        public string DEPARTTEL { get; set; }
    }

    [Alias("t_bp_Equipment_ST")]
    public partial class t_bp_Equipment_ST : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string customid { get; set; }
        public string EquName { get; set; }
        public string checkitem { get; set; }
        public string equtype { get; set; }
        public string equspec { get; set; }
        public string testrange { get; set; }
        public string degree { get; set; }
        public string uncertainty { get; set; }
        public string checkunit { get; set; }
        public string repairunit { get; set; }
        public string checkcerfnum { get; set; }
        public string repaircerfnum { get; set; }
        public string checkcerfnumpath { get; set; }
        public string repaircerfnumpath { get; set; }
        public DateTime? checkstartdate { get; set; }
        public DateTime? checkenddate { get; set; }
        public DateTime? repairstartdate { get; set; }
        public DateTime? repairenddate { get; set; }
        public string selfcheckitem { get; set; }
        public string selfrepairitem { get; set; }
        public string selfcheckstandardname { get; set; }
        public string selfchecknum { get; set; }
        public string selfrepairstandardname { get; set; }
        public string selfrepairnum { get; set; }
        public string explain { get; set; }
        public string isautoacs { get; set; }
        public string autoacsprovider { get; set; }
        public string approvalstatus { get; set; }
        public string approveadvice { get; set; }
        public string supcustomid { get; set; }
        public string equclass { get; set; }
        public string equnum { get; set; }
        public DateTime? buytime { get; set; }
        public DateTime? timestart { get; set; }
        public DateTime? timeend { get; set; }
        public string sysarea { get; set; }
        public string yharea { get; set; }
        public DateTime? checktime { get; set; }
        public string data_status { get; set; }
    }

    [Alias("t_bp_equItemSubItemList_ST")]
    public partial class t_bp_equItemSubItemList_ST : IHasId<long>
    {
        [Alias("id")]
        [AutoIncrement]
        public long Id { get; set; }
        [Required]
        public int equId { get; set; }
        public string itemTableName { get; set; }
        public string subItemList { get; set; }
        public string subItemCodeList { get; set; }
        public string itemChName { get; set; }
    }

    [Alias("t_bp_PeoAwards_ST")]
    public partial class t_bp_PeoAwards_ST : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public int? PeopleId { get; set; }
        public string AwaContent { get; set; }
        public string AwaUnit { get; set; }
        public DateTime? AwaDate { get; set; }
    }

    [Alias("t_bp_PeoChange_ST")]
    public partial class t_bp_PeoChange_ST : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public int? PeopleId { get; set; }
        public string ChaContent { get; set; }
        public DateTime? ChaDate { get; set; }
    }

    [Alias("t_bp_PeoEducation_ST")]
    public partial class t_bp_PeoEducation_ST : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public int? PeopleId { get; set; }
        public string TrainContent { get; set; }
        public DateTime? TrainDate { get; set; }
        public string TrainUnit { get; set; }
        public DateTime? TestDate { get; set; }
        public string TestResult { get; set; }
    }

    [Alias("t_bp_People_ST")]
    public partial class t_bp_People_ST : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string Customid { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string PhotoPath { get; set; }
        public string SelfNum { get; set; }
        public string Education { get; set; }
        public string School { get; set; }
        public string Professional { get; set; }
        public string Title { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string SBNum { get; set; }
        public string PostType { get; set; }
        public string PostNum { get; set; }
        public string PostPath { get; set; }
        public DateTime? PostDate { get; set; }
        public string PosrPeople { get; set; }
        public string PostIs { get; set; }
        public string Approvalstatus { get; set; }
        public string postDelayReg { get; set; }
        public string selfnumPath { get; set; }
        public DateTime? postnumstartdate { get; set; }
        public DateTime? postnumenddate { get; set; }
        public string approveadvice { get; set; }
        public int? age { get; set; }
        public string postTypeCode { get; set; }
        public string titlepath { get; set; }
        public string zw { get; set; }
        public string iszcgccs { get; set; }
        public string zcgccszh { get; set; }
        public string zcgccszhpath { get; set; }
        public string educationpath { get; set; }
        public DateTime? zcgccszhstartdate { get; set; }
        public DateTime? zcgccszhenddate { get; set; }
        public string isreghere { get; set; }
        public string iscb { get; set; }
        public string ishaspostnum { get; set; }
        public string ismanager { get; set; }
        public string isjs { get; set; }
        public string issy { get; set; }
        public DateTime? posttime { get; set; }
        public string postname { get; set; }
        public string ispostvalid { get; set; }
        public string titleother { get; set; }
        public string sbnumpath { get; set; }
        public string data_status { get; set; }
    }

    [Alias("t_bp_PeoPunish_ST")]
    public partial class t_bp_PeoPunish_ST : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public int? PeopleId { get; set; }
        public string PunName { get; set; }
        public string PunUnit { get; set; }
        public string PunContent { get; set; }
        public DateTime? PunDate { get; set; }
    }

    [Alias("t_bp_pumpvehicle")]
    public partial class t_bp_pumpvehicle : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string customid { get; set; }
        public string spec { get; set; }
        public int? num { get; set; }
        public string issubunit { get; set; }
        public string variety { get; set; }
    }

    [Alias("t_bp_pumpsystem")]
    public partial class t_bp_pumpsystem : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string customid { get; set; }
        public string type { get; set; }
        public int? num { get; set; }
        public string producefactory { get; set; }
        public string spec { get; set; }
    }

    [Alias("t_bp_JZJNCheck")]
    public partial class t_bp_JZJNCheck
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string CustomId { get; set; }
        public string CheckPeople { get; set; }
        public DateTime? CHECKDate { get; set; }
        public string Remark { get; set; }
        public string JCYPYes { get; set; }
        public string JCYPNo { get; set; }
        public string JCYPQuestion { get; set; }
        public string JZQYYes { get; set; }
        public string JZQYNo { get; set; }
        public string JZQYQuestion { get; set; }
        public string BQXYes { get; set; }
        public string BQXNo { get; set; }
        public string BQXQuestion { get; set; }
        public string YQXYes { get; set; }
        public string YQXNo { get; set; }
        public string YQXQuestion { get; set; }
        public string JLGGYes { get; set; }
        public string JLGGNo { get; set; }
        public string JLGGQuestion { get; set; }
        public string BBWZYes { get; set; }
        public string BBWZNo { get; set; }
        public string BBWZQuestion { get; set; }
        public string GGFHCXYes { get; set; }
        public string GGFHCXNo { get; set; }
        public string GGFHCXQuestion { get; set; }
        public string SPJKYes { get; set; }
        public string SPJKNo { get; set; }
        public string SPJKQuestion { get; set; }
        public string BHGTZYes { get; set; }
        public string BHGTZNo { get; set; }
        public string BHGTZQuestion { get; set; }
    }


    [Alias("t_bp_SZDLCheck")]
    public partial class t_bp_SZDLCheck
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string CustomId { get; set; }
        public string CheckPeople { get; set; }
        public DateTime? CHECKDate { get; set; }
        public string Remark { get; set; }
        public string JZQYYes { get; set; }
        public string JZQYNo { get; set; }
        public string JZQYQuestion { get; set; }
        public string BBWZYes { get; set; }
        public string BBWZNo { get; set; }
        public string BBWZQuestion { get; set; }
        public string GGFHCXYes { get; set; }
        public string GGFHCXNo { get; set; }
        public string GGFHCXQuestion { get; set; }
        public string SPJKYes { get; set; }
        public string SPJKNo { get; set; }
        public string SPJKQuestion { get; set; }
        public string BHGTZYes { get; set; }
        public string BHGTZNo { get; set; }
        public string BHGTZQuestion { get; set; }
    }


    [Alias("t_bp_SLKQCheck")]
    public partial class t_bp_SLKQCheck
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string CustomId { get; set; }
        public string CheckPeople { get; set; }
        public DateTime? CHECKDate { get; set; }
        public string Remark { get; set; }
        public string BGSCYes { get; set; }
        public string BGSCNo { get; set; }
        public string BGSCQuestion { get; set; }
        public string CYSLYes { get; set; }
        public string CYSLNo { get; set; }
        public string CYSLQuestion { get; set; }
        public string BBWZYes { get; set; }
        public string BBWZNo { get; set; }
        public string BBWZQuestionQuestion { get; set; }
        public string YQYXYes { get; set; }
        public string YQYXNo { get; set; }
        public string YQYXQuestion { get; set; }
        public string BGGGYes { get; set; }
        public string BGGGNo { get; set; }
        public string BGGGQuestion { get; set; }
        public string SPJKYes { get; set; }
        public string SPJKNo { get; set; }
        public string SPJKQuestion { get; set; }
        public string BUHGTZYes { get; set; }
        public string BUHGTZNo { get; set; }
        public string BUHGTZQuestion { get; set; }
    }

    [Alias("CheckUnitRecord_LiftingEquipCheck")]
    public partial class CheckUnitRecord_LiftingEquipCheck : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string CustomId { get; set; }
        public string JCBGSCS { get; set; }
        public string JCBGSCQ { get; set; }
        public string BGWZZQS { get; set; }
        public string BGWZZQQ { get; set; }
        public string SCMPXCJCZPS { get; set; }
        public string SCMPXCJCZPQ { get; set; }
        public string SCGCGKFAXXS { get; set; }
        public string SCGCGKFAXXQ { get; set; }
        public string SYYQSBZYXQS { get; set; }
        public string SYYQSBZYXQQ { get; set; }
        public string Remark { get; set; }
        public string CheckPeople { get; set; }
        public DateTime? CheckDate { get; set; }
    }

    [Alias("CheckUnitRecord_MajorStructure")]
    public partial class CheckUnitRecord_MajorStructure : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string CustomId { get; set; }
        public string JCBGSCS { get; set; }
        public string JCBGSCQ { get; set; }
        public string BGWZZQS { get; set; }
        public string BGWZZQQ { get; set; }
        public string CYSLMZGFYQS { get; set; }
        public string CYSLMZGFYQq { get; set; }
        public string SYYQSBZYXQS { get; set; }
        public string SYYQSBZYXQQ { get; set; }
        public string BGGGFHCXS { get; set; }
        public string BGGGFHCXQ { get; set; }
        public string BHGTZS { get; set; }
        public string BHGTZQ { get; set; }
        public string Remark { get; set; }
        public string CheckPeople { get; set; }
        public DateTime? CheckDate { get; set; }
    }

    [Alias("totalitems")]
    public partial class totalitems
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string typecode { get; set; }
        public string itemtype { get; set; }
        public string itemcode { get; set; }
    }
    
    /// <summary>
    /// 检测机构网上基本信息检查记录表
    /// </summary>
    public class t_instcheck
    {
        [Alias("Id")]
        [AutoIncrement]
        public int? Id { get; set; }
        public string CustomId { get; set; }
        public string CustomAddress { get; set; }
        public string zzzs_y { get; set; }
        public string zzzs_w { get; set; }
        public string zzzs_gq { get; set; }
        public string zzzs_wgq { get; set; }
        public string zzzs_wt { get; set; }
        public string jlzs_y { get; set; }
        public string jlzs_w { get; set; }
        public string jlzs_gq { get; set; }
        public string jlzs_wgq { get; set; }
        public string jlzs_wt { get; set; }
        public string zzxx_jzqy { get; set; }
        public string zzxx_djjc { get; set; }
        public string zzxx_ztjg { get; set; }
        public string zzxx_gjg { get; set; }
        public string zzxx_snhj { get; set; }
        public string zzxx_jzmq { get; set; }
        public string zzxx_jzjn { get; set; }
        public string zzxx_szdl { get; set; }
        public string zzxx_szql { get; set; }
        public string zzxx_jzwfssb { get; set; }
        public string zzxx_qzsb { get; set; }
        public string zzxx_qt { get; set; }
        public string zzxx_wt { get; set; }
        public string zcyts { get; set; }
        public string zcyts_mz { get; set; }
        public string zcyts_bmz { get; set; }
        public string zcjgs { get; set; }
        public string zcjgs_mz { get; set; }
        public string zcjgs_bmz { get; set; }
        public string qzsbjys { get; set; }
        public string qzsbjys_mz { get; set; }
        public string qzsbjys_bmz { get; set; }
        public string zzzcszs_wt { get; set; }
        public string zjysjszc_mz { get; set; }
        public string zjysjszc_bmz { get; set; }
        public string gcjszjyscz_mz { get; set; }
        public string gcjszjyscz_bmz { get; set; }
        public string jsfzr_wt { get; set; }
        public string sgxx_mz { get; set; }
        public string sgxx_bmz { get; set; }
        public string sgxx_wt { get; set; }
        public string sbrymxbsmj_y { get; set; }
        public string sbrymxbsmj_w { get; set; }
        public string sbrymxbsmj_q { get; set; }
        public string sbrymxbsmj_wt { get; set; }
        public string jzxx_mz { get; set; }
        public string jzxx_bmz { get; set; }
        public string jzxx_wt { get; set; }
        public string dcs_s { get; set; }
        public string dcs_f { get; set; }
        public int? dcs_ge { get; set; }
        public string dcs_wt { get; set; }
        public string jcry { get; set; }
        public DateTime? jcrq { get; set; }
        public DateTime? adddate { get; set; }
        public string addman { get; set; }
    }
    public class t_jclcheck
    {
        [AutoIncrement]
        public int? Id { get; set; }
        public string CustomId { get; set; }
        public string qyjzxx_s { get; set; }
        public string qyjzxx_f { get; set; }
        public string gyjzxx_wt { get; set; }
        public string jyypewmxx_s { get; set; }
        public string jyypewmxx_f { get; set; }
        public string jyypewmxx_wt { get; set; }
        public string jcxm_bqx_s { get; set; }
        public string jcxm_bqx_f { get; set; }
        public string jcxm_bqx_wt { get; set; }
        public string jcxm_yqx_s { get; set; }
        public string jcxm_yqx_f { get; set; }
        public string jcxm_yqx_wt { get; set; }
        public string jcxm_fhcx_s { get; set; }
        public string jcxm_fhcx_f { get; set; }
        public string jcxm_fhcx_wt { get; set; }
        public string al_s { get; set; }
        public string al_f { get; set; }
        public string al_wt { get; set; }
        public string bl_s { get; set; }
        public string bl_f { get; set; }
        public string bl_wt { get; set; }
        public string cl_s { get; set; }
        public string cl_f { get; set; }
        public string cl_wt { get; set; }
        public string spjkzlbc_s { get; set; }
        public string spjkzlbc_f { get; set; }
        public string spjkzlbc_wt { get; set; }
        public string bhgtz_y { get; set; }
        public string bhgtz_w { get; set; }
        public string bhgtz_wt { get; set; }
        public string beizhu { get; set; }
        public DateTime? jcrq { get; set; }
        public string jcry { get; set; }
        public string addman { get; set; }
        public DateTime? adddate { get; set; }
    }

    public class t_zjxccheck
    {
        [AutoIncrement]
        public int? Id { get; set; }
        public string CustomId { get; set; }
        public string jcbgsc_s { get; set; }
        public string jcbgsc_f { get; set; }
        public string jcbgsc_wt { get; set; }
        public string sjsssc_s { get; set; }
        public string sjsssc_f { get; set; }
        public string sjsssc_wt { get; set; }
        public string scjcryzp_s { get; set; }
        public string scjcryzp_f { get; set; }
        public string scjcryzp_wt { get; set; }
        public string ysjlwph_s { get; set; }
        public string ysjlwph_f { get; set; }
        public string ysjlwph_wt { get; set; }
        public string qzgz_f { get; set; }
        public string qzgz_s { get; set; }
        public string qzgz_wt { get; set; }
        public string yxq_s { get; set; }
        public string yxq_f { get; set; }
        public string yxq_wt { get; set; }
        public string beizhun { get; set; }
        public string jcry { get; set; }
        public DateTime? jcrq { get; set; }
        public string addman { get; set; }
        public DateTime? adddate { get; set; }
    }

    [Alias("tab_qrinfo")]
    public partial class tab_qrinfo : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string qrinfo { get; set; }
        public DateTime? usedate { get; set; }
        public string openid { get; set; }
        public string liftno { get; set; }
    }

    [Alias("tab_qz_photoinfo")]
    public partial class tab_qz_photoinfo : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string customid { get; set; }
        public string checknum { get; set; }
        public string qrpath { get; set; }
        public string photopath { get; set; }
        public string peoplepath { get; set; }
        public string remake { get; set; }
        public decimal? longitude { get; set; }
        public decimal? latitude { get; set; }
        public DateTime? addtime { get; set; }
    }

    [Alias("tab_qz_programme")]
    public partial class tab_qz_programme : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string projectnum { get; set; }
        public string checknum { get; set; }
        public string checktype { get; set; }
        public string testingpeople { get; set; }
        public string witnesspeople { get; set; }
        public string opterpeople { get; set; }
        public string equipmentfactroy { get; set; }
        public string equipmentno { get; set; }
        public string checkequipmentno { get; set; }
        public string installationunit { get; set; }
        public DateTime? addtime { get; set; }
        public int? stuas { get; set; }
        public string theodoliteno { get; set; }
        public string customid { get; set; }
    }

    [Alias("tab_qz_report")]
    public partial class tab_qz_report : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string customid { get; set; }
        public string projectnum { get; set; }
        public string checknum { get; set; }
        public string reportnum { get; set; }
        public DateTime? addtime { get; set; }
        public string itemname { get; set; }
        public string sysprimarykey { get; set; }
        public string title { get; set; }
    }

    [Alias("view_programmeLiftList")]
    public partial class view_programmeLiftList 
    {
      
        public int id { get; set; }
        public string projectnum { get; set; }
        public string checknum { get; set; }
        public string checktype { get; set; }
        public string testingpeople { get; set; }
        public string witnesspeople { get; set; }
        public string opterpeople { get; set; }
        public string equipmentfactroy { get; set; }
        public string equipmentno { get; set; }
        public string checkequipmentno { get; set; }
        public string installationunit { get; set; }
        public DateTime? addtime { get; set; }
        public int? stuas { get; set; }
        public string theodoliteno { get; set; }
        public string customid { get; set; }
        public string projectname { get; set; }
        public string projectregnum { get; set; }
        public string areainfo { get; set; }
        public string constructionunit { get; set; }
        public string supervisionunit { get; set; }
        public string designunit { get; set; }
        public string constructionunits { get; set; }
        public string personinchargename { get; set; }
        public string personinchargetel { get; set; }
        public string projectaddress { get; set; }
        public string customname { get; set; }
        public string qrpath { get; set; }
        public string photopath { get; set; }
        public string peoplepath { get; set; }
        public decimal? longitude { get; set; }
        public decimal? latitude { get; set; }
        public string reportcount { get; set; }
        public string qrinfo { get; set; }
        public string photoid { get; set; }
    }
    [Alias("view_reportbh")]
    public class view_reportbh
    {
        public string customid { get; set; }
        public int? id { get; set; }
        public string DetectUnit { get; set; }
        public string DelegateUnit { get; set; }
        public string TestNo { get; set; }
        public string UserProjName { get; set; }
        public string ProjPart { get; set; }
        public string ImproperDetails { get; set; }
        public string ProductCorpName { get; set; }
        public string TestConclusion { get; set; }
        public string TestType { get; set; }
        public DateTime? TestDate { get; set; }
        public string SYSPRIMARYKEY { get; set; }
        public int? show { get; set; }
        public string SAMPLEDISPOSEPHASE { get; set; }
        public string qrinfo { get; set; }
        public string samplenum { get; set; }
        public string entrustnum { get; set; }
        public string reportnum { get; set; }
        public string slclosedman { get; set; }
        public DateTime? slcloseddate { get; set; }
        public string spnclosedman { get; set; }
        public DateTime? spncloseddate { get; set; }
        public int? status { get; set; }
        public DateTime? showDate { get; set; }
        public string projectnum { get; set; }
        public string WITNESSUNIT { get; set; }
        public string WITNESSMAN { get; set; }
        public string WITNESSMANNUM { get; set; }
        public string WITNESSMANTEL { get; set; }
        public string TAKESAMPLEMANNUM { get; set; }
        public string TAKESAMPLEMANTEL { get; set; }
        public string TAKESAMPLEMAN { get; set; }
        public string itemcode { get; set; }
        public string closurebefore { get; set; }
        public DateTime? time1 { get; set; }
        public string closed { get; set; }
        public DateTime? time2 { get; set; }
        public string closureafter { get; set; }
        public DateTime? time3 { get; set; }
        public DateTime? reportdate { get; set; }
        public string itemname { get; set; }
        public string projectname { get; set; }
        public int? closurebeforenum { get; set; }
        public int? closednum { get; set; }
        public int? closureafternum { get; set; }
        public string customname { get; set; }
        public string area { get; set; }
        public DateTime? customcloseddate { get; set; }
    }

    [Alias("tab_pile_programme")]
    public partial class tab_pile_programme : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        public string unitcode { get; set; }

        /// <summary>
        /// 工程编码
        /// </summary>
        public string projectnum { get; set; }
        /// <summary>
        /// 工程名称
        /// </summary>
        public string projectname { get; set; }
        /// <summary>
        /// 检测流水号
        /// </summary>
        public string checknum { get; set; }

        /// <summary>
        /// 人员id
        /// </summary>
        public string testingpeople { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public string testingequipment { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? planstartdate { get; set; }

        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? planenddate { get; set; }

        /// <summary>
        /// 基础形式
        /// </summary>
        public string basictype { get; set; }

        /// <summary>
        /// 建筑结构形式
        /// </summary>
        public string structuretype { get; set; }

        /// <summary>
        /// 桩型
        /// </summary>
        public string piletype { get; set; }

        /// <summary>
        /// 桩顶/基底标高
        /// </summary>
        public double? elevation { get; set; }

        /// <summary>
        /// 设计总桩数
        /// </summary>
        public double? pilenum { get; set; }

        /// <summary>
        /// 单桩（复合地基）  承载力特征值
        /// </summary>
        public string eigenvalues { get; set; }

        /// <summary>
        /// 设计桩长
        /// </summary>
        public string pilelenght { get; set; }

        /// <summary>
        /// 面积置换率
        /// </summary>
        public double? areadisplacement { get; set; }


        /// <summary>
        /// 设计桩径
        /// </summary>
        public string pilediameter { get; set; }

        /// <summary>
        /// 设计砼强度
        /// </summary>
        public string concretestrength { get; set; }

        /// <summary>
        /// 抽测数量
        /// </summary>
        public int? jzsynum { get; set; }

        /// <summary>
        /// 基桩号
        /// </summary>
        public string jzsynos { get; set; }

        /// <summary>
        /// 方案文件 路径
        /// </summary>
        public string filepath { get; set; }

        /// <summary>
        /// 方案文件 名称
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// 会签页 附件
        /// </summary>
        public string hqfilepath { get; set; }

        /// <summary>
        /// 会签页 名称
        /// </summary>
        public string hqfilename { get; set; }

        public DateTime? createtime { get; set; }

        /// <summary>
        /// 地区信息
        /// </summary>
        public string Area { get; set; }
    }

    public class pile_programme_model : tab_pile_programme
    {
        /// <summary>
        /// 检测人员（多人）
        /// </summary>
        public string testingpeople { get; set; }

        /// <summary>
        /// 检测设备（多设备）
        /// </summary>
        public string testingequipment { get; set; }
    }

    [Alias("t_prog_people")]
    public class t_prog_people
    {
        [Alias("id")]
        [AutoIncrement]
        public int id { get; set; }

        /// <summary>
        /// 方案id
        /// </summary>
        public int progid { get; set; }

        /// <summary>
        /// 检测流水号
        /// </summary>
        public string checknum { get; set; }

        /// <summary>
        /// 检测人员id
        /// </summary>
        public int peopleid { get; set; }

    }

    [Alias("t_prog_equip")]
    public class t_prog_equip
    {
        [Alias("id")]
        [AutoIncrement]
        public int id { get; set; }

        /// <summary>
        /// 方案id
        /// </summary>
        public int progid { get; set; }

        /// <summary>
        /// 检测流水号
        /// </summary>
        public string checknum { get; set; }

        /// <summary>
        /// 检测设备id
        /// </summary>
        public int equipid { get; set; }

    }

    [Alias("t_prog_Image")]
    public partial class t_prog_Image
    {
        [AutoIncrement]
        public int Id { get; set; }
        public int? ProgId { get; set; }
        public string CheckNum { get; set; }
        public int? Status { get; set; }
        public string Path { get; set; }
        public string jzsynos { get; set; }
        /// <summary>
        /// 拍摄时间
        /// </summary>
        public DateTime? TakeTime { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime? UploadTime { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal? Longitude { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public decimal? Latitude { get; set; }
        /// <summary>
        /// 照片描述
        /// </summary>
        public string Description { get; set; }
    }

    [Alias("tab_pile_exception")]
    public partial class tab_pile_exception : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// 单位编码
        /// </summary>
        public string unitcode { get; set; }

        /// <summary>
        /// 工程编码
        /// </summary>
        public string projectnum { get; set; }

        /// <summary>
        /// 检测流水号
        /// </summary>
        public string checknum { get; set; }

        /// <summary>
        /// 试桩编号
        /// </summary>
        public string pileno { get; set; }


        /// <summary>
        /// 上报人
        /// </summary>
        public string people { get; set; }

        /// <summary>
        /// 异常类别
        /// </summary>
        public string typeinfo { get; set; }

        /// <summary>
        /// 异常情况说明
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 照片路径1-3张
        /// </summary>
        public string photo { get; set; }

        /// <summary>
        /// 上报时间
        /// </summary>
        public DateTime? createTime { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>
        public string handleContent { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        public string handlePeople { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime? handleTime { get; set; }

        /// <summary>
        /// 执行回复
        /// </summary>
        public string executeContent { get; set; }

        /// <summary>
        /// 执行人
        /// </summary>
        public string executePeople { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime? executeTime { get; set; }

        /// <summary>
        /// 执行照片1-3张
        /// </summary>
        public string executephoto { get; set; }
    }

    [Alias("t_PileReportLink")]
    public class t_PileReportLink
    {
        [Alias("id")]
        [AutoIncrement]
        public int id { get; set; }
        /// <summary>
        /// 检测流水号，多个用英文逗号隔开
        /// </summary>
        public string CheckNums { get; set; }
        /// <summary>
        /// 检测报告主键
        /// </summary>
        public string SysPrimaryKey { get; set; }
        public DateTime? UploadDt { get; set; }
    }

}
