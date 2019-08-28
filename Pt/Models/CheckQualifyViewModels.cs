using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{
    public class CheckQualifyViewModels
    {
        public List<SysDict> CompanyTypes { get; set; }
        public List<SysDict> CompanyStatus { get; set; }
        public List<SysDict> Sex { get; set; }
        public List<SysDict> CheckWork { get; set; }
        public List<t_sys_region> Region { get; set; }
        public List<CusachievementModel> Cusachievement { get; set; }
        public List<CusawardsModel> Cusawards { get; set; }
        public List<CuspunishModel> Cuspunish { get; set; }
        public List<CheckcustomModel> Checkcustom { get; set; }
        public List<CuschangeModel> Cuschange { get; set; }
        public List<FileGridModel> File { get; set; }
        public Dictionary<string, List<SysDict>> CompayTypeList { get; set; }
        public Dictionary<string, string> CheckInsts { get; set; }
        public bool IsAdmin { get; set; }
        public CheckQuailityDetailModel myT_bp_Custom { get; set; }
        public bool DETECTPATHStatus { get; set; }
    }

    public class STQualifyViewModels
    {
        public t_bp_custom_ST myT_bp_Custom { get; set; }
        public Dictionary<string, List<SysDict>> CompayTypeList { get; set; }
        public List<t_sys_region> Region { get; set; }
        public List<STCusachievementModel> Cusachievement { get; set; }
        public List<STCusawardsModel> Cusawards { get; set; }
        //public List<STCuschangeModel> Cuschange { get; set; }
        public List<STCuspunishModel> CusPunish { get; set; }
        public List<STCheckcustomModel> CheckCustom { get; set; }
        public List<FileGridModel> File { get; set; }
        public Dictionary<string, string> Area;
        public int PerCount { get; set; }
        public int ManagerCount { get; set; }
        public int JsglCount { get; set; }//有职称的工程技术人员和经济管理人员
        public int TestCount { get; set; }
        public List<t_bp_pumpsystem> PumpSystems { get; set; }
        public List<t_bp_carriervehicle> Carriervehicles { get; set; }
        public List<t_bp_pumpvehicle> Pumpvehicles { get; set; }
    }


    public class CheckQualifyApplyChangeViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class CheckQualifyPrintSetConfigViewModel
    {
        public string ID { get; set; }
        public string printConfig { get; set; }
    }


    public class ChekQualityUIModel
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string DETECTNUM { get; set; }
        public string MEASNUM { get; set; }
        public string FR { get; set; }
        public string TEL { get; set; }
        public string APPROVALSTATUS { get; set; }
        public string bgfs { get; set; }

        public int? IsUse { get; set; }
        public string zzlbmc { get; set; }
    }


    public class STChekQualityUIModel
    {
        public string Id { get; set; }
        public string NAME { get; set; }
        public string businesspermit { get; set; }
        public string cerfno { get; set; }
        public string FR { get; set; }
        public string TEL { get; set; }
        public string APPROVALSTATUS { get; set; }
    }


    public class STPumpSystemModel
    {
        public List<t_bp_pumpsystem> PumpSystems { get; set; }
    }

    public class STCarriervehicleModel
    {
        public List<t_bp_carriervehicle> Carriervehicles { get; set; }
    }

    public class STPumpvehicleModel
    {
        public List<t_bp_pumpvehicle> Pumpvehicles { get; set; }
    }

    public class CheckQuailifyShortModel
    {
        public string zzlbmc { get; set; }
        public string wjlr { get; set; }
    }



    public class CheckQuailityDetailModel
    {
        [DisplayName("数据上传编码")]
        public string ID { get; set; }
        [DisplayName("检测机构名称")]
        public string NAME { get; set; }
        [DisplayName("所在区域")]
        public string area { get; set; }
        [DisplayName("成立时间")]
        public DateTime? CREATETIME { get; set; }
        [DisplayName("桩基检测机构")]
        public string ispile { get; set; }
        [DisplayName("发证机关")]
        public string BUSINESSNUMUNIT { get; set; }
        [DisplayName("工商营业执照号码")]
        public string BUSINESSNUM { get; set; }
        [DisplayName("机构地址")]
        public string ADDRESS { get; set; }
        [DisplayName("法人手机号码")]
        public string phone { get; set; }
        [DisplayName("经济性质")]
        public string ECONOMICNATURE { get; set; }
        [DisplayName("法人")]
        public string FR { get; set; }
        [DisplayName("检测机构资质证书")]
        public string DETECTNUM { get; set; }
        [DisplayName("计量认证证书发证机关")]
        public string MEASUNIT { get; set; }
        [DisplayName("检测资质证书发证机关")]
        public string detectunit { get; set; }
        [DisplayName("计量认证证书")]
        public string MEASNUM { get; set; }
        [DisplayName("房屋建筑面积")]
        public string HOUSEAREA { get; set; }
        [DisplayName("计量认证证书有效起始日期")]
        public DateTime? detectnumStartDate { get; set; }
        [DisplayName("计量认证证书有效结束日期")]
        public DateTime? detectnumEndDate { get; set; }
        [DisplayName("检测资质证书批准日期")]
        public DateTime? detectappldate { get; set; }
        [DisplayName("计量认证证书批准日期")]
        public DateTime? APPLDATE { get; set; }
        [DisplayName("检测资质证书有效起始日期")]
        public DateTime? measnumStartDate { get; set; }
        [DisplayName("检测资质证书有效期")]
        public DateTime? measnumEndDate { get; set; }
        [DisplayName("检测场地面积")]
        public string DETECTAREA { get; set; }
        [DisplayName("仪器设备总[台]套数")]
        public string instrumentNum { get; set; }
        [DisplayName("设备固定资产原值[万元]")]
        public string INSTRUMENTPRICE { get; set; }
        [DisplayName("联系电话")]
        public string TEL { get; set; }
        [DisplayName("电子邮箱")]
        public string EMAIL { get; set; }
        [DisplayName("注册资金[万元]")]
        public string REGAPRICE { get; set; }
        [DisplayName("社保人员数量")]
        public string shebaopeoplenum { get; set; }
        [DisplayName("邮政编码")]
        public string POSTCODE { get; set; }
        [DisplayName("已取得检测资质项目个数")]
        public string zzxmgs { get; set; }
        [DisplayName("已取得检测资质类别个数")]
        public string zzlbgs { get; set; }
        [DisplayName("已取得检测资质参数个数")]
        public string zzcsgs { get; set; }
        [DisplayName("技术负责人姓名")]
        public string JSNAME { get; set; }
        [DisplayName("技术负责人职称")]
        public string JSTIILE { get; set; }
        [DisplayName("技术负责人相关工作年限")]
        public string JSYEAR { get; set; }
        [DisplayName("质量负责人姓名")]
        public string ZLNAME { get; set; }
        [DisplayName("质量负责人职称")]
        public string ZLTITLE { get; set; }
        [DisplayName("质量负责人相关工作年限")]
        public string ZLYEAR { get; set; }
        [DisplayName("人员总数")]
        public string PERCOUNT { get; set; }
        [DisplayName("中级职称人数")]
        public string MIDPERCOUNT { get; set; }
        [DisplayName("高级职称人数")]
        public string HEIPERCOUNT { get; set; }
        [DisplayName("持证人员数量")]
        public string hasNumPerCount { get; set; }
        [DisplayName("注册岩土工程师人数")]
        public string REGYTSTA { get; set; }
        [DisplayName("注册结构工程师人数")]
        public string REGJGSTA { get; set; }
        [DisplayName("企业性质")]
        public string companytype { get; set; }
        [DisplayName("工商营业执照号码")]
        public string businessnumPath { get; set; }
        [DisplayName("检测机构资质证书")]
        public string DETECTPATH { get; set; }
        [DisplayName("计量认证证书")]
        public string MEASNUMPATH { get; set; }
        [DisplayName("仪器设备总（台）套数总台帐")]
        public string instrumentpath { get; set; }
        [DisplayName("社保人员明细")]
        public string shebaopeoplelistpath { get; set; }
        [DisplayName("状态")]
        public string APPROVALSTATUS { get; set; }
        [DisplayName("仪器资质")]
        public string EquclassId { get; set; }


    }

    public class CheckQualifySearchModel
    {
        public string CheckUnitName { get; set; }
        public string CMANum { get; set; }
        public string CheckCertNum { get; set; }
        public string LegalPeople { get; set; }
        public string companyType { get; set; }
        public string CheckCode { get; set; }
        public string CompanyQualification { get; set; }
        public string QuantityCategory { get; set; }
        public DateTime? CMADStartDt { get; set; }
        public DateTime? CMADEndDt { get; set; }
        public DateTime? CheckCertStartDt { get; set; }
        public DateTime? CheckCertEndDt { get; set; }
        public string status { get; set; }
        public string Area { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
        public int? orderColInd { get; set; }
        public string direct { get; set; }
        public int? IsUse { get; set; }
        public int? ParentId { get; set; }
    }

    public class STQualifyModel
    {
        public string CustomId { get; set; }
        public string CompanyType { get; set; }
        public string FR { get; set; }
        public string Status { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    

    /// <summary>
    /// 单位业绩
    /// </summary>
    public class CusachievementModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string AchievementTime { get; set; }
        public string AchievementContent { get; set; }
        public string AchievementRem { get; set; }
    }

    public class STCusachievementModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string AchievementTime { get; set; }
        public string AchievementContent { get; set; }
        public string AchievementRem { get; set; }
    }


    /// <summary>
    /// 获奖情况
    /// </summary>
    public class CusawardsModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string AwaName { get; set; }
        public string AwaUnit { get; set; }
        public string AwaContent { get; set; }
        public string AwaRem { get; set; }
        public string AwaDate { get; set; }
    }

    public class STCusawardsModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string AwaName { get; set; }
        public string AwaUnit { get; set; }
        public string AwaContent { get; set; }
        public string AwaRem { get; set; }
        public string AwaDate { get; set; }
    }

    

    /// <summary>
    /// 处罚情况
    /// </summary>
    public class CuspunishModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string PunName { get; set; }
        public string PunUnit { get; set; }
        public string PunContent { get; set; }
        public string PunRem { get; set; }
        public string PunDate { get; set; }
    }

    public class STCuspunishModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string PunName { get; set; }
        public string PunUnit { get; set; }
        public string PunContent { get; set; }
        public string PunRem { get; set; }
        public string PunDate { get; set; }
    }

    /// <summary>
    /// 复查情况
    /// </summary>
    public class CheckcustomModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string CheResult { get; set; }
        public string CheRem { get; set; }
        public string CheDate { get; set; }
    }

    public class STCheckcustomModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string CheResult { get; set; }
        public string CheRem { get; set; }
        public string CheDate { get; set; }
    }

    /// <summary>
    /// 变更情况
    /// </summary>
    public class CuschangeModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string ChaDate { get; set; }
        public string ChaContent { get; set; }
        public string ChaRem { get; set; }
    }
    public class STCuschangeModel
    {
        public string Id { get; set; }
        public int Number { get; set; }
        public string ChaDate { get; set; }
        public string ChaContent { get; set; }
        public string ChaRem { get; set; }
    }

    public class FileGridModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string Url { get; set; }
        public int Modify { get; set; }
        public string Type { get; set; } //标识类别，编辑用
    }

    public class AddFileModel
    {
        public Dictionary<string, int> Ids { get; set; }
        public int Modify { get; set; }
        public int Number { get; set; }
    }

    //public class FileModel
    //{
    //    public Dictionary<string, int> Paths { get; set; }
    //    public int Modify { get; set; }
    //    public int Number { get; set; }
    //}

    public class EditCheckQulifyAttachFileModel
    {
        public string path { get; set; }//最后的路径返回用
        public Dictionary<int, string> paths { get; set; }
        public int IsFile { get; set; }
        public string Id { get; set; }
    }

    public class AddtableModel
    {
        public string CheckUnitName { get; set; }
        public string HasCert { get; set; }
        public string TechTitle { get; set; }
        public string IsTech { get; set; }
        public string PeopleStatus { get; set; }

    }

    public class CheckPeoplModel
    {
        public int number { get; set; }
        public int id { get; set; }
        public string Name { get; set; }
        public string CustomName { get; set; }
        public string SelfNum { get; set; }
        public string PostNum { get; set; }
        public string zw { get; set; }
        public string Title { get; set; }
        public string iscb { get; set; }
        public string Approvalstatus { get; set; }
        public DateTime? postdate { get; set; }
        public int? IsUse { get; set; }
    }

    public class AddTablePostModle
    {
        public string CheckPeopl { get; set; }
        public int Count { get; set; }
    }

    public class GetUnitQualificationModel
    {
        public string Wjlr { get; set; }
        public string QualificationTree { get; set; }
        /// <summary>
        /// 标识是否显示保存按钮
        /// </summary>
        public bool IsSaveButton { get; set; }
        public string CustomId { get; set; }
    }

    public class ChildrenModels
    {
        public string label { get; set; }
        public bool check { get; set; }
        public bool spread { get; set; }
    }


    public class CheckQualifyUpdateNameModel
    {
        public string id { get; set; }
        public string CustomName { get; set; }
    }
}


