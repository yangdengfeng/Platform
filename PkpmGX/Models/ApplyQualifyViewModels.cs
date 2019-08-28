using Pkpm.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PkpmGX.Models
{

    public class ApplyQualifyTwoUIModels : ApplyQualifyTwoViewModels
    {
        public List<SysDict> CompanyTypes { get; set; }
        public List<SysDict> CompanyStatus { get; set; }
        public List<SysDict> Sex { get; set; }
        public List<SysDict> CheckWork { get; set; }
        public List<SysDict> PersonnelStaff { get; set; }
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
        public bool DETECTPATHStatus { get; set; }
    }

    public class ApplyQualifyTwoViewModels
    {
        public ApplyQuailityDetailModel myT_bp_Custom { get; set; }

        public ApplyQualifyTwoSaveModels myT_D_UserTableTwo { get; set; }
    }


    public class ApplyQualifyTwoSaveModels
    {
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


    public class DetailQualifyFiveSearchModel
    {
        public int? posStart { get; set; }
        public int? count { get; set; }
        public string pid { get; set; }
    }

    public class DetailQualifyFiveSearchViewModel
    {
        public string name { get; set; }
        public string zgzsh { get; set; }
        public string zcdwmc { get; set; }
        public string zc { get; set; }
        public string zy { get; set; }
        public string xl { get; set; }
        public string jcnx { get; set; }
        public string jclb { get; set; }
        public string jclr { get; set; }
        public string xlpath { get; set; }
        public string zcpath { get; set; }
        public string zgzshpath { get; set; }
        public string bz { get; set; }
        public string detectnumstartdate { get; set; }
        public string detectnumenddate { get; set; }
        public string sgzsh { get; set; }
        public string sgzshpath { get; set; }
      
    }


    public class UserTableFiveModel : DetailQualifyFiveSearchViewModel
    {
        public int id { get; set; }
        public int? Staitc { get; set; }
        public string pid { get; set; }
        public string unitcode { get; set; }
        public List<ApplyQualifyFileModel> file { get; set; }
    }

    public class ApplyQualifyFileModel
    {
        public int Number { get; set; }
        public string FileName { get; set; }
        public string FileState { get; set; }
        public string Path { get; set; }
    }

    public class ApplyQualifyOneAuditViewModel
    {
        public int pid { get; set; }
        public string slbh { get; set; }
        public DateTime? sltime { get; set; }
    }

    public class ApplyQualifyOneViewModel
    {
        public List<t_sys_region> Region { get; set; }
        public int Id { get; set; }
        public string UnitCode { get; set; }
        public string UnitName { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string Selfnum { get; set; }
        public DateTime? AddTime { get; set; }
    }

    public class ApplyQualityUIModel
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public string UNITCODE { get; set; }
        public string UNITNAME { get; set; }
        public string SELFNUM { get; set; }
        public string Time { get; set; }
        public string onepath_zl { get; set; }
        public string twopath_zl { get; set; }
        public string threepath_zl { get; set; }
        public string Fourpath_zl { get; set; }
        public string fivepath_zl { get; set; }
        public string Sixpath_zl { get; set; }
        public string Sevenpath_zl { get; set; }
        public string Type { get; set; }
        public string Area { get; set; }
        public int? @static { get; set; }
    }

    public class ApplyQualifySearchModel
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
    }


    public class QualifyPrintUIModel
    {
        public string unitname { get; set; }
        public DateTime? time { get; set; }
        public string EndTime { get; set; }
        public string ImageUrl { get; set; }
    }


    public class UnitQualifyUIModel
    {
        public string ID { get; set; }
        public string UnitName { get; set; }
        public string FRDB { get; set; }
        public string SSFZR { get; set; }
        
        public string YYJCZZ { get; set; }
        public string OnesSb { get; set; }
        public string MeasnumPath { get; set; }
        public string sqjcyw { get; set; }
    }

    public class ExpertUnitQualifySaveModel
    {
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
        public int? pid { get; set; }

        public string OnesSb { get; set; }
        public string MeasnumPath { get; set; }
        public string sqjcyw { get; set; }
    }

    public class SpecialQualifySaveModel
    {
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
        public int? pid { get; set; }

        public string OnesSb { get; set; }
        public string MeasnumPath { get; set; }
    }


    public class CBRUnitSearchModel
    {
        public int id { get; set; }
        public string CheckUnitName { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
        public int? orderColInd { get; set; }
        public string direct { get; set; }
    }

    public class CBRUnitUIModel
    {
        public int id { get; set; }
        public int? pid { get; set; }
        public string unitname { get; set; }
        public int? zjsp1 { get; set; }
        public int? zjsp2 { get; set; }
        public string slr { get; set; }
        public string slbh { get; set; }
        public DateTime? sltime { get; set; }
        public string cbr { get; set; }
        public int? @static { get; set; }
        public string outstaticinfo { get; set; }
    }

    public class CBRUnitSaveModel
    {
        public int? pid { get; set; }
        public string content { get; set; }
        public string csbh { get; set; }
    }

    public class CBRUnitApprovalUIModel
    {
        public int id { get; set; }
        public int? pid { get; set; }
        public string ThreeFZr { get; set; }
    }

    public class CBRUnitApprovalSaveModel
    {
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


    public class ApplyQualifyApplyChangeViewModel
    {
        public string ID { get; set; }
        public string Name { get; set; }
    }

    public class ApplyQualifyPrintSetConfigViewModel
    {
        public string ID { get; set; }
        public string printConfig { get; set; }
    }


    public class ApplyQuailityDetailModel
    {
        [DisplayName("数据上传编码")]
        public string ID { get; set;}
        [DisplayName("pid")]
        public string pid { get; set; }
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
        [DisplayName("传真")]
        public string fax { get; set; }
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

        [DisplayName("申报人")]
        public string SQname { get; set; }
        [DisplayName("申报人联系电话")]
        public string SQTel { get; set; }
        [DisplayName("申请检测业务内容")]
        public string sqjcyw { get; set; }

        [DisplayName("法人性别")]
        public string FRSex { get; set; }
        [DisplayName("法人出生年月")]
        public string FRBirth { get; set; }
        [DisplayName("法人职称")]
        public string FRTITLE { get; set; }
        [DisplayName("法人工作年限")]
        public string FRYEAR { get; set; }
        [DisplayName("法人毕业时间")]
        public string FRGraduationTime { get; set; }
        [DisplayName("法人毕业院校")]
        public string FRCollege { get; set; }
        [DisplayName("法人学历")]
        public string FREducation { get; set; }
        [DisplayName("法人专业")]
        public string FRSubject { get; set; }
        [DisplayName("法人办公电话")]
        public string FRTel { get; set; }
        [DisplayName("法人移动电话")]
        public string FRMobile { get; set; }
        [DisplayName("法人工作简历")]
        public string frgzjl { get; set; }

        [DisplayName("技术负责人性别")]
        public string JSSex { get; set; }
        [DisplayName("技术负责人出生年月")]
        public string JSBirth { get; set; }
        [DisplayName("技术负责人毕业时间")]
        public string JSGraduationTime { get; set; }
        [DisplayName("技术负责人毕业院校")]
        public string JSCollege { get; set; }
        [DisplayName("技术负责人学历")]
        public string JSEducation { get; set; }
        [DisplayName("技术负责人专业")]
        public string JSSubject { get; set; }
        [DisplayName("技术负责人办公电话")]
        public string JSTel { get; set; }
        [DisplayName("技术负责人移动电话")]
        public string JSMobile { get; set; }
        [DisplayName("技术负责人工作简历")]
        public string jsgzjl { get; set; }

        [DisplayName("办公场所证明")]
        public string bgcszmPath { get; set; }
        [DisplayName("资信证明文件")]
        public string zxzmPath { get; set; }
        [DisplayName("股权证明文件")]
        public string gqzmPath { get; set; }
        [DisplayName("质量手册")]
        public string zzscwjPath { get; set; }
        [DisplayName("管理手册")]
        public string glscwjPath { get; set; }

        [DisplayName("法人职称图片")]
        public string frzcpath { get; set; }
        [DisplayName("法人学历图片")]
        public string frxlpath { get; set; }
        [DisplayName("技术负责人职称图片")]
        public string jszcpath { get; set; }
        [DisplayName("技术负责人学历图片")]
        public string jsxlpath { get; set; }
    }

    public class DetailQualifyThreeVeiwModel
    {
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
        public List<DetailQualifyThreeVeiwDataModel> Gzjl { get; set; }
        public List<DetailQualifyThreeVeiwPhotoModel> Fj { get; set; }
    }


    public class DetailQualifyThreeVeiwDataModel
    {
        public string Date { get; set; }
        public string Type { get; set; }
    }

    public class DetailQualifyThreeVeiwPhotoModel
    {
        public string name { get; set; }
        public string path { get; set; }
        public string state { get; set; }
    }

    public class EditQualifyThreeVeiwModel : DetailQualifyThreeVeiwModel
    {
        public List<SysDict> PersonnelStaff { get; set; }
        public Dictionary<string, List<SysDict>> CompayTypeList { get; set; }
        public List<FileGridModel> File { get; set; }
        public List<DetailQualifyThreeVeiwDataModel> Sex { get; set; }
    }

    public class EditQualifyThreeSaveMdoel
    {
        public string photopath { get; set; }
        public string xlpath { get; set; }
        public string zcpath { get; set; }
        public string pid { get; set; }
        public string name { get; set; }
        public string sex { get; set; }
        public DateTime? time { get; set; }
        public string zw { get; set; }
        public string xl { get; set; }
        public string bgdh { get; set; }
        public string yddh { get; set; }
        public string jcgzlx { get; set; }
        public string hshxhzy { get; set; }
        public string postNum { get; set; }
        public string zc { get; set; }
        public string gzjl { get; set; }
        public string postNumpath { get; set; }
        public List<gzjlModel> gzjls { get; set; }
    }

    public class gzjlModel
    {
        public string WorkExperienceTime { get; set; }
        public string WorkExperienceContent { get; set; }
    }

    public class DetailsAttachFileModel
    {
        public int Number { get; set; }
        public string[] Paths { get; set; }
        public string Name { get; set; }
    }



}


