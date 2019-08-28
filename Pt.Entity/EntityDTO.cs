using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity
{
    [Serializable]
    public class UIArea
    {
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
    }

    [Serializable]
    public class InstShortInfos : Dictionary<string, string>
    {
        public InstShortInfos()
        {

        }

        protected InstShortInfos(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public static InstShortInfos FromDictonary(Dictionary<string, string> data)
        {
            var insts = new InstShortInfos();
            foreach (var item in data)
            {
                insts[item.Key] = item.Value == null ? string.Empty : item.Value;
            }
            return insts;
        }
    }

    [Serializable]
    public class ItemShortInfos : Dictionary<string, string>
    {

        public ItemShortInfos()
        {

        }

        protected ItemShortInfos(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }


        public static ItemShortInfos FromDictonary(Dictionary<string, string> data)
        {
            var insts = new ItemShortInfos();
            foreach (var item in data)
            {
                insts[item.Key] = item.Value == null ? string.Empty : item.Value;
            }
            return insts;
        }

        public static ItemShortInfos FromList(List<ItemKeyWithCNName> itemKeyWithNames)
        {
            var insts = new ItemShortInfos();
            foreach (var keWithName in itemKeyWithNames)
            {
                insts[keWithName.ITEMTABLENAME] = keWithName.ITEMCHNAME;
            }
            return insts;
        }
    }

    [Serializable]
    public class ItemKeyWithCNName
    {
        public string ITEMTABLENAME { get; set; }
        public string ITEMCHNAME { get; set; }
    }
     
    [Serializable]
    public class TypeCodeWithName
    {
        public int Id { get; set; }
        public string CheckItemCode { get; set; }
        public string CheckItemName { get; set; }
    }
    public class TypeCodeEditName : TypeCodeWithName
    {
        public string oldtypecode { get; set; }
    }

    [Serializable]
    public class ItemCodeWithName
    {
        public int Id { get; set; }
        public string typecode { get; set; }
        public string typename { get; set; }
        public string itemcode { get; set; }
        public string itemname { get; set; }
        public string itemtype { get; set; }
    }
    public class ItemCodeEditName : ItemCodeWithName
    {
        public string olditemcode { get; set; }
    }

    [Serializable]
    public class ParmCodeWithName
    {
        public int Id { get; set; }
        public string typecode { get; set; }
        public string typename { get; set; }
        public string itemcode { get; set; }
        public string itemname { get; set; }
        public string parmcode { get; set; }
        public string parmname { get; set; }
        public bool spandays { get; set; }
    }


    public class InstFilter
    {
        public bool NeedFilter { get; set; }

        public List<string> FilterInstIds { get; set; }
    }

    public class InspectFilter
    {
        public bool NeedFilter { get; set; }

        public UserInspect UserInspect { get; set; }
    }

    [Serializable]
    public class UserInspect
    {
        public string InspectId { get; set; }
    }

    [Serializable]
    public class InstWithArea
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string area { get; set; }
    }

    public class UICardChange
    {
        public string CustomName { get; set; }

        public string PeopleName { get; set; }

        public string PostType2 { get; set; }

        public string PostType { get; set; }

    }


    [Serializable]
    public class ComonItemCNName
    {
        public string ItemTableName { get; set; }
        public string ItemCNName { get; set; }
    }

    public class CheckCustomSaveModel
    {
        public int OpUserId { get; set; }
        public t_bp_custom custom { get; set; }
        public List<t_bp_CusAchievement> CusAchievement { get; set; }
        public List<t_bp_CusAward> CusAward { get; set; }
        public List<t_bp_CusPunish> CusPunish { get; set; }
        public List<t_bp_CheckCustom> CheckCustom { get; set; }
        public List<t_bp_CusChange> CusChange { get; set; }
    }


    public class CheckCustomTmpSaveModel
    {
        public int OpUserId { get; set; }
        public t_bp_custom_tmp custom { get; set; }
        public List<t_bp_CusAchievement> CusAchievement { get; set; }
        public List<t_bp_CusAward> CusAward { get; set; }
        public List<t_bp_CusPunish> CusPunish { get; set; }
        public List<t_bp_CheckCustom> CheckCustom { get; set; }
        public List<t_bp_CusChange> CusChange { get; set; }
    }

    public class ApplyCustomSaveModel
    {
        public int OpUserId { get; set; }
        public t_bp_custom_apply custom { get; set; }
        public List<t_bp_CusAchievement> CusAchievement { get; set; }
        public List<t_bp_CusAward> CusAward { get; set; }
        public List<t_bp_CusPunish> CusPunish { get; set; }
        public List<t_bp_CheckCustom> CheckCustom { get; set; }
        public List<t_bp_CusChange> CusChange { get; set; }
    }

    public class ColumnsDiffModel
    {
        public string Column;
        public string ColumnName;
        public string OldValue;
        public string NewValue;
    }

    public class STCheckPeopleSaveModel
    {
      
        public t_bp_People_ST People { get; set; }
        public List<t_bp_PeoAwards_ST> PeoAward { get; set; }
        public List<t_bp_PeoPunish_ST> PeoPunish { get; set; }
        public List<t_bp_PeoEducation_ST> PeoEducation { get; set; }
        public List<t_bp_PeoChange_ST> PeoChange { get; set; }
    }

    public class STCustomSaveModel
    {
        public int OpUserId { get; set; }
        public t_bp_custom_ST custom { get; set; }
        //public List<t_bp_CusAchievement_ST> CusAchievement { get; set; }
        public List<t_bp_CusAwards_ST> stCusAwards { get; set; }
        public List<t_bp_CusPunish_ST> stCusPunishs { get; set; }
        public List<t_bp_CheckCustom_ST> stCustomReChecks { get; set; }
        //public List<t_bp_CusChange_ST>  { get; set; }
        public List<t_bp_pumpsystem> PumpSystems { get;set;}
        public List<t_bp_pumpvehicle> Pumpvehicles { get; set; }
        public List<t_bp_carriervehicle> Carrievechicles { get; set; }
    }

    public class CheckCustomApplyChange
    {
        public string SubmitName { get; set; }
        public string SubmitText { get; set; }
        public string SubmitId { get; set; }
        public string Status { get; set; }
        public bool Result { get; set; }
    }

    public class CheckCustomSaveViewModel : t_bp_custom
    {
        //public WorkExperience[] frWorkExperience { get; set; }
        //public WorkExperience[] jsWorkExperience { get; set; }
        public t_bp_CusAchievement[] cusachievement { get; set; }
        public t_bp_CusAward[] cusawards { get; set; }
        public t_bp_CusPunish[] cuspunish { get; set; }
        public t_bp_CheckCustom[] checkcustom { get; set; }
        public t_bp_CusChange[] cuschange { get; set; }
        public string detectnumdate { get; set; }
        public string measnumDate { get; set; }
    }

    public class ApplyCustomSaveViewModel : t_bp_custom_apply
    {
        public WorkExperience[] frWorkExperience { get; set; }
        public WorkExperience[] jsWorkExperience { get; set; }
        public t_bp_CusAchievement[] cusachievement { get; set; }
        public t_bp_CusAward[] cusawards { get; set; }
        public t_bp_CusPunish[] cuspunish { get; set; }
        public t_bp_CheckCustom[] checkcustom { get; set; }
        public t_bp_CusChange[] cuschange { get; set; }
        public string detectnumdate { get; set; }
        public string measnumDate { get; set; }
    }

    /// <summary>
    /// 工作简历
    /// </summary>
    public class WorkExperience
    {
        public string WorkExperienceTime { get; set; }
        public string WorkExperienceContent { get; set; }
    }

    public class stCustomSaveViewModel : t_bp_custom_ST
    {
        //public t_bp_CusAchievement_ST[] cusachievement { get; set; }
        public t_bp_CusAwards_ST[] stCusAwards { get; set; }
        public List<t_bp_CusPunish_ST> stCusPunishs { get; set; }
        //public t_bp_CheckCustom[] checkcustom { get; set; }
        public t_bp_CheckCustom_ST[] stCustomReChecks { get; set; }
        public string detectnumdate { get; set; }
        public string measnumDate { get; set; }

        public List<t_bp_pumpsystem> PumpSystems { get; set; }
        public List<t_bp_pumpvehicle> Pumpvehicles { get; set; }
        public List<t_bp_carriervehicle> Carrievechicles { get; set; }

    }

    public class CheckEquipSaveModel
    {
        public int OpUserId { get; set; }
        public t_bp_Equipment equipment { get; set; }
        public List<t_bp_equItemSubItemList> Projects { get; set; }
    }

    public class CheckEquipTmpSaveModel
    {
        public int OpUserId { get; set; }
        public t_bp_Equipment_tmp equipment { get; set; }
        public List<t_bp_equItemSubItemList> Projects { get; set; }
    }

    public enum UserCustomType
    {
        /// <summary>
        /// 上传软件版本号
        /// </summary>
        UploadVersion,

        /// <summary>
        /// 上传软件状态
        /// </summary>
        UploadStatus,

        /// <summary>
        /// 上传软件异常
        /// </summary>
        UploadError,

        /// <summary>
        /// 报告上传数量统计
        /// </summary>
        ReportUploadCount,

        /// <summary>
        /// 曲线数据不一致
        /// </summary>
        AcsTimeCout,

        /// <summary>
        /// 用户登录设置的机构
        /// </summary>
        UserLogCustom,

        /// <summary>
        /// 不合格报告数量统计
        /// </summary>
        UnQualifiedCount,

        /// <summary>
        /// 无曲线报告数量统计
        /// </summary>
        NoAcsCount,

        /// <summary>
        /// 异常报告数量统计
        /// </summary>
        AcsExcepCount,
        /// <summary>
        /// 报告数量分析
        /// </summary>
        ReportDataAnalysis
    }

    public class SysSearchModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Num { get; set; }

     

    

        /// <summary>
        /// 
        /// </summary>
        public string SupervisoryName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? IsChanged { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DtType { get; set; }
        public string EntrustUnit { get; set; }
        public string CheckInstID { get; set; }
        public string CheckStatus { get; set; }
        public DateTime? StartDt { get; set; }
        public DateTime? EndDt { get; set; }
        public DateTime? GetStartDt { get; set; }
        public DateTime? GetEndDt { get; set; }
        public string CheckItem { get; set; }
        public string CheckItemCodes { get; set; }
        public int? HasArc { get; set; }
        public string Area { get; set; }
        public string SampleNum { get; set; }

        /// <summary>
        /// 监督抽选类型
        /// </summary>
        public string JDType { get; set; }

        /// <summary>
        /// 报告编号
        /// </summary>
        public string ReportNum { get; set; }
        public int? IsReport { get; set; }

        /// <summary>
        /// 当前检测项目(不合格报告中使用）
        /// </summary>
        public string itemkey { get; set; }

        /// <summary>
        /// 是否根据月份分组统计  按照1月 2月统计数据
        /// </summary>
        public bool IsMonthAgg { get; set; }

        /// <summary>
        /// 报告查询开始时间
        /// </summary>
        public DateTime? ReportStartDt { get; set; }

        /// <summary>
        /// 报告查询结束时间
        /// </summary>
        public DateTime? ReportEndDt { get; set; }
        /// <summary>
        /// 检测类别
        /// </summary>
        public string TestCategories { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        public string DataState { get; set; }

        /// <summary>
        /// 委托编号
        /// </summary>
        public string EntrustNum { get; set; }

        public string SelectTime { get; set; }


        /// <summary>
        /// 建筑施工许可证号(暂无数据）
        /// </summary>
        public string ConstructNum { get; set; }

        public int? orderColInd { get; set; }
        public string direct { get; set; }
        public string ReportStatus { get; set; }
        public int? IsCType { get; set; }
        public string GroupType { get; set; }

        //报告分类统计所加
        public string ParamCodeRaw { get; set; }
        public string SubItemCodeRaw { get; set; }

        /// <summary>
        ///  不合格报告具体的检测项目
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 不合格报告页面中按机构分组
        /// </summary>
        //public string CustomID { get; set; }
        public string CustomId { get; set; }
        /// <summary>
        /// 不合格报告页面中按工程项目分组
        /// </summary>
        public string ProjectNameRaw { get; set; }
        public int? IsRecheck { get; set; }


        /// <summary>
        /// 空号详情对应的流水号前缀
        /// </summary>
        public string ReportNumPrefix { get; set; }

        /// <summary>
        /// 存储从哪个控制器过来的..
        /// </summary>
        public SysSearchModelModelType modelType { get; set; }

        /// <summary>
        /// 存储综合统计点击的按钮是查询还是按月，按天....
        /// </summary>
        public string SearchType { get; set; }

        /// <summary>
        /// 检测工程编码
        /// </summary>
        public string JCPROJECT { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ISCUNIT { get; set; }

        /// <summary>
        /// 检测结论  很长一段文字的那个结论
        /// </summary>
        public string checkConclusion { get; set; }

        /// <summary>
        /// 全局统计报表用，区分按照检测项目分还是按照工程聚合  0 项目  1 工程
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 用来标识在siderbar中点击的类型  （checkstatis中有用,checkstatis中存的是项目编号）  
        /// </summary>
        public string SiderbarType { get; set; }

        /// <summary>
        /// 建设单位
        /// </summary>
        public string ConstructUnit { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
        public string IsUse { get; set; }
    }


    public enum SysSearchModelModelType
    {
        DefaultModel,
        TotalSearch,
        TotalSearchView,
        NoAcsSearch,
        UnQualified,
        AcsTimeStatisc,
        ReportdataAnalysis,
        ReportCategory,
        NoDataUploadAllDetails,
        ModifyReport,
        CheckStatis
    }


    public class SearchResult<T> where T : class
    {
        public SearchResult()
        {
            this.Results = new List<T>();
        }

        public SearchResult(int totalCount, List<T> results)
        {
            this.TotalCount = totalCount;
            this.Results = results;
        }

        public int TotalCount { get; set; }
        public List<T> Results { get; set; }
    }

    public class STCheckEquipEditServiceModel
    {
        public string Customid { get; set; }
        public string EquName { get; set; }
        public string equspec { get; set; }
        public DateTime? checktime { get; set; }
        public string equtype { get; set; }
        public DateTime? buyTime { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public string id { get; set; }
        public string Time { get; set; }
    }

    public class ApplyQualifySaveModel
    {
        public int Id { get; set; }
        public string Area { get; set; }
        public string UnitName { get; set; }
        public string UnitCode { get; set; }
        public DateTime? AddTime { get; set; }
        public string Name { get; set; }
        public string Selfnum { get; set; }
        public string Type { get; set; }
    }

    public class ReportCheck
    {
        public string qrinfo { get; set; }
        public string projectname { get; set; }
        public string reportnum { get; set; }
        public string structpart { get; set; }
        public string itemname { get; set; }
        public string checkparm { get; set; }
        public string checkconclusion { get; set; }
        public string reportdate { get; set; }
        public string itemcode { get; set; }
        public string customid { get; set; }
        public string customname { get; set; }
        public string checkman { get; set; }
        public string projectaddress { get; set; }
        public string ls_ifexceptioninfo { get; set; }
        public string reportvalcode { get; set; }
    }



}
