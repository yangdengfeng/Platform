using Pkpm.Entity;
using Pkpm.Entity.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class TotalSearchModels
    {
        public string ProjectName { get; set; }
        public int? IsChanged { get; set; }
        public string DtType { get; set; }
        public string TestCategories { get; set; }
        public string CheckItem { get; set; }
        public string CheckStatus { get; set; }
        public DateTime? StartDt { get; set; }
        public DateTime? EndDt { get; set; }
        public string DataState { get; set; }
        public string Area { get; set; }
        public int? HasArc { get; set; }
        public string ReportNum { get; set; }
        public string SampleNum { get; set; }
        public string EntrustNum { get; set; }
        public SysSearchModelModelType modelType { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class ReportDetailViewModel
    {
        public es_t_bp_item MainItem { get; set; }
        public string CustomName { get; set; }
        public bool IsCTypeReport { get; set; }
        public bool HasQrCodeBar { get; set; }
        public bool HasWxSchedule { get; set; }
        public string ItemCNName { get; set; }
        public List<TotalSearchAcsTimeModel> acsTimeModel { get; set; }
        public List<TotalSearchCReportModel> cReportModel { get; set; }
        public List<TotalSearchCommonReportModel> commonReprtModel { get; set; }
        public List<TotalSearchModifyModel> modifyModel { get; set; }
        public List<TotalSearchScheduleModel> scheduleModel { get; set; }
        public List<TotalSearchSampleNum> chuJianModel { get; set; }
        public List<TotalSearchSampleNum> fuJianModel { get; set; }
        public string ENTRUSTDATE { get; set; }
        public string AUDITINGDATE { get; set; }
        public string APPROVEDATE { get; set; }
        public string CHECKDATE { get; set; }
        public string PRINTDATE { get; set; }
    }

    public class TotalSearchAcsTimeModel
    {
        public string SysPrimaryKey { get; set; }
        public string index { get; set; }
        public string SAMPLENUM { get; set; }
        public string ACSTIME { get; set; }
        public string MAXVALUE { get; set; }
    }
    public class TotalSearchCReportModel
    {
        public string SysPrimartKey { get; set; }
        public string index { get; set; }
        public string UploadTime { get; set; }
        public string ReportTypes { get; set; }
        public string Id { get; set; }
        public string FileType { get; set; }
    }

    public class TotalSearchCommonReportModel
    {
        public string index { get; set; }
        public string UpLoadTime { get; set; }
        public string ReportNum { get; set; }
        public string ReportPath { get; set; }
        public string Id { get; set; }
    }

    public class TotalSearchModifyModel
    {
        public string index { get; set; }
        public string FieldName { get; set; }
        public string BeforeModifyValues { get; set; }
        public string AfterModifyValues { get; set; }
        public string ModifyDateTime { get; set; }
        public string QuestionPrimaryKey { get; set; }
    }

    public class TotalSearchScheduleModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string CheckTypeName { get; set; }
        public string CheckItemName { get; set; }
        public string CheckParamName { get; set; }
        public string Status { get; set; }
        public string DeclareDate { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public string Times { get; set; }
        public string FinishTime { get; set; }
    }


    //初检报告
    public class TotalSearchSampleNum
    {
        public string SysPrimaryKey { get; set; }
        public string Index { get; set; }
        public string CustomName { get; set; }
        public string CustomId { get; set; }
        public string ProjectName { get; set; }
        public string Structpart { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string CheckDate { get; set; }
        public string PrintDate { get; set; }
        public string SampleNum { get; set; }
        public string ReportNum { get; set; }
        public string IsUrl { get; set; }//标识是否有下划线

    }

    public class TotalSearchQuestionModel
    {
        public string index { get; set; }
        public string RecordTime { get; set; }
        public string RecordingPhase { get; set; }
        public string QuestionType { get; set; }
        public string Context { get; set; }
        public string RecordMan { get; set; }
    }


    public class ModifyRecordDetailsModel
    {
        public List<TotalSearchQuestionModel> ModifyDetailsModel { get; set; }
    }

    //public class ReportDetailViewModel
    //{
    //    public es_t_bp_item MainItem { get; set; }
    //    public string ItemCNName { get; set; }
    //    public bool IsCTypeReport { get; set; }
    //    public string CheckItemNormal { get; set; }
    //    public bool HasWxSchedule { get; set; }
    //    public bool HasQrCodeBar { get; set; }
    //    public string FujianID { get; set; }
    //    public string CutomName { get; set; }
    //    public List<TotalSearchAcsTimeModel> acsTimeModel { get; set; }
    //    public List<TotalSearchCReportModel> cReportModel { get; set; }
    //    public List<TotalSearchCommonReportModel> commonReprtModel { get; set; }
    //    public List<TotalSearchModifyModel> modifyModel { get; set; }
    //    public List<TotalSearchScheduleModel> scheduleModel { get; set; }
    //    public List<TotalSearchSampleNum> chuJianModel { get; set; }
    //    public List<TotalSearchSampleNum> fuJianModel { get; set; }
    //}

    public class ReportBHSearchModel
    {
        public string CheckInst { get; set; }
        public string Area { get; set; }
        public string ItemName { get; set; }
        public DateTime? StartDt { get; set; }
        public DateTime? EndDt { get; set; }
        public int? IsBH { get; set; }
        public string ProjectName { get; set; }
        public string EntrustNo { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class QrCodeOthersinfoModel
    {
        public string QRNum { get; set; }
        public string cameraImage { get; set; }
        public string verifyImage { get; set; }
    }

    public class QrCodeUpdatedataModel
    {
        public int Id { get; set; }
        public string qrinfo { get; set; }
        public string phones { get; set; }
        public string columneg { get; set; }
        public string columnch { get; set; }
        public string oldvalue { get; set; }
        public string newvalue { get; set; }
        public DateTime? date { get; set; }
        public string DateStr { get; set; }
    }

    public class QrCodeColumnsinfoModel
    {
        public int Id { get; set; }
        public string Columns { get; set; }
        public string Columnsbs { get; set; }
        public string Names { get; set; }
        public string Itemcode { get; set; }
        public string Itemcodebs { get; set; }
        public string Seq { get; set; }
        public string Columnstype { get; set; }
        public string ColChartypeumns { get; set; }
        public string Charvalues { get; set; }
        public string Value { get; set; }
    }

    public class QrCodeProjectinfoModel
    {
        public int Id { get; set; }
        public string Supervisenum { get; set; }
        public string Projectname { get; set; }
        public string Projectaddress { get; set; }
        public string Projectarea { get; set; }
        public string Constractunit { get; set; }
        public string Constractunitman { get; set; }
        public string Constractionunit { get; set; }
        public string Constractionunitman { get; set; }
        public string Designunit { get; set; }
        public string Designunitman { get; set; }
        public string Investigateunit { get; set; }
        public string Investigateunitman { get; set; }
        public string Superunit { get; set; }
        public string Superman { get; set; }
        public string Inspectunit { get; set; }
        public string Inspectman { get; set; }
        public string Addtime { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public string Isno { get; set; }
        public string Peopleid { get; set; }
        public string Projectareas { get; set; }
        public string Status { get; set; }
        public string Approvalstatus { get; set; }
        public string Approvalpeople { get; set; }
        public string Approvaldate { get; set; }
        public string Approvaltip { get; set; }
        public string Inspeccode { get; set; }
        public string Slpeoplevalue { get; set; }
        public string Spnpeoplevalue { get; set; }
        public string Jlpeoplevalue { get; set; }
        public string Zjpeoplevalue { get; set; }
        public decimal? Longitude1 { get; set; }
        public decimal? Latitude1 { get; set; }
        public decimal? Longitude2 { get; set; }
        public decimal? Latitude2 { get; set; }
        public string Cjcount { get; set; }
        public string Checktype { get; set; }
        public string Areacode { get; set; }
        public string Bprojectid { get; set; }
        public string Inspectunitcode { get; set; }
        public string Customid { get; set; }
        public string Customname { get; set; }
        public string Validtime { get; set; }
        public string Entrustunit { get; set; }
    }

    public class QrCodeItemInfoModel
    {
        public string Id { get; set; }
        public string QRINFO { get; set; }
        public string QRSTATUS { get; set; }
        public string PROJECTID { get; set; }
        public string PROJECTNAME { get; set; }
        public string CPROJECTNAME { get; set; }
        public string CONSTRACTIONUNIT { get; set; }
        public string ENTRUSTUNIT { get; set; }
        public string INSPECTUNIT { get; set; }
        public string INSPECTCODE { get; set; }
        public string CUSTOMID { get; set; }
        public string CUSTOMNAME { get; set; }
        public string STRUCTPART { get; set; }
        public string ITEMNAME { get; set; }
        public string ITEMCODE { get; set; }
        public string ITEMJSON { get; set; }

        public string VALCODE { get; set; }

        public string SLID { get; set; }
        public string SLNAME { get; set; }
        public string SLPOSTNUM { get; set; }
        public string SLPHONES { get; set; }
        public decimal? SLLONG { get; set; }
        public decimal? SLLAT { get; set; }
        public string SLIMGPHOTO { get; set; }
        public string SLIMGQR { get; set; }
        public string SLIMGPEOPLE { get; set; }
        public string SLIMGPHONE { get; set; }
        public string SLSCORE { get; set; }
        public DateTime? SLDATE { get; set; }
        public DateTime? SLCHECKTIME { get; set; }

        public string SPNID { get; set; }
        public string SPNNAME { get; set; }
        public string SPNPOSTNUM { get; set; }
        public string SPNPHONES { get; set; }
        public decimal? SPNLONG { get; set; }
        public decimal? SPNLAT { get; set; }
        public string SPNIMGPHOTO { get; set; }
        public string SPNIMGQR { get; set; }
        public string SPNIMGPEOPLE { get; set; }
        public string SPNIMGPHONE { get; set; }
        public string SPNSCORE { get; set; }
        public DateTime? SPNDATE { get; set; }


        public string REMID { get; set; }
        public string REMNAME { get; set; }
        public string REMPOSTNUM { get; set; }
        public string REMPHONES { get; set; }
        public decimal? REMLONG { get; set; }
        public decimal? REMLAT { get; set; }
        public string REMIMGPHOTO { get; set; }
        public string REMIMGQR { get; set; }
        public string REMIMGPEOPLE { get; set; }
        public string REMIMGPHONE { get; set; }
        public string REMSCORE { get; set; }
        public DateTime? REMDATE { get; set; }

        public string UTYPES { get; set; }
        public string ISHIDE { get; set; }
        public string ISORDER { get; set; }
        public string ISCHECK { get; set; }
        public string CHECKPHOTO { get; set; }
        public DateTime? CHECKTIME { get; set; }
        public string CHECKCUSTOM { get; set; }
        public int SAMPLESTATUS { get; set; }
        public string SAMPLENUM { get; set; }
        public DateTime? ENTRUSTDATE { get; set; }
        public string ENTRUSTNUM { get; set; }
        public DateTime? CHECKDATE { get; set; }
        public DateTime? REPORTDATE { get; set; }
        public string REPORTNUM { get; set; }
        public string CONCLUSIONCODE { get; set; }
        public string RESERVECOLUMN1 { get; set; }
        public string RESERVECOLUMN2 { get; set; }
        public string RESERVECOLUMN3 { get; set; }
        public string RESERVECOLUMN4 { get; set; }
        public string RESERVECOLUMN5 { get; set; }
        public string entrustdate { get; set; }
        public string remdate { get; set; }
        public string spndate { get; set; }
        public string sldate { get; set; }

    }

    public class QrCodelResultsModel
    {
        public string results { get; set; }
        public QrCodelDetailModel reason { get; set; }
    }
    public class QrCodelDetailModel
    {
        public string qrinfo { get; set; }
        public QrCodeItemInfoModel Iteminfo { get; set; }
        public QrCodeProjectinfoModel projectinfo { get; set; }
        public List<QrCodeColumnsinfoModel> columnsinfo { get; set; }
        public List<QrCodeUpdatedataModel> updatedata { get; set; }
        public List<QrCodeOthersinfoModel> othersinfo { get; set; }
        public List<SampleInfoNameAndKeyt> sampleInfo { get; set; }
    }
    public class QrCodelItemJsonModel
    {
        public string structpart { get; set; }
        public string shejidengji { get; set; }
        public string chengxingriqi { get; set; }
        public string chicun { get; set; }
        public string producefactory { get; set; }
        public string xingzhuang { get; set; }
        public string yanghutiaojian { get; set; }
        public string chengxingfangfa { get; set; }
        public string ljwd { get; set; }
        public string yqlq { get; set; }
        public string samplename { get; set; }
        public string deputybatch { get; set; }
        public string explain { get; set; }
        public string pihao { get; set; }
        public string ghjc_num { get; set; }
        public string gcpz { get; set; }
        public string da { get; set; }
        public string db { get; set; }
        public string sampleamount { get; set; }
        public string hjhs { get; set; }
        public string hg_man { get; set; }
        public string hg_no { get; set; }
        public string paihao { get; set; }
        public string shejiqiangdudengji { get; set; }
        public string shajiangpinzhong { get; set; }
        public string size { get; set; }
        public string chengxingdate { get; set; }
        public string importdatanum { get; set; }
        public string testbatch { get; set; }
        public string ycbh { get; set; }
        public string oper_man { get; set; }
        public string oper_no { get; set; }
        public string jtlx { get; set; }
        public string gjjb { get; set; }
        public string chkform { get; set; }
        public string gjpz { get; set; }
        public string hg_code { get; set; }
        public string kzdj { get; set; }
        public string ptpaihao { get; set; }
        public string jgzt { get; set; }
        public string jgtype { get; set; }
        public string pingzhong { get; set; }
        public string chanpindengji { get; set; }
        public string shejiqiangdu { get; set; }
        public string size1 { get; set; }
        public string size2 { get; set; }
        public string size3 { get; set; }
        public string midudengji { get; set; }
        public string fenghuaquyv { get; set; }
        public string producedate { get; set; }
        public string c_grade { get; set; }
        public string c_sort { get; set; }
        public string sbiao { get; set; }
        public string samplestatus { get; set; }
        public string c_paihao { get; set; }
        public string qiangdudengji { get; set; }
        public string qktype { get; set; }
        public string diqu { get; set; }
        public string linqi { get; set; }
        public string chandi { get; set; }
        public string pdksdj { get; set; }
        public string sjqddj { get; set; }
        public string chxdate { get; set; }
        public string lingqi { get; set; }
        public string position { get; set; }
        public string spec { get; set; }
        public string requestcheckdate { get; set; }
        public string sz_leib { get; set; }
        public string sz_variety { get; set; }
        public string silx { get; set; }
        public string producearea { get; set; }
    }

    public class SampleInfoNameAndKeyt
    {
        public string Name { get; set; }
        public string Value { get; set; }

    }
    public class HBDetailsModels
    {
        public BHDetailsModel BH { get; set; }
    }


    public class BHChangeDetailsModels
    {
        public BHChangeDetailsModel BHChangeDetails { get; set; }
        public string oldphoto { get; set; }
        public string oldtime { get; set; }
    }

    public class BHChangeDetailsModel
    {
        public string total { get; set; }
        public List<BHChangeDetailsRowModel> rows { get; set; }
    }

    public class BHChangeDetailsRowModel
    {
        public string rowid { get; set; }
        public string id { get; set; }
        public string sysprimarykey { get; set; }
        public string status { get; set; }
        public string oldphoto { get; set; }
        public string oldtime { get; set; }
        public string newphoto { get; set; }
        public string newtime { get; set; }
        
    }


    public class BHDetailsModel
    {
        public string rowid { get; set; }
        public string customid { get; set; }
        public string id { get; set; }
        public string detectunit { get; set; }
        public string delegateunit { get; set; }
        public string testno { get; set; }
        public string userprojname { get; set; }
        public string projpart { get; set; }
        public string improperdetails { get; set; }
        public string productcorpname { get; set; }
        public string testconclusion { get; set; }
        public string testtype { get; set; }
        public string testdate { get; set; }
        public string sysprimarykey { get; set; }
        public string show { get; set; }
        public string sampledisposephase { get; set; }
        public string qrinfo { get; set; }
        public string samplenum { get; set; }
        public string entrustnum { get; set; }
        public string reportnum { get; set; }
        public string slclosedman { get; set; }
        public string slcloseddate { get; set; }
        public string spnclosedman { get; set; }
        public string spncloseddate { get; set; }
        public string status { get; set; }
        public string showdate { get; set; }
        public string projectnum { get; set; }
        public string witnessunit { get; set; }
        public string witnessman { get; set; }
        public string witnessmannum { get; set; }
        public string witnessmantel { get; set; }
        public string takesamplemannum { get; set; }
        public string takesamplemantel { get; set; }
        public string takesampleman { get; set; }
        public string itemcode { get; set; }
        public string closurebefore { get; set; }
        public string time1 { get; set; }
        public string closed { get; set; }
        public string time2 { get; set; }
        public string closureafter { get; set; }
        public string time3 { get; set; }
        public string reportdate { get; set; }
        public string itemname { get; set; }
        public string projectname { get; set; }
        public string closurebeforenum { get; set; }
        public string closednum { get; set; }
        public string closureafternum { get; set; }
        public string customname { get; set; }
        public string area { get; set; }
        public string customcloseddate { get; set; }
    }

    public class IsUseModel
    {
        public string ID { get; set; }
        public string NAME { get; set; }

    }

}