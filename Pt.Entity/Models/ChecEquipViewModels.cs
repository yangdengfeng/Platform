using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pkpm.Entity.Models
{
    public class CheckEquipViewModels
    {
        public List<SysDict> CompayTypes { get; set; }
        public List<SysDict> apparatusType { get; set; }
        public List<SysDict> personnelStatus { get; set; }
        public List<SysDict> YesNo { get; set; }
        public t_bp_Equipment Equip { get; set; }
        public Dictionary<string, string> CheckInsts { get; set; }
        public string IsCheckcerfNumPath { get; set; }
        public string IsRepaircerfNumPath { get; set; }
        public List<t_bp_equItemSubItemList> CheckItems { get; set; }
        public List<SysDict> EquClass { get; set; }
    }
   
    public class CheckEquipSearchModel
    {
        public string CheckInst { get; set; }
        public string CheckUnit { get; set; }
        public string RepairUnit { get; set; }
        public string EquNum { get; set; }
        public string EquType { get; set; }
        public string CustomType { get; set; }
        public string EquName { get; set; }
        public DateTime? CheckStartDt { get; set; }
        public DateTime? CheckEndDt { get; set; }
        public DateTime? RepairStartDt { get; set; }
        public DateTime? RepairEndDt { get; set; }
        public string EquClass { get; set; }
        public string Status { get; set; }
        public string CompanyType { get; set; }
        public int? posStart { get; set; }
        public int? count { get; set; }
    }

    public class CheckEquipSaveViewModel : t_bp_Equipment
    {
        public string ProjectGrid { get; set; }
        public string checkdate { get; set; }
        public string repairedate { get; set; }
    }

    public class CheckEuipApplyChange
    {
        public string SubmitName { get; set; }
        public string SubmitText { get; set; }
        public int SubmitId { get; set; }
        public string SubmitCustomId { get; set; }
    }


    public class CheckEquipDetailsModel
    {
        public t_bp_Equipment Equip { get; set; }
        public List<t_bp_equItemSubItemList> CheckItems { get; set; }
        public string IsCheckcerfNumPath { get; set; }
        public string IsRepaircerfNumPath { get; set; }
    }

    public class CheckEquipEditAttachFileModel
    {
        public string FilePath { get; set; }
        public string[] Paths { get; set; }
        public string Name { get; set; } //存储当前查看项目名称
    }

    public class CheckEquipCompareModel
    {
        [DisplayName("设备ID")]
        public int id { get; set; }
        [DisplayName("检测机构名称")]
        public string customid { get; set; }
        [DisplayName("设备名称")]
        public string EquName { get; set; }
        [DisplayName("checkitem")]
        public string checkitem { get; set; }
        [DisplayName("设备型号")]
        public string equtype { get; set; }
        [DisplayName("设备规格")]
        public string equspec { get; set; }
        [DisplayName("设备编号")]
        public string equnum { get; set; }
        [DisplayName("测量范围")]
        public string testrange { get; set; }
        [DisplayName("准确度等级")]
        public string degree { get; set; }
        [DisplayName("不确定度")]
        public string uncertainty { get; set; }
        [DisplayName("检定/校准机构")]
        public string checkunit { get; set; }
        [DisplayName("设备名称")]
        public string repairunit { get; set; }
        [DisplayName("设备检定/校准证书号")]
        public string checkcerfnum { get; set; }
        [DisplayName("检定/校准有效起始日期")]
        public DateTime? checkstartdate { get; set; }
        [DisplayName("检定/校准有效截止日期")]
        public DateTime? checkenddate { get; set; }
        [DisplayName("repaircerfnum")]
        public string repaircerfnum { get; set; }
        [DisplayName("checkcerfnumpath")]
        public string checkcerfnumpath { get; set; }
        [DisplayName("repaircerfnumpath")]
        public string repaircerfnumpath { get; set; }
        [DisplayName("repairstartdate")]
        public DateTime? repairstartdate { get; set; }
        [DisplayName("repairenddate")]
        public DateTime? repairenddate { get; set; }
        [DisplayName("selfcheckitem")]
        public string selfcheckitem { get; set; }
        [DisplayName("selfrepairitem")]
        public string selfrepairitem { get; set; }
        [DisplayName("selfcheckstandardname")]
        public string selfcheckstandardname { get; set; }
        [DisplayName("selfchecknum")]
        public string selfchecknum { get; set; }
        [DisplayName("selfrepairstandardname")]
        public string selfrepairstandardname { get; set; }
        [DisplayName("selfrepairnum")]
        public string selfrepairnum { get; set; }
        [DisplayName("explain")]
        public string explain { get; set; }
        [DisplayName("isautoacs")]
        public string isautoacs { get; set; }
        [DisplayName("autoacsprovider")]
        public string autoacsprovider { get; set; }
        [DisplayName("approvalstatus")]
        public string approvalstatus { get; set; }
        [DisplayName("approveadvice")]
        public string approveadvice { get; set; }
        [DisplayName("supcustomid")]
        public string supcustomid { get; set; }
        [DisplayName("equclass")]
        public string equclass { get; set; }
        [DisplayName("buytime")]
        public DateTime? buytime { get; set; }
        [DisplayName("timestart")]
        public DateTime? timestart { get; set; }
        [DisplayName("timeend")]
        public DateTime? timeend { get; set; }
        [DisplayName("sysarea")]
        public string sysarea { get; set; }
        [DisplayName("yharea")]
        public string yharea { get; set; }
        [DisplayName("checktime")]
        public DateTime? checktime { get; set; }
        [DisplayName("ispile")]
        public string ispile { get; set; }
        [DisplayName("djtime")]
        public DateTime? djtime { get; set; }
        [DisplayName("仪器资质范围")]
        public string equclassId { get; set; }
    }
}
