using ServiceStack.DataAnnotations;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZWebService.ServiceModel
{
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

    [Alias("view_GpsPileInfo")]
    public partial class view_GpsPileInfo
    {
        public int BasicInfoId { get; set; }
        public string SerialNo { get; set; }
        public float GpsLongitude { get; set; }
        public float GpsLatitude { get; set; }
        public string PileNo { get; set; }
        public string projectname { get; set; }
        public int id { get; set; }
        public string areainfo { get; set; }
        public string areaname { get; set; }
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



    [Alias("Dc_BasicInfo")]
    public partial class Dc_BasicInfo : IHasId<int>
    {
        [Alias("BasicInfoId")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string MachineId { get; set; }
        [Required]
        public string SerialNo { get; set; }
        [Required]
        public string PileNo { get; set; }
        [Required]
        public DateTime TestTime { get; set; }
        public float? PileLength { get; set; }
        public string PileDiameter { get; set; }
        public float? PileVelocity { get; set; }
        public string ConcreteStrength { get; set; }
        [Required]
        public byte GpsIsValid { get; set; }
        [Required]
        public double GpsLongitude { get; set; }
        [Required]
        public double GpsLatitude { get; set; }
        public string ShangGangZheng { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public string CreaterName { get; set; }
    }

    [Alias("Dc_Channel")]
    public partial class Dc_Channel : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int BasicInfoId { get; set; }
        [Required]
        public byte SignalType { get; set; }
        [Required]
        public float SampleInterval { get; set; }
        [Required]
        public short SampleGain { get; set; }
        [Required]
        public short SampleLength { get; set; }
        public float? SensorSensitive { get; set; }
        public short? FilterFrequency { get; set; }
        [Required]
        public DateTime SampleTime { get; set; }
        [Required]
        public byte[] ChannelData { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public string CreaterName { get; set; }
    }

    [Alias("Dc_SourceFile")]
    public partial class Dc_SourceFile : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int BasicInfoId { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public string CreaterName { get; set; }
        [Required]
        public byte[] SourceFile { get; set; }
    }

    [Alias("Jy_BasicInfo")]
    public partial class Jy_BasicInfo : IHasId<int>
    {
        [Alias("BasicInfoId")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string MachineId { get; set; }
        [Required]
        public string SerialNo { get; set; }
        [Required]
        public string PileNo { get; set; }
        [Required]
        public string TestType { get; set; }
        [Required]
        public float MaxLoad { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public string SourceParam { get; set; }
        [Required]
        public short RecordCount { get; set; }
        [Required]
        public byte GpsIsValid { get; set; }
        [Required]
        public double GpsLongitude { get; set; }
        [Required]
        public double GpsLatitude { get; set; }
        [Required]
        public byte IsTesting { get; set; }
        public string CurrentParam { get; set; }
        public string CurrentData { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public DateTime UpdateTime { get; set; }
        [Required]
        public string CreaterName { get; set; }
        public string ShangGangZheng { get; set; }
        public DateTime? LastSampleTime { get; set; }
    }

    [Alias("Jy_DetailsData")]
    public partial class Jy_DetailsDatum
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int BasicInfoId { get; set; }
        [Required]
        public DateTime SampleTime { get; set; }
        [Required]
        public byte LoadDirect { get; set; }
        [Required]
        public byte Grade { get; set; }
        [Required]
        public short SampleCount { get; set; }
        [Required]
        public short TimeCount { get; set; }
        [Required]
        public float Loading { get; set; }
        [Required]
        public float RealLoading { get; set; }
        public float? RealPress { get; set; }
        public float? S1 { get; set; }
        public float? S2 { get; set; }
        public float? S3 { get; set; }
        public float? S4 { get; set; }
        public float? S5 { get; set; }
        public float? S6 { get; set; }
        public float? S7 { get; set; }
        public float? S8 { get; set; }
        public float? S9 { get; set; }
        public float? S10 { get; set; }
        public float? S11 { get; set; }
        public float? S12 { get; set; }
        [Required]
        public float SAverage { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public string CreaterName { get; set; }
        public float? U1 { get; set; }
        public float? U2 { get; set; }
        public float? U3 { get; set; }
        public float? U4 { get; set; }
        public float? U5 { get; set; }
        public float? U6 { get; set; }
        public float? U7 { get; set; }
        public float? U8 { get; set; }
        public float? U9 { get; set; }
        public float? U10 { get; set; }
        public float? U11 { get; set; }
        public float? U12 { get; set; }
    }

    [Alias("Jy_TestingLogInfo")]
    public partial class Jy_TestingLogInfo
    {
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int BasicInfoId { get; set; }
        [Required]
        public DateTime EventTime { get; set; }
        [Required]
        public string EventInfo { get; set; }
        public string Remark { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public string CreaterName { get; set; }
    }

    [Alias("Sb_BasicInfo")]
    public partial class Sb_BasicInfo : IHasId<int>
    {
        [Alias("BasicInfoId")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public string MachineId { get; set; }
        [Required]
        public string SerialNo { get; set; }
        [Required]
        public string PileNo { get; set; }
        [Required]
        public DateTime TestTime { get; set; }
        [Required]
        public float PileLength { get; set; }
        public string PileDiameter { get; set; }
        public string ConcreteStrength { get; set; }
        [Required]
        public byte TubeCount { get; set; }
        [Required]
        public byte SectionCount { get; set; }
        [Required]
        public short Step { get; set; }
        [Required]
        public short Angle { get; set; }
        [Required]
        public byte GpsIsValid { get; set; }
        [Required]
        public double GpsLongitude { get; set; }
        [Required]
        public double GpsLatitude { get; set; }
        public string ShangGangZheng { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public string CreaterName { get; set; }
    }

    [Alias("Sb_Section")]
    public partial class Sb_Section : IHasId<int>
    {
        [Alias("Id")]
        [AutoIncrement]
        public int Id { get; set; }
        [Required]
        public int BasicInfoId { get; set; }
        [Required]
        public string SectionName { get; set; }
        [Required]
        public byte TestMode { get; set; }
        [Required]
        public short TubeDistance { get; set; }
        [Required]
        public short Step { get; set; }
        [Required]
        public float ZeroTime { get; set; }
        [Required]
        public float SampleInterval { get; set; }
        [Required]
        public short SampleLength { get; set; }
        [Required]
        public short NodesCount { get; set; }
        [Required]
        public byte DataVersion { get; set; }
        [Required]
        public byte[] SectionData { get; set; }
        [Required]
        public DateTime CreateTime { get; set; }
        [Required]
        public string CreaterName { get; set; }
    }

    [Alias("tab_custominfo")]
    public partial class tab_custominfo : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string customid { get; set; }
        public string name { get; set; }
        public int? projectnum { get; set; }
        public string password { get; set; }
    }

    [Alias("tab_parmslist")]
    public partial class tab_parmslist : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string parmname { get; set; }
        public string parmcode { get; set; }
        public string parmvalue { get; set; }
        public string parmother { get; set; }
    }

    [Alias("tab_projectinfo")]
    public partial class tab_projectinfo : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string projectnum { get; set; }
        public string projectname { get; set; }
        public string projectregnum { get; set; }
        public string witnesspeople { get; set; }
        public string customid { get; set; }
        public string areainfo { get; set; }
        public string constructionunit { get; set; }
        public string supervisionunit { get; set; }
        public string designunit { get; set; }
        public string constructionunits { get; set; }
        public string personinchargename { get; set; }
        public string personinchargetel { get; set; }
        public string projectaddress { get; set; }
        public DateTime? addtime { get; set; }
        public string checknum { get; set; }
    }

    [Alias("tab_qz_testingpeople")]
    public partial class tab_qz_testingperson : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string customid { get; set; }
        public string username { get; set; }
        public string postnum { get; set; }
        public string phone { get; set; }
    }

    [Alias("tab_report_other")]
    public partial class tab_report_other : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string tablename { get; set; }
        public string projectnum { get; set; }
        public string checknum { get; set; }
        public string reportnum { get; set; }
        public DateTime? updatetime { get; set; }
    }

    [Alias("tab_report_updatelog")]
    public partial class tab_report_updatelog : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string tablename { get; set; }
        public string projectnum { get; set; }
        public string checknum { get; set; }
        public string reportnum { get; set; }
        public DateTime? updatetime { get; set; }
    }

    [Alias("tab_update_countersign")]
    public partial class tab_update_countersign : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string checknum { get; set; }
        public string types { get; set; }
        public DateTime? updatetime { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
    }

    [Alias("tab_update_photo")]
    public partial class tab_update_photo : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string checknum { get; set; }
        public string pileno { get; set; }
        public string types { get; set; }
        public DateTime? updatetime { get; set; }
        public string photostatus { get; set; }
        public string photopath { get; set; }
        public decimal? longitude { get; set; }
        public decimal? latitude { get; set; }
        public DateTime? uploadtime { get; set; }
    }

    [Alias("tab_update_programme")]
    public partial class tab_update_programme : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string checknum { get; set; }
        public string types { get; set; }
        public DateTime? updatetime { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
    }

    [Alias("tab_userinfo")]
    public partial class tab_userinfo : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string customid { get; set; }
        public string openid { get; set; }
        public string phone { get; set; }
        public DateTime? addtime { get; set; }
    }

    [Alias("tab_xc_photoinfo")]
    public partial class tab_xc_photoinfo : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string customid { get; set; }
        public string checknum { get; set; }
        public string photopath { get; set; }
        public string peoplepath { get; set; }
        public string remake { get; set; }
        public decimal? longitude { get; set; }
        public decimal? latitude { get; set; }
        public DateTime? addtime { get; set; }
    }

    [Alias("tab_xc_programme")]
    public partial class tab_xc_programme : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string projectnum { get; set; }
        public string checknum { get; set; }
        public string checktype { get; set; }
        public string testingpeople { get; set; }
        public DateTime? planstartdate { get; set; }
        public DateTime? planenddate { get; set; }
        public string structpart { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public string hqfilepath { get; set; }
        public string hqfilename { get; set; }
        public DateTime? addtime { get; set; }
        public int? stuas { get; set; }
        public string customid { get; set; }
    }

    [Alias("tab_xc_report")]
    public partial class tab_xc_report : IHasId<int>
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

    [Alias("tab_zj_exception")]
    public partial class tab_zj_exception : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string unitcode { get; set; }
        public string onsidetestnum { get; set; }
        public string unitname { get; set; }
        public string itemchname { get; set; }
        public string testaddress { get; set; }
        public string testtime { get; set; }
        public string samplenum { get; set; }
        public string outagetime { get; set; }
        public string recoverytime { get; set; }
        public string abnormalsituation { get; set; }
        public string abnormalhandling { get; set; }
        public string applyMan { get; set; }
        public DateTime? applydate { get; set; }
        public string authorizationText { get; set; }
        public string approveMan { get; set; }
        public DateTime? approvedate { get; set; }
        public string dataphase { get; set; }
        public string checknum { get; set; }
        public string projectnum { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
    }

    [Alias("tab_zj_photoinfo")]
    public partial class tab_zj_photoinfo : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string customid { get; set; }
        public string serialno { get; set; }
        public string pileno { get; set; }
        public string photopath { get; set; }
        public string phototype { get; set; }
        public int? photostatus { get; set; }
        public int? testtype { get; set; }
        public decimal? longitude { get; set; }
        public decimal? latitude { get; set; }
        public DateTime? addtime { get; set; }
    }

    [Alias("tab_zj_programme")]
    public partial class tab_zj_programme : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string projectnum { get; set; }
        public string checknum { get; set; }
        public string testingpeople { get; set; }
        public string testingequipment { get; set; }
        public DateTime? planstartdate { get; set; }
        public DateTime? planenddate { get; set; }
        public string basictype { get; set; }
        public string structuretype { get; set; }
        public string piletype { get; set; }
        public double? elevation { get; set; }
        public double? pilenum { get; set; }
        public string eigenvalues { get; set; }
        public string pilelenght { get; set; }
        public double? areadisplacement { get; set; }
        public string pilediameter { get; set; }
        public string concretestrength { get; set; }
        public int? jzsynum { get; set; }
        public string jzsynos { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public DateTime? addtime { get; set; }
        public int? stuas { get; set; }
        public string hqfilepath { get; set; }
        public string hqfilename { get; set; }
        public string customid { get; set; }
    }

    [Alias("tab_zj_report")]
    public partial class tab_zj_report : IHasId<int>
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

    [Alias("tab_zj_updatelog")]
    public partial class tab_zj_updatelog
    {
        [AutoIncrement]
        public int id { get; set; }
        public string customid { get; set; }
        public string customname { get; set; }
        public string basicinfoid { get; set; }
        public string utype { get; set; }
        public string ordervalue { get; set; }
        public string newvalue { get; set; }
        public DateTime? addtime { get; set; }
    }

    [Alias("trigger_info")]
    public partial class trigger_info : IHasId<int>
    {
        [Alias("id")]
        [AutoIncrement]
        public int Id { get; set; }
        public string customid { get; set; }
        public string checknum { get; set; }
        public string pileno { get; set; }
        public int? istesting { get; set; }
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
        public int reportcount { get; set; }
        public string qrinfo { get; set; }
        public int? photoid { get; set; }
    }

    [Alias("view_testingHis")]
    public partial class view_testingHis
    {
        public int pid { get; set; }
        public string projectnum { get; set; }
        public string projectname { get; set; }
        public string name { get; set; }
        public string customid { get; set; }
        public string SerialNo { get; set; }
        public int? num { get; set; }
        public int? difftime { get; set; }
        public string areainfo { get; set; }
        public string projectaddress { get; set; }
        public string testingpeople { get; set; }
        public string witnesspeople { get; set; }
        public string testingequipment { get; set; }
        public string piletype { get; set; }
        public int? reportcount { get; set; }
        public string testtype { get; set; }
    }

    [Alias("view_testingSite")]
    public partial class view_testingSite
    {
        public int? pid { get; set; }
        public string projectnum { get; set; }
        public string projectname { get; set; }
        public string name { get; set; }
        public string customid { get; set; }
        public string SerialNo { get; set; }
        public int? num { get; set; }
        public int? difftime { get; set; }
        public string areainfo { get; set; }
        public string projectaddress { get; set; }
        public string testingpeople { get; set; }
        public string witnesspeople { get; set; }
        public string testingequipment { get; set; }
        public string piletype { get; set; }
        public int? reportcount { get; set; }
        public string testtype { get; set; }
    }

    [Alias("view_projectinfo")]
    public partial class view_projectinfo
    {
        public int Id { get; set; }
        public string projectnum { get; set; }
        public string projectname { get; set; }
        public string projectregnum { get; set; }
        public string witnesspeople { get; set; }
        public string customid { get; set; }
        public string areainfo { get; set; }
        public string constructionunit { get; set; }
        public string supervisionunit { get; set; }
        public string designunit { get; set; }
        public string constructionunits { get; set; }
        public string personinchargename { get; set; }
        public string personinchargetel { get; set; }
        public string projectaddress { get; set; }
        public DateTime? addtime { get; set; }
        public string checknum { get; set; }

        public string areaname { get; set; }
        public string customname { get; set; }
    }

    [Alias("view_programmePileList")]
    public partial class view_programmePileList
    {
        public int Id { get; set; }
        public string projectnum { get; set; }
        public string checknum { get; set; }
        public string testingpeople { get; set; }
        public string testingequipment { get; set; }
        public DateTime? planstartdate { get; set; }
        public DateTime? planenddate { get; set; }
        public string basictype { get; set; }
        public string structuretype { get; set; }
        public string piletype { get; set; }
        public double? elevation { get; set; }
        public double? pilenum { get; set; }
        public string eigenvalues { get; set; }
        public string pilelenght { get; set; }
        public double? areadisplacement { get; set; }
        public string pilediameter { get; set; }
        public string concretestrength { get; set; }
        public int? jzsynum { get; set; }
        public string jzsynos { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public DateTime? addtime { get; set; }
        public int? stuas { get; set; }
        public string hqfilepath { get; set; }
        public string hqfilename { get; set; }
        public string customid { get; set; }
        public string witnesspeople { get; set; }
        public string projectname { get; set; }
        public string projectregnum { get; set; }
        public string areainfo { get; set; }
        public string areaname { get; set; }
        public string constructionunit { get; set; }
        public string supervisionunit { get; set; }
        public string designunit { get; set; }
        public string constructionunits { get; set; }
        public string personinchargename { get; set; }
        public string personinchargetel { get; set; }
        public string projectaddress { get; set; }
        public string customname { get; set; }
        public int? reportcount { get; set; }
    }


    [Alias("view_programmeSecneList")]
    public partial class view_programmeSecneList
    {
        public int Id { get; set; }
        public string projectnum { get; set; }
        public string checknum { get; set; }
        public string checktype { get; set; }
        public string testingpeople { get; set; }
        public DateTime? planstartdate { get; set; }
        public DateTime? planenddate { get; set; }
        public string structpart { get; set; }
        public string filepath { get; set; }
        public string filename { get; set; }
        public string hqfilepath { get; set; }
        public string hqfilename { get; set; }
        public DateTime? addtime { get; set; }
        public int? stuas { get; set; }
        public string customid { get; set; }
        public string witnesspeople { get; set; }
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
        public string photopath { get; set; }
        public string peoplepath { get; set; }
        public decimal? longitude { get; set; }
        public decimal? latitude { get; set; }
        public int reportcount { get; set; }
        public int? photoid { get; set; }
    }

    [Alias("view_pileDataList")]
    public partial class view_pileDataList
    {
        public int id { get; set; }
        public string TestType { get; set; }
        public string customid { get; set; }
        public string checknum { get; set; }
        public string projectnum { get; set; }
        public string projectname { get; set; }
        public string testingpeople { get; set; }
        public DateTime? addtime { get; set; }
        public string SerialNo { get; set; }
        public string PileNo { get; set; }
        public string num { get; set; }
        public int? IsTesting { get; set; }
        public float? GpsLongitude { get; set; }
        public float? GpsLatitude { get; set; }
        public string areainfo { get; set; }
        public DateTime? StartTime { get; set; }
    }

    public class TestSiteDetailsModel : Jy_BasicInfo
    {
        public string checknum { get; set; }
        public string projectname { get; set; }
        public string testingequipment { get; set; }
        public int reportcount { get; set; }
        public int nn { get; set; }
    }

    [Alias("t_bp_item")]
    public partial class t_bp_item : IHasId<string>
    {
        [Alias("SYSPRIMARYKEY")]
        [Required]
        public string Id { get; set; }
        public string PROJECTNUM { get; set; }
        public string ITEMNAME { get; set; }
        public string SENDSAMPLEMAN { get; set; }
        public DateTime? REPORTCONSENTDATE { get; set; }
        public string SAMPLENUM { get; set; }
        public string ENTRUSTNUM { get; set; }
        public string REPORTNUM { get; set; }
        public string SAMPLENAME { get; set; }
        public string STANDARDNAME { get; set; }
        public string CHECKTYPE { get; set; }
        public string STRUCTPART { get; set; }
        public string SAMPLEDISPOSEPHASE { get; set; }
        public string PRODUCEFACTORY { get; set; }
        public DateTime? ENTRUSTDATE { get; set; }
        public DateTime? CHECKDATE { get; set; }
        public DateTime? AUDITINGDATE { get; set; }
        public DateTime? APPROVEDATE { get; set; }
        public DateTime? PRINTDATE { get; set; }
        public string CHECKCONCLUSION { get; set; }
        public string CONCLUSIONCODE { get; set; }
        public string ACCEPTSAMPLEMAN { get; set; }
        public string FIRSTCHECKMAN { get; set; }
        public string SECONDCHECKMAN { get; set; }
        public string VERIFYMAN { get; set; }
        public string AUDITINGMAN { get; set; }
        public string APPROVEMAN { get; set; }
        public string EXTENDMAN { get; set; }
        public string PRINTMAN { get; set; }
        public string EXPLAIN { get; set; }
        public int? REPORTCONSENTDAYS { get; set; }
        public string SAMPLECHARGETYPE { get; set; }
        public double? NEEDCHARGEMONEY { get; set; }
        public string COLLATEMAN { get; set; }
        public DateTime? COLLATEDATE { get; set; }
        public DateTime? VERIFYDATE { get; set; }
        public DateTime? EXTENDDATE { get; set; }
        public string BEFORESTATUS { get; set; }
        public string AFTERSTATUS { get; set; }
        public string TEMPERATURE { get; set; }
        public string HUMIDITY { get; set; }
        public string PDSTANDARDNAME { get; set; }
        public DateTime? FACTCHECKDATE { get; set; }
        public string SENDSAMPLEMANTEL { get; set; }
        public string DATATYPE { get; set; }
        public string REPORTFILE { get; set; }
        public string INVALIDATE { get; set; }
        public string INVALIDATETEXT { get; set; }
        public DateTime? ADDTIME { get; set; }
        public string REPORTSTYLESTYPES { get; set; }
        public string SENDTOWEB { get; set; }
        public string CONSTRACTUNIT { get; set; }
        public string ENTRUSTUNIT { get; set; }
        public string certCheck { get; set; }
        public string PROJECTNAME { get; set; }
        public int? haveACS { get; set; }
        public int? haveLOG { get; set; }
        public DateTime? updatetime { get; set; }
        public string WORKSTATION { get; set; }
        public double? POINTCOUNT { get; set; }
        public string itemChName { get; set; }
        public string SUBITEMLIST { get; set; }
        public string unitCode { get; set; }
        public int? isCreport { get; set; }
        public string QRCODEBAR { get; set; }
        public string supercode { get; set; }
        public string superunit { get; set; }
        public DateTime? AcsTime { get; set; }
        public string TAKESAMPLEMAN { get; set; }
        public string CSIZE { get; set; }
        public string INSTRUMENTNUM { get; set; }
        public string INSTRUMENTNAME { get; set; }
        public string CUSTOMCODE { get; set; }
        public string CODEBAR { get; set; }
        public string COLUMN1 { get; set; }
        public string COLUMN2 { get; set; }
        public string CONTRACTNUM { get; set; }
        public string TASKNUM { get; set; }
        public string RECORDNUM { get; set; }
        public string ReportPath { get; set; }
        public int? ReportTypes { get; set; }
        public int? JDstatic { get; set; }
        public string WITNESSUNIT { get; set; }
        public string WITNESSMAN { get; set; }
        public string WITNESSMANNUM { get; set; }
        public string WITNESSMANTEL { get; set; }
        public string TAKESAMPLEMANNUM { get; set; }
        public string TAKESAMPLEMANTEL { get; set; }
        public string TAKESAMPLEMAN1 { get; set; }
        public string InitialCheckNum { get; set; }
    }

    [Alias("view_pilephoto")]
    public partial class view_pilephoto
    {
        public int id { get; set; }
        public string customid { get; set; }
        public string SerialNo { get; set; }
        public string PileNo { get; set; }
        public string photopath { get; set; }
        public string phototype { get; set; }
        public string photostatus { get; set; }
        public string testtype { get; set; }
        public decimal? longitude { get; set; }
        public decimal? latitude { get; set; }
        public DateTime? addtime { get; set; }
        public string typeinfo { get; set; }
    }
}
